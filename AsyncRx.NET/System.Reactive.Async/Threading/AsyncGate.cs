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
        // continuations run inline by the releasing thread. A SemaphoreSlim would not do here:
        // it completes its waiters with RunContinuationsAsynchronously, so every contended
        // acquisition would hop to the thread pool, which costs a pool hop per contention and
        // makes contended operators nondeterministic under a single-threaded virtual-time
        // scheduler.
        //
        // The lock is not re-entrant: a flow that holds it and acquires it again waits for
        // itself. Re-entrancy cannot be tracked reliably here. The only handle on a flow is an
        // AsyncLocal, and ExecutionContext flows into every piece of work scheduled or
        // subscribed while the lock is held, so each such child flow would inherit a copy of
        // the holder's count; when the parent later released the lock, the children would keep
        // their copies and take their next acquisition for a re-entrant one, entering a gate
        // that was free or held by someone else. A deadlock on genuine re-entry is loud; a lock
        // that admits a stranger corrupts state silently.
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
