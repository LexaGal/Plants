namespace PlantingLib.PlantsRequirements
{
    public class Humidity : MeasurableParameter
    {
        public Humidity(int optimalHumidity, int minHumidity, int maxHumidity)
            : base(optimalHumidity, minHumidity, maxHumidity)
        {
            MeasurableType = MeasurableTypesEnum.Humidity;
        }
    }
}