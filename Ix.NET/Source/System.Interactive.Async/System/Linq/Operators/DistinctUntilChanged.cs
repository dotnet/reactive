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
        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return DistinctUntilChangedCore(source, comparer: null);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource>(this IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return DistinctUntilChangedCore(source, comparer);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctUntilChangedCore(source, keySelector, comparer: null);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctUntilChangedCore(source, keySelector, comparer);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctUntilChangedCore<TSource, TKey>(source, keySelector, comparer: null);
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctUntilChangedCore<TSource, TKey>(source, keySelector, comparer: null);
        }
#endif

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctUntilChangedCore(source, keySelector, comparer);
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctUntilChangedCore(source, keySelector, comparer);
        }
#endif

        private static IAsyncEnumerable<TSource> DistinctUntilChangedCore<TSource>(IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
#if USE_ASYNC_ITERATOR
            if (comparer == null)
            {
                comparer = EqualityComparer<TSource>.Default;
            }

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var e = source.GetAsyncEnumerator(cancellationToken).ConfigureAwait(false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        yield break;
                    }

                    TSource latest = e.Current;

                    yield return latest;

                    while (await e.MoveNextAsync())
                    {
                        TSource item = e.Current;

                        if (!comparer.Equals(latest, item))
                        {
                            latest = item;

                            yield return latest;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }
            }
#else
            return new DistinctUntilChangedAsyncIterator<TSource>(source, comparer);
#endif
        }

        private static IAsyncEnumerable<TSource> DistinctUntilChangedCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
#if USE_ASYNC_ITERATOR
            if (comparer == null)
            {
                comparer = EqualityComparer<TKey>.Default;
            }

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var e = source.GetAsyncEnumerator(cancellationToken).ConfigureAwait(false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        yield break;
                    }

                    TSource item = e.Current;

                    TKey latestKey = keySelector(item);

                    yield return item;

                    while (await e.MoveNextAsync())
                    {
                        item = e.Current;

                        TKey currentKey = keySelector(item);

                        if (!comparer.Equals(latestKey, currentKey))
                        {
                            latestKey = currentKey;

                            yield return item;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }
            }
#else
            return new DistinctUntilChangedAsyncIterator<TSource, TKey>(source, keySelector, comparer);
#endif
        }

        private static IAsyncEnumerable<TSource> DistinctUntilChangedCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
#if USE_ASYNC_ITERATOR
            if (comparer == null)
            {
                comparer = EqualityComparer<TKey>.Default;
            }

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var e = source.GetAsyncEnumerator(cancellationToken).ConfigureAwait(false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        yield break;
                    }

                    TSource item = e.Current;

                    TKey latestKey = await keySelector(item).ConfigureAwait(false);

                    yield return item;

                    while (await e.MoveNextAsync())
                    {
                        item = e.Current;

                        TKey currentKey = await keySelector(item).ConfigureAwait(false);

                        if (!comparer.Equals(latestKey, currentKey))
                        {
                            latestKey = currentKey;

                            yield return item;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }
            }
#else
            return new DistinctUntilChangedAsyncIteratorWithTask<TSource, TKey>(source, keySelector, comparer);
#endif
        }

#if !NO_DEEP_CANCELLATION
        private static IAsyncEnumerable<TSource> DistinctUntilChangedCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
#if USE_ASYNC_ITERATOR
            if (comparer == null)
            {
                comparer = EqualityComparer<TKey>.Default;
            }

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var e = source.GetAsyncEnumerator(cancellationToken).ConfigureAwait(false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    if (!await e.MoveNextAsync())
                    {
                        yield break;
                    }

                    TSource item = e.Current;

                    TKey latestKey = await keySelector(item, cancellationToken).ConfigureAwait(false);

                    yield return item;

                    while (await e.MoveNextAsync())
                    {
                        item = e.Current;

                        TKey currentKey = await keySelector(item, cancellationToken).ConfigureAwait(false);

                        if (!comparer.Equals(latestKey, currentKey))
                        {
                            latestKey = currentKey;

                            yield return item;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync();
                }
            }
#else
            return new DistinctUntilChangedAsyncIteratorWithTaskAndCancellation<TSource, TKey>(source, keySelector, comparer);
#endif
        }
#endif

