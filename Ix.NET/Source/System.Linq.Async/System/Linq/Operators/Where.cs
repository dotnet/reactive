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

            if (source is AsyncIterator<TSource> iterator)
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

            return new WhereEnumerableWithIndexAsyncIterator<TSource>(source, predicate);
        }

        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            if (source is AsyncIterator<TSource> iterator)
            {
                return iterator.Where(predicate);
            }

            // TODO: Can we add array/list optimizations here, does it make sense?
            return new WhereEnumerableAsyncIteratorWithTask<TSource>(source, predicate);
        }

        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, Task<bool>> predicate)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return new WhereEnumerableWithIndexAsyncIteratorWithTask<TSource>(source, predicate);
        }

        private static Func<TSource, bool> CombinePredicates<TSource>(Func<TSource, bool> predicate1, Func<TSource, bool> predicate2)
        {
            return x => predicate1(x) && predicate2(x);
        }

        private static Func<TSource, Task<bool>> CombinePredicates<TSource>(Func<TSource, Task<bool>> predicate1, Func<TSource, Task<bool>> predicate2)
        {
            return async x => await predicate1(x).ConfigureAwait(false) && await predicate2(x).ConfigureAwait(false);
        }

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

            public override AsyncIterator<TSource> Clone()
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

        internal sealed class WhereEnumerableWithIndexAsyncIterator<TSource> : AsyncIterator<TSource>
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

            public override AsyncIterator<TSource> Clone()
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

        internal sealed class WhereEnumerableAsyncIteratorWithTask<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, Task<bool>> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;
            private IAsyncEnumerator<TSource> _enumerator;

            public WhereEnumerableAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate)
            {
                Debug.Assert(source != null);
                Debug.Assert(predicate != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIterator<TSource> Clone()
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

            public override IAsyncEnumerable<TSource> Where(Func<TSource, Task<bool>> predicate)
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

        internal sealed class WhereEnumerableWithIndexAsyncIteratorWithTask<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, int, Task<bool>> _predicate;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;
            private int _index;

            public WhereEnumerableWithIndexAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, int, Task<bool>> predicate)
            {
                Debug.Assert(source != null);
                Debug.Assert(predicate != null);

                _source = source;
                _predicate = predicate;
            }

            public override AsyncIterator<TSource> Clone()
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

        internal sealed class WhereSelectEnumerableAsyncIterator<TSource, TResult> : AsyncIterator<TResult>
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

            public override AsyncIterator<TResult> Clone()
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
