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
    /// S3: the duration-based skip and take operators. <c>Skip</c> and <c>Take</c> switch half a period after the
    /// middle of the virtual timeline, so the timer fires mid-run and both halves of each row are exercised.
    /// <c>SkipLast</c>, <c>TakeLast</c> and <c>TakeLastBuffer</c> use the virtual scheduler as their clock and keep
    /// the last quarter of the timeline.
    /// </summary>
    /// <remarks>
    /// After <c>Take</c>'s timer completes the sequence, the source keeps pushing; the operator drops those
    /// elements under its gate.
    /// </remarks>
    [BenchmarkCategory("Time")]
    public class SkipTakeTimeBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public ValueTask Skip_Duration() =>
            RunTimedAsync(N, static (source, scheduler, n) => source.Skip(TimeSpan.FromTicks(PeriodTicks * n / 2 + PeriodTicks / 2), scheduler));

        [Benchmark]
        public ValueTask Take_Duration() =>
            RunTimedAsync(N, static (source, scheduler, n) => source.Take(TimeSpan.FromTicks(PeriodTicks * n / 2 + PeriodTicks / 2), scheduler));

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
