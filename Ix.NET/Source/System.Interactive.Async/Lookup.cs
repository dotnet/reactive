// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return ToLookup(source, keySelector, elementSelector, comparer, CancellationToken.None);
        }

        public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return ToLookup(source, keySelector, elementSelector, CancellationToken.None);
        }

        public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return ToLookup(source, keySelector, comparer, CancellationToken.None);
        }

        public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return ToLookup(source, keySelector, CancellationToken.None);
        }

        public static async Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            var lookup = await Internal.Lookup<TKey, TElement>.CreateAsync(source, keySelector, elementSelector, comparer, cancellationToken)
                                       .ConfigureAwait(false);

            return lookup;
        }

        public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.ToLookup(keySelector, elementSelector, EqualityComparer<TKey>.Default, cancellationToken);
        }

        public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.ToLookup(keySelector, x => x, comparer, cancellationToken);
        }

        public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.ToLookup(keySelector, x => x, EqualityComparer<TKey>.Default, cancellationToken);
        }
    }
}

// This is internal because System.Linq exposes a public Lookup that we cannot directly use here

namespace System.Linq.Internal
{
    internal class Lookup<TKey, TElement> : ILookup<TKey, TElement>, IIListProvider<IGrouping<TKey, TElement>>, IIListProvider<IAsyncGrouping<TKey, TElement>>
    {
        private readonly IEqualityComparer<TKey> _comparer;
        private Grouping<TKey, TElement>[] _groupings;
        private Grouping<TKey, TElement> _lastGrouping;

        private Lookup(IEqualityComparer<TKey> comparer)
        {
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
            _groupings = new Grouping<TKey, TElement>[7];
        }

        public int Count { get; private set; }

        public IEnumerable<TElement> this[TKey key]
        {
            get
            {
                var grouping = GetGrouping(key, create: false);
                if (grouping != null)
                {
                    return grouping;
                }

#if NO_ARRAY_EMPTY
                return EmptyArray<TElement>.Value;
#else
                return Array.Empty<TElement>();
#endif
            }
        }

