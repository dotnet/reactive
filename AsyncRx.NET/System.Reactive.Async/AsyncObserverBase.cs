// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive
{
    public abstract class AsyncObserverBase<T> : IAsyncObserver<T>
    {
        private const int Idle = 0;
        private const int Busy = 1;
        private const int Done = 2;

        private int _status = Idle;

        public ValueTask OnCompletedAsync()
        {
            if (!TryEnter())
            {
                return default;
            }

            try
            {
                return OnCompletedAsyncCore();
            }
            finally
            {
                Interlocked.Exchange(ref _status, Done);
            }
        }

        protected abstract ValueTask OnCompletedAsyncCore();

        public ValueTask OnErrorAsync(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            if (!TryEnter())
            {
                return default;
            }

            try
            {
                return OnErrorAsyncCore(error);
            }
            finally
            {
                Interlocked.Exchange(ref _status, Done);
            }
        }

        protected abstract ValueTask OnErrorAsyncCore(Exception error);

        public ValueTask OnNextAsync(T value)
        {
            if (!TryEnter())
            {
                return default;
            }

            try
            {
                return OnNextAsyncCore(value);
            }
            finally
            {
                Interlocked.Exchange(ref _status, Idle);
            }
        }

        protected abstract ValueTask OnNextAsyncCore(T value);

        // Returns false when the observer has already terminated: as in Rx.NET's ObserverBase,
        // a notification after OnError or OnCompleted is ignored rather than an error, so a
        // source that keeps calling after it has terminated (as Create's callback may) is
        // harmless. A concurrent call is still an error.
        private bool TryEnter()
        {
            var old = Interlocked.CompareExchange(ref _status, Busy, Idle);

            switch (old)
            {
                case Busy:
                    throw new InvalidOperationException("The observer is currently processing a notification.");
                case Done:
                    return false;
            }

            return true;
        }
    }
}
