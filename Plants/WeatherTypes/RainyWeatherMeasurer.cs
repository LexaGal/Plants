namespace Planting.WeatherTypes
{
    public class RainyWeatherMeasurer : WeatherMeasurer
    {
        public RainyWeatherMeasurer()
        {
            WeatherType = WeatherTypesEnum.Hot;
            SetFunctions();
        }
    }
}