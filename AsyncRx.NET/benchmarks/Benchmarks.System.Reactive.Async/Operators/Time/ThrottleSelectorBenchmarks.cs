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
    /// S3: <c>Throttle(throttleSelector)</c>, where each element's throttle window is a virtual <c>Timer</c> of half
    /// a period, so every element passes. Unlike the time-span overload, each element subscribes to a new
    /// observable.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class ThrottleSelectorBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public ValueTask Throttle_Selector() =>
            RunTimedAsync(0, static (source, scheduler, _) =>
                source.Throttle(_ => AsyncObservable.Timer(TimeSpan.FromTicks(PeriodTicks / 2), scheduler)));
    }
}
