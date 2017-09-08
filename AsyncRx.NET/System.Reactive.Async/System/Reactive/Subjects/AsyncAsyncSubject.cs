// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Subjects
{
    public abstract class AsyncAsyncSubject<T> : IAsyncSubject<T>
    {
        private readonly object _gate = new object();
        private readonly List<IAsyncObserver<T>> _observers = new List<IAsyncObserver<T>>();
        private bool _hasValue;
        private T _value;
        private bool _done;
        private Exception _error;

        public async Task OnCompletedAsync()
        {
            bool hasValue;
            T value;

            IAsyncObserver<T>[] observers;

            lock (_gate)
            {
                if (_done || _error != null)
                {
                    return;
                }

                _done = true;

                hasValue = _hasValue;
                value = _value;

                observers = _observers.ToArray();
            }

            if (hasValue)
            {
                await OnNextAsyncCore(observers, value).ConfigureAwait(false);
            }

            await OnCompletedAsyncCore(observers).ConfigureAwait(false);
        }

        protected abstract Task OnCompletedAsyncCore(IEnumerable<IAsyncObserver<T>> observers);

        public Task OnErrorAsync(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            IAsyncObserver<T>[] observers;

            lock (_gate)
            {
                if (_done || _error != null)
                {
                    return Task.CompletedTask;
                }

                _error = error;

                observers = _observers.ToArray();
            }

            return OnErrorAsyncCore(observers, error);
        }

        protected abstract Task OnErrorAsyncCore(IEnumerable<IAsyncObserver<T>> observers, Exception error);

        public Task OnNextAsync(T value)
        {
            lock (_gate)
            {
                if (!_done && _error == null)
                {
                    _hasValue = true;
                    _value = value;
                }
            }

            return Task.CompletedTask;
        }

        protected abstract Task OnNextAsyncCore(IEnumerable<IAsyncObserver<T>> observers, T value);

        public async Task<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            bool hasValue;
            T value;
            bool done;
            Exception error;

            lock (_gate)
            {
                done = _done;
                error = _error;

                hasValue = _hasValue;
                value = _value;

                if (!done && error == null)
                {
                    _observers.Add(observer);
                }
            }

            if (error != null)
            {
                await observer.OnErrorAsync(error).ConfigureAwait(false);

                return AsyncDisposable.Nop;
            }
            else if (done)
            {
                if (hasValue)
                {
                    await observer.OnNextAsync(value).ConfigureAwait(false);
                }

                await observer.OnCompletedAsync().ConfigureAwait(false);

                return AsyncDisposable.Nop;
            }

            return AsyncDisposable.Create(() =>
            {
                lock (_gate)
                {
                    var i = _observers.LastIndexOf(observer);

                    if (i >= 0)
                    {
                        _observers.RemoveAt(i);
                    }
                }

                return Task.CompletedTask;
            });
        }
    }
}
