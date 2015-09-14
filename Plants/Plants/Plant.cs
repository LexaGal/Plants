using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planting.PlantsRequirements;

namespace Planting.Plants
{
    public class Plant
    {
        public Plant(Temperature temperature, Humidity humidity, SoilPh soilPh,
            Nutrient nutrient, DateTime growingTime, TimeSpan wateringSpan, TimeSpan nutrientingSpan)
        {
            Temperature = temperature;
            Humidity = humidity;
            SoilPh = soilPh;
            Nutrient = nutrient;
            GrowingTimeLimit = growingTime;
            WateringSpan = wateringSpan;
            NutrientingSpan = nutrientingSpan;
        }

        public Temperature Temperature { get; private set; }
        public Humidity Humidity { get; private set; }
        public SoilPh SoilPh { get; private set; }
        public Nutrient Nutrient { get; private set; }
        public DateTime GrowingTimeLimit { get; private set; }
        public TimeSpan WateringSpan { get; private set; }
        public TimeSpan NutrientingSpan { get; private set; }
    }
}
