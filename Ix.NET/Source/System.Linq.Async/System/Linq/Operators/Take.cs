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
        public static IAsyncEnumerable<TSource> Take<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (count <= 0)
            {
                return new EmptyTake<TSource>(source);
            }
            else if (source is IAsyncPartition<TSource> partition)
            {
                return partition.Take(count);
            }
            else if (source is IList<TSource> list)
            {
                return new AsyncListPartition<TSource>(list, 0, count - 1);
            }

            return new AsyncEnumerablePartition<TSource>(source, 0, count - 1);
        }

        /// <summary>
        /// An empty source that triggers any side-effects with GetAsyncEnumerator
        /// and disposes it immediately when the consumer calls MoveNextAsync.
        /// </summary>
        /// <typeparam name="TSource">The element type of the source async sequence.</typeparam>
        private sealed class EmptyTake<TSource> : IAsyncEnumerable<TSource>
        {
            private readonly IAsyncEnumerable<TSource> _source;

            public EmptyTake(IAsyncEnumerable<TSource> source)
            {
                _source = source;
            }

            public IAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return new EmptyTakeEnumerator(_source.GetAsyncEnumerator(cancellationToken));
            }

            private sealed class EmptyTakeEnumerator : IAsyncEnumerator<TSource>
            {
                private readonly IAsyncEnumerator<TSource> _source;

                public TSource Current => default;

                public EmptyTakeEnumerator(IAsyncEnumerator<TSource> source)
                {
                    _source = source;
                }

                public ValueTask DisposeAsync()
                {
                    return TaskExt.CompletedTask;
                }

                public async ValueTask<bool> MoveNextAsync()
                {
                    await _source.DisposeAsync().ConfigureAwait(false);
                    return false;
                }
            }
        }
    }
}
