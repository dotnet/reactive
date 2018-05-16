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
        private object guard = new object();
        private Queue<Action> queue;
        private bool isAcquired = false;
        private bool hasFaulted = false;

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
                throw new ArgumentNullException(nameof(action));

            // allow one thread to update the state
            lock (guard)
            {
                // if a previous action crashed, ignore any future actions
                if (hasFaulted)
                {
                    return;
                }

                // if the "lock" is busy, queue up the extra work
                // otherwise there is no need to queue up "action"
                if (isAcquired)
                {
                    // create the queue if necessary
                    var q = queue;
                    if (q == null)
                    {
                        q = new Queue<Action>();
                        queue = q;
                    }
                    // enqueue the work
                    q.Enqueue(action);
                    return;
                }

                // indicate there is processing going on
                isAcquired = true;
            }

            // if we get here, execute the "action" first

            for (; ; )
            {
                try
                {
                    action();
                }
                catch
                {
                    // the execution failed, terminate this AsyncLock
                    lock (guard)
                    {
                        // throw away the queue
                        queue = null;
                        // report fault
                        hasFaulted = true;
                    }
                    throw;
                }

                // execution succeeded, let's see if more work has to be done
                lock (guard)
                {
                    var q = queue;
                    // either there is no queue yet or we run out of work
                    if (q == null || q.Count == 0)
                    {
                        // release the lock
                        isAcquired = false;
                        return;
                    }

                    // get the next work action
                    action = q.Dequeue();
                }
                // loop back and execute the action
            }
        }

        /// <summary>
        /// Clears the work items in the queue and drops further work being queued.
        /// </summary>
        public void Dispose()
        {
            lock (guard)
            {
                queue = null;
                hasFaulted = true;
            }
        }
    }
}
