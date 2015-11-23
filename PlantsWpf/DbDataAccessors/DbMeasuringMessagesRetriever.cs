using System;
using System.Collections.Generic;
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

        public DbMeasuringMessagesRetriever(IMeasuringMessageMappingRepository measuringMessageMappingRepository)
        {
            _measuringMessageMappingRepository = measuringMessageMappingRepository;
        }

        public IEnumerable<KeyValuePair<DateTime, double>> RetrieveMessagesStatistics(ChartDescriptor chartDescriptor)
        {
            Expression<Func<MeasuringMessageMapping, bool>> expression = mapping =>
                mapping.MeasurableType.Equals(chartDescriptor.MeasurableType) &&
                mapping.PlantsAreaId.Equals(chartDescriptor.PlantsAreaId) &&
                mapping.DateTime > chartDescriptor.DateTimeFrom &&
                mapping.DateTime < chartDescriptor.DateTimeTo;

            if (chartDescriptor.OnlyCritical)
            {
                expression = mapping =>
                    mapping.MeasurableType == chartDescriptor.MeasurableType &&
                    mapping.PlantsAreaId == chartDescriptor.PlantsAreaId &&
                    mapping.DateTime > chartDescriptor.DateTimeFrom &&
                    mapping.DateTime < chartDescriptor.DateTimeTo &&
                    mapping.MessageType == MessageTypeEnum.CriticalInfo.ToString();
            }
            Task<List<MeasuringMessageMapping>> measuringMessageMappingsTask =
                _measuringMessageMappingRepository.GetAllAsync(expression);
            List<KeyValuePair<DateTime, double>> list = new List<KeyValuePair<DateTime, double>>();

            measuringMessageMappingsTask.Result?.ForEach(
                mapping => list.Add(new KeyValuePair<DateTime, double>(mapping.DateTime, mapping.ParameterValue)));
            return list.Take(Math.Min(list.Count, chartDescriptor.Number));
        }
    }
}