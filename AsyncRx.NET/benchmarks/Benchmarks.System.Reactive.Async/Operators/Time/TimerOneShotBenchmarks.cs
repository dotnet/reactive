// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Time
{
    /// <summary>
    /// Per-subscription cost of the one-shot <c>Timer</c> (relative and absolute due time) on a fresh
    /// <see cref="Microsoft.Reactive.Testing.Async.TestAsyncScheduler"/>: schedule, advance, fire, complete. No N sweep.
    /// </summary>
    [BenchmarkCategory("Time")]
    public class TimerOneShotBenchmarks
    {
        private const long DueTicks = 10;

        private readonly Consumer _consumer = new();

        [Benchmark(Baseline = true)]
        public ValueTask Timer_TimeSpan()
        {
            var sink = Sinks.For<long>(_consumer);
            return VirtualTimeDriver.RunSourceAsync(sink, sink.Completion, 0, static (scheduler, _) =>
                AsyncObservable.Timer(TimeSpan.FromTicks(DueTicks), scheduler));
        }

        [Benchmark]
        public ValueTask Timer_DateTimeOffset()
        {
            var sink = Sinks.For<long>(_consumer);
            return VirtualTimeDriver.RunSourceAsync(sink, sink.Completion, 0, static (scheduler, _) =>
                AsyncObservable.Timer(scheduler.Now.AddTicks(DueTicks), scheduler));
        }
    }
}
