// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<HashSet<TSource>> ToHashSet<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource, HashSet<TSource>>(source, static (source, observer) => source.SubscribeSafeAsync(AsyncObserver.ToHashSet(observer)));
        }

        public static IAsyncObservable<HashSet<TSource>> ToHashSet<TSource>(this IAsyncObservable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return CreateAsyncObservable<HashSet<TSource>>.From(
                source,
                comparer,
                static (source, comparer, observer) => source.SubscribeSafeAsync(AsyncObserver.ToHashSet(observer, comparer)));
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> ToHashSet<TSource>(IAsyncObserver<HashSet<TSource>> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return ToHashSet(observer, EqualityComparer<TSource>.Default);
        }

        public static IAsyncObserver<TSource> ToHashSet<TSource>(IAsyncObserver<HashSet<TSource>> observer, IEqualityComparer<TSource> comparer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Aggregate<TSource, HashSet<TSource>>(observer, new HashSet<TSource>(comparer), (set, x) => { set.Add(x); return set; });
        }
    }
}
