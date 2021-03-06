﻿using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AspNet.Identity.MySQL.Repository.Concrete;
using AspNet.Identity.MySQL.WebApiModels;
using Mapper.MapperContext;
using NLog;
using PlantingLib.Plants;
using PlantingLib.Plants.ServicesScheduling;
using PlantingLib.Sensors;
using PlantingLib.Timers;

namespace PlantsWpf.DbDataAccessors
{
    internal class MySqlDbDataModifier
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly DbMapper _dbMapper;
        private readonly PlantsAreas _plantsAreas;
        private readonly SensorsCollection _sensorsCollection;
        private readonly MySqlMeasurableParameterMappingRepository _sqlMeasurableParameterMappingRepository;

        private readonly MySqlMeasuringMessageMappingRepository _sqlMeasuringMessageMappingRepository;
        private readonly MySqlPlantMappingRepository _sqlPlantMappingRepository;
        private readonly MySqlPlantsAreaMappingRepository _sqlPlantsAreaMappingRepository;
        private readonly MySqlSensorMappingRepository _sqlSensorMappingRepository;
        private readonly MySqlServiceScheduleMappingRepository _sqlServiceScheduleMappingRepository;

        private readonly string _baseServerUr = "http://qwertyuiop1.azurewebsites.net/";

        public MySqlDbDataModifier(MySqlMeasurableParameterMappingRepository sqlMeasurableParameterMappingRepository,
            MySqlPlantMappingRepository sqlPlantMappingRepository,
            MySqlPlantsAreaMappingRepository sqlPlantsAreaMappingRepository,
            MySqlSensorMappingRepository sqlSensorMappingRepository,
            MySqlServiceScheduleMappingRepository sqlServiceScheduleMappingRepository,
            MySqlMeasuringMessageMappingRepository sqlMeasuringMessageMappingRepository,
            SensorsCollection sensorsCollection, PlantsAreas plantsAreas)
        {
            _sqlMeasurableParameterMappingRepository = sqlMeasurableParameterMappingRepository;
            _sqlPlantMappingRepository = sqlPlantMappingRepository;
            _sqlPlantsAreaMappingRepository = sqlPlantsAreaMappingRepository;
            _sqlServiceScheduleMappingRepository = sqlServiceScheduleMappingRepository;
            _sqlMeasuringMessageMappingRepository = sqlMeasuringMessageMappingRepository;
            _sensorsCollection = sensorsCollection;
            _plantsAreas = plantsAreas;
            _sqlSensorMappingRepository = sqlSensorMappingRepository;
            _dbMapper = new DbMapper(_sqlPlantMappingRepository,
                _sqlMeasurableParameterMappingRepository, _sqlServiceScheduleMappingRepository);
        }

        public MySqlDbDataModifier()
        {
        }

        public MySqlMeasurableParameterMappingRepository SqlMeasurableParameterMappingRepository =>
            _sqlMeasurableParameterMappingRepository ?? new MySqlMeasurableParameterMappingRepository();

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

        public bool SaveSensor(PlantsArea area, Sensor sensor, ServiceSchedule serviceSchedule)
        {
            try
            {
                var measurableParameterMapping =
                    _dbMapper.GetMeasurableParameterMapping(sensor.MeasurableParameter);

                if (!(_sqlMeasurableParameterMappingRepository.Save(measurableParameterMapping,
                          measurableParameterMapping.Id) &&
                      area.Plant.AddMeasurableParameter(sensor.MeasurableParameter)))
                    return false;

                if (!area.AddSensor(sensor))
                    return false;

                var sensorMapping = _dbMapper.GetSensorMapping(sensor);
                if (!(_sqlSensorMappingRepository.Save(sensorMapping, sensorMapping.Id) &&
                      _sensorsCollection.AddSensor(sensor)))
                    return false;

                if (serviceSchedule != null)
                    if (!SaveServiceSchedule(area, serviceSchedule))
                        return false;

                //if custom sensor
                if (sensor.IsCustom)
                {
                    var plantMapping = _dbMapper.GetPlantMapping(area.Plant);
                    if (!_sqlPlantMappingRepository.Save(plantMapping, plantMapping.Id))
                        return false;
                }

                return true;
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return false;
            }
        }

