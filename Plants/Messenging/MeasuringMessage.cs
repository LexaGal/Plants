using System;
using Planting.PlantsRequirements;

namespace Planting.Messenging
{
    public class MeasuringMessage
    {
        public MeasuringMessage(DateTime dateTime, MessageTypesEnum messageType,
            MeasurableTypesEnum measurableType, string plantsAreaId, double parameterValue)
        {
            DateTime = dateTime;
            MessageType = messageType;
            MeasurableType = measurableType;
            PlantsAreaId = plantsAreaId;
            ParameterValue = parameterValue;
        }

        public DateTime DateTime { get; private set; }
        public MessageTypesEnum MessageType { get; private set; }
        public MeasurableTypesEnum MeasurableType { get; private set; }
        public string PlantsAreaId { get; private set; }
        public double ParameterValue { get; private set; }

        public override string ToString()
        {
            return string.Format("[{0}] Level: {1}, {2} at {3} PlantsArea: {4}",
                DateTime.ToLongTimeString(), MessageType, MeasurableType,
                PlantsAreaId, ParameterValue.ToString("F2"));
        }
    }
}
