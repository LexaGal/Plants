using System;
using Planting.Messenging;

namespace Planting.WeatherTypes
{
    public static class Weather
    {
        public static WeatherTypesEnum WeatherType { get; private set; }
        public static Action<WeatherTypesEnum> WeatherTypeChanged;

        public static void OnWeatherTypeChanged()
        {
            Action<WeatherTypesEnum> handler = WeatherTypeChanged;
            if (handler != null)
            {
                handler(WeatherType);
            }
        }

        public static void SetWeather(WeatherTypesEnum type)
        {
            WeatherType = type;
            OnWeatherTypeChanged();
        }

        public static WeatherMeasurer GetWeatherMeasurer()
        {
            switch (WeatherType)
            {
                    case WeatherTypesEnum.Cold:
                    return new ColdWeatherMeasurer();

                    case WeatherTypesEnum.Warm:
                    return new WarmWeatherMeasurer();

                    case WeatherTypesEnum.Hot:
                    return new HotWeatherMeasurer();

                    case WeatherTypesEnum.Rainy:
                    return new RainyWeatherMeasurer();

                default: return new WarmWeatherMeasurer();
            }
        }
    }
}