using System;
using System.Timers;

namespace PlantingLib.Timers
{
    public static class SystemTimer
    {
        private static Timer _timer;
        public static TimeSpan CurrentTimeSpan { get; set; }
        public static TimeSpan RestartTimeSpan = new TimeSpan(0, 3, 0);

        public static void Start(ElapsedEventHandler handler, TimeSpan timeSpan)
        {
            _timer = new Timer(timeSpan.TotalMilliseconds);
            _timer.Elapsed += handler;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            Console.ReadLine();
        }

        public static void Restart()
        {
            _timer.Stop();
            _timer.Start();
        }
    }
}