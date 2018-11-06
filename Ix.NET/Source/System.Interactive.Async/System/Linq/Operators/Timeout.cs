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
                throw new ArgumentNullException(nameof(source));

            var num = (long)timeout.TotalMilliseconds;
            if (num < -1L || num > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(timeout));

            return new TimeoutAsyncIterator<TSource>(source, timeout);
        }

        private sealed class TimeoutAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IAsyncEnumerable<TSource> source;
            private readonly TimeSpan timeout;

            private IAsyncEnumerator<TSource> enumerator;

            public TimeoutAsyncIterator(IAsyncEnumerable<TSource> source, TimeSpan timeout)
            {
                Debug.Assert(source != null);

                this.source = source;
                this.timeout = timeout;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new TimeoutAsyncIterator<TSource>(source, timeout);
            }

            public override async ValueTask DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetAsyncEnumerator(cancellationToken);

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        var moveNext = enumerator.MoveNextAsync();

                        if (!moveNext.IsCompleted)
                        {
                            using (var delayCts = new CancellationTokenSource())
                            {
                                var delay = Task.Delay(timeout, delayCts.Token);

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
                            current = enumerator.Current;
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
