using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using WeatherUtil;
// Namespace for CloudConfigurationManager
// Namespace for CloudStorageAccount

// Namespace for Queue storage types

namespace WebApi.Controllers
{
    public class WeatherController : ApiController
    {
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/weather/set")]
        public JsonResult Set(WeatherModel weather)
        {
            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            var queueClient = storageAccount.CreateCloudQueueClient();

            var queue = queueClient.GetQueueReference("myqueue");

            queue.CreateIfNotExists();

            var w = JsonConvert.SerializeObject(weather);

            var message = new CloudQueueMessage(w);
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

    //    {
    // var        internal void ProceedData(List<IInfoData> data) //List<Object> data) //ValueType> data)


                            //area = _plantsAreas.Areas.Single(a => a.Id == g);
    //        {
    // var            foreach (var item in data)
    // var            {
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