// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> TakeLast<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            if (count <= 0)
            {
                return Empty<TSource>();
            }

#if CSHARP8 && USE_ASYNC_ITERATOR && ASYNC_ITERATOR_CAN_RETURN_AETOR // https://github.com/dotnet/roslyn/pull/31114
            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                Queue<TSource> queue;

                await using (var e = source.GetAsyncEnumerator(cancellationToken).ConfigureAwait(false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        yield break;
                    }

                    queue = new Queue<TSource>();
                    queue.Enqueue(e.Current);

                    while (await e.MoveNextAsync())
                    {
                        if (queue.Count < count)
                        {
                            queue.Enqueue(e.Current);
                        }
                        else
                        {
                            do
                            {
                                queue.Dequeue();
                                queue.Enqueue(e.Current);
                            }
                            while (await e.MoveNextAsync());
                            break;
                        }
                    }
                }

                Debug.Assert(queue.Count <= count);
                do
                {
                    yield return queue.Dequeue();
                }
                while (queue.Count > 0);
            }
#else
            return new TakeLastAsyncIterator<TSource>(source, count);
#endif
        }

#if !(CSHARP8 && USE_ASYNC_ITERATOR && ASYNC_ITERATOR_CAN_RETURN_AETOR)
        private sealed class TakeLastAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly int _count;
            private readonly IAsyncEnumerable<TSource> _source;

            private IAsyncEnumerator<TSource> _enumerator;
            private bool _isDone;
            private Queue<TSource> _queue;

            public TakeLastAsyncIterator(IAsyncEnumerable<TSource> source, int count)
            {
                Debug.Assert(source != null);

                _source = source;
                _count = count;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new TakeLastAsyncIterator<TSource>(_source, _count);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                _queue = null; // release the memory

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _queue = new Queue<TSource>();
                        _isDone = false;

                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;


                    case AsyncIteratorState.Iterating:
                        while (true)
                        {
                            if (!_isDone)
                            {
                                if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    if (_count > 0)
                                    {
                                        var item = _enumerator.Current;
                                        if (_queue.Count >= _count)
                                        {
                                            _queue.Dequeue();
                                        }
                                        _queue.Enqueue(item);
                                    }
                                }
                                else
                                {
                                    _isDone = true;
                                    // Dispose early here as we can
                                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                                    _enumerator = null;
                                }

                                continue; // loop until queue is drained
                            }

                            if (_queue.Count > 0)
                            {
                                _current = _queue.Dequeue();
                                return true;
                            }

                            break; // while
                        }

                        break; // case
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
#endif
    }
}
