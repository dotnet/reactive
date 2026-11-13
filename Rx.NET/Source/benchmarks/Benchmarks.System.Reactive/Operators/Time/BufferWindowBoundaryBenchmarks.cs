// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Time
{
    /// <summary>
    /// S3: the observable-boundary <c>Buffer</c> / <c>Window</c> (boundaries close each batch), complementing the
    /// count and time shapes. Boundaries fire every 16 periods under virtual time.
    /// </summary>
    /// <remarks>
    /// The boundary <c>Interval</c> runs on <see cref="PeriodicVirtualScheduler"/>, so it takes Rx's real
    /// <c>ISchedulerPeriodic</c> path. On a plain <see cref="HistoricalScheduler"/> it would take the stopwatch-emulated
    /// fallback (an <c>AutoResetEvent</c> and host-lifecycle handlers per subscription), and the rows would mostly
    /// measure that fallback. <c>TimerIntervalBenchmarks.Interval_EmulatedPeriodic</c> measures the fallback itself.
    /// </remarks>
    [BenchmarkCategory("Time")]
    public class BufferWindowBoundaryBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public void Buffer_Boundary()
        {
            var scheduler = new PeriodicVirtualScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var boundaries = Observable.Interval(TimeSpan.FromTicks(PeriodTicks * 16), scheduler);

            VirtualTimeSource.Timed(scheduler, n, period)
                .Buffer(boundaries)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void Window_Boundary()
        {
            var scheduler = new PeriodicVirtualScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var boundaries = Observable.Interval(TimeSpan.FromTicks(PeriodTicks * 16), scheduler);

            VirtualTimeSource.Timed(scheduler, n, period)
                .Window(boundaries)
                .SelectMany(static w => w)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
