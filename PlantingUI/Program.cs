using System;
using MongoDbConnector;
using PlantingLib.MeasuringsProviders;
using PlantingLib.Observation;
using PlantingLib.Plants;
using PlantingLib.Sensors;
using PlantingLib.ServiceSystems;

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
        
        //public static void Initialize()
        //{
        //    IPlantMappingRepository plantRepository = new PlantMappingRepository();
        //    IPlantsAreaMappingRepository plantsAreaRepository = new PlantsAreaMappingRepository();
        //    IMeasurableParameterMappingRepository measurableParameterRepository = new MeasurableParameterMappingRepository();
        //    ISensorMappingRepository sensorRepository = new SensorMappingRepository();
            
        //    DbMapper dbMapper = new DbMapper(plantRepository,
        //        measurableParameterRepository, null);
        //    int a = 5;
        //    //List<PlantsAreaMapping> plantsAreasMappings = plantsAreaRepository.GetAll();
            
        //    List<SensorMapping> sensorMappings = sensorRepository.GetAll();
        //    _sensorsCollection = new SensorsCollection();
        //    //sensorMappings.ForEach(m => _sensorsCollection.AddSensor(dbMapper.RestoreSensor(m, new PlantsArea())));

        //    _plantsAreas = new PlantsAreas();

        //    _sensorsCollection.Sensors.ToList().ForEach(s => _plantsAreas.AddPlantsArea(s.PlantsArea));

        //    _sensorsMeasuringsProvider = new SensorsMeasuringsProvider(_sensorsCollection);

        //    _plantsAreas = new PlantsAreas(_plantsAreas.Areas.Distinct(new PlantsAreaEqualityComparer()).ToList());

        //    _observer = new Observer(_sensorsMeasuringsProvider, _plantsAreas);

        //    _serviceProvider = new ServiceProvider(_observer, _plantsAreas);

        //    _beginDateTime = DateTime.Now;
        //}
        
        //private static void Send(object sender, ElapsedEventArgs args)
        //{
        //    if (_sensorsMeasuringsProvider != null)
        //    {
        //        TimeSpan timeSpan = args.SignalTime.Subtract(_beginDateTime);

        //        if (timeSpan.TotalSeconds > SystemTimer.RestartTimeSpan.TotalSeconds)
        //        {
        //            _beginDateTime = _beginDateTime.Add(SystemTimer.RestartTimeSpan);

        //            timeSpan = new TimeSpan(0, 0, (int) (timeSpan.TotalSeconds%SystemTimer.RestartTimeSpan.TotalSeconds));

        //            //restarting timer and reseting all functions values to base values (new day after night sleep)
        //            SystemTimer.Restart();
        //            _sensorsCollection.Sensors.ToList().ForEach(s => s.Function.ResetFunction(s.MeasurableParameter.Optimal));
        //        }

        //        SystemTimer.CurrentTimeSpan = timeSpan;
        //        _sensorsMeasuringsProvider.SendMessages(timeSpan);
        //    }
        //}

        static void Main(string[] args)
        {
            MongoDbAccessor mongoDbAccessor = new MongoDbAccessor();
            mongoDbAccessor.Connect();

            //Initialize();
            //Weather.SetWeather(WeatherTypesEnum.Warm);
            //SystemTimer.Start(Send, new TimeSpan(0, 0, 0, 0, 1000));


        }
    }
}
