// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IAsyncEnumerable<TInner>, TResult> resultSelector) =>
            GroupJoin(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer: null);

        public static IAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IAsyncEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (outer == null)
                throw Error.ArgumentNull(nameof(outer));
            if (inner == null)
                throw Error.ArgumentNull(nameof(inner));
            if (outerKeySelector == null)
                throw Error.ArgumentNull(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw Error.ArgumentNull(nameof(innerKeySelector));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

#if CSHARP8 && USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TResult> Core(CancellationToken cancellationToken)
            {
                await using (var e = outer.GetAsyncEnumerator(cancellationToken).ConfigureAwait(false))
                {
                    if (await e.MoveNextAsync())
                    {
                        var lookup = await Internal.Lookup<TKey, TInner>.CreateForJoinAsync(inner, innerKeySelector, comparer, cancellationToken).ConfigureAwait(false);

                        do
                        {
                            var item = e.Current;
                            var outerKey = outerKeySelector(item);
                            yield return resultSelector(item, lookup[outerKey].ToAsyncEnumerable());
                        }
                        while (await e.MoveNextAsync());
                    }
                }
            }
#else
            return new GroupJoinAsyncEnumerable<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
#endif
        }

        public static IAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, ValueTask<TKey>> outerKeySelector, Func<TInner, ValueTask<TKey>> innerKeySelector, Func<TOuter, IAsyncEnumerable<TInner>, ValueTask<TResult>> resultSelector) =>
            GroupJoin<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer: null);

        public static IAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, ValueTask<TKey>> outerKeySelector, Func<TInner, ValueTask<TKey>> innerKeySelector, Func<TOuter, IAsyncEnumerable<TInner>, ValueTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (outer == null)
                throw Error.ArgumentNull(nameof(outer));
            if (inner == null)
                throw Error.ArgumentNull(nameof(inner));
            if (outerKeySelector == null)
                throw Error.ArgumentNull(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw Error.ArgumentNull(nameof(innerKeySelector));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

#if CSHARP8 && USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TResult> Core(CancellationToken cancellationToken)
            {
                await using (var e = outer.GetAsyncEnumerator(cancellationToken).ConfigureAwait(false))
                {
                    if (await e.MoveNextAsync())
                    {
                        var lookup = await Internal.LookupWithTask<TKey, TInner>.CreateForJoinAsync(inner, innerKeySelector, comparer, cancellationToken).ConfigureAwait(false);

                        do
                        {
                            var item = e.Current;
                            var outerKey = await outerKeySelector(item).ConfigureAwait(false);
                            yield return await resultSelector(item, lookup[outerKey].ToAsyncEnumerable()).ConfigureAwait(false);
                        }
                        while (await e.MoveNextAsync());
                    }
                }
            }
#else
            return new GroupJoinAsyncEnumerableWithTask<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
#endif
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, CancellationToken, ValueTask<TKey>> outerKeySelector, Func<TInner, CancellationToken, ValueTask<TKey>> innerKeySelector, Func<TOuter, IAsyncEnumerable<TInner>, CancellationToken, ValueTask<TResult>> resultSelector) =>
            GroupJoin<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer: null);

        public static IAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, CancellationToken, ValueTask<TKey>> outerKeySelector, Func<TInner, CancellationToken, ValueTask<TKey>> innerKeySelector, Func<TOuter, IAsyncEnumerable<TInner>, CancellationToken, ValueTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (outer == null)
                throw Error.ArgumentNull(nameof(outer));
            if (inner == null)
                throw Error.ArgumentNull(nameof(inner));
            if (outerKeySelector == null)
                throw Error.ArgumentNull(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw Error.ArgumentNull(nameof(innerKeySelector));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

#if CSHARP8 && USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TResult> Core(CancellationToken cancellationToken)
            {
                await using (var e = outer.GetAsyncEnumerator(cancellationToken).ConfigureAwait(false))
                {
                    if (await e.MoveNextAsync())
                    {
                        var lookup = await Internal.LookupWithTask<TKey, TInner>.CreateForJoinAsync(inner, innerKeySelector, comparer, cancellationToken).ConfigureAwait(false);

                        do
                        {
                            var item = e.Current;
                            var outerKey = await outerKeySelector(item, cancellationToken).ConfigureAwait(false);
                            yield return await resultSelector(item, lookup[outerKey].ToAsyncEnumerable(), cancellationToken).ConfigureAwait(false);
                        }
                        while (await e.MoveNextAsync());
                    }
                }
            }
#else
            return new GroupJoinAsyncEnumerableWithTaskAndCancellation<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
#endif
        }
