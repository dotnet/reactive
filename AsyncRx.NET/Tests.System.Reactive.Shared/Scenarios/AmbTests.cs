// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Amb</c> scenarios, from Rx.NET's <c>AmbTest.cs</c>.</summary>
/// <remarks>
/// 18 of the file's 19 tests (<c>Amb_ArgumentChecking</c> is the code-generated stratum):
/// the sixteen virtual-time tests over the two-source, <c>params</c> and enumerable forms
/// transcribe as they are; the two <c>*_Many_Sources*</c> tests, which race up to a hundred
/// sources in real time, collect with <c>ToListAsync</c> where the originals block on
/// <c>ToList().First()</c>, and count with <c>Assert.HasCount</c>.
/// </remarks>
public abstract class AmbTests : SharedReactiveTest
{
    [TestMethod]
    public void Amb_Never2()
    {
        var l = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var r = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = Scheduler.Start(() =>
            l.Amb(r)
        );

        res.Messages.AssertEqual(
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Amb_Never3()
    {
        var n1 = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var n2 = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var n3 = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = Scheduler.Start(() =>
            new[] { n1, n2, n3 }.Amb()
        );

        res.Messages.AssertEqual(
        );

        n1.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        n2.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        n3.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Amb_Never3_Params()
    {
        var n1 = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var n2 = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var n3 = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = Scheduler.Start(() =>
            Seq.Amb(n1, n2, n3)
        );

        res.Messages.AssertEqual(
        );

        n1.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        n2.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        n3.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Amb_NeverEmpty()
    {
        var n = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var e = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(225)
        );

        var res = Scheduler.Start(() =>
            n.Amb(e)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(225)
        );

        n.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );

        e.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void Amb_EmptyNever()
    {
        var n = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var e = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(225)
        );

        var res = Scheduler.Start(() =>
            e.Amb(n)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(225)
        );

        n.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );

        e.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void Amb_RegularShouldDisposeLoser()
    {
        var o1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(240)
        );

        var o2 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 3),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            o1.Amb(o2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnCompleted<int>(240)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Amb_WinnerThrows()
    {
        var ex = new Exception();

        var o1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnError<int>(220, ex)
        );

        var o2 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 3),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            o1.Amb(o2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnError<int>(220, ex)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Amb_LoserThrows()
    {
        var ex = new Exception();

        var o1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnError<int>(230, ex)
        );

        var o2 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 3),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            o1.Amb(o2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 3),
            OnCompleted<int>(250)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Amb_ThrowsBeforeElectionLeft()
    {
        var ex = new Exception();

        var o1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex)
        );

