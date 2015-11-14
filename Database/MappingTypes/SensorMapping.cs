

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

        public SensorMapping()
        {
        }

        public SensorMapping(Guid id, Guid plantsAreaId, int measuringTimeout, Guid measurableParameterId, string type)
        {
            Id = id;
            PlantsAreaId = plantsAreaId;
            MeasuringTimeout = measuringTimeout;
            MeasurableParameterId = measurableParameterId;
            Type = type;
        }

        public void CopyTo(SensorMapping sensorMapping)
        {
            sensorMapping.Id = Id;
            sensorMapping.PlantsAreaId = PlantsAreaId;
            sensorMapping.MeasuringTimeout = MeasuringTimeout;
            sensorMapping.MeasurableParameterId = MeasurableParameterId;
            sensorMapping.Type = Type;
        }
    }
}