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
    /// S3: the absolute-time <c>SkipUntil</c> / <c>TakeUntil</c> (<c>DateTimeOffset</c>) overloads. The boundary is
    /// the midpoint of the stream. The virtual <see cref="HistoricalScheduler"/> is keyed on <c>DateTimeOffset</c>,
    /// so the boundary is expressed relative to its <c>MinValue</c> start clock.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class SkipUntilTakeUntilBenchmarks : TemporalBenchmarkBase
    {
        private DateTimeOffset Midpoint => DateTimeOffset.MinValue + TimeSpan.FromTicks(PeriodTicks * (N / 2));

        [Benchmark]
        public void SkipUntil_Time()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var boundary = Midpoint;

            VirtualTimeSource.Timed(scheduler, n, period)
                .SkipUntil(boundary, scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void TakeUntil_Time()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var boundary = Midpoint;

            VirtualTimeSource.Timed(scheduler, n, period)
                .TakeUntil(boundary, scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
