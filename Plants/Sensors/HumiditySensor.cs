using System;
using System.ComponentModel.DataAnnotations.Schema;
using PlantingLib.ParametersFunctions;
using PlantingLib.Plants;
using PlantingLib.PlantsRequirements;

namespace PlantingLib.Sensors
{
    [NotMapped]
    public class HumiditySensor : Sensor
    {
        public HumiditySensor(Tuple<int, int> location, PlantsArea plantsArea, TimeSpan measuringTimeout,
            Humidity humidity)
            : base(location, plantsArea, measuringTimeout, humidity)
        {
            Function = new HumidityFunction(humidity);
        }

        public HumiditySensor()
        {}
    }
}