// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Text;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>GroupByUntil</c> scenarios, from Rx.NET's <c>GroupByUntilTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 56 of the file's 58 tests (the two <c>*_ArgumentChecking</c> tests are the code-generated
/// stratum), all transcribed by the rules that produced the <c>GroupBy</c> scenarios, which
/// reproduce those 48 from their originals exactly: the outer sequence completing, failing,
/// disposed, with a key, element or comparer that throws; the inner groups subscribed by hand a
/// tick after they appear, completing when their duration signals, failing, disposed, and
/// independent of one another; a group subscribed after the outer sequence ended; null keys;
/// and all of it again with a capacity. The scheduler-driven comparer is
/// <see cref="GroupByComparer"/>, the group handler subscriptions are the kit's handler form
/// (<c>async</c> where the handler awaits), and the inner subscriptions and disposals are
/// awaited where the originals call <c>Subscribe</c> and <c>Dispose</c>.
/// </para>
/// <para>
/// The duration selector takes the group as a <see cref="Group{TKey, T}"/> description, which
/// the bridge hands it from the target's own group, and returns a description.
/// </para>
/// </remarks>
public abstract class GroupByUntilTests : SharedReactiveTest
{
    private const int GroupByUntilCapacity = 1024;

    [TestMethod]
    public void GroupByUntil_WithKeyComparer()
    {
        var keyInvoked = 0;

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);

        var res = Scheduler.Start(() =>
            xs.GroupByUntil(
                x =>
                {
                    keyInvoked++;
                    return x.Trim();
                },
                g => g.Skip(2),
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnNext(470, "FOO"),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );

        Assert.AreEqual(12, keyInvoked);
    }

    [TestMethod]
    public void GroupByUntil_Outer_Complete()
    {
        var keyInvoked = 0;
        var eleInvoked = 0;

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);

        var res = Scheduler.Start(() =>
            xs.GroupByUntil(
                x =>
                {
                    keyInvoked++;
                    return x.Trim();
                },
                x =>
                {
                    eleInvoked++;
                    return Reverse(x);
                },
                g => g.Skip(2),
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnNext(470, "FOO"),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );

        Assert.AreEqual(12, keyInvoked);
        Assert.AreEqual(12, eleInvoked);
    }

    [TestMethod]
    public void GroupByUntil_Outer_Error()
    {
        var keyInvoked = 0;
        var eleInvoked = 0;
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnError<string>(570, ex),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);

        var res = Scheduler.Start(() =>
            xs.GroupByUntil(
                x =>
                {
                    keyInvoked++;
                    return x.Trim();
                },
                x =>
                {
                    eleInvoked++;
                    return Reverse(x);
                },
                g => g.Skip(2),
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnNext(470, "FOO"),
            OnError<string>(570, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );

        Assert.AreEqual(12, keyInvoked);
        Assert.AreEqual(12, eleInvoked);
    }

    [TestMethod]
    public void GroupByUntil_Outer_Dispose()
    {
        var keyInvoked = 0;
        var eleInvoked = 0;

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);

        var res = Scheduler.Start(() =>
            xs.GroupByUntil(
                x =>
                {
                    keyInvoked++;
                    return x.Trim();
                },
                x =>
                {
                    eleInvoked++;
                    return Reverse(x);
                },
                g => g.Skip(2),
                comparer
            ).Select(g => g.Key),
            355
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz")
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 355)
        );

