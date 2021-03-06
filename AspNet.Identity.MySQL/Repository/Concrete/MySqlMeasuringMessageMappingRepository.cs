﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Database.MappingTypes;
using NLog;

namespace AspNet.Identity.MySQL.Repository.Concrete
{
    public class MySqlMeasuringMessageMappingRepository : MySqlRepository<MeasuringMessageMapping>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly object _lockObject = new object();

        public override List<MeasuringMessageMapping> GetAll(
            Expression<Func<MeasuringMessageMapping, bool>> func = null)
        {
            var measuringMessageMappings = new List<MeasuringMessageMapping>();
            var commandText = "Select * from measuringmessage";
            var parameters = new Dictionary<string, object>();

            var rows = Database.Query(commandText, parameters);
            foreach (var row in rows)
                measuringMessageMappings.Add(CreateMapping(row));
            if (func != null)
                return measuringMessageMappings.AsQueryable().Where(func).ToList();
            return measuringMessageMappings;
        }

        public override MeasuringMessageMapping Get(Guid id)
        {
            var commandText = "Select * from measuringmessage where Id = @Id";
            var parameters = new Dictionary<string, object> {{"@Id", id.ToString()}};

            var rows = Database.Query(commandText, parameters);
            foreach (var row in rows)
                return CreateMapping(row);
            return null;
        }

        public override bool Save(MeasuringMessageMapping value, Guid id)
        {
            lock (_lockObject)
            {
                var commandText =
                    "Insert into measuringmessage(Id, PlantsAreaId, DateTime, MeasurableType, MessageType, ParameterValue) values(@Id, @PlantsAreaId, @DateTime, @MeasurableType, @MessageType, @ParameterValue) " +
                    "ON DUPLICATE KEY UPDATE " +
                    "Id = values(Id), " +
                    "PlantsAreaId = values(PlantsAreaId), " +
                    "DateTime = values(DateTime), " +
                    "MeasurableType = values(MeasurableType), " +
                    "ParameterValue = values(ParameterValue), " +
                    "MessageType = values(`MessageType`)";

                var parameters = new Dictionary<string, object>
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
        }

        public override bool Edit(MeasuringMessageMapping value)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(Guid id)
        {
            var commandText = "Delete from measuringmessage where Id = @Id";
            var parameters = new Dictionary<string, object> {{"@Id", id.ToString()}};

            Database.Execute(commandText, parameters);
            return true;
        }

        protected override MeasuringMessageMapping CreateMapping(Dictionary<string, string> row)
        {
            var measuringMessageMapping =
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
            measuringMessageMapping.ParameterValue = double.Parse(row["ParameterValue"]);
            return measuringMessageMapping;
        }

        public void SaveMany(List<MeasuringMessageMapping> measuringMessageMappings)
        {
            try
            {
                measuringMessageMappings.ForEach(mapping =>
                {
                    //Database.EnsureConnectionClosed();
                    Save(mapping, mapping.Id);
                });
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
            }
        }

        //                .OrderBy(mapping => mapping.DateTime)
        //            measuringMessageMappings = Context.MeasuringMessagesSet
        //        {
        //        if (func != null)
        //        IQueryable<MeasuringMessageMapping> measuringMessageMappings;
        //    {
        //    using (Context = new PlantingDb())
        //{

        //public int DeleteMany(int n = 1000, Expression<Func<MeasuringMessageMapping, bool>> func = null)
        //                .Where(func)
        //                .Take(Math.Min(n, Context.MeasuringMessagesSet.Count() / 3));

        //            Context.MeasuringMessagesSet.RemoveRange(measuringMessageMappings);

        //            return Context.SaveChanges();
        //        }

        //        measuringMessageMappings = Context.MeasuringMessagesSet
        //            .OrderBy(mapping => mapping.DateTime)
        //            .Take(Math.Min(n, Context.MeasuringMessagesSet.Count() / 3));

        //        Context.MeasuringMessagesSet.RemoveRange(measuringMessageMappings);

        //        try
        //        {
        //            //return
        //            Context.SaveChanges();
        //            MessageBox.Show("Good!");
        //            return 0;
        //        }
        //        catch (Exception e)
        //        {
        //            MessageBox.Show(e.Message);
        //            return 0;
        //        }
        //    }
        //}
    }
}