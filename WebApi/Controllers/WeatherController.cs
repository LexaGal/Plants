using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace WebApi.Controllers
{
    public class WeatherController : ApiController
    {
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/weather/set")]
        public JsonResult Set(WeatherModel weather)
        {
            return new JsonResult
            {
                Data = new
                {
                    status = HttpStatusCode.OK                    
                }
            };
        }

        // GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
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