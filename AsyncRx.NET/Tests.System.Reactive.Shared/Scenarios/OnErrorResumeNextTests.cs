// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics;
using System.Reactive;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>OnErrorResumeNext</c> scenarios, from Rx.NET's <c>OnErrorResumeNextTest.cs</c>.
/// </summary>
/// <remarks>
/// <para>
/// 20 of the file's 21 tests (<c>OnErrorResumeNext_ArgumentChecking</c> is the code-generated
/// stratum), in the <c>Catch</c> scenarios' shapes, the file being <c>CatchTest.cs</c>'s
/// twin: the two-source, <c>params</c> and enumerable forms, an enumerable of sources as the
/// shared <see cref="MockEnumerable{T}"/> or <see cref="RogueEnumerable{T}"/> with the
/// original's <c>Subscriptions</c> assertions on it as <c>Enumerations</c>, and
/// <c>OnErrorResumeNext_TailRecursive1</c>'s computed expectation. The three
/// <c>*_DefaultScheduler_*</c> tests sum what <c>ToListAsync</c> collects where the originals
/// block on an event; <c>OnErrorResumeNext_TailRecursive2</c>, dead upstream under an
/// undefined <c>HAS_STACKTRACE</c>, collects with <c>ToListAsync</c> where the original uses
/// the blocking <c>ForEach</c> and keeps its check that the stack does not deepen with
/// recursion; <c>OnErrorResumeNext_TailRecursive3</c> takes the single element where the
/// original blocks on <c>Wait()</c>.
/// </para>
/// </remarks>
public abstract class OnErrorResumeNextTests : SharedReactiveTest
{
    [TestMethod]
    public void OnErrorResumeNext_IEofIO_GetEnumeratorThrows()
    {
        var ex = new Exception();
        var xss = new RogueEnumerable<Seq<int>>(ex);

        var res = Scheduler.Start(() =>
            Seq.OnErrorResumeNext(xss)
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex)
        );
    }

    [TestMethod]
    public void OnErrorResumeNext_IEofIO()
    {
        var xs1 = Scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnCompleted<int>(40)
        );

        var xs2 = Scheduler.CreateColdObservable(
            OnNext(10, 4),
            OnNext(20, 5),
            OnError<int>(30, new Exception())
        );

        var xs3 = Scheduler.CreateColdObservable(
            OnNext(10, 6),
            OnNext(20, 7),
            OnNext(30, 8),
            OnNext(40, 9),
            OnCompleted<int>(50)
        );

        var res = Scheduler.Start(() =>
            Seq.OnErrorResumeNext([xs1, xs2, xs3])
        );

        res.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(250, 4),
            OnNext(260, 5),
            OnNext(280, 6),
            OnNext(290, 7),
            OnNext(300, 8),
            OnNext(310, 9),
            OnCompleted<int>(320)
        );

        xs1.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );

        xs2.Subscriptions.AssertEqual(
            Subscribe(240, 270)
        );

        xs3.Subscriptions.AssertEqual(
            Subscribe(270, 320)
        );
    }

    [TestMethod]
    public void OnErrorResumeNext_NoErrors()
    {
        var o1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnCompleted<int>(230)
        );

        var o2 = Scheduler.CreateHotObservable(
            OnNext(240, 4),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            o1.OnErrorResumeNext(o2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(240, 4),
            OnCompleted<int>(250)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(230, 250)
        );
    }

    [TestMethod]
    public void OnErrorResumeNext_Error()
    {
        var o1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnError<int>(230, new Exception())
        );

        var o2 = Scheduler.CreateHotObservable(
            OnNext(240, 4),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            o1.OnErrorResumeNext(o2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(240, 4),
            OnCompleted<int>(250)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(230, 250)
        );
    }

    [TestMethod]
    public void OnErrorResumeNext_ErrorMultiple()
    {
        var o1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnError<int>(220, new Exception())
        );

        var o2 = Scheduler.CreateHotObservable(
            OnNext(230, 3),
            OnError<int>(240, new Exception())
        );

        var o3 = Scheduler.CreateHotObservable(
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            Seq.OnErrorResumeNext(o1, o2, o3)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(230, 3),
            OnCompleted<int>(250)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(220, 240)
        );

        o3.Subscriptions.AssertEqual(
            Subscribe(240, 250)
        );
    }

    [TestMethod]
    public void OnErrorResumeNext_EmptyReturnThrowAndMore()
    {
        var o1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(205)
        );

        var o2 = Scheduler.CreateHotObservable(
            OnNext(215, 2),
            OnCompleted<int>(220)
        );

        var o3 = Scheduler.CreateHotObservable(
            OnNext(225, 3),
            OnNext(230, 4),
            OnCompleted<int>(235)
        );

        var o4 = Scheduler.CreateHotObservable(
            OnError<int>(240, new Exception())
        );

        var o5 = Scheduler.CreateHotObservable(
            OnNext(245, 5),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            new[] { o1, o2, o3, o4, o5 }.OnErrorResumeNext()
        );

        res.Messages.AssertEqual(
            OnNext(215, 2),
            OnNext(225, 3),
            OnNext(230, 4),
            OnNext(245, 5),
            OnCompleted<int>(250)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 205)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(205, 220)
        );

        o3.Subscriptions.AssertEqual(
            Subscribe(220, 235)
        );

        o4.Subscriptions.AssertEqual(
            Subscribe(235, 240)
        );

        o5.Subscriptions.AssertEqual(
            Subscribe(240, 250)
        );
    }

    [TestMethod]
    public void OnErrorResumeNext_LastIsntSpecial()
    {
        var ex = new Exception();

        var o1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(220)
        );

        var o2 = Scheduler.CreateHotObservable(
            OnError<int>(230, ex)
        );

        var res = Scheduler.Start(() =>
            o1.OnErrorResumeNext(o2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnCompleted<int>(230)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(220, 230)
        );
    }

    [TestMethod]
    public void OnErrorResumeNext_SingleSourceDoesntThrow()
    {
        var ex = new Exception();

        var o1 = Scheduler.CreateHotObservable(
            OnError<int>(230, ex)
        );

        var res = Scheduler.Start(() =>
            Seq.OnErrorResumeNext(o1)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(230)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void OnErrorResumeNext_EndWithNever()
    {
        var ex = new Exception();

        var o1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(220)
        );

        var o2 = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = Scheduler.Start(() =>
            Seq.OnErrorResumeNext(o1, o2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(220, 1000)
        );
    }

    [TestMethod]
    public void OnErrorResumeNext_StartWithNever()
    {
        var ex = new Exception();

        var o1 = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var o2 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(220)
        );

        var res = Scheduler.Start(() =>
            Seq.OnErrorResumeNext(o1, o2)
        );

        res.Messages.AssertEqual(
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        o2.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public async Task OnErrorResumeNext_DefaultScheduler_Binary()
    {
        var res = await ToListAsync(Seq.Return(1).OnErrorResumeNext(Seq.Return(2)));

        Assert.AreEqual(3, res.Sum());
    }

    [TestMethod]
    public async Task OnErrorResumeNext_DefaultScheduler_Nary()
    {
        var res = await ToListAsync(
            Seq.OnErrorResumeNext(Seq.Return(1), Seq.Return(2), Seq.Return(3)));

        Assert.AreEqual(6, res.Sum());
    }

    [TestMethod]
    public async Task OnErrorResumeNext_DefaultScheduler_NaryEnumerable()
    {
        IEnumerable<Seq<int>> sources = [Seq.Return(1), Seq.Return(2), Seq.Return(3)];

        var res = await ToListAsync(Seq.OnErrorResumeNext(sources));

        Assert.AreEqual(6, res.Sum());
    }

    [TestMethod]
    public void OnErrorResumeNext_IteratorThrows()
    {
        var ex = new Exception();

        var res = Scheduler.Start(() =>
            Seq.OnErrorResumeNext(Catch_IteratorThrows_Source(ex, true))
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex)
        );
    }

    [TestMethod]
    public void OnErrorResumeNext_EnumerableThrows()
    {
        var o = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnError<int>(225, new Exception())
        );

        var ex = new Exception();
        var xss = new MockEnumerable<Seq<int>>(Scheduler, GetObservablesForOnErrorResumeNextThrow(o, ex));

        var res = Scheduler.Start(() =>
            xss.OnErrorResumeNext()
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnError<int>(225, ex)
        );

        o.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );

        xss.Enumerations.AssertEqual(
            new Enumeration(200, 225)
        );
    }

    [TestMethod]
    public void OnErrorResumeNext_EnumerableTiming()
    {
        var o1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2), // !
            OnNext(220, 3), // !
            OnCompleted<int>(230)
        );

        var o2 = Scheduler.CreateColdObservable(
            OnNext(50, 4),  // !
            OnNext(60, 5),  // !
            OnNext(70, 6),  // !
            OnError<int>(80, new Exception())
        );

        var o3 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(200, 2),
            OnNext(210, 3),
            OnNext(220, 4),
            OnNext(230, 5),
            OnNext(270, 6),
            OnNext(320, 7), // !
            OnNext(330, 8), // !
            OnCompleted<int>(340)
        );

        var xss = new MockEnumerable<TestableSeq<int>>(Scheduler, [o1, o2, o3, o2]);

        var res = Scheduler.Start(() =>
            xss.Select(xs => (Seq<int>)xs).OnErrorResumeNext()
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(280, 4),
            OnNext(290, 5),
            OnNext(300, 6),
            OnNext(320, 7),
            OnNext(330, 8),
            OnNext(390, 4),
            OnNext(400, 5),
            OnNext(410, 6),
            OnCompleted<int>(420)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(230, 310),
            Subscribe(340, 420)
        );

        o3.Subscriptions.AssertEqual(
            Subscribe(310, 340)
        );

        xss.Enumerations.AssertEqual(
            new Enumeration(200, 420)
        );
    }

    [TestMethod]
    public void OnErrorResumeNext_Enumerable_Dispose()
    {
        var o1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnError<int>(230, new Exception())
        );

        var o2 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(200, 2),
            OnNext(210, 3),
            OnNext(240, 4),
            OnNext(270, 5),
            OnNext(320, 6),
            OnNext(330, 7),
            OnCompleted<int>(340)
        );

        var xss = new MockEnumerable<TestableSeq<int>>(Scheduler, [o1, o2]);

        var res = Scheduler.Start(() =>
            xss.Select(xs => (Seq<int>)xs).OnErrorResumeNext(),
            300
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(240, 4),
            OnNext(270, 5)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(230, 300)
        );

        xss.Enumerations.AssertEqual(
            new Enumeration(200, 300)
        );
    }

    [TestMethod]
    public void OnErrorResumeNext_TailRecursive1()
    {
        var create = 0L;
        var start = 200L;
        var end = 1000L;
        var o = Scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnError<int>(40, new Exception())
        );

        Seq<int> f() => Seq.Defer(() => o.OnErrorResumeNext(f()));
        var res = Scheduler.Start(() => f(), create, start, end);

        var expected = new List<Recorded<Notification<int>>>();

        var t = start;
        while (t <= end)
        {
            var n = (t - start) / 10;
            if (n % 4 != 0)
            {
                expected.Add(OnNext(t, (int)(n % 4)));
            }

            t += 10;
        }

        res.Messages.AssertEqual(expected);
    }

    [TestMethod]
    public async Task OnErrorResumeNext_TailRecursive2()
    {
        Func<int, Seq<int>> f = null!;
        f = x => Seq.Defer(() =>
            Seq.Throw<int>(new Exception(), ThreadPoolScheduler).StartWith(x).OnErrorResumeNext(f(x + 1)));

        var lst = await ToListAsync(f(0).Select(x => new StackTrace().FrameCount).Take(10));

        Assert.IsTrue(lst.Last() - lst.First() < 10);
    }

    [TestMethod]
    public async Task OnErrorResumeNext_TailRecursive3()
    {
        var ex = new Exception();

        var res =
            Seq.OnErrorResumeNext(
                Seq.Return(1),
                Seq.Defer(() =>
                {
                    if (ex != null)
                    {
                        throw ex;
                    }

                    return Seq.Return(-2);
                }),
                Seq.Defer(() =>
                {
                    if (ex != null)
                    {
                        throw ex;
                    }

                    return Seq.Return(-1);
                }),
                Seq.Return(2)
            )
            .SequenceEqual([1, 2]);

        Assert.IsTrue((await ToListAsync(res)).Single());
    }

    private static IEnumerable<Seq<int>> GetObservablesForOnErrorResumeNextThrow(
        Seq<int> first,
        Exception ex)
    {
        yield return first;
        throw ex;
    }

    private static IEnumerable<Seq<int>> Catch_IteratorThrows_Source(Exception ex, bool b)
    {
        if (b)
        {
            throw ex;
        }
        else
        {
            yield break;
        }
    }
}
