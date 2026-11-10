// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>ToDictionary</c> scenarios, from Rx.NET's <c>ToDictionaryTest.cs</c>.
/// </summary>
/// <remarks>
/// 7 of the file's 8 tests (<c>ToDictionary_ArgumentChecking</c> is the code-generated
/// stratum), all transcribed as they are: completion, failure, a key selector and an element
/// selector that throw, disposal before completion, a duplicate key, and the key-only forms
/// over <c>Range</c>; where the original blocks on <c>First()</c> the shared text reads the
/// one element of what <c>ToListAsync</c> collects.
/// </remarks>
public abstract class ToDictionaryTests : SharedReactiveTest
{
    [TestMethod]
    public void ToDictionary_Completed()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(220, 2),
            OnNext(330, 3),
            OnNext(440, 4),
            OnNext(550, 5),
            OnCompleted<int>(660)
        );

        var res = Scheduler.Start(() =>
            xs.ToDictionary(x => x * 2, x => x * 4, EqualityComparer<int>.Default)
        );

        res.Messages.AssertEqual(
            OnNext<IDictionary<int, int>>(660, d =>
            {
                return d.Count == 4
                    && d[4] == 8
                    && d[6] == 12
                    && d[8] == 16
                    && d[10] == 20;
            }),
            OnCompleted<IDictionary<int, int>>(660)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 660)
        );
    }

    [TestMethod]
    public void ToDictionary_Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(220, 2),
            OnNext(330, 3),
            OnNext(440, 4),
            OnNext(550, 5),
            OnError<int>(660, ex)
        );

        var res = Scheduler.Start(() =>
            xs.ToDictionary(x => x * 2, x => x * 4, EqualityComparer<int>.Default)
        );

        res.Messages.AssertEqual(
            OnError<IDictionary<int, int>>(660, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 660)
        );
    }

    [TestMethod]
    public void ToDictionary_KeySelectorThrows()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(220, 2),
            OnNext(330, 3),
            OnNext(440, 4),
            OnNext(550, 5),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.ToDictionary(x => { if (x < 4) { return x * 2; } throw ex; }, x => x * 4, EqualityComparer<int>.Default)
        );

        res.Messages.AssertEqual(
            OnError<IDictionary<int, int>>(440, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 440)
        );
    }

    [TestMethod]
    public void ToDictionary_ElementSelectorThrows()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(220, 2),
            OnNext(330, 3),
            OnNext(440, 4),
            OnNext(550, 5),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.ToDictionary(x => x * 2, x => { if (x < 4) { return x * 4; } throw ex; }, EqualityComparer<int>.Default)
        );

        res.Messages.AssertEqual(
            OnError<IDictionary<int, int>>(440, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 440)
        );
    }

    [TestMethod]
    public void ToDictionary_Disposed()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(220, 2),
            OnNext(330, 3),
            OnNext(440, 4),
            OnNext(550, 5)
        );

        var res = Scheduler.Start(() =>
            xs.ToDictionary(x => x * 2, x => x * 4, EqualityComparer<int>.Default)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void ToDictionary_MultipleAdd()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(220, 2),
            OnNext(330, 3),
            OnNext(440, 4),
            OnNext(550, 5),
            OnCompleted<int>(660)
        );

        var res = Scheduler.Start(() =>
            xs.ToDictionary(x => x % 2, x => x * 2, EqualityComparer<int>.Default)
        );

        res.Messages.AssertEqual(
            OnError<IDictionary<int, int>>(440, e => true)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 440)
        );
    }

    [TestMethod]
    public async Task ToDictionary_Default()
    {
        var d1 = (await ToListAsync(Seq.Range(1, 10).ToDictionary(x => x.ToString()))).Single();
        Assert.AreEqual(7, d1["7"]);

        var d2 = (await ToListAsync(
            Seq.Range(1, 10).ToDictionary(x => x.ToString(), x => x * 2)))
            .Single();
        Assert.AreEqual(18, d2["9"]);

        var d3 = (await ToListAsync(
            Seq.Range(1, 10).ToDictionary(x => x.ToString(), EqualityComparer<string>.Default)))
            .Single();
        Assert.AreEqual(7, d3["7"]);
    }
}
