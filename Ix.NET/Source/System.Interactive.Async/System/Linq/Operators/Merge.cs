// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        public static IAsyncEnumerable<TSource> Merge<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return new MergeAsyncIterator<TSource>(sources);
        }

        public static IAsyncEnumerable<TSource> Merge<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return sources.ToAsyncEnumerable().SelectMany(source => source);
        }

        public static IAsyncEnumerable<TSource> Merge<TSource>(this IAsyncEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return sources.SelectMany(source => source);
        }

        private sealed class MergeAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IAsyncEnumerable<TSource>[] sources;

            private IAsyncEnumerator<TSource>[] enumerators;
            private ValueTask<bool>[] moveNexts;
            private int active;

            public MergeAsyncIterator(IAsyncEnumerable<TSource>[] sources)
            {
                Debug.Assert(sources != null);

                this.sources = sources;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new MergeAsyncIterator<TSource>(sources);
            }

            public override async ValueTask DisposeAsync()
            {
                if (enumerators != null)
                {
                    var n = enumerators.Length;

                    var disposes = new ValueTask[n];

                    for (var i = 0; i < n; i++)
                    {
                        var dispose = enumerators[i].DisposeAsync();
                        disposes[i] = dispose;
                    }

                    await Task.WhenAll(disposes.Select(t => t.AsTask())).ConfigureAwait(false);
                    enumerators = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        var n = sources.Length;

                        enumerators = new IAsyncEnumerator<TSource>[n];
                        moveNexts = new ValueTask<bool>[n];
                        active = n;

                        for (var i = 0; i < n; i++)
                        {
                            var enumerator = sources[i].GetAsyncEnumerator();
                            enumerators[i] = enumerator;
                            moveNexts[i] = enumerator.MoveNextAsync();
                        }

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        while (active > 0)
                        {
                            //
                            // REVIEW: This approach does have a bias towards giving sources on the left
                            //         priority over sources on the right when yielding values. We may
                            //         want to consider a "prefer fairness" option.
                            //

                            var moveNext = await Task.WhenAny(moveNexts.Select(t => t.AsTask())).ConfigureAwait(false);

                            var index = Array.IndexOf(moveNexts, moveNext);

                            if (!await moveNext.ConfigureAwait(false))
                            {
                                moveNexts[index] = TaskExt.Never;
                                active--;
                            }
                            else
                            {
                                var enumerator = enumerators[index];
                                current = enumerator.Current;
                                moveNexts[index] = enumerator.MoveNextAsync();
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
