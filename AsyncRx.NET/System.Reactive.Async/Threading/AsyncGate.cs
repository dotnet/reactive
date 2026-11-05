// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
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
        //
        // The lock is not re-entrant. An earlier form admitted a flow that already held it, by an
        // AsyncLocal count; but ExecutionContext flows into every scheduled or subscribed piece
        // of work, so work started while a flow held the lock inherited the count. Since
        // AsyncLocal values get copied into the ExecutionContext, the effect was that when the
        // parent flow later released the lock, dropping the count to zero, any child flows
        // would continue to have their non-zero copies of the count, meaning that they would
        // mistakenly think they still held the lock, so attempts to acquire it would be
        // misinterpreted as a re-entrant acquisition. That made it possible for one of these child
        // flows to think they had acquired the lock while the gate was free or held by someone
        // else. Across the test suite 86 of 90 cases where the gate believed it was seeing
        // re-entrant acquisition occurred when the gate had in fact been freed. (The four genuine
        // re-entries were one helper in Join. That was the one place where we truly relied on
        // re-entrant locking, and that code has since been restructured.) The downside is that a 
        // flow that holds the lock and acquires it again now waits for itself, causing deadlock.
        // But this previous behaviour was worse: state could be corrupted silently because
        // child flows (e.g. any scheduled work) could incorrectly think they held the lock.
        private readonly object _gate = new();
        private readonly Queue<TaskCompletionSource<Releaser>> _waiters = new();
        private bool _held;

        public ValueTask<Releaser> LockAsync()
        {
            lock (_gate)
            {
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
