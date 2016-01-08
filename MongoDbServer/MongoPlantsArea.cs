using System;
using MongoDB.Bson;

namespace MongoDbServer
{
    public class MongoPlantsArea
    {
        public ObjectId ObjId { get; set; }

        public string Info { get; set; }

        public string Name { get; set; }

        public int NumberOfSensors { get; set; }

        public MongoPlantsArea(string info, string name, int numberOfSensors)
        {
            ObjId = ObjectId.GenerateNewId();
            Info = info;
            Name = name;
            NumberOfSensors = numberOfSensors;
        }
    }
}