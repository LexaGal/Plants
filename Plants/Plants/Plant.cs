using System;
using System.Collections.Generic;
using System.Linq;
using PlantingLib.MeasurableParameters;

namespace PlantingLib.Plants
{
    public class Plant
    {
        public Plant(Temperature temperature, Humidity humidity, SoilPh soilPh,
            Nutrient nutrient, PlantNameEnum name)
        {
            Id = Guid.NewGuid();
            Temperature = temperature;
            Humidity = humidity;
            SoilPh = soilPh;
            Nutrient = nutrient;
            Name = name;

            MeasurableParameters = new List<MeasurableParameter>
            {
                Temperature,
                Humidity,
                SoilPh,
                Nutrient
            };
            CustomParameters = new List<CustomParameter>();
        }

        public Plant(Guid id, Temperature temperature, Humidity humidity, SoilPh soilPh,
            Nutrient nutrient, PlantNameEnum name)
        {
            Id = id;
            Temperature = temperature;
            Humidity = humidity;
            SoilPh = soilPh;
            Nutrient = nutrient;
            Name = name;

            MeasurableParameters = new List<MeasurableParameter>
            {
                Temperature,
                Humidity,
                SoilPh,
                Nutrient
            };
            CustomParameters = new List<CustomParameter>(); 
        }

        public MeasurableParameter GetMeasurableParameter(string type)
        {
            return MeasurableParameters.First(mp => mp.MeasurableType == type);
        }

        public void AddCustomParameters(List<CustomParameter> customParameters)
        {
            CustomParameters.AddRange(customParameters);
            MeasurableParameters.AddRange(customParameters);
        }

        public void AddCustomParameter(CustomParameter customParameters)
        {
            CustomParameters.Add(customParameters);
            MeasurableParameters.Add(customParameters);
        }

        public bool RemoveCustomParameter(CustomParameter customParameter)
        {
            //!!! not .Remove(item)
            CustomParameters.RemoveAll(cp => cp.Id == customParameter.Id);
            MeasurableParameters.RemoveAll(cp => cp.Id == customParameter.Id);
            return true;
        }

        public Guid Id { get; private set; }
        public PlantNameEnum Name { get; private set; }
        public Temperature Temperature { get; private set; }
        public Humidity Humidity { get; private set; }
        public SoilPh SoilPh { get; private set; }
        public Nutrient Nutrient { get; private set; }
        public List<MeasurableParameter> MeasurableParameters { get; private set; }
        public List<CustomParameter> CustomParameters { get; private set; }

    }
}
