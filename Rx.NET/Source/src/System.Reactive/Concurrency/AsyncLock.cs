// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Asynchronous lock.
    /// </summary>
    public sealed class AsyncLock : IDisposable
    {
        private bool _isAcquired;
        private bool _hasFaulted;
        private readonly object _guard = new object();
        private Queue<(Action<Delegate, object> action, Delegate @delegate, object state)> _queue;

        /// <summary>
        /// Queues the action for execution. If the caller acquires the lock and becomes the owner,
        /// the queue is processed. If the lock is already owned, the action is queued and will get
        /// processed by the owner.
        /// </summary>
        /// <param name="action">Action to queue for execution.</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        public void Wait(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            Wait(action, closureAction => closureAction());
        }

        /// <summary>
        /// Queues the action for execution. If the caller acquires the lock and becomes the owner,
        /// the queue is processed. If the lock is already owned, the action is queued and will get
        /// processed by the owner.
        /// </summary>
        /// <param name="action">Action to queue for execution.</param>
        /// <param name="state">The state to pass to the action when it gets invoked under the lock.</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        /// <remarks>In case TState is a value type, this operation will involve boxing of <paramref name="state"/>.
        /// However, this is often an improvement over the allocation of a closure object and a delegate.</remarks>
        internal void Wait<TState>(TState state, Action<TState> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            Wait(state, action, (actionObject, stateObject) => ((Action<TState>)actionObject)((TState)stateObject));
        }

        private void Wait(object state, Delegate @delegate, Action<Delegate, object> action)
        {
            // allow one thread to update the state
            lock (_guard)
            {
                // if a previous action crashed, ignore any future actions
                if (_hasFaulted)
                {
                    return;
                }

                // if the "lock" is busy, queue up the extra work
                // otherwise there is no need to queue up "action"
                if (_isAcquired)
                {
                    // create the queue if necessary
                    var q = _queue;
                    if (q == null)
                    {
                        q = new Queue<(Action<Delegate, object> action, Delegate @delegate, object state)>();
                        _queue = q;
                    }
                    // enqueue the work
                    q.Enqueue((action, @delegate, state));
                    return;
                }

                // indicate there is processing going on
                _isAcquired = true;
            }

            // if we get here, execute the "action" first

            for (; ; )
            {
                try
                {
                    action(@delegate, state);
                }
                catch
                {
                    // the execution failed, terminate this AsyncLock
                    lock (_guard)
                    {
                        // throw away the queue
                        _queue = null;
                        // report fault
                        _hasFaulted = true;
                    }
                    throw;
                }

                // execution succeeded, let's see if more work has to be done
                lock (_guard)
                {
                    var q = _queue;
                    // either there is no queue yet or we run out of work
                    if (q == null || q.Count == 0)
                    {
                        // release the lock
                        _isAcquired = false;
                        return;
                    }

                    // get the next work action
                    (action, @delegate, state) = q.Dequeue();
                }
                // loop back and execute the action
            }
        }

        /// <summary>
        /// Clears the work items in the queue and drops further work being queued.
        /// </summary>
        public void Dispose()
        {
            lock (_guard)
            {
                _queue = null;
                _hasFaulted = true;
            }
        }
    }
}
