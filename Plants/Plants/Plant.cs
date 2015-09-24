using System;
using System.Collections.Generic;
using System.Linq;
using PlantingLib.MeasurableParameters;

namespace PlantingLib.Plants
{
    public class Plant
    {
        public Plant(Temperature temperature, Humidity humidity, SoilPh soilPh,
            Nutrient nutrient, TimeSpan growingTime, TimeSpan wateringSpan, TimeSpan nutrientingSpan)
        {
            Id = Guid.NewGuid();
            Temperature = temperature;
            Humidity = humidity;
            SoilPh = soilPh;
            Nutrient = nutrient;
            GrowingTime = growingTime;
            WateringSpan = wateringSpan;
            NutrientingSpan = nutrientingSpan;

            MeasurableParameters = new List<MeasurableParameter>
            {
                Temperature,
                Humidity,
                SoilPh,
                Nutrient
            };
        }

        public Plant(Guid id, Temperature temperature, Humidity humidity, SoilPh soilPh,
            Nutrient nutrient, TimeSpan growingTime, TimeSpan wateringSpan, TimeSpan nutrientingSpan)
        {
            Id = id;
            Temperature = temperature;
            Humidity = humidity;
            SoilPh = soilPh;
            Nutrient = nutrient;
            GrowingTime = growingTime;
            WateringSpan = wateringSpan;
            NutrientingSpan = nutrientingSpan;

            MeasurableParameters = new List<MeasurableParameter>
            {
                Temperature,
                Humidity,
                SoilPh,
                Nutrient
            };
        }

        public MeasurableParameter GetMeasurableParameter(MeasurableTypesEnum type)
        {
            return MeasurableParameters.First(mp => mp.MeasurableType == type);
        }

        public Guid Id { get; private set; }
        public Temperature Temperature { get; private set; }
        public Humidity Humidity { get; private set; }
        public SoilPh SoilPh { get; private set; }
        public Nutrient Nutrient { get; private set; }
        public TimeSpan GrowingTime { get; private set; }
        public TimeSpan WateringSpan { get; private set; }
        public TimeSpan NutrientingSpan { get; private set; }
        public IList<MeasurableParameter> MeasurableParameters { get; private set; }
    }
}
