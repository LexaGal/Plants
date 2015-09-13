using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planting.ParametersFunctions;
using Planting.Plants;
using Planting.PlantsRequirements;

namespace Planting.Sensors
{
    public class TemperatureSensor : Sensor
    {
        public TemperatureSensor(Tuple<int, int> location, PlantsArea plantsArea, TimeSpan measuringTimeout,
            Temperature temperature)
            : base(location, plantsArea, measuringTimeout, temperature)
        {
            Function = new TemperatureFunction(temperature);
        }
    }
}
