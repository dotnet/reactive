// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Time
{
    /// <summary>
    /// S3: time-based <c>Window</c> produces a sub-observable per window. The inner windows are flattened with
    /// <c>SelectMany</c> so the benchmark measures element delivery, not just window-open cost.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class WindowTimeBenchmarks : RateWindowBenchmarkBase
    {
        [Benchmark]
        public void Window_Time()
        {
            var scheduler = new PeriodicVirtualScheduler();   // Window(TimeSpan) runs on the real periodic path
            var n = N;
            var window = Window;
            var period = Period;

            VirtualTimeSource.Timed(scheduler, n, period)
                .Window(window, scheduler)
                .SelectMany(static w => w)     // realise inner elements, else only window-open cost is measured
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
