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
        public static IAsyncEnumerable<TSource> Concat<TSource>(this IAsyncEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return new ConcatAsyncEnumerableAsyncIterator<TSource>(sources);
        }

        public static IAsyncEnumerable<TSource> Concat<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return ConcatCore(sources);
        }

        public static IAsyncEnumerable<TSource> Concat<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return ConcatCore(sources);
        }

        private static IAsyncEnumerable<TSource> ConcatCore<TSource>(IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            return new ConcatEnumerableAsyncIterator<TSource>(sources);
        }

        private sealed class ConcatEnumerableAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IEnumerable<IAsyncEnumerable<TSource>> source;

            public ConcatEnumerableAsyncIterator(IEnumerable<IAsyncEnumerable<TSource>> source)
            {
                Debug.Assert(source != null);

                this.source = source;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new ConcatEnumerableAsyncIterator<TSource>(source);
            }

            public override async Task DisposeAsync()
            {
                if (outerEnumerator != null)
                {
                    outerEnumerator.Dispose();
                    outerEnumerator = null;
                }

                if (currentEnumerator != null)
                {
                    await currentEnumerator.DisposeAsync().ConfigureAwait(false);
                    currentEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            // State machine vars
            private IEnumerator<IAsyncEnumerable<TSource>> outerEnumerator;
            private IAsyncEnumerator<TSource> currentEnumerator;
            private int mode;

            private const int State_OuterNext = 1;
            private const int State_While = 4;

            protected override async Task<bool> MoveNextCore()
            {

                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        outerEnumerator = source.GetEnumerator();
                        mode = State_OuterNext;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (mode)
                        {
                            case State_OuterNext:
                                if (outerEnumerator.MoveNext())
                                {
                                    // make sure we dispose the previous one if we're about to replace it
                                    if (currentEnumerator != null)
                                    {
                                        await currentEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    currentEnumerator = outerEnumerator.Current.GetAsyncEnumerator();

                                    mode = State_While;
                                    goto case State_While;
                                }

                                break;
                            case State_While:
                                if (await currentEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    current = currentEnumerator.Current;
                                    return true;
                                }

                                // No more on the inner enumerator, move to the next outer
                                goto case State_OuterNext;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }

        private sealed class ConcatAsyncEnumerableAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly IAsyncEnumerable<IAsyncEnumerable<TSource>> source;

            public ConcatAsyncEnumerableAsyncIterator(IAsyncEnumerable<IAsyncEnumerable<TSource>> source)
            {
                Debug.Assert(source != null);

                this.source = source;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new ConcatAsyncEnumerableAsyncIterator<TSource>(source);
            }

            public override async Task DisposeAsync()
            {
                if (outerEnumerator != null)
                {
                    await outerEnumerator.DisposeAsync().ConfigureAwait(false);
                    outerEnumerator = null;
                }

                if (currentEnumerator != null)
                {
                    await currentEnumerator.DisposeAsync().ConfigureAwait(false);
                    currentEnumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            // State machine vars
            private IAsyncEnumerator<IAsyncEnumerable<TSource>> outerEnumerator;
            private IAsyncEnumerator<TSource> currentEnumerator;
            private int mode;

            private const int State_OuterNext = 1;
            private const int State_While = 4;

            protected override async Task<bool> MoveNextCore()
            {

                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        outerEnumerator = source.GetAsyncEnumerator();
                        mode = State_OuterNext;
                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        switch (mode)
                        {
                            case State_OuterNext:
                                if (await outerEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    // make sure we dispose the previous one if we're about to replace it
                                    if (currentEnumerator != null)
                                    {
                                        await currentEnumerator.DisposeAsync().ConfigureAwait(false);
                                    }

                                    currentEnumerator = outerEnumerator.Current.GetAsyncEnumerator();

                                    mode = State_While;
                                    goto case State_While;
                                }

                                break;
                            case State_While:
                                if (await currentEnumerator.MoveNextAsync().ConfigureAwait(false))
                                {
                                    current = currentEnumerator.Current;
                                    return true;
                                }

                                // No more on the inner enumerator, move to the next outer
                                goto case State_OuterNext;
                        }

                        await DisposeAsync().ConfigureAwait(false);
                        break;
                }

                return false;
            }
        }
    }
}
