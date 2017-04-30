using System;
using System.Collections.Generic;
using System.Linq;
using Brisebois.WindowsAzure;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AzureQueuing
{
    public abstract class QueueWorker : PollingTask<CloudQueueMessage>
    {
        private readonly CloudQueueClient _client;
        private readonly CloudQueue _poisonQueue;
        private readonly CloudQueue _queue;
        private readonly TimeSpan _visibilityTimeout;
        private readonly int _maxAttempts;

        protected QueueWorker(string connectionString,
            string queueName,
            string poisonQueueName,
            int maxAttempts = 10,
            int visibilityTimeoutInMinutes = 10)
        {
            _maxAttempts = maxAttempts;

            var cs = CloudConfigurationManager.GetSetting(connectionString);
            var account = CloudStorageAccount.Parse(cs);

            _client = account.CreateCloudQueueClient();

            //_client.RetryPolicy = new ExponentialRetry(new TimeSpan(0, 0, 0, 2), 10);

            _queue = _client.GetQueueReference(queueName);
            _queue.CreateIfNotExists();

            _poisonQueue = _client.GetQueueReference(poisonQueueName);
            _poisonQueue.CreateIfNotExists();

            _visibilityTimeout = TimeSpan.FromMinutes(visibilityTimeoutInMinutes);
        }

        protected abstract void OnExecuting(CloudQueueMessage workItem);

        private void PlaceMessageOnPoisonQueue(CloudQueueMessage workItem)
        {
            var message = new CloudQueueMessage(workItem.AsString);
            _poisonQueue.AddMessage(message);
            Completed(workItem);
        }

        protected override void Execute(CloudQueueMessage workItem)
        {
            if (workItem.DequeueCount > _maxAttempts)
            {
                PlaceMessageOnPoisonQueue(workItem);
                return;
            }

            OnExecuting(workItem);
        }
        
        protected override void Completed(CloudQueueMessage workItem)
        {
            try
            {
                _queue.DeleteMessage(workItem);
            }
            catch (Exception ex)
            {
                Report(ex.ToString());
            }
        }

        protected override  ICollection<CloudQueueMessage> TryGetWork()
        {
            return _queue.GetMessages(32, _visibilityTimeout)
                .ToList();
        }
    }
}

