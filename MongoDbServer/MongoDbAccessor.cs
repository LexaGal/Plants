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


        public MongoDbAccessor(string database = "meteor", string connectionString = "mongodb://localhost:3001")
        {
            _database = database;
            _connectionString = connectionString;
        }

        public void ConnectToMongoDatabase()
        {
            MongoClient client = new MongoClient(_connectionString);
            Database = client.GetDatabase(_database);
        }

        public void ListDocumentsOfMongoCollection(IMongoCollection<BsonDocument> collection)
        {
            IAsyncCursor<BsonDocument> asyncCursor = collection.FindAsync(new BsonDocument()).Result;

            while (asyncCursor.MoveNext())
            {
                asyncCursor.Current.ToList().ForEach(document =>
                {
                    Console.WriteLine(document.ToJson());
                });
            }
        }

        public void UpdateSensorsCollection()
        {
            IMongoCollection<BsonDocument> sensorsCollection = Database.GetCollection<BsonDocument>("sensors");
            IMongoCollection<BsonDocument> plantsAreasCollection = Database.GetCollection<BsonDocument>("plantsareas");

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
                MongoPlantsArea mongoPlantsArea = new MongoPlantsArea(area.ToString(),
                    $"{area.Plant.Name}{area.Id.ToString().Substring(0, 8)}", area.Sensors.Count);

                mongoPlantsAreas.Add(mongoPlantsArea);
                
                mongoSensors.AddRange(area.Sensors.Select(sensor => new MongoSensor(sensor, mongoPlantsArea.ObjId)));
             }

            sensorsCollection.DeleteMany(new BsonDocument());
            plantsAreasCollection.DeleteMany(new BsonDocument());

            IEnumerable<BsonDocument> docsSensor = mongoSensors.ConvertAll(input => input.ToBsonDocument());
            IEnumerable<BsonDocument> docsPlantsArea = mongoPlantsAreas.ConvertAll(input => input.ToBsonDocument());

            sensorsCollection.InsertManyAsync(docsSensor);
            plantsAreasCollection.InsertManyAsync(docsPlantsArea);
        }
    }
}