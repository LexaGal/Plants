using System;
using Database.MappingTypes;

namespace MongoDbServer.MongoDocs
{
    public class MongoUser
    {
        public MongoUser( //ApplicationUser user)
            User user)
        {
            objId = user.Id.ToString();
            first_name = user.FirstName;
            last_name = user.LastName;
            email = user.Email;
            passwordToken = user.PasswordHash;
            dateTime = DateTime.Now;
            name = $"{first_name} {last_name}";
        }

        public string objId { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string passwordToken { get; set; }
        public DateTime dateTime { get; set; }
        public string name { get; set; }
    }
}