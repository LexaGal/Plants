using System;
using System.Linq;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;

namespace PlantingLib.ServiceSystems
{
    public class CustomSystem : ServiceSystem
    {
        public CustomSystem(string measurableType, double parameterValue, PlantsArea plantsArea)
            : base(measurableType, parameterValue, plantsArea)
        {
        }

        public override TimeSpan ComputeTimeForService()
        {
            if (PlantsArea != null)
            {
                TimeSpan timeSpan = new TimeSpan(0, 0, (int) (Math.Abs(ParameterValue -
                                                                       PlantsArea.Plant.CustomParameters.First(
                                                                           cp => cp.MeasurableType == MeasurableType)
                                                                           .Optimal)));
                return timeSpan;
            }
            return TimeSpan.Zero;
        }
    };
}