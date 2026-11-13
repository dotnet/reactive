// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

namespace Benchmarks.System.Reactive.Async.Operators.Threading
{
    /// <summary>
    /// Measures the AsyncRx locking primitive directly. <c>AsyncGate</c> is a FIFO lock guarded by a monitor.
    /// <c>AsyncGate_LockAsync</c> is the uncontended path that every gated operator pays per element: a free gate
    /// returns an already-completed <c>ValueTask</c>. <c>AsyncGate_LockAsync_Handoff</c> is the contended path:
    /// a second acquisition queues a <c>TaskCompletionSource</c> waiter, and the holder's release hands the gate
    /// to it inline, with no thread-pool hop. <c>SemaphoreSlim_WaitAsync</c> is the reference: the uncontended
    /// BCL primitive.
    /// </summary>
    /// <remarks>
    /// The gate is not re-entrant (a holder that acquires it again waits for itself), so there is no recursive
    /// row.
    /// </remarks>
    [BenchmarkCategory("Threading")]
    public class AsyncGateBenchmarks
    {
        [Params(1, 100, 10_000)]
        public int N;

        private readonly AsyncGate _gate = new();
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        [Benchmark(Baseline = true)]
        public async ValueTask SemaphoreSlim_WaitAsync()
        {
            for (var i = 0; i < N; i++)
            {
                await _semaphore.WaitAsync().ConfigureAwait(false);
                _semaphore.Release();
            }
        }

        [Benchmark]
        public async ValueTask AsyncGate_LockAsync()
        {
            for (var i = 0; i < N; i++)
            {
                using (await _gate.LockAsync().ConfigureAwait(false))
                {
                }
            }
        }

        [Benchmark]
        public async ValueTask AsyncGate_LockAsync_Handoff()
        {
            for (var i = 0; i < N; i++)
            {
                var held = await _gate.LockAsync().ConfigureAwait(false);
                var pending = _gate.LockAsync();
                held.Dispose();
                (await pending.ConfigureAwait(false)).Dispose();
            }
        }
    }
}
