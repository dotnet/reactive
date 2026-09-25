// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Time
{
    /// <summary>
    /// S3 (temporal / "data over time") flagship benchmark. <c>N</c> elements are emitted across a virtual
    /// timeline (spaced <c>PeriodTicks</c> apart) and run to completion under a <see cref="HistoricalScheduler"/>;
    /// <c>Start()</c> drains the whole timeline synchronously, so even a huge <c>Dense</c> window costs
    /// microseconds of wall-clock. The <c>Density</c> knob varies the window-vs-arrival relationship that
    /// drives Throttle's hot path (dense arrivals → constant cancel+reschedule).
    /// </summary>
    [BenchmarkCategory("Time")]
    public class ThrottleBenchmarks : RateWindowBenchmarkBase
    {
        [Benchmark]
        public void Throttle()
        {
            var scheduler = new HistoricalScheduler();      // fresh per iteration (Clock/queue are instance state)
            var n = N;                                      // hoist params into locals → no field-capture closures
            var window = Window;
            var period = Period;

            VirtualTimeSource.Timed(scheduler, n, period)
                .Throttle(window, scheduler)                // MUST pass the virtual scheduler, or it waits in real time
                .SubscribeConsume(Consumer);                // Consumer defeats DCE without allocating a closure

            scheduler.Start();                              // drains the whole timeline synchronously
        }
    }
}
