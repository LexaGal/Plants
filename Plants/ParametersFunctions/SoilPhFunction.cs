﻿using Planting.PlantsRequirements;
using Planting.Timers;
using Planting.WeatherTypes;

namespace Planting.ParametersFunctions
{
    public class SoilPhFunction : ParameterFunction
    {
        public SoilPhFunction(SoilPh soilPh)
            : base(soilPh)
        {
        }

        public override double NewFunctionValue()
        {
            switch (WeatherType)
            {
                case WeatherTypesEnum.Cold:
                    return CurrentFunctionValue;

                case WeatherTypesEnum.Warm:
                    return CurrentFunctionValue;

                case WeatherTypesEnum.Hot:
                    if (SystemTimer.CurrentTimeSpan.Seconds < SystemTimer.RestartTimeSpan.TotalSeconds / 2 ||
                        SystemTimer.CurrentTimeSpan.Seconds > SystemTimer.RestartTimeSpan.TotalSeconds / 1.5)
                    {
                        return CurrentFunctionValue += 0.25 * Random.NextDouble();
                    }

                    if (SystemTimer.CurrentTimeSpan.Seconds >= SystemTimer.RestartTimeSpan.TotalSeconds / 2
                        && SystemTimer.CurrentTimeSpan.Seconds < SystemTimer.RestartTimeSpan.TotalSeconds / 1.5)
                    {
                        return CurrentFunctionValue += 0.5 * Random.NextDouble();
                    }
                    return CurrentFunctionValue;

                case WeatherTypesEnum.Rainy:
                    if (SystemTimer.CurrentTimeSpan.Seconds < SystemTimer.RestartTimeSpan.TotalSeconds / 1.5)
                    {
                        return CurrentFunctionValue -= 0.25 * Random.NextDouble();
                    }

                    if (SystemTimer.CurrentTimeSpan.Seconds > SystemTimer.RestartTimeSpan.TotalSeconds / 1.5)
                    {
                        return CurrentFunctionValue -= 0.1 * Random.NextDouble();
                    }
                    return CurrentFunctionValue;
            }
            return CurrentFunctionValue;
        }
    }
}