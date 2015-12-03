using System.Data.Entity;
using Database.MappingTypes;
using MeasurableParameterMapping = Database.MappingTypes.MeasurableParameterMapping;
using MeasuringMessageMapping = Database.MappingTypes.MeasuringMessageMapping;
using PlantMapping = Database.MappingTypes.PlantMapping;
using PlantsAreaMapping = Database.MappingTypes.PlantsAreaMapping;
using SensorMapping = Database.MappingTypes.SensorMapping;

namespace Database
{
    [DbConfigurationType(typeof (DbContextConfiguration))]
    public class PlantingDb : DbContext
    {
        public DbSet<MeasurableParameterMapping> MeasurableParametersSet { get; set; }
        public DbSet<PlantMapping> PlantsSet { get; set; }
        public DbSet<PlantsAreaMapping> PlantsAreasSet { get; set; }
        public DbSet<SensorMapping> SensorsSet { get; set; }
        public DbSet<MeasuringMessageMapping> MeasuringMessagesSet { get; set; }
        public DbSet<ServiceScheduleMapping> ServiceSchedulesSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeasurableParameterMapping>().ToTable("MeasurableParameter");
            modelBuilder.Entity<PlantMapping>().ToTable("Plant");
            modelBuilder.Entity<PlantsAreaMapping>().ToTable("PlantsArea");
            modelBuilder.Entity<SensorMapping>().ToTable("Sensor");
            modelBuilder.Entity<MeasuringMessageMapping>().ToTable("MeasuringMessage");
            modelBuilder.Entity<ServiceScheduleMapping>().ToTable("ServiceSchedule");
        }
    }
} 