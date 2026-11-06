// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>FromEvent</c> scenarios, from Rx.NET's <c>FromEventTest.cs</c>.</summary>
/// <remarks>
/// 11 of the file's 12 tests (<c>FromEvent_ArgumentChecking</c> is the code-generated stratum).
/// The five conversion tests and <c>FromEvent_ImplicitPublish</c> subscribe through the kit's
/// handler form and dispose asynchronously where the originals call <c>Subscribe</c> and
/// <c>Dispose</c>; <c>FromEvent_SynchronizationContext</c> keeps its threads and blocks on the
/// subscription and disposal inside them, since the original's claim is about which context
/// each runs on, and realizes the description on the context's thread through
/// <see cref="SharedReactiveTest.Realize{T}"/>, where the original calls the operator; the four <c>*_Scheduler*</c> tests count the scheduler's uses through
/// <see cref="MyEventScheduler"/>, the original's <c>LocalScheduler</c> double as a shared
/// <see cref="SchedulerDouble"/>.
/// </remarks>
public abstract class FromEventTests : SharedReactiveTest
{
    [TestMethod]
    public async Task FromEvent_Action()
    {
        var fe = new FromEvent();

        var xs = Seq.FromEvent(h => fe.A += h, h => fe.A -= h);

        fe.OnA();

        var n = 0;
        var d = await xs.SubscribeAsync(Scheduler, _ => n++);

        fe.OnA();
        fe.OnA();

        await d.DisposeAsync();

        fe.OnA();

        Assert.AreEqual(2, n);
    }

    [TestMethod]
    public async Task FromEvent_ActionOfInt()
    {
        var fe = new FromEvent();

        var xs = Seq.FromEvent<int>(h => fe.B += h, h => fe.B -= h);

        fe.OnB(1);

        var n = 0;
        var d = await xs.SubscribeAsync(Scheduler, x => n += x);

        fe.OnB(2);
        fe.OnB(3);

        await d.DisposeAsync();

        fe.OnB(4);

        Assert.AreEqual(2 + 3, n);
    }

    [TestMethod]
    public async Task FromEvent_ActionOfInt_SpecifiedExplicitly()
    {
        var fe = new FromEvent();

        var xs = Seq.FromEvent<Action<int>, int>(h => fe.B += h, h => fe.B -= h);

        fe.OnB(1);

        var n = 0;
        var d = await xs.SubscribeAsync(Scheduler, x => n += x);

        fe.OnB(2);
        fe.OnB(3);

        await d.DisposeAsync();

        fe.OnB(4);

        Assert.AreEqual(2 + 3, n);
    }

    [TestMethod]
    public async Task FromEvent_ActionOfInt_SpecifiedExplicitly_TrivialConversion()
    {
        var fe = new FromEvent();

        var xs = Seq.FromEvent<Action<int>, int>(h => h, h => fe.B += h, h => fe.B -= h);

        fe.OnB(1);

        var n = 0;
        var d = await xs.SubscribeAsync(Scheduler, x => n += x);

        fe.OnB(2);
        fe.OnB(3);

        await d.DisposeAsync();

        fe.OnB(4);

        Assert.AreEqual(2 + 3, n);
    }

    [TestMethod]
    public async Task FromEvent_MyAction()
    {
        var fe = new FromEvent();

        var xs = Seq.FromEvent<MyAction, int>(h => new MyAction(h), h => fe.C += h, h => fe.C -= h);

        fe.OnC(1);

        var n = 0;
        var d = await xs.SubscribeAsync(Scheduler, x => n += x);

        fe.OnC(2);
        fe.OnC(3);

        await d.DisposeAsync();

        fe.OnC(4);

        Assert.AreEqual(2 + 3, n);
    }

    [TestMethod]
    public async Task FromEvent_ImplicitPublish()
    {
        var src = new MyEventSource();

        var addCount = 0;
        var remCount = 0;

        var xs = Seq.FromEventPattern<MyEventArgs>(
            h => { addCount++; src.Bar += h; },
            h => { src.Bar -= h; remCount++; },
            ImmediateScheduler);

        Assert.AreEqual(0, addCount);
        Assert.AreEqual(0, remCount);

        src.OnBar(41);

        var fst = new List<int>();
        var d1 = await xs.SubscribeAsync(Scheduler, e => fst.Add(e.EventArgs.Value));

        Assert.AreEqual(1, addCount);
        Assert.AreEqual(0, remCount);

        src.OnBar(42);

        Assert.IsTrue(fst.SequenceEqual([42]));

        await d1.DisposeAsync();

        Assert.AreEqual(1, addCount);
        Assert.AreEqual(1, remCount);

        var snd = new List<int>();
        var d2 = await xs.SubscribeAsync(Scheduler, e => snd.Add(e.EventArgs.Value));

        Assert.AreEqual(2, addCount);
        Assert.AreEqual(1, remCount);

        src.OnBar(43);

        Assert.IsTrue(fst.SequenceEqual([42]));
        Assert.IsTrue(snd.SequenceEqual([43]));

        var thd = new List<int>();
        var d3 = await xs.SubscribeAsync(Scheduler, e => thd.Add(e.EventArgs.Value));

        Assert.AreEqual(2, addCount);
        Assert.AreEqual(1, remCount);

        src.OnBar(44);

        Assert.IsTrue(fst.SequenceEqual([42]));
        Assert.IsTrue(snd.SequenceEqual([43, 44]));
        Assert.IsTrue(thd.SequenceEqual([44]));

        await d2.DisposeAsync();

        Assert.AreEqual(2, addCount);
        Assert.AreEqual(1, remCount);

        src.OnBar(45);

        Assert.IsTrue(fst.SequenceEqual([42]));
        Assert.IsTrue(snd.SequenceEqual([43, 44]));
        Assert.IsTrue(thd.SequenceEqual([44, 45]));

        await d3.DisposeAsync();

        Assert.AreEqual(2, addCount);
        Assert.AreEqual(2, remCount);

        src.OnBar(46);

        Assert.IsTrue(fst.SequenceEqual([42]));
        Assert.IsTrue(snd.SequenceEqual([43, 44]));
        Assert.IsTrue(thd.SequenceEqual([44, 45]));
    }

