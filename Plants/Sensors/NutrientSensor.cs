﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using PlantingLib.MeasurableParameters;
using PlantingLib.ParametersFunctions;
using PlantingLib.Plants;

namespace PlantingLib.Sensors
{
    public class NutrientSensor : Sensor
    {
        public NutrientSensor( PlantsArea plantsArea, TimeSpan measuringTimeout,
            Nutrient nutrient)
            : base(plantsArea, measuringTimeout, nutrient)
        {
            Function = new NutrientFunction(nutrient);
        }

        public NutrientSensor(Guid id, PlantsArea plantsArea, TimeSpan measuringTimeout, Nutrient nutrient) 
            : base(id, plantsArea, measuringTimeout, nutrient)
        {
            Function = new NutrientFunction(nutrient);
        }
    }
}