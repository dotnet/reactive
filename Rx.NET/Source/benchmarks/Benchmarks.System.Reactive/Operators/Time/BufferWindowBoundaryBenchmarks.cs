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
    [BenchmarkCategory("Time")]
    public class BufferWindowBoundaryBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public void Buffer_Boundary()
        {
            var scheduler = new HistoricalScheduler();
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
            var scheduler = new HistoricalScheduler();
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
