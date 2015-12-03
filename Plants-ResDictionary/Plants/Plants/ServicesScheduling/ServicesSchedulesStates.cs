using System;
using System.ComponentModel;
using System.Linq;

namespace PlantingLib.Plants.ServicesScheduling
{
    public class ServicesSchedulesStates
    {
        public BindingList<ServiceSchedule> ServicesSchedules { get; private set; }

        public ServicesSchedulesStates()
        {
            ServicesSchedules = new BindingList<ServiceSchedule>
            {
                AllowNew = true,
                AllowEdit = true,
                AllowRemove = true
            };
        }

        public ServiceSchedule GetServiceSchedule(Func<ServiceSchedule, bool> func)
        {
            try
            {
                return ServicesSchedules.SingleOrDefault(func);
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }

        public bool AddServiceSchedule(ServiceSchedule serviceSchedule)
        {
            if (ServicesSchedules == null)
            {
                ServicesSchedules = new BindingList<ServiceSchedule>();
            }
            ServicesSchedules.Add(serviceSchedule);
            return true;
        }

        public bool RemoveServiceState(ServiceSchedule serviceSchedule)
        {
            if (ServicesSchedules != null)
            {
                return ServicesSchedules.Remove(serviceSchedule);
            }
            return false;
        }
    }
}