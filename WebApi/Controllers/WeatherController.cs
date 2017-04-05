using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

// Namespace for Queue storage types

namespace WebApi.Controllers
{
    public class WeatherController : ApiController
    {
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/weather/set")]
        public JsonResult Set(WeatherModel weather)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container.
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            // Create the queue if it doesn't already exist 
            queue.CreateIfNotExists();

            var w = JsonConvert.SerializeObject(weather);

            // Create a message and add it to the queue.
            CloudQueueMessage message = new CloudQueueMessage(w);//"Hello, World");
            queue.AddMessage(message);

            return new JsonResult
            {
                Data = new
                {
                    status = HttpStatusCode.OK
                }
            };
        }

        struct InfoData: IInfoData
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        class Source
        {
            internal void CheckAndProceed(List<InfoData> data)
            {
                var dest = new Destination();

                //do something

                dest.ProceedData(data.Select(i => i as IInfoData).ToList()); // as List<IInfoData>//new List<object> {data});
            }
        }

        class Destination
        {
            internal void ProceedData(List<IInfoData> data) //List<Object> data) //ValueType> data)
            {
                foreach (var item in data)
                {
                    //do something
                }
            }
        }
    }

    internal interface IInfoData
    {
        string FirstName { get; set; }
        string LastName { get; set; }
    }

    public class WeatherItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class WeatherModel
    {
        public IEnumerable<string> AreasIds { get; set; }
        public IEnumerable<WeatherItem> WeatherItems { get; set; }
    }
}