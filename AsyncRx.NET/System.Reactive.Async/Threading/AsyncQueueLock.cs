// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Threading
{
    // A serializing work queue, as Rx.NET's AsyncLock: an action handed in while none is running
    // runs at once, on the caller, which then drains whatever arrives meanwhile; an action
    // handed in while one is running is queued for that drainer, and its caller returns at
    // once. So a notification delivered re-entrantly, from inside the delivery of another, runs
    // after it rather than inside it. An action that throws faults the queue: the exception
    // reaches the drainer's caller, and everything queued or handed in afterwards is dropped.
    // The guard is a monitor, since nothing is awaited while it is held.
    public sealed class AsyncQueueLock : IAsyncDisposable
    {
        private readonly object _guard = new();
        private readonly Queue<Func<ValueTask>> _queue = new();
        private bool _isAcquired;
        private bool _hasFaulted;

        public async ValueTask WaitAsync(Func<ValueTask> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            lock (_guard)
            {
                if (_hasFaulted)
                {
                    return;
                }

                if (_isAcquired)
                {
                    _queue.Enqueue(action);
                    return;
                }

                _isAcquired = true;
            }

            while (true)
            {
                try
                {
                    await action().ConfigureAwait(false);
                }
                catch
                {
                    lock (_guard)
                    {
                        _queue.Clear();
                        _hasFaulted = true;
                    }

                    throw;
                }

                lock (_guard)
                {
                    if (_queue.Count == 0)
                    {
                        _isAcquired = false;
                        return;
                    }

                    action = _queue.Dequeue();
                }
            }
        }

        public ValueTask DisposeAsync()
        {
            lock (_guard)
            {
                _queue.Clear();
                _hasFaulted = true;
            }

            return default;
        }
    }
}
