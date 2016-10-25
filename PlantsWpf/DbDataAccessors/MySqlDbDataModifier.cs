using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AspNet.Identity.MySQL.Repository.Concrete;
using AspNet.Identity.MySQL.WebApiModels;
using Database.MappingTypes;

namespace PlantsWpf.DbDataAccessors
{
    class MySqlDbDataModifier
    {
        private readonly MySqlMeasurableParameterMappingRepository _mySqlMeasurableParameterMappingRepository;
        
        private string _baseServerUr = "http://localhost:63958/";

        public MySqlDbDataModifier(MySqlMeasurableParameterMappingRepository mySqlMeasurableParameterMappingRepository)
        {
            _mySqlMeasurableParameterMappingRepository = mySqlMeasurableParameterMappingRepository;
        }

        public List<MeasurableParameterMapping> GetMeasurableParameterMappings()
        {
            return _mySqlMeasurableParameterMappingRepository.GetAll();
        }

        public HttpResponseMessage RegisterUser(RegisterViewModel registerViewModel)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseServerUr);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PostAsJsonAsync("api/identity/register", registerViewModel).Result;
                return response;
            }
        }

        public HttpResponseMessage LoginUser(LoginViewModel loginViewModel)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseServerUr);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PostAsJsonAsync("api/identity/login", loginViewModel).Result;
                return response;
            }
        }
    }
}
