using System;
using System.ComponentModel.DataAnnotations;

namespace PlantingLib.MappingTypes
{
    public class SensorMapping
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PlantsAreaId { get; set; }
        public int MeasuringTimeout { get; set; }
        public Guid MeasurableParameterId { get; set; }

        public SensorMapping()
        {
        }

        public SensorMapping(Guid id, Guid plantsAreaId, int measuringTimeout, Guid measurableParameterId)
        {
            Id = id;
            PlantsAreaId = plantsAreaId;
            MeasuringTimeout = measuringTimeout;
            MeasurableParameterId = measurableParameterId;
        }
    }
}