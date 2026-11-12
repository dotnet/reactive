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
    /// Measures the AsyncRx locking primitives directly. <c>AsyncGate.LockAsync</c> acquires its semaphore
    /// through <c>SemaphoreSlim.WaitAsync().ContinueWith(...)</c>, which queues to the thread pool even when the
    /// gate is free; these rows quantify that cost, which every gated operator pays per element. A recursive
    /// acquisition takes the fast path (no semaphore). <c>SemaphoreSlim_WaitAsync</c> is the reference: the
    /// uncontended primitive without the continuation.
    /// </summary>
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
        public async ValueTask AsyncGate_LockAsync_Recursive()
        {
            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                for (var i = 0; i < N; i++)
                {
                    using (await _gate.LockAsync().ConfigureAwait(false))
                    {
                    }
                }
            }
        }
    }
}
