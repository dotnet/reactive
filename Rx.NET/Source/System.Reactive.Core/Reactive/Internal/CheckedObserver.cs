// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Threading;

namespace System.Reactive
{
    internal class CheckedObserver<T> : IObserver<T>
    {
        private readonly IObserver<T> _observer;
        private int _state;

        private const int IDLE = 0;
        private const int BUSY = 1;
        private const int DONE = 2;

        public CheckedObserver(IObserver<T> observer)
        {
            _observer = observer;
        }

        public void OnNext(T value)
        {
            CheckAccess();

            try
            {
                _observer.OnNext(value);
            }
            finally
            {
                Interlocked.Exchange(ref _state, IDLE);
            }
        }

        public void OnError(Exception error)
        {
            CheckAccess();

            try
            {
                _observer.OnError(error);
            }
            finally
            {
                Interlocked.Exchange(ref _state, DONE);
            }
        }

        public void OnCompleted()
        {
            CheckAccess();

            try
            {
                _observer.OnCompleted();
            }
            finally
            {
                Interlocked.Exchange(ref _state, DONE);
            }
        }

        private void CheckAccess()
        {
            switch (Interlocked.CompareExchange(ref _state, BUSY, IDLE))
            {
                case BUSY:
                    throw new InvalidOperationException(Strings_Core.REENTRANCY_DETECTED);
                case DONE:
                    throw new InvalidOperationException(Strings_Core.OBSERVER_TERMINATED);
            }
        }
    }
}
