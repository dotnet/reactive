// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Time
{
    /// <summary>
    /// S3: <c>Timestamp</c> and <c>TimeInterval</c> with the virtual scheduler as their clock, so each element
    /// reads the virtual time rather than the system clock.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class TimestampTimeIntervalBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public ValueTask Timestamp() =>
            RunTimedAsync(0, static (source, scheduler, _) => source.Timestamp(scheduler));

        [Benchmark]
        public ValueTask TimeInterval() =>
            RunTimedAsync(0, static (source, scheduler, _) => source.TimeInterval(scheduler));
    }
}
