using System;
using PlantingLib.MappingTypes;

namespace PlantingLib.PlantsRequirements
{
    public abstract class MeasurableParameter
    {
        protected MeasurableParameter(int optimal, int min, int max)
        {
            Id = Guid.NewGuid();
            Optimal = optimal;
            Min = min;
            Max = max;
        }

        public Guid Id { get; private set; }
        public int Optimal { get; private set; }
        public int Min { get; private set; }
        public int Max { get; private set; }
        public MeasurableTypesEnum MeasurableType { get; protected set; }

        public MeasurableParameterMapping GetMapping
        {
            get
            {
                return new MeasurableParameterMapping(Id, Optimal, Optimal, Max, MeasurableType.ToString());
            }
        }
    }
}
