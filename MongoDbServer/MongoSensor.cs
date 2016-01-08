using System;
using MongoDB.Bson;
using PlantingLib.Sensors;

namespace MongoDbServer
{
    public class MongoSensor
    {
        public ObjectId ObjId { get; set; }

        public ObjectId PlantsAreaObjId { get; set; }

        public TimeSpan MeasuringTimeout { get; set; }
        
        public string MeasurableType { get; set; }

        public int NumberOfMessages { get; set; }

        public bool IsOn { get; set; }

        public bool IsCustom { get; set; }

        public MongoSensor(Sensor sensor, ObjectId objectId)
        {
            ObjId = ObjectId.GenerateNewId();
            PlantsAreaObjId = objectId;
            MeasurableType = sensor.MeasurableType;
            MeasuringTimeout = sensor.MeasuringTimeout;
            NumberOfMessages = sensor.NumberOfTimes;
            IsOn = sensor.IsOn;
            IsCustom = sensor.IsCustom;
        }
    }
}