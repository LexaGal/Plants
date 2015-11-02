using System;
using System.ComponentModel.DataAnnotations;

namespace Database.MappingTypes
{
    public class ServiceScheduleMapping
    {
        [Key]
        public Guid Id { get; private set; }

        public Guid PlantsAreaId { get; private set; }
        public string ServiceState { get; private set; }
        public int ServicingSpan { get; private set; }
        public int ServicingPauseSpan { get; private set; }

        public string MeasurableParametersIds { get; private set; }

        public ServiceScheduleMapping()
        {
        }

        public ServiceScheduleMapping(Guid id, Guid plantsAreaId, string serviceState, int servicingSpan,
            int servicingPauseSpan, string measurableParametersIds)
        {
            Id = id;
            PlantsAreaId = plantsAreaId;
            ServiceState = serviceState;
            ServicingSpan = servicingSpan;
            ServicingPauseSpan = servicingPauseSpan;
            MeasurableParametersIds = measurableParametersIds;
        }

        public void CopyTo(ServiceScheduleMapping ssm)
        {
            ssm.Id = Id;
            ssm.PlantsAreaId = PlantsAreaId;
            ssm.ServiceState = ServiceState;
            ssm.ServicingSpan = ServicingSpan;
            ssm.ServicingPauseSpan = ServicingPauseSpan;
            ssm.MeasurableParametersIds = MeasurableParametersIds;
        
        }
    }
}