using System;
using Planting.PlantRequirements;
using Planting.Plants;

namespace Planting.Sensors
{
    public class HumiditySensor : Sensor
    {
        public HumiditySensor(Tuple<int, int> location, PlantsArea plantsArea, TimeSpan measuringTimeout, MeasurableParameter requirement)
            : base(location, plantsArea, measuringTimeout, requirement)
        { }
    }
}