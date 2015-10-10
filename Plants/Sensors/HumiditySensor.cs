using System;
using System.ComponentModel.DataAnnotations.Schema;
using PlantingLib.MeasurableParameters;
using PlantingLib.ParametersFunctions;
using PlantingLib.Plants;

namespace PlantingLib.Sensors
{
    public class HumiditySensor : Sensor
    {
        public HumiditySensor(PlantsArea plantsArea, TimeSpan measuringTimeout,
            Humidity humidity, int numberOfTimes)
            : base(plantsArea, measuringTimeout, humidity, numberOfTimes)
        {
            Function = new HumidityFunction(humidity);
        }

        public HumiditySensor(Guid id, PlantsArea plantsArea, TimeSpan measuringTimeout, Humidity humidity, int numberOfTimes) 
            : base(id, plantsArea, measuringTimeout, humidity, numberOfTimes)
        {
            Function = new HumidityFunction(humidity);
        }
    }
}