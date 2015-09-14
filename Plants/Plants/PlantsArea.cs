using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planting.Sensors;

namespace Planting.Plants
{
    public class PlantsArea
    {
        public string Id { get; private set; }
        public IList<Plant> Plants { get; private set; }
        public IList<Sensor> Sensors { get; private set; }

        public PlantsArea(IList<Plant> plants)
        {
            Id = Guid.NewGuid().ToString().Substring(0, 8);
            Plants = plants;
        }

        public PlantsArea()
        {
            Id = Guid.NewGuid().ToString().Substring(0, 8);
            Plants = new List<Plant>();
        }

        public void AddPlant(Plant plant)
        {
            if (Plants == null)
            {
                Plants = new List<Plant>();
            }
            Plants.Add(plant);
        }

        public void AddSensor(Sensor sensor)
        {
            if (Sensors == null)
            {
                Sensors = new List<Sensor>();
            }
            Sensors.Add(sensor);
        }

        public Plant PlantRequirements
        {
            get { return Plants.First(); }
        }
    }
}
