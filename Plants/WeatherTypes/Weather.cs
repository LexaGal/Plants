using System;

namespace PlantingLib.WeatherTypes
{
    public static class Weather
    {
        public static Action<WeatherTypesEnum> WeatherTypeChanged;
        public static WeatherTypesEnum WeatherType { get; private set; }

        public static void OnWeatherTypeChanged()
        {
            var handler = WeatherTypeChanged;
            handler?.Invoke(WeatherType);
        }

        public static void SetWeather(WeatherTypesEnum type)
        {
            WeatherType = type;
            OnWeatherTypeChanged();
        }
    }
}