using System;
using System.ComponentModel.DataAnnotations;

namespace PlantingLib.MappingTypes
{
    public class MeasuringMessageMapping
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime DateTime { get; set; }
        public string MessageType { get; set; }
        public string MeasurableType { get; set; }
        public Guid PlantsAreaId { get; set; }
        public double ParameterValue { get; set; }

        public MeasuringMessageMapping()
        {
        }

        public MeasuringMessageMapping(Guid id, DateTime dateTime, string messageType,
            string measurableType, Guid plantsAreaId, double parameterValue)
        {
            Id = id;
            DateTime = dateTime;
            MessageType = messageType;
            MeasurableType = measurableType;
            PlantsAreaId = plantsAreaId;
            ParameterValue = parameterValue;
        }
    }
}