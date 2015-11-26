using Database.DatabaseStructure.Repository.Abstract;
using Database.DatabaseStructure.Repository.Concrete;
using Quartz;

namespace Server
{
    public class DatabaseCleaner : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            using (IMeasuringMessageMappingRepository measuringMessageMappingRepository = new MeasuringMessageMappingRepository())
            {
                 measuringMessageMappingRepository.DeleteMany(10000);
            }
        }
    }
}
