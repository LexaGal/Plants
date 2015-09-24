using System;
using System.Collections.Generic;
using PlantingLib.Sensors;

namespace PlantingLib.Plants
{
    public class PlantsArea
    {
        public Guid Id { get; private set; }

        public Plant Plant { get; private set; }
        public IList<Sensor> Sensors { get; private set; }
        public int Number { get; private set; }

        public PlantsArea(Plant plant, int number)
        {
            Id = Guid.NewGuid();
            Plant = plant;
            Number = number;
        }

        public PlantsArea(Guid id, Plant plant, int number)
        {
            Id = id;
            Plant = plant;
            Number = number;
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
