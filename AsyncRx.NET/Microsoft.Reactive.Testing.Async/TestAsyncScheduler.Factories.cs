// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Microsoft.Reactive.Testing.Async;

public sealed partial class TestAsyncScheduler
{
    /// <summary>
    /// Creates a hot async observable playing back the given messages at their absolute
    /// virtual times, recording four-timestamp subscriptions and serializing deliveries
    /// (a delivery starts at max(scheduled tick, completion of the previous delivery)).
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
    /// Creates a cold async observable playing back the given messages at virtual times
    /// relative to each subscription, recording four-timestamp subscriptions.
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
    /// Creates an observer that records received notifications with delivery start and
    /// completion virtual times.
    /// </summary>
    public ITestableAsyncObserver<T> CreateObserver<T>() => new MockAsyncObserver<T>(this);

    /// <summary>
    /// Creates a recording observer whose handler participates in delivery completion: each
    /// delivery is not complete until the handler's returned task is. A handler that awaits
    /// something logically in the future (e.g. a virtual-time delay) produces prolonged
    /// completions — records whose completion tick is later than their start tick,
    /// assertable with the extended expectation forms.
    /// </summary>
    public ITestableAsyncObserver<T> CreateObserver<T>(Func<Notification<T>, ValueTask> onNotification)
    {
        if (onNotification == null)
        {
            throw new ArgumentNullException(nameof(onNotification));
        }

        return new MockAsyncObserver<T>(this, onNotification);
    }
}
