using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Database.MappingTypes;

namespace AspNet.Identity.MySQL.Repository.Concrete
{
    public class MySqlServiceScheduleMappingRepository : MySqlRepository<ServiceScheduleMapping>
    {
        public override List<ServiceScheduleMapping> GetAll(
            Expression<Func<ServiceScheduleMapping, bool>> func = null)
        {
            var serviceScheduleMappings = new List<ServiceScheduleMapping>();
            var commandText = "Select * from serviceschedule";
            var parameters = new Dictionary<string, object>();

            var rows = Database.Query(commandText, parameters);
            foreach (var row in rows)
                serviceScheduleMappings.Add(CreateMapping(row));
            if (func != null)
                return serviceScheduleMappings.AsQueryable().Where(func).ToList();
            return serviceScheduleMappings;
        }

        public override ServiceScheduleMapping Get(Guid id)
        {
            var commandText = "Select * from serviceschedule where Id = @Id";
            var parameters = new Dictionary<string, object> {{"@Id", id.ToString()}};

            var rows = Database.Query(commandText, parameters);
            foreach (var row in rows)
                return CreateMapping(row);
            return null;
        }

        public override bool Save(ServiceScheduleMapping value, Guid id)
        {
            var commandText =
                "Insert into serviceschedule(Id, PlantsAreaId, MeasurableParametersIds, ServicingSpan, ServicingPauseSpan, ServiceState) values(@Id, @PlantsAreaId, @MeasurableParametersIds, @ServicingSpan, @ServicingPauseSpan, @ServiceState) " +
                "ON DUPLICATE KEY UPDATE " +
                "Id = values(Id), " +
                "PlantsAreaId = values(PlantsAreaId), " +
                "MeasurableParametersIds = values(MeasurableParametersIds), " +
                "ServicingSpan = values(ServicingSpan), " +
                "ServicingPauseSpan = values(ServicingPauseSpan), " +
                "ServiceState = values(ServiceState)";

            var parameters = new Dictionary<string, object>
            {
                {"@Id", value.Id.ToString()},
                {"@PlantsAreaId", value.PlantsAreaId},
                {"@MeasurableParametersIds", value.MeasurableParametersIds},
                {"@ServicingSpan", value.ServicingSpan},
                {"@ServicingPauseSpan", value.ServicingPauseSpan},
                {"@ServiceState", value.ServiceState}
            };

            Database.Execute(commandText, parameters);
            return true;
        }

        public override bool Edit(ServiceScheduleMapping value)
        {
            throw new NotImplementedException();
        }

        public override bool Delete(Guid id)
        {
            var commandText = "Delete from serviceschedule where Id = @Id";
            var parameters = new Dictionary<string, object> {{"@Id", id.ToString()}};

            Database.Execute(commandText, parameters);
            return true;
        }

        protected override ServiceScheduleMapping CreateMapping(Dictionary<string, string> row)
        {
            var serviceScheduleMapping =
                (ServiceScheduleMapping) Activator.CreateInstance(typeof(ServiceScheduleMapping));
            serviceScheduleMapping.Id = Guid.Parse(row["Id"]);
            serviceScheduleMapping.PlantsAreaId = Guid.Parse(row["PlantsAreaId"]);
            serviceScheduleMapping.MeasurableParametersIds = string.IsNullOrEmpty(row["MeasurableParametersIds"])
                ? null
                : row["MeasurableParametersIds"];
            serviceScheduleMapping.ServicingSpan = Convert.ToInt32(row["ServicingSpan"]);
            serviceScheduleMapping.ServicingPauseSpan = Convert.ToInt32(row["ServicingPauseSpan"]);
            serviceScheduleMapping.ServiceState = string.IsNullOrEmpty(row["ServiceState"])
                ? null
                : row["ServiceState"];
            return serviceScheduleMapping;
        }
    }
}