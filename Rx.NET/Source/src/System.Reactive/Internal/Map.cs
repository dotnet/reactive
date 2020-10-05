// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

#nullable disable

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace System.Reactive
{
    internal sealed class Map<TKey, TValue>
    {
        // Taken from ConcurrentDictionary in the BCL.

        // The default concurrency level is DEFAULT_CONCURRENCY_MULTIPLIER * #CPUs. The higher the
        // DEFAULT_CONCURRENCY_MULTIPLIER, the more concurrent writes can take place without interference
        // and blocking, but also the more expensive operations that require all locks become (e.g. table
        // resizing, ToArray, Count, etc). According to brief benchmarks that we ran, 4 seems like a good
        // compromise.
        private const int DefaultConcurrencyMultiplier = 4;

        private static int DefaultConcurrencyLevel => DefaultConcurrencyMultiplier * Environment.ProcessorCount;

        private readonly ConcurrentDictionary<TKey, TValue> _map;

        public Map(int? capacity, IEqualityComparer<TKey> comparer)
        {
            if (capacity.HasValue)
            {
                _map = new ConcurrentDictionary<TKey, TValue>(DefaultConcurrencyLevel, capacity.Value, comparer);
            }
            else
            {
                _map = new ConcurrentDictionary<TKey, TValue>(comparer);
            }
        }

        public TValue GetOrAdd(TKey key, Func<TValue> valueFactory, out bool added)
        {
            added = false;

            TValue value;
            var newValue = default(TValue);
            var hasNewValue = false;
            while (true)
            {
                if (_map.TryGetValue(key, out value))
                {
                    break;
                }

                if (!hasNewValue)
                {
                    newValue = valueFactory();
                    hasNewValue = true;
                }

                if (_map.TryAdd(key, newValue))
                {
                    added = true;
                    value = newValue;
                    break;
                }
            }

            return value;
        }

        public IEnumerable<TValue> Values => _map.Values.ToArray();

        public bool Remove(TKey key)
        {
            return _map.TryRemove(key, out _);
        }
    }
}
