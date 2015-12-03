using System;

namespace PlantingLib.MeasurableParameters
{
    public class CustomParameter: MeasurableParameter
    {
        public CustomParameter(Guid id, int optimal, int min, int max, string measurableType)
            : base(id, optimal, min, max)
        {
            MeasurableType = measurableType;
        }

        public bool IsForService(string serviceName)
        {
            return serviceName == $"*{MeasurableType}*";
        }
    }
}