    [TestMethod]
    public void FromEvent_SynchronizationContext()
    {
        var beforeSubscribeNull = false;
        var afterSubscribeNull = false;
        var subscribeOnCtx = false;

        var fstNext = false;
        var sndNext = false;
        var thdNext = false;

        var beforeDisposeNull = false;
        var afterDisposeNull = false;
        var disposeOnCtx = false;

        RunWithContext(new MyEventSyncCtx(), ctx =>
        {
            var src = new MyEventSource();

            var addCtx = default(SynchronizationContext);
            var remCtx = default(SynchronizationContext);

            var addEvt = new ManualResetEvent(false);
            var remEvt = new ManualResetEvent(false);

            var xs = Seq.FromEventPattern<MyEventArgs>(
                h => { addCtx = SynchronizationContext.Current; src.Bar += h; addEvt.Set(); },
                h => { remCtx = SynchronizationContext.Current; src.Bar -= h; remEvt.Set(); });

            Realize(xs);

            Assert.IsNull(addCtx);
            Assert.IsNull(remCtx);

            IAsyncDisposable? d = null;
            var res = new List<int>();

            var s = new Thread(() =>
            {
                beforeSubscribeNull = SynchronizationContext.Current is null;
                d = xs.SubscribeAsync(Scheduler, e => res.Add(e.EventArgs.Value))
                    .AsTask().GetAwaiter().GetResult();
                afterSubscribeNull = SynchronizationContext.Current is null;
            });

            s.Start();
            s.Join();

            addEvt.WaitOne();

            subscribeOnCtx = ReferenceEquals(addCtx, ctx);

            src.OnBar(42);
            fstNext = res.SequenceEqual([42]);

            src.OnBar(43);
            sndNext = res.SequenceEqual([42, 43]);

            var u = new Thread(() =>
            {
                beforeDisposeNull = SynchronizationContext.Current is null;
                d!.DisposeAsync().AsTask().GetAwaiter().GetResult();
                afterDisposeNull = SynchronizationContext.Current is null;
            });

            u.Start();
            u.Join();

            remEvt.WaitOne();

            disposeOnCtx = ReferenceEquals(remCtx, ctx);

            src.OnBar(44);
            thdNext = res.SequenceEqual([42, 43]);
        });

        Assert.IsTrue(beforeSubscribeNull);
        Assert.IsTrue(subscribeOnCtx);
        Assert.IsTrue(afterSubscribeNull);

        Assert.IsTrue(fstNext);
        Assert.IsTrue(sndNext);
        Assert.IsTrue(thdNext);

        Assert.IsTrue(beforeDisposeNull);
        Assert.IsTrue(disposeOnCtx);
        Assert.IsTrue(afterDisposeNull);
    }

    [TestMethod]
    public async Task FromEvent_Scheduler1()
    {
        await RunWithScheduler((s, add, remove) =>
            Seq.FromEvent<MyEventArgs>(h => { add(); }, h => { remove(); }, s));
    }

    [TestMethod]
    public async Task FromEvent_Scheduler2()
    {
        await RunWithScheduler((s, add, remove) =>
            Seq.FromEvent(h => { add(); }, h => { remove(); }, s));
    }

    [TestMethod]
    public async Task FromEvent_Scheduler3()
    {
        await RunWithScheduler((s, add, remove) =>
            Seq.FromEvent<Action<MyEventArgs>, MyEventArgs>(
                h => { add(); },
                h => { remove(); },
                s));
    }

    [TestMethod]
    public async Task FromEvent_Scheduler4()
    {
        await RunWithScheduler((s, add, remove) =>
            Seq.FromEvent<Action, MyEventArgs>(
                h => () => { },
                h => { add(); },
                h => { remove(); },
                s));
    }

    private void RunWithContext<T>(T ctx, Action<T> run)
        where T : SynchronizationContext
    {
        var t = new Thread(() =>
        {
            SynchronizationContext.SetSynchronizationContext(ctx);
            run(ctx);
        });

        t.Start();
        t.Join();
    }

    private async Task RunWithScheduler<T>(Func<SchedulerRef, Action, Action, Seq<T>> run)
    {
        var n = 0;
        var a = 0;
        var r = 0;

        var s = new MyEventScheduler(Target, () => n++);
        var add = new Action(() => a++);
        var rem = new Action(() => r++);

        var xs = run(s, add, rem);

        Assert.AreEqual(0, n);
        Assert.AreEqual(0, a);
        Assert.AreEqual(0, r);

        var d1 = await xs.SubscribeAsync(Scheduler, _ => { });

        Assert.AreEqual(1, n);
        Assert.AreEqual(1, a);
        Assert.AreEqual(0, r);

        var d2 = await xs.SubscribeAsync(Scheduler, _ => { });

        Assert.AreEqual(1, n);
        Assert.AreEqual(1, a);
        Assert.AreEqual(0, r);

        await d1.DisposeAsync();

        Assert.AreEqual(1, n);
        Assert.AreEqual(1, a);
        Assert.AreEqual(0, r);

        await d2.DisposeAsync();

        Assert.AreEqual(2, n);
        Assert.AreEqual(1, a);
        Assert.AreEqual(1, r);
    }
}
