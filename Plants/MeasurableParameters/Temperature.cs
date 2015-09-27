using System;

namespace PlantingLib.MeasurableParameters
{
    public class Temperature : MeasurableParameter
    {
        public Temperature(int optimalTemperature, int minTemperature, int maxTemperature)
            : base(optimalTemperature, minTemperature, maxTemperature)
        {
            MeasurableType = MeasurableTypeEnum.Temperature;
        
        }

        public Temperature(Guid id, int optimal, int min, int max) : base(id, optimal, min, max)
        {
            MeasurableType = MeasurableTypeEnum.Temperature;
        }
    }
}
