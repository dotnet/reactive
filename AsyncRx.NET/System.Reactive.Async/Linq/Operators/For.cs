// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        // TODO: Add IAsyncEnumerable<T> based overlaod.

        public static IAsyncObservable<TResult> For<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, IAsyncObservable<TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            // As in Rx.NET: a concatenation of the selected sequences, through Concat's
            // tail-recursive sink (see While). The enumeration and the selector run inside the
            // sink's enumerator, so an exception from either ends the result with that error.
            return new ConcatAsyncObservable<TResult>(source.Select(resultSelector));
        }

        // The ValueTask-returning selector cannot be evaluated by a synchronous enumerator, so
        // this form still resubscribes from the completion handler and grows the stack with each
        // iteration of a synchronously completing source.
        public static IAsyncObservable<TResult> For<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, ValueTask<IAsyncObservable<TResult>>> resultSelector)
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

                async ValueTask MoveNext()
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
                    return default;
                });

                return StableCompositeAsyncDisposable.Create(disposeEnumerator, subscription);
            });
        }
    }
}
