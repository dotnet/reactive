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
        public static IAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IAsyncEnumerable<TInner>, TResult> resultSelector)
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

            return new GroupJoinAsyncEnumerable<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer: null);
        }

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

            return new GroupJoinAsyncEnumerable<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        public static IAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, Task<TKey>> outerKeySelector, Func<TInner, Task<TKey>> innerKeySelector, Func<TOuter, IAsyncEnumerable<TInner>, Task<TResult>> resultSelector)
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

            return new GroupJoinAsyncEnumerableWithTask<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer: null);
        }

        public static IAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, Task<TKey>> outerKeySelector, Func<TInner, Task<TKey>> innerKeySelector, Func<TOuter, IAsyncEnumerable<TInner>, Task<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
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

            return new GroupJoinAsyncEnumerableWithTask<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

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
                => new GroupJoinAsyncEnumerator(
                    _outer.GetAsyncEnumerator(cancellationToken),
                    _inner,
                    _outerKeySelector,
                    _innerKeySelector,
                    _resultSelector,
                    _comparer,
                    cancellationToken);

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
            private readonly Func<TInner, Task<TKey>> _innerKeySelector;
            private readonly IAsyncEnumerable<TOuter> _outer;
            private readonly Func<TOuter, Task<TKey>> _outerKeySelector;
            private readonly Func<TOuter, IAsyncEnumerable<TInner>, Task<TResult>> _resultSelector;

            public GroupJoinAsyncEnumerableWithTask(
                IAsyncEnumerable<TOuter> outer,
                IAsyncEnumerable<TInner> inner,
                Func<TOuter, Task<TKey>> outerKeySelector,
                Func<TInner, Task<TKey>> innerKeySelector,
                Func<TOuter, IAsyncEnumerable<TInner>, Task<TResult>> resultSelector,
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
                => new GroupJoinAsyncEnumeratorWithTask(
                    _outer.GetAsyncEnumerator(cancellationToken),
                    _inner,
                    _outerKeySelector,
                    _innerKeySelector,
                    _resultSelector,
                    _comparer,
                    cancellationToken);

            private sealed class GroupJoinAsyncEnumeratorWithTask : IAsyncEnumerator<TResult>
            {
                private readonly IEqualityComparer<TKey> _comparer;
                private readonly IAsyncEnumerable<TInner> _inner;
                private readonly Func<TInner, Task<TKey>> _innerKeySelector;
                private readonly IAsyncEnumerator<TOuter> _outer;
                private readonly Func<TOuter, Task<TKey>> _outerKeySelector;
                private readonly Func<TOuter, IAsyncEnumerable<TInner>, Task<TResult>> _resultSelector;
                private readonly CancellationToken _cancellationToken;

                private Internal.LookupWithTask<TKey, TInner> _lookup;

                public GroupJoinAsyncEnumeratorWithTask(
                    IAsyncEnumerator<TOuter> outer,
                    IAsyncEnumerable<TInner> inner,
                    Func<TOuter, Task<TKey>> outerKeySelector,
                    Func<TInner, Task<TKey>> innerKeySelector,
                    Func<TOuter, IAsyncEnumerable<TInner>, Task<TResult>> resultSelector,
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
    }
}
