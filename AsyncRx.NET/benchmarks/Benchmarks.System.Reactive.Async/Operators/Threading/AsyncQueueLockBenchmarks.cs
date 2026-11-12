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
    /// <c>AsyncQueueLock.WaitAsync</c>: the queue-based serialiser used by the <c>AsyncQueueLockAsyncObserver</c>.
    /// Each call runs the action under the lock and drains any actions queued meanwhile.
    /// </summary>
    [BenchmarkCategory("Threading")]
    public class AsyncQueueLockBenchmarks
    {
        [Params(1, 100, 10_000)]
        public int N;

        private static readonly global::System.Func<ValueTask> Nop = static () => default;

        private readonly AsyncQueueLock _lock = new();

        [Benchmark]
        public async ValueTask AsyncQueueLock_WaitAsync()
        {
            for (var i = 0; i < N; i++)
            {
                await _lock.WaitAsync(Nop).ConfigureAwait(false);
            }
        }
    }
}
