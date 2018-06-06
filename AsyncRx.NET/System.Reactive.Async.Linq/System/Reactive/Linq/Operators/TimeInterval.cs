// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TimeInterval<TSource>> TimeInterval<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TimeInterval<TSource>>(observer => source.SubscribeSafeAsync(AsyncObserver.TimeInterval(observer)));
        }

        public static IAsyncObservable<TimeInterval<TSource>> TimeInterval<TSource>(this IAsyncObservable<TSource> source, IClock clock)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (clock == null)
                throw new ArgumentNullException(nameof(clock));

            return Create<TimeInterval<TSource>>(observer => source.SubscribeSafeAsync(AsyncObserver.TimeInterval(observer, clock)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> TimeInterval<TSource>(IAsyncObserver<TimeInterval<TSource>> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return TimeInterval(observer, Clock.Default);
        }

        public static IAsyncObserver<TSource> TimeInterval<TSource>(IAsyncObserver<TimeInterval<TSource>> observer, IClock clock)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (clock == null)
                throw new ArgumentNullException(nameof(clock));

            var last = clock.Now;

            return Select<TSource, TimeInterval<TSource>>(observer, x =>
            {
                var now = clock.Now;
                var interval = now - last;
                last = now;

                return new TimeInterval<TSource>(x, interval);
            });
        }
    }
}
