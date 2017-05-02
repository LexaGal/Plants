using System;
using System.ComponentModel.DataAnnotations;

namespace Database.MappingTypes
{
    public class MeasurableParameterMapping
    {
        public MeasurableParameterMapping(Guid id, int optimal, int min, int max, string type)
        {
            Id = id;
            Optimal = optimal;
            Min = min;
            Max = max;
            Type = type;
        }

        public MeasurableParameterMapping()
        {
        }

        [Key]
        public Guid Id { get; set; }

        public int Optimal { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public string Type { get; set; }

        public void CopyTo(MeasurableParameterMapping measurableParameterMapping)
        {
            measurableParameterMapping.Id = Id;
            measurableParameterMapping.Optimal = Optimal;
            measurableParameterMapping.Min = Min;
            measurableParameterMapping.Max = Max;
            measurableParameterMapping.Type = Type;
        }
    }
}