using System;
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
            BsonClassMapsSetter.SetMongoSensorMap();
            BsonClassMapsSetter.SetMongoPlantsArea();
        }

        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<SensorsCollectionUpdator>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule(s =>
                    s.WithIntervalInSeconds(20)
                        .OnEveryDay()
                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0)))
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
