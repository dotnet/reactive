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
        public static IAsyncEnumerable<IList<TSource>> Buffer<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (count <= 0)
                throw Error.ArgumentOutOfRange(nameof(count));

#if USE_ASYNC_ITERATOR
            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<IList<TSource>> Core(CancellationToken cancellationToken)
            {
                var buffer = new List<TSource>(count);

                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    buffer.Add(item);

                    if (buffer.Count == count)
                    {
                        yield return buffer;

                        buffer = new List<TSource>(count);
                    }
                }

                if (buffer.Count > 0)
                {
                    yield return buffer;
                }
            }
#else
            return new BufferAsyncIterator<TSource>(source, count, count);
#endif
        }

        public static IAsyncEnumerable<IList<TSource>> Buffer<TSource>(this IAsyncEnumerable<TSource> source, int count, int skip)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (count <= 0)
                throw Error.ArgumentOutOfRange(nameof(count));
            if (skip <= 0)
                throw Error.ArgumentOutOfRange(nameof(skip));

#if USE_ASYNC_ITERATOR
            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<IList<TSource>> Core(CancellationToken cancellationToken)
            {
                var buffers = new Queue<IList<TSource>>();

                var index = 0;

                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (index++ % skip == 0)
                    {
                        buffers.Enqueue(new List<TSource>(count));
                    }

                    foreach (var buffer in buffers)
                    {
                        buffer.Add(item);
                    }

                    if (buffers.Count > 0 && buffers.Peek().Count == count)
                    {
                        yield return buffers.Dequeue();
                    }
                }

                while (buffers.Count > 0)
                {
                    yield return buffers.Dequeue();
                }
            }
#else
            return new BufferAsyncIterator<TSource>(source, count, skip);
#endif
        }

#if !USE_ASYNC_ITERATOR
        private sealed class BufferAsyncIterator<TSource> : AsyncIterator<IList<TSource>>
        {
            private readonly int _count;
            private readonly int _skip;
            private readonly IAsyncEnumerable<TSource> _source;

            private Queue<IList<TSource>> _buffers;
            private IAsyncEnumerator<TSource> _enumerator;
            private int _index;
            private bool _stopped;

            public BufferAsyncIterator(IAsyncEnumerable<TSource> source, int count, int skip)
            {
                Debug.Assert(source != null);

                _source = source;
                _count = count;
                _skip = skip;
            }

            public override AsyncIteratorBase<IList<TSource>> Clone()
            {
                return new BufferAsyncIterator<TSource>(_source, _count, _skip);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }

                _buffers = null;

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _buffers = new Queue<IList<TSource>>();
                        _index = 0;
                        _stopped = false;

                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (true)
                        {
                            if (!_stopped)
                            {
                                if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    var item = _enumerator.Current;

                                    if (_index++ % _skip == 0)
                                    {
                                        _buffers.Enqueue(new List<TSource>(_count));
                                    }

                                    foreach (var buffer in _buffers)
                                    {
                                        buffer.Add(item);
                                    }

                                    if (_buffers.Count > 0 && _buffers.Peek().Count == _count)
                                    {
                                        _current = _buffers.Dequeue();
                                        return true;
                                    }

                                    continue; // loop
                                }

                                _stopped = true;
                                await _enumerator.DisposeAsync().ConfigureAwait(false);
                                _enumerator = null;

                                continue; // loop
                            }

                            if (_buffers.Count > 0)
                            {
                                _current = _buffers.Dequeue();
                                return true;
                            }

                            break; // exit the while
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
