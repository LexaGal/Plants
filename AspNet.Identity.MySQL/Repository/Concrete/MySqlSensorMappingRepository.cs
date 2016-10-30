using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Database.MappingTypes;

namespace AspNet.Identity.MySQL.Repository.Concrete
{
    public class MySqlSensorMappingRepository : MySqlRepository<SensorMapping>
    {

        public override List<SensorMapping> GetAll(
              Expression<Func<SensorMapping, bool>> func = null)
        {
            List<SensorMapping> sensorMappings = new List<SensorMapping>();
            string commandText = "Select * from sensor";
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            var rows = Database.Query(commandText, parameters);
            foreach (var row in rows)
            {
                var sensorMapping = (SensorMapping)Activator.CreateInstance(typeof(SensorMapping));
                sensorMapping.Id = Guid.Parse(row["Id"]);
                sensorMapping.PlantsAreaId = Guid.Parse(row["PlantsAreaId"]);
                sensorMapping.MeasurableParameterId = Guid.Parse(row["MeasurableParameterId"]);
                sensorMapping.MeasuringTimeout = Convert.ToInt32(row["MeasuringTimeout"]);
                sensorMapping.Type = string.IsNullOrEmpty(row["Type"]) ? null : row["Type"];
                sensorMappings.Add(sensorMapping);
            }
            return sensorMappings;
        }

        public override SensorMapping Get(Guid id)
        {
            string commandText = "Select * from sensor where Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id.ToString() } };

            var rows = Database.Query(commandText, parameters);
            foreach (var row in rows)
            {
                var sensorMapping = (SensorMapping)Activator.CreateInstance(typeof(SensorMapping));
                sensorMapping.Id = Guid.Parse(row["Id"]);
                sensorMapping.PlantsAreaId = Guid.Parse(row["PlantsAreaId"]);
                sensorMapping.MeasurableParameterId = Guid.Parse(row["MeasurableParameterId"]);
                sensorMapping.MeasuringTimeout = Convert.ToInt32(row["MeasuringTimeout"]);
                sensorMapping.Type = string.IsNullOrEmpty(row["Type"]) ? null : row["Type"];
                return sensorMapping;
            }
            return null;
        }

        public override bool Save(SensorMapping value, Guid id)
        {
            string commandText =
                "Insert into sensor(Id, PlantsAreaId, MeasurableParameterId, MeasuringTimeout, Type) values(@Id, @PlantsAreaId, @MeasurableParameterId, @MeasuringTimeout, @Type) " +
                "ON DUPLICATE KEY UPDATE " +
                "Id = values(Id), " +
                "PlantsAreaId = values(PlantsAreaId), " +
                "MeasurableParameterId = values(MeasurableParameterId), " +
                "MeasuringTimeout = values(MeasuringTimeout), " +
                "Type = values(`Type`)";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@Id", value.Id.ToString()},
                {"@PlantsAreaId", value.PlantsAreaId},
                {"@MeasurableParameterId", value.MeasurableParameterId},
                {"@MeasuringTimeout", value.MeasuringTimeout},
                {"@Type", value.Type}
            };

            Database.Execute(commandText, parameters);
            return true;
        }

        public override bool Edit(SensorMapping value)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(Guid id)
        {
            string commandText = "Delete from sensor where Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id.ToString() } };

            Database.Execute(commandText, parameters);
            return true;
        }
    }
}
