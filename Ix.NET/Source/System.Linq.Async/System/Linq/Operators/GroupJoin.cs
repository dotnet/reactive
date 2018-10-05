// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IAsyncEnumerable<TInner>, TResult> resultSelector)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return outer.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IAsyncEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new GroupJoinAsyncEnumerable<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        public static IAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, Task<TKey>> outerKeySelector, Func<TInner, Task<TKey>> innerKeySelector, Func<TOuter, IAsyncEnumerable<TInner>, Task<TResult>> resultSelector)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return outer.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, Task<TKey>> outerKeySelector, Func<TInner, Task<TKey>> innerKeySelector, Func<TOuter, IAsyncEnumerable<TInner>, Task<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

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

            public IAsyncEnumerator<TResult> GetAsyncEnumerator()
                => new GroupJoinAsyncEnumerator(
                    _outer.GetAsyncEnumerator(),
                    _inner,
                    _outerKeySelector,
                    _innerKeySelector,
                    _resultSelector,
                    _comparer);

            private sealed class GroupJoinAsyncEnumerator : IAsyncEnumerator<TResult>
            {
                private readonly IEqualityComparer<TKey> _comparer;
                private readonly IAsyncEnumerable<TInner> _inner;
                private readonly Func<TInner, TKey> _innerKeySelector;
                private readonly IAsyncEnumerator<TOuter> _outer;
                private readonly Func<TOuter, TKey> _outerKeySelector;
                private readonly Func<TOuter, IAsyncEnumerable<TInner>, TResult> _resultSelector;

                private Internal.Lookup<TKey, TInner> _lookup;

                public GroupJoinAsyncEnumerator(
                    IAsyncEnumerator<TOuter> outer,
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

                public async ValueTask<bool> MoveNextAsync()
                {
                    // nothing to do 
                    if (!await _outer.MoveNextAsync().ConfigureAwait(false))
                    {
                        return false;
                    }

                    if (_lookup == null)
                    {
                        _lookup = await Internal.Lookup<TKey, TInner>.CreateForJoinAsync(_inner, _innerKeySelector, _comparer).ConfigureAwait(false);
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

            public IAsyncEnumerator<TResult> GetAsyncEnumerator()
                => new GroupJoinAsyncEnumeratorWithTask(
                    _outer.GetAsyncEnumerator(),
                    _inner,
                    _outerKeySelector,
                    _innerKeySelector,
                    _resultSelector,
                    _comparer);

            private sealed class GroupJoinAsyncEnumeratorWithTask : IAsyncEnumerator<TResult>
            {
                private readonly IEqualityComparer<TKey> _comparer;
                private readonly IAsyncEnumerable<TInner> _inner;
                private readonly Func<TInner, Task<TKey>> _innerKeySelector;
                private readonly IAsyncEnumerator<TOuter> _outer;
                private readonly Func<TOuter, Task<TKey>> _outerKeySelector;
                private readonly Func<TOuter, IAsyncEnumerable<TInner>, Task<TResult>> _resultSelector;

                private Internal.LookupWithTask<TKey, TInner> _lookup;

                public GroupJoinAsyncEnumeratorWithTask(
                    IAsyncEnumerator<TOuter> outer,
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

                public async ValueTask<bool> MoveNextAsync()
                {
                    // nothing to do 
                    if (!await _outer.MoveNextAsync().ConfigureAwait(false))
                    {
                        return false;
                    }

                    if (_lookup == null)
                    {
                        _lookup = await Internal.LookupWithTask<TKey, TInner>.CreateForJoinAsync(_inner, _innerKeySelector, _comparer).ConfigureAwait(false);
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
