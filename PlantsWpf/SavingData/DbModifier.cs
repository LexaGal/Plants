using System;
using System.Linq;
using System.Windows.Forms;
using Database.DatabaseStructure.Repository.Abstract;
using Database.DatabaseStructure.Repository.Concrete;
using Database.MappingTypes;
using Mapper.MapperContext;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;
using PlantingLib.Plants.ServicesScheduling;
using PlantingLib.Plants.ServiceStates;
using PlantingLib.Sensors;

namespace PlantsWpf.SavingData
{
    public class DbModifier
    {
        private readonly PlantsAreas _plantsAreas;
        private readonly SensorsCollection _sensorsCollection;
        private readonly DbMapper _dbMapper;
        private readonly IMeasurableParameterMappingRepository _measurableParameterMappingRepository;
        private readonly IPlantMappingRepository _plantMappingRepository;
        private readonly ISensorMappingRepository _sensorMappingRepository;
        private readonly IPlantsAreaMappingRepository _plantsAreaMappingRepository;
        private readonly IServiceScheduleMappingRepository _serviceScheduleMappingRepository;

        public DbModifier(PlantsAreas plantsAreas, SensorsCollection sensorsCollection,
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
            _dbMapper = new DbMapper(_plantMappingRepository, _plantsAreaMappingRepository,
                _measurableParameterMappingRepository, _serviceScheduleMappingRepository);
        }

        public void SaveSensor(PlantsArea area, Sensor sensor, ServiceSchedule serviceSchedule)
        {
            //if custom sensor
            if (sensor.IsCustom)
            {
                MeasurableParameterMapping measurableParameterMapping =
                    _dbMapper.GetMeasurableParameterMapping(sensor.MeasurableParameter);

                _measurableParameterMappingRepository.Save(measurableParameterMapping, measurableParameterMapping.Id);

                area.Plant.AddCustomParameter(sensor.MeasurableParameter as CustomParameter);

                PlantMapping plantMapping = _dbMapper.GetPlantMapping(area.Plant);
                _plantMappingRepository.Save(plantMapping, plantMapping.Id);

                //ServiceState serviceState = new ServiceState(sensor.MeasurableType, true);
                //area.PlantsAreaServicesStates.AddServiceState(serviceState);

                //if (ParameterServicesInfo.GetParameterInfo(sensor.MeasurableType) == null)
                //{
                //    ParameterServicesInfo.ParametersServices.Add(new ParameterServices(sensor.MeasurableType,
                //        new List<ServiceState> {serviceState}));
                //}

                //if (area.ServicesSchedulesStates.ServicesSchedules.All(
                //        schedule => schedule.ServiceState != serviceState.ServiceName))
                //{
                //    ServiceSchedule serviceSchedule = new ServiceSchedule(Guid.NewGuid(), area.Id,
                //        serviceState.ServiceName,
                //        new TimeSpan(0, 0, 10), new TimeSpan(0, 1, 0),
                //        new List<MeasurableParameter> {sensor.MeasurableParameter});

                if (serviceSchedule != null)
                {
                    area.ServicesSchedulesStates.AddServiceSchedule(serviceSchedule);
                    SaveServiceSchedule(area, serviceSchedule);
                }
            }

            area.AddSensor(sensor);
            SensorMapping sensorMapping = _dbMapper.GetSensorMapping(sensor);
            _sensorMappingRepository.Save(sensorMapping, sensorMapping.Id);
            _sensorsCollection.AddSensor(sensor);
        }

