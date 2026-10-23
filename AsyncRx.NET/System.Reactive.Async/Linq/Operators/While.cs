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
        public static IAsyncObservable<TSource> While<TSource>(Func<bool> condition, IAsyncObservable<TSource> source)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            // As in Rx.NET: a concatenation of the source for as long as the condition holds,
            // through Concat's tail-recursive sink, so that a source completing synchronously
            // does not nest one more subscription's frames per iteration. The condition is
            // evaluated by the enumerator, before the first subscription and after each
            // completion, and an exception from it ends the result with that error.
            return new ConcatAsyncObservable<TSource>(WhileCore(condition, source));
        }

        // The ValueTask-returning condition cannot be evaluated by a synchronous enumerator, so
        // this form still resubscribes from the completion handler and grows the stack with each
        // iteration of a synchronously completing source.
        public static IAsyncObservable<TSource> While<TSource>(Func<ValueTask<bool>> condition, IAsyncObservable<TSource> source)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return CreateAsyncObservable<TSource>.From(async observer =>
            {
                var subscription = new SerialAsyncDisposable();

                var o = default(IAsyncObserver<TSource>);

                o = AsyncObserver.CreateUnsafe<TSource>(
                        observer.OnNextAsync,
                        observer.OnErrorAsync,
                        MoveNext
                    );

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
                        var sad = new SingleAssignmentAsyncDisposable();
                        await subscription.AssignAsync(sad).ConfigureAwait(false);

                        var d = await source.SubscribeSafeAsync(o).ConfigureAwait(false);
                        await sad.AssignAsync(d).ConfigureAwait(false);
                    }
                    else
                    {
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }

                await MoveNext().ConfigureAwait(false);

                return subscription;
            });
        }

        private static IEnumerable<IAsyncObservable<TSource>> WhileCore<TSource>(Func<bool> condition, IAsyncObservable<TSource> source)
        {
            while (condition())
            {
                yield return source;
            }
        }
    }
}
