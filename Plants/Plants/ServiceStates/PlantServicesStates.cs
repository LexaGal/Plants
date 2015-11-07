using System.ComponentModel;
using System.Linq;

namespace PlantingLib.Plants.ServiceStates
{
    public class PlantServicesStates
    {
        public BindingList<ServiceState> ServicesStates { get; private set; }

        public PlantServicesStates()
        {
            ServicesStates = new BindingList<ServiceState>
            {
                AllowNew = true,
                AllowEdit = true,
                AllowRemove = true
            };
        }

        public bool AddServiceState(ServiceState serviceState)
        {
            if (ServicesStates == null)
            {
                ServicesStates = new BindingList<ServiceState>();
            }
            if (ServicesStates.Any(s => s.ServiceName == serviceState.ServiceName))
            {
                return false;
            }
            ServicesStates.Add(serviceState);
            return true;
        }

        public bool RemoveServiceState(ServiceState serviceState)
        {
            if (ServicesStates != null)
            {
                if (ServicesStates.All(s => s.ServiceName != serviceState.ServiceName))
                {
                    return false;
                }
                return ServicesStates.Remove(serviceState);
            }
            return false;
        }
    }
}