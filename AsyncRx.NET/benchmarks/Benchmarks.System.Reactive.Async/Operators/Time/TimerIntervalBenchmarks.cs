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
    /// S3: periodic time <i>sources</i> on a <see cref="Microsoft.Reactive.Testing.Async.TestAsyncScheduler"/>, each limited to <c>N</c>
    /// ticks. Every tick is a <c>scheduler.Delay(period)</c> wake-up followed by a <c>RendezVous</c> continuation,
    /// so these rows measure the AsyncRx periodic loop. AsyncRx has no <c>ISchedulerPeriodic</c> fast path, so
    /// there is no emulated-periodic comparison row as in the Rx suite.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class TimerIntervalBenchmarks : TemporalBenchmarkBase
    {
        private Func<int, bool> _belowN = default!;
        private Func<int, ValueTask<bool>> _belowNAsync = default!;

        // The Generate conditions depend on N, so they are built once here and passed to the static builders as their
        // state: the measured region then allocates no closure or delegate for them.
        [GlobalSetup]
        public void Setup()
        {
            var n = N;
            _belowN = i => i < n;
            _belowNAsync = i => new ValueTask<bool>(i < n);
        }

        [Benchmark]
        public ValueTask Interval_Take() =>
            RunSourceAsync(N, static (scheduler, n) => AsyncObservable.Interval(TimeSpan.FromTicks(PeriodTicks), scheduler).Take(n));

        [Benchmark]
        public ValueTask TimerPeriodic_Take() =>
            RunSourceAsync(N, static (scheduler, n) => AsyncObservable.Timer(TimeSpan.FromTicks(PeriodTicks), TimeSpan.FromTicks(PeriodTicks), scheduler).Take(n));

        [Benchmark]
        public ValueTask GenerateTimed() =>
            RunSourceAsync(_belowN, static (scheduler, condition) => AsyncObservable.Generate(0, condition, static i => i + 1, static i => i, static _ => TimeSpan.FromTicks(PeriodTicks), scheduler));

        [Benchmark]
        public ValueTask GenerateTimed_Async() =>
            RunSourceAsync(_belowNAsync, static (scheduler, condition) => AsyncObservable.Generate(
                0,
                condition,
                static i => new ValueTask<int>(i + 1),
                static i => new ValueTask<int>(i),
                static _ => new ValueTask<TimeSpan>(TimeSpan.FromTicks(PeriodTicks)),
                scheduler));
    }
}
