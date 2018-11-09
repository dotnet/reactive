// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> ToAsyncEnumerable<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            // optimize these adapters for lists and collections
            if (source is IList<TSource> list)
                return new AsyncIListEnumerableAdapter<TSource>(list);

            if (source is ICollection<TSource> collection)
                return new AsyncICollectionEnumerableAdapter<TSource>(collection);

            return new AsyncEnumerableAdapter<TSource>(source);
        }

        public static IAsyncEnumerable<TSource> ToAsyncEnumerable<TSource>(this Task<TSource> task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            return CreateEnumerable(
                () =>
                {
                    var called = 0;

                    var value = default(TSource);
                    return CreateEnumerator(
                        async ct =>
                        {
                            if (Interlocked.CompareExchange(ref called, 1, 0) == 0)
                            {
                                value = await task.ConfigureAwait(false);
                                return true;
                            }
                            return false;
                        },
                        () => value,
                        () => TaskExt.CompletedTask);
                });
        }

        public static IAsyncEnumerable<TSource> ToAsyncEnumerable<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new ObservableToAsyncEnumerable<TSource>(source);
        }

        /// <summary>
        /// Wraps an IObservable and exposes it as an IAsyncEnumerable, buffering
        /// all source items until they are requested via MoveNextAsync.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source and result.</typeparam>
        private sealed class ObservableToAsyncEnumerable<TSource> : IAsyncEnumerable<TSource>
        {
            private readonly IObservable<TSource> _source;

            public ObservableToAsyncEnumerable(IObservable<TSource> source)
            {
                _source = source;
            }

            public IAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                var observer = new ObserverAsyncEnumerator();
                observer.SetCancellation(cancellationToken, _source.Subscribe(observer));
                return observer;
            }

            private sealed class ObserverAsyncEnumerator : IAsyncEnumerator<TSource>, IObserver<TSource>, IDisposable
            {
                public TSource Current { get; private set; }

                private IDisposable _disposable;

                private CancellationTokenRegistration _tokenReg;

                private ConcurrentQueue<TSource> _queue;
                private Exception _error;
                private volatile bool _done;

                private TaskCompletionSource<bool> _resume;

                internal ObserverAsyncEnumerator()
                {
                    Volatile.Write(ref _queue, new ConcurrentQueue<TSource>());
                }

                internal void SetCancellation(CancellationToken token, IDisposable disposable)
                {
                    if (Interlocked.CompareExchange(ref _disposable, disposable, null) == null)
                    {
                        _tokenReg = token.Register(state => ((ObserverAsyncEnumerator)state).DisposeForToken(), this);
                    }
                    else
                    {
                        disposable.Dispose();
                    }
                }

                private void DisposeForToken()
                {
                    DisposeSource();
                    _tokenReg.Dispose();
                    _tokenReg = default;
                    _queue = null;
                    _error = null;
                }

                public void Dispose()
                {
                    // "this" is the disposed indicator
                }

                public ValueTask DisposeAsync()
                {
                    DisposeForToken();
                    return TaskExt.CompletedTask;
                }

                public async ValueTask<bool> MoveNextAsync()
                {
                    for (; ; )
                    {
                        var isDone = _done;
                        var queue = _queue;
                        if (queue == null)
                        {
                            return false;
                        }
                        var hasItem = queue.TryDequeue(out var item);

                        if (isDone && !hasItem)
                        {
                            var ex = _error;
                            if (ex != null)
                            {
                                throw ex;
                            }
                            return false;
                        }

                        if (hasItem)
                        {
                            Current = item;
                            return true;
                        }

                        await Resume().ConfigureAwait(false);
                        Interlocked.Exchange(ref _resume, null);
                    }
                }

                private Task Resume()
                {
                    var next = default(TaskCompletionSource<bool>);
                    for (; ; )
                    {
                        var current = Volatile.Read(ref _resume);
                        if (current != null)
                        {
                            return current.Task;
                        }
                        if (next == null)
                        {
                            next = new TaskCompletionSource<bool>();
                        }
                        if (Interlocked.CompareExchange(ref _resume, next, null) == null)
                        {
                            return next.Task;
                        }
                    }
                }

                public void OnCompleted()
                {
                    _done = true;
                    DisposeSource();
                    Signal();
                }

                public void OnError(Exception error)
                {
                    _error = error;
                    _done = true;
                    DisposeSource();
                    Signal();
                }

                public void OnNext(TSource value)
                {
                    _queue?.Enqueue(value);

                    Signal();
                }

                private void DisposeSource()
                {
                    var old = Interlocked.Exchange(ref _disposable, this);
                    if (old != this)
                    {
                        old?.Dispose();
                    }
                }

                public void Signal()
                {
                    for (; ; )
                    {
                        var current = Volatile.Read(ref _resume);
                        if (current == TaskExt.ResumeTrue)
                        {
                            break;
                        }
                        if (current != null)
                        {
                            current.TrySetResult(true);
                            break;
                        }
                        if (Interlocked.CompareExchange(ref _resume, TaskExt.ResumeTrue, null) == null)
                        {
                            break;
                        }
                    }
                }
            }
        }

        internal sealed class AsyncEnumerableAdapter<T> : AsyncIterator<T>, IAsyncIListProvider<T>
        {
            private readonly IEnumerable<T> _source;

            private IEnumerator<T> _enumerator;
 
            public AsyncEnumerableAdapter(IEnumerable<T> source)
            {
                Debug.Assert(source != null);

                _source = source;
            }

            public override AsyncIterator<T> Clone()
            {
                return new AsyncEnumerableAdapter<T>(_source);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    _enumerator.Dispose();
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (_enumerator.MoveNext())
                        {
                            current = _enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }

            // These optimizations rely on the Sys.Linq impls from IEnumerable to optimize
            // and short circuit as appropriate
            public Task<T[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_source.ToArray());
            }

            public Task<List<T>> ToListAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_source.ToList());
            }

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                return Task.FromResult(_source.Count());
            }
        }

        internal sealed class AsyncIListEnumerableAdapter<T> : AsyncIterator<T>, IAsyncIListProvider<T>, IList<T>
        {
            private readonly IList<T> _source;
            private IEnumerator<T> _enumerator;

            public AsyncIListEnumerableAdapter(IList<T> source)
            {
                Debug.Assert(source != null);

                _source = source;
            }

            public override AsyncIterator<T> Clone()
            {
                return new AsyncIListEnumerableAdapter<T>(_source);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    _enumerator.Dispose();
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (_enumerator.MoveNext())
                        {
                            current = _enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }

            public override IAsyncEnumerable<TResult> Select<TResult>(Func<T, TResult> selector)
            {
                return new SelectIListIterator<T, TResult>(_source, selector);
            }

            // These optimizations rely on the Sys.Linq impls from IEnumerable to optimize
            // and short circuit as appropriate
            public Task<T[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_source.ToArray());
            }

            public Task<List<T>> ToListAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_source.ToList());
            }

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                return Task.FromResult(_source.Count);
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => _source.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => _source.GetEnumerator();

            void ICollection<T>.Add(T item) => _source.Add(item);

            void ICollection<T>.Clear() => _source.Clear();

            bool ICollection<T>.Contains(T item) => _source.Contains(item);

            void ICollection<T>.CopyTo(T[] array, int arrayIndex) => _source.CopyTo(array, arrayIndex);

            bool ICollection<T>.Remove(T item) => _source.Remove(item);

            int ICollection<T>.Count => _source.Count;

            bool ICollection<T>.IsReadOnly => _source.IsReadOnly;

            int IList<T>.IndexOf(T item) => _source.IndexOf(item);

            void IList<T>.Insert(int index, T item) => _source.Insert(index, item);

            void IList<T>.RemoveAt(int index) => _source.RemoveAt(index);

            T IList<T>.this[int index]
            {
                get { return _source[index]; }
                set { _source[index] = value; }
            }
        }

        internal sealed class AsyncICollectionEnumerableAdapter<T> : AsyncIterator<T>, IAsyncIListProvider<T>, ICollection<T>
        {
            private readonly ICollection<T> _source;
            private IEnumerator<T> _enumerator;

            public AsyncICollectionEnumerableAdapter(ICollection<T> source)
            {
                Debug.Assert(source != null);

                _source = source;
            }

            public override AsyncIterator<T> Clone()
            {
                return new AsyncICollectionEnumerableAdapter<T>(_source);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    _enumerator.Dispose();
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (_enumerator.MoveNext())
                        {
                            current = _enumerator.Current;
                            return true;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }

            // These optimizations rely on the Sys.Linq impls from IEnumerable to optimize
            // and short circuit as appropriate
            public Task<T[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_source.ToArray());
            }

            public Task<List<T>> ToListAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_source.ToList());
            }

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                return Task.FromResult(_source.Count);
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => _source.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => _source.GetEnumerator();

            void ICollection<T>.Add(T item) => _source.Add(item);

            void ICollection<T>.Clear() => _source.Clear();

            bool ICollection<T>.Contains(T item) => _source.Contains(item);

            void ICollection<T>.CopyTo(T[] array, int arrayIndex) => _source.CopyTo(array, arrayIndex);

            bool ICollection<T>.Remove(T item) => _source.Remove(item);

            int ICollection<T>.Count => _source.Count;

            bool ICollection<T>.IsReadOnly => _source.IsReadOnly;
        }
    }
}
