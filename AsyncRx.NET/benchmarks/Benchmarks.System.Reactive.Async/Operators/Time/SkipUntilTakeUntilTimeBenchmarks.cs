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
    /// S3: the absolute-time gates. <c>TakeUntil(endTime)</c> stops at the middle of the virtual timeline.
    /// </summary>
    /// <remarks>
    /// KNOWN GAP: <c>SkipUntil(endTime, scheduler)</c> ignores the scheduler argument and runs its timer on
    /// <c>TaskPoolAsyncScheduler.Default</c> against the real clock, so it cannot fire in virtual time. It is
    /// measured with an end time one day in the real future (it drops everything).
    /// </remarks>
    [BenchmarkCategory("Time")]
    public class SkipUntilTakeUntilTimeBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public ValueTask SkipUntil_Time_NeverFires() =>
            RunTimedAsync(0, static (source, scheduler, _) => source.SkipUntil(DateTimeOffset.UtcNow.AddDays(1), scheduler));

        [Benchmark]
        public ValueTask TakeUntil_Time() =>
            RunTimedAsync(N, static (source, scheduler, n) => source.TakeUntil(scheduler.Now.AddTicks(PeriodTicks * n / 2 + PeriodTicks / 2), scheduler));
    }
}
