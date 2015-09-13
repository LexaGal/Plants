namespace Planting.WeatherTypes
{
    public class WarmWeatherMeasurer : WeatherMeasurer
    {
        public WarmWeatherMeasurer()
        {
            WeatherType = WeatherTypesEnum.Warm;
            SetFunctions();
        }
    }
}