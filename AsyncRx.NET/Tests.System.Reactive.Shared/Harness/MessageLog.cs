// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The messages a <see cref="TestableObserver{T}"/> recorded, as something a shared scenario can
/// assert over in the sync vocabulary: <c>res.Messages.AssertEqual(OnNext(210, 1), ...)</c>.
/// </summary>
/// <remarks>
/// This is a handle, not a collection. The records live in the observer's <see cref="TestableObserver{T}.Native"/>,
/// in the target's own record type, and <see cref="AssertEqual(Recorded{Notification{T}}[])"/> hands the
/// observer and the expectations to <see cref="IRxTarget.AssertMessages{T}"/> for the target to compare.
/// That is what lets the async target check a compact <c>OnNext(210, 1)</c> against a record that has
/// both a delivery start and end tick, and name the timestamp that mismatched. A failure is prefixed
/// with the query the observer was started over.
/// </remarks>
public readonly struct MessageLog<T>(TestableObserver<T> observer)
{
    public void AssertEqual(params Recorded<Notification<T>>[] expected)
    {
        try
        {
            observer.Target.AssertMessages(observer, expected);
        }
        catch (Exception ex) when (observer.Query != "")
        {
            throw new AssertFailedException($"Messages of {observer.Query}: {ex.Message}", ex);
        }
    }

    public void AssertEqual(IEnumerable<Recorded<Notification<T>>> expected) => AssertEqual(expected.ToArray());
}
