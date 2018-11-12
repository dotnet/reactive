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
    /// An iterator that yields the items of part of an <see cref="IList{TSource}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the source list.</typeparam>
    internal sealed class AsyncListPartition<TSource> : AsyncIterator<TSource>, IAsyncPartition<TSource>
    {
        private readonly IList<TSource> _source;
        private readonly int _minIndexInclusive;
        private readonly int _maxIndexInclusive;
        private int _index;

        public AsyncListPartition(IList<TSource> source, int minIndexInclusive, int maxIndexInclusive)
        {
            Debug.Assert(source != null);
            Debug.Assert(minIndexInclusive >= 0);
            Debug.Assert(minIndexInclusive <= maxIndexInclusive);
            _source = source;
            _minIndexInclusive = minIndexInclusive;
            _maxIndexInclusive = maxIndexInclusive;
            _index = 0;
        }

        public override AsyncIterator<TSource> Clone()
        {
            return new AsyncListPartition<TSource>(_source, _minIndexInclusive, _maxIndexInclusive);
        }

        protected override async ValueTask<bool> MoveNextCore()
        {
            if ((uint)_index <= (uint)(_maxIndexInclusive - _minIndexInclusive) && _index < _source.Count - _minIndexInclusive)
            {
                current = _source[_minIndexInclusive + _index];
                ++_index;
                return true;
            }

            await DisposeAsync().ConfigureAwait(false);
            return false;
        }

#if NOT_YET
        public override IEnumerable<TResult> Select<TResult>(Func<TSource, TResult> selector)
        {
            return new SelectListPartitionIterator<TSource, TResult>(_source, selector, _minIndexInclusive, _maxIndexInclusive);
        }

        public override IEnumerable<TResult> Select<TResult>(Func<TSource, Task<TResult>> selector)
        {
            return new SelectListPartitionIterator<TSource, TResult>(_source, selector, _minIndexInclusive, _maxIndexInclusive);
        }
#endif

        public IAsyncPartition<TSource> Skip(int count)
        {
            var minIndex = _minIndexInclusive + count;
            if ((uint)minIndex > (uint)_maxIndexInclusive)
            {
                return AsyncEnumerable.EmptyAsyncIterator<TSource>.Instance;
            }
            else
            {
                return new AsyncListPartition<TSource>(_source, minIndex, _maxIndexInclusive);
            }
        }

        public IAsyncPartition<TSource> Take(int count)
        {
            var maxIndex = _minIndexInclusive + count - 1;
            if ((uint)maxIndex >= (uint)_maxIndexInclusive)
            {
                return this;
            }
            else
            {
                return new AsyncListPartition<TSource>(_source, _minIndexInclusive, maxIndex);
            }
        }

        public Task<Maybe<TSource>> TryGetElementAsync(int index, CancellationToken cancellationToken)
        {
            if ((uint)index <= (uint)(_maxIndexInclusive - _minIndexInclusive) && index < _source.Count - _minIndexInclusive)
            {
                var res = _source[_minIndexInclusive + index];
                return Task.FromResult(new Maybe<TSource>(res));
            }

            return Task.FromResult(new Maybe<TSource>());
        }

        public Task<Maybe<TSource>> TryGetFirstAsync(CancellationToken cancellationToken)
        {
            if (_source.Count > _minIndexInclusive)
            {
                var res = _source[_minIndexInclusive];
                return Task.FromResult(new Maybe<TSource>(res));
            }

            return Task.FromResult(new Maybe<TSource>());
        }

        public Task<Maybe<TSource>> TryGetLastAsync(CancellationToken cancellationToken)
        {
            var lastIndex = _source.Count - 1;
            if (lastIndex >= _minIndexInclusive)
            {
                var res = _source[Math.Min(lastIndex, _maxIndexInclusive)];
                return Task.FromResult(new Maybe<TSource>(res));
            }

            return Task.FromResult(new Maybe<TSource>());
        }

        private int Count
        {
            get
            {
                var count = _source.Count;
                if (count <= _minIndexInclusive)
                {
                    return 0;
                }

                return Math.Min(count - 1, _maxIndexInclusive) - _minIndexInclusive + 1;
            }
        }

        public Task<TSource[]> ToArrayAsync(CancellationToken cancellationToken)
        {
            var count = Count;
            if (count == 0)
            {
                return Task.FromResult(
#if NO_ARRAY_EMPTY
                    EmptyArray<TSource>.Value
#else
                    Array.Empty<TSource>()
#endif
                );
            }

            var array = new TSource[count];
            for (int i = 0, curIdx = _minIndexInclusive; i != array.Length; ++i, ++curIdx)
            {
                array[i] = _source[curIdx];
            }

            return Task.FromResult(array);
        }

        public Task<List<TSource>> ToListAsync(CancellationToken cancellationToken)
        {
            var count = Count;
            if (count == 0)
            {
                return Task.FromResult(new List<TSource>());
            }

            var list = new List<TSource>(count);
            var end = _minIndexInclusive + count;
            for (var i = _minIndexInclusive; i != end; ++i)
            {
                list.Add(_source[i]);
            }

            return Task.FromResult(list);
        }

        public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
        {
            return Task.FromResult(Count);
        }
    }
}
