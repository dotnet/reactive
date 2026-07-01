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
    /// S3: <c>Timestamp</c> and <c>TimeInterval</c> annotate each element with scheduler time. Both project into
    /// value-type wrappers (<c>Timestamped&lt;T&gt;</c> / <c>TimeInterval&lt;T&gt;</c>), so — unlike the reference-type
    /// <c>Notification&lt;T&gt;</c> path — they should not allocate per element; the benchmark confirms that.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class TimestampTimeIntervalBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public void Timestamp()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);

            VirtualTimeSource.Timed(scheduler, n, period)
                .Timestamp(scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void TimeInterval()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);

            VirtualTimeSource.Timed(scheduler, n, period)
                .TimeInterval(scheduler)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
