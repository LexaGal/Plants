using Quartz;

namespace MongoDbServer
{
    public class CollectionsUpdator : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var mongoDbAccessor = new MongoDbAccessor();
            mongoDbAccessor.UpdateCollections();
        }
    }
}