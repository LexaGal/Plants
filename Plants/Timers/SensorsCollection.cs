using System.Collections.Generic;
using System.Linq;
using PlantingLib.Sensors;

namespace PlantingLib.Timers
{
    public class SensorsCollection
    {
        public List<Sensor> Sensors { get; private set; }

        public SensorsCollection()
        {
            Sensors = new List<Sensor>();
         }

        public SensorsCollection(List<Sensor> sensors)
        {
            Sensors = sensors;
        }

        public bool AddSensor(Sensor sensor)
        {
            if (Sensors == null)
            {
                Sensors = new List<Sensor>();
            }
            if (Sensors.Any(s => s != null && s.Id == sensor.Id))
            {
                Sensor old = Sensors.First(s => s.Id == sensor.Id);
                old = sensor;
                return true;
            } 
            Sensors.Add(sensor);
            return true;
        }

        public bool RemoveSensor(Sensor sensor)
        {
            if (Sensors != null)
            {
                if (Sensors.All(s => s.Id != sensor.Id))
                {
                    return false;
                }
                return Sensors.Remove(sensor);
            }
            return false;
        }
    }
}
