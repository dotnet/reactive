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
    /// S3: <c>Delay</c> time-shifts every element (density-independent, so N + a fixed delay). <c>DelaySubscription</c>
    /// shifts only the subscription, then relays elements untouched.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class DelayBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public void Delay()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var delay = TimeSpan.FromTicks(PeriodTicks * 4);

            VirtualTimeSource.Timed(scheduler, n, period)
                .Delay(delay, scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void DelaySubscription()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var delay = TimeSpan.FromTicks(PeriodTicks * 4);

            VirtualTimeSource.Timed(scheduler, n, period)
                .DelaySubscription(delay, scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
