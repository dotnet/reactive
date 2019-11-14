// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

// Copied from https://github.com/dotnet/corefx/blob/5f1dd8298e4355b63bb760d88d437a91b3ca808c/src/System.Linq/src/System/Linq/Partition.cs

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    /// <summary>
    /// An iterator that yields the items of part of an <see cref="IAsyncEnumerable{TSource}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the source enumerable.</typeparam>
    internal sealed class AsyncEnumerablePartition<TSource> : AsyncIterator<TSource>, IAsyncPartition<TSource>
    {
        private readonly IAsyncEnumerable<TSource> _source;
        private readonly int _minIndexInclusive;
        private readonly int _maxIndexInclusive; // -1 if we want everything past _minIndexInclusive.
                                                 // If this is -1, it's impossible to set a limit on the count.
        private IAsyncEnumerator<TSource>? _enumerator;

        internal AsyncEnumerablePartition(IAsyncEnumerable<TSource> source, int minIndexInclusive, int maxIndexInclusive)
        {
            Debug.Assert(!(source is IList<TSource>), $"The caller needs to check for {nameof(IList<TSource>)}.");
            Debug.Assert(minIndexInclusive >= 0);
            Debug.Assert(maxIndexInclusive >= -1);
            // Note that although maxIndexInclusive can't grow, it can still be int.MaxValue.
            // We support partitioning enumerables with > 2B elements. For example, e.Skip(1).Take(int.MaxValue) should work.
            // But if it is int.MaxValue, then minIndexInclusive must != 0. Otherwise, our count may overflow.
            Debug.Assert(maxIndexInclusive == -1 || (maxIndexInclusive - minIndexInclusive < int.MaxValue), $"{nameof(Limit)} will overflow!");
            Debug.Assert(maxIndexInclusive == -1 || minIndexInclusive <= maxIndexInclusive);

            _source = source;
            _minIndexInclusive = minIndexInclusive;
            _maxIndexInclusive = maxIndexInclusive;
        }

        // If this is true (e.g. at least one Take call was made), then we have an upper bound
        // on how many elements we can have.
        private bool HasLimit => _maxIndexInclusive != -1;

        private int Limit => (_maxIndexInclusive + 1) - _minIndexInclusive; // This is that upper bound.

        public override AsyncIteratorBase<TSource> Clone()
        {
            return new AsyncEnumerablePartition<TSource>(_source, _minIndexInclusive, _maxIndexInclusive);
        }

        public override async ValueTask DisposeAsync()
        {
            if (_enumerator != null)
            {
                await _enumerator.DisposeAsync().ConfigureAwait(false);
                _enumerator = null;
            }

            await base.DisposeAsync().ConfigureAwait(false);
        }

        public ValueTask<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
        {
            if (onlyIfCheap)
            {
                return new ValueTask<int>(-1);
            }

            return Core();

            async ValueTask<int> Core()
            {
                if (!HasLimit)
                {
                    // If HasLimit is false, we contain everything past _minIndexInclusive.
                    // Therefore, we have to iterate the whole enumerable.
                    return Math.Max(await _source.CountAsync(cancellationToken).ConfigureAwait(false) - _minIndexInclusive, 0);
                }

                var en = _source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    // We only want to iterate up to _maxIndexInclusive + 1.
                    // Past that, we know the enumerable will be able to fit this partition,
                    // so the count will just be _maxIndexInclusive + 1 - _minIndexInclusive.

                    // Note that it is possible for _maxIndexInclusive to be int.MaxValue here,
                    // so + 1 may result in signed integer overflow. We need to handle this.
                    // At the same time, however, we are guaranteed that our max count can fit
                    // in an int because if that is true, then _minIndexInclusive must > 0.

                    var count = await SkipAndCountAsync((uint)_maxIndexInclusive + 1, en).ConfigureAwait(false);
                    Debug.Assert(count != (uint)int.MaxValue + 1 || _minIndexInclusive > 0, "Our return value will be incorrect.");
                    return Math.Max((int)count - _minIndexInclusive, 0);
                }
                finally
                {
                    await en.DisposeAsync().ConfigureAwait(false);
                }
            }
        }

        private bool _hasSkipped;
        private int _taken;

        protected override async ValueTask<bool> MoveNextCore()
        {
            switch (_state)
            {
                case AsyncIteratorState.Allocated:
                    _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                    _hasSkipped = false;
                    _taken = 0;

                    _state = AsyncIteratorState.Iterating;
                    goto case AsyncIteratorState.Iterating;

                case AsyncIteratorState.Iterating:
                    if (!_hasSkipped)
                    {
                        if (!await SkipBeforeFirstAsync(_enumerator!).ConfigureAwait(false))
                        {
                            // Reached the end before we finished skipping.
                            break;
                        }

                        _hasSkipped = true;
                    }

                    if ((!HasLimit || _taken < Limit) && await _enumerator!.MoveNextAsync().ConfigureAwait(false))
                    {
                        if (HasLimit)
                        {
                            // If we are taking an unknown number of elements, it's important not to increment _state.
                            // _state - 3 may eventually end up overflowing & we'll hit the Dispose branch even though
                            // we haven't finished enumerating.
                            _taken++;
                        }

                        _current = _enumerator.Current;
                        return true;
                    }

                    break;
            }

            await DisposeAsync().ConfigureAwait(false);
            return false;
        }

