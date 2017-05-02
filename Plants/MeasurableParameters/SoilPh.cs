using System;

namespace PlantingLib.MeasurableParameters
{
    public class SoilPh : MeasurableParameter
    {
        public SoilPh(Guid id, int optimal, int min, int max) : base(id, optimal, min, max)
        {
            MeasurableType = ParameterEnum.SoilPh.ToString();
        }
    }
}