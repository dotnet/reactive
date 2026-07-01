// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Time
{
    /// <summary>
    /// S3: the remaining temporal overload shapes — <c>Sample</c> by observable sampler, <c>Delay</c> and
    /// <c>Timeout</c> by per-element selector, and the timed <c>TakeLastBuffer</c> — complementing the primary
    /// TimeSpan overloads elsewhere in Operators/Time.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class TimeOverloadGapsBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public void Sample_Observable()
        {
            // The sampler is Observable.Interval, which hits SchedulePeriodic — use the periodic scheduler.
            var scheduler = new PeriodicVirtualScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var sampler = Observable.Interval(TimeSpan.FromTicks(PeriodTicks * 4), scheduler);

            VirtualTimeSource.Timed(scheduler, n, period)
                .Sample(sampler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void Delay_Selector()
        {
            // One-shot Timer only (no periodic path); same scheduler type keeps the file uniform.
            var scheduler = new PeriodicVirtualScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var delay = TimeSpan.FromTicks(PeriodTicks * 4);

            VirtualTimeSource.Timed(scheduler, n, period)
                .Delay(_ => Observable.Timer(delay, scheduler))
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void Timeout_Selector()
        {
            var scheduler = new PeriodicVirtualScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var timeout = TimeSpan.FromTicks(PeriodTicks * 4);   // longer than the gap → never fires

            VirtualTimeSource.Timed(scheduler, n, period)
                .Timeout(_ => Observable.Timer(timeout, scheduler))
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void TakeLastBuffer_Time()
        {
            var scheduler = new PeriodicVirtualScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var duration = TimeSpan.FromTicks(PeriodTicks * (n / 2));

            VirtualTimeSource.Timed(scheduler, n, period)
                .TakeLastBuffer(duration, scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
