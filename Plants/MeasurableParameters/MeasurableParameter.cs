using System;

namespace PlantingLib.MeasurableParameters
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

        protected MeasurableParameter(Guid id, int optimal, int min, int max)
        {
            Id = id;
            Optimal = optimal;
            Min = min;
            Max = max;
        }

        public Guid Id { get; private set; }
        public int Optimal { get; private set; }
        public int Min { get; private set; }
        public int Max { get; private set; }
        public string MeasurableType { get; protected set; }
    }
}
