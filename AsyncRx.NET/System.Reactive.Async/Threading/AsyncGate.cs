// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace System.Threading
{
    public sealed class AsyncGate
    {
        // A FIFO async lock whose waiters are handed the lock directly on release, with their
        // continuations run inline by the releasing thread. This replaced a SemaphoreSlim-based
        // implementation: SemaphoreSlim completes its waiters with RunContinuationsAsynchronously,
        // so every contended acquisition hopped to the thread pool — which made contended
        // operators nondeterministic under a single-threaded virtual-time scheduler and cost a
        // pool hop per contention in production. (An earlier ContinueWith without
        // ExecuteSynchronously hopped even when uncontended.)
        private readonly object _gate = new();
        private readonly Queue<TaskCompletionSource<Releaser>> _waiters = new();
        private readonly AsyncLocal<int> _recursionCount = new();
        private bool _held;

        public ValueTask<Releaser> LockAsync()
        {
            lock (_gate)
            {
                if (_recursionCount.Value > 0)
                {
                    // Re-entrant acquisition on the same logical flow.
                    _recursionCount.Value++;
                    return new ValueTask<Releaser>(new Releaser(this));
                }

                _recursionCount.Value = 1;

                if (!_held)
                {
                    _held = true;
                    return new ValueTask<Releaser>(new Releaser(this));
                }

                var waiter = new TaskCompletionSource<Releaser>();
                _waiters.Enqueue(waiter);
                return new ValueTask<Releaser>(waiter.Task);
            }
        }

        private void Release()
        {
            var next = default(TaskCompletionSource<Releaser>);

            lock (_gate)
            {
                Debug.Assert(_recursionCount.Value > 0);

                if (--_recursionCount.Value == 0)
                {
                    if (_waiters.Count > 0)
                    {
                        // Hand the lock straight to the next waiter; it stays held throughout.
                        next = _waiters.Dequeue();
                    }
                    else
                    {
                        _held = false;
                    }
                }
            }

            // Outside the lock: the waiter's continuation runs inline from here.
            next?.TrySetResult(new Releaser(this));
        }

        public readonly struct Releaser : IDisposable
        {
            private readonly AsyncGate _parent;

            public Releaser(AsyncGate parent) => _parent = parent;

            public void Dispose() => _parent.Release();
        }
    }
}
