using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Database.DatabaseStructure.Repository.Abstract;
using Database.DatabaseStructure.Repository.Concrete;
using Database.MappingTypes;
using PlantingLib.MeasurableParameters;
using PlantingLib.Messenging;
using PlantingLib.Plants;
using PlantingLib.Plants.ServicesScheduling;
using PlantingLib.Plants.ServiceStates;
using PlantingLib.Sensors;

namespace Mapper.MapperContext
{
    public class DbMapper
    {
        private readonly IPlantMappingRepository _plantRepository;
        private readonly IPlantsAreaMappingRepository _plantsAreaRepository;
        private readonly IMeasurableParameterMappingRepository _measurableParameterRepository;
        private readonly IServiceScheduleMappingRepository _serviceScheduleMappingRepository;

        public DbMapper(IPlantMappingRepository plantRepository, IPlantsAreaMappingRepository plantsAreaRepository,
            IMeasurableParameterMappingRepository measurableParameterRepository, IServiceScheduleMappingRepository serviceScheduleMappingRepository)
        {
            _plantRepository = plantRepository;
            _plantsAreaRepository = plantsAreaRepository;
            _measurableParameterRepository = measurableParameterRepository;
            _serviceScheduleMappingRepository = serviceScheduleMappingRepository;
        }

        public PlantMapping GetPlantMapping(Plant plant)
        {
            //if custom sensor
            StringBuilder builder = new StringBuilder();

            if (plant.CustomParameters.Count != 0)
            {
                plant.CustomParameters.ToList().ForEach(c => builder.Append(c.Id.ToString() + ','));

                builder.Remove(builder.Length - 1, 1);
            }

            return new PlantMapping(plant.Id, plant.Temperature.Id, plant.Humidity.Id,
                plant.SoilPh.Id, plant.Nutrient.Id, plant.Name.ToString(),
                builder.ToString());
        }

        public PlantsAreaMapping GetPlantsAreaMapping(PlantsArea plantsArea)
        {
            return new PlantsAreaMapping(plantsArea.Id, plantsArea.Plant.Id, plantsArea.Number);
        }

        public MeasuringMessageMapping GetMeasuringMessageMapping(MeasuringMessage measuringMessage)
        {
            return new MeasuringMessageMapping(measuringMessage.Id, measuringMessage.DateTime,
                measuringMessage.MeasurableType, measuringMessage.MeasurableType,
                measuringMessage.PlantsAreaId, measuringMessage.ParameterValue);
        }

        public MeasurableParameterMapping GetMeasurableParameterMapping(MeasurableParameter measurableParameter)
        {
            return new MeasurableParameterMapping(measurableParameter.Id, measurableParameter.Optimal,
                measurableParameter.Min, measurableParameter.Max,
                measurableParameter.MeasurableType);
        }

        public SensorMapping GetSensorMapping(Sensor sensor)
        {
            return new SensorMapping(sensor.Id, sensor.PlantsArea.Id,
                (int) sensor.MeasuringTimeout.TotalSeconds, sensor.MeasurableParameter.Id,
                sensor.MeasurableParameter.MeasurableType, sensor.NumberOfTimes);
        }

        public ServiceScheduleMapping GetServiceScheduleMapping(ServiceSchedule serviceSchedule)
        {
            StringBuilder builder = new StringBuilder();

            if (serviceSchedule.MeasurableParameters.Count != 0)
            {
                serviceSchedule.MeasurableParameters.ToList().ForEach(c => builder.Append(c.Id.ToString() + ','));

                builder.Remove(builder.Length - 1, 1);
            }
            return new ServiceScheduleMapping(serviceSchedule.Id, serviceSchedule.PlantsAreaId,
                serviceSchedule.ServiceState.ToString(),
                (int) serviceSchedule.ServicingSpan.TotalSeconds,
                (int) serviceSchedule.ServicingPauseSpan.TotalSeconds, builder.ToString());
        }

        public MeasurableParameter RestoreMeasurableParameter(MeasurableParameterMapping measurableParameterMapping)
        {
            try
            {
                ParameterEnum parameter;
                bool parsed = Enum.TryParse(measurableParameterMapping.Type, out parameter);

                if (parsed)
                {
                    switch (parameter)
                    {
                        case ParameterEnum.Nutrient:
                            return new Nutrient(measurableParameterMapping.Id, measurableParameterMapping.Optimal,
                                measurableParameterMapping.Min, measurableParameterMapping.Max);
                        case ParameterEnum.SoilPh:
                            return new SoilPh(measurableParameterMapping.Id, measurableParameterMapping.Optimal,
                                measurableParameterMapping.Min, measurableParameterMapping.Max);
                        case ParameterEnum.Humidity:
                            return new Humidity(measurableParameterMapping.Id, measurableParameterMapping.Optimal,
                                measurableParameterMapping.Min, measurableParameterMapping.Max);
                        case ParameterEnum.Temperature:
                            return new Temperature(measurableParameterMapping.Id, measurableParameterMapping.Optimal,
                                measurableParameterMapping.Min, measurableParameterMapping.Max);
                    }
                }
                //if custom sensor
                return new CustomParameter(measurableParameterMapping.Id, measurableParameterMapping.Optimal,
                    measurableParameterMapping.Min, measurableParameterMapping.Max, measurableParameterMapping.Type);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
                return null;
            }
        }

