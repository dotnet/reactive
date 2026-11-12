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
    /// S3 (temporal) flagship benchmark. <c>N</c> elements are pushed <c>PeriodTicks</c> apart on a
    /// <see cref="VirtualTimeAsyncScheduler"/>, so even a huge <c>Dense</c> window costs no wall-clock waiting.
    /// The <c>Density</c> knob varies the window-vs-arrival relationship that drives the Throttle hot path (dense
    /// arrivals mean constant cancel-and-reschedule). Throttle takes an <c>AsyncGate</c> per element and per
    /// timer callback.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class ThrottleBenchmarks : RateWindowBenchmarkBase
    {
        [Benchmark]
        public ValueTask Throttle() =>
            RunTimedAsync(Window, static (source, scheduler, window) => source.Throttle(window, scheduler));
    }
}
