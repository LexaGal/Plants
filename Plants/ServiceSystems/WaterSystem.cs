using System;
using Planting.Plants;
using Planting.PlantsRequirements;

namespace Planting.ServiceSystems
{
    public class WaterSystem : ServiceSystem
    {
        public WaterSystem(MeasurableTypesEnum measurableType, double parameterValue, PlantsArea plantsArea)
            : base(measurableType, parameterValue, plantsArea)
        {
        }

        public override TimeSpan ComputeTimeForService()
        {
            if (PlantsArea != null)
            {
                TimeSpan timeSpan = new TimeSpan(0, 0, (int)(Math.Abs(ParameterValue -
                    PlantsArea.PlantRequirements.Humidity.Optimal))/3);
                return timeSpan;
            }
            return TimeSpan.Zero;
        }
    }
}
