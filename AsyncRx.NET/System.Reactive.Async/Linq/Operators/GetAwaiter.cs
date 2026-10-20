// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static AsyncAsyncSubject<TSource> GetAwaiter<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var subject = new SequentialAsyncAsyncSubject<TSource>();

            var ignored = RunAsyncCore(subject, async () =>
            {
                await source.SubscribeSafeAsync(subject).ConfigureAwait(false);
            });

            return subject;
        }

        public static AsyncAsyncSubject<TSource> GetAwaiter<TSource>(this IConnectableAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var subject = new SequentialAsyncAsyncSubject<TSource>();

            var ignored = RunAsyncCore(subject, async () =>
            {
                await source.SubscribeSafeAsync(subject).ConfigureAwait(false);
                await source.ConnectAsync().ConfigureAwait(false);
            });

            return subject;
        }

        // Runs the subscription as the caller's continuation, so that when it completes
        // synchronously (a hot source, a test scheduler) everything has happened by the time
        // the subject is returned, and when it does not, it resumes where the source completes
        // it rather than on a thread-pool continuation. A failure of the subscription itself
        // is the subject's error, as the exception a synchronous Subscribe throws is in Rx.NET.
        private static async ValueTask RunAsyncCore<TSource>(AsyncAsyncSubject<TSource> subject, Func<ValueTask> subscribeAsync)
        {
            try
            {
                await subscribeAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await subject.OnErrorAsync(ex).ConfigureAwait(false);
            }
        }
    }
}