        public void SavePlantsArea(PlantsArea plantsArea)
        {
            IPlantMappingRepository plantMappingRepository = new PlantMappingRepository();
            IPlantsAreaMappingRepository plantsAreaMappingRepository = new PlantsAreaMappingRepository();
            IMeasurableParameterMappingRepository measurableParameterMappingRepository =
                new MeasurableParameterMappingRepository();
            ISensorMappingRepository sensorMappingRepository = new SensorMappingRepository();

            Temperature temperature = plantsArea.Plant.Temperature;
            MeasurableParameterMapping measurableParameterMapping = _dbMapper.GetMeasurableParameterMapping(temperature);
            measurableParameterMappingRepository.Save(measurableParameterMapping, measurableParameterMapping.Id);

            Humidity humidity = plantsArea.Plant.Humidity;
            measurableParameterMapping = _dbMapper.GetMeasurableParameterMapping(humidity);
            measurableParameterMappingRepository.Save(measurableParameterMapping, measurableParameterMapping.Id);

            SoilPh soilPh = plantsArea.Plant.SoilPh;
            measurableParameterMapping = _dbMapper.GetMeasurableParameterMapping(soilPh);
            measurableParameterMappingRepository.Save(measurableParameterMapping, measurableParameterMapping.Id);

            Nutrient nutrient = plantsArea.Plant.Nutrient;
            measurableParameterMapping = _dbMapper.GetMeasurableParameterMapping(nutrient);
            measurableParameterMappingRepository.Save(measurableParameterMapping, measurableParameterMapping.Id);

            Plant plant = plantsArea.Plant;
            PlantMapping plantMapping = _dbMapper.GetPlantMapping(plant);
            plantMappingRepository.Save(plantMapping,plantMapping.Id);

            PlantsAreaMapping plantsAreaMapping = _dbMapper.GetPlantsAreaMapping(plantsArea);
            plantsAreaMappingRepository.Save(plantsAreaMapping,plantsAreaMapping.Id);

            foreach (Sensor sensor in plantsArea.Sensors)
            {
                SensorMapping sensorMapping = _dbMapper.GetSensorMapping(sensor);
                sensorMappingRepository.Save(sensorMapping, sensorMapping.Id);
                _sensorsCollection.AddSensor(sensor);
            }
            foreach (ServiceSchedule serviceSchedule in plantsArea.ServicesSchedulesStates.ServicesSchedules)
            {
                ServiceScheduleMapping serviceScheduleMapping = _dbMapper.GetServiceScheduleMapping(serviceSchedule);
                _serviceScheduleMappingRepository.Save(serviceScheduleMapping, serviceScheduleMapping.Id);
            }

            _plantsAreas.AddPlantsArea(plantsArea);
        }

        public void SaveServiceSchedule(PlantsArea area, ServiceSchedule serviceSchedule)
        {
            ServiceScheduleMapping serviceScheduleMapping = _dbMapper.GetServiceScheduleMapping(serviceSchedule);
            _serviceScheduleMappingRepository.Save(serviceScheduleMapping, serviceScheduleMapping.Id);
        }

        public bool RemoveSensor(PlantsArea area, Sensor sensor)
        {
            try
            {
                _sensorMappingRepository.Delete(sensor.Id);
                _sensorsCollection.RemoveSensor(sensor);
                area.RemoveSensor(sensor);

                //if custom sensor
                ServiceState serviceState =
                    area.PlantsAreaServicesStates.ServicesStates.FirstOrDefault(
                        s => s.IsFor(sensor.MeasurableType));

                if (serviceState != null)
                {
                    area.PlantsAreaServicesStates.RemoveServiceState(serviceState);

                    area.Plant.RemoveCustomParameter(sensor.MeasurableParameter as CustomParameter);

                    PlantMapping plantMapping = _dbMapper.GetPlantMapping(area.Plant);
                    _plantMappingRepository.Edit(plantMapping);

                    _measurableParameterMappingRepository.Delete(sensor.MeasurableParameter.Id);

                    ServiceSchedule servicesSchedule =
                        area.ServicesSchedulesStates.ServicesSchedules.SingleOrDefault(
                            s => s.ServiceState == serviceState.ServiceName);
                    if (servicesSchedule != null)
                    {
                        return _serviceScheduleMappingRepository.Delete(servicesSchedule.Id);
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
                return false;
            }
        }

        public bool RemovePlantsArea(PlantsArea plantsArea)
        {
            try
            {
                _plantsAreaMappingRepository.Delete(plantsArea.Id);
                _plantMappingRepository.Delete(plantsArea.Plant.Id);

                foreach (ServiceSchedule servicesSchedule in plantsArea.ServicesSchedulesStates.ServicesSchedules)
                {
                    _serviceScheduleMappingRepository.Delete(servicesSchedule.Id);
                }
                plantsArea.ServicesSchedulesStates.ServicesSchedules.Clear();

                foreach (Sensor sensor in plantsArea.Sensors)
                {
                    _sensorMappingRepository.Delete(sensor.Id);
                    _measurableParameterMappingRepository.Delete(sensor.MeasurableParameter.Id);
                }

                plantsArea.Sensors.ToList().ForEach(s => _sensorsCollection.RemoveSensor(s));
                plantsArea.Sensors.Clear();
                return _plantsAreas.RemovePlantsArea(plantsArea);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
                return false;
            }
        }
    }
}
