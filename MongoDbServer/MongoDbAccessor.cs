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
                case "users":
                    return Database.GetCollection<BsonDocument>("users");
                case "messages":
                    return Database.GetCollection<BsonDocument>("messages");
                default:
                    return null;
            }
        }

        public MongoDbAccessor(string database = "meteor", string connectionString = "mongodb://localhost:3001")
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
            GetMongoCollection(collectionName).DeleteManyAsync(new BsonDocument());
        }

        public void UpdateCollections()
        {
            IMongoCollection<BsonDocument> sensorsCollection = GetMongoCollection("sensors");
            IMongoCollection<BsonDocument> plantsAreasCollection = GetMongoCollection("plantsareas");

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

            sensorsCollection.DeleteManyAsync(new BsonDocument());
            plantsAreasCollection.DeleteManyAsync(new BsonDocument());

            IEnumerable<BsonDocument> docsSensor = mongoSensors.ConvertAll(input => input.ToBsonDocument());
            IEnumerable<BsonDocument> docsPlantsArea = mongoPlantsAreas.ConvertAll(input => input.ToBsonDocument());

            sensorsCollection.InsertManyAsync(docsSensor);
            plantsAreasCollection.InsertManyAsync(docsPlantsArea);
        }

        public void AddMongoPlantsArea(MongoPlantsArea mongoPlantsArea)
        {
            IMongoCollection<BsonDocument> plantsAreasCollection = GetMongoCollection("plantsareas");
            plantsAreasCollection.InsertOneAsync(mongoPlantsArea.ToBsonDocument());
        }

        public void AddMongoSensor(MongoSensor mongoSensor)
        {
            IMongoCollection<BsonDocument> sensorsCollection = GetMongoCollection("sensors");
            sensorsCollection.InsertOneAsync(mongoSensor.ToBsonDocument());
        }

        public void AddMongoUser(MongoUser mongoUser)
        {
            IMongoCollection<BsonDocument> usersCollection = GetMongoCollection("users");
            usersCollection.InsertOneAsync(mongoUser.ToBsonDocument());
        }

        public void AddMongoMessage(MongoMessage mongoMessage)
        {
            IMongoCollection<BsonDocument> messagesCollection = GetMongoCollection("messages");
            messagesCollection.InsertOneAsync(mongoMessage.ToBsonDocument());
        }

        public void DeleteMongoPlantsArea(MongoPlantsArea mongoPlantsArea)
        {
            IMongoCollection<BsonDocument> plantsAreasCollection = GetMongoCollection("plantsareas");
            plantsAreasCollection.DeleteOneAsync(mongoPlantsArea.ToBsonDocument());
        }

        public void DeleteMongoSensor(MongoSensor mongoSensor)
        {
            IMongoCollection<BsonDocument> sensorsCollection = GetMongoCollection("sensors");
            sensorsCollection.DeleteOneAsync(mongoSensor.ToBsonDocument());
        }
    }
}