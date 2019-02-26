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
        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

#if USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var element = e.Current;

                        if (!predicate(element))
                        {
                            yield return element;

                            while (await e.MoveNextAsync())
                            {
                                yield return e.Current;
                            }

                            yield break;
                        }
                    }
                }
            }
#else
            return new SkipWhileAsyncIterator<TSource>(source, predicate);
#endif
        }

        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

#if USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    var index = -1;

                    while (await e.MoveNextAsync())
                    {
                        checked
                        {
                            index++;
                        }

                        var element = e.Current;

                        if (!predicate(element, index))
                        {
                            yield return element;

                            while (await e.MoveNextAsync())
                            {
                                yield return e.Current;
                            }

                            yield break;
                        }
                    }
                }
            }
#else
            return new SkipWhileWithIndexAsyncIterator<TSource>(source, predicate);
#endif
        }

        public static IAsyncEnumerable<TSource> SkipWhileAwait<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

#if USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var element = e.Current;

                        if (!await predicate(element).ConfigureAwait(false))
                        {
                            yield return element;

                            while (await e.MoveNextAsync())
                            {
                                yield return e.Current;
                            }

                            yield break;
                        }
                    }
                }
            }
#else
            return new SkipWhileAsyncIteratorWithTask<TSource>(source, predicate);
#endif
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TSource> SkipWhileAwaitWithCancellation<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

#if USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var element = e.Current;

                        if (!await predicate(element, cancellationToken).ConfigureAwait(false))
                        {
                            yield return element;

                            while (await e.MoveNextAsync())
                            {
                                yield return e.Current;
                            }

                            yield break;
                        }
                    }
                }
            }
#else
            return new SkipWhileAsyncIteratorWithTaskAndCancellation<TSource>(source, predicate);
#endif
        }
#endif

        public static IAsyncEnumerable<TSource> SkipWhileAwait<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

#if USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    var index = -1;

                    while (await e.MoveNextAsync())
                    {
                        checked
                        {
                            index++;
                        }

                        var element = e.Current;

                        if (!await predicate(element, index).ConfigureAwait(false))
                        {
                            yield return element;

                            while (await e.MoveNextAsync())
                            {
                                yield return e.Current;
                            }

                            yield break;
                        }
                    }
                }
            }
#else
            return new SkipWhileWithIndexAsyncIteratorWithTask<TSource>(source, predicate);
#endif
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TSource> SkipWhileAwaitWithCancellation<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

#if USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    var index = -1;

                    while (await e.MoveNextAsync())
                    {
                        checked
                        {
                            index++;
                        }

                        var element = e.Current;

                        if (!await predicate(element, index, cancellationToken).ConfigureAwait(false))
                        {
                            yield return element;

                            while (await e.MoveNextAsync())
                            {
                                yield return e.Current;
                            }

                            yield break;
                        }
                    }
                }
            }
#else
            return new SkipWhileWithIndexAsyncIteratorWithTaskAndCancellation<TSource>(source, predicate);
#endif
        }
#endif

