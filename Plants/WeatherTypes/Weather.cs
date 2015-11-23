using System;

namespace PlantingLib.WeatherTypes
{
    public static class Weather
    {
        public static WeatherTypesEnum WeatherType { get; private set; }
        public static Action<WeatherTypesEnum> WeatherTypeChanged;

        public static void OnWeatherTypeChanged()
        {
            Action<WeatherTypesEnum> handler = WeatherTypeChanged;
            handler?.Invoke(WeatherType);
        }

        public static void SetWeather(WeatherTypesEnum type)
        {
            WeatherType = type;
            OnWeatherTypeChanged();
        }
    }
}