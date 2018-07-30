// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive
{
    internal sealed class PriorityQueue<T> where T : IComparable<T>
    {
        private long _count = long.MinValue;
        private IndexedItem[] _items;
        private int _size;

        public PriorityQueue()
            : this(16)
        {
        }

        public PriorityQueue(int capacity)
        {
            _items = new IndexedItem[capacity];
            _size = 0;
        }

        private bool IsHigherPriority(int left, int right)
        {
            return _items[left].CompareTo(_items[right]) < 0;
        }

        private int Percolate(int index)
        {
            if (index >= _size || index < 0)
            {
                return index;
            }

            var parent = (index - 1) / 2;
            while (parent >= 0 && parent != index && IsHigherPriority(index, parent))
            { 
                // swap index and parent
                var temp = _items[index];
                _items[index] = _items[parent];
                _items[parent] = temp;

                index = parent;
                parent = (index - 1) / 2;
            }

            return index;
        }

        private void Heapify(int index)
        {
            if (index >= _size || index < 0)
            {
                return;
            }

            while (true)
            {
                var left = 2 * index + 1;
                var right = 2 * index + 2;
                var first = index;

                if (left < _size && IsHigherPriority(left, first))
                {
                    first = left;
                }

                if (right < _size && IsHigherPriority(right, first))
                {
                    first = right;
                }

                if (first == index)
                {
                    break;
                }

                // swap index and first
                var temp = _items[index];
                _items[index] = _items[first];
                _items[first] = temp;

                index = first;
            }
        }

        public int Count => _size;

        public T Peek()
        {
            if (_size == 0)
            {
                throw new InvalidOperationException(Strings_Core.HEAP_EMPTY);
            }

            return _items[0].Value;
        }

        private void RemoveAt(int index)
        {
            _items[index] = _items[--_size];
            _items[_size] = default;

            if (Percolate(index) == index)
            {
                Heapify(index);
            }

            if (_size < _items.Length / 4)
            {
                var temp = _items;
                _items = new IndexedItem[_items.Length / 2];
                Array.Copy(temp, 0, _items, 0, _size);
            }
        }

        public T Dequeue()
        {
            var result = Peek();
            RemoveAt(0);
            return result;
        }

        public void Enqueue(T item)
        {
            if (_size >= _items.Length)
            {
                var temp = _items;
                _items = new IndexedItem[_items.Length * 2];
                Array.Copy(temp, _items, temp.Length);
            }

            var index = _size++;
            _items[index] = new IndexedItem { Value = item, Id = ++_count };
            Percolate(index);
        }

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

        private struct IndexedItem : IComparable<IndexedItem>
        {
            public T Value;
            public long Id;

            public int CompareTo(IndexedItem other)
            {
                var c = Value.CompareTo(other.Value);
                if (c == 0)
                {
                    c = Id.CompareTo(other.Id);
                }

                return c;
            }
        }
    }
}
