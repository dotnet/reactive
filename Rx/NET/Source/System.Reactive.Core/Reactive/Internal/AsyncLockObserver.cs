// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Reactive.Concurrency;

namespace System.Reactive
{
    internal class AsyncLockObserver<T> : ObserverBase<T>
    {
        private readonly AsyncLock _gate;
        private readonly IObserver<T> _observer;

        public AsyncLockObserver(IObserver<T> observer, AsyncLock gate)
        {
            _gate = gate;
            _observer = observer;
        }

        protected override void OnNextCore(T value)
        {
            _gate.Wait(() =>
            {
                _observer.OnNext(value);
            });
        }

        protected override void OnErrorCore(Exception exception)
        {
            _gate.Wait(() =>
            {
                _observer.OnError(exception);
            });
        }

        protected override void OnCompletedCore()
        {
            _gate.Wait(() =>
            {
                _observer.OnCompleted();
            });
        }
    }
}
