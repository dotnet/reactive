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
    /// S3: <c>Delay</c> and <c>DelaySubscription</c> in virtual time. The Delay drain is scheduled on the virtual
    /// scheduler and guarded by an <c>AsyncGate</c>, so it runs inside the pump with the pushes.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class DelayBenchmarks : TemporalBenchmarkBase
    {
        [Benchmark]
        public ValueTask Delay() =>
            RunTimedAsync(0, static (source, scheduler, _) => source.Delay(TimeSpan.FromTicks(PeriodTicks * 3), scheduler));

        [Benchmark]
        public ValueTask DelaySubscription_TimeSpan() =>
            RunTimedAsync(0, static (source, scheduler, _) => source.DelaySubscription(TimeSpan.FromTicks(PeriodTicks / 2), scheduler));

        [Benchmark]
        public ValueTask DelaySubscription_DateTimeOffset() =>
            RunTimedAsync(0, static (source, scheduler, _) => source.DelaySubscription(scheduler.Now.AddTicks(PeriodTicks / 2), scheduler));
    }
}
