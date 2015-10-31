using System;
using System.Collections.Generic;
using System.Linq;
using Database.DatabaseStructure.Repository.Abstract;
using Database.DatabaseStructure.Repository.Concrete;
using Database.MappingTypes;
using Mapper.MapperContext;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;
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

        public DbModifier(PlantsAreas plantsAreas, SensorsCollection sensorsCollection,
            IMeasurableParameterMappingRepository measurableParameterMappingRepository,
            IPlantMappingRepository plantMappingRepository, ISensorMappingRepository sensorMappingRepository)
        {
            _plantsAreas = plantsAreas;
            _sensorsCollection = sensorsCollection;
            _measurableParameterMappingRepository = measurableParameterMappingRepository;
            _plantMappingRepository = plantMappingRepository;
            _sensorMappingRepository = sensorMappingRepository;
            _dbMapper = new DbMapper();
        }

        public void SaveAddedSensor(PlantsArea area, Sensor sensor)
        {
            //if custom sensor
            if (area.FindMainSensorsToAdd().SingleOrDefault(s =>
                s.MeasurableType == sensor.MeasurableType) == null)
            {
                MeasurableParameterMapping measurableParameterMapping =
                    _dbMapper.GetMeasurableParameterMapping(sensor.MeasurableParameter);
                _measurableParameterMappingRepository.Add(measurableParameterMapping);

                area.Plant.AddCustomParameter(sensor.MeasurableParameter as CustomParameter);

                PlantMapping plantMapping = _dbMapper.GetPlantMapping(area.Plant);
                _plantMappingRepository.Edit(area.Plant.Id, plantMapping);

                ServiceState serviceState = new ServiceState(sensor.MeasurableType, true);
                area.PlantsAreaServiceState.AddServiceState(serviceState);

                if (MeasurableParametersInfo.GetParameterInfo(sensor.MeasurableType) == null)
                {
                    MeasurableParametersInfo.ParametersInfo.Add(new ParameterInfo(sensor.MeasurableType,
                        new List<ServiceState> {serviceState}));
                }
            }

            area.AddSensor(sensor);
            SensorMapping sensorMapping = _dbMapper.GetSensorMapping(sensor);
            _sensorMappingRepository.Add(sensorMapping);
            _sensorsCollection.AddSensor(sensor);
        }

        public void RemoveSensor(PlantsArea area, Sensor sensor)
        {
            _sensorMappingRepository.Delete(sensor.Id);
            _sensorsCollection.RemoveSensor(sensor);
            area.RemoveSensor(sensor);

            //if custom sensor
            ServiceState serviceState =
                area.PlantsAreaServiceState.ServiceStates.FirstOrDefault(
                    s => s.ServiceName == String.Format("*{0}*", sensor.MeasurableType));
            if (serviceState != null)
            {
                area.PlantsAreaServiceState.RemoveServiceState(serviceState);

                area.Plant.RemoveCustomParameter(sensor.MeasurableParameter as CustomParameter);

                PlantMapping plantMapping = _dbMapper.GetPlantMapping(area.Plant);
                _plantMappingRepository.Edit(area.Plant.Id, plantMapping);

                _measurableParameterMappingRepository.Delete(sensor.MeasurableParameter.Id);
            }
        }

        public void SaveAddedPlantsArea(PlantsArea plantsArea)
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
