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

        public MySqlPlantsAreaMappingRepository SqlPlantsAreaMappingRepository =>
            _sqlPlantsAreaMappingRepository ?? new MySqlPlantsAreaMappingRepository();

        public MySqlSensorMappingRepository SqlSensorMappingRepository =>
            _sqlSensorMappingRepository ?? new MySqlSensorMappingRepository();

        public MySqlServiceScheduleMappingRepository SqlServiceScheduleMappingRepository =>
            _sqlServiceScheduleMappingRepository ?? new MySqlServiceScheduleMappingRepository();

        public MySqlMeasuringMessageMappingRepository SqlMeasuringMessageMappingRepository =>
            _sqlMeasuringMessageMappingRepository ?? new MySqlMeasuringMessageMappingRepository();

        private readonly MySqlMeasuringMessageMappingRepository _sqlMeasuringMessageMappingRepository;
        private readonly MySqlServiceScheduleMappingRepository _sqlServiceScheduleMappingRepository;
        private readonly MySqlSensorMappingRepository _sqlSensorMappingRepository;
        private readonly MySqlMeasurableParameterMappingRepository _mySqlMeasurableParameterMappingRepository;
        private readonly MySqlPlantMappingRepository _sqlPlantMappingRepository;
        private readonly MySqlPlantsAreaMappingRepository _sqlPlantsAreaMappingRepository;

        private string _baseServerUr = "http://localhost:63958/";

        public MySqlDbDataModifier(MySqlMeasurableParameterMappingRepository mySqlMeasurableParameterMappingRepository,
            MySqlPlantMappingRepository sqlPlantMappingRepository,
            MySqlPlantsAreaMappingRepository sqlPlantsAreaMappingRepository,
            MySqlSensorMappingRepository sqlSensorMappingRepository,
            MySqlServiceScheduleMappingRepository sqlServiceScheduleMappingRepository,
            MySqlMeasuringMessageMappingRepository sqlMeasuringMessageMappingRepository)
        {
            _mySqlMeasurableParameterMappingRepository = mySqlMeasurableParameterMappingRepository;
            _sqlPlantMappingRepository = sqlPlantMappingRepository;
            _sqlPlantsAreaMappingRepository = sqlPlantsAreaMappingRepository;
            _sqlServiceScheduleMappingRepository = sqlServiceScheduleMappingRepository;
            _sqlMeasuringMessageMappingRepository = sqlMeasuringMessageMappingRepository;
            _sqlSensorMappingRepository = sqlSensorMappingRepository;
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
