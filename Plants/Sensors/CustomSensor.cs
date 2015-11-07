using System;
using PlantingLib.MeasurableParameters;
using PlantingLib.ParametersFunctions;
using PlantingLib.Plants;

namespace PlantingLib.Sensors
{
    public class CustomSensor : Sensor
    {
        public CustomSensor(Guid id, PlantsArea plantsArea, TimeSpan measuringTimeout, CustomParameter customParameter,
            int numberOfTimes)
            : base(id, plantsArea, measuringTimeout, customParameter, numberOfTimes)
        {
            IsCustom = true;
            Function = new CustomParameterFunction(customParameter);
        }
    }
}