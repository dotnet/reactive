// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// A testable source (hot or cold) the target created through <see cref="TestScheduler.CreateHotObservable{T}"/>
/// or <see cref="TestScheduler.CreateColdObservable{T}"/>: a leaf of a description that also
/// records its subscriptions and knows the messages it plays.
/// </summary>
/// <remarks>
/// <see cref="NativeSeq{T}.Native"/> is the target's own testable observable: on Rx.NET an
/// <c>ITestableObservable&lt;T&gt;</c>, on AsyncRx.NET an <c>ITestableAsyncObservable&lt;T&gt;</c>.
/// <see cref="Subscriptions"/> is a handle for asserting over the subscriptions it recorded, not the collection itself.
/// </remarks>
public sealed class TestableSeq<T>(IRxTarget target, object native, IReadOnlyList<Recorded<Notification<T>>> messages, string description) : NativeSeq<T>(native, description)
{
    public IRxTarget Target => target;

    /// <summary>The recorded subscriptions, as something to assert over in the shared vocabulary.</summary>
    public SubscriptionLog<T> Subscriptions => new(this);

    /// <summary>The messages this source was created to play (the scenario's own data, in the shared vocabulary).</summary>
    public IReadOnlyList<Recorded<Notification<T>>> Messages => messages;
}
