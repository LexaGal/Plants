using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AzureQueuing
{
    public class MessageQueueWorker : QueueWorker
    {
        public MessageQueueWorker(string accountConnectionStringName,
            string queueName,
            string poisonQueueName,
            int maxAttempts = 10,
            int visibilityTimeOutInMinutes = 10)
            : base(accountConnectionStringName,
                queueName,
                poisonQueueName,
                maxAttempts,
                visibilityTimeOutInMinutes)
        {
        }

        protected override void Report(string message)
        {
            Console.WriteLine(message);
        }

        //protected override ICollection<CloudQueueMessage> TryGetWork()
        //{
        //    throw new NotImplementedException();
        //}

        protected override void OnExecuting(CloudQueueMessage workItem)
        {
            //Do some work 
            var message = workItem.AsString;
            Trace.WriteLine(message);

            //Used for testing the poison queue
            if (message == "fail")
                throw new Exception(message);

            Thread.Sleep(TimeSpan.FromSeconds(10));
        }
    }
}