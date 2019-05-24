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
        // REVIEW: This is a non-standard LINQ operator, because we don't have a non-generic IAsyncEnumerable.
        //
        //         Unfortunately, this has limited use because it requires the source to be IAsyncEnumerable<object>,
        //         thus it doesn't bind for value types. Adding a first generic parameter for the element type of
        //         the source is not an option, because it would require users to specify two type arguments, unlike
        //         what's done in Enumerable.OfType. Should we move this method to Ix, thus doing away with OfType
        //         in the API surface altogether?

        public static IAsyncEnumerable<TResult> OfType<TResult>(this IAsyncEnumerable<object> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Create(Core);

            async IAsyncEnumerator<TResult> Core(CancellationToken cancellationToken)
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
    }
}
