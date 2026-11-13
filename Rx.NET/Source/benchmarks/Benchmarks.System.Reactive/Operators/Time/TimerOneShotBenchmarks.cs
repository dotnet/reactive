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
    /// S4: the one-shot <c>Timer</c> as the subject of measurement — creating, subscribing and firing a single
    /// relative (<c>Timer.Single.Relative</c>) or absolute (<c>Timer.Single.Absolute</c>) timer, which is what
    /// per-element duration selectors pay for. Each invocation uses a fresh <see cref="HistoricalScheduler"/> (its
    /// construction is included and is the same for both methods) and drains it with <c>Start()</c>. There is no N:
    /// the cost is a fixed per-subscription figure. The periodic <c>Timer(dueTime, period)</c> is in
    /// <see cref="TimerIntervalBenchmarks"/>.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class TimerOneShotBenchmarks : BenchmarkBase
    {
        private static readonly TimeSpan DueTime = TimeSpan.FromTicks(10);

        [Benchmark]
        public void Timer_Relative()
        {
            var scheduler = new HistoricalScheduler();

            Observable.Timer(DueTime, scheduler).SubscribeConsume(Consumer);

            scheduler.Start();
        }

        [Benchmark]
        public void Timer_Absolute()
        {
            var scheduler = new HistoricalScheduler();

            Observable.Timer(scheduler.Now + DueTime, scheduler).SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
