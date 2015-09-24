using System;
using PlantingLib.MeasurableParameters;

namespace PlantingLib.ServiceSystems
{
    public class ServiceMessage
    {
        public ServiceMessage(Guid plantsAreaId, MeasurableTypesEnum measurableType, double parameterValue, TimeSpan timeSpan)
        {
            PlantsAreaId = plantsAreaId;
            MeasurableType = measurableType;
            ParameterValue = parameterValue;
            TimeSpan = timeSpan;
        }

        public Guid PlantsAreaId { get; private set; }
        public MeasurableTypesEnum MeasurableType { get; private set; }
        public double ParameterValue { get; private set; }
        public TimeSpan TimeSpan { get; private set; }

        public override string ToString()
        {
            return String.Format("{0} at {1} plants area was set to {2} during {3}.", MeasurableType, PlantsAreaId, ParameterValue, TimeSpan);
        }
    }
}
