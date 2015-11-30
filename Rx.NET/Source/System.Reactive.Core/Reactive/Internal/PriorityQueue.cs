// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Threading;
using System.Collections.Generic;
using System.Collections;

namespace System.Reactive
{
    /// <summary>
    /// Priority queue data structure implemented using a min-heap. 
    /// </summary>
    public class PriorityQueue<T> : IReadOnlyCollection<T> where T : IComparable<T>
    {
#if !NO_INTERLOCKED_64
        private static long _count = long.MinValue;
#else
        private static int _count = int.MinValue;
#endif
        private IndexedItem[] _items;
        private int _size;

        /// <summary>
        /// Create an empty priority queue.
        /// </summary>
        public PriorityQueue()
            : this(16)
        {
        }

        /// <summary>
        /// Create an empty priority queue with given initial capacity.
        /// </summary>
        public PriorityQueue(int capacity)
        {
            _items = new IndexedItem[capacity];
            _size = 0;
        }

        private bool IsHigherPriority(int left, int right)
        {
            return _items[left].CompareTo(_items[right]) < 0;
        }

        private void Percolate(int index)
        {
            if (index >= _size || index < 0)
                return;
            var parent = (index - 1) / 2;
            if (parent < 0 || parent == index)
                return;

            if (IsHigherPriority(index, parent))
            {
                var temp = _items[index];
                _items[index] = _items[parent];
                _items[parent] = temp;
                Percolate(parent);
            }
        }

        private void Heapify()
        {
            Heapify(0);
        }

        private void Heapify(int index)
        {
            if (index >= _size || index < 0)
                return;

            var left = 2 * index + 1;
            var right = 2 * index + 2;
            var first = index;

            if (left < _size && IsHigherPriority(left, first))
                first = left;
            if (right < _size && IsHigherPriority(right, first))
                first = right;
            if (first != index)
            {
                var temp = _items[index];
                _items[index] = _items[first];
                _items[first] = temp;
                Heapify(first);
            }
        }

        /// <summary>
        /// Number of elements in the priority queue.
        /// </summary>
        public int Count { get { return _size; } }

        /// <summary>
        /// Find the minimum element in the priority queue. O(1)
        /// </summary>
        public T Peek()
        {
            if (_size == 0)
                throw new InvalidOperationException(Strings_Core.HEAP_EMPTY);

            return _items[0].Value;
        }

        private void RemoveAt(int index)
        {
            _items[index] = _items[--_size];
            _items[_size] = default(IndexedItem);
            Heapify();
            if (_size < _items.Length / 4)
            {
                var temp = _items;
                _items = new IndexedItem[_items.Length / 2];
                Array.Copy(temp, 0, _items, 0, _size);
            }
        }

        /// <summary>
        /// Remove and return the minimum element in the priority queue. O(log n)
        /// </summary>
        public T Dequeue()
        {
            var result = Peek();
            RemoveAt(0);
            return result;
        }

        /// <summary>
        /// Add an element to the priority queue. O(log n)
        /// </summary>
        public void Enqueue(T item)
        {
            if (_size >= _items.Length)
            {
                var temp = _items;
                _items = new IndexedItem[_items.Length * 2];
                Array.Copy(temp, _items, temp.Length);
            }

            var index = _size++;
            _items[index] = new IndexedItem { Value = item, Id = Interlocked.Increment(ref _count) };
            Percolate(index);
        }

        /// <summary>
        /// Remove the first matching element from the priority queue. O(n)
        /// </summary>
        public bool Remove(T item)
        {
            for (var i = 0; i < _size; ++i)
            {
                if (EqualityComparer<T>.Default.Equals(_items[i].Value, item))
                {
                    RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// List all the elements in the priority queue.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _items)
                yield return item.Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        struct IndexedItem : IComparable<IndexedItem>
        {
            public T Value;
#if !NO_INTERLOCKED_64
            public long Id;
#else
            public int Id;
#endif

            public int CompareTo(IndexedItem other)
            {
                var c = Value.CompareTo(other.Value);
                if (c == 0)
                    c = Id.CompareTo(other.Id);
                return c;
            }
        }
    }
}
