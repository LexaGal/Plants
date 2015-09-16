using System;
using PlantingLib.MappingTypes;
using PlantingLib.ParametersFunctions;
using PlantingLib.Plants;
using PlantingLib.PlantsRequirements;

namespace PlantingLib.Sensors
{
    public abstract class Sensor
    {
        public Guid Id { get; private set; }
        public TimeSpan MeasuringTimeout { get; private set; }
        public PlantsArea PlantsArea { get; private set; }
        public MeasurableParameter MeasurableParameter { get; private set; }
        public Tuple<int, int> Location { get; private set; }
        public ParameterFunction Function { get; set; }
        public bool IsOn { get; set; }

        protected Sensor()
        {
        }

        protected Sensor(Tuple<int, int> location, PlantsArea plantsArea,
            TimeSpan measuringTimeout, MeasurableParameter measurableParameter)
        {
            Id = Guid.NewGuid();
            Location = location;
            PlantsArea = plantsArea;
            plantsArea.AddSensor(this);
            MeasuringTimeout = measuringTimeout;
            MeasurableParameter = measurableParameter;
            IsOn = true;
        }

        public double GetCurrentMeasuring
        {
            get { return Function.NewFunctionValue(); }
        }

        public SensorMapping GetMapping
        {
            get
            {
                return new SensorMapping(Id, PlantsArea.Id, (int) MeasuringTimeout.TotalSeconds, MeasurableParameter.Id);
            }
        }
    }
}
