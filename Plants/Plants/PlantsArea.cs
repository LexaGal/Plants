using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planting.Plants
{
    public class PlantsArea
    {
        public string Id { get; private set; }
        public IList<Plant> Plants { get; private set; }

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

        public Plant PlantRequirements
        {
            get { return Plants.First(); }
        }
    }
}
