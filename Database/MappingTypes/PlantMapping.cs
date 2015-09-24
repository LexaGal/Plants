using System;
using System.ComponentModel.DataAnnotations;

namespace Database.MappingTypes
{
    public class PlantMapping
    {
        [Key]
        public Guid Id { get; set; }

        public Guid TemperatureId { get; set; }
        public Guid HumidityId { get; set; }
        public Guid SoilPhId { get; set; }
        public Guid NutrientId { get; set; }
        public int GrowingTime { get; set; }
        public int WateringSpan { get; set; }
        public int NutrientingSpan { get; set; }

        public PlantMapping()
        {
        }

        public PlantMapping(Guid id, Guid temperatureId, Guid humidityId, Guid soilPhId,
            Guid nutrientId, int growingTimeLimit, int wateringSpan, int nutrientingSpan)
        {
            Id = id;
            TemperatureId = temperatureId;
            HumidityId = humidityId;
            SoilPhId = soilPhId;
            NutrientId = nutrientId;
            GrowingTime = growingTimeLimit;
            WateringSpan = wateringSpan;
            NutrientingSpan = nutrientingSpan;
        }
    }
}