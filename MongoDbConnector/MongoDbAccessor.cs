using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDbConnector
{
    public class MongoDbAccessor
    {
        public async void Connect()
        {
            MongoClient client = new MongoClient("mongodb://localhost:3001");
            IMongoDatabase database = client.GetDatabase("meteor");
            IMongoCollection<BsonDocument> mongoCollection = database.GetCollection<BsonDocument>("lists");
            List<BsonDocument> docs = await mongoCollection.Find(new BsonDocument()).ToListAsync();
            docs.ForEach(Console.WriteLine);
        }
    }
}
