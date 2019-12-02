// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        /// <summary>
        /// Returns an async-enumerable sequence that contains only distinct elements according to the keySelector.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the discriminator key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to retain distinct elements for.</param>
        /// <param name="keySelector">A function to compute the comparison key for each element.</param>
        /// <returns>An async-enumerable sequence only containing the distinct elements, based on a computed key value, from the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        /// <remarks>Usage of this operator should be considered carefully due to the maintenance of an internal lookup structure which can grow large.</remarks>
        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctCore(source, keySelector, comparer: null);
        }

        /// <summary>
        /// Returns an async-enumerable sequence that contains only distinct elements according to the keySelector and the comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the discriminator key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to retain distinct elements for.</param>
        /// <param name="keySelector">A function to compute the comparison key for each element.</param>
        /// <param name="comparer">Equality comparer for source elements.</param>
        /// <returns>An async-enumerable sequence only containing the distinct elements, based on a computed key value, from the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>Usage of this operator should be considered carefully due to the maintenance of an internal lookup structure which can grow large.</remarks>
        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctCore(source, keySelector, comparer);
        }

        /// <summary>
        /// Returns an async-enumerable sequence that contains only distinct elements according to the asynchronous keySelector.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the discriminator key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to retain distinct elements for.</param>
        /// <param name="keySelector">An asynchronous function to compute the comparison key for each element.</param>
        /// <returns>An async-enumerable sequence only containing the distinct elements, based on a computed key value, from the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        /// <remarks>Usage of this operator should be considered carefully due to the maintenance of an internal lookup structure which can grow large.</remarks>
        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctCore<TSource, TKey>(source, keySelector, comparer: null);
        }

#if !NO_DEEP_CANCELLATION
        /// <summary>
        /// Returns an async-enumerable sequence that contains only distinct elements according to the asynchronous (cancellable) keySelector.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the discriminator key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to retain distinct elements for.</param>
        /// <param name="keySelector">An asynchronous (cancellable) function to compute the comparison key for each element.</param>
        /// <returns>An async-enumerable sequence only containing the distinct elements, based on a computed key value, from the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        /// <remarks>Usage of this operator should be considered carefully due to the maintenance of an internal lookup structure which can grow large.</remarks>
        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctCore<TSource, TKey>(source, keySelector, comparer: null);
        }
#endif

        /// <summary>
        /// Returns an async-enumerable sequence that contains only distinct elements according to the asynchronous keySelector and the comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the discriminator key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to retain distinct elements for.</param>
        /// <param name="keySelector">An asynchronous function to compute the comparison key for each element.</param>
        /// <param name="comparer">Equality comparer for source elements.</param>
        /// <returns>An async-enumerable sequence only containing the distinct elements, based on a computed key value, from the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>Usage of this operator should be considered carefully due to the maintenance of an internal lookup structure which can grow large.</remarks>
        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctCore(source, keySelector, comparer);
        }

#if !NO_DEEP_CANCELLATION
        /// <summary>
        /// Returns an async-enumerable sequence that contains only distinct elements according to the asynchronous (cancellable) keySelector and the comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the discriminator key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to retain distinct elements for.</param>
        /// <param name="keySelector">An asynchronous (cancellable) function to compute the comparison key for each element.</param>
        /// <param name="comparer">Equality comparer for source elements.</param>
        /// <returns>An async-enumerable sequence only containing the distinct elements, based on a computed key value, from the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>Usage of this operator should be considered carefully due to the maintenance of an internal lookup structure which can grow large.</remarks>
        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctCore(source, keySelector, comparer);
        }
#endif

        private static IAsyncEnumerable<TSource> DistinctCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            return new DistinctAsyncIterator<TSource, TKey>(source, keySelector, comparer);
        }

        private static IAsyncEnumerable<TSource> DistinctCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        {
            return new DistinctAsyncIteratorWithTask<TSource, TKey>(source, keySelector, comparer);
        }

#if !NO_DEEP_CANCELLATION
        private static IAsyncEnumerable<TSource> DistinctCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        {
            return new DistinctAsyncIteratorWithTaskAndCancellation<TSource, TKey>(source, keySelector, comparer);
        }
