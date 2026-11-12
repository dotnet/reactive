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
    /// S3: time <i>sources</i> with an absolute due time, on a
    /// <see cref="Microsoft.Reactive.Testing.Async.TestAsyncScheduler"/>, each producing <c>N</c> elements
    /// <see cref="TemporalBenchmarkBase.PeriodTicks"/> apart: the periodic <c>Timer(DateTimeOffset, period)</c>
    /// limited by <c>Take(N)</c>, and the <c>Generate</c> forms whose time selector returns a
    /// <see cref="DateTimeOffset"/> (synchronous and <c>ValueTask</c> delegates). These are the absolute-time
    /// siblings of <c>TimerIntervalBenchmarks</c>. The <c>Generate</c> rows capture the run's start time, and
    /// so allocate one closure per run.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class TimeSourceAbsoluteBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public ValueTask TimerAbsolutePeriodic_Take() =>
            RunSourceAsync(N, static (scheduler, n) => AsyncObservable.Timer(scheduler.Now.AddTicks(PeriodTicks), TimeSpan.FromTicks(PeriodTicks), scheduler).Take(n));

        [Benchmark]
        public ValueTask GenerateAbsolute() =>
            RunSourceAsync(N, static (scheduler, n) =>
            {
                var origin = scheduler.Now;
                return AsyncObservable.Generate(
                    0,
                    i => i < n,
                    static i => i + 1,
                    static i => i,
                    i => origin.AddTicks(PeriodTicks * (i + 1)),
                    scheduler);
            });

        [Benchmark]
        public ValueTask GenerateAbsolute_Async() =>
            RunSourceAsync(N, static (scheduler, n) =>
            {
                var origin = scheduler.Now;
                return AsyncObservable.Generate(
                    0,
                    i => new ValueTask<bool>(i < n),
                    static i => new ValueTask<int>(i + 1),
                    static i => new ValueTask<int>(i),
                    i => new ValueTask<DateTimeOffset>(origin.AddTicks(PeriodTicks * (i + 1))),
                    scheduler);
            });
    }
}
