using System;

namespace PlantingLib.MeasurableParameters
{
    public class CustomParameter: MeasurableParameter
    {
        public CustomParameter(int optimal, int min, int max, string measurableType)
            : base(optimal, min, max)
        {
            MeasurableType = measurableType;

        }

        public CustomParameter(Guid id, int optimal, int min, int max, string measurableType)
            : base(id, optimal, min, max)
        {
            MeasurableType = measurableType;
        }
    }
}