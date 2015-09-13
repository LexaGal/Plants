using Planting.ParametersFunctions;

namespace Planting.WeatherTypes
{
    public abstract class WeatherMeasurer
    {
        public TemperatureFunction TemperatureFunction { get; set; }
        public HumidityFunction HumidityFunction { get; set; }
        public SoilPhFunction SoilPhFunction { get; set; }
        public NutrientFunction NutrientFunction { get; set; }
        public WeatherTypesEnum WeatherType { get; protected set; }
        
        public void SetFunctions()
        {
            TemperatureFunction.SetWeatherType(WeatherType);
            HumidityFunction.SetWeatherType(WeatherType);
            SoilPhFunction.SetWeatherType(WeatherType);
            NutrientFunction.SetWeatherType(WeatherType);
        }
    }
}