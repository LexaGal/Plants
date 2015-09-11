using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planting.PlantRequirements
{
    public class Temperature : MeasurableParameter
    {
        public Temperature(uint optimalTemperature, uint minTemperature, uint maxTemperature)
            : base(optimalTemperature, minTemperature, maxTemperature)
        {
            Type = MeasurableTypesEnum.Temperature;
        }
    }
}
