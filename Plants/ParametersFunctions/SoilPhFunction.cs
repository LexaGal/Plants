using System;
using PlantingLib.MeasurableParameters;
using PlantingLib.Timers;
using PlantingLib.WeatherTypes;

namespace PlantingLib.ParametersFunctions
{
    public class SoilPhFunction : ParameterFunction
    {
        public SoilPhFunction(SoilPh soilPh)
            : base(soilPh)
        {
        }

        public override double NewFunctionValue()
        {
            try
            {
                if (!double.IsNaN(CurrentWeatherValue))
                {
                    return base.NewFunctionValue();
                }
                switch (WeatherType)
                {
                    case WeatherTypesEnum.Cold:
                        return CurrentFunctionValue += 0.125*Random.Next(-1, 2);

                    case WeatherTypesEnum.Warm:
                        return CurrentFunctionValue += 0.125*Random.Next(-1, 2);

                    case WeatherTypesEnum.Hot:
                        if ((SystemTimer.CurrentTimeSpan.Seconds < SystemTimer.RestartTimeSpan.TotalSeconds/2) ||
                            (SystemTimer.CurrentTimeSpan.Seconds > SystemTimer.RestartTimeSpan.TotalSeconds/1.5))
                            return CurrentFunctionValue += 0.25*Random.NextDouble();

                        if ((SystemTimer.CurrentTimeSpan.Seconds >= SystemTimer.RestartTimeSpan.TotalSeconds/2)
                            && (SystemTimer.CurrentTimeSpan.Seconds < SystemTimer.RestartTimeSpan.TotalSeconds/1.5))
                            return CurrentFunctionValue += 0.5*Random.NextDouble();
                        return CurrentFunctionValue;

                    case WeatherTypesEnum.Rainy:
                        if (SystemTimer.CurrentTimeSpan.Seconds < SystemTimer.RestartTimeSpan.TotalSeconds/1.5)
                            return CurrentFunctionValue -= 0.25*Random.NextDouble();

                        if (SystemTimer.CurrentTimeSpan.Seconds > SystemTimer.RestartTimeSpan.TotalSeconds/1.5)
                            return CurrentFunctionValue -= 0.1*Random.NextDouble();
                        return CurrentFunctionValue;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                return CurrentFunctionValue + Random.NextDouble();
            }
            return CurrentFunctionValue;
        }
    }
}