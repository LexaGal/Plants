using System.Collections.Generic;

namespace PlantingLib.Plants
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
