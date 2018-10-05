// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        // REVIEW: Use a tail-recursive sink.
        // TODO: Add IAsyncEnumerable<T> based overlaod.

        public static IAsyncObservable<TResult> For<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, IAsyncObservable<TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return For(source, x => Task.FromResult(resultSelector(x)));
        }

        public static IAsyncObservable<TResult> For<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, Task<IAsyncObservable<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return Create<TResult>(async observer =>
            {
                var subscription = new SerialAsyncDisposable();

                var enumerator = source.GetEnumerator();

                var o = default(IAsyncObserver<TResult>);

                o = AsyncObserver.CreateUnsafe<TResult>(
                        observer.OnNextAsync,
                        observer.OnErrorAsync,
                        MoveNext
                    );

                async Task MoveNext()
                {
                    var b = default(bool);
                    var next = default(IAsyncObservable<TResult>);

                    try
                    {
                        b = enumerator.MoveNext();

                        if (b)
                        {
                            next = await resultSelector(enumerator.Current).ConfigureAwait(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    if (b)
                    {
                        var sad = new SingleAssignmentAsyncDisposable();
                        await subscription.AssignAsync(sad).ConfigureAwait(false);

                        var d = await next.SubscribeSafeAsync(o).ConfigureAwait(false);
                        await sad.AssignAsync(d).ConfigureAwait(false);
                    }
                    else
                    {
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }

                await MoveNext().ConfigureAwait(false);

                var disposeEnumerator = AsyncDisposable.Create(() =>
                {
                    enumerator.Dispose();
                    return Task.CompletedTask;
                });

                return StableCompositeAsyncDisposable.Create(disposeEnumerator, subscription);
            });
        }
    }
}
