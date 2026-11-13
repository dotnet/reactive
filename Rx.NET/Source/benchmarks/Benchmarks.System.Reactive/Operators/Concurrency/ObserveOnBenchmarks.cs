// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Concurrency
{
    /// <summary>
    /// Concurrency-category exemplar (async S1): <c>ObserveOn</c> marshals every element onto another scheduler.
    /// The benchmark blocks until completion via <c>SubscribeBlocking</c>, so every invocation measures the whole
    /// hand-off and the default auto-tuned job applies. N is capped because each element crosses a thread boundary.
    /// </summary>
    [BenchmarkCategory("Concurrency")]
    public class ObserveOnBenchmarks
    {
        [Params(1_000, 10_000, 100_000)]
        public int N;

        private readonly Consumer _consumer = new();
        private EventLoopScheduler _scheduler = default!;
        private EventLoopScheduler _scheduler2 = default!;

        [GlobalSetup]
        public void Setup()
        {
            _scheduler = new EventLoopScheduler();
            _scheduler2 = new EventLoopScheduler();
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _scheduler.Dispose();
            _scheduler2.Dispose();
        }

        [Benchmark]
        public void ObserveOn() => Observable.Range(1, N).ObserveOn(_scheduler).SubscribeBlocking(_consumer);

        [Benchmark]
        public void SubscribeOn_ObserveOn() =>
            Observable.Range(1, N).SubscribeOn(_scheduler).ObserveOn(_scheduler2).SubscribeBlocking(_consumer);
    }
}
