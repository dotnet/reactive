// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive
{
    internal sealed class AsyncQueueLockAsyncObserver<T> : AsyncObserverBase<T>
    {
        private readonly AsyncQueueLock _gate = new AsyncQueueLock();
        private readonly IAsyncObserver<T> _observer;

        public AsyncQueueLockAsyncObserver(IAsyncObserver<T> observer)
        {
            _observer = observer;
        }

        protected override Task OnCompletedAsyncCore() => _gate.WaitAsync(_observer.OnCompletedAsync);

        protected override Task OnErrorAsyncCore(Exception error) => _gate.WaitAsync(() => _observer.OnErrorAsync(error));

        protected override Task OnNextAsyncCore(T value) => _gate.WaitAsync(() => _observer.OnNextAsync(value));
    }
}
