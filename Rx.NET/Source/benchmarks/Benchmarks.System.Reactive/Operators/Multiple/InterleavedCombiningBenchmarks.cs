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
    /// The combining operators over two sources that <b>genuinely alternate</b> on one virtual clock (left at
    /// step, 2·step, …; right offset by step/2), instead of the cold-<c>Range</c> versions elsewhere where one
    /// source drains fully before the other emits. This exercises the real interleaving path — e.g.
    /// <c>CombineLatest</c> actually re-combines on each side's change rather than overwriting an unused value.
    /// </summary>
    [BenchmarkCategory("Multiple")]
    public class InterleavedCombiningBenchmarks : TemporalBenchmarkBase
    {
        private IObservable<int> Left(IScheduler scheduler) =>
            VirtualTimeSource.Timed(scheduler, N, TimeSpan.FromTicks(PeriodTicks));

        private IObservable<int> Right(IScheduler scheduler) =>
            VirtualTimeSource.Timed(scheduler, N, TimeSpan.FromTicks(PeriodTicks), TimeSpan.FromTicks(PeriodTicks / 2));

        [Benchmark]
        public void Zip_Interleaved()
        {
            var scheduler = new HistoricalScheduler();
            Observable.Zip(Left(scheduler), Right(scheduler), static (a, b) => a + b).SubscribeConsume(Consumer);
            scheduler.Start();
        }

        [Benchmark]
        public void CombineLatest_Interleaved()
        {
            var scheduler = new HistoricalScheduler();
            Observable.CombineLatest(Left(scheduler), Right(scheduler), static (a, b) => a + b).SubscribeConsume(Consumer);
            scheduler.Start();
        }

        [Benchmark]
        public void WithLatestFrom_Interleaved()
        {
            var scheduler = new HistoricalScheduler();
            Left(scheduler).WithLatestFrom(Right(scheduler), static (a, b) => a + b).SubscribeConsume(Consumer);
            scheduler.Start();
        }

        [Benchmark]
        public void Merge_Interleaved()
        {
            var scheduler = new HistoricalScheduler();
            Observable.Merge(Left(scheduler), Right(scheduler)).SubscribeConsume(Consumer);
            scheduler.Start();
        }

        [Benchmark]
        public void Amb_Interleaved()
        {
            var scheduler = new HistoricalScheduler();
            Left(scheduler).Amb(Right(scheduler)).SubscribeConsume(Consumer);   // both live; right (earlier offset) wins
            scheduler.Start();
        }

        [Benchmark]
        public void Switch_Interleaved()
        {
            var scheduler = new HistoricalScheduler();
            var n = N;
            var period = TimeSpan.FromTicks(PeriodTicks);
            var innerStep = TimeSpan.FromTicks(PeriodTicks / 2);

            // Each inner spans ~2 outer periods, so a new inner arrives while the previous is still emitting →
            // Switch cancels it mid-flight (its defining behaviour), unlike the synchronous cold version.
            VirtualTimeSource.Timed(scheduler, n, period)
                .Select(_ => VirtualTimeSource.Timed(scheduler, 4, innerStep))
                .Switch()
                .SubscribeConsume(Consumer);

            scheduler.Start();
        }
    }
}
