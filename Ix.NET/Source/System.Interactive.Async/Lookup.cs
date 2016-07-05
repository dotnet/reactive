// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq.Internal
{
    internal class Lookup<TKey, TElement> : ILookup<TKey, TElement>
    {
        private readonly IEqualityComparer<TKey> _comparer;
        private Grouping<TKey, TElement>[] _groupings;
        private Grouping<TKey, TElement> _lastGrouping;
        private int _count;

        internal static Lookup<TKey, TElement> Create<TSource>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            Debug.Assert(source != null);
            Debug.Assert(keySelector != null);
            Debug.Assert(elementSelector != null);

            Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
            foreach (TSource item in source)
            {
                lookup.GetGrouping(keySelector(item), create: true).Add(elementSelector(item));
            }

            return lookup;
        }

        internal static async Task<Lookup<TKey, TElement>> CreateAsync<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            Debug.Assert(source != null);
            Debug.Assert(keySelector != null);
            Debug.Assert(elementSelector != null);

            Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
            using (var enu = source.GetEnumerator())
            {
                while (await enu.MoveNext(cancellationToken)
                                .ConfigureAwait(false))
                {
                    lookup.GetGrouping(keySelector(enu.Current), create: true).Add(elementSelector(enu.Current));
                }
            }

            return lookup;
        }

        internal static Lookup<TKey, TElement> Create(IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            Debug.Assert(source != null);
            Debug.Assert(keySelector != null);

            Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
            foreach (TElement item in source)
            {
                lookup.GetGrouping(keySelector(item), create: true).Add(item);
            }

            return lookup;
        }

        internal static Lookup<TKey, TElement> CreateForJoin(IEnumerable<TElement> source, Func<TElement, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
            foreach (TElement item in source)
            {
                TKey key = keySelector(item);
                if (key != null)
                {
                    lookup.GetGrouping(key, create: true).Add(item);
                }
            }

            return lookup;
        }

        internal static async Task<Lookup<TKey, TElement>> CreateForJoinAsync(IAsyncEnumerable<TElement> source, Func<TElement, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            Lookup<TKey, TElement> lookup = new Lookup<TKey, TElement>(comparer);
            using (var enu = source.GetEnumerator())
            {
                while (await enu.MoveNext(cancellationToken)
                                .ConfigureAwait(false))
                {
                    TKey key = keySelector(enu.Current);
                    if (key != null)
                    {
                        lookup.GetGrouping(key, create: true).Add(enu.Current);
                    }
                }
            }

            return lookup;
        }

        private Lookup(IEqualityComparer<TKey> comparer)
        {
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
            _groupings = new Grouping<TKey, TElement>[7];
        }

        public int Count
        {
            get { return _count; }
        }

        public IEnumerable<TElement> this[TKey key]
        {
            get
            {
                Grouping<TKey, TElement> grouping = GetGrouping(key, create: false);
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

        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            Grouping<TKey, TElement> g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    yield return g;
                }
                while (g != _lastGrouping);
            }
        }

        internal TResult[] ToArray<TResult>(Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
        {
            TResult[] array = new TResult[_count];
            int index = 0;
            Grouping<TKey, TElement> g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    g.Trim();
                    array[index] = resultSelector(g._key, g._elements);
                    ++index;
                }
                while (g != _lastGrouping);
            }

            return array;
        }


        internal List<TResult> ToList<TResult>(Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
        {
            List<TResult> list = new List<TResult>(_count);
            Grouping<TKey, TElement> g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    g.Trim();
                    list.Add(resultSelector(g._key, g._elements));
                }
                while (g != _lastGrouping);
            }

            return list;
        }

        public IEnumerable<TResult> ApplyResultSelector<TResult>(Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
        {
            Grouping<TKey, TElement> g = _lastGrouping;
            if (g != null)
            {
                do
                {
                    g = g._next;
                    g.Trim();
                    yield return resultSelector(g._key, g._elements);
                }
                while (g != _lastGrouping);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal int InternalGetHashCode(TKey key)
        {
            // Handle comparer implementations that throw when passed null
            return (key == null) ? 0 : _comparer.GetHashCode(key) & 0x7FFFFFFF;
        }

        internal Grouping<TKey, TElement> GetGrouping(TKey key, bool create)
        {
            int hashCode = InternalGetHashCode(key);
            for (Grouping<TKey, TElement> g = _groupings[hashCode % _groupings.Length]; g != null; g = g._hashNext)
            {
                if (g._hashCode == hashCode && _comparer.Equals(g._key, key))
                {
                    return g;
                }
            }

            if (create)
            {
                if (_count == _groupings.Length)
                {
                    Resize();
                }

                int index = hashCode % _groupings.Length;
                Grouping<TKey, TElement> g = new Grouping<TKey, TElement>();
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
                _count++;
                return g;
            }

            return null;
        }

        private void Resize()
        {
            int newSize = checked((_count * 2) + 1);
            Grouping<TKey, TElement>[] newGroupings = new Grouping<TKey, TElement>[newSize];
            Grouping<TKey, TElement> g = _lastGrouping;
            do
            {
                g = g._next;
                int index = g._hashCode % newSize;
                g._hashNext = newGroupings[index];
                newGroupings[index] = g;
            }
            while (g != _lastGrouping);

            _groupings = newGroupings;
        }
    }

}
