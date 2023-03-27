// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<Timestamped<TSource>> Timestamp<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource, Timestamped<TSource>>(source, static (source, observer) => source.SubscribeSafeAsync(AsyncObserver.Timestamp(observer)));
        }

        public static IAsyncObservable<Timestamped<TSource>> Timestamp<TSource>(this IAsyncObservable<TSource> source, IClock clock)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (clock == null)
                throw new ArgumentNullException(nameof(clock));

            return CreateAsyncObservable<Timestamped<TSource>>.From(
                source,
                clock,
                static (source, clock, observer) => source.SubscribeSafeAsync(AsyncObserver.Timestamp(observer, clock)));
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Timestamp<TSource>(IAsyncObserver<Timestamped<TSource>> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Timestamp(observer, Clock.Default);
        }

        public static IAsyncObserver<TSource> Timestamp<TSource>(IAsyncObserver<Timestamped<TSource>> observer, IClock clock)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (clock == null)
                throw new ArgumentNullException(nameof(clock));

            return Select<TSource, Timestamped<TSource>>(observer, x => new Timestamped<TSource>(x, clock.Now));
        }
    }
}
