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
    /// S3: the absolute-time gates. <c>SkipUntil(endTime)</c> starts and <c>TakeUntil(endTime)</c> stops half a
    /// period after the middle of the virtual timeline, so each timer fires mid-run.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class SkipUntilTakeUntilTimeBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public ValueTask SkipUntil_Time() =>
            RunTimedAsync(N, static (source, scheduler, n) => source.SkipUntil(scheduler.Now.AddTicks(MidpointTicks(n)), scheduler));

        [Benchmark]
        public ValueTask TakeUntil_Time() =>
            RunTimedAsync(N, static (source, scheduler, n) => source.TakeUntil(scheduler.Now.AddTicks(MidpointTicks(n)), scheduler));
    }
}
