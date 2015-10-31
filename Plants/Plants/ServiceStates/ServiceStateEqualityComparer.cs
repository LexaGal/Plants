using System.Collections.Generic;

namespace PlantingLib.Plants.ServiceStates
{
    public class ServiceStateEqualityComparer : IEqualityComparer<ServiceState>
    {
        public bool Equals(ServiceState ss1, ServiceState ss2)
        {
            if (ss1.ServiceName == ss2.ServiceName)
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(ServiceState pa)
        {
            return typeof(ServiceState).GetHashCode();
        }
    }
}