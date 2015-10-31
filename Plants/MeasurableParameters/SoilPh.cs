using System;

namespace PlantingLib.MeasurableParameters
{
    public class SoilPh : MeasurableParameter
    {
        public SoilPh(int optimalSoilPh, int minSoilPh, int maxSoilPh)
            : base(optimalSoilPh, minSoilPh, maxSoilPh)
        {
            MeasurableType = ParameterEnum.SoilPh.ToString();

        }

        public SoilPh(Guid id, int optimal, int min, int max) : base(id, optimal, min, max)
        {
            MeasurableType = ParameterEnum.SoilPh.ToString();

        }
    }

}