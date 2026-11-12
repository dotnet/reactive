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
    /// S3: <c>Window(timeSpan)</c> in virtual time, with every window flattened through <c>Merge</c>.
    /// <c>Density</c> sets how many windows open per element (Sparse: about two; Dense: one in total).
    /// </summary>
    [BenchmarkCategory("Time")]
    public class WindowTimeBenchmarks : RateWindowBenchmarkBase
    {
        [Benchmark]
        public ValueTask Window_Time() =>
            RunTimedAsync(Window, static (source, scheduler, window) => source.Window(window, scheduler).Merge());
    }
}
