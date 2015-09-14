using System;
using Planting.Plants;
using Planting.PlantsRequirements;

namespace Planting.ServiceSystems
{
    public abstract class ServiceSystem
    {
        public PlantsArea PlantsArea { get; private set; }
        public MeasurableTypesEnum MeasurableType { get; private set; }
        public double ParameterValue { get; private set; }

        protected ServiceSystem(MeasurableTypesEnum measurableType, double parameterValue, PlantsArea plantsArea)
        {
            PlantsArea = plantsArea;
            MeasurableType = measurableType;
            ParameterValue = parameterValue;
        }

        public abstract TimeSpan ComputeTimeForService();

        public abstract void StartService(TimeSpan timeSpan, Func<TimeSpan, TimeSpan> func);
    }
}