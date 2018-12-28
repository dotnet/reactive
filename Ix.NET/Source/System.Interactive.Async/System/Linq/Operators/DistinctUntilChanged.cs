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
            return new DistinctUntilChangedAsyncIterator<TSource>(source, comparer);
        }

        private static IAsyncEnumerable<TSource> DistinctUntilChangedCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new DistinctUntilChangedAsyncIterator<TSource, TKey>(source, keySelector, comparer);
        }

        private static IAsyncEnumerable<TSource> DistinctUntilChangedCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new DistinctUntilChangedAsyncIteratorWithTask<TSource, TKey>(source, keySelector, comparer);
        }

#if !NO_DEEP_CANCELLATION
        private static IAsyncEnumerable<TSource> DistinctUntilChangedCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new DistinctUntilChangedAsyncIteratorWithTaskAndCancellation<TSource, TKey>(source, keySelector, comparer);
        }
#endif

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
                            TSource item = _enumerator.Current;
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
                            TSource item = _enumerator.Current;
                            TKey key = _keySelector(item);
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
                            TSource item = _enumerator.Current;
                            TKey key = await _keySelector(item).ConfigureAwait(false);
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
                            TSource item = _enumerator.Current;
                            TKey key = await _keySelector(item, _cancellationToken).ConfigureAwait(false);
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
    }
}
