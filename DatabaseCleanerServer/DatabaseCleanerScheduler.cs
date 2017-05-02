using Quartz;
using Quartz.Impl;

namespace DatabaseCleanerServer
{
    public class DatabaseCleanerScheduler
    {
        public static void Start()
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            var job = JobBuilder.Create<DatabaseCleaner>().Build();

            var trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule(s =>
                    s.WithIntervalInSeconds(60)
                        .OnEveryDay()
                        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0)))
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}