// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> DoWhile<TSource>(IAsyncObservable<TSource> source, Func<bool> condition)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            // As in Rx.NET: the source once, then for as long as the condition holds, through
            // Concat's tail-recursive sink (see While).
            return new ConcatAsyncObservable<TSource>(DoWhileCore(source, condition));
        }

        // The ValueTask-returning condition cannot be evaluated by a synchronous enumerator, so
        // this form still resubscribes from the completion handler and grows the stack with each
        // iteration of a synchronously completing source.
        public static IAsyncObservable<TSource> DoWhile<TSource>(IAsyncObservable<TSource> source, Func<ValueTask<bool>> condition)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            return Create(
                source,
                condition,
                static async (source, condition, observer) =>
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

                    async ValueTask MoveNext()
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

        private static IEnumerable<IAsyncObservable<TSource>> DoWhileCore<TSource>(IAsyncObservable<TSource> source, Func<bool> condition)
        {
            yield return source;

            while (condition())
            {
                yield return source;
            }
        }
    }
}
