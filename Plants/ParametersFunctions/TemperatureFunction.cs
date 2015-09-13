using Planting.PlantsRequirements;
using Planting.Timers;
using Planting.WeatherTypes;

namespace Planting.ParametersFunctions
{
    public class TemperatureFunction : ParameterFunction
    {
        public TemperatureFunction(Temperature temperature) : base(temperature)
        {
        }

        public override double NewFunctionValue()
        {
            switch (WeatherType)
            {
                case WeatherTypesEnum.Cold:
                    if (SystemTimer.CurrentTimeSpan.Seconds < SystemTimer.RestartTimeSpan.TotalSeconds/3)
                    {
                        return CurrentFunctionValue += Random.Next(-1, 2) *
                                                       (Random.Next(1, 5) + Random.NextDouble());
                    }

                    if (SystemTimer.CurrentTimeSpan.Seconds >= SystemTimer.RestartTimeSpan.TotalSeconds/3)
                    {
                        return CurrentFunctionValue -= (Random.Next(1, 3) + Random.NextDouble());
                    }
                    return CurrentFunctionValue;

                case WeatherTypesEnum.Warm:
                    if (SystemTimer.CurrentTimeSpan.Seconds < SystemTimer.RestartTimeSpan.TotalSeconds/1.5)
                    {
                        return CurrentFunctionValue += Random.NextDouble() *
                                                       (Random.Next(1, 3) + Random.NextDouble());
                    }

                    if (SystemTimer.CurrentTimeSpan.Seconds >= SystemTimer.RestartTimeSpan.TotalSeconds/1.5)
                    {
                        return CurrentFunctionValue += Random.Next(-1, 2) *
                                                       (Random.Next(1, 3) + Random.NextDouble());
                    }
                    return CurrentFunctionValue;

                case WeatherTypesEnum.Hot:
                    if (SystemTimer.CurrentTimeSpan.Seconds < SystemTimer.RestartTimeSpan.TotalSeconds/2 ||
                        SystemTimer.CurrentTimeSpan.Seconds > SystemTimer.RestartTimeSpan.TotalSeconds/1.5)
                    {
                        return CurrentFunctionValue += Random.NextDouble() *
                                                       (Random.Next(1, 3) + Random.NextDouble());
                    }

                    if (SystemTimer.CurrentTimeSpan.Seconds >= SystemTimer.RestartTimeSpan.TotalSeconds/2
                        && SystemTimer.CurrentTimeSpan.Seconds < SystemTimer.RestartTimeSpan.TotalSeconds/1.5)
                    {
                        return CurrentFunctionValue += Random.NextDouble() *
                                                       (Random.Next(1, 5) + Random.NextDouble());
                    }
                    return CurrentFunctionValue;

                case WeatherTypesEnum.Rainy:
                    if (SystemTimer.CurrentTimeSpan.Seconds < SystemTimer.RestartTimeSpan.TotalSeconds/1.5)
                    {
                        return CurrentFunctionValue += Random.Next(-1, 3) *
                                                       (Random.Next(1, 3) + Random.NextDouble());
                    }

                    if (SystemTimer.CurrentTimeSpan.Seconds > SystemTimer.RestartTimeSpan.TotalSeconds/1.5)
                    {
                        return CurrentFunctionValue -= Random.NextDouble() *
                                                       (Random.Next(1, 5) + Random.NextDouble());
                    }
                    return CurrentFunctionValue;
            }
            return CurrentFunctionValue;
        }
    }
}