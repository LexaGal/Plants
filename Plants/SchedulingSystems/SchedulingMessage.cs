using System;
using System.Collections.Generic;
using Planting.PlantRequirements;
using Planting.Plants;

namespace Planting.SchedulingSystems
{
    public class SchedulingMessage
    {
        public SchedulingMessage(DateTime dateTime, string plantsAreaId,
            IDictionary<MeasurableTypesEnum, IEnumerable<int>> measuringsDictionary)
        {
            DateTime = dateTime;
            PlantsAreaId = plantsAreaId;
            MeasuringsDictionary = measuringsDictionary;
        }

        public DateTime DateTime { get; private set; }
        public string PlantsAreaId { get; private set; }
        public IDictionary<MeasurableTypesEnum, IEnumerable<int>> MeasuringsDictionary { get; private set; }
    }
}