#endif

#if !(CSHARP8 && USE_ASYNC_ITERATOR)
        private sealed class GroupJoinAsyncEnumerable<TOuter, TInner, TKey, TResult> : IAsyncEnumerable<TResult>
        {
            private readonly IEqualityComparer<TKey> _comparer;
            private readonly IAsyncEnumerable<TInner> _inner;
            private readonly Func<TInner, TKey> _innerKeySelector;
            private readonly IAsyncEnumerable<TOuter> _outer;
            private readonly Func<TOuter, TKey> _outerKeySelector;
            private readonly Func<TOuter, IAsyncEnumerable<TInner>, TResult> _resultSelector;

            public GroupJoinAsyncEnumerable(
                IAsyncEnumerable<TOuter> outer,
                IAsyncEnumerable<TInner> inner,
                Func<TOuter, TKey> outerKeySelector,
                Func<TInner, TKey> innerKeySelector,
                Func<TOuter, IAsyncEnumerable<TInner>, TResult> resultSelector,
                IEqualityComparer<TKey> comparer)
            {
                _outer = outer;
                _inner = inner;
                _outerKeySelector = outerKeySelector;
                _innerKeySelector = innerKeySelector;
                _resultSelector = resultSelector;
                _comparer = comparer;
            }

            public IAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested(); // NB: [LDM-2018-11-28] Equivalent to async iterator behavior.

                return new GroupJoinAsyncEnumerator(
                    _outer.GetAsyncEnumerator(cancellationToken),
                    _inner,
                    _outerKeySelector,
                    _innerKeySelector,
                    _resultSelector,
                    _comparer,
                    cancellationToken);
            }

            private sealed class GroupJoinAsyncEnumerator : IAsyncEnumerator<TResult>
            {
                private readonly IEqualityComparer<TKey> _comparer;
                private readonly IAsyncEnumerable<TInner> _inner;
                private readonly Func<TInner, TKey> _innerKeySelector;
                private readonly IAsyncEnumerator<TOuter> _outer;
                private readonly Func<TOuter, TKey> _outerKeySelector;
                private readonly Func<TOuter, IAsyncEnumerable<TInner>, TResult> _resultSelector;
                private readonly CancellationToken _cancellationToken;

                private Internal.Lookup<TKey, TInner> _lookup;

                public GroupJoinAsyncEnumerator(
                    IAsyncEnumerator<TOuter> outer,
                    IAsyncEnumerable<TInner> inner,
                    Func<TOuter, TKey> outerKeySelector,
                    Func<TInner, TKey> innerKeySelector,
                    Func<TOuter, IAsyncEnumerable<TInner>, TResult> resultSelector,
                    IEqualityComparer<TKey> comparer,
                    CancellationToken cancellationToken)
                {
                    _outer = outer;
                    _inner = inner;
                    _outerKeySelector = outerKeySelector;
                    _innerKeySelector = innerKeySelector;
                    _resultSelector = resultSelector;
                    _comparer = comparer;
                    _cancellationToken = cancellationToken;
                }

                public async ValueTask<bool> MoveNextAsync()
                {
                    // nothing to do 
                    if (!await _outer.MoveNextAsync().ConfigureAwait(false))
                    {
                        return false;
                    }

                    if (_lookup == null)
                    {
                        _lookup = await Internal.Lookup<TKey, TInner>.CreateForJoinAsync(_inner, _innerKeySelector, _comparer, _cancellationToken).ConfigureAwait(false);
                    }

                    var item = _outer.Current;

                    var outerKey = _outerKeySelector(item);
                    var inner = _lookup[outerKey].ToAsyncEnumerable();

                    Current = _resultSelector(item, inner);

                    return true;
                }

                public TResult Current { get; private set; }

                public ValueTask DisposeAsync() => _outer.DisposeAsync();
            }
        }

        private sealed class GroupJoinAsyncEnumerableWithTask<TOuter, TInner, TKey, TResult> : IAsyncEnumerable<TResult>
        {
            private readonly IEqualityComparer<TKey> _comparer;
            private readonly IAsyncEnumerable<TInner> _inner;
            private readonly Func<TInner, ValueTask<TKey>> _innerKeySelector;
            private readonly IAsyncEnumerable<TOuter> _outer;
            private readonly Func<TOuter, ValueTask<TKey>> _outerKeySelector;
            private readonly Func<TOuter, IAsyncEnumerable<TInner>, ValueTask<TResult>> _resultSelector;

            public GroupJoinAsyncEnumerableWithTask(
                IAsyncEnumerable<TOuter> outer,
                IAsyncEnumerable<TInner> inner,
                Func<TOuter, ValueTask<TKey>> outerKeySelector,
                Func<TInner, ValueTask<TKey>> innerKeySelector,
                Func<TOuter, IAsyncEnumerable<TInner>, ValueTask<TResult>> resultSelector,
                IEqualityComparer<TKey> comparer)
            {
                _outer = outer;
                _inner = inner;
                _outerKeySelector = outerKeySelector;
                _innerKeySelector = innerKeySelector;
                _resultSelector = resultSelector;
                _comparer = comparer;
            }

            public IAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested(); // NB: [LDM-2018-11-28] Equivalent to async iterator behavior.

                return new GroupJoinAsyncEnumeratorWithTask(
                    _outer.GetAsyncEnumerator(cancellationToken),
                    _inner,
                    _outerKeySelector,
                    _innerKeySelector,
                    _resultSelector,
                    _comparer,
                    cancellationToken);
            }

            private sealed class GroupJoinAsyncEnumeratorWithTask : IAsyncEnumerator<TResult>
            {
                private readonly IEqualityComparer<TKey> _comparer;
                private readonly IAsyncEnumerable<TInner> _inner;
                private readonly Func<TInner, ValueTask<TKey>> _innerKeySelector;
                private readonly IAsyncEnumerator<TOuter> _outer;
                private readonly Func<TOuter, ValueTask<TKey>> _outerKeySelector;
                private readonly Func<TOuter, IAsyncEnumerable<TInner>, ValueTask<TResult>> _resultSelector;
                private readonly CancellationToken _cancellationToken;

                private Internal.LookupWithTask<TKey, TInner> _lookup;

                public GroupJoinAsyncEnumeratorWithTask(
                    IAsyncEnumerator<TOuter> outer,
                    IAsyncEnumerable<TInner> inner,
                    Func<TOuter, ValueTask<TKey>> outerKeySelector,
                    Func<TInner, ValueTask<TKey>> innerKeySelector,
                    Func<TOuter, IAsyncEnumerable<TInner>, ValueTask<TResult>> resultSelector,
                    IEqualityComparer<TKey> comparer,
                    CancellationToken cancellationToken)
                {
                    _outer = outer;
                    _inner = inner;
                    _outerKeySelector = outerKeySelector;
                    _innerKeySelector = innerKeySelector;
                    _resultSelector = resultSelector;
                    _comparer = comparer;
                    _cancellationToken = cancellationToken;
                }

                public async ValueTask<bool> MoveNextAsync()
                {
                    // nothing to do 
                    if (!await _outer.MoveNextAsync().ConfigureAwait(false))
                    {
                        return false;
                    }

                    if (_lookup == null)
                    {
                        _lookup = await Internal.LookupWithTask<TKey, TInner>.CreateForJoinAsync(_inner, _innerKeySelector, _comparer, _cancellationToken).ConfigureAwait(false);
                    }

                    var item = _outer.Current;

                    var outerKey = await _outerKeySelector(item).ConfigureAwait(false);
                    var inner = _lookup[outerKey].ToAsyncEnumerable();

                    Current = await _resultSelector(item, inner).ConfigureAwait(false);

                    return true;
                }

                public TResult Current { get; private set; }

                public ValueTask DisposeAsync() => _outer.DisposeAsync();
            }
        }

