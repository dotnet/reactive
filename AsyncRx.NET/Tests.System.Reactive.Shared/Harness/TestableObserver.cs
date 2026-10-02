// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The recording observer a scenario ends in: what
/// <see cref="TestSchedulerRef.Start{T}(Func{Seq{T}})"/> returns, or what
/// <see cref="TestSchedulerRef.CreateObserver{T}"/> creates for the raw surface.
/// </summary>
/// <remarks>
/// Nothing in the shared library records anything itself. <see cref="Native"/> is the target's
/// own testable observer, which holds the recorded messages in the target's own record type: on
/// Rx.NET an <c>ITestableObserver&lt;T&gt;</c> whose <c>Messages</c> are
/// <c>Recorded&lt;Notification&lt;T&gt;&gt;</c>; on AsyncRx.NET an
/// <c>ITestableAsyncObserver&lt;T&gt;</c> whose <c>Messages</c> are
/// <c>AsyncRecorded&lt;Notification&lt;T&gt;&gt;</c>, each carrying a delivery start and end tick.
/// <see cref="Messages"/> is therefore not that collection but a handle for asserting over it:
/// the target does the comparison, so that its own diagnostics are the ones a failure reports.
/// </remarks>
public sealed class TestableObserver<T>(IRxTarget target, object native, string query)
{
    /// <summary>The target that created this observer.</summary>
    public IRxTarget Target => target;

    /// <summary>The target's own testable observer (see the remarks on this type).</summary>
    public object Native => native;

    /// <summary>
    /// The query this observer was started over (its description), for diagnostics.
    /// </summary>
    /// <remarks>Empty for a raw-surface observer.</remarks>
    public string Query => query;

    /// <summary>
    /// The recorded messages, as something to assert over in the shared vocabulary.
    /// </summary>
    public MessageLog<T> Messages => new(this);

    /// <summary>Records <paramref name="value"/> as if the sequence had produced it.</summary>
    /// <param name="value">The value to record.</param>
    /// <remarks>
    /// For scenarios that feed an observer by hand (an outer observer told each group's key, say)
    /// or pass its handlers to a subscription. Async-shaped like the raw surface.
    /// </remarks>
    public ValueTask OnNextAsync(T value) => target.OnNextAsync(this, value);

    /// <summary>Records <paramref name="error"/> as if the sequence had failed with it.</summary>
    /// <param name="error">The error to record.</param>
    public ValueTask OnErrorAsync(Exception error) => target.OnErrorAsync(this, error);

    /// <summary>Records completion.</summary>
    public ValueTask OnCompletedAsync() => target.OnCompletedAsync(this);
}
