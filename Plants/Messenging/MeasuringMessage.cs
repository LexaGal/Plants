using System;
using System.ComponentModel.DataAnnotations;

namespace PlantingLib.Messenging
{
    public class MeasuringMessage
    {
        public MeasuringMessage(DateTime dateTime, MessageTypeEnum messageType,
            string measurableType, Guid plantsAreaId, double parameterValue)
        {
            Id = Guid.NewGuid();
            DateTime = dateTime;
            MessageType = messageType;
            MeasurableType = measurableType;
            PlantsAreaId = plantsAreaId;
            ParameterValue = parameterValue;
        }

        [Key]
        public Guid Id { get; private set; }

        public DateTime DateTime { get; }
        public MessageTypeEnum MessageType { get; }
        public string MeasurableType { get; }
        public Guid PlantsAreaId { get; }
        public double ParameterValue { get; }

        public override string ToString()
        {
            return
                $"[{DateTime.ToLongTimeString()}] Level: {MessageType}, {MeasurableType} at {PlantsAreaId} PlantsArea: {ParameterValue.ToString("F2")}.";
        }
    }
}