// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The observer a scenario's callback drives, as <c>Create</c> hands it one.</summary>
/// <param name="onNext">Pushes a value to the target's observer.</param>
/// <param name="onError">Pushes an error to the target's observer.</param>
/// <param name="onCompleted">Completes the target's observer.</param>
/// <remarks>
/// Where the Rx.NET test's callback receives an <c>IObserver&lt;T&gt;</c> and calls
/// <c>OnNext</c>, the shared callback receives one of these and awaits <see cref="OnNextAsync"/>,
/// so that the same text drives AsyncRx.NET's observer, whose methods are awaited; on Rx.NET
/// each call completes synchronously. The <see cref="DescriptionBridge"/> wraps the target's
/// real observer in one of these when it adapts the callback.
/// </remarks>
public sealed class ObserverRef<T>(
    Func<T, ValueTask> onNext,
    Func<Exception, ValueTask> onError,
    Func<ValueTask> onCompleted)
{
    /// <summary>Pushes <paramref name="value"/> to the observer.</summary>
    /// <param name="value">The value to push.</param>
    public ValueTask OnNextAsync(T value) => onNext(value);

    /// <summary>Pushes <paramref name="error"/> to the observer.</summary>
    /// <param name="error">The error to push.</param>
    public ValueTask OnErrorAsync(Exception error) => onError(error);

    /// <summary>Completes the observer.</summary>
    public ValueTask OnCompletedAsync() => onCompleted();
}
