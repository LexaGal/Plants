using System;
using System.ComponentModel.DataAnnotations;
using PlantingLib.MeasurableParameters;

namespace PlantingLib.Messenging
{
    public class MeasuringMessage
    {
        public MeasuringMessage(DateTime dateTime, MessageTypeEnum messageType,
            MeasurableTypeEnum measurableType, Guid plantsAreaId, double parameterValue)
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

        public DateTime DateTime { get; private set; }
        public MessageTypeEnum MessageType { get; private set; }
        public MeasurableTypeEnum MeasurableType { get; private set; }
        public Guid PlantsAreaId { get; private set; }
        public double ParameterValue { get; private set; }

        public override string ToString()
        {
            return string.Format("[{0}] Level: {1}, {2} at {3} PlantsArea: {4}.",
                DateTime.ToLongTimeString(), MessageType, MeasurableType,
                PlantsAreaId, ParameterValue.ToString("F2"));
        }

      
    }
}
