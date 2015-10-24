using System;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;

namespace PlantingLib.ServiceSystems
{
    public class WaterSystem : ServiceSystem
    {
        public WaterSystem(string measurableType, double parameterValue, PlantsArea plantsArea)
            : base(measurableType, parameterValue, plantsArea)
        {
        }

        public override TimeSpan ComputeTimeForService()
        {
            if (PlantsArea != null)
            {
                TimeSpan timeSpan = new TimeSpan(0, 0, (int)(Math.Abs(ParameterValue -
                    PlantsArea.Plant.Humidity.Optimal))/3);
                return timeSpan;
            }
            return TimeSpan.Zero;
        }
    }
}