#if NOTYET
        public override IAsyncEnumerable<TResult> Select<TResult>(Func<TSource, TResult> selector)
        {
            return new SelectIPartitionIterator<TSource, TResult>(this, selector);
        }

        public override IAsyncEnumerable<TResult> Select<TResult>(Func<TSource, ValueTask<TResult>> selector)
        {
            return new SelectIPartitionIterator<TSource, TResult>(this, selector);
        }
#endif

        public IAsyncPartition<TSource> Skip(int count)
        {
            var minIndex = _minIndexInclusive + count;
            if (!HasLimit)
            {
                if (minIndex < 0)
                {
                    // If we don't know our max count and minIndex can no longer fit in a positive int,
                    // then we will need to wrap ourselves in another iterator.
                    // This can happen, for example, during e.Skip(int.MaxValue).Skip(int.MaxValue).
                    return new AsyncEnumerablePartition<TSource>(this, count, -1);
                }
            }
            else if ((uint)minIndex > (uint)_maxIndexInclusive)
            {
                // If minIndex overflows and we have an upper bound, we will go down this branch.
                // We know our upper bound must be smaller than minIndex, since our upper bound fits in an int.
                // This branch should not be taken if we don't have a bound.
                return AsyncEnumerable.EmptyAsyncIterator<TSource>.Instance;
            }

            Debug.Assert(minIndex >= 0, $"We should have taken care of all cases when {nameof(minIndex)} overflows.");
            return new AsyncEnumerablePartition<TSource>(_source, minIndex, _maxIndexInclusive);
        }

        public IAsyncPartition<TSource> Take(int count)
        {
            var maxIndex = _minIndexInclusive + count - 1;
            if (!HasLimit)
            {
                if (maxIndex < 0)
                {
                    // If we don't know our max count and maxIndex can no longer fit in a positive int,
                    // then we will need to wrap ourselves in another iterator.
                    // Note that although maxIndex may be too large, the difference between it and
                    // _minIndexInclusive (which is count - 1) must fit in an int.
                    // Example: e.Skip(50).Take(int.MaxValue).

                    return new AsyncEnumerablePartition<TSource>(this, 0, count - 1);
                }
            }
            else if ((uint)maxIndex >= (uint)_maxIndexInclusive)
            {
                // If we don't know our max count, we can't go down this branch.
                // It's always possible for us to contain more than count items, as the rest
                // of the enumerable past _minIndexInclusive can be arbitrarily long.
                return this;
            }

            Debug.Assert(maxIndex >= 0, $"We should have taken care of all cases when {nameof(maxIndex)} overflows.");
            return new AsyncEnumerablePartition<TSource>(_source, _minIndexInclusive, maxIndex);
        }

        public async ValueTask<Maybe<TSource>> TryGetElementAtAsync(int index, CancellationToken cancellationToken)
        {
            // If the index is negative or >= our max count, return early.
            if (index >= 0 && (!HasLimit || index < Limit))
            {
                var en = _source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    Debug.Assert(_minIndexInclusive + index >= 0, $"Adding {nameof(index)} caused {nameof(_minIndexInclusive)} to overflow.");

                    if (await SkipBeforeAsync(_minIndexInclusive + index, en).ConfigureAwait(false) && await en.MoveNextAsync().ConfigureAwait(false))
                    {
                        return new Maybe<TSource>(en.Current);
                    }
                }
                finally
                {
                    await en.DisposeAsync().ConfigureAwait(false);
                }
            }

            return new Maybe<TSource>();
        }

        public async ValueTask<Maybe<TSource>> TryGetFirstAsync(CancellationToken cancellationToken)
        {
            var en = _source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (await SkipBeforeFirstAsync(en).ConfigureAwait(false) && await en.MoveNextAsync().ConfigureAwait(false))
                {
                    return new Maybe<TSource>(en.Current);
                }
            }
            finally
            {
                await en.DisposeAsync().ConfigureAwait(false);
            }

            return new Maybe<TSource>();
        }

        public async ValueTask<Maybe<TSource>> TryGetLastAsync(CancellationToken cancellationToken)
        {
            var en = _source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (await SkipBeforeFirstAsync(en).ConfigureAwait(false) && await en.MoveNextAsync().ConfigureAwait(false))
                {
                    var remaining = Limit - 1; // Max number of items left, not counting the current element.
                    var comparand = HasLimit ? 0 : int.MinValue; // If we don't have an upper bound, have the comparison always return true.
                    TSource result;

                    do
                    {
                        remaining--;
                        result = en.Current;
                    }
                    while (remaining >= comparand && await en.MoveNextAsync().ConfigureAwait(false));

                    return new Maybe<TSource>(result);
                }
            }
            finally
            {
                await en.DisposeAsync().ConfigureAwait(false);
            }

            return new Maybe<TSource>();
        }

        public async ValueTask<TSource[]> ToArrayAsync(CancellationToken cancellationToken)
        {
            var en = _source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (await SkipBeforeFirstAsync(en).ConfigureAwait(false) && await en.MoveNextAsync().ConfigureAwait(false))
                {
                    var remaining = Limit - 1; // Max number of items left, not counting the current element.
                    var comparand = HasLimit ? 0 : int.MinValue; // If we don't have an upper bound, have the comparison always return true.

                    // REVIEW: If this ends up in corefx, the code below can use LargeArrayBuilder<T>.

                    var builder = HasLimit ? new List<TSource>(Limit) : new List<TSource>();

                    do
                    {
                        remaining--;
                        builder.Add(en.Current);
                    }
                    while (remaining >= comparand && await en.MoveNextAsync().ConfigureAwait(false));

                    return builder.ToArray();
                }
            }
            finally
            {
                await en.DisposeAsync().ConfigureAwait(false);
            }

