using System;
using PlantingLib.MeasurableParameters;
using PlantingLib.ParametersFunctions;
using PlantingLib.Plants;

namespace PlantingLib.Sensors
{
    public class CustomSensor : Sensor
    {
        public CustomSensor(PlantsArea plantsArea, TimeSpan measuringTimeout,
            CustomParameter customParameter, int numberOfTimes)
            : base(plantsArea, measuringTimeout, customParameter, numberOfTimes)
        {
            Function = new CustomParameterFunction(customParameter);
        }

        public CustomSensor(Guid id, PlantsArea plantsArea, TimeSpan measuringTimeout, CustomParameter customParameter,
            int numberOfTimes)
            : base(id, plantsArea, measuringTimeout, customParameter, numberOfTimes)
        {
            Function = new CustomParameterFunction(customParameter);
        }
    }
}