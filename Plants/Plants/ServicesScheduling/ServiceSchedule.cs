using System;
using System.Collections.Generic;
using System.Linq;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants.ServiceStates;

namespace PlantingLib.Plants.ServicesScheduling
{
    public class ServiceSchedule
    {
        private string _serviceName;
        public Guid Id { get; private set; }

        public Guid PlantsAreaId { get; private set; }

        public string ServiceName
        {
            get { return _serviceName; }
            set
            {
                _serviceName = value;
                OnNewServiceName();
            }
        }

        public TimeSpan ServicingSpan { get; set; }
        public TimeSpan ServicingPauseSpan { get; set; }
        public DateTime LastServicingTime { get; set; }

        public bool IsOn { get; set; }

        public List<MeasurableParameter> MeasurableParameters { get; set; }

        public ServiceSchedule(Guid id, Guid plantsAreaId, string serviceName, TimeSpan servicingSpan,

            TimeSpan servicingPauseSpan, List<MeasurableParameter> measurableParameters)
        {
            Id = id;
            PlantsAreaId = plantsAreaId;
            ServiceName = serviceName;
            ServicingSpan = servicingSpan;
            ServicingPauseSpan = servicingPauseSpan;
            LastServicingTime = DateTime.Now;

            IsOn = true;

            MeasurableParameters = measurableParameters;
        }

        public bool IsFor(string serviceName)
        {
            return ServiceName == serviceName;
        }

        public bool AddMeasurableParameter(MeasurableParameter measurableParameter)
        {
            if (MeasurableParameters == null)
            {
                MeasurableParameters = new List<MeasurableParameter>();
            }
            if (MeasurableParameters.Any(parameter => measurableParameter != null && parameter.Id == measurableParameter.Id))
            {
                MeasurableParameter old = MeasurableParameters.First(mp => mp.Id == measurableParameter.Id);
                old = measurableParameter;
                return true;
            }
            MeasurableParameters.Add(measurableParameter);
            return true;
        }
        public event EventHandler NewServiceName;

        protected virtual void OnNewServiceName()
        {
            EventHandler handler = NewServiceName;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
