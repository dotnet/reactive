// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Concurrency
{
    /// <summary>
    /// S1 (gated sweep): <c>Synchronize</c> with its own gate and with a caller-supplied <see cref="AsyncGate"/>.
    /// Uncontended, so these rows isolate the per-element gate cost (a thread-pool hop per acquisition).
    /// </summary>
    [BenchmarkCategory("Concurrency")]
    public class SynchronizeBenchmarks : GatedOperatorBenchmarkBase
    {
        private readonly AsyncGate _gate = new();

        [Benchmark]
        public ValueTask Synchronize() =>
            BenchmarkSources.Range(N).Synchronize().SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask Synchronize_Gate() =>
            BenchmarkSources.Range(N).Synchronize(_gate).SubscribeConsumeAsync(Consumer);
    }
}