        public ServiceSchedule RestoreServiceSchedule(ServiceScheduleMapping serviceScheduleMapping)
        {
            ServiceStateEnum serviceState =
                (ServiceStateEnum) Enum.Parse(typeof (ServiceStateEnum), serviceScheduleMapping.ServiceState);

            List<MeasurableParameter> measurableParameters = null;
            if (!string.IsNullOrEmpty(serviceScheduleMapping.MeasurableParametersIds))
            {
                string[] ids = serviceScheduleMapping.MeasurableParametersIds.Split(',');
                List<MeasurableParameterMapping> measurableParameterMappings =
                    ids.Select(id => _measurableParameterRepository.Get(Guid.Parse(id))).ToList();

                measurableParameters = measurableParameterMappings.Select(m => RestoreMeasurableParameter(m))
                    .ToList();
            }
            return new ServiceSchedule(serviceScheduleMapping.Id, serviceScheduleMapping.PlantsAreaId, serviceState,
                new TimeSpan(0, 0, serviceScheduleMapping.ServicingSpan),
                new TimeSpan(0, 0, serviceScheduleMapping.ServicingPauseSpan),
                measurableParameters);
        }

        public Plant RestorePlant(PlantMapping plantMapping)
        {
            try
            {
                MeasurableParameterMapping temperatureMapping =
                    _measurableParameterRepository.Get(plantMapping.TemperatureId);
                MeasurableParameterMapping soilPhMapping =
                    _measurableParameterRepository.Get(plantMapping.SoilPhId);
                MeasurableParameterMapping humidityMapping =
                    _measurableParameterRepository.Get(plantMapping.HumidityId);
                MeasurableParameterMapping nutrientMapping =
                    _measurableParameterRepository.Get(plantMapping.NutrientId);

                Temperature temperature = RestoreMeasurableParameter(temperatureMapping) as Temperature;
                Humidity humidity = RestoreMeasurableParameter(humidityMapping) as Humidity;
                SoilPh soilPh = RestoreMeasurableParameter(soilPhMapping) as SoilPh;
                Nutrient nutrient = RestoreMeasurableParameter(nutrientMapping) as Nutrient;

                PlantNameEnum name = (PlantNameEnum) Enum.Parse(typeof (PlantNameEnum), plantMapping.Name);

                Plant plant = new Plant(plantMapping.Id, temperature, humidity, soilPh, nutrient, name);

                //if custom sensor
                if (!string.IsNullOrEmpty(plantMapping.CustomParametersIds))
                {
                    string[] ids = plantMapping.CustomParametersIds.Split(',');
                    List<MeasurableParameterMapping> measurableParameterMappings =
                        ids.Select(id => _measurableParameterRepository.Get(Guid.Parse(id))).ToList();

                    List<CustomParameter> customParameters =
                        measurableParameterMappings.Select(m => RestoreMeasurableParameter(m) as CustomParameter)
                            .ToList();
                    plant.AddCustomParameters(customParameters);
                }

                return plant;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
                return null;
            }
        }


        public PlantsArea RestorePlantArea(PlantsAreaMapping plantsAreaMapping)
        {
            try
            {
                PlantMapping plantMapping = _plantRepository.Get(plantsAreaMapping.PlantId);
                Plant plant = RestorePlant(plantMapping);
                
                PlantsArea area = new PlantsArea(plantsAreaMapping.Id, plant, plantsAreaMapping.Number);

                IList<ServiceScheduleMapping> serviceScheduleMappings =
                    _serviceScheduleMappingRepository.GetAll().AsEnumerable().Where(s => s.PlantsAreaId == area.Id).ToList();

                if (serviceScheduleMappings.Count != 0)
                {
                    ServicesSchedulesState servicesSchedulesState = new ServicesSchedulesState();
                    serviceScheduleMappings.ToList()
                        .ForEach(s => servicesSchedulesState.AddServiceSchedule(RestoreServiceSchedule(s)));

                    area.ServicesSchedulesState = servicesSchedulesState;
                }
                return area;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
                return null;
            }
        }

        public Sensor RestoreSensor(SensorMapping sensorMapping)
        {
            try
            {
                PlantsAreaMapping plantsAreaMapping = _plantsAreaRepository.Get(sensorMapping.PlantsAreaId);
                PlantsArea plantsArea = RestorePlantArea(plantsAreaMapping);

                MeasurableParameterMapping measurableParameterMapping =
                    _measurableParameterRepository.Get(sensorMapping.MeasurableParameterId);
                MeasurableParameter measurableParameter = RestoreMeasurableParameter(measurableParameterMapping);

                ParameterEnum parameter;
                bool parsed = Enum.TryParse(measurableParameterMapping.Type, out parameter);

                if (parsed)
                {
                    switch (parameter)
                    {
                        case ParameterEnum.Nutrient:
                            return new NutrientSensor(sensorMapping.Id, plantsArea,
                                new TimeSpan(0, 0, sensorMapping.MeasuringTimeout), measurableParameter as Nutrient,
                                sensorMapping.NumberOfTimes);
                        case ParameterEnum.SoilPh:
                            return new SoilPhSensor(sensorMapping.Id, plantsArea,
                                new TimeSpan(0, 0, sensorMapping.MeasuringTimeout), measurableParameter as SoilPh,
                                sensorMapping.NumberOfTimes);
                        case ParameterEnum.Humidity:
                            return new HumiditySensor(sensorMapping.Id, plantsArea,
                                new TimeSpan(0, 0, sensorMapping.MeasuringTimeout), measurableParameter as Humidity,
                                sensorMapping.NumberOfTimes);
                        case ParameterEnum.Temperature:
                            return new TemperatureSensor(sensorMapping.Id, plantsArea,
                                new TimeSpan(0, 0, sensorMapping.MeasuringTimeout), measurableParameter as Temperature,
                                sensorMapping.NumberOfTimes);
                    }
                }
                //if custom sensor
                return new CustomSensor(sensorMapping.Id, plantsArea,
                    new TimeSpan(0, 0, sensorMapping.MeasuringTimeout), measurableParameter as CustomParameter,
                    sensorMapping.NumberOfTimes);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
                return null;
            }
        }
    }
}
