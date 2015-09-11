using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planting.PlantRequirements;
using Planting.Plants;

namespace Planting.Sensors
{
    public class TemperatureSensor : Sensor
    {
        public TemperatureSensor(Tuple<int, int> location, PlantsArea plantsArea, TimeSpan measuringTimeout, MeasurableParameter requirement)
        : base(location, plantsArea, measuringTimeout, requirement)
        {}
    }
}
