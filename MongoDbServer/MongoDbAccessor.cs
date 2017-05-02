using System;
using System.Collections.Generic;
using System.Linq;
using AspNet.Identity.MySQL.Repository.Concrete;
using Mapper.MapperContext;
using MongoDbServer.MongoDocs;
using MongoDB.Bson;
using MongoDB.Driver;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;

namespace MongoDbServer
{
    public class MongoDbAccessor
    {
        private readonly string _connectionString;
        private readonly string _database;

        //public MongoDbAccessor(string database = "meteor", string connectionString = "mongodb://localhost:3001")
        public MongoDbAccessor(string database = "alexmongodb",
            string connectionString = "mongodb://Alex:qYYfO8Di@ds056998.mongolab.com:56998/alexmongodb")
        {
            _database = database;
            _connectionString = connectionString;
            ConnectToMongoDatabase();
        }

        public IMongoDatabase Database { get; private set; }

        public IMongoCollection<BsonDocument> GetMongoCollection(string collectionName)
        {
            switch (collectionName)
            {
                case "sensors":
                    return Database.GetCollection<BsonDocument>("sensors");
                case "plantsareas":
                    return Database.GetCollection<BsonDocument>("plantsareas");
                case "usersNET":
                    return Database.GetCollection<BsonDocument>("usersNET");
                case "messages":
                    return Database.GetCollection<BsonDocument>("messages");
                case "notifications":
                    return Database.GetCollection<BsonDocument>("notifications");
                default:
                    return null;
            }
        }

        public void ConnectToMongoDatabase()
        {
            var client = new MongoClient(_connectionString);
            Database = client.GetDatabase(_database);
        }

        public void ListDocumentsOfMongoCollection(string collectionName)
        {
            var asyncCursor =
                GetMongoCollection(collectionName).FindAsync(new BsonDocument()).Result;

            while (asyncCursor.MoveNext())
                asyncCursor.Current.ToList().ForEach(document => { Console.WriteLine(document.ToJson()); });
        }

        public void DeleteCollectionData(string collectionName)
        {
            GetMongoCollection(collectionName).DeleteMany(new BsonDocument());
        }

        public void UpdateCollections()
        {
            var dbMapper = DbMapper.GetMySqlDbMapper();
            //IPlantsAreaMappingRepository plantsAreaMappingRepository = new PlantsAreaMappingRepository();
            //ISensorMappingRepository sensorMappingRepository = new SensorMappingRepository();
            var sqlPlantsAreaMappingRepository = new MySqlPlantsAreaMappingRepository();
            var sqlSensorMappingRepository = new MySqlSensorMappingRepository();

            ParameterServicesInfo.SetBaseParameters();
            var plantsAreaMappings = sqlPlantsAreaMappingRepository.GetAll();

            var mongoPlantsAreas = new List<MongoPlantsArea>();
            var mongoSensors = new List<MongoSensor>();

            var pas = new PlantsAreas();
            plantsAreaMappings.ForEach(p => pas.AddPlantsArea(dbMapper.RestorePlantArea(p)));

            foreach (var area in pas.Areas)
                sqlSensorMappingRepository.GetAll(sm => sm.PlantsAreaId == area.Id)
                    .ForEach(sensorMapping => dbMapper.RestoreSensor(sensorMapping, area));

            foreach (var area in pas.Areas)
            {
                var mongoPlantsArea = new MongoPlantsArea(area);

                mongoPlantsAreas.Add(mongoPlantsArea);

                mongoSensors.AddRange(area.Sensors.Select(sensor => new MongoSensor(sensor)));
            }

            DeleteCollectionData("messages");
            DeleteCollectionData("notifications");

            var sensorsCollection = GetMongoCollection("sensors");
            var plantsAreasCollection = GetMongoCollection("plantsareas");

            sensorsCollection.DeleteMany(new BsonDocument());
            plantsAreasCollection.DeleteMany(new BsonDocument());

            IEnumerable<BsonDocument> docsSensor = mongoSensors.ConvertAll(input => input.ToBsonDocument());
            IEnumerable<BsonDocument> docsPlantsArea = mongoPlantsAreas.ConvertAll(input => input.ToBsonDocument());

            sensorsCollection.InsertMany(docsSensor);
            plantsAreasCollection.InsertMany(docsPlantsArea);
        }

        public void SaveMongoPlantsArea(MongoPlantsArea mongoPlantsArea)
        {
            var plantsAreasCollection = GetMongoCollection("plantsareas");
            plantsAreasCollection.ReplaceOneAsync(bsonDocument => bsonDocument["_id"] == mongoPlantsArea.objId,
                mongoPlantsArea.ToBsonDocument(), new UpdateOptions {IsUpsert = true});
        }

        public void SaveMongoSensor(MongoSensor mongoSensor)
        {
            var sensorsCollection = GetMongoCollection("sensors");
            sensorsCollection.ReplaceOneAsync(bsonDocument => bsonDocument["_id"] == mongoSensor.objId,
                mongoSensor.ToBsonDocument(), new UpdateOptions {IsUpsert = true});
        }

        public void AddMongoUser(MongoUser mongoUser)
        {
            var usersCollection = GetMongoCollection("usersNET");
            usersCollection.InsertOneAsync(mongoUser.ToBsonDocument());
        }

        public void AddMongoMessage(MongoMessage mongoMessage)
        {
            var messagesCollection = GetMongoCollection("messages");
            messagesCollection.InsertOneAsync(mongoMessage.ToBsonDocument());
        }

        public void AddMongoNotification(MongoNotification mongoNotification)
        {
            var messagesCollection = GetMongoCollection("notifications");
            messagesCollection.InsertOneAsync(mongoNotification.ToBsonDocument());
        }

        public void DeleteMongoPlantsArea(MongoPlantsArea mongoPlantsArea)
        {
            var plantsAreasCollection = GetMongoCollection("plantsareas");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", mongoPlantsArea.objId);
            plantsAreasCollection.DeleteOneAsync(filter);
        }

        public void DeleteMongoSensor(MongoSensor mongoSensor)
        {
            var sensorsCollection = GetMongoCollection("sensors");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", mongoSensor.objId);
            sensorsCollection.DeleteOneAsync(filter);
        }
    }
}