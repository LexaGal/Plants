using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using Planting.MeasuringsProviding;
using Planting.Observing;
using Planting.PlantRequirements;
using Planting.Plants;
using Planting.SchedulingSystems;
using Planting.Sensors;
using Planting.Timer;

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
        private static SchedulingSystem _schedulingSystem;
        private static Schedule _schedule;

        private static DateTime _beginDateTime;
        
        public static void Initialize()
        {
            _plant = new Plant(new Temperature(25, 15, 35), new Humidity(60, 40, 90),
                new SoilPh(5, 4, 7), new Nutrient(14, 11, 20), DateTime.Now.AddMonths(1));
            
            _plantsArea = new PlantsArea();
            _plantsArea.AddPlant(_plant);

            _plantsAreas = new PlantsAreas();
            _plantsAreas.AddPlantsArea(_plantsArea);

            _sensor = new HumiditySensor(new Tuple<int, int>(1, 1), _plantsArea,
                new TimeSpan(2*TimeSpan.TicksPerSecond), _plant.Humidity);

            _sensorsCollection = new SensorsCollection();
            _sensorsCollection.AddSensor(_sensor);

            _sensorsMeasuringsProvider = new SensorsMeasuringsProvider(_sensorsCollection);
            
            _observer = new Observer(_sensorsMeasuringsProvider, _plantsAreas);

            _schedule = new Schedule(_plantsArea.Id, 1000, 300, _plant);
            _schedulingSystem = new SchedulingSystem(_observer, new List<Schedule>{_schedule});

            _beginDateTime = DateTime.Now;
        }

        public static void Send(object sender, ElapsedEventArgs args)
        {
           if (_sensorsMeasuringsProvider != null)
           {
               TimeSpan timeSpan = args.SignalTime.Subtract(_beginDateTime);
                _sensorsMeasuringsProvider.SendMessages(timeSpan);
            }
        }

        static void Main(string[] args)
        {
            Initialize();
            MessageTimer.Start(Send, new TimeSpan(0, 0, 0, 0, 1000));
        }
    }
}