        var o2 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 3),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            o1.Amb(o2)
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Amb_ThrowsBeforeElectionRight()
    {
        var ex = new Exception();

        var o1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 3),
            OnCompleted<int>(250)
        );

        var o2 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex)
        );

        var res = Scheduler.Start(() =>
            o1.Amb(o2)
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Amb_Many_Array_OnNext()
    {
        var ex = new Exception();

        var o1 = Scheduler.CreateColdObservable(
            OnNext(150, 1),
            OnNext(220, 3),
            OnCompleted<int>(250)
        );

        var o2 = Scheduler.CreateColdObservable(
            OnNext(150, 2),
            OnError<int>(210, ex)
        );

        var o3 = Scheduler.CreateColdObservable(
            OnCompleted<int>(150)
        );

        var res = Scheduler.Start(() =>
            Seq.Amb(o1, o2, o3)
        );

        res.Messages.AssertEqual(
            OnNext(350, 1),
            OnNext(420, 3),
            OnCompleted<int>(450)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 450)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );

        o3.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );
    }

    [TestMethod]
    public void Amb_Many_Array_OnError()
    {
        var ex = new Exception();

        var o1 = Scheduler.CreateColdObservable(
            OnError<int>(150, ex)
        );

        var o2 = Scheduler.CreateColdObservable(
            OnNext(150, 1),
            OnNext(220, 3),
            OnCompleted<int>(250)
        );

        var o3 = Scheduler.CreateColdObservable(
            OnCompleted<int>(150)
        );

        var res = Scheduler.Start(() =>
            Seq.Amb(o1, o2, o3)
        );

        res.Messages.AssertEqual(
            OnError<int>(350, ex)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );

        o3.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );
    }

    [TestMethod]
    public void Amb_Many_Array_OnCompleted()
    {
        var ex = new Exception();

        var o1 = Scheduler.CreateColdObservable(
            OnCompleted<int>(150)
        );

        var o2 = Scheduler.CreateColdObservable(
            OnNext(150, 1),
            OnNext(220, 3),
            OnCompleted<int>(250)
        );

        var o3 = Scheduler.CreateColdObservable(
            OnNext(150, 2),
            OnError<int>(210, ex)
        );


        var res = Scheduler.Start(() =>
            Seq.Amb(o1, o2, o3)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(350)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );

        o3.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );
    }

    [TestMethod]
    public void Amb_Many_Enumerable_OnNext()
    {
        var ex = new Exception();

        var o1 = Scheduler.CreateColdObservable(
            OnNext(150, 1),
            OnNext(220, 3),
            OnCompleted<int>(250)
        );

        var o2 = Scheduler.CreateColdObservable(
            OnNext(150, 2),
            OnError<int>(210, ex)
        );

        var o3 = Scheduler.CreateColdObservable(
            OnCompleted<int>(150)
        );

        var res = Scheduler.Start(() =>
            new[] { o1, o2, o3 }.Amb()
        );

        res.Messages.AssertEqual(
            OnNext(350, 1),
            OnNext(420, 3),
            OnCompleted<int>(450)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 450)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );

        o3.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );
    }

    [TestMethod]
    public void Amb_Many_Enumerable_OnError()
    {
        var ex = new Exception();

        var o1 = Scheduler.CreateColdObservable(
            OnError<int>(150, ex)
        );

        var o2 = Scheduler.CreateColdObservable(
            OnNext(150, 1),
            OnNext(220, 3),
            OnCompleted<int>(250)
        );

        var o3 = Scheduler.CreateColdObservable(
            OnCompleted<int>(150)
        );

        var res = Scheduler.Start(() =>
            new[] { o1, o2, o3 }.Amb()
        );

        res.Messages.AssertEqual(
            OnError<int>(350, ex)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );

        o3.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );
    }

    [TestMethod]
    public void Amb_Many_Enumerable_OnCompleted()
    {
        var ex = new Exception();

        var o1 = Scheduler.CreateColdObservable(
            OnCompleted<int>(150)
        );

        var o2 = Scheduler.CreateColdObservable(
            OnNext(150, 1),
            OnNext(220, 3),
            OnCompleted<int>(250)
        );

        var o3 = Scheduler.CreateColdObservable(
            OnNext(150, 2),
            OnError<int>(210, ex)
        );


        var res = Scheduler.Start(() =>
            new[] { o1, o2, o3 }.Amb()
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(350)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );

        o3.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );
    }

    [TestMethod]
    public async Task Amb_Many_Enumerable_Many_Sources()
    {
        for (var i = 0; i < 32; i++)
        {
            var sources = new List<Seq<int>>();
            for (var j = 0; j < i; j++)
            {
                sources.Add(Seq.Return(j));
            }

            var result = await ToListAsync(sources.Amb());

            if (i == 0)
            {
                Assert.HasCount(0, result);
            }
            else
            {
                Assert.HasCount(1, result);
                Assert.AreEqual(0, result[0]);
            }
        }
    }

    [TestMethod]
    public async Task Amb_Many_Enumerable_Many_Sources_NoStackOverflow()
    {
        for (var i = 0; i < 100; i++)
        {
            var sources = new List<Seq<int>>();
            for (var j = 0; j < i; j++)
            {
                if (j == i - 1)
                {
                    sources.Add(Seq.Return(j));
                }
                else
                {
                    sources.Add(Seq.Never<int>());
                }
            }

            var result = await ToListAsync(sources.Amb());

            if (i == 0)
            {
                Assert.HasCount(0, result);
            }
            else
            {
                Assert.HasCount(1, result);
                Assert.AreEqual(i - 1, result[0]);
            }
        }
    }
}
