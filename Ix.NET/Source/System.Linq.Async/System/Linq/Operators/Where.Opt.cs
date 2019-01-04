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
        private static Func<TSource, bool> CombinePredicates<TSource>(Func<TSource, bool> predicate1, Func<TSource, bool> predicate2)
        {
            return x => predicate1(x) && predicate2(x);
        }

        private static Func<TSource, ValueTask<bool>> CombinePredicates<TSource>(Func<TSource, ValueTask<bool>> predicate1, Func<TSource, ValueTask<bool>> predicate2)
        {
            return async x => await predicate1(x).ConfigureAwait(false) && await predicate2(x).ConfigureAwait(false);
        }

#if !NO_DEEP_CANCELLATION
        private static Func<TSource, CancellationToken, ValueTask<bool>> CombinePredicates<TSource>(Func<TSource, CancellationToken, ValueTask<bool>> predicate1, Func<TSource, CancellationToken, ValueTask<bool>> predicate2)
        {
            return async (x, ct) => await predicate1(x, ct).ConfigureAwait(false) && await predicate2(x, ct).ConfigureAwait(false);
        }
#endif
    }
}
