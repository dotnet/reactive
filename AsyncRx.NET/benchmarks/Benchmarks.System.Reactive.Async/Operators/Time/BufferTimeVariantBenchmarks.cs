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
    /// S3: the other time-based <c>Buffer</c> overloads: overlapping buffers (<c>timeSpan</c> 4 periods,
    /// <c>timeShift</c> 2 periods) and buffers closed by time or count (4 periods or 3 elements, whichever comes
    /// first).
    /// </summary>
    [BenchmarkCategory("Time")]
    public class BufferTimeVariantBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public ValueTask Buffer_TimeShift() =>
            RunTimedAsync(0, static (source, scheduler, _) =>
                source.Buffer(TimeSpan.FromTicks(PeriodTicks * 4), TimeSpan.FromTicks(PeriodTicks * 2), scheduler));

        [Benchmark]
        public ValueTask Buffer_TimeOrCount() =>
            RunTimedAsync(0, static (source, scheduler, _) =>
                source.Buffer(TimeSpan.FromTicks(PeriodTicks * 4), 3, scheduler));
    }
}
