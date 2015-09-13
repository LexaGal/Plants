using System;
using Planting.ParametersFunctions;
using Planting.Plants;
using Planting.PlantsRequirements;

namespace Planting.Sensors
{
    public class NutrientSensor : Sensor
    {
        public NutrientSensor(Tuple<int, int> location, PlantsArea plantsArea, TimeSpan measuringTimeout,
            Nutrient nutrient)
            : base(location, plantsArea, measuringTimeout, nutrient)
        {
            Function = new NutrientFunction(nutrient);
        }
    }
}