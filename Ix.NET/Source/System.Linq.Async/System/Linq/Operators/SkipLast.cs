// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> SkipLast<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            if (count <= 0)
            {
                // Return source if not actually skipping, but only if it's a type from here, to avoid
                // issues if collections are used as keys or otherwise must not be aliased.
                if (source is AsyncIteratorBase<TSource>)
                {
                    return source;
                }

                count = 0;
            }

            return Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                var queue = new Queue<TSource>();

                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        if (queue.Count == count)
                        {
                            do
                            {
                                yield return queue.Dequeue();
                                queue.Enqueue(e.Current);
                            }
                            while (await e.MoveNextAsync());
                            break;
                        }
                        else
                        {
                            queue.Enqueue(e.Current);
                        }
                    }
                }
            }
        }
    }
}
