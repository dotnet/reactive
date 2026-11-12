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
    /// S1 (gated sweep): <c>ObserveOn</c> onto the task pool (the real cross-thread hand-off), onto the immediate
    /// scheduler (the queueing overhead alone), and <c>SubscribeOn</c> plus <c>ObserveOn</c> on the task pool.
    /// The sink completes on a pool thread for the task-pool rows.
    /// </summary>
    /// <remarks>
    /// KNOWN GAP: <c>ObserveOn(ImmediateAsyncScheduler.Instance)</c> never completes (it hung at N = 1 in the dry
    /// run). The immediate scheduler awaits a scheduled action inline, which appears to run the observer's drain
    /// loop inside <c>SubscribeAsync</c>; the cause was not isolated further. The row is commented out.
    /// KNOWN GAP: <c>ObserveOn</c> onto the task pool (and onto a synchronization context) passed at N = 1 and 100
    /// but hung at N = 1,000 and 10,000 (an apparent per-element race in the scheduled observer), so this class
    /// sweeps only N = 1 and 100.
    /// </remarks>
    [BenchmarkCategory("Concurrency")]
    public class ObserveOnBenchmarks : BenchmarkBase
    {
        [Params(1, 100)]
        public int N;

        [Benchmark]
        public ValueTask ObserveOn_TaskPool() =>
            BenchmarkSources.Range(N).ObserveOn(TaskPoolAsyncScheduler.Default).SubscribeConsumeAsync(Consumer);

        // KNOWN GAP (see remarks): hangs.
        // [Benchmark]
        // public ValueTask ObserveOn_Immediate() =>
        //     BenchmarkSources.Range(N).ObserveOn(ImmediateAsyncScheduler.Instance).SubscribeConsumeAsync(Consumer);

        [Benchmark]
        public ValueTask SubscribeOn_ObserveOn_TaskPool() =>
            BenchmarkSources.Range(N).SubscribeOn(TaskPoolAsyncScheduler.Default).ObserveOn(TaskPoolAsyncScheduler.Default).SubscribeConsumeAsync(Consumer);
    }
}
