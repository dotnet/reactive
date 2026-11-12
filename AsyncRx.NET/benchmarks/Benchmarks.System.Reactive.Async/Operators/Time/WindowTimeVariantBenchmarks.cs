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
    /// S3: the other time-based <c>Window</c> overloads, flattened through <c>Merge</c>: overlapping windows
    /// (<c>timeSpan</c> 4 periods, <c>timeShift</c> 2 periods) and windows closed by time or count (4 periods or 3
    /// elements).
    /// </summary>
    [BenchmarkCategory("Time")]
    public class WindowTimeVariantBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public ValueTask Window_TimeShift() =>
            RunTimedAsync(0, static (source, scheduler, _) =>
                source.Window(TimeSpan.FromTicks(PeriodTicks * 4), TimeSpan.FromTicks(PeriodTicks * 2), scheduler).Merge());

        [Benchmark]
        public ValueTask Window_TimeOrCount() =>
            RunTimedAsync(0, static (source, scheduler, _) =>
                source.Window(TimeSpan.FromTicks(PeriodTicks * 4), 3, scheduler).Merge());
    }
}
