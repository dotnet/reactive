// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

// from https://github.com/dotnet/corefx/blob/ec2685715b01d12f16b08d0dfa326649b12db8ec/src/System.Linq/src/System/Linq/Set.cs
namespace System.Linq
{
    internal sealed class Set<TElement>
    {
        private readonly IEqualityComparer<TElement> _comparer;
        private int[] _buckets;
#if DEBUG
        private bool _haveRemoved;
#endif
        private Slot[] _slots;

        public Set(IEqualityComparer<TElement> comparer)
        {
            _comparer = comparer ?? EqualityComparer<TElement>.Default;
            _buckets = new int[7];
            _slots = new Slot[7];
        }

        internal int Count { get; private set; }

        // If value is not in set, add it and return true; otherwise return false
        public bool Add(TElement value)
        {
#if DEBUG
            Debug.Assert(!_haveRemoved, "This class is optimised for never calling Add after Remove. If your changes need to do so, undo that optimization.");
#endif
            var hashCode = InternalGetHashCode(value);
            for (var i = _buckets[hashCode%_buckets.Length] - 1; i >= 0; i = _slots[i]._next)
            {
                if (_slots[i]._hashCode == hashCode && _comparer.Equals(_slots[i]._value, value))
                {
                    return false;
                }
            }

            if (Count == _slots.Length)
            {
                Resize();
            }

            var index = Count;
            Count++;
            var bucket = hashCode%_buckets.Length;
            _slots[index]._hashCode = hashCode;
            _slots[index]._value = value;
            _slots[index]._next = _buckets[bucket] - 1;
            _buckets[bucket] = index + 1;
            return true;
        }

        // If value is in set, remove it and return true; otherwise return false
        public bool Remove(TElement value)
        {
#if DEBUG
            _haveRemoved = true;
#endif
            var hashCode = InternalGetHashCode(value);
            var bucket = hashCode%_buckets.Length;
            var last = -1;
            for (var i = _buckets[bucket] - 1; i >= 0; last = i, i = _slots[i]._next)
            {
                if (_slots[i]._hashCode == hashCode && _comparer.Equals(_slots[i]._value, value))
                {
                    if (last < 0)
                    {
                        _buckets[bucket] = _slots[i]._next + 1;
                    }
                    else
                    {
                        _slots[last]._next = _slots[i]._next;
                    }

                    _slots[i]._hashCode = -1;
                    _slots[i]._value = default(TElement);
                    _slots[i]._next = -1;
                    return true;
                }
            }

            return false;
        }

        internal int InternalGetHashCode(TElement value)
        {
            // Handle comparer implementations that throw when passed null
            return (value == null) ? 0 : _comparer.GetHashCode(value) & 0x7FFFFFFF;
        }

        internal TElement[] ToArray()
        {
#if DEBUG
            Debug.Assert(!_haveRemoved, "Optimised ToArray cannot be called if Remove has been called.");
#endif
            var array = new TElement[Count];
            for (var i = 0; i != array.Length; ++i)
            {
                array[i] = _slots[i]._value;
            }

            return array;
        }

        internal List<TElement> ToList()
        {
#if DEBUG
            Debug.Assert(!_haveRemoved, "Optimised ToList cannot be called if Remove has been called.");
#endif
            var count = Count;
            var list = new List<TElement>(count);
            for (var i = 0; i != count; ++i)
            {
                list.Add(_slots[i]._value);
            }

            return list;
        }

        private void Resize()
        {
            var newSize = checked((Count*2) + 1);
            var newBuckets = new int[newSize];
            var newSlots = new Slot[newSize];
            Array.Copy(_slots, 0, newSlots, 0, Count);
            for (var i = 0; i < Count; i++)
            {
                var bucket = newSlots[i]._hashCode%newSize;
                newSlots[i]._next = newBuckets[bucket] - 1;
                newBuckets[bucket] = i + 1;
            }

            _buckets = newBuckets;
            _slots = newSlots;
        }

        internal struct Slot
        {
            internal int _hashCode;
            internal int _next;
            internal TElement _value;
        }
    }
}