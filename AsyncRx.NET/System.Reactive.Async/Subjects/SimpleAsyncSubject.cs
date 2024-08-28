// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Subjects
{
    public abstract class SimpleAsyncSubject<T> : IAsyncSubject<T>
    {
        private readonly object _gate = new();
        private readonly List<IAsyncObserver<T>> _observers = new();
        private bool _done;
        private Exception _error;

        public ValueTask OnCompletedAsync()
        {
            IAsyncObserver<T>[] observers;

            lock (_gate)
            {
                if (_done || _error != null)
                {
                    return default;
                }

                _done = true;

                observers = _observers.ToArray();
            }

            return OnCompletedAsyncCore(observers);
        }

        protected abstract ValueTask OnCompletedAsyncCore(IEnumerable<IAsyncObserver<T>> observers);

        public ValueTask OnErrorAsync(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            IAsyncObserver<T>[] observers;

            lock (_gate)
            {
                if (_done || _error != null)
                {
                    return default;
                }

                _error = error;

                observers = _observers.ToArray();
            }

            return OnErrorAsyncCore(observers, error);
        }

        protected abstract ValueTask OnErrorAsyncCore(IEnumerable<IAsyncObserver<T>> observers, Exception error);

        public ValueTask OnNextAsync(T value)
        {
            IAsyncObserver<T>[] observers;

            lock (_gate)
            {
                if (_done || _error != null)
                {
                    return default;
                }

                observers = _observers.ToArray();
            }

            return OnNextAsyncCore(observers, value);
        }

        protected abstract ValueTask OnNextAsyncCore(IEnumerable<IAsyncObserver<T>> observers, T value);

        public async ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            bool done;
            Exception error;

            lock (_gate)
            {
                done = _done;
                error = _error;

                if (!done && error == null)
                {
                    _observers.Add(observer);
                }
            }

            if (done)
            {
                await observer.OnCompletedAsync().ConfigureAwait(false);

                return AsyncDisposable.Nop;
            }
            else if (error != null)
            {
                await observer.OnErrorAsync(error).ConfigureAwait(false);

                return AsyncDisposable.Nop;
            }
            else
            {
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

                    return default;
                });
            }
        }
    }
}
