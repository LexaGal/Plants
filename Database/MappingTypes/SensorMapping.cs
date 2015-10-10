using System;
using System.ComponentModel.DataAnnotations;

namespace Database.MappingTypes
{
    public class SensorMapping
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PlantsAreaId { get; set; }
        public int MeasuringTimeout { get; set; }
        public Guid MeasurableParameterId { get; set; }
        public string Type { get; set; }
        public int NumberOfTimes { get; set; }

        public SensorMapping()
        {
        }

        public SensorMapping(Guid id, Guid plantsAreaId, int measuringTimeout, Guid measurableParameterId, string type,
            int numberOfTimes)
        {
            Id = id;
            PlantsAreaId = plantsAreaId;
            MeasuringTimeout = measuringTimeout;
            MeasurableParameterId = measurableParameterId;
            Type = type;
            NumberOfTimes = numberOfTimes;
        }
    }
}