using System;
using MongoDB.Bson;

namespace MongoDbServer
{
    public class MongoPlantsArea
    {
        public string objId { get; set; }

        public string info { get; set; }

        public string name { get; set; }

        public int numberOfSensors { get; set; }

        public DateTime dateTime { get; set; }

        public MongoPlantsArea(string newInfo, string newName, int newNumberOfSensors)
        {
            objId = ObjectId.GenerateNewId().ToString();
            info = newInfo;
            name = newName;
            numberOfSensors = newNumberOfSensors;
            dateTime = DateTime.Now;
        }
    }
}