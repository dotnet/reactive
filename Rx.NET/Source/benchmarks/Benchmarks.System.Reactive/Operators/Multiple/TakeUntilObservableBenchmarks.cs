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
    /// S3: the observable-triggered <c>TakeUntil(other)</c> (<c>TakeUntil&lt;TSource, TOther&gt;</c> sink) with a
    /// trigger that really fires: a one-shot virtual-time <c>Timer</c> at the stream midpoint, so N/2 elements pass
    /// and then the trigger completes the output and unsubscribes the source. Complements the never-firing
    /// <c>TakeUntil(Never)</c> in <c>ComparisonBenchmarks</c> and mirrors <see cref="SkipUntilObservableBenchmarks"/>.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class TakeUntilObservableBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public void TakeUntil_Observable()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var trigger = Observable.Timer(TimeSpan.FromTicks(PeriodTicks * (n / 2)), scheduler);

            VirtualTimeSource.Timed(scheduler, n, period)
                .TakeUntil(trigger)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
