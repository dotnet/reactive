// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

using Benchmarks.System.Reactive.Async.Infrastructure;

namespace Benchmarks.System.Reactive.Async.Operators.Concurrency
{
    /// <summary>
    /// The scheduler primitives that <c>SchedulerBenchmarks</c> does not cover, N calls in sequence: the
    /// state-passing <c>IAsyncScheduler.ScheduleAsync&lt;TState&gt;</c> interface method, the state and result
    /// forms of <c>ExecuteAsync</c> on the immediate and task-pool schedulers, the relative-due-time
    /// <c>ScheduleAsync</c> forms on the task pool, and <c>Delay(DateTimeOffset)</c> on the task pool.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The schedulers are held in fields typed <see cref="IAsyncScheduler"/>. On the concrete types,
    /// <c>ScheduleAsync(state, action)</c> would bind the <c>AsyncSchedulerBase</c> virtual method instead of
    /// the interface method. All delegates are static and cached.
    /// </para>
    /// <para>
    /// <c>ScheduleAsync</c> does not wait for the action to run, so the two <c>TaskPool_ScheduleAsync_*</c> rows
    /// let the action release a <see cref="SemaphoreSlim"/> and await it. Those rows measure scheduling plus the
    /// hop to a pool thread and back; when the action has not run by the time the row waits, the wait allocates
    /// one task.
    /// </para>
    /// <para>
    /// The shared <see cref="CompletionSignal"/> cannot be used here. <c>Set</c> publishes <c>IsSet</c> before it
    /// completes the underlying value-task source, so when <c>Set</c> runs on a pool thread, <c>WaitAsync</c> can
    /// return early and the next iteration's <c>Reset</c> races the pool thread's <c>SetResult</c>. At N = 10,000
    /// that fails with <c>InvalidOperationException</c> from <c>GetResult</c>.
    /// </para>
    /// </remarks>
    [BenchmarkCategory("Concurrency")]
    public class SchedulerStateBenchmarks
    {
        private static readonly Func<int, CancellationToken, ValueTask> NopState = static (_, _) => default;
        private static readonly Func<CancellationToken, ValueTask<int>> Result = static _ => new ValueTask<int>(42);
        private static readonly Func<int, CancellationToken, ValueTask<int>> StateResult = static (state, _) => new ValueTask<int>(state);
        private static readonly Func<SemaphoreSlim, CancellationToken, ValueTask> ReleaseState = static (done, _) =>
        {
            done.Release();
            return default;
        };

        private readonly IAsyncScheduler _immediate = ImmediateAsyncScheduler.Instance;
        private readonly IAsyncScheduler _taskPool = TaskPoolAsyncScheduler.Default;
        private readonly SemaphoreSlim _done = new(0);
        private readonly Consumer _consumer = new();
        private readonly Func<CancellationToken, ValueTask> _release;

        [Params(1, 100, 10_000)]
        public int N;

        public SchedulerStateBenchmarks()
        {
            var done = _done;
            _release = _ =>
            {
                done.Release();
                return default;
            };
        }

        [GlobalCleanup]
        public void Cleanup() => _done.Dispose();

        [Benchmark]
        public async ValueTask Immediate_ScheduleAsync_State()
        {
            for (var i = 0; i < N; i++)
            {
                await _immediate.ScheduleAsync(i, NopState).ConfigureAwait(false);
            }
        }

        [Benchmark]
        public async ValueTask Immediate_ExecuteAsync_State()
        {
            for (var i = 0; i < N; i++)
            {
                await _immediate.ExecuteAsync(i, NopState).ConfigureAwait(false);
            }
        }

        [Benchmark]
        public async ValueTask Immediate_ExecuteAsync_Result()
        {
            for (var i = 0; i < N; i++)
            {
                _consumer.Consume(await _immediate.ExecuteAsync(Result).ConfigureAwait(false));
            }
        }

        [Benchmark]
        public async ValueTask Immediate_ExecuteAsync_State_Result()
        {
            for (var i = 0; i < N; i++)
            {
                _consumer.Consume(await _immediate.ExecuteAsync(i, StateResult).ConfigureAwait(false));
            }
        }

        [Benchmark]
        public async ValueTask TaskPool_ExecuteAsync_Result()
        {
            for (var i = 0; i < N; i++)
            {
                _consumer.Consume(await _taskPool.ExecuteAsync(Result).ConfigureAwait(false));
            }
        }

        [Benchmark]
        public async ValueTask TaskPool_ScheduleAsync_DueTime()
        {
            for (var i = 0; i < N; i++)
            {
                await _taskPool.ScheduleAsync(_release, TimeSpan.Zero).ConfigureAwait(false);
                await _done.WaitAsync().ConfigureAwait(false);
            }
        }

        [Benchmark]
        public async ValueTask TaskPool_ScheduleAsync_State_DueTime()
        {
            for (var i = 0; i < N; i++)
            {
                await _taskPool.ScheduleAsync(_done, TimeSpan.Zero, ReleaseState).ConfigureAwait(false);
                await _done.WaitAsync().ConfigureAwait(false);
            }
        }

        [Benchmark]
        public async ValueTask TaskPool_Delay_DateTimeOffset()
        {
            for (var i = 0; i < N; i++)
            {
                await _taskPool.Delay(_taskPool.Now).ConfigureAwait(false);
            }
        }
    }
}
