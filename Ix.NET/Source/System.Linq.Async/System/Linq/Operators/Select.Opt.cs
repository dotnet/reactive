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
        private static Func<TSource, TResult> CombineSelectors<TSource, TMiddle, TResult>(Func<TSource, TMiddle> selector1, Func<TMiddle, TResult> selector2)
        {
            return x => selector2(selector1(x));
        }

        private static Func<TSource, ValueTask<TResult>> CombineSelectors<TSource, TMiddle, TResult>(Func<TSource, ValueTask<TMiddle>> selector1, Func<TMiddle, ValueTask<TResult>> selector2)
        {
            return async x => await selector2(await selector1(x).ConfigureAwait(false)).ConfigureAwait(false);
        }

#if !NO_DEEP_CANCELLATION
        private static Func<TSource, CancellationToken, ValueTask<TResult>> CombineSelectors<TSource, TMiddle, TResult>(Func<TSource, CancellationToken, ValueTask<TMiddle>> selector1, Func<TMiddle, CancellationToken, ValueTask<TResult>> selector2)
        {
            return async (x, ct) => await selector2(await selector1(x, ct).ConfigureAwait(false), ct).ConfigureAwait(false);
        }
#endif
    }
}
