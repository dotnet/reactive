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
        public static IAsyncEnumerable<TSource> Amb<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw Error.ArgumentNull(nameof(first));
            if (second == null)
                throw Error.ArgumentNull(nameof(second));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                IAsyncEnumerator<TSource>? firstEnumerator = null;
                IAsyncEnumerator<TSource>? secondEnumerator = null;

                Task<bool>? firstMoveNext = null;
                Task<bool>? secondMoveNext = null;

                try
                {
                    //
                    // REVIEW: We start both sequences unconditionally. An alternative implementation could be to just stick
                    //         to the first sequence if we notice it already has a value (or exception) available. This would
                    //         be similar to Task.WhenAny behavior (see CommonCWAnyLogic in dotnet/coreclr). We could consider
                    //         adding a WhenAny combinator that does exactly that. We can even avoid calling AsTask.
                    //

                    firstEnumerator = first.GetAsyncEnumerator(cancellationToken);
                    firstMoveNext = firstEnumerator.MoveNextAsync().AsTask();

                    //
                    // REVIEW: Order of operations has changed here compared to the original, but is now in sync with the N-ary
                    //         overload which performs GetAsyncEnumerator/MoveNextAsync in pairs, rather than phased.
                    //

                    secondEnumerator = second.GetAsyncEnumerator(cancellationToken);
                    secondMoveNext = secondEnumerator.MoveNextAsync().AsTask();
                }
                catch
                {
                    // NB: AwaitMoveNextAsyncAndDispose checks for null for both arguments, reducing the need for many null
                    //     checks over here.

                    var cleanup = new[]
                    {
                        AwaitMoveNextAsyncAndDispose(secondMoveNext, secondEnumerator),
                        AwaitMoveNextAsyncAndDispose(firstMoveNext, firstEnumerator)
                    };

                    await Task.WhenAll(cleanup).ConfigureAwait(false);

                    throw;
                }

                //
                // REVIEW: Consider using the WhenAny combinator defined for Merge in TaskExt, which would avoid the need
                //         to convert to Task<bool> prior to calling Task.WhenAny.
                //

                var moveNextWinner = await Task.WhenAny(firstMoveNext, secondMoveNext).ConfigureAwait(false);

                //
                // REVIEW: An alternative option is to call DisposeAsync on the other and await it, but this has two drawbacks:
                //
                // 1. Concurrent DisposeAsync while a MoveNextAsync is in flight.
                // 2. The winner elected by Amb is blocked to yield results until the loser unblocks.
                //

                IAsyncEnumerator<TSource> winner;
                Task disposeLoser;

                if (moveNextWinner == firstMoveNext)
                {
                    winner = firstEnumerator;
                    disposeLoser = AwaitMoveNextAsyncAndDispose(secondMoveNext, secondEnumerator);
                }
                else
                {
                    winner = secondEnumerator;
                    disposeLoser = AwaitMoveNextAsyncAndDispose(firstMoveNext, firstEnumerator);
                }

                try
                {                    
                    await using (winner.ConfigureAwait(false))
                    {
                        if (!await moveNextWinner.ConfigureAwait(false))
                        {
                            yield break;
                        }

                        yield return winner.Current;

                        while (await winner.MoveNextAsync().ConfigureAwait(false))
                        {
                            yield return winner.Current;
                        }
                    }
                }
                finally
                {
                    //
                    // REVIEW: This behavior differs from the original implementation in that we never discard any in flight
                    //         asynchronous operations. If an exception occurs while enumerating the winner, it can be
                    //         subsumed by an exception thrown due to cleanup of the loser. Also, if Amb is used to deal with
                    //         a potentially long-blocking sequence, this implementation would transfer this blocking behavior
                    //         to the resulting sequence. However, it seems never discarding a non-completed task should be a
                    //         general design tenet, and fire-and-forget dispose behavior could be added as another "unsafe"
                    //         operator, so all such sins are made explicit by the user. Nonetheless, this change is a breaking
                    //         change for Ix Async.
                    //

                    await disposeLoser.ConfigureAwait(false);
                }
            }
        }

        public static IAsyncEnumerable<TSource> Amb<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                //
                // REVIEW: See remarks on binary overload for changes compared to the original.
                //

                var n = sources.Length;

                var enumerators = new IAsyncEnumerator<TSource>[n];
                var moveNexts = new Task<bool>[n];

                try
                {
                    for (var i = 0; i < n; i++)
                    {
                        var enumerator = sources[i].GetAsyncEnumerator(cancellationToken);

                        enumerators[i] = enumerator;
                        moveNexts[i] = enumerator.MoveNextAsync().AsTask();
                    }
                }
                catch
                {
                    var cleanup = new Task[n];

                    for (var i = 0; i < n; i++)
                    {
                        cleanup[i] = AwaitMoveNextAsyncAndDispose(moveNexts[i], enumerators[i]);
                    }

                    await Task.WhenAll(cleanup).ConfigureAwait(false);
                    throw;
                }

                var moveNextWinner = await Task.WhenAny(moveNexts).ConfigureAwait(false);

                //
                // NB: The use of IndexOf is fine. If task N completed by returning a ValueTask<bool>
                //     which is equivalent to the task returned by task M (where M < N), AsTask may
                //     return the same reference (e.g. due to caching of completed Boolean tasks). In
                //     such a case, IndexOf will find task M rather than N in the array, but both have
                //     an equivalent completion state (because they're reference equal). This only leads
                //     to a left-bias in selection of sources, but given Amb's "ambiguous" nature, this
                //     is acceptable.
                //

                var winnerIndex = Array.IndexOf(moveNexts, moveNextWinner);

                var winner = enumerators[winnerIndex];

                var loserCleanupTasks = new List<Task>(n - 1);

                for (var i = 0; i < n; i++)
                {
                    if (i != winnerIndex)
                    {
                        var loserCleanupTask = AwaitMoveNextAsyncAndDispose(moveNexts[i], enumerators[i]);
                        loserCleanupTasks.Add(loserCleanupTask);
                    }
                }

                var cleanupLosers = Task.WhenAll(loserCleanupTasks);

                try
                {
                    await using (winner.ConfigureAwait(false))
                    {
                        if (!await moveNextWinner.ConfigureAwait(false))
                        {
                            yield break;
                        }

                        yield return winner.Current;

                        while (await winner.MoveNextAsync().ConfigureAwait(false))
                        {
                            yield return winner.Current;
                        }
                    }
                }
                finally
                {
                    await cleanupLosers.ConfigureAwait(false);
                }
            }
        }

        public static IAsyncEnumerable<TSource> Amb<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

            return Amb(sources.ToArray());
        }

        private static async Task AwaitMoveNextAsyncAndDispose<T>(Task<bool>? moveNextAsync, IAsyncEnumerator<T>? enumerator)
        {
            if (enumerator != null)
            {
                await using (enumerator.ConfigureAwait(false))
                {
                    if (moveNextAsync != null)
                    {
                        await moveNextAsync.ConfigureAwait(false);
                    }
                }
            }
        }
    }
}
