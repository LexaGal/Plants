using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace PlantingLib.MeasurableParameters
{
    public static class MeasurableParametersDictionary
    {
        public static Dictionary<string, List<string>> Dictionary = new Dictionary<string, List<string>>();

        public static void SetDictionary()
        {
            Dictionary.Add("Nutrient", new List<string> {"Nutrienting"});
            Dictionary.Add("SoilPh", new List<string> {"Nutrienting"});
            Dictionary.Add("Humidity", new List<string> {"Watering"});
            Dictionary.Add("Temperature", new List<string> {"Warming", "Cooling"});
        }
    }
}