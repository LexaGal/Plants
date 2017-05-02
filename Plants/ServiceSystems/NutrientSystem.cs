using System;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;

namespace PlantingLib.ServiceSystems
{
    public class NutrientSystem : ServiceSystem
    {
        public NutrientSystem(string measurableType, double parameterValue, PlantsArea plantsArea,
            TimeSpan serviceTimeSpan)
            : base(measurableType, parameterValue, plantsArea, serviceTimeSpan)
        {
        }

        public override TimeSpan ComputeTimeForService()
        {
            if (PlantsArea != null)
            {
                if (ServiceTimeSpan == TimeSpan.Zero)
                {
                    TimeSpan timeSpan;
                    if (MeasurableType == ParameterEnum.Nutrient.ToString())
                    {
                        timeSpan = new TimeSpan(0, 0, (int) Math.Abs(ParameterValue -
                                                                     PlantsArea.Plant.Nutrient.Optimal)*2);
                        return timeSpan;
                    }
                    if (MeasurableType == ParameterEnum.SoilPh.ToString())
                    {
                        timeSpan = new TimeSpan(0, 0, (int) Math.Abs(ParameterValue -
                                                                     PlantsArea.Plant.SoilPh.Optimal)*4);
                        return timeSpan;
                    }
                }
                return ServiceTimeSpan;
            }
            return TimeSpan.Zero;
        }
    }
}