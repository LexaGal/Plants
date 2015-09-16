using PlantingLib.PlantsRequirements;
using PlantingLib.Timers;
using PlantingLib.WeatherTypes;

namespace PlantingLib.ParametersFunctions
{
    public class NutrientFunction : ParameterFunction
    {
        public NutrientFunction(Nutrient nutrient) : base(nutrient)
        {
        }

        public override double NewFunctionValue()
        {
            switch (WeatherType)
            {
                case WeatherTypesEnum.Cold:
                    return CurrentFunctionValue -= 0.5 * Random.NextDouble();

                case WeatherTypesEnum.Warm:
                    return CurrentFunctionValue -= 0.5 * Random.NextDouble();

                case WeatherTypesEnum.Hot:
                    if (SystemTimer.CurrentTimeSpan.Seconds < SystemTimer.RestartTimeSpan.TotalSeconds / 2 ||
                        SystemTimer.CurrentTimeSpan.Seconds > SystemTimer.RestartTimeSpan.TotalSeconds / 1.5)
                    {
                        return CurrentFunctionValue -= 0.25 * Random.NextDouble();
                    }

                    if (SystemTimer.CurrentTimeSpan.Seconds >= SystemTimer.RestartTimeSpan.TotalSeconds / 2
                        && SystemTimer.CurrentTimeSpan.Seconds < SystemTimer.RestartTimeSpan.TotalSeconds / 1.5)
                    {
                        return CurrentFunctionValue -= 0.1 * Random.NextDouble();
                    }
                    return CurrentFunctionValue;

                case WeatherTypesEnum.Rainy:
                    if (SystemTimer.CurrentTimeSpan.Seconds < SystemTimer.RestartTimeSpan.TotalSeconds / 1.5)
                    {
                        return CurrentFunctionValue -= 0.75 * Random.NextDouble();
                    }

                    if (SystemTimer.CurrentTimeSpan.Seconds > SystemTimer.RestartTimeSpan.TotalSeconds / 1.5)
                    {
                        return CurrentFunctionValue -= 0.5 * Random.NextDouble();
                    }
                    return CurrentFunctionValue;
            }
            return CurrentFunctionValue;
        }
    }
}