        Assert.AreEqual(5, keyInvoked);
        Assert.AreEqual(5, eleInvoked);
    }

    [TestMethod]
    public void GroupByUntil_Outer_KeyThrow()
    {
        var keyInvoked = 0;
        var eleInvoked = 0;
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);

        var res = Scheduler.Start(() =>
            xs.GroupByUntil(
                x =>
                {
                    keyInvoked++;
                    if (keyInvoked == 10)
                    {
                        throw ex;
                    }

                    return x.Trim();
                },
                x =>
                {
                    eleInvoked++;
                    return Reverse(x);
                },
                g => g.Skip(2),
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnNext(470, "FOO"),
            OnError<string>(480, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 480)
        );

        Assert.AreEqual(10, keyInvoked);
        Assert.AreEqual(9, eleInvoked);
    }

    [TestMethod]
    public void GroupByUntil_Outer_EleThrow()
    {
        var keyInvoked = 0;
        var eleInvoked = 0;
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);

        var res = Scheduler.Start(() =>
            xs.GroupByUntil(
                x =>
                {
                    keyInvoked++;
                    return x.Trim();
                },
                x =>
                {
                    eleInvoked++;
                    if (eleInvoked == 10)
                    {
                        throw ex;
                    }

                    return Reverse(x);
                },
                g => g.Skip(2),
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnNext(470, "FOO"),
            OnError<string>(480, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 480)
        );

        Assert.AreEqual(10, keyInvoked);
        Assert.AreEqual(10, eleInvoked);
    }

    [TestMethod]
    public void GroupByUntil_Outer_ComparerEqualsThrow()
    {
        var keyInvoked = 0;
        var eleInvoked = 0;

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler, 250, ushort.MaxValue);

        var res = Scheduler.Start(() =>
            xs.GroupByUntil(
                x =>
                {
                    keyInvoked++;
                    return x.Trim();
                },
                x =>
                {
                    eleInvoked++;
                    return Reverse(x);
                },
                g => g.Skip(2),
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnError<string>(310, comparer.EqualsException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );

        Assert.AreEqual(4, keyInvoked);
        Assert.AreEqual(3, eleInvoked);
    }

    [TestMethod]
    public void GroupByUntil_Outer_ComparerGetHashCodeThrow()
    {
        var keyInvoked = 0;
        var eleInvoked = 0;

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler, ushort.MaxValue, 410);

        var res = Scheduler.Start(() =>
            xs.GroupByUntil(
                x =>
                {
                    keyInvoked++;
                    return x.Trim();
                },
                x =>
                {
                    eleInvoked++;
                    return Reverse(x);
                },
                g => g.Skip(2),
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnError<string>(420, comparer.HashCodeException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );

        Assert.AreEqual(8, keyInvoked);
        Assert.AreEqual(7, eleInvoked);
    }

    [TestMethod]
    public async Task GroupByUntil_Inner_Complete()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            Scheduler.ScheduleRelative(100, async () => innerSubscriptions[group.Key] = await group!.SubscribeAsync(result));
        }));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.Start();

        Assert.HasCount(5, inners);

        res["foo"].Messages.AssertEqual(
            OnCompleted<string>(320)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "), // Breaking change > v2.2 - prior to resolving a deadlock, the group would get closed prior to letting this message through
            OnCompleted<string>(420)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(480, "  zab"),
            OnNext(510, " ZAb "), // Breaking change > v2.2 - prior to resolving a deadlock, the group would get closed prior to letting this message through
            OnCompleted<string>(510)
        );

        res["qux"].Messages.AssertEqual(
            OnCompleted<string>(570)
        );

        res["FOO"].Messages.AssertEqual(
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Inner_Complete_All()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            innerSubscriptions[group.Key] = await group!.SubscribeAsync(result);
        }));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.Start();

        Assert.HasCount(5, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnCompleted<string>(310)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "),
            OnCompleted<string>(420)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB "),
            OnNext(480, "  zab"),
            OnNext(510, " ZAb "),
            OnCompleted<string>(510)
        );

        res["qux"].Messages.AssertEqual(
            OnNext(360, " xuq  "),
            OnCompleted<string>(570)
        );

        res["FOO"].Messages.AssertEqual(
            OnNext(470, " OOF"),
            OnNext(530, "    oOf    "),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Inner_Error()
    {
        var ex1 = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnError<string>(570, ex1),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            Scheduler.ScheduleRelative(100, async () => innerSubscriptions[group.Key] = await group!.SubscribeAsync(result));
        }, ex => { }));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.Start();

        Assert.HasCount(5, inners);

        res["foo"].Messages.AssertEqual(
            OnCompleted<string>(320)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "), // Breaking change > v2.2 - prior to resolving a deadlock, the group would get closed prior to letting this message through
            OnCompleted<string>(420)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(480, "  zab"),
            OnNext(510, " ZAb "), // Breaking change > v2.2 - prior to resolving a deadlock, the group would get closed prior to letting this message through
            OnCompleted<string>(510)
        );

        res["qux"].Messages.AssertEqual(
            OnError<string>(570, ex1)
        );

        res["FOO"].Messages.AssertEqual(
            OnError<string>(570, ex1)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Inner_Dispose()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            innerSubscriptions[group.Key] = await group!.SubscribeAsync(result);
        }));

        Scheduler.ScheduleAbsolute(400, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.Start();

        Assert.HasCount(4, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnCompleted<string>(310)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnNext(390, "rab   ")
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB ")
        );

        res["qux"].Messages.AssertEqual(
            OnNext(360, " xuq  ")
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Inner_KeyThrow()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        var keyInvoked = 0;
        var ex = new Exception();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x =>
        {
            keyInvoked++;
            if (keyInvoked == 6)
            {
                throw ex;
            }

            return x.Trim();
        }, x => Reverse(x), g => g.Skip(2), comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            innerSubscriptions[group.Key] = await group!.SubscribeAsync(result);
        }, _ => { }));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.Start();

        Assert.HasCount(3, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnCompleted<string>(310)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnError<string>(360, ex)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB "),
            OnError<string>(360, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 360)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Inner_EleThrow()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        var eleInvoked = 0;
        var ex = new Exception();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x =>
        {
            eleInvoked++;
            if (eleInvoked == 6)
            {
                throw ex;
            }

            return Reverse(x);
        }, g => g.Skip(2), comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            innerSubscriptions[group.Key] = await group!.SubscribeAsync(result);
        }, _ => { }));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.Start();

        Assert.HasCount(4, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnCompleted<string>(310)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnError<string>(360, ex)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB "),
            OnError<string>(360, ex)
        );

        res["qux"].Messages.AssertEqual(
            OnError<string>(360, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 360)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Inner_Comparer_EqualsThrow()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler, 400, ushort.MaxValue);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            innerSubscriptions[group.Key] = await group!.SubscribeAsync(result);
        }, _ => { }));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.Start();

        Assert.HasCount(4, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnCompleted<string>(310)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnNext(390, "rab   "),
            OnError<string>(420, comparer.EqualsException)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB "),
            OnError<string>(420, comparer.EqualsException)
        );

        res["qux"].Messages.AssertEqual(
            OnNext(360, " xuq  "),
            OnError<string>(420, comparer.EqualsException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Inner_Comparer_GetHashCodeThrow()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler, ushort.MaxValue, 400);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            innerSubscriptions[group.Key] = await group!.SubscribeAsync(result);
        }, _ => { }));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.Start();

        Assert.HasCount(4, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnCompleted<string>(310)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnNext(390, "rab   "),
            OnError<string>(420, comparer.HashCodeException)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB "),
            OnError<string>(420, comparer.HashCodeException)
        );

        res["qux"].Messages.AssertEqual(
            OnNext(360, " xuq  "),
            OnError<string>(420, comparer.HashCodeException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Outer_Independence()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();
        var outerResults = Scheduler.CreateObserver<string>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            await outerResults.OnNextAsync(group.Key);
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            innerSubscriptions[group.Key] = await group!.SubscribeAsync(result);
        }, outerResults.OnErrorAsync, outerResults.OnCompletedAsync));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.ScheduleAbsolute(320, async () => await outerSubscription!.DisposeAsync());

        Scheduler.Start();

        Assert.HasCount(2, inners);

        outerResults.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR")
        );

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnCompleted<string>(310)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "),
            OnCompleted<string>(420)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Inner_Independence()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();
        var outerResults = Scheduler.CreateObserver<string>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            await outerResults.OnNextAsync(group.Key);
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            innerSubscriptions[group.Key] = await group!.SubscribeAsync(result);
        }, outerResults.OnErrorAsync, outerResults.OnCompletedAsync));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.ScheduleAbsolute(320, async () => await innerSubscriptions["foo"]!.DisposeAsync());

        Scheduler.Start();

        Assert.HasCount(5, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnCompleted<string>(310)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "),
            OnCompleted<string>(420)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB "),
            OnNext(480, "  zab"),
            OnNext(510, " ZAb "),
            OnCompleted<string>(510)
        );

        res["qux"].Messages.AssertEqual(
            OnNext(360, " xuq  "),
            OnCompleted<string>(570)
        );

        res["FOO"].Messages.AssertEqual(
            OnNext(470, " OOF"),
            OnNext(530, "    oOf    "),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Inner_Multiple_Independence()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();
        var outerResults = Scheduler.CreateObserver<string>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            await outerResults.OnNextAsync(group.Key);
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            innerSubscriptions[group.Key] = await group!.SubscribeAsync(result);
        }, outerResults.OnErrorAsync, outerResults.OnCompletedAsync));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.ScheduleAbsolute(320, async () => await innerSubscriptions["foo"]!.DisposeAsync());
        Scheduler.ScheduleAbsolute(280, async () => await innerSubscriptions["baR"]!.DisposeAsync());
        Scheduler.ScheduleAbsolute(355, async () => await innerSubscriptions["Baz"]!.DisposeAsync());
        Scheduler.ScheduleAbsolute(400, async () => await innerSubscriptions["qux"]!.DisposeAsync());

        Scheduler.Start();

        Assert.HasCount(5, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnCompleted<string>(310)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab")
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB ")
        );

        res["qux"].Messages.AssertEqual(
            OnNext(360, " xuq  ")
        );

        res["FOO"].Messages.AssertEqual(
            OnNext(470, " OOF"),
            OnNext(530, "    oOf    "),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Inner_Escape_Complete()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(310, "foO "),
            OnNext(470, "FOO "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570)
        );

        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inner = default(Seq<string>);
        var innerSubscription = default(IAsyncDisposable);
        var res = Scheduler.CreateObserver<string>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), g => g.Skip(2)));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, group =>
        {
            inner = group;
        }));

        Scheduler.ScheduleAbsolute(600, async () => innerSubscription = await inner!.SubscribeAsync(res));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            await innerSubscription!.DisposeAsync();
        });

        Scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );

        res.Messages.AssertEqual(
            OnCompleted<string>(600)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Inner_Escape_Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(310, "foO "),
            OnNext(470, "FOO "),
            OnNext(530, "    fOo    "),
            OnError<string>(570, ex)
        );

        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inner = default(Seq<string>);
        var innerSubscription = default(IAsyncDisposable);
        var res = Scheduler.CreateObserver<string>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), g => g.Skip(2)));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, group =>
        {
            inner = group;
        }, _ => { }));

        Scheduler.ScheduleAbsolute(600, async () => innerSubscription = await inner!.SubscribeAsync(res));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            await innerSubscription!.DisposeAsync();
        });

        Scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );

        res.Messages.AssertEqual(
            OnError<string>(600, ex)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Inner_Escape_Dispose()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(310, "foO "),
            OnNext(470, "FOO "),
            OnNext(530, "    fOo    "),
            OnError<string>(570, new Exception())
        );

        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inner = default(Seq<string>);
        var innerSubscription = default(IAsyncDisposable);
        var res = Scheduler.CreateObserver<string>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), g => g.Skip(2)));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, group =>
        {
            inner = group;
        }));

        Scheduler.ScheduleAbsolute(290, async () => await outerSubscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(600, async () => innerSubscription = await inner!.SubscribeAsync(res));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await innerSubscription!.DisposeAsync();
        });

        Scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 290)
        );

        res.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public void GroupByUntil_Default()
    {
        var keyInvoked = 0;
        var eleInvoked = 0;

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var res = Scheduler.Start(() =>
            xs.GroupByUntil(
                x =>
                {
                    keyInvoked++;
                    return x.Trim().ToLower();
                },
                x =>
                {
                    eleInvoked++;
                    return Reverse(x);
                },
                g => g.Skip(2)
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "bar"),
            OnNext(350, "baz"),
            OnNext(360, "qux"),
            OnNext(470, "foo"),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );

        Assert.AreEqual(12, keyInvoked);
        Assert.AreEqual(12, eleInvoked);
    }

    [TestMethod]
    public void GroupByUntil_DurationSelector_Throws()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, "foo")
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.GroupByUntil<string, string, string>(x => x, g => { throw ex; })
        );

        res.Messages.AssertEqual(
            OnError<Group<string, string>>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void GroupByUntil_NullKeys_Simple_Never()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, "bar"),
            OnNext(240, "foo"),
            OnNext(310, "qux"),
            OnNext(470, "baz"),
            OnCompleted<string>(500)
        );

        var res = Scheduler.Start(() => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => Seq.Never<Unit>()).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

        res.Messages.AssertEqual(
            OnNext(220, "(null)bar"),
            OnNext(240, "FOOfoo"),
            OnNext(310, "QUXqux"),
            OnNext(470, "(null)baz"),
            OnCompleted<string>(500)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 500)
        );
    }

    [TestMethod]
    public void GroupByUntil_NullKeys_Simple_Expire1()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, "bar"),
            OnNext(240, "foo"),
            OnNext(310, "qux"),
            OnNext(470, "baz"),
            OnCompleted<string>(500)
        );

        var n = 0;
        var res = Scheduler.Start(() => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => { if (g.Key == null) { n++; } return Seq.Timer(TimeSpan.FromTicks(50), Scheduler); }).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

        Assert.AreEqual(2, n);

        res.Messages.AssertEqual(
            OnNext(220, "(null)bar"),
            OnNext(240, "FOOfoo"),
            OnNext(310, "QUXqux"),
            OnNext(470, "(null)baz"),
            OnCompleted<string>(500)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 500)
        );
    }

    [TestMethod]
    public void GroupByUntil_NullKeys_Simple_Expire2()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, "bar"),
            OnNext(240, "foo"),
            OnNext(310, "qux"),
            OnNext(470, "baz"),
            OnCompleted<string>(500)
        );

        var n = 0;
        var res = Scheduler.Start(() => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => { if (g.Key == null) { n++; } return Seq.Timer(TimeSpan.FromTicks(50), Scheduler).IgnoreElements(); }).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

        Assert.AreEqual(2, n);

        res.Messages.AssertEqual(
            OnNext(220, "(null)bar"),
            OnNext(240, "FOOfoo"),
            OnNext(310, "QUXqux"),
            OnNext(470, "(null)baz"),
            OnCompleted<string>(500)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 500)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_NullKeys_Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(220, "bar"),
            OnNext(240, "foo"),
            OnNext(310, "qux"),
            OnNext(470, "baz"),
            OnError<string>(500, ex)
        );

        var nullGroup = Scheduler.CreateObserver<string>();
        var err = default(Exception);

        Scheduler.ScheduleAbsolute(200, () => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => Seq.Never<Unit>()).Where(g => g.Key == null).SubscribeAsync(Scheduler, async g => await g.SubscribeAsync(nullGroup), ex_ => err = ex_));
        Scheduler.Start();

        Assert.AreSame(ex, err);

        nullGroup.Messages.AssertEqual(
            OnNext(220, "bar"),
            OnNext(470, "baz"),
            OnError<string>(500, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 500)
        );
    }

    [TestMethod]
    public void GroupByUntil_Capacity_WithKeyComparer()
    {
        var keyInvoked = 0;

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);

        var res = Scheduler.Start(() =>
            xs.GroupByUntil(
                x =>
                {
                    keyInvoked++;
                    return x.Trim();
                },
                g => g.Skip(2),
                GroupByUntilCapacity,
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnNext(470, "FOO"),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );

        Assert.AreEqual(12, keyInvoked);
    }

    [TestMethod]
    public void GroupByUntil_Capacity_Outer_Complete()
    {
        var keyInvoked = 0;
        var eleInvoked = 0;

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);

        var res = Scheduler.Start(() =>
            xs.GroupByUntil(
                x =>
                {
                    keyInvoked++;
                    return x.Trim();
                },
                x =>
                {
                    eleInvoked++;
                    return Reverse(x);
                },
                g => g.Skip(2),
                GroupByUntilCapacity,
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnNext(470, "FOO"),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );

        Assert.AreEqual(12, keyInvoked);
        Assert.AreEqual(12, eleInvoked);
    }

    [TestMethod]
    public void GroupByUntil_Capacity_Outer_Error()
    {
        var keyInvoked = 0;
        var eleInvoked = 0;
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnError<string>(570, ex),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);

        var res = Scheduler.Start(() =>
            xs.GroupByUntil(
                x =>
                {
                    keyInvoked++;
                    return x.Trim();
                },
                x =>
                {
                    eleInvoked++;
                    return Reverse(x);
                },
                g => g.Skip(2),
                GroupByUntilCapacity,
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnNext(470, "FOO"),
            OnError<string>(570, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );

        Assert.AreEqual(12, keyInvoked);
        Assert.AreEqual(12, eleInvoked);
    }

    [TestMethod]
    public void GroupByUntil_Capacity_Outer_Dispose()
    {
        var keyInvoked = 0;
        var eleInvoked = 0;

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);

        var res = Scheduler.Start(() =>
            xs.GroupByUntil(
                x =>
                {
                    keyInvoked++;
                    return x.Trim();
                },
                x =>
                {
                    eleInvoked++;
                    return Reverse(x);
                },
                g => g.Skip(2),
                GroupByUntilCapacity,
                comparer
            ).Select(g => g.Key),
            355
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz")
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 355)
        );

        Assert.AreEqual(5, keyInvoked);
        Assert.AreEqual(5, eleInvoked);
    }

    [TestMethod]
    public void GroupByUntil_Capacity_Outer_KeyThrow()
    {
        var keyInvoked = 0;
        var eleInvoked = 0;
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);

        var res = Scheduler.Start(() =>
            xs.GroupByUntil(
                x =>
                {
                    keyInvoked++;
                    if (keyInvoked == 10)
                    {
                        throw ex;
                    }

                    return x.Trim();
                },
                x =>
                {
                    eleInvoked++;
                    return Reverse(x);
                },
                g => g.Skip(2),
                GroupByUntilCapacity,
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnNext(470, "FOO"),
            OnError<string>(480, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 480)
        );

        Assert.AreEqual(10, keyInvoked);
        Assert.AreEqual(9, eleInvoked);
    }

    [TestMethod]
    public void GroupByUntil_Capacity_Outer_EleThrow()
    {
        var keyInvoked = 0;
        var eleInvoked = 0;
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);

        var res = Scheduler.Start(() =>
            xs.GroupByUntil(
                x =>
                {
                    keyInvoked++;
                    return x.Trim();
                },
                x =>
                {
                    eleInvoked++;
                    if (eleInvoked == 10)
                    {
                        throw ex;
                    }

                    return Reverse(x);
                },
                g => g.Skip(2),
                GroupByUntilCapacity,
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnNext(470, "FOO"),
            OnError<string>(480, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 480)
        );

        Assert.AreEqual(10, keyInvoked);
        Assert.AreEqual(10, eleInvoked);
    }

    [TestMethod]
    public void GroupByUntil_Capacity_Outer_ComparerEqualsThrow()
    {
        var keyInvoked = 0;
        var eleInvoked = 0;

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler, 250, ushort.MaxValue);

        var res = Scheduler.Start(() =>
            xs.GroupByUntil(
                x =>
                {
                    keyInvoked++;
                    return x.Trim();
                },
                x =>
                {
                    eleInvoked++;
                    return Reverse(x);
                },
                g => g.Skip(2),
                GroupByUntilCapacity,
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnError<string>(310, comparer.EqualsException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );

        Assert.AreEqual(4, keyInvoked);
        Assert.AreEqual(3, eleInvoked);
    }

    [TestMethod]
    public void GroupByUntil_Capacity_Outer_ComparerGetHashCodeThrow()
    {
        var keyInvoked = 0;
        var eleInvoked = 0;

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler, ushort.MaxValue, 410);

        var res = Scheduler.Start(() =>
            xs.GroupByUntil(
                x =>
                {
                    keyInvoked++;
                    return x.Trim();
                },
                x =>
                {
                    eleInvoked++;
                    return Reverse(x);
                },
                g => g.Skip(2),
                GroupByUntilCapacity,
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnError<string>(420, comparer.HashCodeException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );

        Assert.AreEqual(8, keyInvoked);
        Assert.AreEqual(7, eleInvoked);
    }

    [TestMethod]
    public async Task GroupByUntil_Capacity_Inner_Complete()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), GroupByUntilCapacity, comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            Scheduler.ScheduleRelative(100, async () => innerSubscriptions[group.Key] = await group!.SubscribeAsync(result));
        }));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.Start();

        Assert.HasCount(5, inners);

        res["foo"].Messages.AssertEqual(
            OnCompleted<string>(320)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "), // Breaking change > v2.2 - prior to resolving a deadlock, the group would get closed prior to letting this message through
            OnCompleted<string>(420)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(480, "  zab"),
            OnNext(510, " ZAb "), // Breaking change > v2.2 - prior to resolving a deadlock, the group would get closed prior to letting this message through
            OnCompleted<string>(510)
        );

        res["qux"].Messages.AssertEqual(
            OnCompleted<string>(570)
        );

        res["FOO"].Messages.AssertEqual(
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Capacity_Inner_Complete_All()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), GroupByUntilCapacity, comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            innerSubscriptions[group.Key] = await group!.SubscribeAsync(result);
        }));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.Start();

        Assert.HasCount(5, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnCompleted<string>(310)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "),
            OnCompleted<string>(420)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB "),
            OnNext(480, "  zab"),
            OnNext(510, " ZAb "),
            OnCompleted<string>(510)
        );

        res["qux"].Messages.AssertEqual(
            OnNext(360, " xuq  "),
            OnCompleted<string>(570)
        );

        res["FOO"].Messages.AssertEqual(
            OnNext(470, " OOF"),
            OnNext(530, "    oOf    "),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Capacity_Inner_Error()
    {
        var ex1 = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnError<string>(570, ex1),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), GroupByUntilCapacity, comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            Scheduler.ScheduleRelative(100, async () => innerSubscriptions[group.Key] = await group!.SubscribeAsync(result));
        }, ex => { }));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.Start();

        Assert.HasCount(5, inners);

        res["foo"].Messages.AssertEqual(
            OnCompleted<string>(320)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "), // Breaking change > v2.2 - prior to resolving a deadlock, the group would get closed prior to letting this message through
            OnCompleted<string>(420)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(480, "  zab"),
            OnNext(510, " ZAb "), // Breaking change > v2.2 - prior to resolving a deadlock, the group would get closed prior to letting this message through
            OnCompleted<string>(510)
        );

        res["qux"].Messages.AssertEqual(
            OnError<string>(570, ex1)
        );

        res["FOO"].Messages.AssertEqual(
            OnError<string>(570, ex1)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Capacity_Inner_Dispose()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), GroupByUntilCapacity, comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            innerSubscriptions[group.Key] = await group!.SubscribeAsync(result);
        }));

        Scheduler.ScheduleAbsolute(400, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.Start();

        Assert.HasCount(4, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnCompleted<string>(310)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnNext(390, "rab   ")
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB ")
        );

        res["qux"].Messages.AssertEqual(
            OnNext(360, " xuq  ")
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Capacity_Inner_KeyThrow()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        var keyInvoked = 0;
        var ex = new Exception();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x =>
        {
            keyInvoked++;
            if (keyInvoked == 6)
            {
                throw ex;
            }

            return x.Trim();
        }, x => Reverse(x), g => g.Skip(2), GroupByUntilCapacity, comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            innerSubscriptions[group.Key] = await group!.SubscribeAsync(result);
        }, _ => { }));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.Start();

        Assert.HasCount(3, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnCompleted<string>(310)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnError<string>(360, ex)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB "),
            OnError<string>(360, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 360)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Capacity_Inner_EleThrow()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        var eleInvoked = 0;
        var ex = new Exception();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x =>
        {
            eleInvoked++;
            if (eleInvoked == 6)
            {
                throw ex;
            }

            return Reverse(x);
        }, g => g.Skip(2), GroupByUntilCapacity, comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            innerSubscriptions[group.Key] = await group!.SubscribeAsync(result);
        }, _ => { }));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.Start();

        Assert.HasCount(4, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnCompleted<string>(310)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnError<string>(360, ex)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB "),
            OnError<string>(360, ex)
        );

        res["qux"].Messages.AssertEqual(
            OnError<string>(360, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 360)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Capacity_Inner_Comparer_EqualsThrow()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler, 400, ushort.MaxValue);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), GroupByUntilCapacity, comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            innerSubscriptions[group.Key] = await group!.SubscribeAsync(result);
        }, _ => { }));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.Start();

        Assert.HasCount(4, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnCompleted<string>(310)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnNext(390, "rab   "),
            OnError<string>(420, comparer.EqualsException)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB "),
            OnError<string>(420, comparer.EqualsException)
        );

        res["qux"].Messages.AssertEqual(
            OnNext(360, " xuq  "),
            OnError<string>(420, comparer.EqualsException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Capacity_Inner_Comparer_GetHashCodeThrow()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler, ushort.MaxValue, 400);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), GroupByUntilCapacity, comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            innerSubscriptions[group.Key] = await group!.SubscribeAsync(result);
        }, _ => { }));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.Start();

        Assert.HasCount(4, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnCompleted<string>(310)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnNext(390, "rab   "),
            OnError<string>(420, comparer.HashCodeException)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB "),
            OnError<string>(420, comparer.HashCodeException)
        );

        res["qux"].Messages.AssertEqual(
            OnNext(360, " xuq  "),
            OnError<string>(420, comparer.HashCodeException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Capacity_Outer_Independence()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();
        var outerResults = Scheduler.CreateObserver<string>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), GroupByUntilCapacity, comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            await outerResults.OnNextAsync(group.Key);
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            innerSubscriptions[group.Key] = await group!.SubscribeAsync(result);
        }, outerResults.OnErrorAsync, outerResults.OnCompletedAsync));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.ScheduleAbsolute(320, async () => await outerSubscription!.DisposeAsync());

        Scheduler.Start();

        Assert.HasCount(2, inners);

        outerResults.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR")
        );

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnCompleted<string>(310)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "),
            OnCompleted<string>(420)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Capacity_Inner_Independence()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();
        var outerResults = Scheduler.CreateObserver<string>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), GroupByUntilCapacity, comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            await outerResults.OnNextAsync(group.Key);
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            innerSubscriptions[group.Key] = await group!.SubscribeAsync(result);
        }, outerResults.OnErrorAsync, outerResults.OnCompletedAsync));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.ScheduleAbsolute(320, async () => await innerSubscriptions["foo"]!.DisposeAsync());

        Scheduler.Start();

        Assert.HasCount(5, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnCompleted<string>(310)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "),
            OnCompleted<string>(420)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB "),
            OnNext(480, "  zab"),
            OnNext(510, " ZAb "),
            OnCompleted<string>(510)
        );

        res["qux"].Messages.AssertEqual(
            OnNext(360, " xuq  "),
            OnCompleted<string>(570)
        );

        res["FOO"].Messages.AssertEqual(
            OnNext(470, " OOF"),
            OnNext(530, "    oOf    "),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Capacity_Inner_Multiple_Independence()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var comparer = new GroupByComparer(Scheduler);
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();
        var outerResults = Scheduler.CreateObserver<string>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), x => Reverse(x), g => g.Skip(2), GroupByUntilCapacity, comparer));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, async group =>
        {
            await outerResults.OnNextAsync(group.Key);
            var result = Scheduler.CreateObserver<string>();
            inners[group.Key] = group;
            res[group.Key] = result;
            innerSubscriptions[group.Key] = await group!.SubscribeAsync(result);
        }, outerResults.OnErrorAsync, outerResults.OnCompletedAsync));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions.Values)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.ScheduleAbsolute(320, async () => await innerSubscriptions["foo"]!.DisposeAsync());
        Scheduler.ScheduleAbsolute(280, async () => await innerSubscriptions["baR"]!.DisposeAsync());
        Scheduler.ScheduleAbsolute(355, async () => await innerSubscriptions["Baz"]!.DisposeAsync());
        Scheduler.ScheduleAbsolute(400, async () => await innerSubscriptions["qux"]!.DisposeAsync());

        Scheduler.Start();

        Assert.HasCount(5, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnCompleted<string>(310)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab")
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB ")
        );

        res["qux"].Messages.AssertEqual(
            OnNext(360, " xuq  ")
        );

        res["FOO"].Messages.AssertEqual(
            OnNext(470, " OOF"),
            OnNext(530, "    oOf    "),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Capacity_Inner_Escape_Complete()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(310, "foO "),
            OnNext(470, "FOO "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570)
        );

        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inner = default(Seq<string>);
        var innerSubscription = default(IAsyncDisposable);
        var res = Scheduler.CreateObserver<string>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), g => g.Skip(2), GroupByUntilCapacity));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, group =>
        {
            inner = group;
        }));

        Scheduler.ScheduleAbsolute(600, async () => innerSubscription = await inner!.SubscribeAsync(res));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            await innerSubscription!.DisposeAsync();
        });

        Scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );

        res.Messages.AssertEqual(
            OnCompleted<string>(600)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Capacity_Inner_Escape_Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(310, "foO "),
            OnNext(470, "FOO "),
            OnNext(530, "    fOo    "),
            OnError<string>(570, ex)
        );

        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inner = default(Seq<string>);
        var innerSubscription = default(IAsyncDisposable);
        var res = Scheduler.CreateObserver<string>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), g => g.Skip(2), GroupByUntilCapacity));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, group =>
        {
            inner = group;
        }, _ => { }));

        Scheduler.ScheduleAbsolute(600, async () => innerSubscription = await inner!.SubscribeAsync(res));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await outerSubscription!.DisposeAsync();
            await innerSubscription!.DisposeAsync();
        });

        Scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );

        res.Messages.AssertEqual(
            OnError<string>(600, ex)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Capacity_Inner_Escape_Dispose()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(310, "foO "),
            OnNext(470, "FOO "),
            OnNext(530, "    fOo    "),
            OnError<string>(570, new Exception())
        );

        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inner = default(Seq<string>);
        var innerSubscription = default(IAsyncDisposable);
        var res = Scheduler.CreateObserver<string>();

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupByUntil(x => x.Trim(), g => g.Skip(2), GroupByUntilCapacity));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, group =>
        {
            inner = group;
        }));

        Scheduler.ScheduleAbsolute(290, async () => await outerSubscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(600, async () => innerSubscription = await inner!.SubscribeAsync(res));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await innerSubscription!.DisposeAsync();
        });

        Scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 290)
        );

        res.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public void GroupByUntil_Capacity_Default()
    {
        var keyInvoked = 0;
        var eleInvoked = 0;

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, "error"),
            OnNext(110, "error"),
            OnNext(130, "error"),
            OnNext(220, "  foo"),
            OnNext(240, " FoO "),
            OnNext(270, "baR  "),
            OnNext(310, "foO "),
            OnNext(350, " Baz   "),
            OnNext(360, "  qux "),
            OnNext(390, "   bar"),
            OnNext(420, " BAR  "),
            OnNext(470, "FOO "),
            OnNext(480, "baz  "),
            OnNext(510, " bAZ "),
            OnNext(530, "    fOo    "),
            OnCompleted<string>(570),
            OnNext(580, "error"),
            OnCompleted<string>(600),
            OnError<string>(650, new Exception())
        );

        var res = Scheduler.Start(() =>
            xs.GroupByUntil(
                x =>
                {
                    keyInvoked++;
                    return x.Trim().ToLower();
                },
                x =>
                {
                    eleInvoked++;
                    return Reverse(x);
                },
                g => g.Skip(2),
                GroupByUntilCapacity
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "bar"),
            OnNext(350, "baz"),
            OnNext(360, "qux"),
            OnNext(470, "foo"),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );

        Assert.AreEqual(12, keyInvoked);
        Assert.AreEqual(12, eleInvoked);
    }

    [TestMethod]
    public void GroupByUntil_Capacity_DurationSelector_Throws()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, "foo")
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.GroupByUntil<string, string, string>(x => x, g => { throw ex; }, GroupByUntilCapacity)
        );

        res.Messages.AssertEqual(
            OnError<Group<string, string>>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void GroupByUntil_Capacity_NullKeys_Simple_Never()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, "bar"),
            OnNext(240, "foo"),
            OnNext(310, "qux"),
            OnNext(470, "baz"),
            OnCompleted<string>(500)
        );

        var res = Scheduler.Start(() => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => Seq.Never<Unit>(), GroupByUntilCapacity).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

        res.Messages.AssertEqual(
            OnNext(220, "(null)bar"),
            OnNext(240, "FOOfoo"),
            OnNext(310, "QUXqux"),
            OnNext(470, "(null)baz"),
            OnCompleted<string>(500)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 500)
        );
    }

    [TestMethod]
    public void GroupByUntil_Capacity_NullKeys_Simple_Expire1()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, "bar"),
            OnNext(240, "foo"),
            OnNext(310, "qux"),
            OnNext(470, "baz"),
            OnCompleted<string>(500)
        );

        var n = 0;
        var res = Scheduler.Start(() => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => { if (g.Key == null) { n++; } return Seq.Timer(TimeSpan.FromTicks(50), Scheduler); }, GroupByUntilCapacity).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

        Assert.AreEqual(2, n);

        res.Messages.AssertEqual(
            OnNext(220, "(null)bar"),
            OnNext(240, "FOOfoo"),
            OnNext(310, "QUXqux"),
            OnNext(470, "(null)baz"),
            OnCompleted<string>(500)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 500)
        );
    }

    [TestMethod]
    public void GroupByUntil_Capacity_NullKeys_Simple_Expire2()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, "bar"),
            OnNext(240, "foo"),
            OnNext(310, "qux"),
            OnNext(470, "baz"),
            OnCompleted<string>(500)
        );

        var n = 0;
        var res = Scheduler.Start(() => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => { if (g.Key == null) { n++; } return Seq.Timer(TimeSpan.FromTicks(50), Scheduler).IgnoreElements(); }, GroupByUntilCapacity).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

        Assert.AreEqual(2, n);

        res.Messages.AssertEqual(
            OnNext(220, "(null)bar"),
            OnNext(240, "FOOfoo"),
            OnNext(310, "QUXqux"),
            OnNext(470, "(null)baz"),
            OnCompleted<string>(500)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 500)
        );
    }

    [TestMethod]
    public async Task GroupByUntil_Capacity_NullKeys_Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(220, "bar"),
            OnNext(240, "foo"),
            OnNext(310, "qux"),
            OnNext(470, "baz"),
            OnError<string>(500, ex)
        );

        var nullGroup = Scheduler.CreateObserver<string>();
        var err = default(Exception);

        Scheduler.ScheduleAbsolute(200, () => xs.GroupByUntil(x => x[0] == 'b' ? null : x.ToUpper(), g => Seq.Never<Unit>(), GroupByUntilCapacity).Where(g => g.Key == null).SubscribeAsync(Scheduler, async g => await g.SubscribeAsync(nullGroup), ex_ => err = ex_));
        Scheduler.Start();

        Assert.AreSame(ex, err);

        nullGroup.Messages.AssertEqual(
            OnNext(220, "bar"),
            OnNext(470, "baz"),
            OnError<string>(500, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 500)
        );
    }

    private static string Reverse(string s)
    {
        var sb = new StringBuilder();

        for (var i = s.Length - 1; i >= 0; i--)
        {
            sb.Append(s[i]);
        }

        return sb.ToString();
    }
}
