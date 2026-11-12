// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Time
{
    /// <summary>
    /// S3: the <c>Delay</c> and <c>Timeout</c> overloads that <c>DelayBenchmarks</c> and <c>TimeoutBenchmarks</c>
    /// do not cover, in virtual time.
    /// <list type="bullet">
    /// <item><c>Delay_DateTimeOffset</c> holds every element until an absolute time 3.5 periods after the start.</item>
    /// <item><c>Delay_Selector</c> delays each element by its own duration observable (a one-shot timer of three
    /// periods, subscribed once per element). <c>Delay_SubscriptionDelay_Selector</c> also delays the source
    /// subscription by half a period.</item>
    /// <item><c>Timeout_DateTimeOffset_NeverFires</c> has an absolute deadline after the last element.
    /// <c>Timeout_DateTimeOffset_Other</c> has a deadline half way through the timeline, half a period off the
    /// pushes, and then switches to a one-element fallback; the remaining pushes are dropped.</item>
    /// <item><c>Timeout_Selector_NeverFires</c> and <c>Timeout_FirstTimeout_Selector_Other</c> restart a
    /// per-element timeout observable (a one-shot timer of 100 periods) on every element, so neither fires.</item>
    /// </list>
    /// The duration timers are built once per run, inside the build lambda, on the run's scheduler. The selector
    /// rows capture them, so they allocate one closure per run.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class DelayTimeoutVariantBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public ValueTask Delay_DateTimeOffset() =>
            RunTimedAsync(0, static (source, scheduler, _) => source.Delay(scheduler.Now.AddTicks(PeriodTicks * 3 + PeriodTicks / 2), scheduler));

        [Benchmark]
        public ValueTask Delay_Selector() =>
            RunTimedAsync(0, static (source, scheduler, _) =>
            {
                var delay = AsyncObservable.Timer(TimeSpan.FromTicks(PeriodTicks * 3), scheduler);
                return source.Delay(_ => delay);
            });

        [Benchmark]
        public ValueTask Delay_SubscriptionDelay_Selector() =>
            RunTimedAsync(0, static (source, scheduler, _) =>
            {
                var subscriptionDelay = AsyncObservable.Timer(TimeSpan.FromTicks(PeriodTicks / 2), scheduler);
                var delay = AsyncObservable.Timer(TimeSpan.FromTicks(PeriodTicks * 3), scheduler);
                return source.Delay(subscriptionDelay, _ => delay);
            });

        [Benchmark]
        public ValueTask Timeout_DateTimeOffset_NeverFires() =>
            RunTimedAsync(N, static (source, scheduler, n) => source.Timeout(scheduler.Now.AddTicks(PeriodTicks * (n + 100)), scheduler));

        [Benchmark]
        public ValueTask Timeout_DateTimeOffset_Other() =>
            RunTimedAsync(N, static (source, scheduler, n) => source.Timeout(scheduler.Now.AddTicks(PeriodTicks * (n / 2) + PeriodTicks / 2), BenchmarkSources.Return(-1), scheduler));

        [Benchmark]
        public ValueTask Timeout_Selector_NeverFires() =>
            RunTimedAsync(0, static (source, scheduler, _) =>
            {
                var timeout = AsyncObservable.Timer(TimeSpan.FromTicks(PeriodTicks * 100), scheduler);
                return source.Timeout(_ => timeout);
            });

        [Benchmark]
        public ValueTask Timeout_FirstTimeout_Selector_Other() =>
            RunTimedAsync(0, static (source, scheduler, _) =>
            {
                var timeout = AsyncObservable.Timer(TimeSpan.FromTicks(PeriodTicks * 100), scheduler);
                return source.Timeout(timeout, _ => timeout, BenchmarkSources.Return(-1));
            });
    }
}
