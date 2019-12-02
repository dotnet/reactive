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
        /// <summary>
        /// Invokes a specified action after the source async-enumerable sequence terminates gracefully or exceptionally.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="finallyAction">Action to invoke after the source async-enumerable sequence terminates.</param>
        /// <returns>Source sequence with the action-invoking termination behavior applied.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="finallyAction"/> is null.</exception>
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

        /// <summary>
        /// Invokes a specified asynchronous action after the source async-enumerable sequence terminates gracefully or exceptionally.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="finallyAction">Action to invoke and await asynchronously after the source async-enumerable sequence terminates.</param>
        /// <returns>Source sequence with the action-invoking termination behavior applied.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="finallyAction"/> is null.</exception>
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
