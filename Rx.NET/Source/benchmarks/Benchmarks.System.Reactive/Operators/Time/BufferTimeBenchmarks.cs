// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Time
{
    /// <summary>
    /// S3: time-based <c>Buffer</c> collects elements into an <c>IList</c> per window. A prime zero-allocation
    /// candidate for the modernization spike (a <c>List</c> per window; the emitted list escapes to the
    /// consumer). The <c>Density</c> knob controls how many elements land in each buffer.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class BufferTimeBenchmarks : RateWindowBenchmarkBase
    {
        [Benchmark]
        public void Buffer_Time()
        {
            var scheduler = new PeriodicVirtualScheduler();   // Buffer(TimeSpan) runs on the real periodic path
            var n = N;
            var window = Window;
            var period = Period;

            VirtualTimeSource.Timed(scheduler, n, period)
                .Buffer(window, scheduler)
                .SubscribeConsume(Consumer);   // consumes each IList<int> batch

            scheduler.Start();
        }
    }
}
