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

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));

            return DistinctUntilChangedCore<TSource, TKey>(source, keySelector, comparer: null);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (keySelector == null)
                throw Error.ArgumentNull(nameof(keySelector));
            if (comparer == null)
                throw Error.ArgumentNull(nameof(comparer));

            return DistinctUntilChangedCore(source, keySelector, comparer);
        }

        private static IAsyncEnumerable<TSource> DistinctUntilChangedCore<TSource>(IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            return new DistinctUntilChangedAsyncIterator<TSource>(source, comparer);
        }

        private static IAsyncEnumerable<TSource> DistinctUntilChangedCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new DistinctUntilChangedAsyncIterator<TSource, TKey>(source, keySelector, comparer);
        }

        private static IAsyncEnumerable<TSource> DistinctUntilChangedCore<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new DistinctUntilChangedAsyncIteratorWithTask<TSource, TKey>(source, keySelector, comparer);
        }

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

            public override AsyncIterator<TSource> Clone()
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

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(cancellationToken);
                        state = AsyncIteratorState.Iterating;
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
                                current = item;
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

            public override AsyncIterator<TSource> Clone()
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

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(cancellationToken);
                        state = AsyncIteratorState.Iterating;
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
                                current = item;
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
            private readonly Func<TSource, Task<TKey>> _keySelector;
            private readonly IAsyncEnumerable<TSource> _source;
            private TKey _currentKeyValue;

            private IAsyncEnumerator<TSource> _enumerator;
            private bool _hasCurrentKey;

            public DistinctUntilChangedAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, Task<TKey>> keySelector, IEqualityComparer<TKey> comparer)
            {
                _source = source;
                _keySelector = keySelector;
                _comparer = comparer ?? EqualityComparer<TKey>.Default;
            }

            public override AsyncIterator<TSource> Clone()
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

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(cancellationToken);
                        state = AsyncIteratorState.Iterating;
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
                                current = item;
                                return true;
                            }
                        }

                        break; // case
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
    }
}
