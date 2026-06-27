// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive
{
    public abstract class AsyncObservableBase<T> : IAsyncObservable<T>
    {
        public async ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var autoDetach = new AutoDetachAsyncObserver(observer);

            var subscription = await SubscribeAsyncCore(autoDetach).ConfigureAwait(false);

            await autoDetach.AssignAsync(subscription).ConfigureAwait(false);

            return autoDetach;
        }

        protected abstract ValueTask<IAsyncDisposable> SubscribeAsyncCore(IAsyncObserver<T> observer);

        private sealed class AutoDetachAsyncObserver : AsyncObserverBase<T>, IAsyncDisposable
        {
            private readonly IAsyncObserver<T> _observer;
            private TaskCompletionSource<object> _pendingOnSomethingCallsTcs;
            private readonly AsyncLocal<bool> _reentrancyFlag = new(); // If any On* method, calls OnDisposeAsync, this will be true
            private readonly object _gate = new();

            private IAsyncDisposable _subscription;
            private bool _disposing;

            public AutoDetachAsyncObserver(IAsyncObserver<T> observer)
            {
                _observer = observer;
            }

            public async ValueTask AssignAsync(IAsyncDisposable subscription)
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

            public async ValueTask DisposeAsync()
            {
                await FinishAsync().ConfigureAwait(false);
            }

            protected override async ValueTask OnCompletedAsyncCore()
            {
                ValueTask task;
                lock (_gate)
                {
                    if (_disposing)
                    {
                        return;
                    }

                    task = WithReentrancyFlagOn(static (@this, _) => @this._observer.OnCompletedAsync(), (object)null);
                }

                try
                {
                    await task.ConfigureAwait(false);
                }
                finally
                {
                    await FinishAsync().ConfigureAwait(false);
                }
            }

            protected override async ValueTask OnErrorAsyncCore(Exception error)
            {
                ValueTask task;
                lock (_gate)
                {
                    if (_disposing)
                    {
                        return;
                    }

                    task = WithReentrancyFlagOn(static (@this, error) => @this._observer.OnErrorAsync(error), error);
                }

                try
                {
                    await task.ConfigureAwait(false);
                }
                finally
                {
                    await FinishAsync().ConfigureAwait(false);
                }
            }

            protected override async ValueTask OnNextAsyncCore(T value)
            {
                ValueTask task;
                lock (_gate)
                {
                    if (_disposing)
                    {
                        return;
                    }

                    task = WithReentrancyFlagOn(static (@this, value) => @this._observer.OnNextAsync(value), value);
                }

                await task.ConfigureAwait(false);
            }

            private async ValueTask FinishAsync()
            {
                // On synchronous Rx, if Dispose is called while we're in the middle of an OnNext/OnError/OnCompleted,
                // we immediately execute the Dispose() method.
                // So it's possible that the On* method finishes after the Dispose() method has completed.
                // What it's impossible is that another On* method STARTS AFTER Dispose() has completed.

                Task onSomethingCall;
                IAsyncDisposable subscription;

                lock (_gate)
                {
                    if (_disposing)
                    {
                        return;
                    }

                    _disposing = true;
                    subscription = _subscription;
                    onSomethingCall = _reentrancyFlag.Value ? null : _pendingOnSomethingCallsTcs?.Task;
                }

                if (onSomethingCall != null)
                {
                    await onSomethingCall.ConfigureAwait(false);
                }

                if (subscription != null)
                {
                    await subscription.DisposeAsync().ConfigureAwait(false);
                }
            }

            private async ValueTask WithReentrancyFlagOn<TState>(Func<AutoDetachAsyncObserver, TState, ValueTask> asyncAction, TState state)
            {
                var runningMethod = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
                _pendingOnSomethingCallsTcs = runningMethod;
                _reentrancyFlag.Value = true;
                try
                {
                    await asyncAction(this, state).ConfigureAwait(false);
                }
                finally
                {
                    _reentrancyFlag.Value = false;
                    _pendingOnSomethingCallsTcs = null;
                    runningMethod.SetResult(null!);
                }
            }
        }
    }
}
