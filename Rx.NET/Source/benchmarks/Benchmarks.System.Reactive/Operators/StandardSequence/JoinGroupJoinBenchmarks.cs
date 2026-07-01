// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.StandardSequence
{
    /// <summary>
    /// The window-join operators, run under virtual time so the overlap is controlled (each element's window
    /// spans ~2 arrivals → ~O(N) pairs, not the O(N²) that open-ended windows on cold sources would produce).
    /// </summary>
    [BenchmarkCategory("StandardSequence")]
    public class JoinGroupJoinBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public void Join()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var window = TimeSpan.FromTicks(PeriodTicks * 2);

            VirtualTimeSource.Timed(scheduler, n, period)
                .Join(
                    VirtualTimeSource.Timed(scheduler, n, period),
                    _ => Observable.Timer(window, scheduler),
                    _ => Observable.Timer(window, scheduler),
                    static (l, r) => l + r)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void GroupJoin()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var window = TimeSpan.FromTicks(PeriodTicks * 2);

            VirtualTimeSource.Timed(scheduler, n, period)
                .GroupJoin(
                    VirtualTimeSource.Timed(scheduler, n, period),
                    _ => Observable.Timer(window, scheduler),
                    _ => Observable.Timer(window, scheduler),
                    static (l, rights) => rights)
                .SelectMany(static rights => rights)
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
