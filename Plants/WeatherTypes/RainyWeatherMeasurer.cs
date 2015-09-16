namespace PlantingLib.WeatherTypes
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