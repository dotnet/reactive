// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Reactive
{
    class Lookup<K, E> : ILookup<K, E>
    {
        Dictionary<K, List<E>> d;

        public Lookup(IEqualityComparer<K> comparer)
        {
            d = new Dictionary<K, List<E>>(comparer);
        }

        public void Add(K key, E element)
        {
            var list = default(List<E>);
            if (!d.TryGetValue(key, out list))
                d[key] = list = new List<E>();
            list.Add(element);
        }

        public bool Contains(K key)
        {
            return d.ContainsKey(key);
        }

        public int Count
        {
            get { return d.Count; }
        }

        public IEnumerable<E> this[K key]
        {
            get { return Hide(d[key]); }
        }

        private IEnumerable<E> Hide(List<E> elements)
        {
            foreach (var x in elements)
                yield return x;
        }

        public IEnumerator<IGrouping<K, E>> GetEnumerator()
        {
            foreach (var kv in d)
                yield return new Grouping(kv);
        }

        class Grouping : IGrouping<K, E>
        {
            KeyValuePair<K, List<E>> kv;

            public Grouping(KeyValuePair<K, List<E>> kv)
            {
                this.kv = kv;
            }

            public K Key
            {
                get { return kv.Key; }
            }

            public IEnumerator<E> GetEnumerator()
            {
                return kv.Value.GetEnumerator();
            }

            Collections.IEnumerator Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}