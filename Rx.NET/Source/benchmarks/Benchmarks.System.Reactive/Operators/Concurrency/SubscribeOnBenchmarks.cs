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
    /// Concurrency (async S1): <c>SubscribeOn</c> moves the subscription (and thus the synchronous source's
    /// execution) onto another scheduler. Blocks until completion via <c>SubscribeBlocking</c>.
    /// </summary>
    [SimpleJob(RunStrategy.Monitoring, launchCount: 1, warmupCount: 3, iterationCount: 10)]   // thread-crossing → repeatable job, not the auto-tuned default
    [BenchmarkCategory("Concurrency")]
    public class SubscribeOnBenchmarks
    {
        [Params(1_000, 10_000, 100_000)]
        public int N;

        private readonly Consumer _consumer = new();
        private EventLoopScheduler _scheduler = default!;

        [GlobalSetup]
        public void Setup() => _scheduler = new EventLoopScheduler();

        [GlobalCleanup]
        public void Cleanup() => _scheduler.Dispose();

        [Benchmark]
        public void SubscribeOn() => Observable.Range(1, N).SubscribeOn(_scheduler).SubscribeBlocking(_consumer);
    }
}
