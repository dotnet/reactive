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

            return new SkipWhileAsyncIterator<TSource>(source, predicate);
        }

        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return new SkipWhileWithIndexAsyncIterator<TSource>(source, predicate);
        }

        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return new SkipWhileAsyncIteratorWithTask<TSource>(source, predicate);
        }

        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return new SkipWhileWithIndexAsyncIteratorWithTask<TSource>(source, predicate);
        }

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
    }
}
