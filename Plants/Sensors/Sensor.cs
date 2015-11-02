using System;
using PlantingLib.MeasurableParameters;
using PlantingLib.Messenging;
using PlantingLib.ParametersFunctions;
using PlantingLib.Plants;

namespace PlantingLib.Sensors
{
    public abstract class Sensor
    {
        public Guid Id { get; private set; }
        public TimeSpan MeasuringTimeout { get; set; }
        public PlantsArea PlantsArea { get; private set; }
        public MeasurableParameter MeasurableParameter { get; private set; }
        public ParameterFunction Function { get; set; }
        public bool IsOn { get; set; }
        public string MeasurableType { get; private set; }
        public int NumberOfTimes { get; set; }
        public bool IsCustom { get; set; }

        protected Sensor(PlantsArea plantsArea, TimeSpan measuringTimeout, MeasurableParameter measurableParameter,
            int numberOfTimes)
        {
            Id = Guid.NewGuid();
            PlantsArea = plantsArea;
            if (plantsArea != null)
            {
                plantsArea.AddSensor(this);
            }
            MeasuringTimeout = measuringTimeout;
            MeasurableParameter = measurableParameter;
            MeasurableType = MeasurableParameter.MeasurableType;
            IsOn = true;
            IsCustom = this is CustomSensor;
            NumberOfTimes = numberOfTimes;
        }

        protected Sensor(Guid id, PlantsArea plantsArea, TimeSpan measuringTimeout, MeasurableParameter measurableParameter, int numberOfTimes)
        {
            Id = id;
            PlantsArea = plantsArea;
            if (plantsArea != null)
            {
                plantsArea.AddSensor(this);
            }
            MeasuringTimeout = measuringTimeout;
            MeasurableParameter = measurableParameter;
            MeasurableType = MeasurableParameter.MeasurableType;
            IsOn = true;
            IsCustom = this is CustomSensor;
            NumberOfTimes = numberOfTimes;
        }

        public void SetMeasuringTimeout(TimeSpan timeSpan)
        {
            MeasuringTimeout = timeSpan;
        }

        public void SetPlantsArea(PlantsArea area)
        {
            PlantsArea = area;
        }

        public double GetNewMeasuring
        {
            get
            {
                Function.NewFunctionValue();
                OnNewMeasuring();
                return Function.CurrentFunctionValue;
            }
        }

        public event EventHandler NewMeasuring;
       
        protected virtual void OnNewMeasuring()
        {
            EventHandler handler = NewMeasuring;
            if (handler != null)
            {
                handler(this, new MessengingEventArgs<Sensor>(this));
            }
        }
    }
}
