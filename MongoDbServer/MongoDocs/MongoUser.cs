using System;
using Database.MappingTypes;

namespace MongoDbServer.MongoDocs
{
    public class MongoUser
    {
        public string objId { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string passwordToken { get; set; }
        public DateTime createdAt { get; set; }
        public string name { get; set; }

        public MongoUser(User user)
        {
            objId = user.Id.ToString();
            first_name = user.FirstName;
            last_name = user.LastName;
            email = user.Email;
            passwordToken = user.PasswordHash;
            createdAt = DateTime.Now;
            name = $"{first_name} {last_name}";
        }
    }
}