// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Microsoft.Reactive.Testing.Async;

internal sealed class MockAsyncObserver<T>(TestAsyncScheduler scheduler, Func<Notification<T>, ValueTask>? onNotification = null) : ITestableAsyncObserver<T>
{
    private readonly List<AsyncRecorded<Notification<T>>> _messages = [];

    public IReadOnlyList<AsyncRecorded<Notification<T>>> Messages => _messages;

    public ValueTask OnNextAsync(T value) => RecordAsync(Notification.CreateOnNext(value));

    public ValueTask OnErrorAsync(Exception error) => RecordAsync(Notification.CreateOnError<T>(error));

    public ValueTask OnCompletedAsync() => RecordAsync(Notification.CreateOnCompleted<T>());

    private async ValueTask RecordAsync(Notification<T> notification)
    {
        // Delivery start is recorded on entry and completion when the returned task
        // completes; a record left Incomplete after the pump finishes diagnoses delivery
        // that was cut off in flight.
        scheduler.EnsurePumpThread($"delivery of {notification} to a recording observer");

        var index = _messages.Count;
        _messages.Add(new AsyncRecorded<Notification<T>>(OperationTime.StartingAt(scheduler.Clock), notification));

        await scheduler.YieldPoint();

        if (onNotification is not null)
        {
            // The handler's work is part of the delivery: a handler that awaits something
            // logically in the future makes the completion prolonged (end tick > start tick).
            await onNotification(notification);
        }

        _messages[index] = new AsyncRecorded<Notification<T>>(_messages[index].Time.CompletedAt(scheduler.Clock), notification);
    }
}
