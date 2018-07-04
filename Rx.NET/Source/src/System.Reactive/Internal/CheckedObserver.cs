// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;

namespace System.Reactive
{
    internal sealed class CheckedObserver<T> : IObserver<T>
    {
        private readonly IObserver<T> _observer;
        private int _state;

        private const int Idle = 0;
        private const int Busy = 1;
        private const int Done = 2;

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
                Interlocked.Exchange(ref _state, Idle);
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
                Interlocked.Exchange(ref _state, Done);
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
                Interlocked.Exchange(ref _state, Done);
            }
        }

        private void CheckAccess()
        {
            switch (Interlocked.CompareExchange(ref _state, Busy, Idle))
            {
                case Busy:
                    throw new InvalidOperationException(Strings_Core.REENTRANCY_DETECTED);
                case Done:
                    throw new InvalidOperationException(Strings_Core.OBSERVER_TERMINATED);
            }
        }
    }
}
