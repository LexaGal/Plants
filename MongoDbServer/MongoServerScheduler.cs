using System;
using MongoDbServer.BsonClassMaps;
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
            //.WithDailyTimeIntervalSchedule(s =>
            //    s.WithIntervalInSeconds(1)
            //        .OnEveryDay()
            //        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0)))

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
