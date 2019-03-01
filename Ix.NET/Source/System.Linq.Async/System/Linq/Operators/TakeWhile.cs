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
        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

#if USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await foreach (var element in AsyncEnumerableExtensions.WithCancellation(source, cancellationToken).ConfigureAwait(false))
                {
                    if (!predicate(element))
                    {
                        break;
                    }

                    yield return element;
                }
            }
#else
            return new TakeWhileAsyncIterator<TSource>(source, predicate);
#endif
        }

        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
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

                    if (!predicate(element, index))
                    {
                        break;
                    }

                    yield return element;
                }
            }
#else
            return new TakeWhileWithIndexAsyncIterator<TSource>(source, predicate);
#endif
        }

        internal static IAsyncEnumerable<TSource> TakeWhileAwaitCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

#if USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await foreach (var element in AsyncEnumerableExtensions.WithCancellation(source, cancellationToken).ConfigureAwait(false))
                {
                    if (!await predicate(element).ConfigureAwait(false))
                    {
                        break;
                    }

                    yield return element;
                }
            }
#else
            return new TakeWhileAsyncIteratorWithTask<TSource>(source, predicate);
#endif
        }

#if !NO_DEEP_CANCELLATION
        internal static IAsyncEnumerable<TSource> TakeWhileAwaitWithCancellationCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

#if USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await foreach (var element in AsyncEnumerableExtensions.WithCancellation(source, cancellationToken).ConfigureAwait(false))
                {
                    if (!await predicate(element, cancellationToken).ConfigureAwait(false))
                    {
                        break;
                    }

                    yield return element;
                }
            }
#else
            return new TakeWhileAsyncIteratorWithTaskAndCancellation<TSource>(source, predicate);
#endif
        }
#endif

        internal static IAsyncEnumerable<TSource> TakeWhileAwaitCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate)
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

                    if (!await predicate(element, index).ConfigureAwait(false))
                    {
                        break;
                    }

                    yield return element;
                }
            }
#else
            return new TakeWhileWithIndexAsyncIteratorWithTask<TSource>(source, predicate);
#endif
        }

#if !NO_DEEP_CANCELLATION
        internal static IAsyncEnumerable<TSource> TakeWhileAwaitWithCancellationCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate)
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

                    if (!await predicate(element, index, cancellationToken).ConfigureAwait(false))
                    {
                        break;
                    }

                    yield return element;
                }
            }
#else
            return new TakeWhileWithIndexAsyncIteratorWithTaskAndCancellation<TSource>(source, predicate);
#endif
        }
#endif

#if !USE_ASYNC_ITERATOR
        private sealed class TakeWhileAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, bool> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;

            public TakeWhileAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new TakeWhileAsyncIterator<TSource>(_source, _predicate);
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

                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;
                            if (!_predicate(item))
                            {
                                break;
                            }

                            _current = item;
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class TakeWhileWithIndexAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, int, bool> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;
            private int _index;

            public TakeWhileWithIndexAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new TakeWhileWithIndexAsyncIterator<TSource>(_source, _predicate);
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
                        if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;

                            checked
                            {
                                _index++;
                            }

                            if (!_predicate(item, _index))
                            {
                                break;
                            }

                            _current = item;
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class TakeWhileAsyncIteratorWithTask<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, ValueTask<bool>> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;

            public TakeWhileAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new TakeWhileAsyncIteratorWithTask<TSource>(_source, _predicate);
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

                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;
                            if (!await _predicate(item).ConfigureAwait(false))
                            {
                                break;
                            }

                            _current = item;
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

#if !NO_DEEP_CANCELLATION
        private sealed class TakeWhileAsyncIteratorWithTaskAndCancellation<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, CancellationToken, ValueTask<bool>> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;

            public TakeWhileAsyncIteratorWithTaskAndCancellation(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new TakeWhileAsyncIteratorWithTaskAndCancellation<TSource>(_source, _predicate);
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

                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;


                    case AsyncIteratorState.Iterating:
                        if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;
                            if (!await _predicate(item, _cancellationToken).ConfigureAwait(false))
                            {
                                break;
                            }

                            _current = item;
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
#endif

        private sealed class TakeWhileWithIndexAsyncIteratorWithTask<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, int, ValueTask<bool>> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;
            private int _index;

            public TakeWhileWithIndexAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new TakeWhileWithIndexAsyncIteratorWithTask<TSource>(_source, _predicate);
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
                        if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;

                            checked
                            {
                                _index++;
                            }

                            if (!await _predicate(item, _index).ConfigureAwait(false))
                            {
                                break;
                            }

                            _current = item;
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

#if !NO_DEEP_CANCELLATION
        private sealed class TakeWhileWithIndexAsyncIteratorWithTaskAndCancellation<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, int, CancellationToken, ValueTask<bool>> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;
            private int _index;

            public TakeWhileWithIndexAsyncIteratorWithTaskAndCancellation(IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new TakeWhileWithIndexAsyncIteratorWithTaskAndCancellation<TSource>(_source, _predicate);
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
                        if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = _enumerator.Current;

                            checked
                            {
                                _index++;
                            }

                            if (!await _predicate(item, _index, _cancellationToken).ConfigureAwait(false))
                            {
                                break;
                            }

                            _current = item;
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
#endif
#endif
    }
}
