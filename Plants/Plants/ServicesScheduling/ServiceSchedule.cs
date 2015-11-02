using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants.ServiceStates;

namespace PlantingLib.Plants.ServicesScheduling
{
    public class ServiceSchedule
    {
        public Guid Id { get; private set; }

        public Guid PlantsAreaId { get; private set; }
        public ServiceStateEnum ServiceState { get; private set; }
        public TimeSpan ServicingSpan { get; set; }
        public TimeSpan ServicingPauseSpan { get; set; }
        public DateTime LastServicingTime { get; set; }

        public IList<MeasurableParameter> MeasurableParameters { get; private set; }

        public ServiceSchedule(Guid id, Guid plantsAreaId, ServiceStateEnum serviceState, TimeSpan servicingSpan,
            TimeSpan servicingPauseSpan, IList<MeasurableParameter> measurableParameters)
        {
            Id = id;
            PlantsAreaId = plantsAreaId;
            ServiceState = serviceState;
            ServicingSpan = servicingSpan;
            ServicingPauseSpan = servicingPauseSpan;
            MeasurableParameters = measurableParameters;
            LastServicingTime = DateTime.Now;
        }

        public ServiceSchedule(Guid plantsAreaId, ServiceStateEnum serviceState, TimeSpan servicingSpan,
            TimeSpan servicingPauseSpan, IList<MeasurableParameter> measurableParameters)
        {
            Id = Guid.NewGuid();
            PlantsAreaId = plantsAreaId;
            ServiceState = serviceState;
            ServicingSpan = servicingSpan;
            ServicingPauseSpan = servicingPauseSpan;
            MeasurableParameters = measurableParameters;
            LastServicingTime = DateTime.Now;
        }
    }
}
