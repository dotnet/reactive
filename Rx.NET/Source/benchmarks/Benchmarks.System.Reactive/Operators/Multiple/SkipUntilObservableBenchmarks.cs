// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Multiple
{
    /// <summary>
    /// S3: the observable-triggered <c>SkipUntil</c> (skip until another sequence emits), complementing the
    /// absolute-time overload under Operators/Time. The trigger fires at the stream midpoint under virtual time.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class SkipUntilObservableBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public void SkipUntil_Observable()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var trigger = Observable.Timer(TimeSpan.FromTicks(PeriodTicks * (n / 2)), scheduler);

            VirtualTimeSource.Timed(scheduler, n, period)
                .SkipUntil(trigger)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
