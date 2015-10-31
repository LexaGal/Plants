using System;
using System.Collections.Generic;
using System.ComponentModel;
using PlantingLib.Messenging;

namespace PlantingLib.Plants.ServiceStates
{
    public class PlantsAreaServiceState
    {
        public BindingList<ServiceState> ServiceStates { get; private set; }

        public PlantsAreaServiceState()
        {
            ServiceStates = new BindingList<ServiceState>
            {
                AllowNew = true,
                AllowEdit = true,
                AllowRemove = true
            };
        }

        public void AddServiceState(ServiceState serviceState)
        {
            ServiceStates.Add(serviceState);
        }

        public bool RemoveServiceState(ServiceState serviceState)
        {
            if (ServiceStates != null)
            {
                return ServiceStates.Remove(serviceState);
            }
            return false;
        }
    }
}