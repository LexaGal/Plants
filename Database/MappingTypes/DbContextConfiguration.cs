using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace Database.MappingTypes
{
    internal class DbContextConfiguration : DbConfiguration
    {
        public DbContextConfiguration()
        {
            SetDatabaseInitializer(new CreateDatabaseIfNotExists<PlantingDb>());
            SetProviderServices(SqlProviderServices.ProviderInvariantName, SqlProviderServices.Instance);
        }
    }
}