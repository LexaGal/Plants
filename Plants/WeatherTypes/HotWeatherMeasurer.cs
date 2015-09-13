namespace Planting.WeatherTypes
{
    public class HotWeatherMeasurer : WeatherMeasurer
    {
        public HotWeatherMeasurer()
        {
            WeatherType = WeatherTypesEnum.Hot;
            SetFunctions();
        }
    }
}