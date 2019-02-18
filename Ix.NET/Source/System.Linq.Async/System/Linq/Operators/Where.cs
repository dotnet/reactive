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
        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            if (source is AsyncIteratorBase<TSource> iterator)
            {
                return iterator.Where(predicate);
            }

            // TODO: Can we add array/list optimizations here, does it make sense?
            return new WhereEnumerableAsyncIterator<TSource>(source, predicate);
        }

        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

#if USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var index = -1;

                await foreach (var element in AsyncEnumerableExtensions.WithCancellation(source, cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        index++;
                    }

                    if (predicate(element, index))
                    {
                        yield return element;
                    }
                }
            }
#else
            return new WhereEnumerableWithIndexAsyncIterator<TSource>(source, predicate);
#endif
        }

        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            if (source is AsyncIteratorBase<TSource> iterator)
            {
                return iterator.Where(predicate);
            }

            // TODO: Can we add array/list optimizations here, does it make sense?
            return new WhereEnumerableAsyncIteratorWithTask<TSource>(source, predicate);
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            if (source is AsyncIteratorBase<TSource> iterator)
            {
                return iterator.Where(predicate);
            }

            // TODO: Can we add array/list optimizations here, does it make sense?
            return new WhereEnumerableAsyncIteratorWithTaskAndCancellation<TSource>(source, predicate);
        }
#endif

        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

#if USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var index = -1;

                await foreach (var element in AsyncEnumerableExtensions.WithCancellation(source, cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        index++;
                    }

                    if (await predicate(element, index).ConfigureAwait(false))
                    {
                        yield return element;
                    }
                }
            }
#else
            return new WhereEnumerableWithIndexAsyncIteratorWithTask<TSource>(source, predicate);
#endif
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

#if USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var index = -1;

                await foreach (var element in AsyncEnumerableExtensions.WithCancellation(source, cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        index++;
                    }

                    if (await predicate(element, index, cancellationToken).ConfigureAwait(false))
                    {
                        yield return element;
                    }
                }
            }
#else
            return new WhereEnumerableWithIndexAsyncIteratorWithTaskAndCancellation<TSource>(source, predicate);
#endif
        }
#endif

        internal sealed class WhereEnumerableAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, bool> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;
            private IAsyncEnumerator<TSource> _enumerator;

            public WhereEnumerableAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
            {
                Debug.Assert(source != null);
                Debug.Assert(predicate != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new WhereEnumerableAsyncIterator<TSource>(_source, _predicate);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            public override IAsyncEnumerable<TResult> Select<TResult>(Func<TSource, TResult> selector)
            {
                return new WhereSelectEnumerableAsyncIterator<TSource, TResult>(_source, _predicate, selector);
            }

            public override IAsyncEnumerable<TSource> Where(Func<TSource, bool> predicate)
            {
                return new WhereEnumerableAsyncIterator<TSource>(_source, CombinePredicates(_predicate, predicate));
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
                            if (_predicate(item))
                            {
                                _current = item;
                                return true;
                            }
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }

#if !USE_ASYNC_ITERATOR
        private sealed class WhereEnumerableWithIndexAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, int, bool> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;
            private int _index;

            public WhereEnumerableWithIndexAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
            {
                Debug.Assert(source != null);
                Debug.Assert(predicate != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new WhereEnumerableWithIndexAsyncIterator<TSource>(_source, _predicate);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _index = -1;
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;

                            checked
                            {
                                _index++;
                            }

                            if (_predicate(item, _index))
                            {
                                _current = item;
                                return true;
                            }
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }
#endif

        internal sealed class WhereEnumerableAsyncIteratorWithTask<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, ValueTask<bool>> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;
            private IAsyncEnumerator<TSource> _enumerator;

            public WhereEnumerableAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate)
            {
                Debug.Assert(source != null);
                Debug.Assert(predicate != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new WhereEnumerableAsyncIteratorWithTask<TSource>(_source, _predicate);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            public override IAsyncEnumerable<TSource> Where(Func<TSource, ValueTask<bool>> predicate)
            {
                return new WhereEnumerableAsyncIteratorWithTask<TSource>(_source, CombinePredicates(_predicate, predicate));
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
                            if (await _predicate(item).ConfigureAwait(false))
                            {
                                _current = item;
                                return true;
                            }
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }

#if !NO_DEEP_CANCELLATION
        internal sealed class WhereEnumerableAsyncIteratorWithTaskAndCancellation<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, CancellationToken, ValueTask<bool>> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;
            private IAsyncEnumerator<TSource> _enumerator;

            public WhereEnumerableAsyncIteratorWithTaskAndCancellation(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate)
            {
                Debug.Assert(source != null);
                Debug.Assert(predicate != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new WhereEnumerableAsyncIteratorWithTaskAndCancellation<TSource>(_source, _predicate);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            public override IAsyncEnumerable<TSource> Where(Func<TSource, CancellationToken, ValueTask<bool>> predicate)
            {
                return new WhereEnumerableAsyncIteratorWithTaskAndCancellation<TSource>(_source, CombinePredicates(_predicate, predicate));
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
                            if (await _predicate(item, _cancellationToken).ConfigureAwait(false))
                            {
                                _current = item;
                                return true;
                            }
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }
#endif

#if !USE_ASYNC_ITERATOR
        private sealed class WhereEnumerableWithIndexAsyncIteratorWithTask<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, int, ValueTask<bool>> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;
            private int _index;

            public WhereEnumerableWithIndexAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate)
            {
                Debug.Assert(source != null);
                Debug.Assert(predicate != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new WhereEnumerableWithIndexAsyncIteratorWithTask<TSource>(_source, _predicate);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _index = -1;
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;

                            checked
                            {
                                _index++;
                            }

                            if (await _predicate(item, _index).ConfigureAwait(false))
                            {
                                _current = item;
                                return true;
                            }
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }

#if !NO_DEEP_CANCELLATION
        private sealed class WhereEnumerableWithIndexAsyncIteratorWithTaskAndCancellation<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, int, CancellationToken, ValueTask<bool>> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;
            private int _index;

            public WhereEnumerableWithIndexAsyncIteratorWithTaskAndCancellation(IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate)
            {
                Debug.Assert(source != null);
                Debug.Assert(predicate != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new WhereEnumerableWithIndexAsyncIteratorWithTaskAndCancellation<TSource>(_source, _predicate);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _index = -1;
                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;

                            checked
                            {
                                _index++;
                            }

                            if (await _predicate(item, _index, _cancellationToken).ConfigureAwait(false))
                            {
                                _current = item;
                                return true;
                            }
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }
#endif
#endif

        private sealed class WhereSelectEnumerableAsyncIterator<TSource, TResult> : AsyncIterator<TResult>
        {
            private readonly Func<TSource, bool> _predicate;
            private readonly Func<TSource, TResult> _selector;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;

            public WhereSelectEnumerableAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, Func<TSource, TResult> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(predicate != null);
                Debug.Assert(selector != null);

                _source = source;
                _predicate = predicate;
                _selector = selector;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new WhereSelectEnumerableAsyncIterator<TSource, TResult>(_source, _predicate, _selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            public override IAsyncEnumerable<TResult1> Select<TResult1>(Func<TResult, TResult1> selector)
            {
                return new WhereSelectEnumerableAsyncIterator<TSource, TResult1>(_source, _predicate, CombineSelectors(_selector, selector));
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
                            if (_predicate(item))
                            {
                                _current = _selector(item);
                                return true;
                            }
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }
    }
}
