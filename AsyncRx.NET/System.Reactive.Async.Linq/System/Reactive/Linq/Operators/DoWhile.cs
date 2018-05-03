// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        // REVIEW: Use a tail-recursive sink.

        public static IAsyncObservable<TSource> DoWhile<TSource>(IAsyncObservable<TSource> source, Func<bool> condition)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            return Create<TSource>(async observer =>
            {
                var subscription = new SerialAsyncDisposable();

                var o = default(IAsyncObserver<TSource>);

                o = AsyncObserver.CreateUnsafe<TSource>(
                        observer.OnNextAsync,
                        observer.OnErrorAsync,
                        MoveNext
                    );

                async Task Subscribe()
                {
                    var sad = new SingleAssignmentAsyncDisposable();
                    await subscription.AssignAsync(sad).ConfigureAwait(false);

                    var d = await source.SubscribeSafeAsync(o).ConfigureAwait(false);
                    await sad.AssignAsync(d).ConfigureAwait(false);
                }

                async Task MoveNext()
                {
                    var b = default(bool);

                    try
                    {
                        b = condition();
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    if (b)
                    {
                        await Subscribe().ConfigureAwait(false);
                    }
                    else
                    {
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }

                await Subscribe().ConfigureAwait(false);

                return subscription;
            });
        }

        public static IAsyncObservable<TSource> DoWhile<TSource>(IAsyncObservable<TSource> source, Func<Task<bool>> condition)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            return Create<TSource>(async observer =>
            {
                var subscription = new SerialAsyncDisposable();

                var o = default(IAsyncObserver<TSource>);

                o = AsyncObserver.CreateUnsafe<TSource>(
                        observer.OnNextAsync,
                        observer.OnErrorAsync,
                        MoveNext
                    );

                async Task Subscribe()
                {
                    var sad = new SingleAssignmentAsyncDisposable();
                    await subscription.AssignAsync(sad).ConfigureAwait(false);

                    var d = await source.SubscribeSafeAsync(o).ConfigureAwait(false);
                    await sad.AssignAsync(d).ConfigureAwait(false);
                }

                async Task MoveNext()
                {
                    var b = default(bool);

                    try
                    {
                        b = await condition().ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    if (b)
                    {
                        await Subscribe().ConfigureAwait(false);
                    }
                    else
                    {
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }

                await Subscribe().ConfigureAwait(false);

                return subscription;
            });
        }
    }
}
