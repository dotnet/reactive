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
    // TODO: Add optimizations for First, Last, and ElementAt.
    
    internal abstract class OrderedAsyncEnumerable<TElement> : AsyncIterator<TElement>, IOrderedAsyncEnumerable<TElement>, IAsyncIListProvider<TElement>
    {
        protected IAsyncEnumerable<TElement> _source;
        private TElement[] _buffer;
        private int[] _indexes;
        private int _index;

        IOrderedAsyncEnumerable<TElement> IOrderedAsyncEnumerable<TElement>.CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            return new OrderedAsyncEnumerable<TElement, TKey>(_source, keySelector, comparer, descending, this);
        }

        IOrderedAsyncEnumerable<TElement> IOrderedAsyncEnumerable<TElement>.CreateOrderedEnumerable<TKey>(Func<TElement, ValueTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending)
        {
            return new OrderedAsyncEnumerableWithTask<TElement, TKey>(_source, keySelector, comparer, descending, this);
        }

        IOrderedAsyncEnumerable<TElement> IOrderedAsyncEnumerable<TElement>.CreateOrderedEnumerable<TKey>(Func<TElement, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending)
        {
            return new OrderedAsyncEnumerableWithTaskAndCancellation<TElement, TKey>(_source, keySelector, comparer, descending, this);
        }

        protected override async ValueTask<bool> MoveNextCore()
        {
            switch (_state)
            {
                case AsyncIteratorState.Allocated:
                    _buffer = await _source.ToArrayAsync(_cancellationToken).ConfigureAwait(false);

                    AsyncEnumerableSorter<TElement> sorter = GetAsyncEnumerableSorter(next: null, _cancellationToken);
                    _indexes = await sorter.Sort(_buffer, _buffer.Length).ConfigureAwait(false);
                    _index = 0;

                    _state = AsyncIteratorState.Iterating;
                    goto case AsyncIteratorState.Iterating;

                case AsyncIteratorState.Iterating:
                    if (_index < _buffer.Length)
                    {
                        _current = _buffer[_indexes[_index++]];
                        return true;
                    }

                    await DisposeAsync().ConfigureAwait(false);
                    break;
            }

            return false;
        }

        public override async ValueTask DisposeAsync()
        {
            _buffer = null;
            _indexes = null;

            await base.DisposeAsync().ConfigureAwait(false);
        }

        internal abstract AsyncEnumerableSorter<TElement> GetAsyncEnumerableSorter(AsyncEnumerableSorter<TElement> next, CancellationToken cancellationToken);

        public async ValueTask<TElement[]> ToArrayAsync(CancellationToken cancellationToken)
        {
            AsyncEnumerableHelpers.ArrayWithLength<TElement> elements = await AsyncEnumerableHelpers.ToArrayWithLength(_source, cancellationToken).ConfigureAwait(false);

            int count = elements.Length;

            if (count == 0)
            {
#if NO_ARRAY_EMPTY
                return EmptyArray<TElement>.Value;
#else
                return Array.Empty<TElement>();
#endif
            }

            TElement[] array = elements.Array;

            int[] map = await SortedMap(array, count, cancellationToken).ConfigureAwait(false);

            var result = new TElement[count];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = array[map[i]];
            }

            return result;
        }

        public async ValueTask<List<TElement>> ToListAsync(CancellationToken cancellationToken)
        {
            AsyncEnumerableHelpers.ArrayWithLength<TElement> elements = await AsyncEnumerableHelpers.ToArrayWithLength(_source, cancellationToken).ConfigureAwait(false);

            int count = elements.Length;

            if (count == 0)
            {
                return new List<TElement>(capacity: 0);
            }

            TElement[] array = elements.Array;

            int[] map = await SortedMap(array, count, cancellationToken).ConfigureAwait(false);

            var result = new List<TElement>(count);

            for (int i = 0; i < count; i++)
            {
                result.Add(array[map[i]]);
            }

            return result;
        }

        public async ValueTask<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
        {
            if (_source is IAsyncIListProvider<TElement> listProv)
            {
                int count = await listProv.GetCountAsync(onlyIfCheap, cancellationToken).ConfigureAwait(false);
            }

            return !onlyIfCheap || _source is ICollection<TElement> || _source is ICollection ? await _source.CountAsync(cancellationToken).ConfigureAwait(false) : -1;
        }

        private ValueTask<int[]> SortedMap(TElement[] elements, int count, CancellationToken cancellationToken)
        {
            AsyncEnumerableSorter<TElement> sorter = GetAsyncEnumerableSorter(next: null, cancellationToken);

            return sorter.Sort(elements, count);
        }
    }

    internal sealed class OrderedAsyncEnumerable<TElement, TKey> : OrderedAsyncEnumerable<TElement>
    {
        private readonly IComparer<TKey> _comparer;
        private readonly bool _descending;
        private readonly Func<TElement, TKey> _keySelector;
        private readonly OrderedAsyncEnumerable<TElement> _parent;

        public OrderedAsyncEnumerable(IAsyncEnumerable<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending, OrderedAsyncEnumerable<TElement> parent)
        {
            Debug.Assert(source != null);
            Debug.Assert(keySelector != null);

            _source = source;
            _keySelector = keySelector;
            _comparer = comparer;
            _descending = descending;
            _parent = parent;
        }

        public override AsyncIteratorBase<TElement> Clone()
        {
            return new OrderedAsyncEnumerable<TElement, TKey>(_source, _keySelector, _comparer, _descending, _parent);
        }

        internal override AsyncEnumerableSorter<TElement> GetAsyncEnumerableSorter(AsyncEnumerableSorter<TElement> next, CancellationToken cancellationToken)
        {
            var sorter = new SyncKeySelectorAsyncEnumerableSorter<TElement, TKey>(_keySelector, _comparer, _descending, next);

            if (_parent != null)
            {
                return _parent.GetAsyncEnumerableSorter(sorter, cancellationToken);
            }

            return sorter;
        }
    }

    internal sealed class OrderedAsyncEnumerableWithTask<TElement, TKey> : OrderedAsyncEnumerable<TElement>
    {
        private readonly IComparer<TKey> _comparer;
        private readonly bool _descending;
        private readonly Func<TElement, ValueTask<TKey>> _keySelector;
        private readonly OrderedAsyncEnumerable<TElement> _parent;

        public OrderedAsyncEnumerableWithTask(IAsyncEnumerable<TElement> source, Func<TElement, ValueTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending, OrderedAsyncEnumerable<TElement> parent)
        {
            Debug.Assert(source != null);
            Debug.Assert(keySelector != null);

            _source = source;
            _keySelector = keySelector;
            _comparer = comparer;
            _descending = descending;
            _parent = parent;
        }

        public override AsyncIteratorBase<TElement> Clone()
        {
            return new OrderedAsyncEnumerableWithTask<TElement, TKey>(_source, _keySelector, _comparer, _descending, _parent);
        }

        internal override AsyncEnumerableSorter<TElement> GetAsyncEnumerableSorter(AsyncEnumerableSorter<TElement> next, CancellationToken cancellationToken)
        {
            var sorter = new AsyncKeySelectorAsyncEnumerableSorter<TElement, TKey>(_keySelector, _comparer, _descending, next);

            if (_parent != null)
            {
                return _parent.GetAsyncEnumerableSorter(sorter, cancellationToken);
            }

            return sorter;
        }
    }