        public bool AddPlantsArea(PlantsArea plantsArea)
        {
            try
            {
                //IPlantMappingRepository plantMappingRepository = new PlantMappingRepository();
                //IPlantsAreaMappingRepository plantsAreaMappingRepository = new PlantsAreaMappingRepository();
                //IMeasurableParameterMappingRepository measurableParameterMappingRepository =
                //    new MeasurableParameterMappingRepository();
                //ISensorMappingRepository sensorMappingRepository = new SensorMappingRepository();

                var temperature = plantsArea.Plant.Temperature;
                var measurableParameterMapping =
                    _dbMapper.GetMeasurableParameterMapping(temperature);
                if (
                    !_sqlMeasurableParameterMappingRepository.Save(measurableParameterMapping,
                        measurableParameterMapping.Id))
                    return false;

                var humidity = plantsArea.Plant.Humidity;
                measurableParameterMapping = _dbMapper.GetMeasurableParameterMapping(humidity);
                if (
                    !_sqlMeasurableParameterMappingRepository.Save(measurableParameterMapping,
                        measurableParameterMapping.Id))
                    return false;

                var soilPh = plantsArea.Plant.SoilPh;
                measurableParameterMapping = _dbMapper.GetMeasurableParameterMapping(soilPh);
                if (
                    !_sqlMeasurableParameterMappingRepository.Save(measurableParameterMapping,
                        measurableParameterMapping.Id))
                    return false;

                var nutrient = plantsArea.Plant.Nutrient;
                measurableParameterMapping = _dbMapper.GetMeasurableParameterMapping(nutrient);
                if (
                    !_sqlMeasurableParameterMappingRepository.Save(measurableParameterMapping,
                        measurableParameterMapping.Id))
                    return false;

                var plant = plantsArea.Plant;
                var plantMapping = _dbMapper.GetPlantMapping(plant);
                if (!_sqlPlantMappingRepository.Save(plantMapping, plantMapping.Id))
                    return false;

                var plantsAreaMapping = _dbMapper.GetPlantsAreaMapping(plantsArea);
                if (!_sqlPlantsAreaMappingRepository.Save(plantsAreaMapping, plantsAreaMapping.Id))
                    return false;

                if ((from sensor in plantsArea.Sensors
                    let sensorMapping = _dbMapper.GetSensorMapping(sensor)
                    where !(_sqlSensorMappingRepository.Save(sensorMapping, sensorMapping.Id) &&
                            _sensorsCollection.AddSensor(sensor))
                    select sensor).Any())
                    return false;

                if (plantsArea.ServicesSchedulesStates.ServicesSchedules
                    .Select(serviceSchedule =>
                            _dbMapper.GetServiceScheduleMapping(serviceSchedule))
                    .Any(serviceScheduleMapping =>
                            !_sqlServiceScheduleMappingRepository.Save(serviceScheduleMapping, serviceScheduleMapping.Id)))
                    return false;

                _plantsAreas.AddPlantsArea(plantsArea);

                return true;
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return false;
            }
        }

        public bool SaveServiceSchedule(PlantsArea area, ServiceSchedule serviceSchedule)
        {
            try
            {
                var serviceScheduleMapping = _dbMapper.GetServiceScheduleMapping(serviceSchedule);
                if (serviceScheduleMapping != null)
                    if (_sqlServiceScheduleMappingRepository.Save(serviceScheduleMapping, serviceScheduleMapping.Id))
                        return true;
                return false;
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return false;
            }
        }

        public bool RemoveSensor(PlantsArea area, Sensor sensor, ServiceSchedule serviceSchedule)
        {
            try
            {
                if (_sqlSensorMappingRepository.Delete(sensor.Id) &&
                    _sensorsCollection.RemoveSensor(sensor) &&
                    area.RemoveSensor(sensor))
                {
                    //if custom sensor
                    var serviceState =
                        area.PlantServicesStates.ServicesStates.FirstOrDefault(
                            s => s.IsFor(sensor.MeasurableType));

                    if (serviceState != null)
                        if (area.PlantServicesStates.RemoveServiceState(serviceState))
                            if (area.Plant.RemoveMeasurableParameter(sensor.MeasurableParameter))
                            {
                                var plantMapping = _dbMapper.GetPlantMapping(area.Plant);
                                if (_sqlPlantMappingRepository.Save(plantMapping, plantMapping.Id))
                                    if (_sqlMeasurableParameterMappingRepository.Delete(sensor.MeasurableParameter.Id))
                                    {
                                        if (serviceSchedule != null)
                                            return _sqlServiceScheduleMappingRepository.Delete(serviceSchedule.Id);

                                        serviceSchedule = area.ServicesSchedulesStates.ServicesSchedules.SingleOrDefault
                                        (
                                            s => s.ServiceName == serviceState.ServiceName);

                                        if (serviceSchedule != null)
                                            return _sqlServiceScheduleMappingRepository.Delete(serviceSchedule.Id);
                                    }
                            }
                }
                return false;
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return false;
            }
        }

        public bool RemovePlantsArea(PlantsArea plantsArea)
        {
            try
            {
                if (_sqlPlantsAreaMappingRepository.Delete(plantsArea.Id) &&
                    _sqlPlantMappingRepository.Delete(plantsArea.Plant.Id))
                {
                    if (plantsArea.ServicesSchedulesStates.ServicesSchedules
                        .Any(servicesSchedule => !_sqlServiceScheduleMappingRepository
                            .Delete(servicesSchedule.Id)))
                        return false;
                    plantsArea.ServicesSchedulesStates.ServicesSchedules.Clear();

                    if (plantsArea.Sensors.Any(sensor =>

                        //del. by cascade 
                        //!(sqlSensorMappingRepository
                        //    .Delete(sensor.Id)
                            !_sqlMeasurableParameterMappingRepository
                                .Delete(sensor.MeasurableParameter.Id)))
                        return false;

                    plantsArea.Sensors.ToList().ForEach(s => _sensorsCollection.RemoveSensor(s));
                    plantsArea.Sensors.Clear();
                    return _plantsAreas.RemovePlantsArea(plantsArea);
                }
                return false;
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return false;
            }
        }

        public async Task<HttpResponseMessage> RegisterUser(RegisterViewModel registerViewModel)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseServerUr);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.PostAsJsonAsync("api/identity/register", registerViewModel);
                //.Result;
                return await
                    response;
            }
        }

        public async Task<HttpResponseMessage> LoginUser(LoginViewModel loginViewModel)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseServerUr);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.PostAsJsonAsync("api/identity/login", loginViewModel);
                //Result;
                return await response;
            }
        }
    }
}