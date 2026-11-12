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
    /// S3: <c>Buffer(timeSpan)</c> in virtual time. <c>Density</c> sets the buffer span against the arrival period:
    /// Sparse emits about two buffers per element (many empty), Dense emits one buffer holding everything. Each
    /// tick allocates a list. Buffer(time) takes an <c>AsyncGate</c> per element and per tick.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class BufferTimeBenchmarks : RateWindowBenchmarkBase
    {
        [Benchmark]
        public ValueTask Buffer_Time() =>
            RunTimedAsync(Window, static (source, scheduler, window) => source.Buffer(window, scheduler));
    }
}
