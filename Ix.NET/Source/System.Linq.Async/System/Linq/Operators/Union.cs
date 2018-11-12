// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Union<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));

            return UnionCore(first, second, EqualityComparer<TSource>.Default);
        }

        public static IAsyncEnumerable<TSource> Union<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));
            if (comparer == null)
                throw Error.ArgumentNull(nameof(comparer));

            return UnionCore(first, second, comparer);
        }

        private static IAsyncEnumerable<TSource> UnionCore<TSource>(IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            return first is UnionAsyncIterator<TSource> union && AreEqualityComparersEqual(comparer, union._comparer) ? union.Union(second) : new UnionAsyncIterator2<TSource>(first, second, comparer);
        }

        private static bool AreEqualityComparersEqual<TSource>(IEqualityComparer<TSource> first, IEqualityComparer<TSource> second)
        {
            return first == second || (first != null && second != null && first.Equals(second));
        }

        /// <summary>
        /// An iterator that yields distinct values from two or more <see cref="IAsyncEnumerable{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source enumerables.</typeparam>
        private abstract class UnionAsyncIterator<TSource> : AsyncIterator<TSource>, IAsyncIListProvider<TSource>
        {
            internal readonly IEqualityComparer<TSource> _comparer;
            private IAsyncEnumerator<TSource> _enumerator;
            private Set<TSource> _set;
            private int _index;

            protected UnionAsyncIterator(IEqualityComparer<TSource> comparer)
            {
                _comparer = comparer;
            }

            public sealed override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                    _set = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            internal abstract IAsyncEnumerable<TSource> GetEnumerable(int index);

            internal abstract UnionAsyncIterator<TSource> Union(IAsyncEnumerable<TSource> next);

            private async Task SetEnumeratorAsync(IAsyncEnumerator<TSource> enumerator)
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                }

                _enumerator = enumerator;
            }

            private void StoreFirst()
            {
                var set = new Set<TSource>(_comparer);
                var element = _enumerator.Current;
                set.Add(element);
                current = element;
                _set = set;
            }

            private async Task<bool> GetNextAsync()
            {
                var set = _set;
                Debug.Assert(set != null);

                while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                {
                    var element = _enumerator.Current;
                    if (set.Add(element))
                    {
                        current = element;
                        return true;
                    }
                }

                return false;
            }

            protected sealed override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _index = 0;

                        for (var enumerable = GetEnumerable(0); enumerable != null; enumerable = GetEnumerable(_index))
                        {
                            ++_index;

                            var enumerator = enumerable.GetAsyncEnumerator(cancellationToken);

                            if (await enumerator.MoveNextAsync().ConfigureAwait(false))
                            {
                                await SetEnumeratorAsync(enumerator).ConfigureAwait(false);
                                StoreFirst();

                                state = AsyncIteratorState.Iterating;
                                return true;
                            }
                        }

                        break;

                    case AsyncIteratorState.Iterating:
                        while (true)
                        {
                            if (await GetNextAsync().ConfigureAwait(false))
                            {
                                return true;
                            }

                            var enumerable = GetEnumerable(_index);
                            if (enumerable == null)
                            {
                                break;
                            }

                            await SetEnumeratorAsync(enumerable.GetAsyncEnumerator(cancellationToken)).ConfigureAwait(false);
                            ++_index;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }

            private async Task<Set<TSource>> FillSetAsync(CancellationToken cancellationToken)
            {
                var set = new Set<TSource>(_comparer);

                for (var index = 0; ; ++index)
                {
                    var enumerable = GetEnumerable(index);
                    if (enumerable == null)
                    {
                        return set;
                    }

                    await set.UnionWithAsync(enumerable, cancellationToken).ConfigureAwait(false);
                }
            }

            public Task<TSource[]> ToArrayAsync(CancellationToken cancellationToken) => FillSetAsync(cancellationToken).ContinueWith(set => set.Result.ToArray());

            public Task<List<TSource>> ToListAsync(CancellationToken cancellationToken) => FillSetAsync(cancellationToken).ContinueWith(set => set.Result.ToList());

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken) => onlyIfCheap ? TaskExt.MinusOne : FillSetAsync(cancellationToken).ContinueWith(set => set.Result.Count);
        }

        /// <summary>
        /// An iterator that yields distinct values from two <see cref="IAsyncEnumerable{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source enumerables.</typeparam>
        private sealed class UnionAsyncIterator2<TSource> : UnionAsyncIterator<TSource>
        {
            private readonly IAsyncEnumerable<TSource> _first;
            private readonly IAsyncEnumerable<TSource> _second;

            public UnionAsyncIterator2(IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
                : base(comparer)
            {
                Debug.Assert(first != null);
                Debug.Assert(second != null);
                _first = first;
                _second = second;
            }

            public override AsyncIterator<TSource> Clone() => new UnionAsyncIterator2<TSource>(_first, _second, _comparer);

            internal override IAsyncEnumerable<TSource> GetEnumerable(int index)
            {
                Debug.Assert(index >= 0 && index <= 2);
                switch (index)
                {
                    case 0:
                        return _first;
                    case 1:
                        return _second;
                    default:
                        return null;
                }
            }

            internal override UnionAsyncIterator<TSource> Union(IAsyncEnumerable<TSource> next)
            {
                var sources = new SingleLinkedNode<IAsyncEnumerable<TSource>>(_first).Add(_second).Add(next);
                return new UnionAsyncIteratorN<TSource>(sources, 2, _comparer);
            }
        }

        /// <summary>
        /// An iterator that yields distinct values from three or more <see cref="IAsyncEnumerable{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source enumerables.</typeparam>
        private sealed class UnionAsyncIteratorN<TSource> : UnionAsyncIterator<TSource>
        {
            private readonly SingleLinkedNode<IAsyncEnumerable<TSource>> _sources;
            private readonly int _headIndex;

            public UnionAsyncIteratorN(SingleLinkedNode<IAsyncEnumerable<TSource>> sources, int headIndex, IEqualityComparer<TSource> comparer)
                : base(comparer)
            {
                Debug.Assert(headIndex >= 2);
                Debug.Assert(sources?.GetCount() == headIndex + 1);

                _sources = sources;
                _headIndex = headIndex;
            }

            public override AsyncIterator<TSource> Clone() => new UnionAsyncIteratorN<TSource>(_sources, _headIndex, _comparer);

            internal override IAsyncEnumerable<TSource> GetEnumerable(int index) => index > _headIndex ? null : _sources.GetNode(_headIndex - index).Item;

            internal override UnionAsyncIterator<TSource> Union(IAsyncEnumerable<TSource> next)
            {
                if (_headIndex == int.MaxValue - 2)
                {
                    // In the unlikely case of this many unions, if we produced a UnionIteratorN
                    // with int.MaxValue then state would overflow before it matched it's index.
                    // So we use the naïve approach of just having a left and right sequence.
                    return new UnionAsyncIterator2<TSource>(this, next, _comparer);
                }

                return new UnionAsyncIteratorN<TSource>(_sources.Add(next), _headIndex + 1, _comparer);
            }
        }
    }
}
