// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TAccumulate> Scan<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return new ScanAsyncEnumerable<TSource, TAccumulate>(source, seed, accumulator);
        }

        public static IAsyncEnumerable<TSource> Scan<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return new ScanAsyncEnumerable<TSource>(source, accumulator);
        }

        private sealed class ScanAsyncEnumerable<TSource, TAccumulate> : AsyncIterator<TAccumulate>
        {
            private readonly Func<TAccumulate, TSource, TAccumulate> accumulator;
            private readonly TAccumulate seed;
            private readonly IAsyncEnumerable<TSource> source;

            private TAccumulate accumulated;
            private IAsyncEnumerator<TSource> enumerator;

            public ScanAsyncEnumerable(IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
            {
                Debug.Assert(source != null);
                Debug.Assert(accumulator != null);

                this.source = source;
                this.seed = seed;
                this.accumulator = accumulator;
            }

            public override AsyncIterator<TAccumulate> Clone()
            {
                return new ScanAsyncEnumerable<TSource, TAccumulate>(source, seed, accumulator);
            }

            public override async Task DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                    accumulated = default(TAccumulate);
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetAsyncEnumerator();
                        accumulated = seed;

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (await enumerator.MoveNextAsync()
                                             .ConfigureAwait(false))
                        {
                            var item = enumerator.Current;
                            accumulated = accumulator(accumulated, item);
                            current = accumulated;
                            return true;
                        }

                        break;
                        
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }

        private sealed class ScanAsyncEnumerable<TSource> : AsyncIterator<TSource>
        {
            private readonly Func<TSource, TSource, TSource> accumulator;
            private readonly IAsyncEnumerable<TSource> source;

            private TSource accumulated;
            private IAsyncEnumerator<TSource> enumerator;

            private bool hasSeed;

            public ScanAsyncEnumerable(IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator)
            {
                Debug.Assert(source != null);
                Debug.Assert(accumulator != null);

                this.source = source;
                this.accumulator = accumulator;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new ScanAsyncEnumerable<TSource>(source, accumulator);
            }

            public override async Task DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                    accumulated = default(TSource);
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetAsyncEnumerator();
                        hasSeed = false;
                        accumulated = default(TSource);

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:

                        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            var item = enumerator.Current;
                            if (!hasSeed)
                            {
                                hasSeed = true;
                                accumulated = item;
                                continue; // loop
                            }

                            accumulated = accumulator(accumulated, item);
                            current = accumulated;
                            return true;
                        }

                        break; // case

                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
    }
}
