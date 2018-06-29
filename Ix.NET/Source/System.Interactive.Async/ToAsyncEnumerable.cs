// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
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
            {
                throw new ArgumentNullException(nameof(source));
            }

            // optimize these adapters for lists and collections
            if (source is IList<TSource> list)
            {
                return new AsyncIListEnumerableAdapter<TSource>(list);
            }

            if (source is ICollection<TSource> collection)
            {
                return new AsyncICollectionEnumerableAdapter<TSource>(collection);
            }

            return new AsyncEnumerableAdapter<TSource>(source);
        }

        public static IAsyncEnumerable<TSource> ToAsyncEnumerable<TSource>(this Task<TSource> task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

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
                        () => { });
                });
        }

        public static IEnumerable<TSource> ToEnumerable<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return ToEnumerable_(source);
        }

        private static IEnumerable<TSource> ToEnumerable_<TSource>(IAsyncEnumerable<TSource> source)
        {
            using (var e = source.GetEnumerator())
            {
                while (true)
                {
                    if (!e.MoveNext(CancellationToken.None)
                          .Result)
                    {
                        break;
                    }

                    var c = e.Current;
                    yield return c;
                }
            }
        }

        internal sealed class AsyncEnumerableAdapter<T> : AsyncIterator<T>, IIListProvider<T>
        {
            private readonly IEnumerable<T> source;

            private IEnumerator<T> enumerator;

            public AsyncEnumerableAdapter(IEnumerable<T> source)
            {
                Debug.Assert(source != null);

                this.source = source;
            }

            public override AsyncIterator<T> Clone()
            {
                return new AsyncEnumerableAdapter<T>(source);
            }

            public override void Dispose()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                }

                base.Dispose();
            }

            protected override Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (enumerator.MoveNext())
                        {
                            current = enumerator.Current;
                            return Task.FromResult(true);
                        }

                        Dispose();
                        break;
                }

                return Task.FromResult(false);
            }

            // These optimizations rely on the Sys.Linq impls from IEnumerable to optimize
            // and short circuit as appropriate
            public Task<T[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(source.ToArray());
            }

            public Task<List<T>> ToListAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(source.ToList());
            }

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                return Task.FromResult(source.Count());
            }
        }

        internal sealed class AsyncIListEnumerableAdapter<T> : AsyncIterator<T>, IIListProvider<T>, IList<T>
        {
            private readonly IList<T> source;
            private IEnumerator<T> enumerator;

            public AsyncIListEnumerableAdapter(IList<T> source)
            {
                Debug.Assert(source != null);

                this.source = source;
            }

            public override AsyncIterator<T> Clone()
            {
                return new AsyncIListEnumerableAdapter<T>(source);
            }

            public override void Dispose()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                }

                base.Dispose();
            }

            protected override Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (enumerator.MoveNext())
                        {
                            current = enumerator.Current;
                            return Task.FromResult(true);
                        }

                        Dispose();
                        break;
                }

                return Task.FromResult(false);
            }

            public override IAsyncEnumerable<TResult> Select<TResult>(Func<T, TResult> selector)
            {
                return new SelectIListIterator<T, TResult>(source, selector);
            }

            // These optimizations rely on the Sys.Linq impls from IEnumerable to optimize
            // and short circuit as appropriate
            public Task<T[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(source.ToArray());
            }

            public Task<List<T>> ToListAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(source.ToList());
            }

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                return Task.FromResult(source.Count);
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => source.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => source.GetEnumerator();

            void ICollection<T>.Add(T item) => source.Add(item);

            void ICollection<T>.Clear() => source.Clear();

            bool ICollection<T>.Contains(T item) => source.Contains(item);

            void ICollection<T>.CopyTo(T[] array, int arrayIndex) => source.CopyTo(array, arrayIndex);

            bool ICollection<T>.Remove(T item) => source.Remove(item);

            int ICollection<T>.Count => source.Count;

            bool ICollection<T>.IsReadOnly => source.IsReadOnly;

            int IList<T>.IndexOf(T item) => source.IndexOf(item);

            void IList<T>.Insert(int index, T item) => source.Insert(index, item);

            void IList<T>.RemoveAt(int index) => source.RemoveAt(index);

            T IList<T>.this[int index]
            {
                get { return source[index]; }
                set { source[index] = value; }
            }
        }

        internal sealed class AsyncICollectionEnumerableAdapter<T> : AsyncIterator<T>, IIListProvider<T>, ICollection<T>
        {
            private readonly ICollection<T> source;
            private IEnumerator<T> enumerator;
            public AsyncICollectionEnumerableAdapter(ICollection<T> source)
            {
                Debug.Assert(source != null);

                this.source = source;
            }

            public override AsyncIterator<T> Clone()
            {
                return new AsyncICollectionEnumerableAdapter<T>(source);
            }

            public override void Dispose()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                }
                base.Dispose();
            }

            protected override Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetEnumerator();
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;
                    case AsyncIteratorState.Iterating:
                        if (enumerator.MoveNext())
                        {
                            current = enumerator.Current;
                            return Task.FromResult(true);
                        }
                        Dispose();
                        break;
                }
                return Task.FromResult(false);
            }

            // These optimizations rely on the Sys.Linq impls from IEnumerable to optimize
            // and short circuit as appropriate
            public Task<T[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(source.ToArray());
            }

            public Task<List<T>> ToListAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(source.ToList());
            }

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                return Task.FromResult(source.Count);
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => source.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => source.GetEnumerator();

            void ICollection<T>.Add(T item) => source.Add(item);

            void ICollection<T>.Clear() => source.Clear();

            bool ICollection<T>.Contains(T item) => source.Contains(item);

            void ICollection<T>.CopyTo(T[] array, int arrayIndex) => source.CopyTo(array, arrayIndex);

            bool ICollection<T>.Remove(T item) => source.Remove(item);

            int ICollection<T>.Count => source.Count;

            bool ICollection<T>.IsReadOnly => source.IsReadOnly;
        }
    }
}
