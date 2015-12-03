using System;
using PlantingLib.MeasurableParameters;
using PlantingLib.ParametersFunctions;
using PlantingLib.Plants;

namespace PlantingLib.Sensors
{
    public abstract class Sensor
    {
        public Guid Id { get; private set; }
        public TimeSpan MeasuringTimeout { get; set; }
        
        public PlantsArea PlantsArea { get; private set; }
        public MeasurableParameter MeasurableParameter { get; set; }
        public ParameterFunction Function { get; protected set; }

        public string MeasurableType => MeasurableParameter.MeasurableType;

        public int NumberOfTimes { get; set; }
    
        public bool IsOn { get; set; }
        public bool IsOffByUser { get; set; }
        public bool IsCustom { get; set; }

        protected Sensor(Guid id, PlantsArea plantsArea, TimeSpan measuringTimeout,
             MeasurableParameter measurableParameter, int numberOfTimes = 0)
        {
            Id = id;

            PlantsArea = plantsArea;
            plantsArea?.AddSensor(this);

            MeasuringTimeout = measuringTimeout;
            MeasurableParameter = measurableParameter;

            NumberOfTimes = numberOfTimes;
            IsOn = true;
            IsOffByUser = false;
        }
        
        public void SetPlantsArea(PlantsArea area)
        {
            PlantsArea = area;
        }

        public void SetFunction(ParameterFunction function)
        {
            Function = function;
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
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}
