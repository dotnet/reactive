// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Reactive.Subjects;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>The shared <c>Take</c> scenarios on Rx.NET, plus its reentrancy regression.</summary>
/// <remarks>
/// <c>Take_DecrementsCountFirst</c> drives a <c>BehaviorSubject</c> back into itself from the
/// handler of <c>Take(1)</c>; if <c>Take</c> forwarded the element before decrementing its count,
/// the re-entrant element would be forwarded too, and so on without end. The original has no
/// assertion and relies on the resulting stack overflow to signal failure; this one counts the
/// handler's calls and fails on a second. AsyncRx.NET's observers reject a re-entrant
/// notification by design, so the shape has no async counterpart.
/// </remarks>
[TestClass]
public sealed class RxTakeTests : TakeTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void Take_DecrementsCountFirst()
    {
        var k = new BehaviorSubject<bool>(true);

        var calls = 0;
        k.Take(1).Subscribe(b =>
        {
            if (++calls > 1)
            {
                throw new InvalidOperationException("Take forwarded a re-entrant element.");
            }

            k.OnNext(!b);
        });

        Assert.AreEqual(1, calls);
    }
}
