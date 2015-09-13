namespace Planting.PlantsRequirements
{
    public class Humidity : MeasurableParameter
    {
        public Humidity(int optimalHumidity, int minHumidity, int maxHumidity)
            : base(optimalHumidity, minHumidity, maxHumidity)
        {
            Type = MeasurableTypesEnum.Humidity;
        }
    }
}