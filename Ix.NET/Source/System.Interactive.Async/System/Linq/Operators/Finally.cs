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
        public static IAsyncEnumerable<TSource> Finally<TSource>(this IAsyncEnumerable<TSource> source, Action finallyAction)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (finallyAction == null)
                throw Error.ArgumentNull(nameof(finallyAction));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                try
                {
                    await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
                finally
                {
                    finallyAction();
                }
            }
        }

        public static IAsyncEnumerable<TSource> Finally<TSource>(this IAsyncEnumerable<TSource> source, Func<Task> finallyAction)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (finallyAction == null)
                throw Error.ArgumentNull(nameof(finallyAction));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                try
                {
                    await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                    {
                        yield return item;
                    }
                }
                finally
                {
                    // REVIEW: No cancellation support for finally action.

                    await finallyAction().ConfigureAwait(false);
                }
            }
        }
    }
}
