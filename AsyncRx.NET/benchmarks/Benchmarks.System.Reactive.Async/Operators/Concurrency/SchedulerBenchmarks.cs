// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive.Async.Operators.Concurrency
{
    /// <summary>
    /// The raw scheduler primitives, N calls in sequence: <c>ScheduleAsync</c> on the immediate scheduler (runs the
    /// action inline), <c>ExecuteAsync</c> (schedule and await completion) on the immediate and task-pool
    /// schedulers, and <c>Delay(TimeSpan.Zero)</c> on the task pool.
    /// </summary>
    [BenchmarkCategory("Concurrency")]
    public class SchedulerBenchmarks
    {
        private static readonly Func<CancellationToken, ValueTask> Nop = static _ => default;

        [Params(1, 100, 10_000)]
        public int N;

        [Benchmark(Baseline = true)]
        public async ValueTask Immediate_ScheduleAsync()
        {
            for (var i = 0; i < N; i++)
            {
                await ImmediateAsyncScheduler.Instance.ScheduleAsync(Nop).ConfigureAwait(false);
            }
        }

        [Benchmark]
        public async ValueTask Immediate_ExecuteAsync()
        {
            for (var i = 0; i < N; i++)
            {
                await ImmediateAsyncScheduler.Instance.ExecuteAsync(Nop).ConfigureAwait(false);
            }
        }

        [Benchmark]
        public async ValueTask TaskPool_ExecuteAsync()
        {
            for (var i = 0; i < N; i++)
            {
                await TaskPoolAsyncScheduler.Default.ExecuteAsync(Nop).ConfigureAwait(false);
            }
        }

        [Benchmark]
        public async ValueTask TaskPool_DelayZero()
        {
            for (var i = 0; i < N; i++)
            {
                await TaskPoolAsyncScheduler.Default.Delay(TimeSpan.Zero).ConfigureAwait(false);
            }
        }
    }
}
