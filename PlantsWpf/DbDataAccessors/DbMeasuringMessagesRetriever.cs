using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
        public DbMeasuringMessagesRetriever(IMeasuringMessageMappingRepository measuringMessageMappingRepository, IDictionary<Guid, List<MeasuringMessage>> messagesDictionary)
        {
            _measuringMessageMappingRepository = measuringMessageMappingRepository;
            _messagesDictionary = messagesDictionary;
        }

        public IEnumerable<KeyValuePair<DateTime, double>> RetrieveMessagesStatistics(ChartDescriptor chartDescriptor)
        {
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
                var measuringMessageMappings = _measuringMessageMappingRepository.GetAll(func);
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
        }
    }
}