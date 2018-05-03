// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
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

        public Task OnCompletedAsync()
        {
            TryEnter();

            try
            {
                return OnCompletedAsyncCore();
            }
            finally
            {
                Interlocked.Exchange(ref _status, Done);
            }
        }

        protected abstract Task OnCompletedAsyncCore();

        public Task OnErrorAsync(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            TryEnter();

            try
            {
                return OnErrorAsyncCore(error);
            }
            finally
            {
                Interlocked.Exchange(ref _status, Done);
            }
        }

        protected abstract Task OnErrorAsyncCore(Exception error);

        public Task OnNextAsync(T value)
        {
            TryEnter();

            try
            {
                return OnNextAsyncCore(value);
            }
            finally
            {
                Interlocked.Exchange(ref _status, Idle);
            }
        }

        protected abstract Task OnNextAsyncCore(T value);

        private void TryEnter()
        {
            var old = Interlocked.CompareExchange(ref _status, Busy, Idle);

            switch (old)
            {
                case Busy:
                    throw new InvalidOperationException("The observer is currently processing a notification.");
                case Done:
                    throw new InvalidOperationException("The observer has already terminated.");
            }
        }
    }
}
