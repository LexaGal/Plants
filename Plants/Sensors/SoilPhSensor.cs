using System;
using PlantingLib.MeasurableParameters;
using PlantingLib.ParametersFunctions;
using PlantingLib.Plants;

namespace PlantingLib.Sensors
{
    public class SoilPhSensor : Sensor
    {
        public SoilPhSensor(Guid id, PlantsArea plantsArea, TimeSpan measuringTimeout, SoilPh soilPh)
            : base(id, plantsArea, measuringTimeout, soilPh)
        {
            Function = new SoilPhFunction(soilPh);
        }
    }
}