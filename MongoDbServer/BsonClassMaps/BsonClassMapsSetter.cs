using MongoDbServer.MongoDocs;
using MongoDB.Bson.Serialization;

namespace MongoDbServer.BsonClassMaps
{
    public static class BsonClassMapsSetter
    {
        public static BsonClassMap<MongoPlantsArea> SetMongoPlantsArea()
        {
            return BsonClassMap.RegisterClassMap<MongoPlantsArea>(a =>
            {
                a.AutoMap();
                a.MapIdMember(area => area.objId); 
            });
        }

        public static BsonClassMap<MongoSensor> SetMongoSensorMap()
        {
            return BsonClassMap.RegisterClassMap<MongoSensor>(s =>
            {
                s.AutoMap();
                s.MapIdMember(sensor => sensor.objId); 
            });
        }

        public static BsonClassMap<MongoUser> SetMongoUserMap()
        {
            return BsonClassMap.RegisterClassMap<MongoUser>(s =>
            {
                s.AutoMap();
                s.MapIdMember(u => u.objId);
            });
        }

        public static BsonClassMap<MongoMessage> SetMongoMessageMap()
        {
            return BsonClassMap.RegisterClassMap<MongoMessage>(s =>
            {
                s.AutoMap();
                s.MapIdMember(m => m.objId);
            });
        }
    }
    }