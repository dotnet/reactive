// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
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

        internal sealed class AsyncEnumerableAdapter<T> : IAsyncEnumerable<T>
        {
            private readonly IEnumerable<T> _source;

            public AsyncEnumerableAdapter(IEnumerable<T> source)
            {
                _source = source;
            }

            public IAsyncEnumerator<T> GetEnumerator()
                => new AsyncEnumeratorAdapter(_source.GetEnumerator());

            private sealed class AsyncEnumeratorAdapter : IAsyncEnumerator<T>
            {
                private readonly IEnumerator<T> _enumerator;

                public AsyncEnumeratorAdapter(IEnumerator<T> enumerator)
                {
                    _enumerator = enumerator;
                }

                public Task<bool> MoveNext(CancellationToken cancellationToken)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    return Task.FromResult(_enumerator.MoveNext());
                }

                public T Current => _enumerator.Current;

                public void Dispose() => _enumerator.Dispose();
            }
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

            public IAsyncEnumerator<TResult> GetEnumerator()
                => new GroupJoinAsyncEnumerator(
                    _outer.GetEnumerator(),
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

                public async Task<bool> MoveNext(CancellationToken cancellationToken)
                {
                    // nothing to do 
                    if (!await _outer.MoveNext(cancellationToken)
                                     .ConfigureAwait(false))
                    {
                        return false;
                    }

                    if (_lookup == null)
                    {
                        _lookup = await Internal.Lookup<TKey, TInner>.CreateForJoinAsync(_inner, _innerKeySelector, _comparer, cancellationToken)
                                                .ConfigureAwait(false);
                    }

                    var item = _outer.Current;
                    Current = _resultSelector(item, new AsyncEnumerableAdapter<TInner>(_lookup[_outerKeySelector(item)]));
                    return true;
                }

                public TResult Current { get; private set; }

                public void Dispose()
                {
                    _outer.Dispose();
                }
            }
        }
    }
}