using System.Collections.Generic;
using System.Linq;

namespace PlantingLib.Sensors
{
    public class SensorsCollection
    {
        public IList<Sensor> AllSensors { get; private set; }

        public SensorsCollection()
        {
            AllSensors = new List<Sensor>();
         }

        public SensorsCollection(IList<Sensor> sensors)
        {
            AllSensors = sensors;
        }

        public bool AddSensor(Sensor sensor)
        {
            if (AllSensors == null)
            {
                AllSensors = new List<Sensor>();
            }
            if (AllSensors.Any(s => s.Id == sensor.Id))
            {
                return false;
            } 
            AllSensors.Add(sensor);
            return true;
        }

        public bool RemoveSensor(Sensor sensor)
        {
            if (AllSensors.All(s => s.Id != sensor.Id))
            {
                return false;
            }
            AllSensors.Remove(sensor);
            return true;
        }
    }
}
