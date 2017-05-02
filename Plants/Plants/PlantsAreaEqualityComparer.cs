using System.Collections.Generic;

namespace PlantingLib.Plants
{
    public class PlantsAreaEqualityComparer : IEqualityComparer<PlantsArea>
    {
        public bool Equals(PlantsArea pa1, PlantsArea pa2)
        {
            if (pa1.Id == pa2.Id)
                return true;
            return false;
        }

        public int GetHashCode(PlantsArea pa)
        {
            return typeof(PlantsArea).GetHashCode();
        }
    }
}