#if !USE_ASYNC_ITERATOR
        private sealed class DistinctUntilChangedAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IEqualityComparer<TSource> _comparer;
            private readonly IAsyncEnumerable<TSource> _source;

            private TSource _currentValue;
            private IAsyncEnumerator<TSource> _enumerator;
            private bool _hasCurrentValue;

            public DistinctUntilChangedAsyncIterator(IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
            {
                Debug.Assert(source != null);

                _source = source;
                _comparer = comparer ?? EqualityComparer<TSource>.Default;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new DistinctUntilChangedAsyncIterator<TSource>(_source, _comparer);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                    _currentValue = default;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;
                            var comparerEquals = false;

                            if (_hasCurrentValue)
                            {
                                comparerEquals = _comparer.Equals(_currentValue, item);
                            }

                            if (!_hasCurrentValue || !comparerEquals)
                            {
                                _hasCurrentValue = true;
                                _currentValue = item;
                                _current = item;
                                return true;
                            }
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class DistinctUntilChangedAsyncIterator<TSource, TKey> : AsyncIterator<TSource>
        {
            private readonly IEqualityComparer<TKey> _comparer;
            private readonly Func<TSource, TKey> _keySelector;
            private readonly IAsyncEnumerable<TSource> _source;
            private TKey _currentKeyValue;

            private IAsyncEnumerator<TSource> _enumerator;
            private bool _hasCurrentKey;

            public DistinctUntilChangedAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
            {
                _source = source;
                _keySelector = keySelector;
                _comparer = comparer ?? EqualityComparer<TKey>.Default;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new DistinctUntilChangedAsyncIterator<TSource, TKey>(_source, _keySelector, _comparer);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                    _currentKeyValue = default;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;
                            var key = _keySelector(item);
                            var comparerEquals = false;

                            if (_hasCurrentKey)
                            {
                                comparerEquals = _comparer.Equals(_currentKeyValue, key);
                            }

                            if (!_hasCurrentKey || !comparerEquals)
                            {
                                _hasCurrentKey = true;
                                _currentKeyValue = key;
                                _current = item;
                                return true;
                            }
                        }

                        break; // case
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class DistinctUntilChangedAsyncIteratorWithTask<TSource, TKey> : AsyncIterator<TSource>
        {
            private readonly IEqualityComparer<TKey> _comparer;
            private readonly Func<TSource, ValueTask<TKey>> _keySelector;
            private readonly IAsyncEnumerable<TSource> _source;
            private TKey _currentKeyValue;

            private IAsyncEnumerator<TSource> _enumerator;
            private bool _hasCurrentKey;

            public DistinctUntilChangedAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
            {
                _source = source;
                _keySelector = keySelector;
                _comparer = comparer ?? EqualityComparer<TKey>.Default;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new DistinctUntilChangedAsyncIteratorWithTask<TSource, TKey>(_source, _keySelector, _comparer);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                    _currentKeyValue = default;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;
                            var key = await _keySelector(item).ConfigureAwait(false);
                            var comparerEquals = false;

                            if (_hasCurrentKey)
                            {
                                comparerEquals = _comparer.Equals(_currentKeyValue, key);
                            }

                            if (!_hasCurrentKey || !comparerEquals)
                            {
                                _hasCurrentKey = true;
                                _currentKeyValue = key;
                                _current = item;
                                return true;
                            }
                        }

                        break; // case
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

#if !NO_DEEP_CANCELLATION
        private sealed class DistinctUntilChangedAsyncIteratorWithTaskAndCancellation<TSource, TKey> : AsyncIterator<TSource>
        {
            private readonly IEqualityComparer<TKey> _comparer;
            private readonly Func<TSource, CancellationToken, ValueTask<TKey>> _keySelector;
            private readonly IAsyncEnumerable<TSource> _source;
            private TKey _currentKeyValue;

            private IAsyncEnumerator<TSource> _enumerator;
            private bool _hasCurrentKey;

            public DistinctUntilChangedAsyncIteratorWithTaskAndCancellation(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
            {
                _source = source;
                _keySelector = keySelector;
                _comparer = comparer ?? EqualityComparer<TKey>.Default;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new DistinctUntilChangedAsyncIteratorWithTaskAndCancellation<TSource, TKey>(_source, _keySelector, _comparer);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                    _currentKeyValue = default;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;
                            var key = await _keySelector(item, _cancellationToken).ConfigureAwait(false);
                            var comparerEquals = false;

                            if (_hasCurrentKey)
                            {
                                comparerEquals = _comparer.Equals(_currentKeyValue, key);
                            }

                            if (!_hasCurrentKey || !comparerEquals)
                            {
                                _hasCurrentKey = true;
                                _currentKeyValue = key;
                                _current = item;
                                return true;
                            }
                        }

                        break; // case
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
#endif
#endif
    }
}
