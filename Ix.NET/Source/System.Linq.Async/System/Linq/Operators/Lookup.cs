// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

// This is internal because System.Linq exposes a public Lookup that we cannot directly use here

namespace System.Linq.Internal
{
    internal class Lookup<TKey, TElement> : ILookup<TKey, TElement>, IAsyncIListProvider<IAsyncGrouping<TKey, TElement>>
    {
        private readonly IEqualityComparer<TKey> _comparer;
        private Grouping<TKey, TElement>[] _groupings;
        private Grouping<TKey, TElement>? _lastGrouping;

        private Lookup(IEqualityComparer<TKey>? comparer)
        {
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
            _groupings = new Grouping<TKey, TElement>[7];
        }

        public int Count { get; private set; }

        public IEnumerable<TElement> this[TKey key]
        {
            get
            {
                var grouping = GetGrouping(key);
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
            return GetGrouping(key) != null;
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
                    g = g!._next;
                    yield return g!;
                } while (g != _lastGrouping);
            }
        }

        public IEnumerable<TResult> ApplyResultSelector<TResult>(Func<TKey, IAsyncEnumerable<TElement>, TResult> resultSelector)
        {
            var g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    g!.Trim();

                    var result = resultSelector(g._key, g._elements.ToAsyncEnumerable());
                    yield return result;
                } while (g != _lastGrouping);
            }
        }

        internal static async Task<Lookup<TKey, TElement>> CreateAsync<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken)
        {
            var lookup = new Lookup<TKey, TElement>(comparer);

            await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                var key = keySelector(item);
                var group = lookup.GetOrCreateGrouping(key);

                var element = elementSelector(item);
                group.Add(element);
            }

            return lookup;
        }

        internal static async Task<Lookup<TKey, TElement>> CreateAsync(IAsyncEnumerable<TElement> source, Func<TElement, TKey> keySelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken)
        {
            var lookup = new Lookup<TKey, TElement>(comparer);

            await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                var key = keySelector(item);
                lookup.GetOrCreateGrouping(key).Add(item);
            }

            return lookup;
        }

        internal static async Task<Lookup<TKey, TElement>> CreateForJoinAsync(IAsyncEnumerable<TElement> source, Func<TElement, TKey> keySelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken)
        {
            var lookup = new Lookup<TKey, TElement>(comparer);

            await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                var key = keySelector(item);
                if (key != null)
                {
                    lookup.GetOrCreateGrouping(key).Add(item);
                }
            }

            return lookup;
        }

        internal Grouping<TKey, TElement>? GetGrouping(TKey key)
        {
            var hashCode = InternalGetHashCode(key);

            return GetGrouping(key, hashCode);
        }

        internal Grouping<TKey, TElement>? GetGrouping(TKey key, int hashCode)
        {
            for (var g = _groupings[hashCode % _groupings.Length]; g != null; g = g._hashNext)
            {
                if (g._hashCode == hashCode && _comparer.Equals(g._key, key))
                {
                    return g;
                }
            }

            return null;
        }

        internal Grouping<TKey, TElement> GetOrCreateGrouping(TKey key)
        {
            var hashCode = InternalGetHashCode(key);

            var grouping = GetGrouping(key, hashCode);
            if (grouping != null)
            {
                return grouping;
            }

            if (Count == _groupings.Length)
            {
                Resize();
            }

            var index = hashCode % _groupings.Length;
            var g = new Grouping<TKey, TElement>(key, hashCode, new TElement[1], _groupings[index]);
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

        internal int InternalGetHashCode(TKey key)
        {
            // Handle comparer implementations that throw when passed null
            return (key == null) ? 0 : _comparer.GetHashCode(key) & 0x7FFFFFFF;
        }

        internal TResult[] ToArray<TResult>(Func<TKey, IAsyncEnumerable<TElement>, TResult> resultSelector)
        {
            var array = new TResult[Count];
            var index = 0;
            var g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    g!.Trim();
                    array[index] = resultSelector(g._key, g._elements.ToAsyncEnumerable());
                    ++index;
                } while (g != _lastGrouping);
            }

            return array;
        }

        internal List<TResult> ToList<TResult>(Func<TKey, IAsyncEnumerable<TElement>, TResult> resultSelector)
        {
            var list = new List<TResult>(Count);
            var g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    g!.Trim();

                    var result = resultSelector(g._key, g._elements.ToAsyncEnumerable());
                    list.Add(result);
                } while (g != _lastGrouping);
            }

            return list;
        }

        private void Resize()
        {
            var newSize = checked((Count * 2) + 1);
            var newGroupings = new Grouping<TKey, TElement>[newSize];
            var g = _lastGrouping;
            do
            {
                g = g!._next;
                var index = g!._hashCode % newSize;
                g._hashNext = newGroupings[index];
                newGroupings[index] = g;
            } while (g != _lastGrouping);

            _groupings = newGroupings;
        }

        public ValueTask<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
        {
            return new ValueTask<int>(Count);
        }

        IAsyncEnumerator<IAsyncGrouping<TKey, TElement>> IAsyncEnumerable<IAsyncGrouping<TKey, TElement>>.GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested(); // NB: [LDM-2018-11-28] Equivalent to async iterator behavior.

            return Enumerable.Cast<IAsyncGrouping<TKey, TElement>>(this).ToAsyncEnumerable().GetAsyncEnumerator(cancellationToken);
        }

        ValueTask<List<IAsyncGrouping<TKey, TElement>>> IAsyncIListProvider<IAsyncGrouping<TKey, TElement>>.ToListAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var list = new List<IAsyncGrouping<TKey, TElement>>(Count);
            var g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g!._next;
                    list.Add(g!);
                }
                while (g != _lastGrouping);
            }

            return new ValueTask<List<IAsyncGrouping<TKey, TElement>>>(list);
        }

        ValueTask<IAsyncGrouping<TKey, TElement>[]> IAsyncIListProvider<IAsyncGrouping<TKey, TElement>>.ToArrayAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var array = new IAsyncGrouping<TKey, TElement>[Count];
            var index = 0;
            var g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g!._next;
                    array[index] = g!;
                    ++index;
                }
                while (g != _lastGrouping);
            }

            return new ValueTask<IAsyncGrouping<TKey, TElement>[]>(array);
        }
    }

    internal class LookupWithTask<TKey, TElement> : ILookup<TKey, TElement>, IAsyncIListProvider<IAsyncGrouping<TKey, TElement>>
    {
        private readonly IEqualityComparer<TKey> _comparer;
        private Grouping<TKey, TElement>[] _groupings;
        private Grouping<TKey, TElement>? _lastGrouping;

        private LookupWithTask(IEqualityComparer<TKey>? comparer)
        {
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
            _groupings = new Grouping<TKey, TElement>[7];
        }

        public int Count { get; private set; }

        public IEnumerable<TElement> this[TKey key]
        {
            get
            {
                var grouping = GetGrouping(key);
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
            return GetGrouping(key) != null;
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
                    g = g!._next;
                    yield return g!;
                } while (g != _lastGrouping);
            }
        }

        internal static async Task<LookupWithTask<TKey, TElement>> CreateAsync<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken)
        {
            var lookup = new LookupWithTask<TKey, TElement>(comparer);

            await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                var key = await keySelector(item).ConfigureAwait(false);
                var group = lookup.GetOrCreateGrouping(key);

                var element = await elementSelector(item).ConfigureAwait(false);
                group.Add(element);
            }

            return lookup;
        }

