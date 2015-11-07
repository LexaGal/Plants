using System;
using PlantingLib.MeasurableParameters;
using PlantingLib.ParametersFunctions;
using PlantingLib.Plants;

namespace PlantingLib.Sensors
{
    public class HumiditySensor : Sensor
    {
        public HumiditySensor(Guid id, PlantsArea plantsArea, TimeSpan measuringTimeout, Humidity humidity, int numberOfTimes) 
            : base(id, plantsArea, measuringTimeout, humidity, numberOfTimes)
        {
            Function = new HumidityFunction(humidity);
        }
    }
}