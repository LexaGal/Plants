using System;
using System.ComponentModel.DataAnnotations;

namespace PlantingLib.MappingTypes
{
    public class PlantsAreaMapping
    {
        public PlantsAreaMapping(Guid id, Guid plantId, int number)
        {
            Id = id;
            PlantId = plantId;
            Number = number;
        }

        public PlantsAreaMapping()
        {
        }

        [Key]
        public Guid Id { get; set; }

        public Guid PlantId { get; set; }
        public int Number { get; set; }
    }
}