// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Joins;

namespace System.Reactive.Linq
{
    // REVIEW: Consider moving join patterns to a separate assembly.

    partial class AsyncObservable
    {
        public static AsyncPattern<TLeft, TRight> And<TLeft, TRight>(this IAsyncObservable<TLeft> left, IAsyncObservable<TRight> right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            return new AsyncPattern<TLeft, TRight>(left, right);
        }
    }
}
