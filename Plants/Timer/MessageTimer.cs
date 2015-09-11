using System;
using System.Threading;
using System.Timers;

namespace Planting.Timer
{
    public static class MessageTimer
    {
        private static System.Timers.Timer _timer;

        public static void Start(ElapsedEventHandler handler, TimeSpan timeSpan)
        {
            _timer = new System.Timers.Timer(timeSpan.TotalMilliseconds);
            _timer.Elapsed += handler;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            Console.ReadLine();
        }
    }
}