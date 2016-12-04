using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Database.MappingTypes;

namespace AspNet.Identity.MySQL.Repository.Concrete
{
    public class MySqlPlantMappingRepository : MySqlRepository<PlantMapping>
    {

        public override List<PlantMapping> GetAll(
              Expression<Func<PlantMapping, bool>> func = null)
        {
            List<PlantMapping> plantMappings = new List<PlantMapping>();
            string commandText = "Select * from plant";
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            List<Dictionary<string, string>> rows = Database.Query(commandText, parameters);
            foreach (Dictionary<string, string> row in rows)
            {
                plantMappings.Add(CreateMapping(row));
            }
            if (func != null)
            {
                return plantMappings.AsQueryable().Where(func).ToList();
            }
            return plantMappings;
        }

        public override PlantMapping Get(Guid id)
        {
            string commandText = "Select * from plant where Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id.ToString() } };

            List<Dictionary<string, string>> rows = Database.Query(commandText, parameters);
            foreach (Dictionary<string, string> row in rows)
            {
                return CreateMapping(row);
            }
            return null;
        }

        public override bool Save(PlantMapping value, Guid id)
        {
            string commandText =
                "Insert into plant(Id, TemperatureId, HumidityId, SoilPhId, NutrientId, Name, CustomParametersIds) values(@Id, @TemperatureId, @HumidityId, @SoilPhId, @NutrientId, @Name, @CustomParametersIds) " +
                "ON DUPLICATE KEY UPDATE " +
                "Id = values(Id), " +
                "TemperatureId = values(TemperatureId), " +
                "HumidityId = values(HumidityId), " +
                "SoilPhId = values(SoilPhId), " +
                "NutrientId = values(NutrientId), " +
                "Name = values(`Name`)," +
                "CustomParametersIds = values(CustomParametersIds)";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@Id", value.Id.ToString()},
                {"@TemperatureId", value.TemperatureId },
                {"@HumidityId", value.HumidityId },
                {"@SoilPhId", value.SoilPhId},
                {"@NutrientId", value.NutrientId },
                {"@Name", value.Name },
                {"@CustomParametersIds", value.CustomParametersIds }
            };

            Database.Execute(commandText, parameters);
            return true;
        }

        public override bool Edit(PlantMapping value)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(Guid id)
        {
            string commandText = "Delete from plant where Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id.ToString() } };

            Database.Execute(commandText, parameters);
            return true;
        }

        protected override PlantMapping CreateMapping(Dictionary<string, string> row)
        {
            PlantMapping plantMapping = (PlantMapping)Activator.CreateInstance(typeof(PlantMapping));
            plantMapping.Id = Guid.Parse(row["Id"]);
            plantMapping.TemperatureId = Guid.Parse(row["TemperatureId"]);
            plantMapping.HumidityId = Guid.Parse(row["HumidityId"]);
            plantMapping.SoilPhId = Guid.Parse(row["SoilPhId"]);
            plantMapping.NutrientId = Guid.Parse(row["NutrientId"]);
            plantMapping.Name = string.IsNullOrEmpty(row["Name"]) ? null : row["Name"];
            plantMapping.CustomParametersIds = string.IsNullOrEmpty(row["CustomParametersIds"])
                ? null
                : row["CustomParametersIds"];
            return plantMapping;
        }

    }
}
