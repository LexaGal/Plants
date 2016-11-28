﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Database.MappingTypes;

namespace AspNet.Identity.MySQL.Repository.Concrete
{
    public class MySqlMeasuringMessageMappingRepository : MySqlRepository<MeasuringMessageMapping>
    {

        public override List<MeasuringMessageMapping> GetAll(
            Expression<Func<MeasuringMessageMapping, bool>> func = null)
        {
            List<MeasuringMessageMapping> measuringMessageMappings = new List<MeasuringMessageMapping>();
            string commandText = "Select * from measuringmessage";
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            List<Dictionary<string, string>> rows = Database.Query(commandText, parameters);
            foreach (Dictionary<string, string> row in rows)
            {               
                measuringMessageMappings.Add(CreateMapping(row));
            }
            if (func != null)
            {
                return measuringMessageMappings.Where(func.Compile()).ToList();
            }
            return measuringMessageMappings;
        }

        public override MeasuringMessageMapping Get(Guid id)
        {
            string commandText = "Select * from measuringmessage where Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object> {{"@Id", id.ToString()}};

            List<Dictionary<string, string>> rows = Database.Query(commandText, parameters);
            foreach (Dictionary<string, string> row in rows)
            {
                return CreateMapping(row);
            }
            return null;
        }

        public override bool Save(MeasuringMessageMapping value, Guid id)
        {
            string commandText =
                "Insert into measuringmessage(Id, PlantsAreaId, DateTime, MeasurableType, MessageType, ParameterValue) values(@Id, @PlantsAreaId, @DateTime, @MeasurableType, @MessageType, @ParameterValue) " +
                "ON DUPLICATE KEY UPDATE " +
                "Id = values(Id), " +
                "PlantsAreaId = values(PlantsAreaId), " +
                "DateTime = values(DateTime), " +
                "MeasurableType = values(MeasurableType), " +
                "ParameterValue = values(ParameterValue), " +
                "MessageType = values(`MessageType`)";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@Id", value.Id.ToString()},
                {"@PlantsAreaId", value.PlantsAreaId},
                {"@DateTime", value.DateTime},
                {"@MeasurableType", value.MeasurableType},
                {"@MessageType", value.MessageType},
                {"ParameterValue", value.ParameterValue}
            };

            Database.Execute(commandText, parameters);
            return true;
        }

        public override bool Edit(MeasuringMessageMapping value)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(Guid id)
        {
            string commandText = "Delete from measuringmessage where Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object> {{"@Id", id.ToString()}};

            Database.Execute(commandText, parameters);
            return true;
        }

        protected override MeasuringMessageMapping CreateMapping(Dictionary<string, string> row)
        {
            MeasuringMessageMapping measuringMessageMapping =
                    (MeasuringMessageMapping) Activator.CreateInstance(typeof(MeasuringMessageMapping));
                measuringMessageMapping.Id = Guid.Parse(row["Id"]);
                measuringMessageMapping.PlantsAreaId = Guid.Parse(row["PlantsAreaId"]);
                measuringMessageMapping.DateTime = DateTime.Parse(row["DateTime"]);
                measuringMessageMapping.MeasurableType = string.IsNullOrEmpty(row["MeasurableType"])
                    ? null
                    : row["MeasurableType"];
                measuringMessageMapping.MessageType = string.IsNullOrEmpty(row["MessageType"])
                    ? null
                    : row["MessageType"];
                measuringMessageMapping.ParameterValue = Double.Parse(row["ParameterValue"]);
                return measuringMessageMapping;
        }
    }
}
