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
    /// <remarks>
    /// This handles all notifications instantaneously.
    /// </remarks>
    public ITestableAsyncObserver<T> CreateObserver<T>() => new MockAsyncObserver<T>(this);

    /// <summary>
    /// Creates a recording observer that records received notifications, passing each message
    /// to a handler.
    /// </summary>
    /// <remarks>
    /// If the handler chooses to prolong handling, this will be reflected with different start
    /// and end times in the <see cref="ITestableAsyncObservable{T}.Messages"/> entries.
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
