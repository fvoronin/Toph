using System;
using System.Diagnostics;

namespace Toph.Common
{
    public static class TimerHelper
    {
        public static TimeSpan Time(Action action)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            action();
            stopwatch.Stop();

            return stopwatch.Elapsed;
        }
    }
}