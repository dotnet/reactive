// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Infrastructure;

namespace Benchmarks.System.Reactive.Operators.Async
{
    /// <summary>
    /// The async factory bridges (single-shot, so no N sweep — this measures the subscription + async-completion
    /// cost): <c>FromAsync</c>, <c>Start</c>, <c>StartAsync</c>, <c>ToAsync</c>. (<c>FromAsyncPattern</c> relies on
    /// delegate <c>BeginInvoke</c>, which is unsupported on modern .NET, so it is not benchmarked.)
    /// </summary>
    [SimpleJob(RunStrategy.Monitoring, launchCount: 1, warmupCount: 3, iterationCount: 10)]   // async completion latency → repeatable job, not the auto-tuned default
    [BenchmarkCategory("Async")]
    public class AsyncFactoryBenchmarks
    {
        private readonly Consumer _consumer = new();

        [Benchmark]
        public void FromAsync() => Observable.FromAsync(static () => Task.FromResult(1)).SubscribeBlocking(_consumer);

        [Benchmark]
        public void Start() => Observable.Start(static () => 1).SubscribeBlocking(_consumer);

        [Benchmark]
        public void StartAsync() => Observable.StartAsync(static () => Task.FromResult(1)).SubscribeBlocking(_consumer);

        [Benchmark]
        public void ToAsync() => Observable.ToAsync(static () => 1)().SubscribeBlocking(_consumer);
    }
}
