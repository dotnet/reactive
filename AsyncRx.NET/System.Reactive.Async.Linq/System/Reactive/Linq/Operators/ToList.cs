// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<IList<TSource>> ToList<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource, IList<TSource>>(source, static (source, observer) => source.SubscribeSafeAsync(AsyncObserver.ToList(observer)));
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> ToList<TSource>(IAsyncObserver<IList<TSource>> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Aggregate<TSource, List<TSource>, IList<TSource>>(observer, new List<TSource>(), (xs, x) => { xs.Add(x); return xs; }, xs => xs);
        }
    }
}
