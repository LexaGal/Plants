using System.Data.Entity;
using PlantingLib.MappingTypes;

namespace Database
{
    [DbConfigurationType(typeof(DbContextConfiguration))]
    public class PlantingDb : DbContext
    {
        public DbSet<MeasurableParameterMapping> MeasurableParametersSet { get; set; }
        public DbSet<PlantMapping> PlantsSet { get; set; }
        public DbSet<PlantsAreaMapping> PlantsAreasSet { get; set; }
        public DbSet<SensorMapping> SensorsSet { get; set; }
        public DbSet<MeasuringMessageMapping> MeasuringMessagesSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeasurableParameterMapping>().ToTable("MeasurableParameter");
            modelBuilder.Entity<PlantMapping>().ToTable("Plant");
            modelBuilder.Entity<PlantsAreaMapping>().ToTable("PlantsArea");
            modelBuilder.Entity<SensorMapping>().ToTable("Sensor");
            modelBuilder.Entity<MeasuringMessageMapping>().ToTable("MeasuringMessage");
        }
    }
} ;