using System.Linq;
using Database.DatabaseStructure.Repository.Abstract;
using Database.DatabaseStructure.Repository.Concrete;
using Database.MappingTypes;
using Mapper.MapperContext;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;
using PlantingLib.Sensors;

namespace PlantsWpf.SavingData
{
    public class DbModifier
    {
        private readonly PlantsAreas _plantsAreas;
        private readonly SensorsCollection _sensorsCollection;
        private readonly DbMapper _dbMapper;

        public DbModifier(PlantsAreas plantsAreas, SensorsCollection sensorsCollection)
        {
            _plantsAreas = plantsAreas;
            _sensorsCollection = sensorsCollection;
            _dbMapper = new DbMapper();
        }
    
        public void SaveSensor(PlantsArea area, Sensor sensor)
        {
            if (area.FindSensorsToAdd().SingleOrDefault(s =>
                s.MeasurableType == sensor.MeasurableType) == null)
            {
                MeasurableParameterMapping measurableParameterMapping =
                    _dbMapper.GetMeasurableParameterMapping(sensor.MeasurableParameter);
                IMeasurableParameterMappingRepository measurableParameterMappingRepository =
                    new MeasurableParameterMappingRepository();
                measurableParameterMappingRepository.Add(measurableParameterMapping);
                
                area.Plant.AddCustomParameter(sensor.MeasurableParameter as CustomParameter);
                PlantMapping plantMapping = _dbMapper.GetPlantMapping(area.Plant);
                IPlantMappingRepository plantMappingRepository = new PlantMappingRepository();
                plantMappingRepository.Edit(area.Plant.Id, plantMapping);
            }
            area.AddSensor(sensor);
            SensorMapping sensorMapping = _dbMapper.GetSensorMapping(sensor);
            ISensorMappingRepository sensorMappingRepository = new SensorMappingRepository();
            sensorMappingRepository.Add(sensorMapping);
            _sensorsCollection.AddSensor(sensor);
        }

        public void RemoveSensor(PlantsArea area, Sensor sensor)
        {
            SensorMapping sensorMapping = _dbMapper.GetSensorMapping(sensor);
            ISensorMappingRepository sensorMappingRepository = new SensorMappingRepository();
            sensorMappingRepository.Delete(sensorMapping.Id);
            _sensorsCollection.RemoveSensor(sensor);
            area.RemoveSensor(sensor);
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
            measurableParameterMappingRepository.Add(measurableParameterMapping);

            Humidity humidity = plantsArea.Plant.Humidity;
            measurableParameterMapping = _dbMapper.GetMeasurableParameterMapping(humidity);
            measurableParameterMappingRepository.Add(measurableParameterMapping);

            SoilPh soilPh = plantsArea.Plant.SoilPh;
            measurableParameterMapping = _dbMapper.GetMeasurableParameterMapping(soilPh);
            measurableParameterMappingRepository.Add(measurableParameterMapping);

            Nutrient nutrient = plantsArea.Plant.Nutrient;
            measurableParameterMapping = _dbMapper.GetMeasurableParameterMapping(nutrient);
            measurableParameterMappingRepository.Add(measurableParameterMapping);

            Plant plant = plantsArea.Plant;
            PlantMapping plantMapping = _dbMapper.GetPlantMapping(plant);
            plantMappingRepository.Add(plantMapping);

            PlantsAreaMapping plantsAreaMapping = _dbMapper.GetPlantsAreaMapping(plantsArea);
            plantsAreaMappingRepository.Add(plantsAreaMapping);

            foreach (Sensor sensor in plantsArea.Sensors)
            {
                SensorMapping sensorMapping = _dbMapper.GetSensorMapping(sensor);
                sensorMappingRepository.Add(sensorMapping);
                _sensorsCollection.AddSensor(sensor);
            }

            _plantsAreas.AddPlantsArea(plantsArea);
        }
    }
}
