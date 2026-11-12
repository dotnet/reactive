// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Select</c> scenarios, from Rx.NET's <c>SelectTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 16 of the file's 18 tests, for the plain and the indexed form alike: the virtual-time tests
/// over a hot source with a counting selector, the four <c>Select_Select*</c> compositions, and
/// two kinds of hand-adjusted test. <c>*_Throws</c> subscribes with handlers that throw, in real
/// time, and expects the exception back from the subscribe call: the shared text awaits the raw
/// surface inside <c>Assert.ThrowsExactlyAsync</c>, as <c>Create_ObserverThrows</c> does.
/// <c>*_DisposeInsideSelector</c> disposes the subscription from inside the synchronous
/// selector; the shared text starts the disposal and leaves it to complete, which it does within
/// the tick on both targets, so the recorded messages and the source's subscription window are
/// the original's.
/// </para>
/// <para>
/// <c>Select</c> was plumbing before this step, a node other scenarios compose with; it keeps
/// that role from its own folder. Not here: the two <c>*_ArgumentChecking</c> tests, the
/// code-generated stratum.
/// </para>
/// </remarks>
public abstract class SelectTests : SharedReactiveTest
{
    [TestMethod]
    public async Task Select_Throws()
    {
        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await Seq.Return(1).Select(x => x).SubscribeAsync(
                Scheduler,
                new Action<int>(x =>
                {
                    throw new InvalidOperationException();
                })));

        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await Seq.Throw<int>(new Exception()).Select(x => x).SubscribeAsync(
                Scheduler,
                x => { },
                exception =>
                {
                    throw new InvalidOperationException();
                }));

        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await Seq.Empty<int>().Select(x => x).SubscribeAsync(
                Scheduler,
                x => { },
                exception => { },
                () =>
                {
                    throw new InvalidOperationException();
                }));

        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await Seq.Create(new Func<ObserverRef<int>, ValueTask<Action?>>(o =>
            {
                throw new InvalidOperationException();
            })).Select(x => x).SubscribeAsync(Scheduler, _ => { }));
    }

    [TestMethod]
    public async Task Select_DisposeInsideSelector()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(200, 2),
            OnNext(500, 3),
            OnNext(600, 4)
        );

        var invoked = 0;

        var res = scheduler.CreateObserver<int>();

        // The original holds the subscription in a SerialDisposable and disposes it from inside
        // the selector. The selector is synchronous, so the disposal is started and left to
        // complete, which it does within the tick on both targets.
        IAsyncDisposable? d = null;

        d = await xs.Select(x =>
        {
            invoked++;
            if (scheduler.Clock > 400)
            {
                _ = d!.DisposeAsync();
            }
            return x;
        }).SubscribeAsync(res);

        scheduler.ScheduleAbsolute(Disposed, async () => await d.DisposeAsync());

        scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(100, 1),
            OnNext(200, 2)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(0, 500)
        );

        Assert.AreEqual(3, invoked);
    }

    [TestMethod]
    public void Select_Completed()
    {
        var scheduler = Scheduler;

        var invoked = 0;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(350, 5),
            OnCompleted<int>(400),
            OnNext(410, -1),
            OnCompleted<int>(420),
            OnError<int>(430, new Exception())
        );

        var res = scheduler.Start(() =>
            xs.Select(x =>
            {
                invoked++;
                return x + 1;
            })
        );

        res.Messages.AssertEqual(
            OnNext(210, 3),
            OnNext(240, 4),
            OnNext(290, 5),
            OnNext(350, 6),
            OnCompleted<int>(400)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        Assert.AreEqual(4, invoked);
    }

    [TestMethod]
    public void Select_NotCompleted()
    {
        var scheduler = Scheduler;

        var invoked = 0;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(350, 5)
        );

        var res = scheduler.Start(() =>
            xs.Select(x =>
            {
                invoked++;
                return x + 1;
            })
        );

        res.Messages.AssertEqual(
            OnNext(210, 3),
            OnNext(240, 4),
            OnNext(290, 5),
            OnNext(350, 6)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        Assert.AreEqual(4, invoked);
    }

    [TestMethod]
    public void Select_Error()
    {
        var scheduler = Scheduler;

        var invoked = 0;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(350, 5),
            OnError<int>(400, ex),
            OnNext(410, -1),
            OnCompleted<int>(420),
            OnError<int>(430, new Exception())
        );

        var res = scheduler.Start(() =>
            xs.Select(x =>
            {
                invoked++;
                return x + 1;
            })
        );

        res.Messages.AssertEqual(
            OnNext(210, 3),
            OnNext(240, 4),
            OnNext(290, 5),
            OnNext(350, 6),
            OnError<int>(400, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        Assert.AreEqual(4, invoked);
    }

    [TestMethod]
    public void Select_SelectorThrows()
    {
        var scheduler = Scheduler;

        var invoked = 0;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(350, 5),
            OnCompleted<int>(400),
            OnNext(410, -1),
            OnCompleted<int>(420),
            OnError<int>(430, new Exception())
        );

        var ex = new Exception();

        var res = scheduler.Start(() =>
            xs.Select(x =>
            {
                invoked++;
                if (invoked == 3)
                {
                    throw ex;
                }

                return x + 1;
            })
        );

        res.Messages.AssertEqual(
            OnNext(210, 3),
            OnNext(240, 4),
            OnError<int>(290, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 290)
        );

        Assert.AreEqual(3, invoked);
    }

    [TestMethod]
    public async Task SelectWithIndex_Throws()
    {
        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await Seq.Return(1).Select((x, index) => x).SubscribeAsync(
                Scheduler,
                new Action<int>(x =>
                {
                    throw new InvalidOperationException();
                })));

        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await Seq.Throw<int>(new Exception()).Select((x, index) => x).SubscribeAsync(
                Scheduler,
                x => { },
                exception =>
                {
                    throw new InvalidOperationException();
                }));

        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await Seq.Empty<int>().Select((x, index) => x).SubscribeAsync(
                Scheduler,
                x => { },
                exception => { },
                () =>
                {
                    throw new InvalidOperationException();
                }));

        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await Seq.Create(new Func<ObserverRef<int>, ValueTask<Action?>>(o =>
            {
                throw new InvalidOperationException();
            })).Select((x, index) => x).SubscribeAsync(Scheduler, _ => { }));
    }

    [TestMethod]
    public async Task SelectWithIndex_DisposeInsideSelector()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(200, 2),
            OnNext(500, 3),
            OnNext(600, 4)
        );

        var invoked = 0;

        var res = scheduler.CreateObserver<int>();

        // The original holds the subscription in a SerialDisposable and disposes it from inside
        // the selector. The selector is synchronous, so the disposal is started and left to
        // complete, which it does within the tick on both targets.
        IAsyncDisposable? d = null;

        d = await xs.Select((x, index) =>
        {
            invoked++;
            if (scheduler.Clock > 400)
            {
                _ = d!.DisposeAsync();
            }
            return x;
        }).SubscribeAsync(res);

        scheduler.ScheduleAbsolute(Disposed, async () => await d.DisposeAsync());

        scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(100, 1),
            OnNext(200, 2)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(0, 500)
        );

        Assert.AreEqual(3, invoked);
    }

    [TestMethod]
    public void SelectWithIndex_Completed()
    {
        var scheduler = Scheduler;

        var invoked = 0;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 5),
            OnNext(210, 4),
            OnNext(240, 3),
            OnNext(290, 2),
            OnNext(350, 1),
            OnCompleted<int>(400),
            OnNext(410, -1),
            OnCompleted<int>(420),
            OnError<int>(430, new Exception())
        );

        var res = scheduler.Start(() =>
            xs.Select((x, index) =>
            {
                invoked++;
                return (x + 1) + (index * 10);
            })
        );

        res.Messages.AssertEqual(
            OnNext(210, 5),
            OnNext(240, 14),
            OnNext(290, 23),
            OnNext(350, 32),
            OnCompleted<int>(400)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        Assert.AreEqual(4, invoked);
    }

    [TestMethod]
    public void SelectWithIndex_NotCompleted()
    {
        var scheduler = Scheduler;

        var invoked = 0;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 5),
            OnNext(210, 4),
            OnNext(240, 3),
            OnNext(290, 2),
            OnNext(350, 1)
        );

        var res = scheduler.Start(() =>
            xs.Select((x, index) =>
            {
                invoked++;
                return (x + 1) + (index * 10);
            })
        );

        res.Messages.AssertEqual(
            OnNext(210, 5),
            OnNext(240, 14),
            OnNext(290, 23),
            OnNext(350, 32)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        Assert.AreEqual(4, invoked);
    }

    [TestMethod]
    public void SelectWithIndex_Error()
    {
        var scheduler = Scheduler;

        var invoked = 0;
        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 5),
            OnNext(210, 4),
            OnNext(240, 3),
            OnNext(290, 2),
            OnNext(350, 1),
            OnError<int>(400, ex),
            OnNext(410, -1),
            OnCompleted<int>(420),
            OnError<int>(430, new Exception())
        );

        var res = scheduler.Start(() =>
            xs.Select((x, index) =>
            {
                invoked++;
                return (x + 1) + (index * 10);
            })
        );

        res.Messages.AssertEqual(
            OnNext(210, 5),
            OnNext(240, 14),
            OnNext(290, 23),
            OnNext(350, 32),
            OnError<int>(400, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        Assert.AreEqual(4, invoked);
    }

    [TestMethod]
    public void SelectWithIndex_SelectorThrows()
    {
        var scheduler = Scheduler;

        var invoked = 0;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 5),
            OnNext(210, 4),
            OnNext(240, 3),
            OnNext(290, 2),
            OnNext(350, 1),
            OnCompleted<int>(400),
            OnNext(410, -1),
            OnCompleted<int>(420),
            OnError<int>(430, new Exception())
        );

        var ex = new Exception();

        var res = scheduler.Start(() =>
            xs.Select((x, index) =>
            {
                invoked++;
                if (invoked == 3)
                {
                    throw ex;
                }

                return (x + 1) + (index * 10);
            })
        );

        res.Messages.AssertEqual(
            OnNext(210, 5),
            OnNext(240, 14),
            OnError<int>(290, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 290)
        );

        Assert.AreEqual(3, invoked);
    }

    [TestMethod]
    public void Select_Select1()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 5),
            OnNext(210, 4),
            OnNext(240, 3),
            OnNext(290, 2),
            OnNext(350, 1),
            OnCompleted<int>(400)
        );

        var res = scheduler.Start(() =>
            xs.Select(x => x + 1).Select(x => x - 2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 4 + 1 - 2),
            OnNext(240, 3 + 1 - 2),
            OnNext(290, 2 + 1 - 2),
            OnNext(350, 1 + 1 - 2),
            OnCompleted<int>(400)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Select_Select2()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 5),
            OnNext(210, 4),
            OnNext(240, 3),
            OnNext(290, 2),
            OnNext(350, 1),
            OnCompleted<int>(400)
        );

        var res = scheduler.Start(() =>
            xs.Select((x, i) => x + i).Select(x => x - 2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 4 + 0 - 2),
            OnNext(240, 3 + 1 - 2),
            OnNext(290, 2 + 2 - 2),
            OnNext(350, 1 + 3 - 2),
            OnCompleted<int>(400)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Select_Select3()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 5),
            OnNext(210, 4),
            OnNext(240, 3),
            OnNext(290, 2),
            OnNext(350, 1),
            OnCompleted<int>(400)
        );

        var res = scheduler.Start(() =>
            xs.Select(x => x + 1).Select((x, i) => x - i)
        );

        res.Messages.AssertEqual(
            OnNext(210, 4 + 1 - 0),
            OnNext(240, 3 + 1 - 1),
            OnNext(290, 2 + 1 - 2),
            OnNext(350, 1 + 1 - 3),
            OnCompleted<int>(400)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Select_Select4()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 5),
            OnNext(210, 4),
            OnNext(240, 3),
            OnNext(290, 2),
            OnNext(350, 1),
            OnCompleted<int>(400)
        );

        var res = scheduler.Start(() =>
            xs.Select((x, i) => x + i).Select((x, i) => x - i)
        );

        res.Messages.AssertEqual(
            OnNext(210, 4),
            OnNext(240, 3),
            OnNext(290, 2),
            OnNext(350, 1),
            OnCompleted<int>(400)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }
}
