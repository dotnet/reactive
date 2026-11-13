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
    /// S3: the observable-duration overloads not covered by <see cref="TimeOverloadGapsBenchmarks"/>.
    /// <c>Throttle_Selector</c> runs the <c>Throttle&lt;TSource, TThrottle&gt;</c> sink, subscribing a per-element
    /// duration observable (a one-shot timer of half a period, so it fires before the next element and every element
    /// passes). <c>Delay_SubscriptionDelay_Selector</c> runs <c>Delay.SelectorWithSubscriptionDelay</c>: the source is
    /// subscribed only after a 2-period subscription-delay timer fires, then each element is held by its own 4-period
    /// timer. All timers are virtual-time <c>Observable.Timer</c>s on the benchmark's scheduler.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class TimeSelectorOverloadBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public void Throttle_Selector()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var window = TimeSpan.FromTicks(PeriodTicks / 2);   // shorter than the gap → every element passes

            VirtualTimeSource.Timed(scheduler, n, period)
                .Throttle(_ => Observable.Timer(window, scheduler))
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void Delay_SubscriptionDelay_Selector()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var subscriptionDelay = Observable.Timer(TimeSpan.FromTicks(PeriodTicks * 2), scheduler);
            var delay = TimeSpan.FromTicks(PeriodTicks * 4);

            VirtualTimeSource.Timed(scheduler, n, period)
                .Delay(subscriptionDelay, _ => Observable.Timer(delay, scheduler))
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
