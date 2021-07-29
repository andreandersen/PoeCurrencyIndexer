using System;
using System.Diagnostics;

namespace PoeCurrencyIndexer.Indexer.Common
{
    public static class StopwatchExtension
    {
        public static int RestartAndGetElapsedMs(this Stopwatch sw)
        {
            var elapsed = sw.Elapsed;
            sw.Restart();
            return Convert.ToInt32(elapsed.TotalMilliseconds);
        }
    }
}
