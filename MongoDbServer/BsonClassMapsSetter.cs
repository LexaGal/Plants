using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using PlantingLib.Sensors;

namespace MongoDbServer
{
    public static class BsonClassMapsSetter
    {
        public static BsonClassMap<MongoPlantsArea> SetMongoPlantsArea()
        {
            return BsonClassMap.RegisterClassMap<MongoPlantsArea>(a =>
            {
                a.AutoMap();
                a.MapIdMember(area => area.ObjId); //.SetIdGenerator(ObjectIdGenerator.Instance);
            });
        }

        public static BsonClassMap<MongoSensor> SetMongoSensorMap()
        {
            return BsonClassMap.RegisterClassMap<MongoSensor>(s =>
            {
                s.AutoMap();
                s.MapIdMember(sensor => sensor.ObjId);//.SetIdGenerator(ObjectIdGenerator.Instance);
            });
        }
    }
}