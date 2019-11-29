// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        /// <summary>
        /// Applies a timeout policy for each element in the async-enumerable sequence.
        /// If the next element isn't received within the specified timeout duration starting from its predecessor, a TimeoutException is propagated to the observer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to perform a timeout for.</param>
        /// <param name="timeout">Maximum duration between values before a timeout occurs.</param>
        /// <returns>The source sequence with a TimeoutException in case of a timeout.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeout"/> is less than TimeSpan.Zero.</exception>
        /// <exception cref="TimeoutException">(Asynchronous) If no element is produced within <paramref name="timeout"/> from the previous element.</exception>
        /// <remarks>
        /// <para>
        /// In case you only want to timeout on the first element, consider using the <see cref="Amb{TSource}(IAsyncEnumerable{TSource}, IAsyncEnumerable{TSource})"/>
        /// operator applied to the source sequence and a delayed <see cref="Throw{TResult}(Exception)"/> sequence.
        /// <!-- FIXME: Timeout with initial and per item timeout option not implemented yet.
        /// Alternatively, the general-purpose overload
        /// of Timeout, <see cref="Timeout{TSource, TTimeout}(IObservable{TSource}, IObservable{TTimeout}, Func{TSource, IObservable{TTimeout}})"/> can be used.
        /// -->
        /// </para>
        /// <para>
        /// Specifying a TimeSpan.Zero value for <paramref name="timeout"/> is not recommended but supported, causing timeout timers to be scheduled that are due
        /// immediately. However, this doesn't guarantee a timeout will occur, even for the first element. This is a side-effect of the asynchrony introduced by the
        /// scheduler, where the action to propagate a timeout may not execute immediately, despite the TimeSpan.Zero due time. In such cases, the next element may
        /// arrive before the scheduler gets a chance to run the timeout action.
        /// </para>
        /// </remarks>
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

            private IAsyncEnumerator<TSource>? _enumerator;

            private Task? _loserTask;

            private CancellationTokenSource? _sourceCTS;

            public TimeoutAsyncIterator(IAsyncEnumerable<TSource> source, TimeSpan timeout)
            {
                _source = source;
                _timeout = timeout;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new TimeoutAsyncIterator<TSource>(_source, _timeout);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_loserTask != null)
                {
                    await _loserTask.ConfigureAwait(false);
                    _loserTask = null;
                    _enumerator = null;
                }
                else if (_enumerator != null)
                {
                    await _enumerator.DisposeAsync().ConfigureAwait(false);
                    _enumerator = null;
                }
                if (_sourceCTS != null)
                {
                    _sourceCTS.Dispose();
                    _sourceCTS = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _sourceCTS = CancellationTokenSource.CreateLinkedTokenSource(_cancellationToken);
                        _enumerator = _source.GetAsyncEnumerator(_sourceCTS.Token);

                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        var moveNext = _enumerator!.MoveNextAsync();

                        if (!moveNext.IsCompleted)
                        {
                            using var delayCts = CancellationTokenSource.CreateLinkedTokenSource(_cancellationToken);

                            var delay = Task.Delay(_timeout, delayCts.Token);

                            var next = moveNext.AsTask();

                            var winner = await Task.WhenAny(next, delay).ConfigureAwait(false);

                            if (winner == delay)
                            {
                                // NB: We still have to wait for the MoveNextAsync operation to complete before we can
                                //     dispose _enumerator. The resulting task will be used by DisposeAsync. Also note
                                //     that throwing an exception here causes a call to DisposeAsync, where we pick up
                                //     the task prepared below.

                                // NB: Any exception reported by a timed out MoveNextAsync operation won't be reported
                                //     to the caller, but the task's exception is not marked as observed, so unhandled
                                //     exception handlers can still observe the exception.

                                // REVIEW: Should exceptions reported by a timed out MoveNextAsync operation come out
                                //         when attempting to call DisposeAsync?

                                _loserTask = next.ContinueWith((_, state) => ((IAsyncDisposable)state!).DisposeAsync().AsTask(), _enumerator);

                                _sourceCTS!.Cancel();

                                throw new TimeoutException();
                            }

                            delayCts.Cancel();
                        }

                        if (await moveNext.ConfigureAwait(false))
                        {
                            _current = _enumerator.Current;
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
