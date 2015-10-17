using System;
using System.Collections.Generic;
using System.Linq;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;
using PlantingLib.Sensors;

namespace PlantingLib.Plants
{
    public class PlantsArea
    {
        public Guid Id { get; private set; }

        public Plant Plant { get; private set; }
        public IList<Sensor> Sensors { get; private set; }
        public int Number { get; private set; }
        public PlantsAreaServiceState PlantsAreaServiceState { get; set; }

        public PlantsArea(Plant plant, int number)
        {
            Id = Guid.NewGuid();
            Plant = plant;
            Number = number;
            PlantsAreaServiceState = new PlantsAreaServiceState
            {
                Watering = false.ToString(),
                Nutrienting = false.ToString(),
                Warming = false.ToString(),
                Cooling = false.ToString()
            }; 
            Sensors = new List<Sensor>();
        }

        public PlantsArea(Guid id, Plant plant, int number)
        {
            Id = id;
            Plant = plant;
            Number = number;
            PlantsAreaServiceState = new PlantsAreaServiceState
            {
                Watering = false.ToString(),
                Nutrienting = false.ToString(),
                Warming = false.ToString(),
                Cooling = false.ToString()
            };
            Sensors = new List<Sensor>();
        }

        public void AddPlant(Plant plant)
        {
            Number ++;
        }

        public void AddSensor(Sensor sensor)
        {
            if (Sensors == null)
            {
                Sensors = new List<Sensor>();
            }
            if (Sensors.Any(s => s.Id == sensor.Id))
            {
                return;
            }
            Sensors.Add(sensor);
        }

        public List<Sensor> FindSensorsToAdd()
        {
            List<Sensor> sensors = new List<Sensor>();
            if (Sensors.All(sensor => sensor.MeasurableType != MeasurableTypeEnum.Temperature))
            {
                sensors.Add(new TemperatureSensor(null, new TimeSpan(0, 0, 1), Plant.Temperature, 0));
            }
            if (Sensors.All(sensor => sensor.MeasurableType != MeasurableTypeEnum.Humidity))
            {
                sensors.Add(new HumiditySensor(null, new TimeSpan(0, 0, 1), Plant.Humidity, 0));
            }
            if (Sensors.All(sensor => sensor.MeasurableType != MeasurableTypeEnum.SoilPh))
            {
                sensors.Add(new SoilPhSensor(null, new TimeSpan(0, 0, 1), Plant.SoilPh, 0));
            }
            if (Sensors.All(sensor => sensor.MeasurableType != MeasurableTypeEnum.Nutrient))
            {
                sensors.Add(new NutrientSensor(null, new TimeSpan(0, 0, 1), Plant.Nutrient, 0));
            }
            return sensors;
        }

       }
}
