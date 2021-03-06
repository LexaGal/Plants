using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using AspNet.Identity.MySQL.Database;
using AspNet.Identity.MySQL.Repository.Concrete;
using Database.DatabaseStructure.Repository.Abstract;
using Database.MappingTypes;
using PlantingLib.Messenging;
using PlantsWpf.ObjectsViews;

namespace PlantsWpf.DbDataAccessors
{
    public class DbMeasuringMessagesRetriever
    {
        private readonly IMeasuringMessageMappingRepository _measuringMessageMappingRepository;
        private readonly IDictionary<Guid, List<MeasuringMessage>> _messagesDictionary;
        private readonly IMySqlRepository<MeasuringMessageMapping> _mySqlMeasuringMessageMappingRepository;

        public DbMeasuringMessagesRetriever(IMeasuringMessageMappingRepository measuringMessageMappingRepository,
            IDictionary<Guid, List<MeasuringMessage>> messagesDictionary)
        {
            _measuringMessageMappingRepository = measuringMessageMappingRepository;
            _messagesDictionary = messagesDictionary;
        }

        public DbMeasuringMessagesRetriever(
            MySqlMeasuringMessageMappingRepository mySqlMeasuringMessageMappingRepository,
            Dictionary<Guid, List<MeasuringMessage>> observerMessagesDictionary)
        {
            _messagesDictionary = observerMessagesDictionary;
            _mySqlMeasuringMessageMappingRepository = mySqlMeasuringMessageMappingRepository;
        }

        public IEnumerable<KeyValuePair<DateTime, double>> RetrieveMessagesStatistics(ChartDescriptor chartDescriptor)
        {
            return ReturnRowsetMySql(chartDescriptor);

/*
                                                lock (_messagesDictionary[chartDescriptor.PlantsAreaId])
                                                {
                                                    List<MeasuringMessage> measuringMessages =
                                                        _messagesDictionary[chartDescriptor.PlantsAreaId].ToList().Where(
                                                            message => message.MeasurableType == chartDescriptor.MeasurableType).ToList();
                                    
                                    
                                                    if (chartDescriptor.OnlyCritical)
                                                    {
                                                        measuringMessages =
                                                            measuringMessages.Where(message => message.MessageType == MessageTypeEnum.CriticalInfo).ToList();
                                                    }
                                    
                                                    List<KeyValuePair<DateTime, double>> list = new List<KeyValuePair<DateTime, double>>();
                                    
                                                    if (!chartDescriptor.RefreshAll && measuringMessages.Count >= chartDescriptor.Number)
                                                    {
                                                        measuringMessages.Skip(measuringMessages.Count -
                                                                               Math.Min(measuringMessages.Count, chartDescriptor.Number)).ToList().ForEach(
                                                                                   message =>
                                                                                       list.Add(new KeyValuePair<DateTime, double>(message.DateTime,
                                                                                           message.ParameterValue)));
                                    
                                                        return list;
                                                    }
                                    
                                                    Expression<Func<MeasuringMessageMapping, bool>> func;
                                    
                                                    if (!chartDescriptor.OnlyCritical)
                                                    {
                                                        func = mapping =>
                                                            mapping.MeasurableType.Equals(chartDescriptor.MeasurableType) &&
                                                            mapping.PlantsAreaId.Equals(chartDescriptor.PlantsAreaId) &&
                                                            mapping.DateTime > chartDescriptor.DateTimeFrom &&
                                                            mapping.DateTime < chartDescriptor.DateTimeTo;
                                                    }
                                                    else
                                                    {
                                                        func = mapping =>
                                                            mapping.MeasurableType == chartDescriptor.MeasurableType &&
                                                            mapping.PlantsAreaId == chartDescriptor.PlantsAreaId &&
                                                            mapping.DateTime > chartDescriptor.DateTimeFrom &&
                                                            mapping.DateTime < chartDescriptor.DateTimeTo &&
                                                            mapping.MessageType == MessageTypeEnum.CriticalInfo.ToString();
                                                    }
                                    
                                                    //using (
                                                    //    var sc =
                                                    //        new SqlConnection(
                                                    //            ConfigurationManager.ConnectionStrings["PlantingDb"].ConnectionString))
                                                    //{
                                                    //    if (sc.State == ConnectionState.Closed)
                                                    //    {
                                                    //        sc.Open();
                                                    //    }
                                    
                                                    //    using (var tr = sc.BeginTransaction(IsolationLevel.ReadCommitted))
                                                    //    {
                                                    //        try
                                                    //        {
                                                    List<MeasuringMessageMapping> measuringMessageMappings = _measuringMessageMappingRepository.GetAll(func);
                                                    //    tr.Commit();
                                                    //}
                                                    //catch (Exception e)
                                                    //{
                                                    //    tr.Rollback();
                                                    //}
                                                    //    }
                                                    //}
                                    
                                                    if (measuringMessageMappings != null)
                                                    {
                                                        measuringMessageMappings.ForEach(
                                                            mapping =>
                                                                list.Add(new KeyValuePair<DateTime, double>(mapping.DateTime, mapping.ParameterValue)));
                                    
                                                        return list.Take(Math.Min(list.Count, chartDescriptor.Number));
                                                    }
                                    
                                                    return list;
                                                }
                                    */
        }

