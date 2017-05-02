using System;

namespace PlantingLib.ServiceSystems
{
    public class ServiceMessage
    {
        public ServiceMessage(Guid plantsAreaId, string measurableType, double parameterValue, TimeSpan timeSpan)
        {
            PlantsAreaId = plantsAreaId;
            MeasurableType = measurableType;
            ParameterValue = parameterValue;
            TimeSpan = timeSpan;
        }

        public Guid PlantsAreaId { get; }
        public string MeasurableType { get; }
        public double ParameterValue { get; }
        public TimeSpan TimeSpan { get; }

        public override string ToString()
        {
            return $"{MeasurableType} at {PlantsAreaId} plants area was set to {ParameterValue} during {TimeSpan}.";
        }
    }
}