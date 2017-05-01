using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using WeatherUtil;

// Namespace for Queue storage types

namespace WebApi.Controllers
{
    public class WeatherController : ApiController
    {
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/weather/set")]
        public JsonResult Set(WeatherModel weather)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            queue.CreateIfNotExists();

            var w = JsonConvert.SerializeObject(weather);

            CloudQueueMessage message = new CloudQueueMessage(w);
            queue.AddMessage(message);
            
            return new JsonResult
            {
                Data = new
                {
                    status = HttpStatusCode.OK
                }
            };
        }
    }

                //, new List<WeatherItem>());

    //            //do something

    //            dest.ProceedData(data.Select(i => i as IInfoData).ToList()); // as List<IInfoData>//new List<object> {data});
    //        }
    //    }

    //    class Destination
    //    {
    //        internal void ProceedData(List<IInfoData> data) //List<Object> data) //ValueType> data)
    //        {
    //            foreach (var item in data)
    //            {
    //                //do something
    //            }
    //        }
    //    }
    //}

    //internal interface IInfoData
    //{
    //    string FirstName { get; set; }
    //    string LastName { get; set; }
    //}   
}