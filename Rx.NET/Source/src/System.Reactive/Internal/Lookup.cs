// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Reactive
{
    internal sealed class Lookup<K, E> : ILookup<K, E>
    {
        private readonly Dictionary<K, List<E>> _dictionary;

        public Lookup(IEqualityComparer<K> comparer)
        {
            _dictionary = new Dictionary<K, List<E>>(comparer);
        }

        public void Add(K key, E element)
        {

            if (!_dictionary.TryGetValue(key, out var list))
            {
                _dictionary[key] = list = new List<E>();
            }

            list.Add(element);
        }

        public bool Contains(K key) => _dictionary.ContainsKey(key);

        public int Count => _dictionary.Count;

        public IEnumerable<E> this[K key]
        {
            get
            {

                if (!_dictionary.TryGetValue(key, out var list))
                    return Enumerable.Empty<E>();

                return Hide(list);
            }
        }

        private IEnumerable<E> Hide(List<E> elements)
        {
            foreach (var x in elements)
            {
                yield return x;
            }
        }

        public IEnumerator<IGrouping<K, E>> GetEnumerator()
        {
            foreach (var kv in _dictionary)
            {
                yield return new Grouping(kv);
            }
        }

        private sealed class Grouping : IGrouping<K, E>
        {
            private readonly KeyValuePair<K, List<E>> _keyValuePair;

            public Grouping(KeyValuePair<K, List<E>> keyValuePair)
            {
                _keyValuePair = keyValuePair;
            }

            public K Key => _keyValuePair.Key;

            public IEnumerator<E> GetEnumerator() => _keyValuePair.Value.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
