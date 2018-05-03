// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;

namespace System.Reactive.Linq
{
    // REVIEW: Consider using these for GetAwaiter.

    partial class AsyncObservable
    {
        public static AsyncAsyncSubject<TSource> RunAsync<TSource>(this IAsyncObservable<TSource> source, CancellationToken token)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var subject = new SequentialAsyncAsyncSubject<TSource>();

            if (token.IsCancellationRequested)
            {
                var ignored = subject.OnErrorAsync(new OperationCanceledException(token));
                return subject;
            }

            var subscribeTask = source.SubscribeSafeAsync(subject);

            subscribeTask.ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    subject.OnErrorAsync(t.Exception); // NB: Should not occur due to use of SubscribeSafeAsync.
                }
            });

            if (token.CanBeCanceled)
            {
                var d = new SingleAssignmentAsyncDisposable();

                subscribeTask.ContinueWith(t =>
                {
                    if (t.Exception == null)
                    {
                        var ignored = d.AssignAsync(t.Result);
                    }
                });

                token.Register(() =>
                {
                    var ignored = d.DisposeAsync();
                });
            }

            return subject;
        }

        public static AsyncAsyncSubject<TSource> RunAsync<TSource>(this IConnectableAsyncObservable<TSource> source, CancellationToken token)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var subject = new SequentialAsyncAsyncSubject<TSource>();

            if (token.IsCancellationRequested)
            {
                var ignored = subject.OnErrorAsync(new OperationCanceledException(token));
                return subject;
            }

            var d = new CompositeAsyncDisposable();

            var subscribeTask = source.SubscribeSafeAsync(subject);

            subscribeTask.ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    subject.OnErrorAsync(t.Exception); // NB: Should not occur due to use of SubscribeSafeAsync.
                }
                else
                {
                    var ignored = d.AddAsync(t.Result);

                    source.ConnectAsync().ContinueWith(t2 =>
                    {
                        if (t2.Exception == null)
                        {
                            var ignored2 = d.AddAsync(t2.Result);
                        }
                    });
                }
            });

            if (token.CanBeCanceled)
            {
                token.Register(() =>
                {
                    var ignored = d.DisposeAsync();
                });
            }

            return subject;
        }
    }
}
