using System;
using System.ComponentModel.DataAnnotations;

namespace Database.MappingTypes
{
    public class ServiceScheduleMapping
    {
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

        [Key]
        public Guid Id { get; set; }

        public Guid PlantsAreaId { get; set; }
        public string ServiceState { get; set; }
        public int ServicingSpan { get; set; }
        public int ServicingPauseSpan { get; set; }

        public string MeasurableParametersIds { get; set; }

        public void CopyTo(ServiceScheduleMapping serviceScheduleMapping)
        {
            serviceScheduleMapping.Id = Id;
            serviceScheduleMapping.PlantsAreaId = PlantsAreaId;
            serviceScheduleMapping.ServiceState = ServiceState;
            serviceScheduleMapping.ServicingSpan = ServicingSpan;
            serviceScheduleMapping.ServicingPauseSpan = ServicingPauseSpan;
            serviceScheduleMapping.MeasurableParametersIds = MeasurableParametersIds;
        }
    }
}