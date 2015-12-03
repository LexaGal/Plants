using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace Database.MappingTypes
{
    class DbContextConfiguration : DbConfiguration
    {
        public DbContextConfiguration()
        {
            SetDatabaseInitializer(new CreateDatabaseIfNotExists<PlantingDb>());
            SetProviderServices(SqlProviderServices.ProviderInvariantName, SqlProviderServices.Instance);
        }
    }
}