        public bool Contains(TKey key)
        {
            return GetGrouping(key, create: false) != null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            var g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    yield return g;
                } while (g != _lastGrouping);
            }
        }

        public IEnumerable<TResult> ApplyResultSelector<TResult>(Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
        {
            var g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    g.Trim();
                    yield return resultSelector(g._key, g._elements);
                } while (g != _lastGrouping);
            }
        }

        internal static Lookup<TKey, TElement> Create<TSource>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            Debug.Assert(source != null);
            Debug.Assert(keySelector != null);
            Debug.Assert(elementSelector != null);

            var lookup = new Lookup<TKey, TElement>(comparer);
            foreach (var item in source)
            {
                lookup.GetGrouping(keySelector(item), create: true)
                      .Add(elementSelector(item));
            }

            return lookup;
        }

        internal static Lookup<TKey, TElement> Create(IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            Debug.Assert(source != null);
            Debug.Assert(keySelector != null);

            var lookup = new Lookup<TKey, TElement>(comparer);
            foreach (var item in source)
            {
                lookup.GetGrouping(keySelector(item), create: true)
                      .Add(item);
            }

            return lookup;
        }

        internal static async Task<Lookup<TKey, TElement>> CreateAsync<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            Debug.Assert(source != null);
            Debug.Assert(keySelector != null);
            Debug.Assert(elementSelector != null);

            var lookup = new Lookup<TKey, TElement>(comparer);
            using (var enu = source.GetEnumerator())
            {
                while (await enu.MoveNext(cancellationToken)
                                .ConfigureAwait(false))
                {
                    lookup.GetGrouping(keySelector(enu.Current), create: true)
                          .Add(elementSelector(enu.Current));
                }
            }

            return lookup;
        }

        internal static async Task<Lookup<TKey, TElement>> CreateAsync(IAsyncEnumerable<TElement> source, Func<TElement, TKey> keySelector,  IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            Debug.Assert(source != null);
            Debug.Assert(keySelector != null);

            var lookup = new Lookup<TKey, TElement>(comparer);
            using (var enu = source.GetEnumerator())
            {
                while (await enu.MoveNext(cancellationToken)
                                .ConfigureAwait(false))
                {
                    lookup.GetGrouping(keySelector(enu.Current), create: true)
                          .Add(enu.Current);
                }
            }

            return lookup;
        }

        internal static Lookup<TKey, TElement> CreateForJoin(IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            var lookup = new Lookup<TKey, TElement>(comparer);
            foreach (var item in source)
            {
                var key = keySelector(item);
                if (key != null)
                {
                    lookup.GetGrouping(key, create: true)
                          .Add(item);
                }
            }

            return lookup;
        }

        internal static async Task<Lookup<TKey, TElement>> CreateForJoinAsync(IAsyncEnumerable<TElement> source, Func<TElement, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            var lookup = new Lookup<TKey, TElement>(comparer);
            using (var enu = source.GetEnumerator())
            {
                while (await enu.MoveNext(cancellationToken)
                                .ConfigureAwait(false))
                {
                    var key = keySelector(enu.Current);
                    if (key != null)
                    {
                        lookup.GetGrouping(key, create: true)
                              .Add(enu.Current);
                    }
                }
            }

            return lookup;
        }

        internal Grouping<TKey, TElement> GetGrouping(TKey key, bool create)
        {
            var hashCode = InternalGetHashCode(key);
            for (var g = _groupings[hashCode%_groupings.Length]; g != null; g = g._hashNext)
            {
                if (g._hashCode == hashCode && _comparer.Equals(g._key, key))
                {
                    return g;
                }
            }

            if (create)
            {
                if (Count == _groupings.Length)
                {
                    Resize();
                }

                var index = hashCode%_groupings.Length;
                var g = new Grouping<TKey, TElement>();
                g._key = key;
                g._hashCode = hashCode;
                g._elements = new TElement[1];
                g._hashNext = _groupings[index];
                _groupings[index] = g;
                if (_lastGrouping == null)
                {
                    g._next = g;
                }
                else
                {
                    g._next = _lastGrouping._next;
                    _lastGrouping._next = g;
                }

                _lastGrouping = g;
                Count++;
                return g;
            }

            return null;
        }

        internal int InternalGetHashCode(TKey key)
        {
            // Handle comparer implementations that throw when passed null
            return (key == null) ? 0 : _comparer.GetHashCode(key) & 0x7FFFFFFF;
        }

        internal TResult[] ToArray<TResult>(Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
        {
            var array = new TResult[Count];
            var index = 0;
            var g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    g.Trim();
                    array[index] = resultSelector(g._key, g._elements);
                    ++index;
                } while (g != _lastGrouping);
            }

            return array;
        }


        internal List<TResult> ToList<TResult>(Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
        {
            var list = new List<TResult>(Count);
            var g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    g.Trim();
                    list.Add(resultSelector(g._key, g._elements));
                } while (g != _lastGrouping);
            }

            return list;
        }

        private void Resize()
        {
            var newSize = checked((Count*2) + 1);
            var newGroupings = new Grouping<TKey, TElement>[newSize];
            var g = _lastGrouping;
            do
            {
                g = g._next;
                var index = g._hashCode%newSize;
                g._hashNext = newGroupings[index];
                newGroupings[index] = g;
            } while (g != _lastGrouping);

            _groupings = newGroupings;
        }

        IAsyncEnumerator<IGrouping<TKey, TElement>> IAsyncEnumerable<IGrouping<TKey, TElement>>.GetEnumerator()
        {
            return new AsyncEnumerable.AsyncEnumerableAdapter<IGrouping<TKey, TElement>>(this).GetEnumerator();
        }

        public Task<IGrouping<TKey, TElement>[]> ToArrayAsync(CancellationToken cancellationToken)
        {
            var array = new IGrouping<TKey, TElement>[Count];
            var index = 0;
            var g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    array[index] = g;
                    ++index;
                }
                while (g != _lastGrouping);
            }

            return Task.FromResult(array);
        }


        public Task<List<IGrouping<TKey, TElement>>> ToListAsync(CancellationToken cancellationToken)
        {
            var list = new List<IGrouping<TKey, TElement>>(Count);
            var g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    list.Add(g);
                }
                while (g != _lastGrouping);
            }

            return Task.FromResult(list);
        }

        public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
        {
            return Task.FromResult(Count);
        }

        IAsyncEnumerator<IAsyncGrouping<TKey, TElement>> IAsyncEnumerable<IAsyncGrouping<TKey, TElement>>.GetEnumerator()
        {
            return new AsyncEnumerable.AsyncEnumerableAdapter<IAsyncGrouping<TKey, TElement>>(Enumerable.Cast<IAsyncGrouping<TKey, TElement>>(this)).GetEnumerator();
        }

        Task<List<IAsyncGrouping<TKey, TElement>>> IIListProvider<IAsyncGrouping<TKey, TElement>>.ToListAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IAsyncGrouping<TKey, TElement>[]> IIListProvider<IAsyncGrouping<TKey, TElement>>.ToArrayAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

    }
}