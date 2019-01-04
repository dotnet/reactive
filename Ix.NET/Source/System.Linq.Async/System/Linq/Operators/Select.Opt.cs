// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        private static Func<TSource, TResult> CombineSelectors<TSource, TMiddle, TResult>(Func<TSource, TMiddle> selector1, Func<TMiddle, TResult> selector2)
        {
            if (selector1.Target is ICombinedSelectors<TSource, TMiddle> c)
            {
                return c.Combine(selector2).Invoke;
            }
            else
            {
                return new CombinedSelectors2<TSource, TMiddle, TResult>(selector1, selector2).Invoke;
            }
        }

        private interface ICombinedSelectors<TSource, TResult>
        {
            ICombinedSelectors<TSource, TNewResult> Combine<TNewResult>(Func<TResult, TNewResult> selector);
            TResult Invoke(TSource x);
        }

        private static Func<TSource, ValueTask<TResult>> CombineSelectors<TSource, TMiddle, TResult>(Func<TSource, ValueTask<TMiddle>> selector1, Func<TMiddle, ValueTask<TResult>> selector2)
        {
            if (selector1.Target is ICombinedAsyncSelectors<TSource, TMiddle> c)
            {
                return c.Combine(selector2).Invoke;
            }
            else
            {
                return new CombinedAsyncSelectors2<TSource, TMiddle, TResult>(selector1, selector2).Invoke;
            }
        }

        private interface ICombinedAsyncSelectors<TSource, TResult>
        {
            ICombinedAsyncSelectors<TSource, TNewResult> Combine<TNewResult>(Func<TResult, ValueTask<TNewResult>> selector);
            ValueTask<TResult> Invoke(TSource x);
        }

#if !NO_DEEP_CANCELLATION
        private static Func<TSource, CancellationToken, ValueTask<TResult>> CombineSelectors<TSource, TMiddle, TResult>(Func<TSource, CancellationToken, ValueTask<TMiddle>> selector1, Func<TMiddle, CancellationToken, ValueTask<TResult>> selector2)
        {
            if (selector1.Target is ICombinedAsyncSelectorsWithCancellation<TSource, TMiddle> c)
            {
                return c.Combine(selector2).Invoke;
            }
            else
            {
                return new CombinedAsyncSelectorsWithCancellation2<TSource, TMiddle, TResult>(selector1, selector2).Invoke;
            }
        }

        private interface ICombinedAsyncSelectorsWithCancellation<TSource, TResult>
        {
            ICombinedAsyncSelectorsWithCancellation<TSource, TNewResult> Combine<TNewResult>(Func<TResult, CancellationToken, ValueTask<TNewResult>> selector);
            ValueTask<TResult> Invoke(TSource x, CancellationToken ct);
        }
#endif
    }
}
