using System;
using System.ComponentModel.DataAnnotations;

namespace Database.MappingTypes
{
    public class PlantMapping
    {
        public PlantMapping()
        {
        }

        public PlantMapping(Guid id, Guid temperatureId, Guid humidityId, Guid soilPhId, Guid nutrientId,
            string name, string customParametersIds)
        {
            Id = id;
            TemperatureId = temperatureId;
            HumidityId = humidityId;
            SoilPhId = soilPhId;
            NutrientId = nutrientId;
            Name = name;
            CustomParametersIds = customParametersIds;
        }

        [Key]
        public Guid Id { get; set; }

        public Guid TemperatureId { get; set; }
        public Guid HumidityId { get; set; }
        public Guid SoilPhId { get; set; }
        public Guid NutrientId { get; set; }
        public string Name { get; set; }
        public string CustomParametersIds { get; set; }

        public void CopyTo(PlantMapping plantMapping)
        {
            plantMapping.Id = Id;
            plantMapping.TemperatureId = TemperatureId;
            plantMapping.HumidityId = HumidityId;
            plantMapping.SoilPhId = SoilPhId;
            plantMapping.NutrientId = NutrientId;
            plantMapping.Name = Name;
            plantMapping.CustomParametersIds = CustomParametersIds;
        }
    }
}