#if !USE_ASYNC_ITERATOR
        private sealed class SkipWhileAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, bool> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;

            private bool _doMoveNext;
            private IAsyncEnumerator<TSource> _enumerator;

            public SkipWhileAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new SkipWhileAsyncIterator<TSource>(_source, _predicate);
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

                        // skip elements as requested
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var element = _enumerator.Current;
                            if (!_predicate(element))
                            {
                                _doMoveNext = false;
                                _state = AsyncIteratorState.Iterating;
                                goto case AsyncIteratorState.Iterating;
                            }
                        }

                        break;

                    case AsyncIteratorState.Iterating:
                        if (_doMoveNext && await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = _enumerator.Current;
                            return true;
                        }

                        if (!_doMoveNext)
                        {
                            _current = _enumerator.Current;
                            _doMoveNext = true;
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class SkipWhileWithIndexAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, int, bool> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;

            private bool _doMoveNext;
            private IAsyncEnumerator<TSource> _enumerator;
            private int _index;

            public SkipWhileWithIndexAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new SkipWhileWithIndexAsyncIterator<TSource>(_source, _predicate);
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

                        // skip elements as requested
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var element = _enumerator.Current;

                            checked
                            {
                                _index++;
                            }

                            if (!_predicate(element, _index))
                            {
                                _doMoveNext = false;
                                _state = AsyncIteratorState.Iterating;
                                goto case AsyncIteratorState.Iterating;
                            }
                        }

                        break;

                    case AsyncIteratorState.Iterating:
                        if (_doMoveNext && await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = _enumerator.Current;
                            return true;
                        }

                        if (!_doMoveNext)
                        {
                            _current = _enumerator.Current;
                            _doMoveNext = true;
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class SkipWhileAsyncIteratorWithTask<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, ValueTask<bool>> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;

            private bool _doMoveNext;
            private IAsyncEnumerator<TSource> _enumerator;

            public SkipWhileAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new SkipWhileAsyncIteratorWithTask<TSource>(_source, _predicate);
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

                        // skip elements as requested
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var element = _enumerator.Current;
                            if (!await _predicate(element).ConfigureAwait(false))
                            {
                                _doMoveNext = false;
                                _state = AsyncIteratorState.Iterating;
                                goto case AsyncIteratorState.Iterating;
                            }
                        }

                        break;

                    case AsyncIteratorState.Iterating:
                        if (_doMoveNext && await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = _enumerator.Current;
                            return true;
                        }

                        if (!_doMoveNext)
                        {
                            _current = _enumerator.Current;
                            _doMoveNext = true;
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

#if !NO_DEEP_CANCELLATION
        private sealed class SkipWhileAsyncIteratorWithTaskAndCancellation<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, CancellationToken, ValueTask<bool>> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;

            private bool _doMoveNext;
            private IAsyncEnumerator<TSource> _enumerator;

            public SkipWhileAsyncIteratorWithTaskAndCancellation(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new SkipWhileAsyncIteratorWithTaskAndCancellation<TSource>(_source, _predicate);
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

                        // skip elements as requested
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var element = _enumerator.Current;
                            if (!await _predicate(element, _cancellationToken).ConfigureAwait(false))
                            {
                                _doMoveNext = false;
                                _state = AsyncIteratorState.Iterating;
                                goto case AsyncIteratorState.Iterating;
                            }
                        }

                        break;

                    case AsyncIteratorState.Iterating:
                        if (_doMoveNext && await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = _enumerator.Current;
                            return true;
                        }

                        if (!_doMoveNext)
                        {
                            _current = _enumerator.Current;
                            _doMoveNext = true;
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
#endif

        private sealed class SkipWhileWithIndexAsyncIteratorWithTask<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, int, ValueTask<bool>> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;

            private bool _doMoveNext;
            private IAsyncEnumerator<TSource> _enumerator;
            private int _index;

            public SkipWhileWithIndexAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new SkipWhileWithIndexAsyncIteratorWithTask<TSource>(_source, _predicate);
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

                        // skip elements as requested
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var element = _enumerator.Current;

                            checked
                            {
                                _index++;
                            }

                            if (!await _predicate(element, _index).ConfigureAwait(false))
                            {
                                _doMoveNext = false;
                                _state = AsyncIteratorState.Iterating;
                                goto case AsyncIteratorState.Iterating;
                            }
                        }

                        break;

                    case AsyncIteratorState.Iterating:
                        if (_doMoveNext && await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = _enumerator.Current;
                            return true;
                        }

                        if (!_doMoveNext)
                        {
                            _current = _enumerator.Current;
                            _doMoveNext = true;
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

#if !NO_DEEP_CANCELLATION
        private sealed class SkipWhileWithIndexAsyncIteratorWithTaskAndCancellation<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, int, CancellationToken, ValueTask<bool>> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;

            private bool _doMoveNext;
            private IAsyncEnumerator<TSource> _enumerator;
            private int _index;

            public SkipWhileWithIndexAsyncIteratorWithTaskAndCancellation(IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate)
            {
                Debug.Assert(predicate != null);
                Debug.Assert(source != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new SkipWhileWithIndexAsyncIteratorWithTaskAndCancellation<TSource>(_source, _predicate);
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

                        // skip elements as requested
                        while (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var element = _enumerator.Current;

                            checked
                            {
                                _index++;
                            }

                            if (!await _predicate(element, _index, _cancellationToken).ConfigureAwait(false))
                            {
                                _doMoveNext = false;
                                _state = AsyncIteratorState.Iterating;
                                goto case AsyncIteratorState.Iterating;
                            }
                        }

                        break;

                    case AsyncIteratorState.Iterating:
                        if (_doMoveNext && await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = _enumerator.Current;
                            return true;
                        }

                        if (!_doMoveNext)
                        {
                            _current = _enumerator.Current;
                            _doMoveNext = true;
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
