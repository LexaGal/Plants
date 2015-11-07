using System;
using System.Collections.Generic;
using System.Linq;
using PlantingLib.MeasurableParameters;

namespace PlantingLib.Plants
{
    public class Plant
    {
        public Guid Id { get; private set; }
        public PlantNameEnum Name { get; private set; }

        public Temperature Temperature { get; private set; }
        public Humidity Humidity { get; private set; }
        public SoilPh SoilPh { get; private set; }
        public Nutrient Nutrient { get; private set; }

        public List<MeasurableParameter> MeasurableParameters { get; private set; }

        public Plant(Guid id, Temperature temperature, Humidity humidity, SoilPh soilPh,
            Nutrient nutrient, PlantNameEnum name)
        {
            Id = id;
            Name = name;

            Temperature = temperature;
            Humidity = humidity;
            SoilPh = soilPh;
            Nutrient = nutrient;

            MeasurableParameters = new List<MeasurableParameter>
            {
                Temperature,
                Humidity,
                SoilPh,
                Nutrient
            };
        }

        public MeasurableParameter GetMeasurableParameter(string type)
        {
            return MeasurableParameters.FirstOrDefault(mp => mp.MeasurableType == type);
        }

        public bool AddCustomParameters(List<CustomParameter> customParameters)
        {
            if (MeasurableParameters == null)
            {
                MeasurableParameters = new List<MeasurableParameter>();
            }
            MeasurableParameters.AddRange(customParameters);
            return true;
        }

        public bool AddCustomParameter(CustomParameter customParameter)
        {
            if (MeasurableParameters == null)
            {
                MeasurableParameters = new List<MeasurableParameter>();
            }
            if (MeasurableParameters.Any(c => c.Id == customParameter.Id))
            {
                MeasurableParameter cp = MeasurableParameters.First(c => c.Id == customParameter.Id);
                cp = customParameter;
                return true;
            }
            MeasurableParameters.Add(customParameter);
            return true;
        }

        public bool RemoveCustomParameter(CustomParameter customParameter)
        {
            //!!! not .Remove(item)
            if (MeasurableParameters == null)
            {
                return false;
            }
            MeasurableParameters.RemoveAll(cp => cp.Id == customParameter.Id);
            return true;
        }
    }
}
