using Quartz;

namespace MongoDbServer
{
    public class SensorsCollectionUpdator : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            MongoDbAccessor mongoDbAccessor = new MongoDbAccessor();
            mongoDbAccessor.ConnectToMongoDatabase();
            mongoDbAccessor.UpdateSensorsCollection();
        }
    }
}