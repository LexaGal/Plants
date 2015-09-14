using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planting.Sensors;

namespace Planting.Plants
{
    public class PlantsAreas
    {
        public IList<PlantsArea> AllPlantsAreas { get; private set; }

        public PlantsAreas()
        {
            AllPlantsAreas = new List<PlantsArea>();
        }

        public PlantsAreas(IList<PlantsArea> areas)
        {
            AllPlantsAreas = areas;
        }

        public void AddPlantsArea(PlantsArea area)
        {
            if (AllPlantsAreas == null)
            {
                AllPlantsAreas = new List<PlantsArea>();
            }
            AllPlantsAreas.Add(area);
        }
    }
}
