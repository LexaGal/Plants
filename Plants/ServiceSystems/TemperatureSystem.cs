﻿using System;
using PlantingLib.Plants;
using PlantingLib.PlantsRequirements;

namespace PlantingLib.ServiceSystems
{
    public class TemperatureSystem : ServiceSystem
    {
        public TemperatureSystem(MeasurableTypesEnum measurableType, double parameterValue, PlantsArea plantsArea)
            : base(measurableType, parameterValue, plantsArea)
        {
        }

        public override TimeSpan ComputeTimeForService()
        {
            if (PlantsArea != null)
            {
                TimeSpan timeSpan = new TimeSpan(0, 0, (int) (Math.Abs(ParameterValue -
                    PlantsArea.Plant.Temperature.Optimal)));
                return timeSpan;
            }
            return TimeSpan.Zero;
        }
     };

}