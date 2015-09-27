using System;
using PlantingLib.MeasurableParameters;
using PlantingLib.ParametersFunctions;
using PlantingLib.Plants;

namespace PlantingLib.Sensors
{
    public abstract class Sensor
    {
        public Guid Id { get; private set; }
        public TimeSpan MeasuringTimeout { get; private set; }
        public PlantsArea PlantsArea { get; private set; }
        public MeasurableParameter MeasurableParameter { get; private set; }
        public ParameterFunction Function { get; set; }
        public bool IsOn { get; set; }
        public MeasurableTypeEnum MeasurableType { get; private set; }
        
        protected Sensor(PlantsArea plantsArea,
            TimeSpan measuringTimeout, MeasurableParameter measurableParameter)
        {
            Id = Guid.NewGuid();
            PlantsArea = plantsArea;
            plantsArea.AddSensor(this);
            MeasuringTimeout = measuringTimeout;
            MeasurableParameter = measurableParameter;
            MeasurableType = MeasurableParameter.MeasurableType;
            IsOn = true;
        }

        protected Sensor(Guid id, PlantsArea plantsArea,
            TimeSpan measuringTimeout, MeasurableParameter measurableParameter)
        {
            Id = id;
            PlantsArea = plantsArea;
            plantsArea.AddSensor(this);
            MeasuringTimeout = measuringTimeout;
            MeasurableParameter = measurableParameter;
            MeasurableType = MeasurableParameter.MeasurableType;
            IsOn = true;
        }
        public double GetNewMeasuring
        {
            get { return Function.NewFunctionValue(); }
        }
    }
}
