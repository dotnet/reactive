// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>ToEvent</c> scenarios, from Rx.NET's <c>ToEventTest.cs</c>.</summary>
/// <remarks>
/// 3 of the file's 4 tests (<c>ToEvent_ArgumentChecks</c> is the code-generated stratum). The
/// event source comes from <see cref="SharedReactiveTest.ToEvent{T}"/>, the target's own
/// operator over a subject the scenario drives; <c>ToEvent_FromEvent</c> subscribes through the
/// kit's handler form with an error handler the original leaves out, failing as its completion
/// handler does.
/// </remarks>
public abstract class ToEventTests : SharedReactiveTest
{
    [TestMethod]
    public async Task ToEvent_Unit()
    {
        var src = CreateSubject<Unit>();
        var evt = ToEvent(src);

        var num = 0;
        var hnd = new Action<Unit>(_ =>
        {
            num++;
        });

        evt.OnNext += hnd;

        Assert.AreEqual(0, num);

        await src.OnNextAsync(new Unit());
        Assert.AreEqual(1, num);

        await src.OnNextAsync(new Unit());
        Assert.AreEqual(2, num);

        evt.OnNext -= hnd;

        await src.OnNextAsync(new Unit());
        Assert.AreEqual(2, num);
    }

    [TestMethod]
    public async Task ToEvent_NonUnit()
    {
        var src = CreateSubject<int>();
        var evt = ToEvent(src);

        var lst = new List<int>();
        var hnd = new Action<int>(e =>
        {
            lst.Add(e);
        });

        evt.OnNext += hnd;

        await src.OnNextAsync(1);
        await src.OnNextAsync(2);

        evt.OnNext -= hnd;

        await src.OnNextAsync(3);

        Assert.IsTrue(lst.SequenceEqual([1, 2]));
    }

    [TestMethod]
    public async Task ToEvent_FromEvent()
    {
        var src = CreateSubject<int>();
        var evt = ToEvent(src);

        var res = Seq.FromEvent<int>(h => evt.OnNext += h, h => evt.OnNext -= h);

        var lst = new List<int>();
        await using (await res.SubscribeAsync(
            Scheduler, e => lst.Add(e), ex => Assert.Fail(), () => Assert.Fail()))
        {
            await src.OnNextAsync(1);
            await src.OnNextAsync(2);
        }

        await src.OnNextAsync(3);

        Assert.IsTrue(lst.SequenceEqual([1, 2]));
    }
}
