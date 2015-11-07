using System;
using PlantingLib.Plants;

namespace PlantingLib.ServiceSystems
{
    public class TemperatureSystem : ServiceSystem
    {
        public TemperatureSystem(string measurableType, double parameterValue, PlantsArea plantsArea, TimeSpan serviceTimeSpan)
            : base(measurableType, parameterValue, plantsArea, serviceTimeSpan)
        {
        }

        public override TimeSpan ComputeTimeForService()
        {
            if (PlantsArea != null)
            {
                if (ServiceTimeSpan == TimeSpan.Zero)
                {
                    TimeSpan timeSpan = new TimeSpan(0, 0, (int) (Math.Abs(ParameterValue -
                                                                           PlantsArea.Plant.Temperature.Optimal)));
                    return timeSpan;
                }
                return ServiceTimeSpan;
            }
            return TimeSpan.Zero;
        }
     }
}
