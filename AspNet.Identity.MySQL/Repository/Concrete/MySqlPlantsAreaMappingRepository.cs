using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Database.MappingTypes;

namespace AspNet.Identity.MySQL.Repository.Concrete
{
    public class MySqlPlantsAreaMappingRepository : MySqlRepository<PlantsAreaMapping>
    {

        public override List<PlantsAreaMapping> GetAll(
              Expression<Func<PlantsAreaMapping, bool>> func = null)
        {
            List<PlantsAreaMapping> plantsAreaMappings = new List<PlantsAreaMapping>();
            string commandText = "Select * from plantsarea";
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            List<Dictionary<string, string>> rows = Database.Query(commandText, parameters);
            foreach (Dictionary<string, string> row in rows)
            {
                plantsAreaMappings.Add(CreateMapping(row));
            }
           if (func != null)
            {
                return plantsAreaMappings//.AsQueryable()
                    .Where(func.Compile()).ToList();
            }
            return plantsAreaMappings;
        }

        public override PlantsAreaMapping Get(Guid id)
        {
            string commandText = "Select * from plantsarea where Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id.ToString() } };

            List<Dictionary<string, string>> rows = Database.Query(commandText, parameters);
            foreach (Dictionary<string, string> row in rows)
            {
                return CreateMapping(row);
            }
            return null;
        }

        public override bool Save(PlantsAreaMapping value, Guid id)
        {
            string commandText =
                "Insert into plantsarea(Id, PlantId, UserId, Number) values(@Id, @PlantId, @UserId, @Number)" +
                "ON DUPLICATE KEY UPDATE " +
                "Id = values(Id), " +
                "PlantId = values(PlantId), " +
                "UserId = values(UserId), " +
                "Number = values(Number)";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@Id", value.Id.ToString()},
                {"@PlantId", value.PlantId},
                {"@UserId", value.UserId},
                {"@Number", value.Number}
            };

            Database.Execute(commandText, parameters);
            return true;
        }

        public override bool Edit(PlantsAreaMapping value)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(Guid id)
        {
            string commandText = "Delete from plantsarea where Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id.ToString() } };

            Database.Execute(commandText, parameters);
            return true;
        }

        protected override PlantsAreaMapping CreateMapping(Dictionary<string, string> row)
        {
            PlantsAreaMapping plantsAreaMapping =
                (PlantsAreaMapping) Activator.CreateInstance(typeof(PlantsAreaMapping));
            plantsAreaMapping.Id = Guid.Parse(row["Id"]);
            plantsAreaMapping.PlantId = Guid.Parse(row["PlantId"]);
            plantsAreaMapping.UserId = Guid.Parse(row["UserId"]);
            plantsAreaMapping.Number = Convert.ToInt32(row["Number"]);
            return plantsAreaMapping;
        }
    }
}
