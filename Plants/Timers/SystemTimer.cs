﻿using System;
using System.Timers;

namespace Planting.Timers
{
    public static class SystemTimer
    {
        private static Timer _timer;
        private static ElapsedEventHandler _handler;
        private static TimeSpan _timeSpan;
        public static TimeSpan CurrentTimeSpan { get; set; }
        public static TimeSpan RestartTimeSpan = new TimeSpan(0, 0, 20);

        public static void Start(ElapsedEventHandler handler, TimeSpan timeSpan)
        {
            _handler = handler;
            _timeSpan = timeSpan;

            _timer = new Timer(_timeSpan.TotalMilliseconds);
            _timer.Elapsed += _handler;
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