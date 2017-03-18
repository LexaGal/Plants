using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using AspNet.Identity.MySQL.Repository.Concrete;
using Database.DatabaseStructure.Repository.Abstract;
using Database.DatabaseStructure.Repository.Concrete;
using Database.MappingTypes;
using PlantingLib.MeasurableParameters;
using PlantingLib.Messenging;
using PlantingLib.Plants;
using PlantingLib.Plants.ServicesScheduling;
using PlantingLib.Sensors;

namespace Mapper.MapperContext
{
    public class DbMapper
    {
        private readonly MySqlPlantMappingRepository _sqlPlantMappingRepository;
        private readonly MySqlMeasurableParameterMappingRepository _sqlMeasurableParameterMappingRepository;
        private readonly MySqlServiceScheduleMappingRepository _sqlServiceScheduleMappingRepository;
        private readonly IPlantMappingRepository _plantRepository;
        private readonly IMeasurableParameterMappingRepository _measurableParameterRepository;
        private readonly IServiceScheduleMappingRepository _serviceScheduleMappingRepository;

        public static DbMapper GetDbMapper()
        {
            return new DbMapper(new PlantMappingRepository(), new MeasurableParameterMappingRepository(), new ServiceScheduleMappingRepository());
        }

        public static DbMapper GetMySqlDbMapper()
        {
            return new DbMapper(new MySqlPlantMappingRepository(), new MySqlMeasurableParameterMappingRepository(), new MySqlServiceScheduleMappingRepository());
        }

        public DbMapper(IPlantMappingRepository plantRepository,
            IMeasurableParameterMappingRepository measurableParameterRepository, IServiceScheduleMappingRepository serviceScheduleMappingRepository)
        {
            _plantRepository = plantRepository;
            _measurableParameterRepository = measurableParameterRepository;
            _serviceScheduleMappingRepository = serviceScheduleMappingRepository;
        }

        public DbMapper(MySqlPlantMappingRepository sqlPlantMappingRepository, MySqlMeasurableParameterMappingRepository sqlMeasurableParameterMappingRepository, MySqlServiceScheduleMappingRepository sqlServiceScheduleMappingRepository)
        {
            _sqlPlantMappingRepository = sqlPlantMappingRepository;
            _sqlMeasurableParameterMappingRepository = sqlMeasurableParameterMappingRepository;
            _sqlServiceScheduleMappingRepository = sqlServiceScheduleMappingRepository;
            //throw new NotImplementedException();
        }

        public PlantMapping GetPlantMapping(Plant plant)
        {
            //if custom sensor
            StringBuilder builder = new StringBuilder();

            List<MeasurableParameter> customParameters = plant.MeasurableParameters.Where(m => m is CustomParameter).ToList();
            if (customParameters.Count != 0)
            {
                customParameters.ForEach(c => builder.Append(c.Id.ToString() + ','));
                builder.Remove(builder.Length - 1, 1);
            }
            return new PlantMapping(plant.Id, plant.Temperature.Id, plant.Humidity.Id,
                plant.SoilPh.Id, plant.Nutrient.Id, plant.Name.ToString(),
                builder.ToString());
        }

        public PlantsAreaMapping GetPlantsAreaMapping(PlantsArea plantsArea)
        {
            return new PlantsAreaMapping(plantsArea.Id, plantsArea.Plant.Id, plantsArea.Number, plantsArea.UserId);
        }

