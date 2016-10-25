using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AspNet.Identity.MySQL.Database;
using Database.DatabaseStructure.Repository.Abstract;
using Database.MappingTypes;

namespace AspNet.Identity.MySQL.Repository.Concrete
{
    public class MySqlMeasurableParameterMappingRepository : MySqlRepository<MeasurableParameterMapping>
    {

      public override List<MeasurableParameterMapping> GetAll(
            Expression<Func<MeasurableParameterMapping, bool>> func = null)
        {
            List<MeasurableParameterMapping> measurableParameterMappings = new List<MeasurableParameterMapping>();
            string commandText = "Select * from measurableparameter";// where Id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>(); //{{"@id", userId}};

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
            throw new NotImplementedException();
        }

        public override bool Save(MeasurableParameterMapping value, Guid id)
        {
            throw new NotImplementedException();
        }

        public override bool Edit(MeasurableParameterMapping value)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
