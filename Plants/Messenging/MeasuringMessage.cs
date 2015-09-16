using System;
using System.ComponentModel.DataAnnotations;
using PlantingLib.MappingTypes;
using PlantingLib.PlantsRequirements;

namespace PlantingLib.Messenging
{
    public class MeasuringMessage
    {
        public MeasuringMessage(DateTime dateTime, MessageTypesEnum messageType,
            MeasurableTypesEnum measurableType, Guid plantsAreaId, double parameterValue)
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
        public MessageTypesEnum MessageType { get; private set; }
        public MeasurableTypesEnum MeasurableType { get; private set; }
        public Guid PlantsAreaId { get; private set; }
        public double ParameterValue { get; private set; }

        public override string ToString()
        {
            return string.Format("[{0}] Level: {1}, {2} at {3} PlantsArea: {4}.",
                DateTime.ToLongTimeString(), MessageType, MeasurableType,
                PlantsAreaId, ParameterValue.ToString("F2"));
        }

        public MeasuringMessageMapping GetMapping
        {
            get
            {
                return new MeasuringMessageMapping(Id, DateTime, MeasurableType.ToString(),
                    MeasurableType.ToString(), PlantsAreaId, ParameterValue);
            }
        }
    }
}
