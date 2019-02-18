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
        public static IAsyncEnumerable<TResult> Repeat<TResult>(TResult element)
        {
#if USE_ASYNC_ITERATOR
            return AsyncEnumerable.Create(Core);

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            async IAsyncEnumerator<TResult> Core(CancellationToken cancellationToken)
            {
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    yield return element;
                }
            }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
#else
            return new RepeatElementAsyncIterator<TResult>(element);
#endif
        }

        public static IAsyncEnumerable<TSource> Repeat<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

#if USE_ASYNC_ITERATOR
            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                while (true)
                {
                    await foreach (var item in AsyncEnumerableExtensions.WithCancellation(source, cancellationToken).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
            }
#else
            return new RepeatSequenceAsyncIterator<TSource>(source, -1);
#endif
        }

        public static IAsyncEnumerable<TSource> Repeat<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (count < 0)
                throw Error.ArgumentOutOfRange(nameof(count));

#if USE_ASYNC_ITERATOR
            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                for (var i = 0; i < count; i++)
                {
                    await foreach (var item in AsyncEnumerableExtensions.WithCancellation(source, cancellationToken).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
            }
#else
            return new RepeatSequenceAsyncIterator<TSource>(source, count);
#endif
        }

#if !USE_ASYNC_ITERATOR
        private sealed class RepeatElementAsyncIterator<TResult> : AsyncIterator<TResult>
        {
            private readonly TResult _element;

            public RepeatElementAsyncIterator(TResult element) => _element = element;

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new RepeatElementAsyncIterator<TResult>(_element);
            }

            protected override ValueTask<bool> MoveNextCore()
            {
                _cancellationToken.ThrowIfCancellationRequested();

                _current = _element;
                return new ValueTask<bool>(true);
            }
        }

        private sealed class RepeatSequenceAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly int _count;
            private readonly bool _isInfinite;
            private readonly IAsyncEnumerable<TSource> _source;

            private int _currentCount;
            private IAsyncEnumerator<TSource> _enumerator;

            public RepeatSequenceAsyncIterator(IAsyncEnumerable<TSource> source, int count)
            {
                Debug.Assert(source != null);

                _source = source;
                _count = count;
                _isInfinite = count < 0;
                _currentCount = count;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new RepeatSequenceAsyncIterator<TSource>(_source, _count);
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

                        if (_enumerator != null)
                        {
                            await _enumerator.DisposeAsync().ConfigureAwait(false);
                            _enumerator = null;
                        }

                        if (!_isInfinite && _currentCount-- == 0)
                            break;

                        _enumerator = _source.GetAsyncEnumerator(_cancellationToken);
                        _state = AsyncIteratorState.Iterating;

                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await _enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _current = _enumerator.Current;
                            return true;
                        }

                        goto case AsyncIteratorState.Allocated;
                }

                await DisposeAsync().ConfigureAwait(false);

                return false;
            }
        }
#endif
    }
}
