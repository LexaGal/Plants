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
            List<ServiceScheduleMapping> serviceScheduleMappings = new List<ServiceScheduleMapping>();
            string commandText = "Select * from serviceschedule";
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            List<Dictionary<string, string>> rows = Database.Query(commandText, parameters);
            foreach (Dictionary<string, string> row in rows)
            {
                serviceScheduleMappings.Add(CreateMapping(row));
            }
           if (func != null)
            {
                return serviceScheduleMappings.AsQueryable().Where(func).ToList();
            }
            return serviceScheduleMappings;
        }

        public override ServiceScheduleMapping Get(Guid id)
        {
            string commandText = "Select * from serviceschedule where Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id.ToString() } };

            List<Dictionary<string, string>> rows = Database.Query(commandText, parameters);
            foreach (Dictionary<string, string> row in rows)
            {
                return CreateMapping(row);
            }
            return null;
        }

        public override bool Save(ServiceScheduleMapping value, Guid id)
        {
            string commandText =
                "Insert into serviceschedule(Id, PlantsAreaId, MeasurableParametersIds, ServicingSpan, ServicingPauseSpan, ServiceState) values(@Id, @PlantsAreaId, @MeasurableParametersIds, @ServicingSpan, @ServicingPauseSpan, @ServiceState) " +
                "ON DUPLICATE KEY UPDATE " +
                "Id = values(Id), " +
                "PlantsAreaId = values(PlantsAreaId), " +
                "MeasurableParametersIds = values(MeasurableParametersIds), " +
                "ServicingSpan = values(ServicingSpan), " +
                "ServicingPauseSpan = values(ServicingPauseSpan), " +
                "ServiceState = values(ServiceState)";

            Dictionary<string, object> parameters = new Dictionary<string, object>
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
            string commandText = "Delete from serviceschedule where Id = @Id";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id.ToString() } };

            Database.Execute(commandText, parameters);
            return true;
        }

        protected override ServiceScheduleMapping CreateMapping(Dictionary<string, string> row)
        {
            ServiceScheduleMapping serviceScheduleMapping =
                                (ServiceScheduleMapping)Activator.CreateInstance(typeof(ServiceScheduleMapping));
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
