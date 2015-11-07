

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

        public void CopyTo(SensorMapping sensorMapping)
        {
            sensorMapping.Id = Id;
            sensorMapping.PlantsAreaId = PlantsAreaId;
            sensorMapping.MeasuringTimeout = MeasuringTimeout;
            sensorMapping.MeasurableParameterId = MeasurableParameterId;
            sensorMapping.Type = Type;
            sensorMapping.NumberOfTimes = NumberOfTimes;
        }
    }
}