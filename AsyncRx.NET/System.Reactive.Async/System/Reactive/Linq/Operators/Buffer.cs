// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<IList<TSource>> Buffer<TSource>(this IAsyncObservable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count <= 0)
                throw new ArgumentNullException(nameof(count));

            return Create<IList<TSource>>(observer => source.SubscribeAsync(AsyncObserver.Buffer(observer, count)));
        }

        public static IAsyncObservable<IList<TSource>> Buffer<TSource>(this IAsyncObservable<TSource> source, int count, int skip)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count <= 0)
                throw new ArgumentNullException(nameof(count));
            if (skip <= 0)
                throw new ArgumentNullException(nameof(skip));

            return Create<IList<TSource>>(observer => source.SubscribeAsync(AsyncObserver.Buffer(observer, count, skip)));
        }

        public static IAsyncObservable<IList<TSource>> Buffer<TSource>(this IAsyncObservable<TSource> source, TimeSpan timeSpan)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));

            return Create<IList<TSource>>(observer => source.SubscribeAsync(AsyncObserver.Buffer(observer, timeSpan)));
        }

        public static IAsyncObservable<IList<TSource>> Buffer<TSource>(this IAsyncObservable<TSource> source, TimeSpan timeSpan, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<IList<TSource>>(observer => source.SubscribeAsync(AsyncObserver.Buffer(observer, timeSpan, scheduler)));
        }

        public static IAsyncObservable<IList<TSource>> Buffer<TSource>(this IAsyncObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));
            if (timeShift < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeShift));

            return Create<IList<TSource>>(observer => source.SubscribeAsync(AsyncObserver.Buffer(observer, timeSpan, timeShift)));
        }

        public static IAsyncObservable<IList<TSource>> Buffer<TSource>(this IAsyncObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));
            if (timeShift < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeShift));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<IList<TSource>>(observer => source.SubscribeAsync(AsyncObserver.Buffer(observer, timeSpan, timeShift, scheduler)));
        }

        public static IAsyncObservable<IList<TSource>> Buffer<TSource>(this IAsyncObservable<TSource> source, TimeSpan timeSpan, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));
            if (count <= 0)
                throw new ArgumentNullException(nameof(count));

            return Create<IList<TSource>>(observer => source.SubscribeAsync(AsyncObserver.Buffer(observer, timeSpan, count)));
        }

        public static IAsyncObservable<IList<TSource>> Buffer<TSource>(this IAsyncObservable<TSource> source, TimeSpan timeSpan, int count, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));
            if (count <= 0)
                throw new ArgumentNullException(nameof(count));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<IList<TSource>>(observer => source.SubscribeAsync(AsyncObserver.Buffer(observer, timeSpan, count, scheduler)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Buffer<TSource>(IAsyncObserver<IList<TSource>> observer, int count)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (count <= 0)
                throw new ArgumentNullException(nameof(count));

            throw new NotImplementedException();
        }

        public static IAsyncObserver<TSource> Buffer<TSource>(IAsyncObserver<IList<TSource>> observer, int count, int skip)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (count <= 0)
                throw new ArgumentNullException(nameof(count));
            if (skip <= 0)
                throw new ArgumentNullException(nameof(skip));

            throw new NotImplementedException();
        }

        public static IAsyncObserver<TSource> Buffer<TSource>(IAsyncObserver<IList<TSource>> observer, TimeSpan timeSpan)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));

            throw new NotImplementedException();
        }

        public static IAsyncObserver<TSource> Buffer<TSource>(IAsyncObserver<IList<TSource>> observer, TimeSpan timeSpan, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            throw new NotImplementedException();
        }

        public static IAsyncObserver<TSource> Buffer<TSource>(IAsyncObserver<IList<TSource>> observer, TimeSpan timeSpan, TimeSpan timeShift)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));
            if (timeShift < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeShift));

            throw new NotImplementedException();
        }

        public static IAsyncObserver<TSource> Buffer<TSource>(IAsyncObserver<IList<TSource>> observer, TimeSpan timeSpan, TimeSpan timeShift, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));
            if (timeShift < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeShift));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            throw new NotImplementedException();
        }

        public static IAsyncObserver<TSource> Buffer<TSource>(IAsyncObserver<IList<TSource>> observer, TimeSpan timeSpan, int count)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));
            if (count <= 0)
                throw new ArgumentNullException(nameof(count));

            throw new NotImplementedException();
        }

        public static IAsyncObserver<TSource> Buffer<TSource>(IAsyncObserver<IList<TSource>> observer, TimeSpan timeSpan, int count, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentNullException(nameof(timeSpan));
            if (count <= 0)
                throw new ArgumentNullException(nameof(count));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            throw new NotImplementedException();
        }
    }
}
