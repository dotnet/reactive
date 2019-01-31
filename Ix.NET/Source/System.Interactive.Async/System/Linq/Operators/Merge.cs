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
        public static IAsyncEnumerable<TSource> Merge<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

#if USE_ASYNC_ITERATOR
            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var count = sources.Length;

                var enumerators = new IAsyncEnumerator<TSource>[count];
                var moveNextTasks = new Task<bool>[count];

                try
                {
                    for (var i = 0; i < count; i++)
                    {
                        IAsyncEnumerator<TSource> enumerator = sources[i].GetAsyncEnumerator(cancellationToken);
                        enumerators[i] = enumerator;

                        // REVIEW: This follows the lead of the original implementation where we kick off MoveNextAsync
                        //         operations immediately. An alternative would be to do this in a separate stage, thus
                        //         preventing concurrency across MoveNextAsync and GetAsyncEnumerator calls and avoiding
                        //         any MoveNextAsync calls before all enumerators are acquired (or an exception has
                        //         occurred doing so).

                        moveNextTasks[i] = enumerator.MoveNextAsync().AsTask();
                    }

                    int active = count;

                    while (active > 0)
                    {
                        // REVIEW: Performance of WhenAny may be an issue when called repeatedly like this. We should
                        //         measure and could consider operating directly on the ValueTask<bool> objects, thus
                        //         also preventing the Task<bool> allocations from AsTask.

                        var moveNextTask = await Task.WhenAny(moveNextTasks).ConfigureAwait(false);

                        int index = Array.IndexOf(moveNextTasks, moveNextTask);

                        IAsyncEnumerator<TSource> enumerator = enumerators[index];

                        if (!await moveNextTask.ConfigureAwait(false))
                        {
                            moveNextTasks[index] = TaskExt.Never;

                            // REVIEW: The original implementation did not dispose eagerly, which could lead to resource
                            //         leaks when merged with other long-running sequences.

                            enumerators[index] = null; // NB: Avoids attempt at double dispose in finally if disposing fails.
                            await enumerator.DisposeAsync().ConfigureAwait(false);

                            active--;
                        }
                        else
                        {
                            TSource item = enumerator.Current;

                            moveNextTasks[index] = enumerator.MoveNextAsync().AsTask();

                            yield return item;
                        }
                    }
                }
                finally
                {
                    // REVIEW: The original implementation performs a concurrent dispose, which seems undesirable given the
                    //         additional uncontrollable source of concurrency and the sequential resource acquisition. In
                    //         this modern implementation, we release resources in opposite order as we acquired them, thus
                    //         guaranteeing determinism (and mimicking a series of nested `await using` statements).

                    // REVIEW: If we decide to phase GetAsyncEnumerator and the initial MoveNextAsync calls at the start of
                    //         the operator implementation, we should make this symmetric and first await all in flight
                    //         MoveNextAsync operations, prior to disposing the enumerators.

                    var errors = default(List<Exception>);

                    for (var i = count - 1; i >= 0; i--)
                    {
                        Task<bool> moveNextTask = moveNextTasks[i];
                        IAsyncEnumerator<TSource> enumerator = enumerators[i];

                        try
                        {
                            try
                            {
                                if (moveNextTask != null && moveNextTask != TaskExt.Never)
                                {
                                    _ = await moveNextTask.ConfigureAwait(false);
                                }
                            }
                            finally
                            {
                                if (enumerator != null)
                                {
                                    await enumerator.DisposeAsync().ConfigureAwait(false);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (errors == null)
                            {
                                errors = new List<Exception>();
                            }

                            errors.Add(ex);
                        }
                    }

                    // NB: If we had any errors during cleaning (and awaiting pending operations), we throw these exceptions
                    //     instead of the original exception that may have led to running the finally block. This is similar
                    //     to throwing from any finally block (except that we catch all exceptions to ensure cleanup of all
                    //     concurrent sequences being merged).

                    if (errors != null)
                    {
                        throw new AggregateException(errors);
                    }
                }
            }
#else
            return new MergeAsyncIterator<TSource>(sources);
#endif
        }

        public static IAsyncEnumerable<TSource> Merge<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

            //
            // REVIEW: This implementation does not exploit concurrency. We should not introduce such behavior in order to
            //         avoid breaking changes, but we could introduce a parallel ConcurrentMerge implementation. It is
            //         unfortunate though that the Merge overload accepting an array has always been concurrent, so we can't
            //         change that either (in order to have consistency where Merge is non-concurrent, and ConcurrentMerge
            //         is). We could consider a breaking change to Ix Async to streamline this, but we should do so when
            //         shipping with the BCL interfaces (which is already a breaking change to existing Ix Async users). If
            //         we go that route, we can either have:
            //
            //         - All overloads of Merge are concurrent
            //           - and continue to be named Merge, or,
            //           - are renamed to ConcurrentMerge for clarity (likely alongside a ConcurrentZip).
            //         - All overloads of Merge are non-concurrent
            //           - and are simply SelectMany operator macros (maybe more optimized)
            //         - Have ConcurrentMerge next to Merge overloads
            //           - where ConcurrentMerge may need a degree of concurrency parameter (and maybe other options), and,
            //           - where the overload set of both families may be asymmetric
            //

            return sources.ToAsyncEnumerable().SelectMany(source => source);
        }

        public static IAsyncEnumerable<TSource> Merge<TSource>(this IAsyncEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw Error.ArgumentNull(nameof(sources));

            //
            // REVIEW: This implementation does not exploit concurrency. We should not introduce such behavior in order to
            //         avoid breaking changes, but we could introduce a parallel ConcurrentMerge implementation.
            //

            return sources.SelectMany(source => source);
        }

        private sealed class MergeAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IAsyncEnumerable<TSource>[] _sources;

            private IAsyncEnumerator<TSource>[] _enumerators;
            private Task<bool>[] _moveNexts;
            private int _active;

            public MergeAsyncIterator(IAsyncEnumerable<TSource>[] sources)
            {
                Debug.Assert(sources != null);

                _sources = sources;
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new MergeAsyncIterator<TSource>(_sources);
            }

            public override async ValueTask DisposeAsync()
            {
                if (_enumerators != null)
                {
                    var n = _enumerators.Length;

                    var disposes = new ValueTask[n];

                    for (var i = 0; i < n; i++)
                    {
                        var dispose = _enumerators[i].DisposeAsync();
                        disposes[i] = dispose;
                    }

                    await Task.WhenAll(disposes.Select(t => t.AsTask())).ConfigureAwait(false);
                    _enumerators = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        var n = _sources.Length;

                        _enumerators = new IAsyncEnumerator<TSource>[n];
                        _moveNexts = new Task<bool>[n];
                        _active = n;

                        for (var i = 0; i < n; i++)
                        {
                            var enumerator = _sources[i].GetAsyncEnumerator(_cancellationToken);
                            _enumerators[i] = enumerator;
                            _moveNexts[i] = enumerator.MoveNextAsync().AsTask();
                        }

                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (_active > 0)
                        {
                            //
                            // REVIEW: This approach does have a bias towards giving sources on the left
                            //         priority over sources on the right when yielding values. We may
                            //         want to consider a "prefer fairness" option.
                            //

                            var moveNext = await Task.WhenAny(_moveNexts).ConfigureAwait(false);

                            var index = Array.IndexOf(_moveNexts, moveNext);

                            if (!await moveNext.ConfigureAwait(false))
                            {
                                _moveNexts[index] = TaskExt.Never;
                                _active--;
                            }
                            else
                            {
                                var enumerator = _enumerators[index];
                                _current = enumerator.Current;
                                _moveNexts[index] = enumerator.MoveNextAsync().AsTask();
                                return true;
                            }
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
    }
}
