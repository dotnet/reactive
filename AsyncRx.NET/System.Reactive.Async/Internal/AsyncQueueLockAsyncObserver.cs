// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive
{
    // The queue lock is this observer's serialization: a notification that arrives while another
    // is being delivered (re-entrantly, as when a GroupByUntil group's duration is derived from
    // the group itself and expires it from inside its own OnNextAsync) is queued and delivered
    // after the current one, as Rx.NET's AsyncLockObserver does. So this does not derive from
    // AsyncObserverBase, whose guard would reject that re-entrant call before the lock saw it;
    // only its stopped check is kept, so a notification after a terminal one is dropped.
    internal sealed class AsyncQueueLockAsyncObserver<T> : IAsyncObserver<T>
    {
        private readonly AsyncQueueLock _gate = new();
        private readonly IAsyncObserver<T> _observer;

        private int _isStopped;

        public AsyncQueueLockAsyncObserver(IAsyncObserver<T> observer)
        {
            _observer = observer;
        }

        public ValueTask OnCompletedAsync()
        {
            if (Interlocked.Exchange(ref _isStopped, 1) == 0)
            {
                return _gate.WaitAsync(_observer.OnCompletedAsync);
            }

            return default;
        }

        public ValueTask OnErrorAsync(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            if (Interlocked.Exchange(ref _isStopped, 1) == 0)
            {
                return _gate.WaitAsync(() => _observer.OnErrorAsync(error));
            }

            return default;
        }

        public ValueTask OnNextAsync(T value)
        {
            if (Volatile.Read(ref _isStopped) == 0)
            {
                return _gate.WaitAsync(() => _observer.OnNextAsync(value));
            }

            return default;
        }
    }
}