#endif

        private sealed class DistinctAsyncIterator<TSource, TKey> : AsyncIterator<TSource>, IAsyncIListProvider<TSource>
        {
            private readonly IEqualityComparer<TKey>? _comparer;
            private readonly Func<TSource, TKey> _keySelector;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource>? _enumerator;
            private Set<TKey>? _set;

            public DistinctAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
            {
                _source = source;
                _keySelector = keySelector;
                _comparer = comparer;
            }

            public async ValueTask<TSource[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var s = await FillSetAsync(cancellationToken).ConfigureAwait(false);
                return s.ToArray();
            }

            public async ValueTask<List<TSource>> ToListAsync(CancellationToken cancellationToken)
            {
                var s = await FillSetAsync(cancellationToken).ConfigureAwait(false);
                return s;
            }

            public async ValueTask<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
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

            public override AsyncIteratorBase<TSource> Clone()
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
                        _set = new Set<TKey>(_comparer);
                        _set.Add(_keySelector(element));
                        _current = element;

                        _state = AsyncIteratorState.Iterating;
                        return true;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator!.MoveNextAsync().ConfigureAwait(false))
                        {
                            element = _enumerator.Current;

                            if (_set!.Add(_keySelector(element)))
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
            private readonly IEqualityComparer<TKey>? _comparer;
            private readonly Func<TSource, ValueTask<TKey>> _keySelector;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource>? _enumerator;
            private Set<TKey>? _set;

            public DistinctAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey>? comparer)
            {
                _source = source;
                _keySelector = keySelector;
                _comparer = comparer;
            }

            public async ValueTask<TSource[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var s = await FillSetAsync(cancellationToken).ConfigureAwait(false);
                return s.ToArray();
            }

            public async ValueTask<List<TSource>> ToListAsync(CancellationToken cancellationToken)
            {
                var s = await FillSetAsync(cancellationToken).ConfigureAwait(false);
                return s;
            }

            public async ValueTask<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
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

            public override AsyncIteratorBase<TSource> Clone()
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
                        _set = new Set<TKey>(_comparer);
                        _set.Add(await _keySelector(element).ConfigureAwait(false));
                        _current = element;

                        _state = AsyncIteratorState.Iterating;
                        return true;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator!.MoveNextAsync().ConfigureAwait(false))
                        {
                            element = _enumerator.Current;

                            if (_set!.Add(await _keySelector(element).ConfigureAwait(false)))
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

            private async ValueTask<List<TSource>> FillSetAsync(CancellationToken cancellationToken)
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

#if !NO_DEEP_CANCELLATION
        private sealed class DistinctAsyncIteratorWithTaskAndCancellation<TSource, TKey> : AsyncIterator<TSource>, IAsyncIListProvider<TSource>
        {
            private readonly IEqualityComparer<TKey>? _comparer;
            private readonly Func<TSource, CancellationToken, ValueTask<TKey>> _keySelector;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource>? _enumerator;
            private Set<TKey>? _set;

            public DistinctAsyncIteratorWithTaskAndCancellation(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey>? comparer)
            {
                _source = source;
                _keySelector = keySelector;
                _comparer = comparer;
            }

            public async ValueTask<TSource[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var s = await FillSetAsync(cancellationToken).ConfigureAwait(false);
                return s.ToArray();
            }

            public async ValueTask<List<TSource>> ToListAsync(CancellationToken cancellationToken)
            {
                var s = await FillSetAsync(cancellationToken).ConfigureAwait(false);
                return s;
            }

            public async ValueTask<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
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

                        if (s.Add(await _keySelector(item, cancellationToken).ConfigureAwait(false)))
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

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new DistinctAsyncIteratorWithTaskAndCancellation<TSource, TKey>(_source, _keySelector, _comparer);
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
                        _set = new Set<TKey>(_comparer);
                        _set.Add(await _keySelector(element, _cancellationToken).ConfigureAwait(false));
                        _current = element;

                        _state = AsyncIteratorState.Iterating;
                        return true;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator!.MoveNextAsync().ConfigureAwait(false))
                        {
                            element = _enumerator.Current;

                            if (_set!.Add(await _keySelector(element, _cancellationToken).ConfigureAwait(false)))
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

            private async ValueTask<List<TSource>> FillSetAsync(CancellationToken cancellationToken)
            {
                var s = new Set<TKey>(_comparer);
                var r = new List<TSource>();

                var enu = _source.GetAsyncEnumerator(cancellationToken);

                try
                {
                    while (await enu.MoveNextAsync().ConfigureAwait(false))
                    {
                        var item = enu.Current;

                        if (s.Add(await _keySelector(item, cancellationToken).ConfigureAwait(false)))
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
#endif
    }
}
