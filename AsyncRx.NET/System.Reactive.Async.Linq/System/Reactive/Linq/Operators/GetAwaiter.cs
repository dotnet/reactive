// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Subjects;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static AsyncAsyncSubject<TSource> GetAwaiter<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var subject = new SequentialAsyncAsyncSubject<TSource>();

            var subscribeTask = source.SubscribeSafeAsync(subject);

            subscribeTask.ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    subject.OnErrorAsync(t.Exception); // NB: Should not occur due to use of SubscribeSafeAsync.
                }
            });

            return subject;
        }

        public static AsyncAsyncSubject<TSource> GetAwaiter<TSource>(this IConnectableAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var subject = new SequentialAsyncAsyncSubject<TSource>();

            var subscribeTask = source.SubscribeSafeAsync(subject);

            subscribeTask.ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    subject.OnErrorAsync(t.Exception); // NB: Should not occur due to use of SubscribeSafeAsync.
                }
                else
                {
                    source.ConnectAsync();
                }
            });

            return subject;
        }
    }
}
