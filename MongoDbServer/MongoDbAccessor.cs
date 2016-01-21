using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Database;
using Database.DatabaseStructure.Repository.Abstract;
using Database.DatabaseStructure.Repository.Concrete;
using Database.MappingTypes;
using Mapper.MapperContext;
using MongoDbServer.BsonClassMaps;
using MongoDbServer.MongoDocs;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;
using PlantingLib.Sensors;
using BsonWriter = Newtonsoft.Json.Bson.BsonWriter;

namespace MongoDbServer
{
    public class MongoDbAccessor
    {
        public IMongoDatabase Database { get; private set; }
        private readonly string _database;
        private readonly string _connectionString;
        
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

        public MongoDbAccessor(string database = "meteor", string connectionString = "mongodb://localhost:3001")
        //public MongoDbAccessor(string database = "alexmongodb", string connectionString = "mongodb://Alex:qYYfO8Di@ds056998.mongolab.com:56998/alexmongodb")
        {
            _database = database;
            _connectionString = connectionString;
            ConnectToMongoDatabase();
        }

        public void ConnectToMongoDatabase()
        {
            MongoClient client = new MongoClient(_connectionString);
            Database = client.GetDatabase(_database);
        }

        public void ListDocumentsOfMongoCollection(string collectionName)
        {
            IAsyncCursor<BsonDocument> asyncCursor =
                GetMongoCollection(collectionName).FindAsync(new BsonDocument()).Result;

            while (asyncCursor.MoveNext())
            {
                asyncCursor.Current.ToList().ForEach(document =>
                {
                    Console.WriteLine(document.ToJson());
                });
            }
        }

        public void DeleteCollectionData(string collectionName)
        {
            GetMongoCollection(collectionName).DeleteMany(new BsonDocument());
        }

        public void UpdateCollections()
        {
            DbMapper dbMapper = DbMapper.GetDbMapper();
            IPlantsAreaMappingRepository plantsAreaMappingRepository = new PlantsAreaMappingRepository();
            ISensorMappingRepository sensorMappingRepository = new SensorMappingRepository();
            ParameterServicesInfo.SetBaseParameters();
            List<PlantsAreaMapping> plantsAreaMappings = plantsAreaMappingRepository.GetAll();

            List<MongoPlantsArea> mongoPlantsAreas = new List<MongoPlantsArea>();
            List<MongoSensor> mongoSensors = new List<MongoSensor>();

            PlantsAreas pas = new PlantsAreas();
            plantsAreaMappings.ForEach(p => pas.AddPlantsArea(dbMapper.RestorePlantArea(p)));

            foreach (PlantsArea area in pas.Areas)
            {
                sensorMappingRepository.GetAll(sm => sm.PlantsAreaId == area.Id)
                    .ForEach(sensorMapping => dbMapper.RestoreSensor(sensorMapping, area));
            }
   
            foreach (PlantsArea area in pas.Areas)
            {
                MongoPlantsArea mongoPlantsArea = new MongoPlantsArea(area);

                mongoPlantsAreas.Add(mongoPlantsArea);
                
                mongoSensors.AddRange(area.Sensors.Select(sensor => new MongoSensor(sensor)));
             }

            DeleteCollectionData("messages");
            DeleteCollectionData("notifications");

            IMongoCollection<BsonDocument> sensorsCollection = GetMongoCollection("sensors");
            IMongoCollection<BsonDocument> plantsAreasCollection = GetMongoCollection("plantsareas");

            sensorsCollection.DeleteMany(new BsonDocument());
            plantsAreasCollection.DeleteMany(new BsonDocument());
            
            IEnumerable<BsonDocument> docsSensor = mongoSensors.ConvertAll(input => input.ToBsonDocument());
            IEnumerable<BsonDocument> docsPlantsArea = mongoPlantsAreas.ConvertAll(input => input.ToBsonDocument());

            sensorsCollection.InsertMany(docsSensor);
            plantsAreasCollection.InsertMany(docsPlantsArea);
        }

        public void SaveMongoPlantsArea(MongoPlantsArea mongoPlantsArea)
        {
            IMongoCollection<BsonDocument> plantsAreasCollection = GetMongoCollection("plantsareas");
            plantsAreasCollection.ReplaceOneAsync(bsonDocument => bsonDocument["_id"] == mongoPlantsArea.objId,
                mongoPlantsArea.ToBsonDocument(), new UpdateOptions {IsUpsert = true});
        }

        public void SaveMongoSensor(MongoSensor mongoSensor)
        {
            IMongoCollection<BsonDocument> sensorsCollection = GetMongoCollection("sensors");
            sensorsCollection.ReplaceOneAsync(bsonDocument => bsonDocument["_id"] == mongoSensor.objId,
                    mongoSensor.ToBsonDocument(), new UpdateOptions {IsUpsert = true});
        }

        public void AddMongoUser(MongoUser mongoUser)
        {
            IMongoCollection<BsonDocument> usersCollection = GetMongoCollection("usersNET");
            usersCollection.InsertOneAsync(mongoUser.ToBsonDocument());
        }

        public void AddMongoMessage(MongoMessage mongoMessage)
        {
            IMongoCollection<BsonDocument> messagesCollection = GetMongoCollection("messages");
            messagesCollection.InsertOneAsync(mongoMessage.ToBsonDocument());
        }

        public void AddMongoNotification(MongoNotification mongoNotification)
        {
            IMongoCollection<BsonDocument> messagesCollection = GetMongoCollection("notifications");
            messagesCollection.InsertOneAsync(mongoNotification.ToBsonDocument());
        }

        public void DeleteMongoPlantsArea(MongoPlantsArea mongoPlantsArea)
        {
            IMongoCollection<BsonDocument> plantsAreasCollection = GetMongoCollection("plantsareas");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", mongoPlantsArea.objId);
            plantsAreasCollection.DeleteOneAsync(filter);
        }

        public void DeleteMongoSensor(MongoSensor mongoSensor)
        {
            IMongoCollection<BsonDocument> sensorsCollection = GetMongoCollection("sensors");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", mongoSensor.objId);
            sensorsCollection.DeleteOneAsync(filter);
        }
    }
}