using System;
using Planting.ParametersFunctions;
using Planting.Plants;
using Planting.PlantsRequirements;

namespace Planting.Sensors
{
    public class SoilPhSensor : Sensor
    {
        public SoilPhSensor(Tuple<int, int> location, PlantsArea plantsArea, TimeSpan measuringTimeout, SoilPh soilPh)
            : base(location, plantsArea, measuringTimeout, soilPh)
        {
            Function = new SoilPhFunction(soilPh); 
        }
    }
}