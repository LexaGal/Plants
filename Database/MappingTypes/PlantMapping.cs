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
        public string Name { get; set; }
        public string CustomParametersIds { get; set; }

        public PlantMapping()
        {
        }

        public PlantMapping(Guid id, Guid temperatureId, Guid humidityId, Guid soilPhId, Guid nutrientId,
            int growingTime, int wateringSpan, int nutrientingSpan, string name, string customParametersIds)
        {
            Id = id;
            TemperatureId = temperatureId;
            HumidityId = humidityId;
            SoilPhId = soilPhId;
            NutrientId = nutrientId;
            GrowingTime = growingTime;
            WateringSpan = wateringSpan;
            NutrientingSpan = nutrientingSpan;
            Name = name;
            CustomParametersIds = customParametersIds;
        }

        public void CopyTo(PlantMapping pm)
        {
            pm.Id = Id;
            pm.TemperatureId = TemperatureId;
            pm.HumidityId = HumidityId;
            pm.SoilPhId = SoilPhId;
            pm.NutrientId = NutrientId;
            pm.GrowingTime = GrowingTime;
            pm.WateringSpan = WateringSpan;
            pm.NutrientingSpan = NutrientingSpan;
            pm.Name = Name;
            pm.CustomParametersIds = CustomParametersIds;
        }
    }
}