using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using Planting.MeasuringsProviding;
using Planting.Observation;
using Planting.Plants;
using Planting.PlantsRequirements;
using Planting.Sensors;
using Planting.ServiceSystems;
using Planting.Timers;
using Planting.WeatherTypes;

namespace Planting
{
    class Program
    {
        private static Plant _plant;
        private static PlantsArea _plantsArea;
        private static PlantsAreas _plantsAreas;
        private static Sensor _sensor;
        private static SensorsCollection _sensorsCollection;
        private static Observer _observer;
        private static SensorsMeasuringsProvider _sensorsMeasuringsProvider;

        private static DateTime _beginDateTime;
        
        public static void Initialize()
        {
            _plant = new Plant(new Temperature(25, 20, 30), new Humidity(60, 40, 90),
                new SoilPh(5, 4, 7), new Nutrient(14, 11, 20), DateTime.Now.AddMonths(1),
                new TimeSpan(0, 0, 2), new TimeSpan(0, 0, 1));
            
            _plantsArea = new PlantsArea();
            _plantsArea.AddPlant(_plant);

            _plantsAreas = new PlantsAreas();
            _plantsAreas.AddPlantsArea(_plantsArea);

            _sensor = new TemperatureSensor(new Tuple<int, int>(1, 1), _plantsArea,
                new TimeSpan(3*TimeSpan.TicksPerSecond), _plant.Temperature);
            Sensor s = new TemperatureSensor(new Tuple<int, int>(1, 1), _plantsArea,
                new TimeSpan(2*TimeSpan.TicksPerSecond), _plant.Temperature);

            _sensorsCollection = new SensorsCollection();
            _sensorsCollection.AddSensor(_sensor);
            _sensorsCollection.AddSensor(s);

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
