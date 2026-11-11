// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static Task ForEachAsync<TSource>(this IAsyncObservable<TSource> source, Action<TSource> onNext, CancellationToken token = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            return ForEachAsyncCore(source, (x, i) => { onNext(x); return Task.CompletedTask; }, token);
        }

        public static Task ForEachAsync<TSource>(this IAsyncObservable<TSource> source, Func<TSource, Task> onNext, CancellationToken token = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            return ForEachAsyncCore(source, (x, i) => onNext(x), token);
        }

        public static Task ForEachAsync<TSource>(this IAsyncObservable<TSource> source, Action<TSource, int> onNext, CancellationToken token = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            return ForEachAsyncCore(source, (x, i) => { onNext(x, i); return Task.CompletedTask; }, token);
        }

        public static Task ForEachAsync<TSource>(this IAsyncObservable<TSource> source, Func<TSource, int, Task> onNext, CancellationToken token = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            return ForEachAsyncCore(source, onNext, token);
        }

        private static async Task ForEachAsyncCore<TSource>(IAsyncObservable<TSource> source, Func<TSource, int, Task> onNext, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var tcs = new TaskCompletionSource<object>();

            var subscription = new SingleAssignmentAsyncDisposable();

            using (token.Register(() =>
            {
                tcs.TrySetCanceled(token);

                var ignored = DisposeOnCancelAsync(subscription);
            }))
            {
                var i = 0;

                var o = AsyncObserver.Create<TSource>(
                    async x =>
                    {
                        try
                        {
                            await onNext(x, checked(i++)).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                tcs.TrySetException(ex);
                            }
                            finally
                            {
                                await subscription.DisposeAsync().ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        try
                        {
                            tcs.TrySetException(ex);
                        }
                        finally
                        {
                            await subscription.DisposeAsync().ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        try
                        {
                            tcs.TrySetResult(null);
                        }
                        finally
                        {
                            await subscription.DisposeAsync().ConfigureAwait(false);
                        }
                    }
                );

                //
                // NB: If any of the lines below throw, the result will go into the Task returned from the async method.
                //     There's also no need to use SubscribeSafeAsync here; the exception will propagate just fine.
                //

                var d = await source.SubscribeAsync(o).ConfigureAwait(false);

                await subscription.AssignAsync(d).ConfigureAwait(false);

                await tcs.Task.ConfigureAwait(false);
            }
        }

        // The cancellation callback cannot await the disposal, so it is left to run on its own.
        // Calling an async method and discarding its task runs the disposal on the callback's
        // thread until it suspends and resumes it wherever it completes; a continuation attached
        // to the disposal's task would instead be queued to the thread pool, moving work off the
        // scheduler the subscription was cancelled on. A failure of the disposal has nowhere to
        // go, since the task returned to the caller has already been cancelled, so it is dropped.
        private static async ValueTask DisposeOnCancelAsync(IAsyncDisposable subscription)
        {
            try
            {
                await subscription.DisposeAsync().ConfigureAwait(false);
            }
            catch
            {
                // Nothing can observe a failure of the disposal; see above.
            }
        }
    }
}
