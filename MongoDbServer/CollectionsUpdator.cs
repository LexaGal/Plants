using Quartz;

namespace MongoDbServer
{
    public class CollectionsUpdator : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            MongoDbAccessor mongoDbAccessor = new MongoDbAccessor();
            mongoDbAccessor.UpdateCollections();
        }
    }
}