#if NO_ARRAY_EMPTY
            return EmptyArray<TSource>.Value;
#else
            return Array.Empty<TSource>();
#endif
        }

        public async ValueTask<List<TSource>> ToListAsync(CancellationToken cancellationToken)
        {
            var list = new List<TSource>();

            var en = _source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (await SkipBeforeFirstAsync(en).ConfigureAwait(false) && await en.MoveNextAsync().ConfigureAwait(false))
                {
                    var remaining = Limit - 1; // Max number of items left, not counting the current element.
                    var comparand = HasLimit ? 0 : int.MinValue; // If we don't have an upper bound, have the comparison always return true.

                    do
                    {
                        remaining--;
                        list.Add(en.Current);
                    }
                    while (remaining >= comparand && await en.MoveNextAsync().ConfigureAwait(false));
                }
            }
            finally
            {
                await en.DisposeAsync().ConfigureAwait(false);
            }

            return list;
        }

        private ValueTask<bool> SkipBeforeFirstAsync(IAsyncEnumerator<TSource> en) => SkipBeforeAsync(_minIndexInclusive, en);

        private static async ValueTask<bool> SkipBeforeAsync(int index, IAsyncEnumerator<TSource> en)
        {
            var n = await SkipAndCountAsync(index, en).ConfigureAwait(false);
            return n == index;
        }

        private static async ValueTask<int> SkipAndCountAsync(int index, IAsyncEnumerator<TSource> en)
        {
            Debug.Assert(index >= 0);
            return (int)await SkipAndCountAsync((uint)index, en).ConfigureAwait(false);
        }

        private static async ValueTask<uint> SkipAndCountAsync(uint index, IAsyncEnumerator<TSource> en)
        {
            for (uint i = 0; i < index; i++)
            {
                if (!await en.MoveNextAsync().ConfigureAwait(false))
                {
                    return i;
                }
            }

            return index;
        }
    }
}
