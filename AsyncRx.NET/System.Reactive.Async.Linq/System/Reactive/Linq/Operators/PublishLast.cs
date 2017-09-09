// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    // REVIEW: Expose PublishLast using ConcurrentAsyncAsyncSubject<T> underneath.

    partial class AsyncObservable
    {
        public static IConnectableAsyncObservable<TSource> PublishLast<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Multicast(source, new SequentialAsyncAsyncSubject<TSource>());
        }

        public static IAsyncObservable<TResult> PublishLast<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, IAsyncObservable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Multicast(source, () => new SequentialAsyncAsyncSubject<TSource>(), selector);
        }

        public static IAsyncObservable<TResult> PublishLast<TSource, TResult>(this IAsyncObservable<TSource> source, Func<IAsyncObservable<TSource>, Task<IAsyncObservable<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Multicast(source, () => Task.FromResult<IAsyncSubject<TSource, TSource>>(new SequentialAsyncAsyncSubject<TSource>()), selector);
        }
    }
}