#if !NO_DEEP_CANCELLATION
        internal static async Task<LookupWithTask<TKey, TElement>> CreateAsync<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken)
        {
            var lookup = new LookupWithTask<TKey, TElement>(comparer);

            await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                var key = await keySelector(item, cancellationToken).ConfigureAwait(false);
                var group = lookup.GetOrCreateGrouping(key);

                var element = await elementSelector(item, cancellationToken).ConfigureAwait(false);
                group.Add(element);
            }

            return lookup;
        }
#endif

        internal static async Task<LookupWithTask<TKey, TElement>> CreateAsync(IAsyncEnumerable<TElement> source, Func<TElement, ValueTask<TKey>> keySelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken)
        {
            var lookup = new LookupWithTask<TKey, TElement>(comparer);

            await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                var key = await keySelector(item).ConfigureAwait(false);
                lookup.GetOrCreateGrouping(key).Add(item);
            }

            return lookup;
        }

#if !NO_DEEP_CANCELLATION
        internal static async Task<LookupWithTask<TKey, TElement>> CreateAsync(IAsyncEnumerable<TElement> source, Func<TElement, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken)
        {
            var lookup = new LookupWithTask<TKey, TElement>(comparer);

            await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                var key = await keySelector(item, cancellationToken).ConfigureAwait(false);
                lookup.GetOrCreateGrouping(key).Add(item);
            }

            return lookup;
        }
#endif

        internal static async Task<LookupWithTask<TKey, TElement>> CreateForJoinAsync(IAsyncEnumerable<TElement> source, Func<TElement, ValueTask<TKey>> keySelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken)
        {
            var lookup = new LookupWithTask<TKey, TElement>(comparer);

            await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                var key = await keySelector(item).ConfigureAwait(false);
                if (key != null)
                {
                    lookup.GetOrCreateGrouping(key).Add(item);
                }
            }

            return lookup;
        }

#if !NO_DEEP_CANCELLATION
        internal static async Task<LookupWithTask<TKey, TElement>> CreateForJoinAsync(IAsyncEnumerable<TElement> source, Func<TElement, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken)
        {
            var lookup = new LookupWithTask<TKey, TElement>(comparer);

            await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                var key = await keySelector(item, cancellationToken).ConfigureAwait(false);
                if (key != null)
                {
                    lookup.GetOrCreateGrouping(key).Add(item);
                }
            }

            return lookup;
        }
