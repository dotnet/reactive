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
        public static IAsyncEnumerable<TSource> Take<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (count <= 0)
            {
                return Empty<TSource>();
            }
            else if (source is IAsyncPartition<TSource> partition)
            {
                return partition.Take(count);
            }

            return new TakeAsyncIterator<TSource>(source, count);
        }

        private sealed class TakeAsyncIterator<TSource> : AsyncIterator<TSource>
        {
            private readonly int count;
            private readonly IAsyncEnumerable<TSource> source;

            private int currentCount;
            private IAsyncEnumerator<TSource> enumerator;

            public TakeAsyncIterator(IAsyncEnumerable<TSource> source, int count)
            {
                Debug.Assert(source != null);

                this.source = source;
                this.count = count;
                currentCount = count;
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new TakeAsyncIterator<TSource>(source, count);
            }

            public override async Task DisposeAsync()
            {
                if (enumerator != null)
                {
                    await enumerator.DisposeAsync().ConfigureAwait(false);
                    enumerator = null;
                }

                await base.DisposeAsync().ConfigureAwait(false);
            }


            protected override async Task<bool> MoveNextCore()
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        enumerator = source.GetAsyncEnumerator();

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (currentCount > 0 && await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            current = enumerator.Current;
                            currentCount--;
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
