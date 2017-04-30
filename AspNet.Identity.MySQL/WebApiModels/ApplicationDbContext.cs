using AspNet.Identity.MySQL.Database;

namespace AspNet.Identity.MySQL.WebApiModels
{
    public class ApplicationDbContext : MySQLDatabase
    {
        public ApplicationDbContext(string connectionName)
            : base(connectionName)
        {
        }

        public ApplicationDbContext()
            : base("MySqlDefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}