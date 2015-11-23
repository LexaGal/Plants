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

        public Temperature Temperature { get; }
        public Humidity Humidity { get; }
        public SoilPh SoilPh { get; }
        public Nutrient Nutrient { get; }

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

        public bool AddMeasurableParameters(List<MeasurableParameter> measurableParameters)
        {
            if (MeasurableParameters == null)
            {
                MeasurableParameters = new List<MeasurableParameter>();
            }
            if (measurableParameters != null)
            {
                MeasurableParameters.AddRange(measurableParameters);
            }
            return true;
        }

        public bool AddMeasurableParameter(MeasurableParameter measurableParameter)
        {
            if (MeasurableParameters == null)
            {
                MeasurableParameters = new List<MeasurableParameter>();
            }
            if (MeasurableParameters.Any(c => measurableParameter != null && c.Id == measurableParameter.Id))
            {
                MeasurableParameter mp = MeasurableParameters.First(c => c.Id == measurableParameter.Id);
                mp = measurableParameter;
                return true;
            }
            MeasurableParameters.Add(measurableParameter);
            return true;
        }

        public bool RemoveMeasurableParameter(MeasurableParameter measurableParameter)
        {
            //!!! not .Remove(item)
            if (MeasurableParameters == null)
            {
                return false;
            }
            MeasurableParameters.RemoveAll(cp => measurableParameter != null && cp.Id == measurableParameter.Id);
            return true;
        }
    }
}
