// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Joins;

namespace System.Reactive.Linq
{
    // REVIEW: Consider moving join patterns to a separate assembly.

    partial class AsyncObservable
    {
        // REVIEW: Consider adding async support.

        public static AsyncPlan<TResult> Then<TSource, TResult>(this IAsyncObservable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new AsyncPattern<TSource>(source).Then(selector);
        }
    }
}
