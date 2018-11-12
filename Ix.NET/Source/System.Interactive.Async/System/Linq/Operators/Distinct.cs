// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctCore(source, keySelector, comparer: null);
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctCore(source, keySelector, comparer);
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctCore<TSource, TKey>(source, keySelector, comparer: null);
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (comparer == null)
                throw Error.ArgumentNull(nameof(comparer));

            return DistinctCore(source, keySelector, comparer);
        }

        private static IAsyncEnumerable<TSource> DistinctCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new DistinctAsyncIterator<TSource, TKey>(source, keySelector, comparer);
        }

        private static IAsyncEnumerable<TSource> DistinctCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new DistinctAsyncIteratorWithTask<TSource, TKey>(source, keySelector, comparer);
        }

        private sealed class DistinctAsyncIterator<TSource, TKey> : AsyncIterator<TSource>, IAsyncIListProvider<TSource>
        {
            private readonly IEqualityComparer<TKey> _comparer;
            private readonly Func<TSource, TKey> _keySelector;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;
            private Set<TKey> _set;

            public DistinctAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(source != null);
                Debug.Assert(keySelector != null);

                _source = source;
                _keySelector = keySelector;
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
                return s;
            }

            public async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return -1;
                }

                var count = 0;
                var s = new Set<TKey>(_comparer);

                var enu = _source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    while (await enu.MoveNextAsync().ConfigureAwait(false))
                    {
                        var item = enu.Current;
                        if (s.Add(_keySelector(item)))
                        {
                            count++;
                        }
                    }
                }
                finally
                {
                    await enu.DisposeAsync().ConfigureAwait(false);
                }

                return count;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new DistinctAsyncIterator<TSource, TKey>(_source, _keySelector, _comparer);
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
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(cancellationToken);

                        if (!await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            await DisposeAsync().ConfigureAwait(false);
                            return false;
                        }

                        var element = _enumerator.Current;
                        _set = new Set<TKey>(_comparer);
                        _set.Add(_keySelector(element));
                        current = element;

                        state = AsyncIteratorState.Iterating;
                        return true;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            element = _enumerator.Current;
                            if (_set.Add(_keySelector(element)))
                            {
                                current = element;
                                return true;
                            }
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }

            private async Task<List<TSource>> FillSetAsync(CancellationToken cancellationToken)
            {
                var s = new Set<TKey>(_comparer);
                var r = new List<TSource>();

                var enu = _source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    while (await enu.MoveNextAsync().ConfigureAwait(false))
                    {
                        var item = enu.Current;
                        if (s.Add(_keySelector(item)))
                        {
                            r.Add(item);
                        }
                    }
                }
                finally
                {
                    await enu.DisposeAsync().ConfigureAwait(false);
                }

                return r;
            }
        }

        private sealed class DistinctAsyncIteratorWithTask<TSource, TKey> : AsyncIterator<TSource>, IAsyncIListProvider<TSource>
        {
            private readonly IEqualityComparer<TKey> _comparer;
            private readonly Func<TSource, Task<TKey>> _keySelector;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;
            private Set<TKey> _set;

            public DistinctAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(source != null);
                Debug.Assert(keySelector != null);

                _source = source;
                _keySelector = keySelector;
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
                return s;
            }

            public async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    return -1;
                }

                var count = 0;
                var s = new Set<TKey>(_comparer);

                var enu = _source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    while (await enu.MoveNextAsync().ConfigureAwait(false))
                    {
                        var item = enu.Current;
                        if (s.Add(await _keySelector(item).ConfigureAwait(false)))
                        {
                            count++;
                        }
                    }
                }
                finally
                {
                    await enu.DisposeAsync().ConfigureAwait(false);
                }

                return count;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new DistinctAsyncIteratorWithTask<TSource, TKey>(_source, _keySelector, _comparer);
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
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(cancellationToken);

                        if (!await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            await DisposeAsync().ConfigureAwait(false);
                            return false;
                        }

                        var element = _enumerator.Current;
                        _set = new Set<TKey>(_comparer);
                        _set.Add(await _keySelector(element).ConfigureAwait(false));
                        current = element;

                        state = AsyncIteratorState.Iterating;
                        return true;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            element = _enumerator.Current;
                            if (_set.Add(await _keySelector(element).ConfigureAwait(false)))
                            {
                                current = element;
                                return true;
                            }
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }

            private async Task<List<TSource>> FillSetAsync(CancellationToken cancellationToken)
            {
                var s = new Set<TKey>(_comparer);
                var r = new List<TSource>();

                var enu = _source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    while (await enu.MoveNextAsync().ConfigureAwait(false))
                    {
                        var item = enu.Current;
                        if (s.Add(await _keySelector(item).ConfigureAwait(false)))
                        {
                            r.Add(item);
                        }
                    }
                }
                finally
                {
                    await enu.DisposeAsync().ConfigureAwait(false);
                }

                return r;
            }
        }
    }
}
