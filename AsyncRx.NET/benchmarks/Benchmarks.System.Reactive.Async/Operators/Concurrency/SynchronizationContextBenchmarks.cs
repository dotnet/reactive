// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Concurrency
{
    /// <summary>
    /// S1 (capped sweep): <c>ObserveOn</c> and <c>SubscribeOn</c> with a <see cref="SynchronizationContextAsyncScheduler"/>
    /// over a context that runs posted work inline, so the rows measure the scheduler and context plumbing without
    /// a real UI thread.
    /// </summary>
    /// <remarks>
    /// The sweep stops at N = 10,000, as in <see cref="ObserveOnBenchmarks"/>: the <c>ObserveOn</c> queue is
    /// unbounded.
    /// </remarks>
    [BenchmarkCategory("Concurrency")]
    public class SynchronizationContextBenchmarks : BenchmarkBase
    {
        [Params(1, 100, 10_000)]
        public int N;

        private readonly SynchronizationContextAsyncScheduler _scheduler = new(new InlineSynchronizationContext());

        [Benchmark]
        public ValueTask ObserveOn_SynchronizationContext() =>
            BenchmarkSources.Range(N).ObserveOn(_scheduler).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SubscribeOn_SynchronizationContext() =>
            BenchmarkSources.Range(N).SubscribeOn(_scheduler).SubscribeConsumeAsync(Consumer);

        internal sealed class InlineSynchronizationContext : SynchronizationContext
        {
            public override void Post(SendOrPostCallback d, object? state) => d(state);

            public override void Send(SendOrPostCallback d, object? state) => d(state);

            public override SynchronizationContext CreateCopy() => this;
        }
    }
}
