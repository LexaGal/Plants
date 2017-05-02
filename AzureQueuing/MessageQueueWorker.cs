using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using ObservationUtil;
using WeatherUtil;

namespace AzureQueuing
{
    public class MessageQueueWorker : QueueWorker, ISender<WeatherModel>
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

        public event EventHandler MessageSending;

        public void OnMessageSending(WeatherModel message)
        {
            var handler = MessageSending;
            handler?.Invoke(this, new MessengingEventArgs<WeatherModel>(message));
        }

        protected override void Report(string message)
        {
            Console.WriteLine(message);
            ResetAttempts();
            Thread.Sleep(TimeSpan.FromSeconds(10));
        }

        //protected override ICollection<CloudQueueMessage> TryGetWork()
        //{
        //}

        protected override void OnExecuting(CloudQueueMessage workItem)
        {
            //Do some work 
            var message = workItem.AsString;
            Trace.WriteLine(message);

            //Used for testing the poison queue
            if (message == "fail")
            {
                throw new Exception(message);
            }

            OnMessageSending(JsonConvert.DeserializeObject<WeatherModel>(message));

            Thread.Sleep(TimeSpan.FromSeconds(10));
        }
    }
}