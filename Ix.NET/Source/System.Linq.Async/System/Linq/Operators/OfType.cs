﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        // REVIEW: This is a non-standard LINQ operator, because we don't have a non-generic IAsyncEnumerable.
        //
        //         Unfortunately, this has limited use because it requires the source to be IAsyncEnumerable<object>,
        //         thus it doesn't bind for value types. Adding a first generic parameter for the element type of
        //         the source is not an option, because it would require users to specify two type arguments, unlike
        //         what's done in Enumerable.OfType. Should we move this method to Ix, thus doing away with OfType
        //         in the API surface altogether?
        // IDG 2025/09/30: System.Linq.AsyncEnumerable implemented this one despite the limitations (and despite
        //         not implementing AsAsyncEnumerable).

#if INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.oftype?view=net-9.0-pp

        /// <summary>
        /// Filters the elements of an async-enumerable sequence based on the specified type.
        /// </summary>
        /// <typeparam name="TResult">The type to filter the elements in the source sequence on.</typeparam>
        /// <param name="source">The async-enumerable sequence that contains the elements to be filtered.</param>
        /// <returns>An async-enumerable sequence that contains elements from the input sequence of type TResult.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IAsyncEnumerable<TResult> OfType<TResult>(this IAsyncEnumerable<object?> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source);

            static async IAsyncEnumerable<TResult> Core(IAsyncEnumerable<object?> source, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await foreach (var obj in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    if (obj is TResult result)
                    {
                        yield return result;
                    }
                }
            }
        }
#endif // INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
    }
}
