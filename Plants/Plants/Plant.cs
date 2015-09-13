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
        public Plant(Temperature temperature, Humidity humidity,
            SoilPh soilPh, Nutrient nutrient, DateTime growingTime)
        {
            Temperature = temperature;
            Humidity = humidity;
            SoilPh = soilPh;
            Nutrient = nutrient;
            GrowingTimeLimit = growingTime;
        }

        public Temperature Temperature { get; private set; }
        public Humidity Humidity { get; private set; }
        public SoilPh SoilPh { get; private set; }
        public Nutrient Nutrient { get; private set; }
        public DateTime GrowingTimeLimit { get; private set; }
    }
}
