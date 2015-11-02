using System.ComponentModel;

namespace PlantingLib.Plants.ServicesScheduling
{
    public class ServicesSchedulesState
    {
        public BindingList<ServiceSchedule> ServicesSchedules { get; private set; }

        public ServicesSchedulesState()
        {
            ServicesSchedules = new BindingList<ServiceSchedule>
            {
                AllowNew = true,
                AllowEdit = true,
                AllowRemove = true
            };
        }

        public void AddServiceSchedule(ServiceSchedule serviceSchedule)
        {
            ServicesSchedules.Add(serviceSchedule);
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