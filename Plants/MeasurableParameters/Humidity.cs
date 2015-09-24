using System;

namespace PlantingLib.MeasurableParameters
{
    public class Humidity : MeasurableParameter
    {
        public Humidity(int optimalHumidity, int minHumidity, int maxHumidity)
            : base(optimalHumidity, minHumidity, maxHumidity)
        {
            MeasurableType = MeasurableTypesEnum.Humidity;
        }

        public Humidity(Guid id, int optimal, int min, int max) : base(id, optimal, min, max)
        {
            MeasurableType = MeasurableTypesEnum.Humidity;
        }
    }
}