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
    /// S3: the duration-based skip and take operators. <c>SkipLast</c>, <c>TakeLast</c> and <c>TakeLastBuffer</c>
    /// use the virtual scheduler as their clock and keep the last quarter of the timeline.
    /// </summary>
    /// <remarks>
    /// KNOWN GAP: <c>Skip(duration, scheduler)</c> and <c>Take(duration, scheduler)</c> ignore the scheduler
    /// argument and run their timer on <c>TaskPoolAsyncScheduler.Default</c> in real time, so they cannot fire in
    /// virtual time. They are measured with a one-day real duration that never fires during a run (Skip drops
    /// everything, Take passes everything): the per-element cost and the pending real timer, not the transition.
    /// </remarks>
    [BenchmarkCategory("Time")]
    public class SkipTakeTimeBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public ValueTask Skip_Duration_NeverFires() =>
            RunTimedAsync(0, static (source, scheduler, _) => source.Skip(TimeSpan.FromDays(1), scheduler));

        [Benchmark]
        public ValueTask Take_Duration_NeverFires() =>
            RunTimedAsync(0, static (source, scheduler, _) => source.Take(TimeSpan.FromDays(1), scheduler));

        [Benchmark]
        public ValueTask SkipLast_Duration() =>
            RunTimedAsync(N, static (source, scheduler, n) => source.SkipLast(TimeSpan.FromTicks(PeriodTicks * n / 4), scheduler));

        [Benchmark]
        public ValueTask TakeLast_Duration() =>
            RunTimedAsync(N, static (source, scheduler, n) => source.TakeLast(TimeSpan.FromTicks(PeriodTicks * n / 4), scheduler, scheduler));

        [Benchmark]
        public ValueTask TakeLastBuffer_Duration() =>
            RunTimedAsync(N, static (source, scheduler, n) => source.TakeLastBuffer(TimeSpan.FromTicks(PeriodTicks * n / 4), scheduler));
    }
}
