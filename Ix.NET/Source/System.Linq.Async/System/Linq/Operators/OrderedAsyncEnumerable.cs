// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    // NB: Large portions of the implementation are based on https://github.com/dotnet/corefx/blob/master/src/System.Linq/src/System/Linq/OrderedEnumerable.cs.

    internal abstract class OrderedAsyncEnumerable<TElement> : AsyncIterator<TElement>, IOrderedAsyncEnumerable<TElement>, IAsyncPartition<TElement>
    {
        protected readonly IAsyncEnumerable<TElement> _source;

        private TElement[]? _buffer;
        private int[]? _indexes;
        private int _index;

        protected OrderedAsyncEnumerable(IAsyncEnumerable<TElement> source)
        {
            _source = source ?? throw Error.ArgumentNull(nameof(source));
        }

        IOrderedAsyncEnumerable<TElement> IOrderedAsyncEnumerable<TElement>.CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey>? comparer, bool descending)
        {
            return new OrderedAsyncEnumerable<TElement, TKey>(_source, keySelector, comparer, descending, this);
        }

        IOrderedAsyncEnumerable<TElement> IOrderedAsyncEnumerable<TElement>.CreateOrderedEnumerable<TKey>(Func<TElement, ValueTask<TKey>> keySelector, IComparer<TKey>? comparer, bool descending)
        {
            return new OrderedAsyncEnumerableWithTask<TElement, TKey>(_source, keySelector, comparer, descending, this);
        }

        IOrderedAsyncEnumerable<TElement> IOrderedAsyncEnumerable<TElement>.CreateOrderedEnumerable<TKey>(Func<TElement, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey>? comparer, bool descending)
        {
            return new OrderedAsyncEnumerableWithTaskAndCancellation<TElement, TKey>(_source, keySelector, comparer, descending, this);
        }

        protected override async ValueTask<bool> MoveNextCore()
        {
            switch (_state)
            {
                case AsyncIteratorState.Allocated:
                    _buffer = await _source.ToArrayAsync(_cancellationToken).ConfigureAwait(false); // TODO: Use buffer.

                    var sorter = GetAsyncEnumerableSorter(_cancellationToken);
                    _indexes = await sorter.Sort(_buffer, _buffer.Length).ConfigureAwait(false);
                    _index = 0;

                    _state = AsyncIteratorState.Iterating;
                    goto case AsyncIteratorState.Iterating;

                case AsyncIteratorState.Iterating:
                    if (_index < _buffer!.Length)
                    {
                        _current = _buffer[_indexes![_index++]];
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

        internal abstract AsyncEnumerableSorter<TElement> GetAsyncEnumerableSorter(AsyncEnumerableSorter<TElement>? next, CancellationToken cancellationToken);

        internal AsyncEnumerableSorter<TElement> GetAsyncEnumerableSorter(CancellationToken cancellationToken) => GetAsyncEnumerableSorter(next: null, cancellationToken);

        public async ValueTask<TElement[]> ToArrayAsync(CancellationToken cancellationToken)
        {
            var elements = await AsyncEnumerableHelpers.ToArrayWithLength(_source, cancellationToken).ConfigureAwait(false);

            var count = elements.Length;

            if (count == 0)
            {
#if NO_ARRAY_EMPTY
                return EmptyArray<TElement>.Value;
#else
                return Array.Empty<TElement>();
#endif
            }

            var array = elements.Array;

            var map = await SortedMap(array, count, cancellationToken).ConfigureAwait(false);

            var result = new TElement[count];

            for (var i = 0; i < result.Length; i++)
            {
                result[i] = array[map[i]];
            }

            return result;
        }

        internal async ValueTask<TElement[]> ToArrayAsync(int minIndexInclusive, int maxIndexInclusive, CancellationToken cancellationToken)
        {
            var elements = await AsyncEnumerableHelpers.ToArrayWithLength(_source, cancellationToken).ConfigureAwait(false);

            var count = elements.Length;

            if (count <= minIndexInclusive)
            {
#if NO_ARRAY_EMPTY
                return EmptyArray<TElement>.Value;
#else
                return Array.Empty<TElement>();
#endif
            }

            if (count <= maxIndexInclusive)
            {
                maxIndexInclusive = count - 1;
            }

            var array = elements.Array;

            if (minIndexInclusive == maxIndexInclusive)
            {
                var sorter = GetAsyncEnumerableSorter(cancellationToken);

                var element = await sorter.ElementAt(array, count, minIndexInclusive).ConfigureAwait(false);

                return new TElement[] { element };
            }

            var map = await SortedMap(array, count, minIndexInclusive, maxIndexInclusive, cancellationToken).ConfigureAwait(false);

            var result = new TElement[maxIndexInclusive - minIndexInclusive + 1];

            for (var i = 0; minIndexInclusive <= maxIndexInclusive; i++)
            {
                result[i] = array[map[minIndexInclusive++]];
            }

            return result;
        }

        public async ValueTask<List<TElement>> ToListAsync(CancellationToken cancellationToken)
        {
            var elements = await AsyncEnumerableHelpers.ToArrayWithLength(_source, cancellationToken).ConfigureAwait(false);

            var count = elements.Length;

            if (count == 0)
            {
                return new List<TElement>(capacity: 0);
            }

            var array = elements.Array;

            var map = await SortedMap(array, count, cancellationToken).ConfigureAwait(false);

            var result = new List<TElement>(count);

            for (var i = 0; i < count; i++)
            {
                result.Add(array[map[i]]);
            }

            return result;
        }

        internal async ValueTask<List<TElement>> ToListAsync(int minIndexInclusive, int maxIndexInclusive, CancellationToken cancellationToken)
        {
            var elements = await AsyncEnumerableHelpers.ToArrayWithLength(_source, cancellationToken).ConfigureAwait(false);

            var count = elements.Length;

            if (count <= minIndexInclusive)
            {
                return new List<TElement>(0);
            }

            if (count <= maxIndexInclusive)
            {
                maxIndexInclusive = count - 1;
            }

            var array = elements.Array;

            if (minIndexInclusive == maxIndexInclusive)
            {
                var sorter = GetAsyncEnumerableSorter(cancellationToken);

                var element = await sorter.ElementAt(array, count, minIndexInclusive).ConfigureAwait(false);

                return new List<TElement>(1) { element };
            }

            var map = await SortedMap(array, count, minIndexInclusive, maxIndexInclusive, cancellationToken).ConfigureAwait(false);

            var list = new List<TElement>(maxIndexInclusive - minIndexInclusive + 1);

            while (minIndexInclusive <= maxIndexInclusive)
            {
                list.Add(array[map[minIndexInclusive++]]);
            }

            return list;
        }

        public async ValueTask<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
        {
            if (_source is IAsyncIListProvider<TElement> listProv)
            {
                var count = await listProv.GetCountAsync(onlyIfCheap, cancellationToken).ConfigureAwait(false);

                return count;
            }

            return !onlyIfCheap || _source is ICollection<TElement> || _source is ICollection ? await _source.CountAsync(cancellationToken).ConfigureAwait(false) : -1;
        }

        internal async ValueTask<int> GetCountAsync(int minIndexInclusive, int maxIndexInclusive, bool onlyIfCheap, CancellationToken cancellationToken)
        {
            var count = await GetCountAsync(onlyIfCheap, cancellationToken).ConfigureAwait(false);

            if (count <= 0)
            {
                return count;
            }

            if (count <= minIndexInclusive)
            {
                return 0;
            }

            return (count <= maxIndexInclusive ? count : maxIndexInclusive + 1) - minIndexInclusive;
        }

        private ValueTask<int[]> SortedMap(TElement[] elements, int count, CancellationToken cancellationToken)
        {
            var sorter = GetAsyncEnumerableSorter(cancellationToken);

            return sorter.Sort(elements, count);
        }

        private ValueTask<int[]> SortedMap(TElement[] elements, int count, int minIndexInclusive, int maxIndexInclusive, CancellationToken cancellationToken)
        {
            var sorter = GetAsyncEnumerableSorter(cancellationToken);

            return sorter.Sort(elements, count, minIndexInclusive, maxIndexInclusive);
        }

        private AsyncCachingComparer<TElement> GetComparer() => GetComparer(childComparer: null);

        internal abstract AsyncCachingComparer<TElement> GetComparer(AsyncCachingComparer<TElement>? childComparer);

        public async ValueTask<Maybe<TElement>> TryGetFirstAsync(CancellationToken cancellationToken)
        {
            await using var e = _source.GetConfiguredAsyncEnumerator(cancellationToken, false);

            if (!await e.MoveNextAsync())
            {
                return new Maybe<TElement>();
            }

            var value = e.Current;

            var comparer = GetComparer();

            await comparer.SetElement(value, cancellationToken).ConfigureAwait(false);

            while (await e.MoveNextAsync())
            {
                var x = e.Current;

                if (await comparer.Compare(x, cacheLower: true, cancellationToken).ConfigureAwait(false) < 0)
                {
                    value = x;
                }
            }

            return new Maybe<TElement>(value);
        }

        public async ValueTask<Maybe<TElement>> TryGetLastAsync(CancellationToken cancellationToken)
        {
            await using var e = _source.GetConfiguredAsyncEnumerator(cancellationToken, false);

            if (!await e.MoveNextAsync())
            {
                return new Maybe<TElement>();
            }

            var value = e.Current;

            var comparer = GetComparer();

            await comparer.SetElement(value, cancellationToken).ConfigureAwait(false);

            while (await e.MoveNextAsync())
            {
                var current = e.Current;

                if (await comparer.Compare(current, cacheLower: false, cancellationToken).ConfigureAwait(false) >= 0)
                {
                    value = current;
                }
            }

            return new Maybe<TElement>(value);
        }

        internal async ValueTask<Maybe<TElement>> TryGetLastAsync(int minIndexInclusive, int maxIndexInclusive, CancellationToken cancellationToken)
        {
            var elements = await AsyncEnumerableHelpers.ToArrayWithLength(_source, cancellationToken).ConfigureAwait(false);

            var count = elements.Length;

            if (minIndexInclusive >= count)
            {
                return new Maybe<TElement>();
            }

            var array = elements.Array;

            TElement last;

            if (maxIndexInclusive < count - 1)
            {
                var sorter = GetAsyncEnumerableSorter(cancellationToken);

                last = await sorter.ElementAt(array, count, maxIndexInclusive).ConfigureAwait(false);
            }
            else
            {
                last = await Last(array, count, cancellationToken).ConfigureAwait(false);
            }

            return new Maybe<TElement>(last);
        }

        private async ValueTask<TElement> Last(TElement[] items, int count, CancellationToken cancellationToken)
        {
            var value = items[0];

            var comparer = GetComparer();

            await comparer.SetElement(value, cancellationToken).ConfigureAwait(false);

            for (var i = 1; i != count; ++i)
            {
                var x = items[i];

                if (await comparer.Compare(x, cacheLower: false, cancellationToken).ConfigureAwait(false) >= 0)
                {
                    value = x;
                }
            }

            return value;
        }

        public IAsyncPartition<TElement> Skip(int count) => new OrderedAsyncPartition<TElement>(this, count, int.MaxValue);

        public IAsyncPartition<TElement> Take(int count) => new OrderedAsyncPartition<TElement>(this, 0, count - 1);

        public ValueTask<Maybe<TElement>> TryGetElementAtAsync(int index, CancellationToken cancellationToken)
        {
            if (index == 0)
            {
                return TryGetFirstAsync(cancellationToken);
            }

            if (index > 0)
            {
                return Core();

                async ValueTask<Maybe<TElement>> Core()
                {
                    var elements = await AsyncEnumerableHelpers.ToArrayWithLength(_source, cancellationToken).ConfigureAwait(false);

                    var count = elements.Length;

                    if (index < count)
                    {
                        var sorter = GetAsyncEnumerableSorter(cancellationToken);

                        var element = await sorter.ElementAt(elements.Array, count, index).ConfigureAwait(false);

                        return new Maybe<TElement>(element);
                    }

                    return new Maybe<TElement>();
                }
            }

            return new ValueTask<Maybe<TElement>>(new Maybe<TElement>());
        }
    }

    internal sealed class OrderedAsyncEnumerable<TElement, TKey> : OrderedAsyncEnumerable<TElement>
    {
        private readonly IComparer<TKey> _comparer;
        private readonly bool _descending;
        private readonly Func<TElement, TKey> _keySelector;
        private readonly OrderedAsyncEnumerable<TElement>? _parent;

        public OrderedAsyncEnumerable(IAsyncEnumerable<TElement> source, Func<TElement, TKey> keySelector, IComparer<TKey>? comparer, bool descending, OrderedAsyncEnumerable<TElement>? parent)
            : base(source)
        {
            _keySelector = keySelector ?? throw Error.ArgumentNull(nameof(keySelector));
            _comparer = comparer ?? Comparer<TKey>.Default;
            _descending = descending;
            _parent = parent;
        }

        public override AsyncIteratorBase<TElement> Clone()
        {
            return new OrderedAsyncEnumerable<TElement, TKey>(_source, _keySelector, _comparer, _descending, _parent);
        }

        internal override AsyncEnumerableSorter<TElement> GetAsyncEnumerableSorter(AsyncEnumerableSorter<TElement>? next, CancellationToken cancellationToken)
        {
            var sorter = new SyncKeySelectorAsyncEnumerableSorter<TElement, TKey>(_keySelector, _comparer, _descending, next);

            if (_parent != null)
            {
                return _parent.GetAsyncEnumerableSorter(sorter, cancellationToken);
            }

            return sorter;
        }

        internal override AsyncCachingComparer<TElement> GetComparer(AsyncCachingComparer<TElement>? childComparer)
        {
            AsyncCachingComparer<TElement> cmp = childComparer == null
                ? new AsyncCachingComparer<TElement, TKey>(_keySelector, _comparer, _descending)
                : new AsyncCachingComparerWithChild<TElement, TKey>(_keySelector, _comparer, _descending, childComparer);

            return _parent != null ? _parent.GetComparer(cmp) : cmp;
        }
    }

    internal sealed class OrderedAsyncEnumerableWithTask<TElement, TKey> : OrderedAsyncEnumerable<TElement>
    {
        private readonly IComparer<TKey> _comparer;
        private readonly bool _descending;
        private readonly Func<TElement, ValueTask<TKey>> _keySelector;
        private readonly OrderedAsyncEnumerable<TElement>? _parent;

        public OrderedAsyncEnumerableWithTask(IAsyncEnumerable<TElement> source, Func<TElement, ValueTask<TKey>> keySelector, IComparer<TKey>? comparer, bool descending, OrderedAsyncEnumerable<TElement>? parent)
            : base(source)
        {
            _keySelector = keySelector ?? throw Error.ArgumentNull(nameof(keySelector));
            _comparer = comparer ?? Comparer<TKey>.Default;
            _descending = descending;
            _parent = parent;
        }

        public override AsyncIteratorBase<TElement> Clone()
        {
            return new OrderedAsyncEnumerableWithTask<TElement, TKey>(_source, _keySelector, _comparer, _descending, _parent);
        }

        internal override AsyncEnumerableSorter<TElement> GetAsyncEnumerableSorter(AsyncEnumerableSorter<TElement>? next, CancellationToken cancellationToken)
        {
            var sorter = new AsyncKeySelectorAsyncEnumerableSorter<TElement, TKey>(_keySelector, _comparer, _descending, next);

            if (_parent != null)
            {
                return _parent.GetAsyncEnumerableSorter(sorter, cancellationToken);
            }

            return sorter;
        }

        internal override AsyncCachingComparer<TElement> GetComparer(AsyncCachingComparer<TElement>? childComparer)
        {
            AsyncCachingComparer<TElement> cmp = childComparer == null
                ? new AsyncCachingComparerWithTask<TElement, TKey>(_keySelector, _comparer, _descending)
                : new AsyncCachingComparerWithTaskAndChild<TElement, TKey>(_keySelector, _comparer, _descending, childComparer);

            return _parent != null ? _parent.GetComparer(cmp) : cmp;
        }
    }

#if !NO_DEEP_CANCELLATION
    internal sealed class OrderedAsyncEnumerableWithTaskAndCancellation<TElement, TKey> : OrderedAsyncEnumerable<TElement>
    {
        private readonly IComparer<TKey> _comparer;
        private readonly bool _descending;
        private readonly Func<TElement, CancellationToken, ValueTask<TKey>> _keySelector;
        private readonly OrderedAsyncEnumerable<TElement>? _parent;

        public OrderedAsyncEnumerableWithTaskAndCancellation(IAsyncEnumerable<TElement> source, Func<TElement, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey>? comparer, bool descending, OrderedAsyncEnumerable<TElement>? parent)
            : base(source)
        {
            _keySelector = keySelector ?? throw Error.ArgumentNull(nameof(keySelector));
            _comparer = comparer ?? Comparer<TKey>.Default;
            _descending = descending;
            _parent = parent;
        }

        public override AsyncIteratorBase<TElement> Clone()
        {
            return new OrderedAsyncEnumerableWithTaskAndCancellation<TElement, TKey>(_source, _keySelector, _comparer, _descending, _parent);
        }

        internal override AsyncEnumerableSorter<TElement> GetAsyncEnumerableSorter(AsyncEnumerableSorter<TElement>? next, CancellationToken cancellationToken)
        {
            var sorter = new AsyncKeySelectorAsyncEnumerableSorterWithCancellation<TElement, TKey>(_keySelector, _comparer, _descending, next, cancellationToken);

            if (_parent != null)
            {
                return _parent.GetAsyncEnumerableSorter(sorter, cancellationToken);
            }

            return sorter;
        }

        internal override AsyncCachingComparer<TElement> GetComparer(AsyncCachingComparer<TElement>? childComparer)
        {
            AsyncCachingComparer<TElement> cmp = childComparer == null
                ? new AsyncCachingComparerWithTaskAndCancellation<TElement, TKey>(_keySelector, _comparer, _descending)
                : new AsyncCachingComparerWithTaskAndCancellationAndChild<TElement, TKey>(_keySelector, _comparer, _descending, childComparer);

            return _parent != null ? _parent.GetComparer(cmp) : cmp;
        }
    }
#endif

    internal abstract class AsyncEnumerableSorter<TElement>
    {
        internal abstract ValueTask ComputeKeys(TElement[] elements, int count);

        internal abstract int CompareAnyKeys(int index1, int index2);

        public async ValueTask<int[]> Sort(TElement[] elements, int count)
        {
            var map = await ComputeMap(elements, count).ConfigureAwait(false);

            QuickSort(map, 0, count - 1);

            return map;
        }

        public async ValueTask<int[]> Sort(TElement[] elements, int count, int minIndexInclusive, int maxIndexInclusive)
        {
            var map = await ComputeMap(elements, count).ConfigureAwait(false);

            PartialQuickSort(map, 0, count - 1, minIndexInclusive, maxIndexInclusive);

            return map;
        }

        public async ValueTask<TElement> ElementAt(TElement[] elements, int count, int index)
        {
            var map = await ComputeMap(elements, count).ConfigureAwait(false);

            return index == 0 ?
                elements[Min(map, count)] :
                elements[QuickSelect(map, count - 1, index)];
        }

        private async ValueTask<int[]> ComputeMap(TElement[] elements, int count)
        {
            await ComputeKeys(elements, count).ConfigureAwait(false);

            var map = new int[count];

            for (var i = 0; i < count; i++)
            {
                map[i] = i;
            }

            return map;
        }

        protected abstract void QuickSort(int[] map, int left, int right);

        protected abstract void PartialQuickSort(int[] map, int left, int right, int minIndexInclusive, int maxIndexInclusive);

        protected abstract int QuickSelect(int[] map, int right, int idx);

        protected abstract int Min(int[] map, int count);
    }

    internal abstract class AsyncEnumerableSorterBase<TElement, TKey> : AsyncEnumerableSorter<TElement>
    {
        private readonly IComparer<TKey> _comparer;
        private readonly bool _descending;
        protected readonly AsyncEnumerableSorter<TElement>? _next;
        protected TKey[]? _keys;

        public AsyncEnumerableSorterBase(IComparer<TKey> comparer, bool descending, AsyncEnumerableSorter<TElement>? next)
        {
            _comparer = comparer;
            _descending = descending;
            _next = next;
        }

        internal sealed override int CompareAnyKeys(int index1, int index2)
        {
            var c = _comparer.Compare(_keys![index1], _keys[index2]); // NB: _keys is assigned before calling this method.

            if (c == 0)
            {
                return _next == null ? index1 - index2 : _next.CompareAnyKeys(index1, index2);
            }
            else
            {
                return (_descending != (c > 0)) ? 1 : -1;
            }
        }

        private int CompareKeys(int index1, int index2) => index1 == index2 ? 0 : CompareAnyKeys(index1, index2);

        protected override void QuickSort(int[] keys, int lo, int hi) => Array.Sort(keys, lo, hi - lo + 1, Comparer<int>.Create(CompareAnyKeys));

        protected override void PartialQuickSort(int[] map, int left, int right, int minIndexInclusive, int maxIndexInclusive)
        {
            do
            {
                var i = left;
                var j = right;
                var x = map[i + ((j - i) >> 1)];
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
                        var temp = map[i];
                        map[i] = map[j];
                        map[j] = temp;
                    }

                    i++;
                    j--;
                }
                while (i <= j);

                if (minIndexInclusive >= i)
                {
                    left = i + 1;
                }
                else if (maxIndexInclusive <= j)
                {
                    right = j - 1;
                }

                if (j - left <= right - i)
                {
                    if (left < j)
                    {
                        PartialQuickSort(map, left, j, minIndexInclusive, maxIndexInclusive);
                    }

                    left = i;
                }
                else
                {
                    if (i < right)
                    {
                        PartialQuickSort(map, i, right, minIndexInclusive, maxIndexInclusive);
                    }

                    right = j;
                }
            }
            while (left < right);
        }

        protected override int QuickSelect(int[] map, int right, int idx)
        {
            var left = 0;
            do
            {
                var i = left;
                var j = right;
                var x = map[i + ((j - i) >> 1)];

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
                        var temp = map[i];
                        map[i] = map[j];
                        map[j] = temp;
                    }

                    i++;
                    j--;
                }
                while (i <= j);

                if (i <= idx)
                {
                    left = i + 1;
                }
                else
                {
                    right = j - 1;
                }

                if (j - left <= right - i)
                {
                    if (left < j)
                    {
                        right = j;
                    }

                    left = i;
                }
                else
                {
                    if (i < right)
                    {
                        left = i;
                    }

                    right = j;
                }
            }
            while (left < right);

            return map[idx];
        }

        protected override int Min(int[] map, int count)
        {
            var index = 0;
            for (var i = 1; i < count; i++)
            {
                if (CompareKeys(map[i], map[index]) < 0)
                {
                    index = i;
                }
            }
            return map[index];
        }
    }

    internal sealed class SyncKeySelectorAsyncEnumerableSorter<TElement, TKey> : AsyncEnumerableSorterBase<TElement, TKey>
    {
        private readonly Func<TElement, TKey> _keySelector;

        public SyncKeySelectorAsyncEnumerableSorter(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending, AsyncEnumerableSorter<TElement>? next)
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

        public AsyncKeySelectorAsyncEnumerableSorter(Func<TElement, ValueTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending, AsyncEnumerableSorter<TElement>? next)
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

        public AsyncKeySelectorAsyncEnumerableSorterWithCancellation(Func<TElement, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending, AsyncEnumerableSorter<TElement>? next, CancellationToken cancellationToken)
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

    internal sealed class OrderedAsyncPartition<TElement> : AsyncIterator<TElement>, IAsyncPartition<TElement>
    {
        private readonly OrderedAsyncEnumerable<TElement> _source;
        private readonly int _minIndexInclusive;
        private readonly int _maxIndexInclusive;

        public OrderedAsyncPartition(OrderedAsyncEnumerable<TElement> source, int minIndexInclusive, int maxIndexInclusive)
        {
            _source = source;
            _minIndexInclusive = minIndexInclusive;
            _maxIndexInclusive = maxIndexInclusive;
        }

        public override AsyncIteratorBase<TElement> Clone() => new OrderedAsyncPartition<TElement>(_source, _minIndexInclusive, _maxIndexInclusive);

        public ValueTask<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken) =>
            _source.GetCountAsync(_minIndexInclusive, _maxIndexInclusive, onlyIfCheap, cancellationToken);

        public IAsyncPartition<TElement> Skip(int count)
        {
            var minIndex = unchecked(_minIndexInclusive + count);

            if (unchecked((uint)minIndex > (uint)_maxIndexInclusive))
            {
                return AsyncEnumerable.EmptyAsyncIterator<TElement>.Instance;
            }
            
            return new OrderedAsyncPartition<TElement>(_source, minIndex, _maxIndexInclusive);
        }

        public IAsyncPartition<TElement> Take(int count)
        {
            var maxIndex = unchecked(_minIndexInclusive + count - 1);

            if (unchecked((uint)maxIndex >= (uint)_maxIndexInclusive))
            {
                return this;
            }

            return new OrderedAsyncPartition<TElement>(_source, _minIndexInclusive, maxIndex);
        }

        public ValueTask<TElement[]> ToArrayAsync(CancellationToken cancellationToken) =>
            _source.ToArrayAsync(_minIndexInclusive, _maxIndexInclusive, cancellationToken);

        public ValueTask<List<TElement>> ToListAsync(CancellationToken cancellationToken) =>
            _source.ToListAsync(_minIndexInclusive, _maxIndexInclusive, cancellationToken);

        public ValueTask<Maybe<TElement>> TryGetElementAtAsync(int index, CancellationToken cancellationToken)
        {
            if (unchecked((uint)index <= (uint)(_maxIndexInclusive - _minIndexInclusive)))
            {
                return _source.TryGetElementAtAsync(index + _minIndexInclusive, cancellationToken);
            }

            return new ValueTask<Maybe<TElement>>(new Maybe<TElement>());
        }

        public ValueTask<Maybe<TElement>> TryGetFirstAsync(CancellationToken cancellationToken) =>
            _source.TryGetElementAtAsync(_minIndexInclusive, cancellationToken);

        public ValueTask<Maybe<TElement>> TryGetLastAsync(CancellationToken cancellationToken) =>
            _source.TryGetLastAsync(_minIndexInclusive, _maxIndexInclusive, cancellationToken);

        // REVIEW: Consider to tear off an iterator object rather than storing this state here?

        private TElement[]? _buffer;
        private int[]? _indexes;
        private int _minIndexIterator;
        private int _maxIndexIterator;

        protected override async ValueTask<bool> MoveNextCore()
        {
            switch (_state)
            {
                case AsyncIteratorState.Allocated:
                    _buffer = await _source.ToArrayAsync(_cancellationToken).ConfigureAwait(false); // TODO: Use buffer.

                    _minIndexIterator = _minIndexInclusive;
                    _maxIndexIterator = _maxIndexInclusive;

                    var count = _buffer.Length;

                    if (count > _minIndexIterator)
                    {
                        if (count <= _maxIndexIterator)
                        {
                            _maxIndexIterator = count - 1;
                        }

                        var sorter = _source.GetAsyncEnumerableSorter(_cancellationToken);

                        if (_minIndexIterator == _maxIndexIterator)
                        {
                            _current = await sorter.ElementAt(_buffer, _buffer.Length, _minIndexIterator).ConfigureAwait(false);

                            _minIndexIterator = int.MaxValue;
                            _maxIndexIterator = int.MinValue;

                            _state = AsyncIteratorState.Iterating;
                            return true;
                        }
                        else
                        {
                            _indexes = await sorter.Sort(_buffer, _buffer.Length, _minIndexIterator, _maxIndexIterator).ConfigureAwait(false);
                        }

                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;
                    }

                    await DisposeAsync();
                    break;

                case AsyncIteratorState.Iterating:
                    if (_minIndexIterator <= _maxIndexIterator)
                    {
                        _current = _buffer![_indexes![_minIndexIterator++]];
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
    }

    internal abstract class AsyncCachingComparer<TElement>
    {
        internal abstract ValueTask<int> Compare(TElement element, bool cacheLower, CancellationToken cancellationToken);

        internal abstract ValueTask SetElement(TElement element, CancellationToken cancellationToken);
    }

    internal class AsyncCachingComparer<TElement, TKey> : AsyncCachingComparer<TElement>
    {
        protected readonly Func<TElement, TKey> _keySelector;
        protected readonly IComparer<TKey> _comparer;
        protected readonly bool _descending;
        protected TKey _lastKey;

        public AsyncCachingComparer(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            _keySelector = keySelector;
            _comparer = comparer;
            _descending = descending;
            _lastKey = default!;
        }

        internal override ValueTask<int> Compare(TElement element, bool cacheLower, CancellationToken cancellationToken)
        {
            var newKey = _keySelector(element);

            var cmp = _descending ? _comparer.Compare(_lastKey, newKey) : _comparer.Compare(newKey, _lastKey);

            if (cacheLower == cmp < 0)
            {
                _lastKey = newKey;
            }

            return new ValueTask<int>(cmp);
        }

        internal override ValueTask SetElement(TElement element, CancellationToken cancellationToken)
        {
            _lastKey = _keySelector(element);

            return new ValueTask();
        }
    }

    internal sealed class AsyncCachingComparerWithChild<TElement, TKey> : AsyncCachingComparer<TElement, TKey>
    {
        private readonly AsyncCachingComparer<TElement> _child;

        public AsyncCachingComparerWithChild(Func<TElement, TKey> keySelector, IComparer<TKey> comparer, bool descending, AsyncCachingComparer<TElement> child)
            : base(keySelector, comparer, descending)
        {
            _child = child;
        }

        internal override async ValueTask<int> Compare(TElement element, bool cacheLower, CancellationToken cancellationToken)
        {
            var newKey = _keySelector(element);

            var cmp = _descending ? _comparer.Compare(_lastKey, newKey) : _comparer.Compare(newKey, _lastKey);

            if (cmp == 0)
            {
                return await _child.Compare(element, cacheLower, cancellationToken).ConfigureAwait(false);
            }

            if (cacheLower == cmp < 0)
            {
                _lastKey = newKey;

                await _child.SetElement(element, cancellationToken).ConfigureAwait(false);
            }

            return cmp;
        }

        internal override async ValueTask SetElement(TElement element, CancellationToken cancellationToken)
        {
            await base.SetElement(element, cancellationToken).ConfigureAwait(false);

            await _child.SetElement(element, cancellationToken).ConfigureAwait(false);
        }
    }

    internal class AsyncCachingComparerWithTask<TElement, TKey> : AsyncCachingComparer<TElement>
    {
        protected readonly Func<TElement, ValueTask<TKey>> _keySelector;
        protected readonly IComparer<TKey> _comparer;
        protected readonly bool _descending;
        protected TKey _lastKey;

        public AsyncCachingComparerWithTask(Func<TElement, ValueTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending)
        {
            _keySelector = keySelector;
            _comparer = comparer;
            _descending = descending;
            _lastKey = default!;
        }

        internal override async ValueTask<int> Compare(TElement element, bool cacheLower, CancellationToken cancellationToken)
        {
            var newKey = await _keySelector(element).ConfigureAwait(false);

            var cmp = _descending ? _comparer.Compare(_lastKey, newKey) : _comparer.Compare(newKey, _lastKey);

            if (cacheLower == cmp < 0)
            {
                _lastKey = newKey;
            }

            return cmp;
        }

        internal override async ValueTask SetElement(TElement element, CancellationToken cancellationToken)
        {
            _lastKey = await _keySelector(element).ConfigureAwait(false);
        }
    }

    internal sealed class AsyncCachingComparerWithTaskAndChild<TElement, TKey> : AsyncCachingComparerWithTask<TElement, TKey>
    {
        private readonly AsyncCachingComparer<TElement> _child;

        public AsyncCachingComparerWithTaskAndChild(Func<TElement, ValueTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending, AsyncCachingComparer<TElement> child)
            : base(keySelector, comparer, descending)
        {
            _child = child;
        }

        internal override async ValueTask<int> Compare(TElement element, bool cacheLower, CancellationToken cancellationToken)
        {
            var newKey = await _keySelector(element).ConfigureAwait(false);

            var cmp = _descending ? _comparer.Compare(_lastKey, newKey) : _comparer.Compare(newKey, _lastKey);

            if (cmp == 0)
            {
                return await _child.Compare(element, cacheLower, cancellationToken).ConfigureAwait(false);
            }

            if (cacheLower == cmp < 0)
            {
                _lastKey = newKey;

                await _child.SetElement(element, cancellationToken).ConfigureAwait(false);
            }

            return cmp;
        }

        internal override async ValueTask SetElement(TElement element, CancellationToken cancellationToken)
        {
            await base.SetElement(element, cancellationToken).ConfigureAwait(false);

            await _child.SetElement(element, cancellationToken).ConfigureAwait(false);
        }
    }

#if !NO_DEEP_CANCELLATION
    internal class AsyncCachingComparerWithTaskAndCancellation<TElement, TKey> : AsyncCachingComparer<TElement>
    {
        protected readonly Func<TElement, CancellationToken, ValueTask<TKey>> _keySelector;
        protected readonly IComparer<TKey> _comparer;
        protected readonly bool _descending;
        protected TKey _lastKey;

        public AsyncCachingComparerWithTaskAndCancellation(Func<TElement, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending)
        {
            _keySelector = keySelector;
            _comparer = comparer;
            _descending = descending;
            _lastKey = default!;
        }

        internal override async ValueTask<int> Compare(TElement element, bool cacheLower, CancellationToken cancellationToken)
        {
            var newKey = await _keySelector(element, cancellationToken).ConfigureAwait(false);

            var cmp = _descending ? _comparer.Compare(_lastKey, newKey) : _comparer.Compare(newKey, _lastKey);

            if (cacheLower == cmp < 0)
            {
                _lastKey = newKey;
            }

            return cmp;
        }

        internal override async ValueTask SetElement(TElement element, CancellationToken cancellationToken)
        {
            _lastKey = await _keySelector(element, cancellationToken).ConfigureAwait(false);
        }
    }

    internal sealed class AsyncCachingComparerWithTaskAndCancellationAndChild<TElement, TKey> : AsyncCachingComparerWithTaskAndCancellation<TElement, TKey>
    {
        private readonly AsyncCachingComparer<TElement> _child;

        public AsyncCachingComparerWithTaskAndCancellationAndChild(Func<TElement, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer, bool descending, AsyncCachingComparer<TElement> child)
            : base(keySelector, comparer, descending)
        {
            _child = child;
        }

        internal override async ValueTask<int> Compare(TElement element, bool cacheLower, CancellationToken cancellationToken)
        {
            var newKey = await _keySelector(element, cancellationToken).ConfigureAwait(false);

            var cmp = _descending ? _comparer.Compare(_lastKey, newKey) : _comparer.Compare(newKey, _lastKey);

            if (cmp == 0)
            {
                return await _child.Compare(element, cacheLower, cancellationToken).ConfigureAwait(false);
            }

            if (cacheLower == cmp < 0)
            {
                _lastKey = newKey;

                await _child.SetElement(element, cancellationToken).ConfigureAwait(false);
            }

            return cmp;
        }

        internal override async ValueTask SetElement(TElement element, CancellationToken cancellationToken)
        {
            await base.SetElement(element, cancellationToken).ConfigureAwait(false);

            await _child.SetElement(element, cancellationToken).ConfigureAwait(false);
        }
    }
#endif
}
