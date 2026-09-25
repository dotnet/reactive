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
    /// S3: the time-based Skip/Take family. The boundary is the midpoint of the stream, so each operator does
    /// meaningful work over time. <c>SkipLast</c>/<c>TakeLast</c> keep a time-bounded queue — allocation
    /// candidates for the modernization spike. Stays on plain <see cref="HistoricalScheduler"/>: these operators
    /// only use one-shot <c>Schedule</c>, never <c>ISchedulerPeriodic</c>.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class SkipTakeTimeBenchmarks : TemporalBenchmarkBase
    {
        private TimeSpan Midpoint => TimeSpan.FromTicks(PeriodTicks * (N / 2));

        [Benchmark]
        public void Skip_Time()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var duration = Midpoint;

            VirtualTimeSource.Timed(scheduler, n, period)
                .Skip(duration, scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void Take_Time()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var duration = Midpoint;

            VirtualTimeSource.Timed(scheduler, n, period)
                .Take(duration, scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void SkipLast_Time()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var duration = Midpoint;

            VirtualTimeSource.Timed(scheduler, n, period)
                .SkipLast(duration, scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void TakeLast_Time()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var duration = Midpoint;

            VirtualTimeSource.Timed(scheduler, n, period)
                .TakeLast(duration, scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
