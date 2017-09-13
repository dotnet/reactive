// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive
{
    public abstract class AsyncObservableBase<T> : IAsyncObservable<T>
    {
        public async Task<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var autoDetach = new AutoDetachAsyncObserver(observer);

            var subscription = await SubscribeAsyncCore(autoDetach).ConfigureAwait(false);

            await autoDetach.AssignAsync(subscription);

            return autoDetach;
        }

        protected abstract Task<IAsyncDisposable> SubscribeAsyncCore(IAsyncObserver<T> observer);

        private sealed class AutoDetachAsyncObserver : AsyncObserverBase<T>, IAsyncDisposable
        {
            private readonly IAsyncObserver<T> _observer;
            private readonly object _gate = new object();

            private IAsyncDisposable _subscription;
            private Task _task;
            private bool _disposing;

            public AutoDetachAsyncObserver(IAsyncObserver<T> observer)
            {
                _observer = observer;
            }

            public async Task AssignAsync(IAsyncDisposable subscription)
            {
                var shouldDispose = false;

                lock (_gate)
                {
                    if (_disposing)
                    {
                        shouldDispose = true;
                    }
                    else
                    {
                        _subscription = subscription;
                    }
                }

                if (shouldDispose)
                {
                    await subscription.DisposeAsync().ConfigureAwait(false);
                }
            }

            public async Task DisposeAsync()
            {
                var task = default(Task);
                var subscription = default(IAsyncDisposable);

                lock (_gate)
                {
                    //
                    // NB: The postcondition of awaiting the first DisposeAsync call to complete is that all message
                    //     processing has ceased, i.e. no further On*AsyncCore calls will be made. This is achieved
                    //     here by setting _disposing to true, which is checked by the On*AsyncCore calls upon
                    //     entry, and by awaiting the task of any in-flight On*AsyncCore calls.
                    //
                    //     Timing of the disposal of the subscription is less deterministic due to the intersection
                    //     with the AssignAsync code path. However, the auto-detach observer can only be returned
                    //     from the SubscribeAsync call *after* a call to AssignAsync has been made and awaited, so
                    //     either AssignAsync triggers the disposal and an already disposed instance is returned, or
                    //     the user calling DisposeAsync will either encounter a busy observer which will be stopped
                    //     in its tracks (as described above) or it will trigger a disposal of the subscription. In
                    //     both these cases the result of awaiting DisposeAsync guarantees no further message flow.
                    //

                    if (!_disposing)
                    {
                        _disposing = true;

                        task = _task;
                        subscription = _subscription;
                    }
                }

                try
                {
                    //
                    // BUGBUG: This causes grief when an outgoing On*Async call reenters the DisposeAsync method and
                    //         results in the task returned from the On*Async call to be awaited to serialize the
                    //         call to subscription.DisposeAsync after it's done. We need to either detect reentrancy
                    //         and queue up the call to DisposeAsync or follow an when we trigger the disposal without
                    //         awaiting outstanding work (thus allowing for concurrency).
                    //
                    // if (task != null)
                    // {
                    //     await task.ConfigureAwait(false);
                    // }
                    //
                }
                finally
                {
                    if (subscription != null)
                    {
                        await subscription.DisposeAsync().ConfigureAwait(false);
                    }
                }
            }

            protected override async Task OnCompletedAsyncCore()
            {
                lock (_gate)
                {
                    if (_disposing)
                    {
                        return;
                    }

                    _task = _observer.OnCompletedAsync();
                }

                try
                {
                    await _task.ConfigureAwait(false);
                }
                finally
                {
                    await FinishAsync().ConfigureAwait(false);
                }
            }

            protected override async Task OnErrorAsyncCore(Exception error)
            {
                lock (_gate)
                {
                    if (_disposing)
                    {
                        return;
                    }

                    _task = _observer.OnErrorAsync(error);
                }

                try
                {
                    await _task.ConfigureAwait(false);
                }
                finally
                {
                    await FinishAsync().ConfigureAwait(false);
                }
            }

            protected override async Task OnNextAsyncCore(T value)
            {
                lock (_gate)
                {
                    if (_disposing)
                    {
                        return;
                    }

                    _task = _observer.OnNextAsync(value);
                }

                try
                {
                    await _task.ConfigureAwait(false);
                }
                finally
                {
                    lock (_gate)
                    {
                        _task = null;
                    }
                }
            }

            private async Task FinishAsync()
            {
                var subscription = default(IAsyncDisposable);

                lock (_gate)
                {
                    if (!_disposing)
                    {
                        _disposing = true;

                        subscription = _subscription;
                    }

                    _task = null;
                }

                if (subscription != null)
                {
                    await subscription.DisposeAsync().ConfigureAwait(false);
                }
            }
        }
    }
}
