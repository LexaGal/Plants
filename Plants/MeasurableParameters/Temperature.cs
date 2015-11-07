using System;

namespace PlantingLib.MeasurableParameters
{
    public class Temperature : MeasurableParameter
    {
        public Temperature(Guid id, int optimal, int min, int max) : base(id, optimal, min, max)
        {
            MeasurableType = ParameterEnum.Temperature.ToString();
        }
    }
}
