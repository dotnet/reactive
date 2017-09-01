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
        public static Task<bool> IsEmpty<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.IsEmpty(CancellationToken.None);
        }

        public static Task<bool> IsEmpty<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return IsEmpty_(source, cancellationToken);
        }

        private static async Task<bool> IsEmpty_<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            return !await source.Any(cancellationToken).ConfigureAwait(false);
        }
    }
}
