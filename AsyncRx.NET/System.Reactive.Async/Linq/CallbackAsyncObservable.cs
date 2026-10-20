// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    // The observable behind the public Create overloads: a subscribe callback started on
    // subscription and left to run, with Rx.NET's semantics for its task-based Create.
    //
    // SubscribeAsync calls the callback inline and returns as soon as the callback returns. If
    // the callback's task has already completed, its result is attached before returning; if
    // not, the attachment is the task's continuation, run wherever the task completes (never on
    // a thread-pool continuation of the library's own making), and a result arriving after the
    // subscriber has unsubscribed is released at once. A failure of the callback, thrown
    // synchronously or as a faulted or cancelled task, is delivered to the observer's OnError,
    // as ObservableBase.Subscribe does for Rx.NET's Create; a synchronous one therefore reaches
    // the caller of SubscribeAsync only through whatever the observer's OnError does with it.
    // Unsubscribing cancels the callback's token first and releases the resource second.
    internal sealed class CallbackAsyncObservable<T> : AsyncObservableBase<T>
    {
        private readonly Func<IAsyncObserver<T>, CancellationToken, ValueTask<IAsyncDisposable>> _subscribeAsync;

        public CallbackAsyncObservable(Func<IAsyncObserver<T>, CancellationToken, ValueTask<IAsyncDisposable>> subscribeAsync)
        {
            _subscribeAsync = subscribeAsync;
        }

        protected override async ValueTask<IAsyncDisposable> SubscribeAsyncCore(IAsyncObserver<T> observer)
        {
            var cancellation = new CancellationTokenSource();
            var resource = new SingleAssignmentAsyncDisposable();

            var subscription = StableCompositeAsyncDisposable.Create(
                AsyncDisposable.Create(() =>
                {
                    cancellation.Cancel();
                    return default;
                }),
                resource);

            ValueTask<IAsyncDisposable> callback;

            try
            {
                callback = _subscribeAsync(observer, cancellation.Token);
            }
            catch (Exception ex)
            {
                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                return subscription;
            }

            if (callback.IsCompleted)
            {
                await AttachAsync(callback, observer, resource).ConfigureAwait(false);
            }
            else
            {
                var ignored = AttachAsync(callback, observer, resource);
            }

            return subscription;
        }

        private static async ValueTask AttachAsync(ValueTask<IAsyncDisposable> callback, IAsyncObserver<T> observer, SingleAssignmentAsyncDisposable resource)
        {
            IAsyncDisposable result;

            try
            {
                result = await callback.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                return;
            }

            await resource.AssignAsync(result ?? AsyncDisposable.Nop).ConfigureAwait(false);
        }
    }
}
