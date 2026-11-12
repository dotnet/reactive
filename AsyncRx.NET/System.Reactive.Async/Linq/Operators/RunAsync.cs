// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static AsyncAsyncSubject<TSource> RunAsync<TSource>(this IAsyncObservable<TSource> source, CancellationToken token)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var subject = new SequentialAsyncAsyncSubject<TSource>();

            if (token.IsCancellationRequested)
            {
                return Cancel(subject, token);
            }

            var subscription = new SingleAssignmentAsyncDisposable();

            var ignored = RunAsyncCore(subject, subscription, token, async () =>
            {
                await subscription.AssignAsync(await source.SubscribeSafeAsync(subject).ConfigureAwait(false)).ConfigureAwait(false);
            });

            return subject;
        }

        public static AsyncAsyncSubject<TSource> RunAsync<TSource>(this IConnectableAsyncObservable<TSource> source, CancellationToken token)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var subject = new SequentialAsyncAsyncSubject<TSource>();

            if (token.IsCancellationRequested)
            {
                return Cancel(subject, token);
            }

            var subscription = new CompositeAsyncDisposable();

            var ignored = RunAsyncCore(subject, subscription, token, async () =>
            {
                await subscription.AddAsync(await source.SubscribeSafeAsync(subject).ConfigureAwait(false)).ConfigureAwait(false);
                await subscription.AddAsync(await source.ConnectAsync().ConfigureAwait(false)).ConfigureAwait(false);
            });

            return subject;
        }

        private static AsyncAsyncSubject<TSource> Cancel<TSource>(AsyncAsyncSubject<TSource> subject, CancellationToken token)
        {
            var ignored = subject.OnErrorAsync(new OperationCanceledException(token));
            return subject;
        }

        // As GetAwaiter's core, with the token: cancelling disposes the subscription and fails
        // the subject with an OperationCanceledException, and the subject's own termination
        // releases the registration, as Rx.NET's RunAsync does.
        private static async ValueTask RunAsyncCore<TSource>(AsyncAsyncSubject<TSource> subject, IAsyncDisposable subscription, CancellationToken token, Func<ValueTask> subscribeAsync)
        {
            if (token.CanBeCanceled)
            {
                var registration = token.Register(() =>
                {
                    var ignored = CancelAsync(subject, subscription, token);
                });

                await subject.SubscribeAsync(AsyncObserver.Create<TSource>(
                    _ => default,
                    _ =>
                    {
                        registration.Dispose();
                        return default;
                    },
                    () =>
                    {
                        registration.Dispose();
                        return default;
                    })).ConfigureAwait(false);
            }

            await RunAsyncCore(subject, subscribeAsync).ConfigureAwait(false);
        }

        private static async ValueTask CancelAsync<TSource>(AsyncAsyncSubject<TSource> subject, IAsyncDisposable subscription, CancellationToken token)
        {
            await subscription.DisposeAsync().ConfigureAwait(false);
            await subject.OnErrorAsync(new OperationCanceledException(token)).ConfigureAwait(false);
        }
    }
}
