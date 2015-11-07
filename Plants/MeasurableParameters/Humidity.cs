using System;

namespace PlantingLib.MeasurableParameters
{
    public class Humidity : MeasurableParameter
    {
        public Humidity(Guid id, int optimal, int min, int max) : base(id, optimal, min, max)
        {
            MeasurableType = ParameterEnum.Humidity.ToString();
        }
    }
}