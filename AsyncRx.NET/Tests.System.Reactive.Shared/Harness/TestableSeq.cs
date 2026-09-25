// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared;

/// <summary>A testable source (hot or cold) the target created: a leaf that also records its subscriptions and knows the messages it plays.</summary>
public sealed class TestableSeq<T>(IRxTarget target, object native, IReadOnlyList<Recorded<Notification<T>>> messages, string description) : NativeSeq<T>(native, description)
{
    public SubscriptionLog<T> Subscriptions => new(target, Native, ToString());

    public IReadOnlyList<Recorded<Notification<T>>> Messages => messages;
}
