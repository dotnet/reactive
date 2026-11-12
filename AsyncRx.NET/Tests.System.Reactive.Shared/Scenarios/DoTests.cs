// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Do</c> scenarios, from Rx.NET's <c>DoTest.cs</c>.</summary>
/// <remarks>
/// 22 of the file's 23 tests (the <c>Do_ArgumentChecking</c> test is the code-generated
/// stratum). All transcribe as they are, every one in virtual time: the handler forms, with
/// handlers that count, sum and flag; the observer form, with <c>ObserverRef.Create</c> where
/// the original writes <c>Observer.Create</c>, since <see cref="ObserverRef{T}"/> is the model's
/// observer as <see cref="Seq{T}"/> is its sequence; and the <c>Do1422_*</c> tests, where a
/// handler throws and the exception ends the result at that tick.
/// </remarks>
public abstract class DoTests : SharedReactiveTest
{
    [TestMethod]
    public void Do_ShouldSeeAllValues()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var i = 0;
        var sum = 2 + 3 + 4 + 5;
        var res = scheduler.Start(() =>
            xs.Do(x => { i++; sum -= x; })
        );

        Assert.AreEqual(4, i);
        Assert.AreEqual(0, sum);

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Do_PlainAction()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var i = 0;
        var res = scheduler.Start(() =>
            xs.Do(_ => { i++; })
        );

        Assert.AreEqual(4, i);

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Do_NextCompleted()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var i = 0;
        var sum = 2 + 3 + 4 + 5;
        var completed = false;
        var res = scheduler.Start(() =>
            xs.Do(x => { i++; sum -= x; }, () => { completed = true; })
        );

        Assert.AreEqual(4, i);
        Assert.AreEqual(0, sum);
        Assert.IsTrue(completed);

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Do_NextCompleted_Never()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable<int>();

        var i = 0;
        var completed = false;
        var res = scheduler.Start(() =>
            xs.Do(x => { i++; }, () => { completed = true; })
        );

        Assert.AreEqual(0, i);
        Assert.IsFalse(completed);

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Do_NextError()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnError<int>(250, ex)
        );

        var i = 0;
        var sum = 2 + 3 + 4 + 5;
        var sawError = false;
        var res = scheduler.Start(() =>
            xs.Do(x => { i++; sum -= x; }, e => { sawError = e == ex; })
        );

        Assert.AreEqual(4, i);
        Assert.AreEqual(0, sum);
        Assert.IsTrue(sawError);

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnError<int>(250, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Do_NextErrorNot()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var i = 0;
        var sum = 2 + 3 + 4 + 5;
        var sawError = false;
        var res = scheduler.Start(() =>
            xs.Do(x => { i++; sum -= x; }, _ => { sawError = true; })
        );

        Assert.AreEqual(4, i);
        Assert.AreEqual(0, sum);
        Assert.IsFalse(sawError);

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Do_NextErrorCompleted()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var i = 0;
        var sum = 2 + 3 + 4 + 5;
        var sawError = false;
        var hasCompleted = false;
        var res = scheduler.Start(() =>
            xs.Do(
                x => { i++; sum -= x; },
                e => { sawError = true; },
                () => { hasCompleted = true; })
        );

        Assert.AreEqual(4, i);
        Assert.AreEqual(0, sum);
        Assert.IsFalse(sawError);
        Assert.IsTrue(hasCompleted);

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Do_NextErrorCompletedError()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnError<int>(250, ex)
        );

        var i = 0;
        var sum = 2 + 3 + 4 + 5;
        var sawError = false;
        var hasCompleted = false;
        var res = scheduler.Start(() =>
            xs.Do(
                x => { i++; sum -= x; },
                e => { sawError = e == ex; },
                () => { hasCompleted = true; })
        );

        Assert.AreEqual(4, i);
        Assert.AreEqual(0, sum);
        Assert.IsTrue(sawError);
        Assert.IsFalse(hasCompleted);

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnError<int>(250, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Do_NextErrorCompletedNever()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable<int>();

        var i = 0;
        var sawError = false;
        var hasCompleted = false;
        var res = scheduler.Start(() =>
            xs.Do(x => { i++; }, e => { sawError = true; }, () => { hasCompleted = true; })
        );

        Assert.AreEqual(0, i);
        Assert.IsFalse(sawError);
        Assert.IsFalse(hasCompleted);

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Do_Observer_SomeDataWithError()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnError<int>(250, ex)
        );

        var i = 0;
        var sum = 2 + 3 + 4 + 5;
        var sawError = false;
        var hasCompleted = false;
        var res = scheduler.Start(() =>
            xs.Do(ObserverRef.Create<int>(
                x => { i++; sum -= x; },
                e => { sawError = e == ex; },
                () => { hasCompleted = true; }))
        );

        Assert.AreEqual(4, i);
        Assert.AreEqual(0, sum);
        Assert.IsTrue(sawError);
        Assert.IsFalse(hasCompleted);

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnError<int>(250, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Do_Observer_SomeDataWithoutError()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var i = 0;
        var sum = 2 + 3 + 4 + 5;
        var sawError = false;
        var hasCompleted = false;
        var res = scheduler.Start(() =>
            xs.Do(ObserverRef.Create<int>(
                x => { i++; sum -= x; },
                e => { sawError = true; },
                () => { hasCompleted = true; }))
        );

        Assert.AreEqual(4, i);
        Assert.AreEqual(0, sum);
        Assert.IsFalse(sawError);
        Assert.IsTrue(hasCompleted);

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Do1422_Next_NextThrows()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Do(x => { throw ex; })
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Do1422_NextCompleted_NextThrows()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Do(x => { throw ex; }, () => { })
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Do1422_NextCompleted_CompletedThrows()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Do(x => { }, () => { throw ex; })
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnError<int>(250, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Do1422_NextError_NextThrows()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Do(x => { throw ex; }, _ => { })
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Do1422_NextError_ErrorThrows()
    {
        var scheduler = Scheduler;

        var ex1 = new Exception();
        var ex2 = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex1)
        );

        var res = scheduler.Start(() =>
            xs.Do(x => { }, _ => { throw ex2; })
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex2)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Do1422_NextErrorCompleted_NextThrows()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Do(x => { throw ex; }, _ => { }, () => { })
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Do1422_NextErrorCompleted_ErrorThrows()
    {
        var scheduler = Scheduler;

        var ex1 = new Exception();
        var ex2 = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex1)
        );

        var res = scheduler.Start(() =>
            xs.Do(x => { }, _ => { throw ex2; }, () => { })
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex2)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Do1422_NextErrorCompleted_CompletedThrows()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Do(x => { }, _ => { }, () => { throw ex; })
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnError<int>(250, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Do1422_Observer_NextThrows()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Do(ObserverRef.Create<int>(x => { throw ex; }, _ => { }, () => { }))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Do1422_Observer_ErrorThrows()
    {
        var scheduler = Scheduler;

        var ex1 = new Exception();
        var ex2 = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex1)
        );

        var res = scheduler.Start(() =>
            xs.Do(ObserverRef.Create<int>(x => { }, _ => { throw ex2; }, () => { }))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex2)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Do1422_Observer_CompletedThrows()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Do(ObserverRef.Create<int>(x => { }, _ => { }, () => { throw ex; }))
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnError<int>(250, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }
}
