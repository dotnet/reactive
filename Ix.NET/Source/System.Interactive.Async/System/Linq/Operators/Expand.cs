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
        public static IAsyncEnumerable<TSource> Expand<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TSource>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var queue = new Queue<IAsyncEnumerable<TSource>>();

                queue.Enqueue(source);

                while (queue.Count > 0)
                {
                    await foreach (var item in queue.Dequeue().WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        queue.Enqueue(selector(item));

                        yield return item;
                    }
                }
            }
        }

        public static IAsyncEnumerable<TSource> Expand<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<IAsyncEnumerable<TSource>>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var queue = new Queue<IAsyncEnumerable<TSource>>();

                queue.Enqueue(source);

                while (queue.Count > 0)
                {
                    await foreach (var item in queue.Dequeue().WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        queue.Enqueue(await selector(item).ConfigureAwait(false));

                        yield return item;
                    }
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TSource> Expand<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<IAsyncEnumerable<TSource>>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var queue = new Queue<IAsyncEnumerable<TSource>>();

                queue.Enqueue(source);

                while (queue.Count > 0)
                {
                    await foreach (var item in queue.Dequeue().WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        queue.Enqueue(await selector(item, cancellationToken).ConfigureAwait(false));

                        yield return item;
                    }
                }
            }
        }
#endif
    }
}
