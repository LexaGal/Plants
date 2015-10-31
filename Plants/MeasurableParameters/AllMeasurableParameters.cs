using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace PlantingLib.MeasurableParameters
{
    public static class AllMeasurableParameters
    {
        public static Dictionary<string, List<string>> ParametersServices = new Dictionary<string, List<string>>();

        public static void SetDictionary()
        {
            ParametersServices.Add("Nutrient", new List<string> {"Nutrienting"});
            ParametersServices.Add("SoilPh", new List<string> {"Nutrienting"});
            ParametersServices.Add("Humidity", new List<string> {"Watering"});
            ParametersServices.Add("Temperature", new List<string> {"Warming", "Cooling"});
        }
    }
}