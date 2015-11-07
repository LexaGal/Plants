using System;
using Database.DatabaseStructure.Repository.Abstract;
using Database.MappingTypes;

namespace Database.DatabaseStructure.Repository.Concrete
{
    public class ServiceScheduleMappingRepository : Repository<ServiceScheduleMapping>, IServiceScheduleMappingRepository
    {
        public override bool Edit(ServiceScheduleMapping value)
        {
            using (Context = new PlantingDb())
            {
                ServiceScheduleMapping serviceScheduleMapping = Context.ServiceSchedulesSet.Find(value.Id);
                if (serviceScheduleMapping == null)
                {
                    throw new ArgumentNullException("value", "Can't find serviceScheduleMapping with such id");
                }
                value.CopyTo(serviceScheduleMapping);
                Context.SaveChanges();
                return true;
            }
        }
    }
}