#if !NO_DEEP_CANCELLATION
        private sealed class GroupJoinAsyncEnumerableWithTaskAndCancellation<TOuter, TInner, TKey, TResult> : IAsyncEnumerable<TResult>
        {
            private readonly IEqualityComparer<TKey> _comparer;
            private readonly IAsyncEnumerable<TInner> _inner;
            private readonly Func<TInner, CancellationToken, ValueTask<TKey>> _innerKeySelector;
            private readonly IAsyncEnumerable<TOuter> _outer;
            private readonly Func<TOuter, CancellationToken, ValueTask<TKey>> _outerKeySelector;
            private readonly Func<TOuter, IAsyncEnumerable<TInner>, CancellationToken, ValueTask<TResult>> _resultSelector;

            public GroupJoinAsyncEnumerableWithTaskAndCancellation(
                IAsyncEnumerable<TOuter> outer,
                IAsyncEnumerable<TInner> inner,
                Func<TOuter, CancellationToken, ValueTask<TKey>> outerKeySelector,
                Func<TInner, CancellationToken, ValueTask<TKey>> innerKeySelector,
                Func<TOuter, IAsyncEnumerable<TInner>, CancellationToken, ValueTask<TResult>> resultSelector,
                IEqualityComparer<TKey> comparer)
            {
                _outer = outer;
                _inner = inner;
                _outerKeySelector = outerKeySelector;
                _innerKeySelector = innerKeySelector;
                _resultSelector = resultSelector;
                _comparer = comparer;
            }

            public IAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested(); // NB: [LDM-2018-11-28] Equivalent to async iterator behavior.

                return new GroupJoinAsyncEnumeratorWithTask(
                    _outer.GetAsyncEnumerator(cancellationToken),
                    _inner,
                    _outerKeySelector,
                    _innerKeySelector,
                    _resultSelector,
                    _comparer,
                    cancellationToken);
            }

            private sealed class GroupJoinAsyncEnumeratorWithTask : IAsyncEnumerator<TResult>
            {
                private readonly IEqualityComparer<TKey> _comparer;
                private readonly IAsyncEnumerable<TInner> _inner;
                private readonly Func<TInner, CancellationToken, ValueTask<TKey>> _innerKeySelector;
                private readonly IAsyncEnumerator<TOuter> _outer;
                private readonly Func<TOuter, CancellationToken, ValueTask<TKey>> _outerKeySelector;
                private readonly Func<TOuter, IAsyncEnumerable<TInner>, CancellationToken, ValueTask<TResult>> _resultSelector;
                private readonly CancellationToken _cancellationToken;

                private Internal.LookupWithTask<TKey, TInner> _lookup;

                public GroupJoinAsyncEnumeratorWithTask(
                    IAsyncEnumerator<TOuter> outer,
                    IAsyncEnumerable<TInner> inner,
                    Func<TOuter, CancellationToken, ValueTask<TKey>> outerKeySelector,
                    Func<TInner, CancellationToken, ValueTask<TKey>> innerKeySelector,
                    Func<TOuter, IAsyncEnumerable<TInner>, CancellationToken, ValueTask<TResult>> resultSelector,
                    IEqualityComparer<TKey> comparer,
                    CancellationToken cancellationToken)
                {
                    _outer = outer;
                    _inner = inner;
                    _outerKeySelector = outerKeySelector;
                    _innerKeySelector = innerKeySelector;
                    _resultSelector = resultSelector;
                    _comparer = comparer;
                    _cancellationToken = cancellationToken;
                }

                public async ValueTask<bool> MoveNextAsync()
                {
                    // nothing to do 
                    if (!await _outer.MoveNextAsync().ConfigureAwait(false))
                    {
                        return false;
                    }

                    if (_lookup == null)
                    {
                        _lookup = await Internal.LookupWithTask<TKey, TInner>.CreateForJoinAsync(_inner, _innerKeySelector, _comparer, _cancellationToken).ConfigureAwait(false);
                    }

                    var item = _outer.Current;

                    var outerKey = await _outerKeySelector(item, _cancellationToken).ConfigureAwait(false);
                    var inner = _lookup[outerKey].ToAsyncEnumerable();

                    Current = await _resultSelector(item, inner, _cancellationToken).ConfigureAwait(false);

                    return true;
                }

                public TResult Current { get; private set; }

                public ValueTask DisposeAsync() => _outer.DisposeAsync();
            }
        }
#endif
#endif
    }
}
