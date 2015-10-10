using System;
using PlantingLib.MeasurableParameters;
using PlantingLib.ParametersFunctions;
using PlantingLib.Plants;

namespace PlantingLib.Sensors
{
    public class TemperatureSensor : Sensor
    {
        public TemperatureSensor(PlantsArea plantsArea, TimeSpan measuringTimeout,
            Temperature temperature, int numberOfTimes)
            : base(plantsArea, measuringTimeout, temperature, numberOfTimes)
        {
            Function = new TemperatureFunction(temperature);
        }

        public TemperatureSensor(Guid id, PlantsArea plantsArea, TimeSpan measuringTimeout, Temperature temperature, int numberOfTimes)
            : base(id, plantsArea, measuringTimeout, temperature, numberOfTimes)
        {
            Function = new TemperatureFunction(temperature);
        }
    }
}
