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
    /// S3: <c>Timeout</c> in virtual time. <c>_NeverFires</c> uses a window far longer than the gap between
    /// elements, so every element restarts the timer (the common steady-state cost). <c>_Other</c> fires before the
    /// first element and switches to a fallback source; <c>_Error</c> fires and terminates with a
    /// <c>TimeoutException</c>. After a fire the remaining pushes are dropped.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class TimeoutBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public ValueTask Timeout_NeverFires() =>
            RunTimedAsync(0, static (source, scheduler, _) => source.Timeout(TimeSpan.FromTicks(PeriodTicks * 100), scheduler));

        [Benchmark]
        public ValueTask Timeout_Other() =>
            RunTimedAsync(0, static (source, scheduler, _) => source.Timeout(TimeSpan.FromTicks(PeriodTicks / 2), BenchmarkSources.Return(-1), scheduler));

        [Benchmark]
        public ValueTask Timeout_Error() =>
            RunTimedAsync(0, static (source, scheduler, _) => source.Timeout(TimeSpan.FromTicks(PeriodTicks / 2), scheduler));
    }
}
