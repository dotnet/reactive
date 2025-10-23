﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Subjects
{
    public abstract class BehaviorAsyncSubject<T> : IAsyncSubject<T>
    {
        private readonly IAsyncGate _gate = AsyncGate.Create();
        private readonly List<IAsyncObserver<T>> _observers = new();
        private T _value;
        private bool _done;
        private Exception _error;

        public BehaviorAsyncSubject(T value)
        {
            _value = value;
        }

        public T Value
        {
            get
            {
                var error = _error;

                if (error != null)
                {
                    throw error;
                }

                return _value;
            }
        }

        public bool TryGetValue(out T value)
        {
            // TODO: support for disposal

            var error = _error;

            if (error != null)
            {
                throw error;
            }

            value = _value;
            return true;
        }

        public async ValueTask OnCompletedAsync()
        {
            IAsyncObserver<T>[] observers;

            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                if (_done || _error != null)
                {
                    return;
                }

                _done = true;

                observers = _observers.ToArray();
            }

            await OnCompletedAsyncCore(observers).ConfigureAwait(false);
        }

        protected abstract ValueTask OnCompletedAsyncCore(IEnumerable<IAsyncObserver<T>> observers);

        public async ValueTask OnErrorAsync(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            IAsyncObserver<T>[] observers;

            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                if (_done || _error != null)
                {
                    return;
                }

                _error = error;

                observers = _observers.ToArray();
            }

            await OnErrorAsyncCore(observers, error).ConfigureAwait(false);
        }

        protected abstract ValueTask OnErrorAsyncCore(IEnumerable<IAsyncObserver<T>> observers, Exception error);

        public async ValueTask OnNextAsync(T value)
        {
            IAsyncObserver<T>[] observers;

            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                if (_done || _error != null)
                {
                    return;
                }

                _value = value;

                observers = _observers.ToArray();
            }

            await OnNextAsyncCore(observers, value).ConfigureAwait(false);
        }

        protected abstract ValueTask OnNextAsyncCore(IEnumerable<IAsyncObserver<T>> observers, T value);

        public async ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            bool done;
            Exception error;

            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                done = _done;
                error = _error;

                if (!done && error == null)
                {
                    _observers.Add(observer);

                    await observer.OnNextAsync(_value).ConfigureAwait(false);
                }
            }

            if (error != null)
            {
                await observer.OnErrorAsync(error).ConfigureAwait(false);

                return AsyncDisposable.Nop;
            }
            else if (done)
            {
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

                return default;
            });
        }
    }
}
