﻿using System;
using System.Linq;
using Database.DatabaseStructure.Repository.Abstract;
using Database.DatabaseStructure.Repository.Concrete;
using Mapper.MapperContext;
using NLog;
using PlantingLib.Plants;
using PlantingLib.Plants.ServicesScheduling;
using PlantingLib.Sensors;
using PlantingLib.Timers;

namespace PlantsWpf.DbDataAccessors
{
    public class DbDataModifier
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly DbMapper _dbMapper;
        private readonly IMeasurableParameterMappingRepository _measurableParameterMappingRepository;
        private readonly IPlantMappingRepository _plantMappingRepository;
        private readonly IPlantsAreaMappingRepository _plantsAreaMappingRepository;

        private readonly PlantsAreas _plantsAreas;
        private readonly ISensorMappingRepository _sensorMappingRepository;
        private readonly SensorsCollection _sensorsCollection;
        private readonly IServiceScheduleMappingRepository _serviceScheduleMappingRepository;

        public DbDataModifier(PlantsAreas plantsAreas, SensorsCollection sensorsCollection,
            IMeasurableParameterMappingRepository measurableParameterMappingRepository,
            IPlantMappingRepository plantMappingRepository, ISensorMappingRepository sensorMappingRepository,
            IPlantsAreaMappingRepository plantsAreaMappingRepository,
            IServiceScheduleMappingRepository serviceScheduleMappingRepository)
        {
            _plantsAreas = plantsAreas;
            _sensorsCollection = sensorsCollection;
            _measurableParameterMappingRepository = measurableParameterMappingRepository;
            _plantMappingRepository = plantMappingRepository;
            _sensorMappingRepository = sensorMappingRepository;
            _plantsAreaMappingRepository = plantsAreaMappingRepository;
            _serviceScheduleMappingRepository = serviceScheduleMappingRepository;
            _dbMapper = new DbMapper(_plantMappingRepository,
                _measurableParameterMappingRepository, _serviceScheduleMappingRepository);
        }

        public bool SaveSensor(PlantsArea area, Sensor sensor, ServiceSchedule serviceSchedule)
        {
            try
            {
                //throw new Exception();
                var measurableParameterMapping =
                    _dbMapper.GetMeasurableParameterMapping(sensor.MeasurableParameter);

                if (!(_measurableParameterMappingRepository.Save(measurableParameterMapping,
                          measurableParameterMapping.Id) &&
                      area.Plant.AddMeasurableParameter(sensor.MeasurableParameter)))
                    return false;

                if (!area.AddSensor(sensor))
                    return false;

                var sensorMapping = _dbMapper.GetSensorMapping(sensor);
                if (!(_sensorMappingRepository.Save(sensorMapping, sensorMapping.Id) &&
                      _sensorsCollection.AddSensor(sensor)))
                    return false;

                if (serviceSchedule != null)
                    if (!SaveServiceSchedule(area, serviceSchedule))
                        return false;

                //if custom sensor
                if (sensor.IsCustom)
                {
                    var plantMapping = _dbMapper.GetPlantMapping(area.Plant);
                    if (!_plantMappingRepository.Save(plantMapping, plantMapping.Id))
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
                //TODO: use private
                IPlantMappingRepository plantMappingRepository = new PlantMappingRepository();
                IPlantsAreaMappingRepository plantsAreaMappingRepository = new PlantsAreaMappingRepository();
                IMeasurableParameterMappingRepository measurableParameterMappingRepository =
                    new MeasurableParameterMappingRepository();
                ISensorMappingRepository sensorMappingRepository = new SensorMappingRepository();

                var temperature = plantsArea.Plant.Temperature;
                var measurableParameterMapping = _dbMapper.GetMeasurableParameterMapping(temperature);
                if (
                    !measurableParameterMappingRepository.Save(measurableParameterMapping, measurableParameterMapping.Id))
                    return false;

                var humidity = plantsArea.Plant.Humidity;
                measurableParameterMapping = _dbMapper.GetMeasurableParameterMapping(humidity);
                if (
                    !measurableParameterMappingRepository.Save(measurableParameterMapping, measurableParameterMapping.Id))
                    return false;

                var soilPh = plantsArea.Plant.SoilPh;
                measurableParameterMapping = _dbMapper.GetMeasurableParameterMapping(soilPh);
                if (
                    !measurableParameterMappingRepository.Save(measurableParameterMapping, measurableParameterMapping.Id))
                    return false;

                var nutrient = plantsArea.Plant.Nutrient;
                measurableParameterMapping = _dbMapper.GetMeasurableParameterMapping(nutrient);
                if (
                    !measurableParameterMappingRepository.Save(measurableParameterMapping, measurableParameterMapping.Id))
                    return false;

                var plant = plantsArea.Plant;
                var plantMapping = _dbMapper.GetPlantMapping(plant);
                if (!plantMappingRepository.Save(plantMapping, plantMapping.Id))
                    return false;

                var plantsAreaMapping = _dbMapper.GetPlantsAreaMapping(plantsArea);
                if (!plantsAreaMappingRepository.Save(plantsAreaMapping, plantsAreaMapping.Id))
                    return false;

                if ((from sensor in plantsArea.Sensors
                    let sensorMapping = _dbMapper.GetSensorMapping(sensor)
                    where !(sensorMappingRepository.Save(sensorMapping, sensorMapping.Id) &&
                            _sensorsCollection.AddSensor(sensor))
                    select sensor).Any())
                    return false;

                if (plantsArea.ServicesSchedulesStates.ServicesSchedules
                    .Select(serviceSchedule =>
                            _dbMapper.GetServiceScheduleMapping(serviceSchedule))
                    .Any(serviceScheduleMapping =>
                            !_serviceScheduleMappingRepository.Save(serviceScheduleMapping, serviceScheduleMapping.Id)))
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
                    if (_serviceScheduleMappingRepository.Save(serviceScheduleMapping, serviceScheduleMapping.Id))
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
                if (_sensorMappingRepository.Delete(sensor.Id) &&
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
                                if (_plantMappingRepository.Save(plantMapping, plantMapping.Id))
                                    if (_measurableParameterMappingRepository.Delete(sensor.MeasurableParameter.Id))
                                    {
                                        if (serviceSchedule != null)
                                            return _serviceScheduleMappingRepository.Delete(serviceSchedule.Id);

                                        serviceSchedule = area.ServicesSchedulesStates.ServicesSchedules.SingleOrDefault
                                        (
                                            s => s.ServiceName == serviceState.ServiceName);

                                        if (serviceSchedule != null)
                                            return _serviceScheduleMappingRepository.Delete(serviceSchedule.Id);
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
                if (_plantsAreaMappingRepository.Delete(plantsArea.Id) &&
                    _plantMappingRepository.Delete(plantsArea.Plant.Id))
                {
                    if (plantsArea.ServicesSchedulesStates.ServicesSchedules
                        .Any(servicesSchedule => !_serviceScheduleMappingRepository
                            .Delete(servicesSchedule.Id)))
                        return false;
                    plantsArea.ServicesSchedulesStates.ServicesSchedules.Clear();

                    if (plantsArea.Sensors.Any(sensor =>

                        //del. by cascade 
                        //!(_sensorMappingRepository
                        //    .Delete(sensor.Id)
                            !_measurableParameterMappingRepository
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
    }
}