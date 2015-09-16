using System;
using PlantingLib.ParametersFunctions;
using PlantingLib.Plants;
using PlantingLib.PlantsRequirements;

namespace PlantingLib.Sensors
{
    public class TemperatureSensor : Sensor
    {
        public TemperatureSensor(Tuple<int, int> location, PlantsArea plantsArea, TimeSpan measuringTimeout,
            Temperature temperature)
            : base(location, plantsArea, measuringTimeout, temperature)
        {
            Function = new TemperatureFunction(temperature);
        }

        public TemperatureSensor()
        {
        }
    }
}
