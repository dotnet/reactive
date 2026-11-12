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
    /// S3: <c>Sample(interval)</c> in virtual time. The sampling loop is periodic (a <c>scheduler.Delay</c> per tick),
    /// so <c>Density</c> sets how many ticks fire per element: Sparse ticks twice per element, Dense ticks once
    /// in total. Sample takes an <c>AsyncGate</c> per element and per tick.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class SampleBenchmarks : RateWindowBenchmarkBase
    {
        [Benchmark]
        public ValueTask Sample() =>
            RunTimedAsync(Window, static (source, scheduler, window) => source.Sample(window, scheduler));
    }
}
