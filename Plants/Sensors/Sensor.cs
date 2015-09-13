using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planting.ParametersFunctions;
using Planting.Plants;
using Planting.PlantsRequirements;
using Planting.Timers;

namespace Planting.Sensors
{
    public abstract class Sensor
    {
        public string Id { get; private set; }
        public ParameterFunction Function { get; set; }

        protected Sensor(Tuple<int, int> location, PlantsArea plantsArea, 
            TimeSpan measuringTimeout, MeasurableParameter measurableParameter)
        {
            Id = Guid.NewGuid().ToString().Substring(0, 8);
            Location = location;
            PlantsArea = plantsArea;
            MeasuringTimeout = measuringTimeout;
            MeasurableParameter = measurableParameter;
        }

        public double GetCurrentMeasuring
        {
            get { return Function.NewFunctionValue(); }
        }

        public Tuple<int, int> Location { get; private set; }
        public PlantsArea PlantsArea { get; private set; }
        public TimeSpan MeasuringTimeout { get; private set; }
        public MeasurableParameter MeasurableParameter { get; private set; }
    }
}
