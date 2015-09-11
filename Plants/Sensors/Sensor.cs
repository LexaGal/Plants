using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Planting.PlantRequirements;
using Planting.Plants;

namespace Planting.Sensors
{
    public class Sensor
    {
        public string Id { get; private set; }
        
        public Sensor(Tuple<int, int> location, PlantsArea plantsArea, 
            TimeSpan measuringTimeout, MeasurableParameter measurableParameter)
        {
            Id = Guid.NewGuid().ToString().Substring(0, 8);
            Location = location;
            PlantsArea = plantsArea;
            MeasuringTimeout = measuringTimeout;
            MeasurableParameter = measurableParameter;
        }

        public int CurrentMeasuring
        {
            get { return MeasurableParameter.RandomValue; }
        }

        public Tuple<int, int> Location { get; private set; }
        public PlantsArea PlantsArea { get; private set; }
        public TimeSpan MeasuringTimeout { get; private set; }
        public MeasurableParameter MeasurableParameter { get; private set; }
    }
}