#if !NO_DEEP_CANCELLATION
    internal sealed class OrderedAsyncEnumerableWithTaskAndCancellation<TElement, TKey> : OrderedAsyncEnumerable<TElement>
    {
        private readonly IComparer<TKey> _comparer;
        private readonly bool _descending;
        private readonly Func<TElement, CancellationToken, ValueTask<TKey>> _keySelector;
        private readonly OrderedAsyncEnumerable<TElement> _parent;

        public OrderedAsyncEnumerableWithTaskAndCancellation(IAsyncEnumerable<TElement> source, Func<TElement, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending, OrderedAsyncEnumerable<TElement> parent)
        {
            Debug.Assert(source != null);
            Debug.Assert(keySelector != null);

            _source = source;
            _keySelector = keySelector;
            _comparer = comparer;
            _descending = descending;
            _parent = parent;
        }

        public override AsyncIteratorBase<TElement> Clone()
        {
            return new OrderedAsyncEnumerableWithTaskAndCancellation<TElement, TKey>(_source, _keySelector, _comparer, _descending, _parent);
        }

        internal override AsyncEnumerableSorter<TElement> GetAsyncEnumerableSorter(AsyncEnumerableSorter<TElement> next, CancellationToken cancellationToken)
        {
            var sorter = new AsyncKeySelectorAsyncEnumerableSorterWithCancellation<TElement, TKey>(_keySelector, _comparer, _descending, next, cancellationToken);

            if (_parent != null)
            {
                return _parent.GetAsyncEnumerableSorter(sorter, cancellationToken);
            }

            return sorter;
        }
    }
