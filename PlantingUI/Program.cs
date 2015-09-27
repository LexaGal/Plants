using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Database.DatabaseStructure.Repository.Abstract;
using Database.DatabaseStructure.Repository.Concrete;
using Database.MappingTypes;
using Mapper.MapperContext;
using PlantingLib.MeasurableParameters;
using PlantingLib.MeasuringsProviding;
using PlantingLib.Observation;
using PlantingLib.Plants;
using PlantingLib.Sensors;
using PlantingLib.ServiceSystems;
using PlantingLib.Timers;
using PlantingLib.WeatherTypes;

namespace PlantingUI
{
    class Program
    {
        private static SensorsCollection _sensorsCollection;
        private static Observer _observer;
        private static PlantsAreas _plantsAreas;
        private static SensorsMeasuringsProvider _sensorsMeasuringsProvider;
        private static ServiceProvider _serviceProvider;

        private static DateTime _beginDateTime;
        
        public static void Initialize()
        {
            IPlantMappingRepository plantRepository = new PlantMappingRepository();
            IPlantsAreaMappingRepository plantsAreaRepository = new PlantsAreaMappingRepository();
            IMeasurableParameterMappingRepository measurableParameterRepository = new MeasurableParameterMappingRepository();
            ISensorMappingRepository sensorRepository = new SensorMappingRepository();
            
            DbMapper dbMapper = new DbMapper(plantRepository, plantsAreaRepository,
                measurableParameterRepository);

            //IList<PlantsAreaMapping> plantsAreasMappings = plantsAreaRepository.GetAll().ToList();
            
            List<SensorMapping> sensorMappings = sensorRepository.GetAll().ToList();
            _sensorsCollection = new SensorsCollection();
            sensorMappings.ForEach(m => _sensorsCollection.AddSensor(dbMapper.RestoreSensor(m)));

            _plantsAreas = new PlantsAreas();

            _sensorsCollection.AllSensors.ToList().ForEach(s => _plantsAreas.AddPlantsArea(s.PlantsArea));

            _sensorsMeasuringsProvider = new SensorsMeasuringsProvider(_sensorsCollection);

            _plantsAreas = new PlantsAreas(_plantsAreas.AllPlantsAreas.Distinct(new PlantsAreaEqualityComparer()).ToList());

            _observer = new Observer(_sensorsMeasuringsProvider, _plantsAreas);

            _serviceProvider = new ServiceProvider(_observer, _plantsAreas);

            _beginDateTime = DateTime.Now;
        }

        private static void Send(object sender, ElapsedEventArgs args)
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
            Weather.SetWeather(WeatherTypesEnum.Warm);
            SystemTimer.Start(Send, new TimeSpan(0, 0, 0, 0, 1000));
        }
    }
}
