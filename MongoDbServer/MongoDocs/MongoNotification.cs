using System;
using MongoDB.Bson;

namespace MongoDbServer.MongoDocs
{
    public class MongoNotification
    {
        public MongoNotification(string plantsareaId, string info, string userId)
        {
            objId = ObjectId.GenerateNewId().ToString();
            this.plantsareaId = plantsareaId;
            this.info = info;
            this.userId = userId;
            dateTime = DateTime.Now;
            read = false.ToString();
        }

        public string objId { get; set; }
        public string plantsareaId { get; set; }
        public DateTime dateTime { get; set; }
        public string info { get; set; }
        public string read { get; set; }
        public string userId { get; set; }
    }
}