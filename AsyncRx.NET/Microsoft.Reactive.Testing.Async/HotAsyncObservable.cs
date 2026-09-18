// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Microsoft.Reactive.Testing.Async;

internal sealed class HotAsyncObservable<T> : ITestableAsyncObservable<T>
{
    private readonly TestAsyncScheduler _scheduler;
    private readonly List<IAsyncObserver<T>> _observers = [];
    private readonly List<AsyncSubscription> _subscriptions = [];
    private readonly Recorded<Notification<T>>[] _messages;
    private Task _deliveryTail = Task.CompletedTask;

    public HotAsyncObservable(TestAsyncScheduler scheduler, Recorded<Notification<T>>[] messages)
    {
        _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        _messages = messages ?? throw new ArgumentNullException(nameof(messages));

        foreach (var message in messages)
        {
            var notification = message.Value;

            scheduler.ScheduleAbsolute(
                message.Time,
                _ =>
                {
                    var delivery = DeliverAsync(_deliveryTail, notification);
                    _deliveryTail = delivery;
                    return new ValueTask(delivery);
                },
                $"hot observable delivery of {notification} scheduled at {message.Time}");
        }
    }

    public IReadOnlyList<AsyncSubscription> Subscriptions => _subscriptions;

    public IReadOnlyList<Recorded<Notification<T>>> Messages => _messages;

    private async Task DeliverAsync(Task previous, Notification<T> notification)
    {
        // Awaited delivery means a hot source must not begin a notification while a previous
        // one is still in flight (overlapping observer calls would violate the observer
        // grammar), so each delivery starts at max(scheduled tick, completion of the
        // previous). Any failure in the previous delivery is reported by its own scheduled
        // work item; here it only gates the start time.
        try
        {
            await previous;
        }
        catch
        {
        }

        await _scheduler.YieldPoint();

        foreach (var observer in _observers.ToArray())
        {
            // An observer disposed while this delivery was pending no longer receives it.
            if (_observers.Contains(observer))
            {
                await notification.AcceptAsync(observer);
            }
        }
    }

    public async ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer)
    {
        if (observer == null)
        {
            throw new ArgumentNullException(nameof(observer));
        }

        var index = _subscriptions.Count;
        _subscriptions.Add(new AsyncSubscription(OperationTime.StartingAt(_scheduler.Clock), OperationTime.Never));

        await _scheduler.YieldPoint();

        _observers.Add(observer);
        _subscriptions[index] = new AsyncSubscription(_subscriptions[index].Subscribe.CompletedAt(_scheduler.Clock), OperationTime.Never);

        return new SubscriptionDisposable(this, observer, index);
    }

    private sealed class SubscriptionDisposable(HotAsyncObservable<T> parent, IAsyncObserver<T> observer, int index) : IAsyncDisposable
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

            parent._observers.Remove(observer);

            current = parent._subscriptions[index];
            parent._subscriptions[index] = new AsyncSubscription(current.Subscribe, current.Dispose.CompletedAt(parent._scheduler.Clock));
        }
    }
}
