// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    // REVIEW: Expose Publish using ConcurrentSimpleAsyncSubject<T> or ConcurrentBehaviorAsyncSubject<T> underneath.

    partial class AsyncObservable
    {
        public static IConnectableAsyncObservable<TSource> Publish<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Multicast(source, new SequentialSimpleAsyncSubject<TSource>());
        }

        public static IConnectableAsyncObservable<TSource> Publish<TSource>(this IAsyncObservable<TSource> source, TSource value)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Multicast(source, new SequentialBehaviorAsyncSubject<TSource>(value));
        }

        public static IAsyncObservable<TResult> Publish<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, IAsyncObservable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Multicast(source, () => new SequentialSimpleAsyncSubject<TSource>(), selector);
        }

        public static IAsyncObservable<TResult> Publish<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, IAsyncObservable<TResult>> selector, TSource value)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Multicast(source, () => new SequentialBehaviorAsyncSubject<TSource>(value), selector);
        }

        public static IAsyncObservable<TResult> Publish<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, Task<IAsyncObservable<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Multicast(source, () => Task.FromResult<IAsyncSubject<TSource, TSource>>(new SequentialSimpleAsyncSubject<TSource>()), selector);
        }

        public static IAsyncObservable<TResult> Publish<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, Task<IAsyncObservable<TResult>>> selector, TSource value)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Multicast(source, () => Task.FromResult<IAsyncSubject<TSource, TSource>>(new SequentialBehaviorAsyncSubject<TSource>(value)), selector);
        }
    }
}
