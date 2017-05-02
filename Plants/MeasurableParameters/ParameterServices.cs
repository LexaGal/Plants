using System.Collections.Generic;
using PlantingLib.Plants.ServiceStates;

namespace PlantingLib.MeasurableParameters
{
    public class ParameterServices
    {
        public ParameterServices(string measurableType, IEnumerable<ServiceState> serviceStates)
        {
            MeasurableType = measurableType;
            ServiceStates = serviceStates;
        }

        public string MeasurableType { get; private set; }
        public IEnumerable<ServiceState> ServiceStates { get; private set; }
    }
}