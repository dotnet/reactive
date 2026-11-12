// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Concurrency
{
    /// <summary>
    /// S1 (capped sweep): <c>ObserveOn</c> onto the task pool (the real cross-thread hand-off), onto the immediate
    /// scheduler (the queueing overhead alone), and <c>SubscribeOn</c> plus <c>ObserveOn</c> on the task pool.
    /// The sink completes on a pool thread for the task-pool rows.
    /// </summary>
    /// <remarks>
    /// <c>ObserveOn</c> queues every element without making the producer wait, so on the task pool the queue can
    /// grow to N. The sweep stops at N = 10,000 to keep that queue, and the run time, bounded.
    /// </remarks>
    [BenchmarkCategory("Concurrency")]
    public class ObserveOnBenchmarks : BenchmarkBase
    {
        [Params(1, 100, 10_000)]
        public int N;

        [Benchmark]
        public ValueTask ObserveOn_TaskPool() =>
            BenchmarkSources.Range(N).ObserveOn(TaskPoolAsyncScheduler.Default).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask ObserveOn_Immediate() =>
            BenchmarkSources.Range(N).ObserveOn(ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SubscribeOn_ObserveOn_TaskPool() =>
            BenchmarkSources.Range(N).SubscribeOn(TaskPoolAsyncScheduler.Default).ObserveOn(TaskPoolAsyncScheduler.Default).SubscribeConsumeAsync(Consumer);
    }
}
