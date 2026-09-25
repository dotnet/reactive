// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The subscriptions a <see cref="TestableSeq{T}"/> recorded, as something a shared scenario can
/// assert over in the sync vocabulary: <c>xs.Subscriptions.AssertEqual(Subscribe(200, 300))</c>.
/// </summary>
/// <remarks>
/// A handle, not a collection, for the same reason as <see cref="MessageLog{T}"/>: the records
/// live in the source's <see cref="NativeSeq{T}.Native"/> (the target's own testable observable)
/// in the target's own record type — on AsyncRx.NET a subscription has four timestamps — and
/// <see cref="IRxTarget.AssertSubscriptions{T}"/> compares them. A failure is prefixed with the source's description.
/// </remarks>
public readonly struct SubscriptionLog<T>(TestableSeq<T> source)
{
    public void AssertEqual(params Subscription[] expected)
    {
        try
        {
            source.Target.AssertSubscriptions(source, expected);
        }
        catch (Exception ex)
        {
            throw new AssertFailedException($"Subscriptions of {source}: {ex.Message}", ex);
        }
    }
}
