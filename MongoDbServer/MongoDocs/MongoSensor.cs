using System;
using PlantingLib.Sensors;

namespace MongoDbServer.MongoDocs
{
    public class MongoSensor
    {
        public MongoSensor(Sensor sensor)
        {
            objId = sensor.Id.ToString();
            plantsareaId = sensor.PlantsAreaId.ToString();
            measurableType = sensor.MeasurableType;
            measuringTimeout = sensor.MeasuringTimeout;
            isOn = sensor.IsOn.ToString();
            dateTime = DateTime.Now;
            isCustom = sensor.IsCustom.ToString();
        }

        public string objId { get; set; }

        public string plantsareaId { get; set; }

        public TimeSpan measuringTimeout { get; set; }

        public string measurableType { get; set; }

        public string isOn { get; set; }

        public string isCustom { get; set; }

        public DateTime dateTime { get; set; }
    }
}