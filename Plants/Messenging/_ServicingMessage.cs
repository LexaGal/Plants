using System;
using System.Collections.Generic;
using Planting.PlantsRequirements;

namespace Planting.Messenging
{
    public class ServicingMessage
    {
        public ServicingMessage(DateTime dateTime, string plantsAreaId,
            KeyValuePair<MeasurableTypesEnum, double> measuring)
        {
            DateTime = dateTime;
            PlantsAreaId = plantsAreaId;
            MeasuringsDictionary = measuring;
        }

        public DateTime DateTime { get; private set; }
        public string PlantsAreaId { get; private set; }
        public KeyValuePair<MeasurableTypesEnum, double> MeasuringsDictionary { get; private set; }
    }
}