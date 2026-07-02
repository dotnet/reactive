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
    /// S3: the timed source factories. <c>Interval</c> and <c>Timer</c> use <c>ISchedulerPeriodic</c>, which
    /// <see cref="PeriodicVirtualScheduler"/> implements, so they run Rx's real periodic fast path in virtual
    /// time. <c>Interval_EmulatedPeriodic</c> keeps the plain <see cref="HistoricalScheduler"/> to quantify the
    /// stopwatch-emulated fallback's overhead. <c>Generate</c> (timed) is the self-rescheduling source the
    /// suite uses to drive the other temporal operators.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class TimerIntervalBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public void Interval()
        {
            var scheduler = new PeriodicVirtualScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);

            Observable.Interval(period, scheduler).Take(n)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void Interval_EmulatedPeriodic()
        {
            // HistoricalScheduler has no ISchedulerPeriodic, so Rx falls back to the stopwatch-emulated
            // periodic path (AutoResetEvent + host-lifecycle registration per subscription).
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);

            Observable.Interval(period, scheduler).Take(n)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void Timer()
        {
            var scheduler = new PeriodicVirtualScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);

            Observable.Timer(period, period, scheduler).Take(n)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void Generate_Timed()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);

            Observable.Generate(0, i => i < n, i => i + 1, i => i, _ => period, scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
