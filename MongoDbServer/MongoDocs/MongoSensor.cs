using System;
using PlantingLib.Sensors;

namespace MongoDbServer.MongoDocs
{
    public class MongoSensor
    {
        public string objId { get; set; }

        public string plantsareaId { get; set; }

        public TimeSpan measuringTimeout { get; set; }
        
        public string measurableType { get; set; }

        public int numberOfMessages { get; set; }

        public bool isOn { get; set; }

        public bool isCustom { get; set; }

        public DateTime dateTime { get; set; }

        public MongoSensor(Sensor sensor, string areaId)
        {
            objId = sensor.Id.ToString();
            plantsareaId = areaId;
            measurableType = sensor.MeasurableType;
            measuringTimeout = sensor.MeasuringTimeout;
            numberOfMessages = sensor.NumberOfTimes;
            isOn = sensor.IsOn;
            dateTime = DateTime.Now;
            isCustom = sensor.IsCustom;
        }
    }
}