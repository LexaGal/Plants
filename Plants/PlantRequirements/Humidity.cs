namespace Planting.PlantRequirements
{
    public class Humidity : MeasurableParameter
    {
        public Humidity(uint optimalHumidity, uint minHumidity, uint maxHumidity)
            : base(optimalHumidity, minHumidity, maxHumidity)
        {
            Type = MeasurableTypesEnum.Humidity;
        }
    }
}