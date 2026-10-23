// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Text;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>GroupBy</c> scenarios, from Rx.NET's <c>GroupByTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// Every behavioural test from that file: 48 of its 56. Deliberately not here: the eight
/// <c>*_ArgumentChecking</c> tests (the code-generated stratum).
/// </para>
/// <para>
/// The scheduler-driven comparer the originals use is <see cref="GroupByComparer"/>, over the
/// shared scheduler handle. The tests that subscribe to groups by hand use the raw surface: an
/// outer subscription with a group handler (and, where the original routes the outer sequence's
/// error and completion to a separate observer, that observer's <c>OnErrorAsync</c> and
/// <c>OnCompletedAsync</c> as the other two handlers), inner subscriptions of testable observers,
/// and asynchronous disposal, all inside <c>async</c> scheduled actions where the original's are
/// synchronous.
/// </para>
/// </remarks>
public abstract class GroupByTests : SharedReactiveTest
{
    private const int GroupByCapacity = 1024;

    [TestMethod]
    public void GroupBy_WithKeyComparer()
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
            xs.GroupBy(x =>
            {
                keyInvoked++;
                return x.Trim();
            }, comparer).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );

        Assert.AreEqual(12, keyInvoked);
    }

    [TestMethod]
    public void GroupBy_Outer_Complete()
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
            xs.GroupBy(
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
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );

        Assert.AreEqual(12, keyInvoked);
        Assert.AreEqual(12, eleInvoked);
    }

    [TestMethod]
    public void GroupBy_Outer_Error()
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
            xs.GroupBy(
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
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnError<string>(570, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );

        Assert.AreEqual(12, keyInvoked);
        Assert.AreEqual(12, eleInvoked);
    }

    [TestMethod]
    public void GroupBy_Outer_Dispose()
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
            xs.GroupBy(
                x =>
                {
                    keyInvoked++;
                    return x.Trim();
                }, x =>
                {
                    eleInvoked++;
                    return Reverse(x);
                }, comparer
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
    public void GroupBy_Outer_KeyThrow()
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
            xs.GroupBy(
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
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnError<string>(480, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 480)
        );

        Assert.AreEqual(10, keyInvoked);
        Assert.AreEqual(9, eleInvoked);
    }

    [TestMethod]
    public void GroupBy_Outer_EleThrow()
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
            xs.GroupBy(
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
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnError<string>(480, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 480)
        );

        Assert.AreEqual(10, keyInvoked);
        Assert.AreEqual(10, eleInvoked);
    }

    [TestMethod]
    public void GroupBy_Outer_ComparerEqualsThrow()
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
            xs.GroupBy(
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
    public void GroupBy_Outer_ComparerGetHashCodeThrow()
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
            xs.GroupBy(
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
    public void GroupBy_Inner_Complete()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), comparer));

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

        Assert.HasCount(4, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(470, " OOF"),
            OnNext(530, "    oOf    "),
            OnCompleted<string>(570)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "),
            OnCompleted<string>(570)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(480, "  zab"),
            OnNext(510, " ZAb "),
            OnCompleted<string>(570)
        );

        res["qux"].Messages.AssertEqual(
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public void GroupBy_Inner_Complete_All()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), comparer));

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

        Assert.HasCount(4, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnNext(470, " OOF"),
            OnNext(530, "    oOf    "),
            OnCompleted<string>(570)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "),
            OnCompleted<string>(570)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB "),
            OnNext(480, "  zab"),
            OnNext(510, " ZAb "),
            OnCompleted<string>(570)
        );

        res["qux"].Messages.AssertEqual(
            OnNext(360, " xuq  "),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public void GroupBy_Inner_Error()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), comparer));

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

        Assert.HasCount(4, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(470, " OOF"),
            OnNext(530, "    oOf    "),
            OnError<string>(570, ex1)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "),
            OnError<string>(570, ex1)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(480, "  zab"),
            OnNext(510, " ZAb "),
            OnError<string>(570, ex1)
        );

        res["qux"].Messages.AssertEqual(
            OnError<string>(570, ex1)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public void GroupBy_Inner_Dispose()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), comparer));

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
            OnNext(310, " Oof")
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
    public void GroupBy_Inner_KeyThrow()
    {
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
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        var keyInvoked = 0;

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x =>
        {
            keyInvoked++;
            if (keyInvoked == 6)
            {
                throw ex;
            }

            return x.Trim();
        }, x => Reverse(x), comparer));

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
            OnError<string>(360, ex)
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
    public void GroupBy_Inner_EleThrow()
    {
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
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        var eleInvoked = 0;

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x =>
        {
            eleInvoked++;
            if (eleInvoked == 6)
            {
                throw ex;
            }

            return Reverse(x);
        }, comparer));

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
            OnError<string>(360, ex)
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
    public void GroupBy_Inner_Comparer_EqualsThrow()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), comparer));

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
            OnError<string>(420, comparer.EqualsException)
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
    public void GroupBy_Inner_Comparer_GetHashCodeThrow()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), comparer));

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
            OnError<string>(420, comparer.HashCodeException)
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
    public void GroupBy_Outer_Independence()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), comparer));

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
            OnNext(470, " OOF"),
            OnNext(530, "    oOf    "),
            OnCompleted<string>(570)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public void GroupBy_Inner_Independence()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), comparer));

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

        Assert.HasCount(4, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof")
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "),
            OnCompleted<string>(570)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB "),
            OnNext(480, "  zab"),
            OnNext(510, " ZAb "),
            OnCompleted<string>(570)
        );

        res["qux"].Messages.AssertEqual(
            OnNext(360, " xuq  "),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public void GroupBy_Inner_Multiple_Independence()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), comparer));

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

        Assert.HasCount(4, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof")
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

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public void GroupBy_Inner_Escape_Complete()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim()));

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
    public void GroupBy_Inner_Escape_Error()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim()));

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
    public void GroupBy_Inner_Escape_Dispose()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim()));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, group =>
        {
            inner = group;
        }));

        Scheduler.ScheduleAbsolute(400, async () => await outerSubscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(600, async () => innerSubscription = await inner!.SubscribeAsync(res));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await innerSubscription!.DisposeAsync();
        });

        Scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        res.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public void GroupBy_NullKeys_Simple()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, "bar"),
            OnNext(240, "foo"),
            OnNext(310, "qux"),
            OnNext(470, "baz"),
            OnCompleted<string>(500)
        );

        var res = Scheduler.Start(() => xs.GroupBy(x => x[0] == 'b' ? null : x.ToUpper()).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

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
    public void GroupBy_NullKeys_Error()
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

        Scheduler.ScheduleAbsolute(200, () => xs.GroupBy(x => x[0] == 'b' ? null : x.ToUpper()).Where(g => g.Key == null).SubscribeAsync(Scheduler, async g => await g.SubscribeAsync(nullGroup), ex_ => err = ex_));
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
    public void GroupBy_Capacity_WithKeyComparer()
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
            xs.GroupBy(x =>
            {
                keyInvoked++;
                return x.Trim();
            }, GroupByCapacity, comparer).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );

        Assert.AreEqual(12, keyInvoked);
    }

    [TestMethod]
    public void GroupBy_Capacity_Outer_Complete()
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
            xs.GroupBy(
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
                GroupByCapacity,
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );

        Assert.AreEqual(12, keyInvoked);
        Assert.AreEqual(12, eleInvoked);
    }

    [TestMethod]
    public void GroupBy_Capacity_Outer_Error()
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
            xs.GroupBy(
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
                GroupByCapacity,
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnError<string>(570, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );

        Assert.AreEqual(12, keyInvoked);
        Assert.AreEqual(12, eleInvoked);
    }

    [TestMethod]
    public void GroupBy_Capacity_Outer_Dispose()
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
            xs.GroupBy(
                x =>
                {
                    keyInvoked++;
                    return x.Trim();
                }, x =>
                {
                    eleInvoked++;
                    return Reverse(x);
                }, GroupByCapacity, comparer
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
    public void GroupBy_Capacity_Outer_KeyThrow()
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
            xs.GroupBy(
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
                GroupByCapacity,
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnError<string>(480, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 480)
        );

        Assert.AreEqual(10, keyInvoked);
        Assert.AreEqual(9, eleInvoked);
    }

    [TestMethod]
    public void GroupBy_Capacity_Outer_EleThrow()
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
            xs.GroupBy(
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
                GroupByCapacity,
                comparer
            ).Select(g => g.Key)
        );

        res.Messages.AssertEqual(
            OnNext(220, "foo"),
            OnNext(270, "baR"),
            OnNext(350, "Baz"),
            OnNext(360, "qux"),
            OnError<string>(480, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 480)
        );

        Assert.AreEqual(10, keyInvoked);
        Assert.AreEqual(10, eleInvoked);
    }

    [TestMethod]
    public void GroupBy_Capacity_Outer_ComparerEqualsThrow()
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
            xs.GroupBy(
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
                GroupByCapacity,
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
    public void GroupBy_Capacity_Outer_ComparerGetHashCodeThrow()
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
            xs.GroupBy(
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
                GroupByCapacity,
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
    public void GroupBy_Capacity_Inner_Complete()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), GroupByCapacity, comparer));

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

        Assert.HasCount(4, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(470, " OOF"),
            OnNext(530, "    oOf    "),
            OnCompleted<string>(570)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "),
            OnCompleted<string>(570)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(480, "  zab"),
            OnNext(510, " ZAb "),
            OnCompleted<string>(570)
        );

        res["qux"].Messages.AssertEqual(
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public void GroupBy_Capacity_Inner_Complete_All()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), GroupByCapacity, comparer));

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

        Assert.HasCount(4, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof"),
            OnNext(470, " OOF"),
            OnNext(530, "    oOf    "),
            OnCompleted<string>(570)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "),
            OnCompleted<string>(570)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB "),
            OnNext(480, "  zab"),
            OnNext(510, " ZAb "),
            OnCompleted<string>(570)
        );

        res["qux"].Messages.AssertEqual(
            OnNext(360, " xuq  "),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public void GroupBy_Capacity_Inner_Error()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), GroupByCapacity, comparer));

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

        Assert.HasCount(4, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(470, " OOF"),
            OnNext(530, "    oOf    "),
            OnError<string>(570, ex1)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "),
            OnError<string>(570, ex1)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(480, "  zab"),
            OnNext(510, " ZAb "),
            OnError<string>(570, ex1)
        );

        res["qux"].Messages.AssertEqual(
            OnError<string>(570, ex1)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public void GroupBy_Capacity_Inner_Dispose()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), GroupByCapacity, comparer));

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
            OnNext(310, " Oof")
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
    public void GroupBy_Capacity_Inner_KeyThrow()
    {
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
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        var keyInvoked = 0;

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x =>
        {
            keyInvoked++;
            if (keyInvoked == 6)
            {
                throw ex;
            }

            return x.Trim();
        }, x => Reverse(x), GroupByCapacity, comparer));

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
            OnError<string>(360, ex)
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
    public void GroupBy_Capacity_Inner_EleThrow()
    {
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
        var outer = default(Seq<Group<string, string>>);
        var outerSubscription = default(IAsyncDisposable);
        var inners = new Dictionary<string, Seq<string>>();
        var innerSubscriptions = new Dictionary<string, IAsyncDisposable>();
        var res = new Dictionary<string, TestableObserver<string>>();

        var eleInvoked = 0;

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x =>
        {
            eleInvoked++;
            if (eleInvoked == 6)
            {
                throw ex;
            }

            return Reverse(x);
        }, GroupByCapacity, comparer));

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
            OnError<string>(360, ex)
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
    public void GroupBy_Capacity_Inner_Comparer_EqualsThrow()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), GroupByCapacity, comparer));

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
            OnError<string>(420, comparer.EqualsException)
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
    public void GroupBy_Capacity_Inner_Comparer_GetHashCodeThrow()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), GroupByCapacity, comparer));

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
            OnError<string>(420, comparer.HashCodeException)
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
    public void GroupBy_Capacity_Outer_Independence()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), GroupByCapacity, comparer));

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
            OnNext(470, " OOF"),
            OnNext(530, "    oOf    "),
            OnCompleted<string>(570)
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public void GroupBy_Capacity_Inner_Independence()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), GroupByCapacity, comparer));

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

        Assert.HasCount(4, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof")
        );

        res["baR"].Messages.AssertEqual(
            OnNext(270, "  Rab"),
            OnNext(390, "rab   "),
            OnNext(420, "  RAB "),
            OnCompleted<string>(570)
        );

        res["Baz"].Messages.AssertEqual(
            OnNext(350, "   zaB "),
            OnNext(480, "  zab"),
            OnNext(510, " ZAb "),
            OnCompleted<string>(570)
        );

        res["qux"].Messages.AssertEqual(
            OnNext(360, " xuq  "),
            OnCompleted<string>(570)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public void GroupBy_Capacity_Inner_Multiple_Independence()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), x => Reverse(x), GroupByCapacity, comparer));

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

        Assert.HasCount(4, inners);

        res["foo"].Messages.AssertEqual(
            OnNext(220, "oof  "),
            OnNext(240, " OoF "),
            OnNext(310, " Oof")
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

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 570)
        );
    }

    [TestMethod]
    public void GroupBy_Capacity_Inner_Escape_Complete()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), GroupByCapacity));

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
    public void GroupBy_Capacity_Inner_Escape_Error()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), GroupByCapacity));

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
    public void GroupBy_Capacity_Inner_Escape_Dispose()
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

        Scheduler.ScheduleAbsolute(Created, () => outer = xs.GroupBy(x => x.Trim(), GroupByCapacity));

        Scheduler.ScheduleAbsolute(Subscribed, async () => outerSubscription = await outer!.SubscribeAsync(Scheduler, group =>
        {
            inner = group;
        }));

        Scheduler.ScheduleAbsolute(400, async () => await outerSubscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(600, async () => innerSubscription = await inner!.SubscribeAsync(res));

        Scheduler.ScheduleAbsolute(Disposed, async () =>
        {
            await innerSubscription!.DisposeAsync();
        });

        Scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        res.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public void GroupBy_Capacity_NullKeys_Simple()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, "bar"),
            OnNext(240, "foo"),
            OnNext(310, "qux"),
            OnNext(470, "baz"),
            OnCompleted<string>(500)
        );

        var res = Scheduler.Start(() => xs.GroupBy(x => x[0] == 'b' ? null : x.ToUpper(), GroupByCapacity).SelectMany(g => g, (g, x) => (g.Key ?? "(null)") + x));

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
    public void GroupBy_Capacity_NullKeys_Error()
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

        Scheduler.ScheduleAbsolute(200, () => xs.GroupBy(x => x[0] == 'b' ? null : x.ToUpper(), GroupByCapacity).Where(g => g.Key == null).SubscribeAsync(Scheduler, async g => await g.SubscribeAsync(nullGroup), ex_ => err = ex_));
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
