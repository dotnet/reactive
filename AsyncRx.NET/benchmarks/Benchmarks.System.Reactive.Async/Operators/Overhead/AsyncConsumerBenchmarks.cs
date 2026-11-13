// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Overhead
{
    /// <summary>
    /// S1 (capped sweep): the same <c>Range</c> + <c>Select</c> pipeline into a synchronous sink (baseline) and into
    /// a truly asynchronous consumer whose <c>OnNextAsync</c> yields before consuming. Every element then
    /// completes asynchronously, which shows the cost of the slow path through each operator's awaits.
    /// </summary>
    /// <remarks>
    /// The sweep stops at N = 10,000, as in <see cref="Concurrency.ObserveOnBenchmarks"/>: every element of the
    /// yielding row is a hop to the thread pool, so N = 1,000,000 would take seconds per invocation. The pipeline
    /// takes no gate, so the class derives from <see cref="BenchmarkBase"/>, not the gated base.
    /// </remarks>
    [BenchmarkCategory("Overhead")]
    public class AsyncConsumerBenchmarks : BenchmarkBase
    {
        [Params(1, 100, 10_000)]
        public int N;

        private YieldingAsyncObserver<int> _yielding = default!;

        [GlobalSetup]
        public void Setup() => _yielding = new YieldingAsyncObserver<int>(Consumer);

        [Benchmark(Baseline = true)]
        public ValueTask SynchronousConsumer() =>
            BenchmarkSources.Range(N).Select(static v => v + 1).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask YieldingConsumer()
        {
            _yielding.Completion.Reset();
            return BenchmarkSources.Range(N).Select(static v => v + 1).SubscribeAwaitAsync(_yielding, _yielding.Completion);
        }
    }
}
