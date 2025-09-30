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
        // NB: This is a non-standard LINQ operator, because we don't have a non-generic IAsyncEnumerable.
        //     We're keeping it to enable `from T x in xs` binding in C#.

#if INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
        // https://learn.microsoft.com/en-us/dotnet/api/system.linq.asyncenumerable.cast?view=net-9.0-pp

        /// <summary>
        /// Converts the elements of an async-enumerable sequence to the specified type.
        /// </summary>
        /// <typeparam name="TResult">The type to convert the elements in the source sequence to.</typeparam>
        /// <param name="source">The async-enumerable sequence that contains the elements to be converted.</param>
        /// <returns>An async-enumerable sequence that contains each element of the source sequence converted to the specified type.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IAsyncEnumerable<TResult> Cast<TResult>(this IAsyncEnumerable<object> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            if (source is IAsyncEnumerable<TResult> typedSource)
            {
                return typedSource;
            }

            return Core(source);

            static async IAsyncEnumerable<TResult> Core(IAsyncEnumerable<object> source, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                await foreach (var obj in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return (TResult)obj;
                }
            }
        }
#endif // INCLUDE_SYSTEM_LINQ_ASYNCENUMERABLE_DUPLICATES
    }
}
