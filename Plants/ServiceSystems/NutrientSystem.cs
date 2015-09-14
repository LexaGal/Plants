using System;
using Planting.Plants;
using Planting.PlantsRequirements;

namespace Planting.ServiceSystems
{
    public class NutrientSystem : ServiceSystem
    {
        public NutrientSystem(MeasurableTypesEnum measurableType, double parameterValue, PlantsArea plantsArea)
            : base(measurableType, parameterValue, plantsArea)
        {
        }

        public override TimeSpan ComputeTimeForService()
        {
            throw new NotImplementedException();
        }

        public override void StartService(TimeSpan timeSpan, Func<TimeSpan, TimeSpan> func1)
        {
            throw new NotImplementedException();
        }
    };
}