using System;
using PlantingLib.Plants;

namespace PlantingLib.ServiceSystems
{
    public class WaterSystem : ServiceSystem
    {
        public WaterSystem(string measurableType, double parameterValue, PlantsArea plantsArea, TimeSpan serviceTimeSpan)
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
                                                                           PlantsArea.Plant.Humidity.Optimal))/3);
                    return timeSpan;
                }
                return ServiceTimeSpan;
            }
            return TimeSpan.Zero;
        }
    }   
}
