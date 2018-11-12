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
        public static IAsyncEnumerable<TSource> Timeout<TSource>(this IAsyncEnumerable<TSource> source, TimeSpan timeout)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            var num = (long)timeout.TotalMilliseconds;
            if (num < -1L || num > int.MaxValue)
                throw Error.ArgumentOutOfRange(nameof(timeout));

            return new TimeoutAsyncIterator<TSource>(source, timeout);
        }

        private sealed class TimeoutAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IAsyncEnumerable<TSource> _source;
            private readonly TimeSpan _timeout;

            private IAsyncEnumerator<TSource> _enumerator;

            public TimeoutAsyncIterator(IAsyncEnumerable<TSource> source, TimeSpan timeout)
            {
                Debug.Assert(source != null);

                _source = source;
                _timeout = timeout;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new TimeoutAsyncIterator<TSource>(_source, _timeout);
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
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _enumerator = _source.GetAsyncEnumerator(cancellationToken);

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        var moveNext = _enumerator.MoveNextAsync();

                        if (!moveNext.IsCompleted)
                        {
                            using (var delayCts = new CancellationTokenSource())
                            {
                                var delay = Task.Delay(_timeout, delayCts.Token);

                                var winner = await Task.WhenAny(moveNext.AsTask(), delay).ConfigureAwait(false);

                                if (winner == delay)
                                {
                                    throw new TimeoutException();
                                }

                                delayCts.Cancel();
                            }
                        }

                        if (await moveNext.ConfigureAwait(false))
                        {
                            current = _enumerator.Current;
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
