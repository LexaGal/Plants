using System.Collections.Generic;

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

        public void AddSensor(Sensor sensor)
        {
            if (AllSensors == null)
            {
                AllSensors = new List<Sensor>();
            }
            AllSensors.Add(sensor);
        }
    }
}
