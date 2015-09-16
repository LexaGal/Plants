using System;
using System.ComponentModel.DataAnnotations.Schema;
using PlantingLib.ParametersFunctions;
using PlantingLib.Plants;
using PlantingLib.PlantsRequirements;

namespace PlantingLib.Sensors
{
    [NotMapped]
    public class NutrientSensor : Sensor
    {
        public NutrientSensor(Tuple<int, int> location, PlantsArea plantsArea, TimeSpan measuringTimeout,
            Nutrient nutrient)
            : base(location, plantsArea, measuringTimeout, nutrient)
        {
            Function = new NutrientFunction(nutrient);
        }

        public NutrientSensor()
        {
        }
    }
}