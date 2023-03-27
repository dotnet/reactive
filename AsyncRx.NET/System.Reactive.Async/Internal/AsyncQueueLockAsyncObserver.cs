// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive
{
    internal sealed class AsyncQueueLockAsyncObserver<T> : AsyncObserverBase<T>
    {
        private readonly AsyncQueueLock _gate = new();
        private readonly IAsyncObserver<T> _observer;

        public AsyncQueueLockAsyncObserver(IAsyncObserver<T> observer)
        {
            _observer = observer;
        }

        protected override ValueTask OnCompletedAsyncCore() => _gate.WaitAsync(_observer.OnCompletedAsync);

        protected override ValueTask OnErrorAsyncCore(Exception error) => _gate.WaitAsync(() => _observer.OnErrorAsync(error));

        protected override ValueTask OnNextAsyncCore(T value) => _gate.WaitAsync(() => _observer.OnNextAsync(value));
    }
}
