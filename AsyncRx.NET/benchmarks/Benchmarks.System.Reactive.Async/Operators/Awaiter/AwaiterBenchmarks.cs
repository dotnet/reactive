// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Awaiter
{
    /// <summary>
    /// S1 (variant sweep): awaiting an async observable for its last value. <c>await source</c> goes through
    /// <c>GetAwaiter</c> (an <c>AsyncAsyncSubject</c>); <c>RunAsync</c> takes a cancellation token, either
    /// <c>None</c> or a live token that never fires; the connectable form awaits a <c>Publish()</c>, which connects
    /// it.
    /// </summary>
    [BenchmarkCategory("Awaiter")]
    public class AwaiterBenchmarks : OperatorVariantBenchmarkBase
    {
        private readonly CancellationTokenSource _cts = new();

        [Benchmark(Baseline = true)]
        public async ValueTask Await()
        {
            Consumer.Consume(await BenchmarkSources.Range(N));
        }

        [Benchmark]
        public async ValueTask RunAsync_NoToken()
        {
            Consumer.Consume(await BenchmarkSources.Range(N).RunAsync(CancellationToken.None));
        }

        [Benchmark]
        public async ValueTask RunAsync_LiveToken()
        {
            Consumer.Consume(await BenchmarkSources.Range(N).RunAsync(_cts.Token));
        }

        [Benchmark]
        public async ValueTask Await_Connectable()
        {
            Consumer.Consume(await BenchmarkSources.Range(N).Publish());
        }

        [GlobalCleanup]
        public void Cleanup() => _cts.Dispose();
    }
}