        // Call the stored procedure.
        private List<KeyValuePair<DateTime, double>> ReturnRowset(ChartDescriptor chartDescriptor)
        {
            var sqlConnection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["PlantingDb"].ConnectionString);
            var cmd = new SqlCommand
            {
                CommandText = "GetStatistics",
                CommandType = CommandType.StoredProcedure,
                Connection = sqlConnection
            };
            cmd.Parameters.Add("@dateTimeFrom", SqlDbType.DateTime).Value = chartDescriptor.DateTimeFrom;
            cmd.Parameters.Add("@dateTimeTo", SqlDbType.DateTime).Value = chartDescriptor.DateTimeTo;
            cmd.Parameters.Add("@measurableType", SqlDbType.NVarChar).Value = chartDescriptor.MeasurableType;
            cmd.Parameters.Add("@plantsAreaId", SqlDbType.NVarChar).Value = chartDescriptor.PlantsAreaId.ToString().ToUpper();
            cmd.Parameters.Add("@criticalInfo", SqlDbType.Bit).Value = chartDescriptor.OnlyCritical ? 1 : 0;
            cmd.Parameters.Add("@number", SqlDbType.Int).Value = chartDescriptor.Number;

            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();

            var reader = cmd.ExecuteReader();
            // Data is accessible through the DataReader object here.
            var list = new List<KeyValuePair<DateTime, double>>();

            while (reader.HasRows)
            {
                while (reader.Read())
                    list.Add(new KeyValuePair<DateTime, double>(reader.GetDateTime(0), reader.GetDouble(1)));
                reader.NextResult();
            }
            sqlConnection.Close();

            return list;
        }

        private List<KeyValuePair<DateTime, double>> ReturnRowsetMySql(ChartDescriptor chartDescriptor)
        {
            var mySqlDatabase = new MySQLDatabase();

            var parameters = new Dictionary<string, object>
            {
                {"@dateTimeFrom", chartDescriptor.DateTimeFrom.ToString("yyyy-MM-dd HH:mm:ss")},
                {"@dateTimeTo", chartDescriptor.DateTimeTo.ToString("yyyy-MM-dd HH:mm:ss")},
                {"@measurableType", chartDescriptor.MeasurableType},
                {"@plantsAreaId", chartDescriptor.PlantsAreaId.ToString().ToUpper()},
                {"@criticalInfo", chartDescriptor.OnlyCritical ? 1 : 0},
                {"@number", chartDescriptor.Number}
            };

            //cmd.Parameters.Add("@dateTimeFrom", SqlDbType.DateTime).Value =   chartDescriptor.DateTimeFrom;
            //cmd.Parameters.Add("@dateTimeTo", SqlDbType.DateTime).Value =     chartDescriptor.DateTimeTo;
            //cmd.Parameters.Add("@measurableType", SqlDbType.NVarChar).Value = chartDescriptor.MeasurableType;
            //cmd.Parameters.Add("@plantsAreaId", SqlDbType.NVarChar).Value =   chartDescriptor.PlantsAreaId.ToString().ToUpper();
            //cmd.Parameters.Add("@criticalInfo", SqlDbType.Bit).Value =        chartDescriptor.OnlyCritical ? 1 : 0;
            //cmd.Parameters.Add("@number", SqlDbType.Int).Value =              chartDescriptor.Number;

            var measuringMessageMappings = new List<KeyValuePair<DateTime, double>>();

            var commandText =
                "call statistics(@dateTimeFrom, @dateTimeTo, @measurableType, @plantsAreaId, @criticalInfo, @number)";

            var rows = mySqlDatabase.Query(commandText, parameters);
            foreach (var row in rows)
                measuringMessageMappings.Add(new KeyValuePair<DateTime, double>(DateTime.Parse(row["DateTime"]), double.Parse(row["ParameterValue"])));
            return measuringMessageMappings;
        }
    }
}