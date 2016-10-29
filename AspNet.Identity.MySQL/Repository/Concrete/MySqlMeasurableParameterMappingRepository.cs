using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Database.MappingTypes;

namespace AspNet.Identity.MySQL.Repository.Concrete
{
    public class MySqlMeasurableParameterMappingRepository : MySqlRepository<MeasurableParameterMapping>
    {

      public override List<MeasurableParameterMapping> GetAll(
            Expression<Func<MeasurableParameterMapping, bool>> func = null)
        {
            List<MeasurableParameterMapping> measurableParameterMappings = new List<MeasurableParameterMapping>();
            string commandText = "Select * from measurableparameter";
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            var rows = Database.Query(commandText, parameters);
            foreach (var row in rows)
            {
                var measurableParameterMapping = (MeasurableParameterMapping)Activator.CreateInstance(typeof(MeasurableParameterMapping));
                measurableParameterMapping.Id = Guid.Parse(row["Id"]);
                measurableParameterMapping.Optimal = Convert.ToInt32(row["Optimal"]);
                measurableParameterMapping.Min = Convert.ToInt32(row["Min"]);
                measurableParameterMapping.Max = Convert.ToInt32(row["Max"]);
                measurableParameterMapping.Type = string.IsNullOrEmpty(row["Type"]) ? null : row["Type"];

                measurableParameterMappings.Add(measurableParameterMapping);
            }
            return measurableParameterMappings;
        }

        public override MeasurableParameterMapping Get(Guid id)
        {
            string commandText = "Select * from measurableparameter where Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object> {{"@Id", id.ToString()}};

            var rows = Database.Query(commandText, parameters);
            foreach (var row in rows)
            {
                var measurableParameterMapping = (MeasurableParameterMapping)Activator.CreateInstance(typeof(MeasurableParameterMapping));
                measurableParameterMapping.Id = Guid.Parse(row["Id"]);
                measurableParameterMapping.Optimal = Convert.ToInt32(row["Optimal"]);
                measurableParameterMapping.Min = Convert.ToInt32(row["Min"]);
                measurableParameterMapping.Max = Convert.ToInt32(row["Max"]);
                measurableParameterMapping.Type = string.IsNullOrEmpty(row["Type"]) ? null : row["Type"];
                return measurableParameterMapping;
            }
            return null;
        }

        public override bool Save(MeasurableParameterMapping value, Guid id)
        {
            string commandText =
                "Insert into measurableparameter(Id, Optimal, Min, Max, Type) values(@Id, @Optimal, @Min, @Max, @Type) " +
                "ON DUPLICATE KEY UPDATE " +
                "Id = values(Id), " +
                "Optimal = values(Optimal), " +
                "Min = values(Min), " +
                "Max = values(Max), " +
                "Type = values(`Type`)";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@Id", value.Id.ToString()},
                {"@Optimal", value.Optimal },
                {"@Min", value.Min },
                {"@Max", value.Max},
                {"@Type", value.Type }
            };

            Database.Execute(commandText, parameters);
            return true;
        }

        public override bool Edit(MeasurableParameterMapping value)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(Guid id)
        {
            string commandText = "Delete from measurableparameter where Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object> {{"@Id", id.ToString()}};

            Database.Execute(commandText, parameters);
            return true;
        }
    }
}
