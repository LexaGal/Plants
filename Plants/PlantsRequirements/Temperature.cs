namespace PlantingLib.PlantsRequirements
{
    public class Temperature : MeasurableParameter
    {
        public Temperature(int optimalTemperature, int minTemperature, int maxTemperature)
            : base(optimalTemperature, minTemperature, maxTemperature)
        {
            MeasurableType = MeasurableTypesEnum.Temperature;
        }
    }
}
