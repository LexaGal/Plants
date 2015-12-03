namespace PlantingLib.WeatherTypes
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