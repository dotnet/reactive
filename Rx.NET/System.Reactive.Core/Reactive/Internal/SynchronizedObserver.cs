// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Reactive
{
    internal class SynchronizedObserver<T> : ObserverBase<T>
    {
        private readonly object _gate;
        private readonly IObserver<T> _observer;

        public SynchronizedObserver(IObserver<T> observer, object gate)
        {
            _gate = gate;
            _observer = observer;
        }

        protected override void OnNextCore(T value)
        {
            lock (_gate)
            {
                _observer.OnNext(value);
            }
        }

        protected override void OnErrorCore(Exception exception)
        {
            lock (_gate)
            {
                _observer.OnError(exception);
            }
        }

        protected override void OnCompletedCore()
        {
            lock (_gate)
            {
                _observer.OnCompleted();
            }
        }
    }
}
