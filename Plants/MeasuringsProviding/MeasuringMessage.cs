using System;
using Planting.PlantRequirements;

namespace Planting.MeasuringsProviding
{
    public class MeasuringMessage
    {
        public MeasuringMessage(DateTime dateTime, MessageTypesEnum messageType,
            MeasurableTypesEnum measurableType, string plantsAreaId, int parameterValue)
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
        public int ParameterValue { get; private set; }

        public override string ToString()
        {
            return string.Format("[{0} {1}] Level: {2}, {3} at {4} PlantsArea: {5}", DateTime.ToLongDateString(),
                DateTime.ToLongTimeString(), MessageType, MeasurableType, PlantsAreaId, ParameterValue);
        }
    }
}