#endif

        internal Grouping<TKey, TElement>? GetGrouping(TKey key)
        {
            var hashCode = InternalGetHashCode(key);

            return GetGrouping(key, hashCode);
        }

        internal Grouping<TKey, TElement>? GetGrouping(TKey key, int hashCode)
        {
            for (var g = _groupings[hashCode % _groupings.Length]; g != null; g = g._hashNext)
            {
                if (g._hashCode == hashCode && _comparer.Equals(g._key, key))
                {
                    return g;
                }
            }

            return null;
        }

        internal Grouping<TKey, TElement> GetOrCreateGrouping(TKey key)
        {
            var hashCode = InternalGetHashCode(key);

            var grouping = GetGrouping(key, hashCode);
            if (grouping != null)
            {
                return grouping;
            }

            if (Count == _groupings.Length)
            {
                Resize();
            }

            var index = hashCode % _groupings.Length;
            var g = new Grouping<TKey, TElement>(key, hashCode, new TElement[1], _groupings[index]);
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

        internal int InternalGetHashCode(TKey key)
        {
            // Handle comparer implementations that throw when passed null
            return (key == null) ? 0 : _comparer.GetHashCode(key) & 0x7FFFFFFF;
        }

        internal async Task<TResult[]> ToArray<TResult>(Func<TKey, IAsyncEnumerable<TElement>, ValueTask<TResult>> resultSelector)
        {
            var array = new TResult[Count];
            var index = 0;
            var g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    g!.Trim();
                    array[index] = await resultSelector(g._key, g._elements.ToAsyncEnumerable()).ConfigureAwait(false);
                    ++index;
                } while (g != _lastGrouping);
            }

            return array;
        }

#if !NO_DEEP_CANCELLATION
        internal async Task<TResult[]> ToArray<TResult>(Func<TKey, IAsyncEnumerable<TElement>, CancellationToken, ValueTask<TResult>> resultSelector, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var array = new TResult[Count];
            var index = 0;
            var g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    g!.Trim();
                    array[index] = await resultSelector(g._key, g._elements.ToAsyncEnumerable(), cancellationToken).ConfigureAwait(false);
                    ++index;
                } while (g != _lastGrouping);
            }

            return array;
        }
#endif

        internal async Task<List<TResult>> ToList<TResult>(Func<TKey, IAsyncEnumerable<TElement>, ValueTask<TResult>> resultSelector)
        {
            var list = new List<TResult>(Count);
            var g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    g!.Trim();

                    var result = await resultSelector(g._key, g._elements.ToAsyncEnumerable()).ConfigureAwait(false);
                    list.Add(result);
                } while (g != _lastGrouping);
            }

            return list;
        }

#if !NO_DEEP_CANCELLATION
        internal async Task<List<TResult>> ToList<TResult>(Func<TKey, IAsyncEnumerable<TElement>, CancellationToken, ValueTask<TResult>> resultSelector, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var list = new List<TResult>(Count);
            var g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    g!.Trim();

                    var result = await resultSelector(g._key, g._elements.ToAsyncEnumerable(), cancellationToken).ConfigureAwait(false);
                    list.Add(result);
                } while (g != _lastGrouping);
            }

            return list;
        }
#endif

        private void Resize()
        {
            var newSize = checked((Count * 2) + 1);
            var newGroupings = new Grouping<TKey, TElement>[newSize];
            var g = _lastGrouping;
            do
            {
                g = g!._next;
                var index = g!._hashCode % newSize;
                g._hashNext = newGroupings[index];
                newGroupings[index] = g;
            } while (g != _lastGrouping);

            _groupings = newGroupings;
        }

        public ValueTask<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
        {
            return new ValueTask<int>(Count);
        }

        IAsyncEnumerator<IAsyncGrouping<TKey, TElement>> IAsyncEnumerable<IAsyncGrouping<TKey, TElement>>.GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested(); // NB: [LDM-2018-11-28] Equivalent to async iterator behavior.

            return Enumerable.Cast<IAsyncGrouping<TKey, TElement>>(this).ToAsyncEnumerable().GetAsyncEnumerator(cancellationToken);
        }

        ValueTask<List<IAsyncGrouping<TKey, TElement>>> IAsyncIListProvider<IAsyncGrouping<TKey, TElement>>.ToListAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var list = new List<IAsyncGrouping<TKey, TElement>>(Count);
            var g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g!._next;
                    list.Add(g!);
                }
                while (g != _lastGrouping);
            }

            return new ValueTask<List<IAsyncGrouping<TKey, TElement>>>(list);
        }

        ValueTask<IAsyncGrouping<TKey, TElement>[]> IAsyncIListProvider<IAsyncGrouping<TKey, TElement>>.ToArrayAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var array = new IAsyncGrouping<TKey, TElement>[Count];
            var index = 0;
            var g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g!._next;
                    array[index] = g!;
                    ++index;
                }
                while (g != _lastGrouping);
            }

            return new ValueTask<IAsyncGrouping<TKey, TElement>[]>(array);
        }
    }
}
