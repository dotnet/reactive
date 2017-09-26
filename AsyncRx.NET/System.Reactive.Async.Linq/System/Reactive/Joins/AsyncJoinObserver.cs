// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Joins
{
    internal sealed class AsyncJoinObserver<T> : AsyncObserverBase<Notification<T>>, IAsyncJoinObserver
    {
        private readonly IAsyncObservable<T> _source;
        private readonly Func<Exception, Task> _onError;

        private readonly List<ActiveAsyncPlan> _activePlans = new List<ActiveAsyncPlan>();
        private readonly SingleAssignmentAsyncDisposable _subscription = new SingleAssignmentAsyncDisposable();

        private AsyncLock _gate;
        private bool _isDisposed;

        public AsyncJoinObserver(IAsyncObservable<T> source, Func<Exception, Task> onError)
        {
            _source = source;
            _onError = onError;
        }

        public Queue<Notification<T>> Queue { get; } = new Queue<Notification<T>>();

        public void Dequeue() => Queue.Dequeue();

        public void AddActivePlan(ActiveAsyncPlan activePlan)
        {
            _activePlans.Add(activePlan);
        }

        internal async Task RemoveActivePlan(ActiveAsyncPlan activePlan)
        {
            _activePlans.Remove(activePlan);

            if (_activePlans.Count == 0)
            {
                await DisposeAsync().ConfigureAwait(false);
            }
        }

        public async Task DisposeAsync()
        {
            if (!_isDisposed)
            {
                await _subscription.DisposeAsync().ConfigureAwait(false);

                _isDisposed = true;
            }
        }

        public async Task SubscribeAsync(AsyncLock gate)
        {
            _gate = gate;

            var d = await _source.Materialize().SubscribeSafeAsync(this).ConfigureAwait(false);
            await _subscription.AssignAsync(d).ConfigureAwait(false);
        }

        protected override Task OnCompletedAsyncCore() => Task.CompletedTask;

        protected override Task OnErrorAsyncCore(Exception error) => Task.CompletedTask;

        protected override async Task OnNextAsyncCore(Notification<T> notification)
        {
            using (await _gate.LockAsync().ConfigureAwait(false))
            {
                if (!_isDisposed)
                {
                    if (notification.Kind == NotificationKind.OnError)
                    {
                        await _onError(notification.Exception).ConfigureAwait(false);
                    }
                    else
                    {
                        Queue.Enqueue(notification);

                        var plans = _activePlans.ToArray();

                        for (var i = 0; i < plans.Length; i++)
                        {
                            await plans[i].Match().ConfigureAwait(false); // REVIEW: Consider concurrent matching.
                        }
                    }
                }
            }
        }
    }
}
