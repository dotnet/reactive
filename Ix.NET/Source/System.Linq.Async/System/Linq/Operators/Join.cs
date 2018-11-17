// Licensed to the .NET Foundation under one or more agreements.
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
        public static IAsyncEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
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

            return new JoinAsyncIterator<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer: null);
        }

        public static IAsyncEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
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

            return new JoinAsyncIterator<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        public static IAsyncEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, ValueTask<TKey>> outerKeySelector, Func<TInner, ValueTask<TKey>> innerKeySelector, Func<TOuter, TInner, ValueTask<TResult>> resultSelector)
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

            return new JoinAsyncIteratorWithTask<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer: null);
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, CancellationToken, ValueTask<TKey>> outerKeySelector, Func<TInner, CancellationToken, ValueTask<TKey>> innerKeySelector, Func<TOuter, TInner, CancellationToken, ValueTask<TResult>> resultSelector)
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

            return new JoinAsyncIteratorWithTaskAndCancellation<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer: null);
        }
#endif

        public static IAsyncEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, ValueTask<TKey>> outerKeySelector, Func<TInner, ValueTask<TKey>> innerKeySelector, Func<TOuter, TInner, ValueTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
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

            return new JoinAsyncIteratorWithTask<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, CancellationToken, ValueTask<TKey>> outerKeySelector, Func<TInner, CancellationToken, ValueTask<TKey>> innerKeySelector, Func<TOuter, TInner, CancellationToken, ValueTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
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

            return new JoinAsyncIteratorWithTaskAndCancellation<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }
