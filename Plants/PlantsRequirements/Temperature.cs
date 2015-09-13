namespace Planting.PlantsRequirements
{
    public class Temperature : MeasurableParameter
    {
        public Temperature(int optimalTemperature, int minTemperature, int maxTemperature)
            : base(optimalTemperature, minTemperature, maxTemperature)
        {
            Type = MeasurableTypesEnum.Temperature;
        }
    }
}
