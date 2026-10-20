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
        // The three forms share Rx.NET's semantics for its task-based Create overloads, which
        // CallbackAsyncObservable implements: SubscribeAsync runs the callback inline and returns
        // when the callback returns, whether or not the task it returned has completed; what the
        // task produces becomes the subscription's resource when it arrives; a failure of the
        // callback, synchronous or asynchronous, is the sequence's error; the token is cancelled
        // when the subscriber unsubscribes, which also releases the resource.
        //
        // Every callback takes the token, as the runtime's async callbacks do; a callback with
        // no use for it ignores it. The operators' own plumbing is CreateAsyncObservable<T>.From,
        // which awaits its subscribe function, and is not this.

        public static IAsyncObservable<T> Create<T>(Func<IAsyncObserver<T>, CancellationToken, ValueTask<IAsyncDisposable>> subscribeAsync)
        {
            if (subscribeAsync == null)
                throw new ArgumentNullException(nameof(subscribeAsync));

            return new CallbackAsyncObservable<T>(subscribeAsync);
        }

        public static IAsyncObservable<T> Create<T>(Func<IAsyncObserver<T>, CancellationToken, ValueTask<Func<ValueTask>>> subscribeAsync)
        {
            if (subscribeAsync == null)
                throw new ArgumentNullException(nameof(subscribeAsync));

            return new CallbackAsyncObservable<T>(async (observer, token) =>
            {
                var dispose = await subscribeAsync(observer, token).ConfigureAwait(false);

                return dispose == null ? AsyncDisposable.Nop : AsyncDisposable.Create(dispose);
            });
        }

        // The auto-completing form: the callback does both the subscription and the production
        // of the elements, so the completion of its task completes the sequence.
        public static IAsyncObservable<T> Create<T>(Func<IAsyncObserver<T>, CancellationToken, ValueTask> subscribeAsync)
        {
            if (subscribeAsync == null)
                throw new ArgumentNullException(nameof(subscribeAsync));

            return new CallbackAsyncObservable<T>(async (observer, token) =>
            {
                await subscribeAsync(observer, token).ConfigureAwait(false);
                await observer.OnCompletedAsync().ConfigureAwait(false);

                return AsyncDisposable.Nop;
            });
        }
    }
}
