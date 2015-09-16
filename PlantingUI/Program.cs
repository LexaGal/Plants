using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Timers;
using Database.DatabaseStructure.Repository.Abstract;
using Database.DatabaseStructure.Repository.Concrete;
using PlantingLib.MappingTypes;
using PlantingLib.MeasuringsProviding;
using PlantingLib.Observation;
using PlantingLib.Plants;
using PlantingLib.PlantsRequirements;
using PlantingLib.Sensors;
using PlantingLib.ServiceSystems;
using PlantingLib.Timers;
using PlantingLib.WeatherTypes;

namespace PlantingUI
{
    class Program
    {
        private static PlantsAreas _plantsAreas;
        private static SensorsCollection _sensorsCollection;
        private static Observer _observer;
        private static SensorsMeasuringsProvider _sensorsMeasuringsProvider;

        private static DateTime _beginDateTime;
        
        public static void Initialize()
        {
            IPlantRepository plantRepository = new PlantRepository();
            List<PlantMapping> plantMappings = plantRepository.GetAll().ToList();

            IPlantsAreaRepository plantsAreaRepository = new PlantsAreaRepository();
            List<PlantsAreaMapping> areas = plantsAreaRepository.GetAll().ToList();

            IMeasurableParameterRepository measurableParameterRepository = new MeasurableParameterRepository();
            List<MeasurableParameterMapping> measurableParameterMappings = measurableParameterRepository.GetAll().ToList();

            ISensorRepository sensorRepository = new SensorRepository();
            List<SensorMapping> sensors = sensorRepository.GetAll().ToList();
            
            PlantMapping plantMapping = new PlantMapping(
                Guid.NewGuid(),
                measurableParameterMappings.First(m => m.Type == MeasurableTypesEnum.Temperature.ToString()).Id,
                measurableParameterMappings.First(m => m.Type == MeasurableTypesEnum.Humidity.ToString()).Id,
                measurableParameterMappings.First(m => m.Type == MeasurableTypesEnum.SoilPh.ToString()).Id,
                measurableParameterMappings.First(m => m.Type == MeasurableTypesEnum.Nutrient.ToString()).Id,
                1000, 1, 1);

            plantRepository.Add(plantMapping);

            _sensorsMeasuringsProvider = new SensorsMeasuringsProvider(_sensorsCollection);
            
            _observer = new Observer(_sensorsMeasuringsProvider, _plantsAreas);
            
            ServiceProvider provider = new ServiceProvider(_observer, _plantsAreas);

            _beginDateTime = DateTime.Now;
        }

        public static void Send(object sender, ElapsedEventArgs args)
        {
            if (_sensorsMeasuringsProvider != null)
            {
                TimeSpan timeSpan = args.SignalTime.Subtract(_beginDateTime);

                if (timeSpan.TotalSeconds > SystemTimer.RestartTimeSpan.TotalSeconds)
                {
                    _beginDateTime = _beginDateTime.Add(SystemTimer.RestartTimeSpan);

                    timeSpan = new TimeSpan(0, 0, (int) (timeSpan.TotalSeconds%SystemTimer.RestartTimeSpan.TotalSeconds));

                    //restarting timer and reseting all functions values to base values (new day after night sleep)
                    SystemTimer.Restart();
                    _sensorsCollection.AllSensors.ToList().ForEach(s => s.Function.ResetFunction());
                }

                SystemTimer.CurrentTimeSpan = timeSpan;
                _sensorsMeasuringsProvider.SendMessages(timeSpan);
            }
        }

        static void Main(string[] args)
        {
            Initialize();
            Weather.SetWeather(WeatherTypesEnum.Rainy);
            SystemTimer.Start(Send, new TimeSpan(0, 0, 0, 0, 1000));
        }
    }
}
