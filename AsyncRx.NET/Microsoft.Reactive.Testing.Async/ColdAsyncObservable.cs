// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Microsoft.Reactive.Testing.Async;

internal sealed class ColdAsyncObservable<T> : ITestableAsyncObservable<T>
{
    private readonly TestAsyncScheduler _scheduler;
    private readonly List<AsyncSubscription> _subscriptions = [];
    private readonly Recorded<Notification<T>>[] _messages;

    public ColdAsyncObservable(TestAsyncScheduler scheduler, Recorded<Notification<T>>[] messages)
    {
        _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        _messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public IReadOnlyList<AsyncSubscription> Subscriptions => _subscriptions;

    public IReadOnlyList<Recorded<Notification<T>>> Messages => _messages;

    public async ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer)
    {
        if (observer == null)
        {
            throw new ArgumentNullException(nameof(observer));
        }

        var index = _subscriptions.Count;
        _subscriptions.Add(new AsyncSubscription(OperationTime.StartingAt(_scheduler.Clock), OperationTime.Never));

        await _scheduler.YieldPoint();

        var state = new SubscriptionState(observer);
        var subscribedAt = _scheduler.Clock;

        foreach (var message in _messages)
        {
            var notification = message.Value;

            state.Schedules.Add(_scheduler.ScheduleAbsolute(
                subscribedAt + message.Time,
                _ =>
                {
                    var delivery = DeliverAsync(state, notification);
                    state.DeliveryTail = delivery;
                    return new ValueTask(delivery);
                },
                $"cold observable delivery of {notification} (relative tick {message.Time}) for the subscription at {subscribedAt}"));
        }

        _subscriptions[index] = new AsyncSubscription(_subscriptions[index].Subscribe.CompletedAt(_scheduler.Clock), OperationTime.Never);

        return new SubscriptionDisposable(this, state, index);
    }

    private async Task DeliverAsync(SubscriptionState state, Notification<T> notification)
    {
        // Same serialized-delivery rule as the hot observable, per subscription.
        try
        {
            await state.DeliveryTail;
        }
        catch
        {
        }

        await _scheduler.YieldPoint();

        if (!state.Disposed)
        {
            await notification.AcceptAsync(state.Observer);
        }
    }

    private sealed class SubscriptionState(IAsyncObserver<T> observer)
    {
        public IAsyncObserver<T> Observer { get; } = observer;
        public List<IDisposable> Schedules { get; } = [];
        public Task DeliveryTail { get; set; } = Task.CompletedTask;
        public bool Disposed { get; set; }
    }

    private sealed class SubscriptionDisposable(ColdAsyncObservable<T> parent, SubscriptionState state, int index) : IAsyncDisposable
    {
        public async ValueTask DisposeAsync()
        {
            var current = parent._subscriptions[index];

            if (current.Dispose.HasStarted)
            {
                return;
            }

            parent._subscriptions[index] = new AsyncSubscription(current.Subscribe, OperationTime.StartingAt(parent._scheduler.Clock));

            await parent._scheduler.YieldPoint();

            state.Disposed = true;

            foreach (var schedule in state.Schedules)
            {
                schedule.Dispose();
            }

            current = parent._subscriptions[index];
            parent._subscriptions[index] = new AsyncSubscription(current.Subscribe, current.Dispose.CompletedAt(parent._scheduler.Clock));
        }
    }
}
