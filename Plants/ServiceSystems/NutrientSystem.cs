using System;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;

namespace PlantingLib.ServiceSystems
{
    public class NutrientSystem : ServiceSystem
    {
        public NutrientSystem(MeasurableTypeEnum measurableType, double parameterValue, PlantsArea plantsArea)
            : base(measurableType, parameterValue, plantsArea)
        {
        }

        public override TimeSpan ComputeTimeForService()
        {
            if (PlantsArea != null)
            {
                TimeSpan timeSpan;
                if (MeasurableType == MeasurableTypeEnum.Nutrient)
                {
                    timeSpan = new TimeSpan(0, 0, (int) (Math.Abs(ParameterValue -
                                                                  PlantsArea.Plant.Nutrient.Optimal))*2);
                    return timeSpan;
                }
                if (MeasurableType == MeasurableTypeEnum.SoilPh)
                {
                    timeSpan = new TimeSpan(0, 0, (int)(Math.Abs(ParameterValue -
                                                                  PlantsArea.Plant.SoilPh.Optimal))*4);
                    return timeSpan;
                }
            }
            return TimeSpan.Zero;
        }
    };
}