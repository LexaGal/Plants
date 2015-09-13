using System;
using Planting.PlantsRequirements;
using Planting.WeatherTypes;

namespace Planting.ParametersFunctions
{
    public abstract class ParameterFunction
    {
        public MeasurableParameter MeasurableParameter { get; private set; }
        public Double CurrentFunctionValue { get; set; }
        public WeatherTypesEnum WeatherType { get; private set; }
        protected readonly Random Random;

        protected ParameterFunction(MeasurableParameter measurableParameter)
        {
            MeasurableParameter = measurableParameter;
            CurrentFunctionValue = MeasurableParameter.Optimal;
            Random = new Random();

            Weather.WeatherTypeChanged += @enum => 
            {
                WeatherType = @enum;
            };
        }

        public void SetWeatherType(WeatherTypesEnum weatherType)
        {
            WeatherType = weatherType;
        }

        public abstract double NewFunctionValue();
        
        public void ResetFunction()
        {
            CurrentFunctionValue = MeasurableParameter.Optimal;
        }       
    }
}
