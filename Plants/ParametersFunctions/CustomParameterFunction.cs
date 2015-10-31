using PlantingLib.MeasurableParameters;
using PlantingLib.Timers;
using PlantingLib.WeatherTypes;

namespace PlantingLib.ParametersFunctions
{
    public class CustomParameterFunction : ParameterFunction
    {
        public CustomParameterFunction(CustomParameter customParameter)
            : base(customParameter)
        {
        }

        public override double NewFunctionValue()
        {
            switch (WeatherType)
            {
                case WeatherTypesEnum.Cold:
                    if (SystemTimer.CurrentTimeSpan.Seconds < SystemTimer.RestartTimeSpan.TotalSeconds / 3)
                    {
                        return CurrentFunctionValue += Random.Next(-1, MeasurableParameter.Min/10) *
                                                       (Random.Next(1, MeasurableParameter.Max/10) + Random.NextDouble());
                    }

                    if (SystemTimer.CurrentTimeSpan.Seconds >= SystemTimer.RestartTimeSpan.TotalSeconds / 3)
                    {
                        return CurrentFunctionValue -= (Random.Next(1, MeasurableParameter.Optimal/10) + Random.NextDouble());
                    }
                    return CurrentFunctionValue;

                case WeatherTypesEnum.Warm:
                    if (SystemTimer.CurrentTimeSpan.Seconds < SystemTimer.RestartTimeSpan.TotalSeconds / 1.5)
                    {
                        return CurrentFunctionValue += Random.NextDouble() *
                                                       (Random.Next(1, MeasurableParameter.Optimal/10) + Random.NextDouble());
                    }

                    if (SystemTimer.CurrentTimeSpan.Seconds >= SystemTimer.RestartTimeSpan.TotalSeconds / 1.5)
                    {
                        return CurrentFunctionValue += Random.Next(-1, MeasurableParameter.Min/10) *
                                                       (Random.Next(1, MeasurableParameter.Optimal/10) + Random.NextDouble());
                    }
                    return CurrentFunctionValue;

                case WeatherTypesEnum.Hot:
                    if (SystemTimer.CurrentTimeSpan.Seconds < SystemTimer.RestartTimeSpan.TotalSeconds / 2 ||
                        SystemTimer.CurrentTimeSpan.Seconds > SystemTimer.RestartTimeSpan.TotalSeconds / 1.5)
                    {
                        return CurrentFunctionValue += Random.NextDouble() *
                                                       (Random.Next(1, MeasurableParameter.Optimal/10) + Random.NextDouble());
                    }

                    if (SystemTimer.CurrentTimeSpan.Seconds >= SystemTimer.RestartTimeSpan.TotalSeconds / 2
                        && SystemTimer.CurrentTimeSpan.Seconds < SystemTimer.RestartTimeSpan.TotalSeconds / 1.5)
                    {
                        return CurrentFunctionValue += Random.NextDouble() *
                                                       (Random.Next(1, MeasurableParameter.Max/10) + Random.NextDouble());
                    }
                    return CurrentFunctionValue;

                case WeatherTypesEnum.Rainy:
                    if (SystemTimer.CurrentTimeSpan.Seconds < SystemTimer.RestartTimeSpan.TotalSeconds / 1.5)
                    {
                        return CurrentFunctionValue += Random.Next(-1, MeasurableParameter.Optimal/10) *
                                                       (Random.Next(1, MeasurableParameter.Optimal/10) + Random.NextDouble());
                    }

                    if (SystemTimer.CurrentTimeSpan.Seconds > SystemTimer.RestartTimeSpan.TotalSeconds / 1.5)
                    {
                        return CurrentFunctionValue -= Random.NextDouble() *
                                                       (Random.Next(1, MeasurableParameter.Max/10) + Random.NextDouble());
                    }
                    return CurrentFunctionValue;
            }
            return CurrentFunctionValue;
        }
    }
}