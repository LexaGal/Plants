using System;
using System.ComponentModel.DataAnnotations.Schema;
using PlantingLib.MeasurableParameters;
using PlantingLib.ParametersFunctions;
using PlantingLib.Plants;

namespace PlantingLib.Sensors
{
    public class SoilPhSensor : Sensor
    {
        public SoilPhSensor( PlantsArea plantsArea, TimeSpan measuringTimeout, SoilPh soilPh, int numberOfTimes)
            : base(plantsArea, measuringTimeout, soilPh, numberOfTimes)
        {
            Function = new SoilPhFunction(soilPh); 
        }

        public SoilPhSensor(Guid id, PlantsArea plantsArea, TimeSpan measuringTimeout, SoilPh soilPh, int numberOfTimes)
            : base(id, plantsArea, measuringTimeout, soilPh, numberOfTimes)
        {
            Function = new SoilPhFunction(soilPh);
        }
    }
}