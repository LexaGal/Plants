using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planting.Plants;
using Planting.PlantsRequirements;

namespace Planting.ServiceSystems
{
    public class ServiceMessage
    {
        public ServiceMessage(string plantsAreaId, MeasurableTypesEnum measurableType, double parameterValue, TimeSpan timeSpan)
        {
            PlantsAreaId = plantsAreaId;
            MeasurableType = measurableType;
            ParameterValue = parameterValue;
            TimeSpan = timeSpan;
        }

        public string PlantsAreaId { get; private set; }
        public MeasurableTypesEnum MeasurableType { get; private set; }
        public double ParameterValue { get; private set; }
        public TimeSpan TimeSpan { get; private set; }

        public override string ToString()
        {
            return String.Format("{0} at {1} plants area was set to {2} during {3}.", MeasurableType, PlantsAreaId, ParameterValue, TimeSpan);
        }
    }
}
