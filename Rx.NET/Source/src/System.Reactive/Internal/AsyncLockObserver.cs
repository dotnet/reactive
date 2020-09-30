// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

namespace System.Reactive
{
    internal sealed class AsyncLockObserver<T> : ObserverBase<T>
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
            _gate.Wait(
                (_observer, value),
                static tuple => tuple._observer.OnNext(tuple.value));
        }

        protected override void OnErrorCore(Exception exception)
        {
            _gate.Wait(
                (_observer, exception),
                static tuple => tuple._observer.OnError(tuple.exception));
        }

        protected override void OnCompletedCore()
        {
            _gate.Wait(
                _observer,
                static closureObserver => closureObserver.OnCompleted());
        }
    }
}