        public MeasuringMessageMapping GetMeasuringMessageMapping(MeasuringMessage measuringMessage)
        {
            return new MeasuringMessageMapping(measuringMessage.Id, measuringMessage.DateTime,
                measuringMessage.MessageType.ToString(), measuringMessage.MeasurableType,
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
                (int)sensor.MeasuringTimeout.TotalSeconds, sensor.MeasurableParameter.Id,
                sensor.MeasurableParameter.MeasurableType);
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
                serviceSchedule.ServiceName,
                (int)serviceSchedule.ServicingSpan.TotalSeconds,
                (int)serviceSchedule.ServicingPauseSpan.TotalSeconds, builder.ToString());
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
                //MessageBox.Show(e.StackTrace, $"MeasurableParameter Id: {measurableParameterMapping.Id}");
                return null;
            }
        }

        public ServiceSchedule RestoreServiceSchedule(ServiceScheduleMapping serviceScheduleMapping, List<MeasurableParameter> measurableParameters)
        {
            try
            {
                List<MeasurableParameter> mps = new List<MeasurableParameter>();
                if (!string.IsNullOrEmpty(serviceScheduleMapping.MeasurableParametersIds))
                {
                    string[] ids = serviceScheduleMapping.MeasurableParametersIds.Split(',');
                    List<MeasurableParameterMapping> measurableParameterMappings =
                        ids.Select(id => _sqlMeasurableParameterMappingRepository.Get(Guid.Parse(id))).ToList();

                    measurableParameterMappings.ForEach(
                        mapping =>
                        {
                            var param =
                                measurableParameters.SingleOrDefault(
                                    parameter => parameter != null && parameter.Id == mapping.Id);
                            if (param != null)
                            {
                                mps.Add(param);
                            }
                        });
                }
                return new ServiceSchedule(serviceScheduleMapping.Id, serviceScheduleMapping.PlantsAreaId,
                    serviceScheduleMapping.ServiceState,
                    new TimeSpan(0, 0, serviceScheduleMapping.ServicingSpan),
                    new TimeSpan(0, 0, serviceScheduleMapping.ServicingPauseSpan),
                    mps);
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.StackTrace, $"ServiceSchedule Id: {serviceScheduleMapping.Id}");
                return null;
            }
        }

        public Plant RestorePlant(PlantMapping plantMapping)
        {
            try
            {
                MeasurableParameterMapping temperatureMapping =
                    _sqlMeasurableParameterMappingRepository.Get(plantMapping.TemperatureId);
                MeasurableParameterMapping soilPhMapping =
                    _sqlMeasurableParameterMappingRepository.Get(plantMapping.SoilPhId);
                MeasurableParameterMapping humidityMapping =
                    _sqlMeasurableParameterMappingRepository.Get(plantMapping.HumidityId);
                MeasurableParameterMapping nutrientMapping =
                    _sqlMeasurableParameterMappingRepository.Get(plantMapping.NutrientId);

                Temperature temperature = RestoreMeasurableParameter(temperatureMapping) as Temperature;
                Humidity humidity = RestoreMeasurableParameter(humidityMapping) as Humidity;
                SoilPh soilPh = RestoreMeasurableParameter(soilPhMapping) as SoilPh;
                Nutrient nutrient = RestoreMeasurableParameter(nutrientMapping) as Nutrient;

                PlantNameEnum name = (PlantNameEnum)Enum.Parse(typeof(PlantNameEnum), plantMapping.Name);
                Plant plant = new Plant(plantMapping.Id, temperature, humidity, soilPh, nutrient, name);

                //if custom sensor
                if (!string.IsNullOrEmpty(plantMapping.CustomParametersIds))
                {
                    string[] ids = plantMapping.CustomParametersIds.Split(',');
                    List<MeasurableParameterMapping> measurableParameterMappings =
                        ids.Select(id => _sqlMeasurableParameterMappingRepository.Get(Guid.Parse(id))).ToList();

                    List<MeasurableParameter> measurableParameters =
                        measurableParameterMappings.Select(RestoreMeasurableParameter)
                            .ToList();

                    plant.AddMeasurableParameters(measurableParameters);
                }
                return plant;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.StackTrace, $"Plant Id: {plantMapping.Id}");
                return null;
            }
        }


        public PlantsArea RestorePlantArea(PlantsAreaMapping plantsAreaMapping)
        {
            try
            {
                PlantMapping plantMapping = _sqlPlantMappingRepository.Get(plantsAreaMapping.PlantId);
                Plant plant = RestorePlant(plantMapping);

                PlantsArea area = new PlantsArea(plantsAreaMapping.Id, plantsAreaMapping.UserId, plant, plantsAreaMapping.Number);

                List<ServiceScheduleMapping> serviceScheduleMappings =
                    _sqlServiceScheduleMappingRepository.GetAll(s => s.PlantsAreaId == area.Id);

                if (serviceScheduleMappings.Count != 0)
                {
                    serviceScheduleMappings.ToList()
                        .ForEach(s => area.ServicesSchedulesStates.AddServiceSchedule(RestoreServiceSchedule(s, area.Plant.MeasurableParameters)));
                }
                return area;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.StackTrace, $"PlantsArea Id: {plantsAreaMapping.Id}");
                return null;
            }
        }

        public Sensor RestoreSensor(SensorMapping sensorMapping, PlantsArea plantsArea)
        {
            try
            {
                MeasurableParameter mp =
                    plantsArea.Plant.MeasurableParameters.SingleOrDefault(
                        m => m != null && m.Id == sensorMapping.MeasurableParameterId);

                if (mp != null)
                {
                    ParameterEnum parameter;
                    bool parsed = Enum.TryParse(mp.MeasurableType, out parameter);

                    if (parsed)
                    {
                        switch (parameter)
                        {
                            case ParameterEnum.Nutrient:
                                return new NutrientSensor(sensorMapping.Id, plantsArea,
                                    new TimeSpan(0, 0, sensorMapping.MeasuringTimeout), mp as Nutrient);
                            case ParameterEnum.SoilPh:
                                return new SoilPhSensor(sensorMapping.Id, plantsArea,
                                    new TimeSpan(0, 0, sensorMapping.MeasuringTimeout), mp as SoilPh);
                            case ParameterEnum.Humidity:
                                return new HumiditySensor(sensorMapping.Id, plantsArea,
                                    new TimeSpan(0, 0, sensorMapping.MeasuringTimeout), mp as Humidity);
                            case ParameterEnum.Temperature:
                                return new TemperatureSensor(sensorMapping.Id, plantsArea,
                                    new TimeSpan(0, 0, sensorMapping.MeasuringTimeout), mp as Temperature);
                        }
                    }
                    //if custom sensor
                    return new CustomSensor(sensorMapping.Id, plantsArea,
                        new TimeSpan(0, 0, sensorMapping.MeasuringTimeout), mp as CustomParameter);
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.StackTrace, $"Sensor Id: {sensorMapping.Id}");
                return null;
            }
            return null;
        }
    }
}
