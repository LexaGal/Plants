using System;
using System.ComponentModel.DataAnnotations;
using PlantingLib.MeasurableParameters;
using PlantingLib.WeatherTypes;

namespace PlantingLib.ParametersFunctions
{
    public abstract class ParameterFunction
    {
        protected readonly Random Random;

        protected ParameterFunction(MeasurableParameter measurableParameter)
        {
            MeasurableParameter = measurableParameter;
            CurrentFunctionValue = MeasurableParameter.Optimal;
            Random = new Random();

            Weather.WeatherTypeChanged += @enum => { WeatherType = @enum; };
        }

        [Key]
        public Guid Id { get; set; }

        public MeasurableParameter MeasurableParameter { get; }
        public double CurrentFunctionValue { get; set; }
        public double CurrentWeatherValue { get; set; } = double.NaN;
        public WeatherTypesEnum WeatherType { get; private set; }

        public void SetWeatherType(WeatherTypesEnum weatherType)
        {
            WeatherType = weatherType;
        }

        public virtual double NewFunctionValue()
        {
            //if ((CurrentFunctionValue > MeasurableParameter.Min &&
            //     CurrentFunctionValue < CurrentWeatherValue) ||
            //    (CurrentFunctionValue < MeasurableParameter.Max &&
            //     CurrentFunctionValue > CurrentWeatherValue))
            //{
            //    return CurrentWeatherValue;
            //}
            return CurrentFunctionValue += (CurrentWeatherValue - MeasurableParameter.Optimal) /
                                           (CurrentWeatherValue > MeasurableParameter.Optimal
                                               ? CurrentWeatherValue / MeasurableParameter.Optimal
                                               : MeasurableParameter.Optimal / CurrentWeatherValue) / 6;
        }

        public void SetWeatherValue(double newWeatherValue)
        {
            CurrentWeatherValue = newWeatherValue;
        }

        public void SetCurrentValue(double newFunctionValue)
        {
            // = newFunctionValue;
            CurrentFunctionValue = newFunctionValue;
        }
    }
}