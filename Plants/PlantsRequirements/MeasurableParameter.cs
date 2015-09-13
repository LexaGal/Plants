using System;

namespace Planting.PlantsRequirements
{
    public abstract class MeasurableParameter
    {
        protected MeasurableParameter(int optimal, int min, int max)
        {
            Optimal = optimal;
            Min = min;
            Max = max;
        }

        public int Optimal { get; private set; }
        public int Min { get; private set; }
        public int Max { get; private set; }
        public MeasurableTypesEnum Type { get; set; }
    }
}
