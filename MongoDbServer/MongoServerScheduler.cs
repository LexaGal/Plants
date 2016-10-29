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
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<CollectionsUpdator>().Build();

            ITrigger trigger = TriggerBuilder.Create().Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
