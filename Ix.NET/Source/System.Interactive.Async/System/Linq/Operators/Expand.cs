// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        public static IAsyncEnumerable<TSource> Expand<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TSource>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return new ExpandAsyncIterator<TSource>(source, selector);
        }

        public static IAsyncEnumerable<TSource> Expand<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<IAsyncEnumerable<TSource>>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return new ExpandAsyncIteratorWithTask<TSource>(source, selector);
        }

        private sealed class ExpandAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, IAsyncEnumerable<TSource>> _selector;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;

            private Queue<IAsyncEnumerable<TSource>> _queue;

            public ExpandAsyncIterator(IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TSource>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                _source = source;
                _selector = selector;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new ExpandAsyncIterator<TSource>(_source, _selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                _queue = null;

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _queue = new Queue<IAsyncEnumerable<TSource>>();
                        _queue.Enqueue(_source);

                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (true)
                        {
                            if (_enumerator == null)
                            {
                                if (_queue.Count > 0)
                                {
                                    var src = _queue.Dequeue();

                                    if (_enumerator != null)
                                    {
                                        await _enumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    _enumerator = src.GetAsyncEnumerator(_cancellationToken);

                                    continue; // loop
                                }

                                break; // while
                            }

                            if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                            {
                                var item = _enumerator.Current;
                                var next = _selector(item);
                                _queue.Enqueue(next);
                                _current = item;
                                return true;
                            }

                            await _enumerator.DisposeAsync().ConfigureAwait(false);
                            _enumerator = null;
                        }

                        break; // case
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class ExpandAsyncIteratorWithTask<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, Task<IAsyncEnumerable<TSource>>> _selector;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;

            private Queue<IAsyncEnumerable<TSource>> _queue;

            public ExpandAsyncIteratorWithTask(IAsyncEnumerable<TSource> source, Func<TSource, Task<IAsyncEnumerable<TSource>>> selector)
            {
                Debug.Assert(source != null);
                Debug.Assert(selector != null);

                _source = source;
                _selector = selector;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new ExpandAsyncIteratorWithTask<TSource>(_source, _selector);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                _queue = null;

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _queue = new Queue<IAsyncEnumerable<TSource>>();
                        _queue.Enqueue(_source);

                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (true)
                        {
                            if (_enumerator == null)
                            {
                                if (_queue.Count > 0)
                                {
                                    var src = _queue.Dequeue();

                                    if (_enumerator != null)
                                    {
                                        await _enumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    _enumerator = src.GetAsyncEnumerator(_cancellationToken);

                                    continue; // loop
                                }

                                break; // while
                            }

                            if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                            {
                                var item = _enumerator.Current;
                                var next = await _selector(item).ConfigureAwait(false);
                                _queue.Enqueue(next);
                                _current = item;
                                return true;
                            }

                            await _enumerator.DisposeAsync().ConfigureAwait(false);
                            _enumerator = null;
                        }

                        break; // case
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
    }
}
