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
    /// S3: the absolute-time (<see cref="DateTimeOffset"/>) overloads, each a separate <c>Absolute</c> nested sink
    /// rather than the <c>Relative</c> one measured by the <see cref="TimeSpan"/> overloads in
    /// <see cref="DelayBenchmarks"/> and <see cref="TimeoutBenchmarks"/>. The due time is computed from the fresh
    /// scheduler's clock at subscription. <c>Delay_Absolute</c> holds the stream until 4 periods after subscription
    /// and then replays it shifted by that offset; <c>DelaySubscription_Absolute</c> subscribes the source 4 periods
    /// late; <c>Timeout_Absolute</c> sets a single deadline after the stream ends, so it never trips and, unlike the
    /// relative timeout, never re-arms per element.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class TimeAbsoluteOverloadBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public void Delay_Absolute()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var dueTime = scheduler.Now + TimeSpan.FromTicks(PeriodTicks * 4);

            VirtualTimeSource.Timed(scheduler, n, period)
                .Delay(dueTime, scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void DelaySubscription_Absolute()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var dueTime = scheduler.Now + TimeSpan.FromTicks(PeriodTicks * 4);

            VirtualTimeSource.Timed(scheduler, n, period)
                .DelaySubscription(dueTime, scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void Timeout_Absolute()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var dueTime = scheduler.Now + TimeSpan.FromTicks(PeriodTicks * (n + 10L));   // after the last element → never trips

            VirtualTimeSource.Timed(scheduler, n, period)
                .Timeout(dueTime, scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
