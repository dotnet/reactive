// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Time
{
    /// <summary>
    /// S3: <c>Sample</c> emits the most recent element on each tick of its sampling window. The <c>Density</c>
    /// knob varies how many source elements fall into each sampling interval.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class SampleBenchmarks : RateWindowBenchmarkBase
    {
        [Benchmark]
        public void Sample()
        {
            var scheduler = new PeriodicVirtualScheduler();   // Sample(TimeSpan) runs on the real periodic path
            var n = N;
            var window = Window;
            var period = Period;

            VirtualTimeSource.Timed(scheduler, n, period)
                .Sample(window, scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
