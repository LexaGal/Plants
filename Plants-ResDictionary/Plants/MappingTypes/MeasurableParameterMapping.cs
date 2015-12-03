using System;
using System.ComponentModel.DataAnnotations;

namespace PlantingLib.MappingTypes
{
    public class MeasurableParameterMapping
    {
        [Key]
        public Guid Id { get; set; }

        public int Optimal { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public string Type { get; set; }

        public MeasurableParameterMapping(Guid id, int optimal, int min, int max, string type)
        {
            Id = id;
            Optimal = optimal;
            Min = min;
            Max = max;
            Type = type;
        }

        public MeasurableParameterMapping()
        {}
    }
}
