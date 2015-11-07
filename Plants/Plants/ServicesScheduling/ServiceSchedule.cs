using System;
using System.Collections.Generic;
using PlantingLib.MeasurableParameters;

namespace PlantingLib.Plants.ServicesScheduling
{
    public class ServiceSchedule
    {
        public Guid Id { get; private set; }

        public Guid PlantsAreaId { get; private set; }
        public string ServiceState { get; private set; }
        public TimeSpan ServicingSpan { get; set; }
        public TimeSpan ServicingPauseSpan { get; set; }
        public DateTime LastServicingTime { get; set; }

        public IList<MeasurableParameter> MeasurableParameters { get; private set; }

        public ServiceSchedule(Guid id, Guid plantsAreaId, string serviceState, TimeSpan servicingSpan,
            TimeSpan servicingPauseSpan, IList<MeasurableParameter> measurableParameters)
        {
            Id = id;
            PlantsAreaId = plantsAreaId;
            ServiceState = serviceState;
            ServicingSpan = servicingSpan;
            ServicingPauseSpan = servicingPauseSpan;
            LastServicingTime = DateTime.Now;

            MeasurableParameters = measurableParameters;
        }

        public ServiceSchedule(Guid plantsAreaId, string serviceState, TimeSpan servicingSpan,
            TimeSpan servicingPauseSpan, IList<MeasurableParameter> measurableParameters)
        {
            Id = Guid.NewGuid();
            PlantsAreaId = plantsAreaId;
            ServiceState = serviceState;
            ServicingSpan = servicingSpan;
            ServicingPauseSpan = servicingPauseSpan;
            LastServicingTime = DateTime.Now;

            MeasurableParameters = measurableParameters;
        }
    }
}
