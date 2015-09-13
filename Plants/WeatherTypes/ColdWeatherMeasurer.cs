namespace Planting.WeatherTypes
{
    public class ColdWeatherMeasurer : WeatherMeasurer
    {
        public ColdWeatherMeasurer()
        {
            WeatherType = WeatherTypesEnum.Cold;
            SetFunctions();
        }
    }
}