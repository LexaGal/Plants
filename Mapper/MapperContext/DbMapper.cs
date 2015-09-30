using System;
using Database.DatabaseStructure.Repository.Abstract;
using Database.DatabaseStructure.Repository.Concrete;
using Database.MappingTypes;
using PlantingLib.MeasurableParameters;
using PlantingLib.Messenging;
using PlantingLib.Plants;
using PlantingLib.Sensors;

namespace Mapper.MapperContext
{
    public class DbMapper
    {
        private readonly IPlantMappingRepository _plantRepository;
        private readonly IPlantsAreaMappingRepository _plantsAreaRepository;
        private readonly IMeasurableParameterMappingRepository _measurableParameterRepository;

        public DbMapper(IPlantMappingRepository plantRepository, IPlantsAreaMappingRepository plantsAreaRepository,
            IMeasurableParameterMappingRepository measurableParameterRepository)
        {
            _plantRepository = plantRepository;
            _plantsAreaRepository = plantsAreaRepository;
            _measurableParameterRepository = measurableParameterRepository;
        }

        public DbMapper()
        {
        }

        public PlantMapping GetPlantMapping(Plant plant)
        {
            return new PlantMapping(plant.Id, plant.Temperature.Id, plant.Humidity.Id,
                plant.SoilPh.Id, plant.Nutrient.Id, (int) plant.GrowingTime.TotalSeconds,
                (int) plant.WateringSpan.TotalSeconds, (int) plant.NutrientingSpan.TotalSeconds, plant.Name.ToString());

        }

        public PlantsAreaMapping GetPlantsAreaMapping(PlantsArea plantsArea)
        {
            return new PlantsAreaMapping(plantsArea.Id, plantsArea.Plant.Id, plantsArea.Number);
        }

        public MeasuringMessageMapping GetMeasuringMessageMapping(MeasuringMessage measuringMessage)
        {
            return new MeasuringMessageMapping(measuringMessage.Id, measuringMessage.DateTime,
                measuringMessage.MeasurableType.ToString(), measuringMessage.MeasurableType.ToString(),
                measuringMessage.PlantsAreaId, measuringMessage.ParameterValue);
        }

        public MeasurableParameterMapping GetMeasurableParameterMapping(MeasurableParameter measurableParameter)
        {
            return new MeasurableParameterMapping(measurableParameter.Id, measurableParameter.Optimal,
                measurableParameter.Min, measurableParameter.Max,
                measurableParameter.MeasurableType.ToString());
        }

        public SensorMapping GetSensorMapping(Sensor sensor)
        {
            return new SensorMapping(sensor.Id, sensor.PlantsArea.Id, 
                (int) sensor.MeasuringTimeout.TotalSeconds, sensor.MeasurableParameter.Id,
                sensor.MeasurableParameter.MeasurableType.ToString());
        }

        public MeasurableParameter RestoreMeasurableParameter(MeasurableParameterMapping measurableParameterMapping)
        {
            switch (measurableParameterMapping.Type)
            {
                case "Nutrient":
                    return new Nutrient(measurableParameterMapping.Id, measurableParameterMapping.Optimal,
                        measurableParameterMapping.Min, measurableParameterMapping.Max);
                case "SoilPh":
                    return new SoilPh(measurableParameterMapping.Id, measurableParameterMapping.Optimal,
                        measurableParameterMapping.Min, measurableParameterMapping.Max);
                case "Humidity":
                    return new Humidity(measurableParameterMapping.Id, measurableParameterMapping.Optimal,
                        measurableParameterMapping.Min, measurableParameterMapping.Max);
                case "Temperature":
                    return new Temperature(measurableParameterMapping.Id, measurableParameterMapping.Optimal,
                        measurableParameterMapping.Min, measurableParameterMapping.Max);
                default:
                    return null;
            }
        }

        public Plant RestorePlant(PlantMapping plantMapping)
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
            
            return new Plant(plantMapping.Id, temperature, humidity, soilPh, nutrient,
                new TimeSpan(0, 0, plantMapping.GrowingTime),
                new TimeSpan(0, 0, plantMapping.WateringSpan),
                new TimeSpan(0, 0, plantMapping.NutrientingSpan), name);
        }

        public PlantsArea RestorePlantArea(PlantsAreaMapping plantsAreaMapping)
        {
            PlantMapping plantMapping = _plantRepository.Get(plantsAreaMapping.PlantId);
            Plant plant = RestorePlant(plantMapping);
            
            return new PlantsArea(plantsAreaMapping.Id, plant, plantsAreaMapping.Number);
        }

        public Sensor RestoreSensor(SensorMapping sensorMapping)
        {
            PlantsAreaMapping plantsAreaMapping = _plantsAreaRepository.Get(sensorMapping.PlantsAreaId);
            PlantsArea plantsArea = RestorePlantArea(plantsAreaMapping);

            MeasurableParameterMapping measurableParameterMapping =
                _measurableParameterRepository.Get(sensorMapping.MeasurableParameterId);
            MeasurableParameter measurableParameter = RestoreMeasurableParameter(measurableParameterMapping);
            
            switch (sensorMapping.Type)
            {
                case "Nutrient":
                    return new NutrientSensor(sensorMapping.Id, plantsArea, 
                        new TimeSpan(0, 0, sensorMapping.MeasuringTimeout), measurableParameter as Nutrient);
                case "SoilPh":
                    return new SoilPhSensor(sensorMapping.Id, plantsArea,
                         new TimeSpan(0, 0, sensorMapping.MeasuringTimeout), measurableParameter as SoilPh);
                case "Humidity":
                     return new HumiditySensor(sensorMapping.Id, plantsArea,
                           new TimeSpan(0, 0, sensorMapping.MeasuringTimeout), measurableParameter as Humidity);
                case "Temperature":
                     return new TemperatureSensor(sensorMapping.Id, plantsArea,
                        new TimeSpan(0, 0, sensorMapping.MeasuringTimeout), measurableParameter as Temperature);
               default:
                    return null;
            }
        }
    }
}
