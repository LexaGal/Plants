using System;

namespace PlantingLib.ServiceSystems
{
    public class ServiceMessage
    {
        public Guid PlantsAreaId { get; private set; }
        public string MeasurableType { get; private set; }
        public double ParameterValue { get; private set; }
        public TimeSpan TimeSpan { get; private set; }
        
        public ServiceMessage(Guid plantsAreaId, string measurableType, double parameterValue, TimeSpan timeSpan)
        {
            PlantsAreaId = plantsAreaId;
            MeasurableType = measurableType;
            ParameterValue = parameterValue;
            TimeSpan = timeSpan;
        }
        
        public override string ToString()
        {
            return String.Format("{0} at {1} plants area was set to {2} during {3}.", MeasurableType, PlantsAreaId, ParameterValue, TimeSpan);
        }
    }
}
