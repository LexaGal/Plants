using System;

namespace PlantingLib.MeasurableParameters
{
    public class Nutrient : MeasurableParameter
    {
        public Nutrient(Guid id, int optimal, int min, int max) : base(id, optimal, min, max)
        {
            MeasurableType = ParameterEnum.Nutrient.ToString();
        }
    }
}