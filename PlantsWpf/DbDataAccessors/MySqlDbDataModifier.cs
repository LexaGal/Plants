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
        public MySqlMeasurableParameterMappingRepository SqlMeasurableParameterMappingRepository =>
            _mySqlMeasurableParameterMappingRepository ?? new MySqlMeasurableParameterMappingRepository();
        
        public MySqlPlantMappingRepository SqlPlantMappingRepository =>
                    _sqlPlantMappingRepository ?? new MySqlPlantMappingRepository();

        private readonly MySqlMeasurableParameterMappingRepository _mySqlMeasurableParameterMappingRepository;
        private readonly MySqlPlantMappingRepository _sqlPlantMappingRepository;

        private string _baseServerUr = "http://localhost:63958/";

        public MySqlDbDataModifier(MySqlMeasurableParameterMappingRepository mySqlMeasurableParameterMappingRepository, MySqlPlantMappingRepository sqlPlantMappingRepository)
        {
            _mySqlMeasurableParameterMappingRepository = mySqlMeasurableParameterMappingRepository;
            _sqlPlantMappingRepository = sqlPlantMappingRepository;
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
