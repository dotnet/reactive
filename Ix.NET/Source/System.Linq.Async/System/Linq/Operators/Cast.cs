// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        // NB: This is a non-standard LINQ operator, because we don't have a non-generic IAsyncEnumerable.
        //     We're keeping it to enable `from T x in xs` binding in C#.

        public static IAsyncEnumerable<TResult> Cast<TResult>(this IAsyncEnumerable<object> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            if (source is IAsyncEnumerable<TResult> typedSource)
            {
                return typedSource;
            }

#if USE_ASYNC_ITERATOR
            return Create(Core);

            async IAsyncEnumerator<TResult> Core(CancellationToken cancellationToken)
            {
                await foreach (var obj in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return (TResult)obj;
                }
            }
#else
            return new CastAsyncIterator<TResult>(source);
#endif
        }

#if !USE_ASYNC_ITERATOR
        private sealed class CastAsyncIterator<TResult> : AsyncIterator<TResult>
        {
            private readonly IAsyncEnumerable<object> _source;
            private IAsyncEnumerator<object> _enumerator;

            public CastAsyncIterator(IAsyncEnumerable<object> source)
            {
                _source = source;
            }

            public override AsyncIteratorBase<TResult> Clone()
            {
                return new CastAsyncIterator<TResult>(_source);
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
                            _current = (TResult)_enumerator.Current;
                            return true;
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
