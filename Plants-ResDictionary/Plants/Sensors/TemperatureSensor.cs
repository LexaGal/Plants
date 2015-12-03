using System;
using PlantingLib.MeasurableParameters;
using PlantingLib.ParametersFunctions;
using PlantingLib.Plants;

namespace PlantingLib.Sensors
{
    public class TemperatureSensor : Sensor
    {
        public TemperatureSensor(Guid id, PlantsArea plantsArea, TimeSpan measuringTimeout, Temperature temperature)
            : base(id, plantsArea, measuringTimeout, temperature)
        {
            Function = new TemperatureFunction(temperature);
        }
    }
}
