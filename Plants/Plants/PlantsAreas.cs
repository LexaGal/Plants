using System.Collections.Generic;
using System.Linq;

namespace PlantingLib.Plants
{
    public class PlantsAreas
    {
        public List<PlantsArea> Areas { get; private set; }

        public PlantsAreas()
        {
            Areas = new List<PlantsArea>();
        }

        public PlantsAreas(List<PlantsArea> areas)
        {
            Areas = areas;
        }

        public void AddPlantsArea(PlantsArea area)
        {
            if (Areas == null)
            {
                Areas = new List<PlantsArea>();
            }
            Areas.Add(area);
            Areas = Areas.OrderBy(p => p.Plant.Name).ToList();
        }

        public bool RemovePlantsArea(PlantsArea area)
        {
            if (Areas != null)
            {
                if (Areas.All(s => s.Id != area.Id))
                {
                    return false;
                }
                return Areas.Remove(area);
            }
            return false;
        }
    }
}
