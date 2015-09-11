using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planting.PlantRequirements
{
    public class ParameterFunction
    {
        public MeasurableParameter MeasurableParameter { get; private set; }
        public double CurrentFunctionValue { get; set; }

        public ParameterFunction(MeasurableParameter measurableParameter)
        {
            MeasurableParameter = measurableParameter;
            CurrentFunctionValue = MeasurableParameter.Optimal;
        }

        public double NewFunctionValue(ParameterFunctionTypesEnum type)
        {
            Random random = new Random();
            
            switch (MeasurableParameter.Type)
            {
                  case MeasurableTypesEnum.Temperature:
                    if (type == ParameterFunctionTypesEnum.Increasing)
                    {
                        return CurrentFunctionValue + random.Next(1, 3);
                    }
                    if (type == ParameterFunctionTypesEnum.Decreasing)
                    {
                        return CurrentFunctionValue - random.Next(1, 3);
                    }
                    if (type == ParameterFunctionTypesEnum.Constant)
                    {
                        return CurrentFunctionValue;
                    }
                    break;

                case MeasurableTypesEnum.SoilPh:
                    if (type == ParameterFunctionTypesEnum.Increasing)
                    {
                        return CurrentFunctionValue + random.NextDouble();
                    }
                    if (type == ParameterFunctionTypesEnum.Decreasing)
                    {
                        return CurrentFunctionValue - random.NextDouble();
                    }
                    if (type == ParameterFunctionTypesEnum.Constant)
                    {
                        return CurrentFunctionValue;
                    }
                    break;
                
                case MeasurableTypesEnum.Humidity:
                    if (type == ParameterFunctionTypesEnum.Increasing)
                    {
                        return CurrentFunctionValue + random.Next(1, 5);
                    }
                    if (type == ParameterFunctionTypesEnum.Decreasing)
                    {
                        return CurrentFunctionValue - random.Next(1, 5);
                    }
                    if (type == ParameterFunctionTypesEnum.Constant)
                    {
                        return CurrentFunctionValue;
                    }
                    break;

                case MeasurableTypesEnum.Nutrient:
                    if (type == ParameterFunctionTypesEnum.Increasing)
                    {
                        return CurrentFunctionValue + random.Next(1, 3) + random.NextDouble();
                    }
                    if (type == ParameterFunctionTypesEnum.Decreasing)
                    {
                        return CurrentFunctionValue - random.Next(1, 3) - random.NextDouble();
                    }
                    if (type == ParameterFunctionTypesEnum.Constant)
                    {
                        return CurrentFunctionValue;
                    }
                    break;
            }
            return CurrentFunctionValue;
        }

        public void ResetFunction()
        {
            CurrentFunctionValue = MeasurableParameter.Optimal;
        }
    }
}
