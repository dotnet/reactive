// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Subjects
{
    public abstract class AsyncAsyncSubject<T> : IAsyncSubject<T>, INotifyCompletion
    {
        private readonly object _gate = new();
        private readonly List<IAsyncObserver<T>> _observers = new();
        private bool _hasValue;
        private T _value;
        private bool _done;
        private Exception _error;

        public async ValueTask OnCompletedAsync()
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
            lock (_gate)
            {
                if (!_done && _error == null)
                {
                    _hasValue = true;
                    _value = value;
                }
            }

            return default;
        }

        protected abstract ValueTask OnNextAsyncCore(IEnumerable<IAsyncObserver<T>> observers, T value);

        public async ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer)
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

                return default;
            });
        }

        public AsyncAsyncSubject<T> GetAwaiter() => this;

        public bool IsCompleted => _done || _error != null;

        public T GetResult()
        {
            if (!IsCompleted)
            {
                var e = new ManualResetEventSlim(initialState: false);

                OnCompleted(() => { e.Set(); }, originalContext: false);

                e.Wait();
            }

            if (_error != null)
            {
                ExceptionDispatchInfo.Capture(_error).Throw();
            }

            if (!_hasValue)
            {
                throw new InvalidOperationException("The subject has no value.");
            }

            return _value;
        }

        public void OnCompleted(Action continuation)
        {
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));

            OnCompleted(continuation, originalContext: true);
        }

        private void OnCompleted(Action continuation, bool originalContext)
        {
            var subscribeTask = SubscribeAsync(new AwaitObserver(continuation, originalContext));

            subscribeTask.AsTask().ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    // TODO: Trace?
                }
            });
        }

        private sealed class AwaitObserver : IAsyncObserver<T>
        {
            private readonly Action _continuation;
            private readonly SynchronizationContext _context;

            public AwaitObserver(Action continuation, bool originalContext)
            {
                _continuation = continuation;

                if (originalContext)
                {
                    _context = SynchronizationContext.Current;
                }
            }

            public ValueTask OnCompletedAsync() => InvokeAsync();

            public ValueTask OnErrorAsync(Exception error) => InvokeAsync();

            public ValueTask OnNextAsync(T value) => default;

            private ValueTask InvokeAsync()
            {
                if (_context != null)
                {
                    _context.Post(c => ((Action)c)(), _continuation);
                }
                else
                {
                    _continuation();
                }

                return default;
            }
        }
    }
}
