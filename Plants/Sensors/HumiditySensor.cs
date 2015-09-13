using System;
using Planting.ParametersFunctions;
using Planting.Plants;
using Planting.PlantsRequirements;

namespace Planting.Sensors
{
    public class HumiditySensor : Sensor
    {
        public HumiditySensor(Tuple<int, int> location, PlantsArea plantsArea, TimeSpan measuringTimeout,
            Humidity humidity)
            : base(location, plantsArea, measuringTimeout, humidity)
        {
            Function = new HumidityFunction(humidity);
        }
    }
}