// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<bool> IsEmpty<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<bool>(observer => source.SubscribeSafeAsync(AsyncObserver.IsEmpty<TSource>(observer)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> IsEmpty<TSource>(IAsyncObserver<bool> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Any<TSource>(Select<bool, bool>(observer, b => !b));
        }
    }
}
