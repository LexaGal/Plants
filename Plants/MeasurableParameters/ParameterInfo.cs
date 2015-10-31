using System.Collections.Generic;
using PlantingLib.Plants.ServiceStates;

namespace PlantingLib.MeasurableParameters
{
    public class ParameterInfo
    {
        public string MeasurableType { get; private set; }
        public IEnumerable<ServiceState> ServiceStates { get; private set; }

        public ParameterInfo(string measurableType, IEnumerable<ServiceState> serviceStates)
        {
            MeasurableType = measurableType;
            ServiceStates = serviceStates;
        }
    }
}