using System;
using System.Collections.Generic;
using System.Linq;
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
    }

    public class PlantsAreaEqualityComparer : IEqualityComparer<PlantsArea>
    {

        public bool Equals(PlantsArea pa1, PlantsArea pa2)
        {
            if (pa1.Id == pa2.Id)
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(PlantsArea pa)
        {
            return typeof(PlantsArea).GetHashCode();
        }

    }

}