#endif

        internal sealed class JoinAsyncIterator<TOuter, TInner, TKey, TResult> : AsyncIterator<TResult>
        {
            private readonly IAsyncEnumerable<TOuter> _outer;
            private readonly IAsyncEnumerable<TInner> _inner;
            private readonly Func<TOuter, TKey> _outerKeySelector;
            private readonly Func<TInner, TKey> _innerKeySelector;
            private readonly Func<TOuter, TInner, TResult> _resultSelector;
            private readonly IEqualityComparer<TKey> _comparer;

            private IAsyncEnumerator<TOuter> _outerEnumerator;

            public JoinAsyncIterator(IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(outer != null);
                Debug.Assert(inner != null);
                Debug.Assert(outerKeySelector != null);
                Debug.Assert(innerKeySelector != null);
                Debug.Assert(resultSelector != null);

                _outer = outer;
                _inner = inner;
                _outerKeySelector = outerKeySelector;
                _innerKeySelector = innerKeySelector;
                _resultSelector = resultSelector;
                _comparer = comparer;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new JoinAsyncIterator<TOuter, TInner, TKey, TResult>(_outer, _inner, _outerKeySelector, _innerKeySelector, _resultSelector, _comparer);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_outerEnumerator != null)
                {
                    await _outerEnumerator.DisposeAsync().ConfigureAwait(false);
                    _outerEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            // State machine vars
            private Internal.Lookup<TKey, TInner> _lookup;
            private int _count;
            private TInner[] _elements;
            private int _index;
            private TOuter _item;
            private int _mode;

            private const int State_If = 1;
            private const int State_DoLoop = 2;
            private const int State_For = 3;
            private const int State_While = 4;

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _outerEnumerator = _outer.GetAsyncEnumerator(_cancellationToken);
                        _mode = State_If;
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (_mode)
                        {
                            case State_If:
                                if (await _outerEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    _lookup = await Internal.Lookup<TKey, TInner>.CreateForJoinAsync(_inner, _innerKeySelector, _comparer, _cancellationToken).ConfigureAwait(false);

                                    if (_lookup.Count != 0)
                                    {
                                        _mode = State_DoLoop;
                                        goto case State_DoLoop;
                                    }
                                }

                                break;

                            case State_DoLoop:
                                _item = _outerEnumerator.Current;
                                var g = _lookup.GetGrouping(_outerKeySelector(_item), create: false);
                                if (g != null)
                                {
                                    _count = g._count;
                                    _elements = g._elements;
                                    _index = 0;
                                    _mode = State_For;
                                    goto case State_For;
                                }

                                // advance to while
                                _mode = State_While;
                                goto case State_While;

                            case State_For:
                                _current = _resultSelector(_item, _elements[_index]);
                                _index++;
                                if (_index == _count)
                                {
                                    _mode = State_While;
                                }

                                return true;

                            case State_While:
                                var hasNext = await _outerEnumerator.MoveNextAsync().ConfigureAwait(false);
                                if (hasNext)
                                {
                                    goto case State_DoLoop;
                                }

                                break;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }

        internal sealed class JoinAsyncIteratorWithTask<TOuter, TInner, TKey, TResult> : AsyncIterator<TResult>
        {
            private readonly IAsyncEnumerable<TOuter> _outer;
            private readonly IAsyncEnumerable<TInner> _inner;
            private readonly Func<TOuter, ValueTask<TKey>> _outerKeySelector;
            private readonly Func<TInner, ValueTask<TKey>> _innerKeySelector;
            private readonly Func<TOuter, TInner, ValueTask<TResult>> _resultSelector;
            private readonly IEqualityComparer<TKey> _comparer;

            private IAsyncEnumerator<TOuter> _outerEnumerator;

            public JoinAsyncIteratorWithTask(IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, ValueTask<TKey>> outerKeySelector, Func<TInner, ValueTask<TKey>> innerKeySelector, Func<TOuter, TInner, ValueTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(outer != null);
                Debug.Assert(inner != null);
                Debug.Assert(outerKeySelector != null);
                Debug.Assert(innerKeySelector != null);
                Debug.Assert(resultSelector != null);

                _outer = outer;
                _inner = inner;
                _outerKeySelector = outerKeySelector;
                _innerKeySelector = innerKeySelector;
                _resultSelector = resultSelector;
                _comparer = comparer;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new JoinAsyncIteratorWithTask<TOuter, TInner, TKey, TResult>(_outer, _inner, _outerKeySelector, _innerKeySelector, _resultSelector, _comparer);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_outerEnumerator != null)
                {
                    await _outerEnumerator.DisposeAsync().ConfigureAwait(false);
                    _outerEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            // State machine vars
            private Internal.LookupWithTask<TKey, TInner> _lookup;
            private int _count;
            private TInner[] _elements;
            private int _index;
            private TOuter _item;
            private int _mode;

            private const int State_If = 1;
            private const int State_DoLoop = 2;
            private const int State_For = 3;
            private const int State_While = 4;

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _outerEnumerator = _outer.GetAsyncEnumerator(_cancellationToken);
                        _mode = State_If;
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (_mode)
                        {
                            case State_If:
                                if (await _outerEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    _lookup = await Internal.LookupWithTask<TKey, TInner>.CreateForJoinAsync(_inner, _innerKeySelector, _comparer, _cancellationToken).ConfigureAwait(false);

                                    if (_lookup.Count != 0)
                                    {
                                        _mode = State_DoLoop;
                                        goto case State_DoLoop;
                                    }
                                }

                                break;

                            case State_DoLoop:
                                _item = _outerEnumerator.Current;
                                var g = _lookup.GetGrouping(await _outerKeySelector(_item).ConfigureAwait(false), create: false);
                                if (g != null)
                                {
                                    _count = g._count;
                                    _elements = g._elements;
                                    _index = 0;
                                    _mode = State_For;
                                    goto case State_For;
                                }

                                // advance to while
                                _mode = State_While;
                                goto case State_While;

                            case State_For:
                                _current = await _resultSelector(_item, _elements[_index]).ConfigureAwait(false);
                                _index++;
                                if (_index == _count)
                                {
                                    _mode = State_While;
                                }

                                return true;

                            case State_While:
                                var hasNext = await _outerEnumerator.MoveNextAsync().ConfigureAwait(false);
                                if (hasNext)
                                {
                                    goto case State_DoLoop;
                                }

                                break;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }

#if !NO_DEEP_CANCELLATION
        internal sealed class JoinAsyncIteratorWithTaskAndCancellation<TOuter, TInner, TKey, TResult> : AsyncIterator<TResult>
        {
            private readonly IAsyncEnumerable<TOuter> _outer;
            private readonly IAsyncEnumerable<TInner> _inner;
            private readonly Func<TOuter, CancellationToken, ValueTask<TKey>> _outerKeySelector;
            private readonly Func<TInner, CancellationToken, ValueTask<TKey>> _innerKeySelector;
            private readonly Func<TOuter, TInner, CancellationToken, ValueTask<TResult>> _resultSelector;
            private readonly IEqualityComparer<TKey> _comparer;

            private IAsyncEnumerator<TOuter> _outerEnumerator;

            public JoinAsyncIteratorWithTaskAndCancellation(IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, CancellationToken, ValueTask<TKey>> outerKeySelector, Func<TInner, CancellationToken, ValueTask<TKey>> innerKeySelector, Func<TOuter, TInner, CancellationToken, ValueTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
            {
                Debug.Assert(outer != null);
                Debug.Assert(inner != null);
                Debug.Assert(outerKeySelector != null);
                Debug.Assert(innerKeySelector != null);
                Debug.Assert(resultSelector != null);

                _outer = outer;
                _inner = inner;
                _outerKeySelector = outerKeySelector;
                _innerKeySelector = innerKeySelector;
                _resultSelector = resultSelector;
                _comparer = comparer;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new JoinAsyncIteratorWithTaskAndCancellation<TOuter, TInner, TKey, TResult>(_outer, _inner, _outerKeySelector, _innerKeySelector, _resultSelector, _comparer);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_outerEnumerator != null)
                {
                    await _outerEnumerator.DisposeAsync().ConfigureAwait(false);
                    _outerEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            // State machine vars
            private Internal.LookupWithTask<TKey, TInner> _lookup;
            private int _count;
            private TInner[] _elements;
            private int _index;
            private TOuter _item;
            private int _mode;

            private const int State_If = 1;
            private const int State_DoLoop = 2;
            private const int State_For = 3;
            private const int State_While = 4;

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _outerEnumerator = _outer.GetAsyncEnumerator(_cancellationToken);
                        _mode = State_If;
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (_mode)
                        {
                            case State_If:
                                if (await _outerEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    _lookup = await Internal.LookupWithTask<TKey, TInner>.CreateForJoinAsync(_inner, _innerKeySelector, _comparer, _cancellationToken).ConfigureAwait(false);

                                    if (_lookup.Count != 0)
                                    {
                                        _mode = State_DoLoop;
                                        goto case State_DoLoop;
                                    }
                                }

                                break;

                            case State_DoLoop:
                                _item = _outerEnumerator.Current;
                                var g = _lookup.GetGrouping(await _outerKeySelector(_item, _cancellationToken).ConfigureAwait(false), create: false);
                                if (g != null)
                                {
                                    _count = g._count;
                                    _elements = g._elements;
                                    _index = 0;
                                    _mode = State_For;
                                    goto case State_For;
                                }

                                // advance to while
                                _mode = State_While;
                                goto case State_While;

                            case State_For:
                                _current = await _resultSelector(_item, _elements[_index], _cancellationToken).ConfigureAwait(false);
                                _index++;
                                if (_index == _count)
                                {
                                    _mode = State_While;
                                }

                                return true;

                            case State_While:
                                var hasNext = await _outerEnumerator.MoveNextAsync().ConfigureAwait(false);
                                if (hasNext)
                                {
                                    goto case State_DoLoop;
                                }

                                break;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }
#endif
    }
}
