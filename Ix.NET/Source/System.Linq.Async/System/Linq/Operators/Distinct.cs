﻿// Licensed to the .NET Foundation under one or more agreements.
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
        public static IAsyncEnumerable<TSource> Distinct<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return new DistinctAsyncIterator<TSource>(source, comparer: null);
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource>(this IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return new DistinctAsyncIterator<TSource>(source, comparer);
        }

        private sealed class DistinctAsyncIterator<TSource> : AsyncIterator<TSource>, IAsyncIListProvider<TSource>
        {
            private readonly IEqualityComparer<TSource> _comparer;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;
            private Set<TSource> _set;

            public DistinctAsyncIterator(IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
            {
                Debug.Assert(source != null);

                _source = source;
                _comparer = comparer;
            }

            public async Task<TSource[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var s = await FillSetAsync(cancellationToken).ConfigureAwait(false);
                return s.ToArray();
            }

            public async Task<List<TSource>> ToListAsync(CancellationToken cancellationToken)
            {
                var s = await FillSetAsync(cancellationToken).ConfigureAwait(false);
                return s.ToList();
            }

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return TaskExt.MinusOne;
                }

                return Core();

                async Task<int> Core()
                {
                    var s = await FillSetAsync(cancellationToken).ConfigureAwait(false);
                    return s.Count;
                }
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new DistinctAsyncIterator<TSource>(_source, _comparer);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                    _set = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        if (!await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            await DisposeAsync().ConfigureAwait(false);
                            return false;
                        }

                        var element = _enumerator.Current;
                        _set = new Set<TSource>(_comparer);
                        _set.Add(element);
                        _current = element;

                        _state = AsyncIteratorState.Iterating;
                        return true;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            element = _enumerator.Current;
                            if (_set.Add(element))
                            {
                                _current = element;
                                return true;
                            }
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }

            private Task<Set<TSource>> FillSetAsync(CancellationToken cancellationToken)
            {
                return AsyncEnumerableHelpers.ToSet(_source, _comparer, cancellationToken);
            }
        }
    }
}