// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Microsoft.Reactive.Testing.Async;

public sealed partial class TestAsyncScheduler
{
    /// <summary>
    /// Creates an async observable that delivers notifications at their specified absolute
    /// virtual times, and which records when subscriptions occur.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="messages">Notifications to surface through the created sequence at their specified absolute virtual times.</param>
    /// <returns>Hot observable sequence that can be used to assert the timing of subscriptions and notifications.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="messages"/> is null.</exception>
    public ITestableAsyncObservable<T> CreateHotObservable<T>(params Recorded<Notification<T>>[] messages)
    {
        if (messages == null)
        {
            throw new ArgumentNullException(nameof(messages));
        }

        return new HotAsyncObservable<T>(this, messages);
    }

    /// <summary>
    /// Creates an async observable that delivers notifications timed relative to each
    /// subscription's start time, and which records when subscriptions occur.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="messages">Notifications to surface through the created sequence at their specified virtual time offsets from the subscription time.</param>
    /// <returns>Cold observable sequence that can be used to assert the timing of subscriptions and notifications.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="messages"/> is null.</exception>
    public ITestableAsyncObservable<T> CreateColdObservable<T>(params Recorded<Notification<T>>[] messages)
    {
        if (messages == null)
        {
            throw new ArgumentNullException(nameof(messages));
        }

        return new ColdAsyncObservable<T>(this, messages);
    }

    /// <summary>
    /// Creates an observer that records received notifications.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <returns>Observer that can be used to assert the timing of received notifications.</returns>
    /// <remarks>
    /// This handles all notifications instantaneously.
    /// </remarks>
    public ITestableAsyncObserver<T> CreateObserver<T>() => new MockAsyncObserver<T>(this);

    /// <summary>
    /// Creates a recording observer that records received notifications, passing each message
    /// to a handler.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="onNotification">
    /// Handler invoked for each notification after it has been recorded. The delivery of the
    /// notification is not considered complete until the task returned by this handler completes.
    /// </param>
    /// <returns>Observer that can be used to assert the timing of received notifications.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onNotification"/> is null.</exception>
    /// <remarks>
    /// If the handler chooses to prolong handling, this will be reflected with different start
    /// and end times in the <see cref="ITestableAsyncObserver{T}.Messages"/> entries.
    /// </remarks>
    public ITestableAsyncObserver<T> CreateObserver<T>(Func<Notification<T>, ValueTask> onNotification)
    {
        if (onNotification == null)
        {
            throw new ArgumentNullException(nameof(onNotification));
        }

        return new MockAsyncObserver<T>(this, onNotification);
    }
}
