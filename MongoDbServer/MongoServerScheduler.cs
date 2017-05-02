using Quartz;
using Quartz.Impl;

namespace MongoDbServer
{
    [DisallowConcurrentExecution]
    public class MongoServerScheduler
    {
        public static void Main()
        {
            Start();
        }

        public static void Start()
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            var job = JobBuilder.Create<CollectionsUpdator>().Build();

            var trigger = TriggerBuilder.Create().Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}