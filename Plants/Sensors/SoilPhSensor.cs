using System;
using System.ComponentModel.DataAnnotations.Schema;
using PlantingLib.ParametersFunctions;
using PlantingLib.Plants;
using PlantingLib.PlantsRequirements;

namespace PlantingLib.Sensors
{
    [NotMapped]
    public class SoilPhSensor : Sensor
    {
        public SoilPhSensor(Tuple<int, int> location, PlantsArea plantsArea, TimeSpan measuringTimeout, SoilPh soilPh)
            : base(location, plantsArea, measuringTimeout, soilPh)
        {
            Function = new SoilPhFunction(soilPh); 
        }

        public SoilPhSensor()
        {
        }
    }
}