#endif

    internal abstract class AsyncEnumerableSorter<TElement>
    {
        internal abstract ValueTask ComputeKeys(TElement[] elements, int count);

        internal abstract int CompareKeys(int index1, int index2);

        public async ValueTask<int[]> Sort(TElement[] elements, int count)
        {
            await ComputeKeys(elements, count).ConfigureAwait(false);

            var map = new int[count];

            for (var i = 0; i < count; i++)
            {
                map[i] = i;
            }

            QuickSort(map, 0, count - 1);

            return map;
        }

        private void QuickSort(int[] map, int left, int right)
        {
            do
            {
                int i = left;
                int j = right;
                int x = map[i + (j - i >> 1)];

                do
                {
                    while (i < map.Length && CompareKeys(x, map[i]) > 0)
                    {
                        i++;
                    }

                    while (j >= 0 && CompareKeys(x, map[j]) < 0)
                    {
                        j--;
                    }

                    if (i > j)
                    {
                        break;
                    }

                    if (i < j)
                    {
                        int temp = map[i];
                        map[i] = map[j];
                        map[j] = temp;
                    }

                    i++;
                    j--;
                }
                while (i <= j);

                if (j - left <= right - i)
                {
                    if (left < j)
                    {
                        QuickSort(map, left, j);
                    }

                    left = i;
                }
                else
                {
                    if (i < right)
                    {
                        QuickSort(map, i, right);
                    }

                    right = j;
                }
            }
            while (left < right);
        }
    }

    internal abstract class AsyncEnumerableSorterBase<TElement, TKey> : AsyncEnumerableSorter<TElement>
    {
        private readonly IComparer<TKey> _comparer;
        private readonly bool _descending;
        protected readonly AsyncEnumerableSorter<TElement> _next;
        protected TKey[] _keys;

        public AsyncEnumerableSorterBase(IComparer<TKey> comparer, bool descending, AsyncEnumerableSorter<TElement> next)
        {
            _comparer = comparer ?? Comparer<TKey>.Default;
            _descending = descending;
            _next = next;
        }

        internal override int CompareKeys(int index1, int index2)
        {
            int c = _comparer.Compare(_keys[index1], _keys[index2]);

            if (c == 0)
            {
                return _next == null ? index1 - index2 : _next.CompareKeys(index1, index2);
            }
            else
            {
                return (_descending != (c > 0)) ? 1 : -1;
            }
        }
    }

    internal sealed class SyncKeySelectorAsyncEnumerableSorter<TElement, TKey> : AsyncEnumerableSorterBase<TElement, TKey>
    {
        private readonly Func<TElement, TKey> _keySelector;

        public SyncKeySelectorAsyncEnumerableSorter(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending, AsyncEnumerableSorter<TElement> next)
            : base(comparer, descending, next)
        {
            _keySelector = keySelector;
        }

        internal override async ValueTask ComputeKeys(TElement[] elements, int count)
        {
            _keys = new TKey[count];

            for (var i = 0; i < count; i++)
            {
                _keys[i] = _keySelector(elements[i]);
            }

            if (_next != null)
            {
                await _next.ComputeKeys(elements, count).ConfigureAwait(false);
            }
        }
    }

    internal sealed class AsyncKeySelectorAsyncEnumerableSorter<TElement, TKey> : AsyncEnumerableSorterBase<TElement, TKey>
    {
        private readonly Func<TElement, ValueTask<TKey>> _keySelector;

        public AsyncKeySelectorAsyncEnumerableSorter(Func<TElement, ValueTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending, AsyncEnumerableSorter<TElement> next)
            : base(comparer, descending, next)
        {
            _keySelector = keySelector;
        }

        internal override async ValueTask ComputeKeys(TElement[] elements, int count)
        {
            _keys = new TKey[count];

            for (var i = 0; i < count; i++)
            {
                _keys[i] = await _keySelector(elements[i]).ConfigureAwait(false);
            }

            if (_next != null)
            {
                await _next.ComputeKeys(elements, count).ConfigureAwait(false);
            }
        }
    }

#if !NO_DEEP_CANCELLATION
    internal sealed class AsyncKeySelectorAsyncEnumerableSorterWithCancellation<TElement, TKey> : AsyncEnumerableSorterBase<TElement, TKey>
    {
        private readonly Func<TElement, CancellationToken, ValueTask<TKey>> _keySelector;
        private readonly CancellationToken _cancellationToken;

        public AsyncKeySelectorAsyncEnumerableSorterWithCancellation(Func<TElement, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending, AsyncEnumerableSorter<TElement> next, CancellationToken cancellationToken)
            : base(comparer, descending, next)
        {
            _keySelector = keySelector;
            _cancellationToken = cancellationToken;
        }

        internal override async ValueTask ComputeKeys(TElement[] elements, int count)
        {
            _keys = new TKey[count];

            for (var i = 0; i < count; i++)
            {
                _keys[i] = await _keySelector(elements[i], _cancellationToken).ConfigureAwait(false);
            }

            if (_next != null)
            {
                await _next.ComputeKeys(elements, count).ConfigureAwait(false);
            }
        }
    }
#endif
}
