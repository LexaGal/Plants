using System;
using System.Collections.Generic;
using System.Linq;
using PlantingLib.MeasurableParameters;
using PlantingLib.Sensors;

namespace PlantingLib.Plants
{
    public class PlantsArea
    {
        public Guid Id { get; private set; }

        public Plant Plant { get; private set; }
        public IList<Sensor> Sensors { get; private set; }
        public int Number { get; private set; }
        public bool IsBeingWatering { get; set; }
        public bool IsBeingNutrienting { get; set; }
        public bool IsBeingWarming { get; set; }
        public bool IsBeingCooling { get; set; }
        
        public PlantsArea(Plant plant, int number)
        {
            Id = Guid.NewGuid();
            Plant = plant;
            Number = number;
            IsBeingWatering = false;
            IsBeingNutrienting = false;
            IsBeingWarming = false;
            IsBeingCooling = false;
            Sensors = new List<Sensor>();
        }

        public PlantsArea(Guid id, Plant plant, int number)
        {
            Id = id;
            Plant = plant;
            Number = number;
            IsBeingWatering = false;
            IsBeingNutrienting = false;
            IsBeingWarming = false;
            IsBeingCooling = false;
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

        public List<Sensor> FindTurnedOffSensors()
        {
            List<Sensor> sensors = new List<Sensor>();
            if (Sensors.All(sensor => sensor.MeasurableType != MeasurableTypeEnum.Temperature))
            {
                sensors.Add(new TemperatureSensor(this, new TimeSpan(0, 0, 1), Plant.Temperature));
            }
            if (Sensors.All(sensor => sensor.MeasurableType != MeasurableTypeEnum.Humidity))
            {
                sensors.Add(new HumiditySensor(this, new TimeSpan(0, 0, 1), Plant.Humidity));
            }
            if (Sensors.All(sensor => sensor.MeasurableType != MeasurableTypeEnum.SoilPh))
            {
                sensors.Add(new SoilPhSensor(this, new TimeSpan(0, 0, 1), Plant.SoilPh));
            }
            if (Sensors.All(sensor => sensor.MeasurableType != MeasurableTypeEnum.Nutrient))
            {
                sensors.Add(new NutrientSensor(this, new TimeSpan(0, 0, 1), Plant.Nutrient));
            }
            return sensors;
        }
     
    }
}
