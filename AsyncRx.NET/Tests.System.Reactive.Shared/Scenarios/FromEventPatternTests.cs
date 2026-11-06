// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.ComponentModel;
using System.Reactive;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>FromEventPattern</c> scenarios, from Rx.NET's <c>FromEventPatternTest.cs</c>.
/// </summary>
/// <remarks>
/// <para>
/// 27 of the file's 34 tests (four <c>*_ArgumentChecking</c> tests are the code-generated
/// stratum; <c>FromEventPattern_Reflection_Instance_MissingAccessors</c> is inside
/// <c>#if NET472</c> upstream, since it emits a dynamic assembly with <c>RunAndSave</c>, and
/// does not run on this platform either; <c>FromEventPattern_ConversionThrows</c> and
/// <c>FromEventPattern_Reflection_Instance_Throws</c> claim that an error raised while
/// subscribing reaches the observer as <c>OnError</c>, which is the open question on
/// <c>AsyncObservableBase.SubscribeAsync</c> from Step 28, so they wait in
/// <c>Tests.System.Reactive.Shared.Rx</c> until it is settled). The twelve virtual-time tests
/// transcribe as they are:
/// events raised from scheduled actions, converted through each overload family (the typed
/// delegate, the conversion, the sender-typed form, the <c>EventHandler</c> forms, and the
/// reflection forms over an instance or a type and an event name), projected and asserted at the
/// ticks the original expects. The handler tests subscribe through the kit's handler form and
/// dispose asynchronously where the originals call <c>Subscribe</c> and <c>Dispose</c>; the
/// eleven <c>*_Scheduler*</c> tests count the scheduler's uses through
/// <see cref="MyEventScheduler"/>, the original's <c>LocalScheduler</c> double as a shared
/// <see cref="SchedulerDouble"/>. The event-bearing doubles are the originals, top-level in
/// <c>Scenarios/</c> since <c>FromEvent</c> and <c>ToEventPattern</c> share them. The tests
/// over the static event and the static add and remove callbacks are marked
/// <c>[DoNotParallelize]</c>: the originals share that static state within one class, and here
/// two runner classes would otherwise run them at once.
/// </para>
/// </remarks>
public abstract class FromEventPatternTests : SharedReactiveTest
{
    [TestMethod]
    public void FromEventPattern_E2()
    {
        var scheduler = Scheduler;

        var fe = new FromEventPattern();

        scheduler.ScheduleAbsolute(50, () => fe.M2(1));
        scheduler.ScheduleAbsolute(150, () => fe.M2(2));
        scheduler.ScheduleAbsolute(250, () => fe.M2(3));
        scheduler.ScheduleAbsolute(350, () => fe.M2(4));
        scheduler.ScheduleAbsolute(450, () => fe.M2(5));
        scheduler.ScheduleAbsolute(1050, () => fe.M2(6));

        var results = scheduler.Start(() =>
            Seq.FromEventPattern<
                EventHandler<FromEventPattern.TestEventArgs>,
                FromEventPattern.TestEventArgs>(
                h => fe.E2 += h,
                h => fe.E2 -= h)
            .Select(evt => new { evt.Sender, evt.EventArgs }));

        results.Messages.AssertEqual(
            OnNext(250, new
            {
                Sender = (object?)fe,
                EventArgs = new FromEventPattern.TestEventArgs { Id = 3 },
            }),
            OnNext(350, new
            {
                Sender = (object?)fe,
                EventArgs = new FromEventPattern.TestEventArgs { Id = 4 },
            }),
            OnNext(450, new
            {
                Sender = (object?)fe,
                EventArgs = new FromEventPattern.TestEventArgs { Id = 5 },
            })
        );
    }

    [TestMethod]
    public void FromEventPattern_Conversion_E4()
    {
        var scheduler = Scheduler;

        var fe = new FromEventPattern();

        scheduler.ScheduleAbsolute(50, () => fe.M4(1));
        scheduler.ScheduleAbsolute(150, () => fe.M4(2));
        scheduler.ScheduleAbsolute(250, () => fe.M4(3));
        scheduler.ScheduleAbsolute(350, () => fe.M4(4));
        scheduler.ScheduleAbsolute(450, () => fe.M4(5));
        scheduler.ScheduleAbsolute(1050, () => fe.M4(6));

        var results = scheduler.Start(() =>
            Seq.FromEventPattern<Action<int>, FromEventPattern.TestEventArgs>(
                h => new Action<int>(x => h(fe, new FromEventPattern.TestEventArgs { Id = x })),
                h => fe.E4 += h,
                h => fe.E4 -= h)
            .Select(evt => new { evt.Sender, evt.EventArgs }));

        results.Messages.AssertEqual(
            OnNext(250, new
            {
                Sender = (object?)fe,
                EventArgs = new FromEventPattern.TestEventArgs { Id = 3 },
            }),
            OnNext(350, new
            {
                Sender = (object?)fe,
                EventArgs = new FromEventPattern.TestEventArgs { Id = 4 },
            }),
            OnNext(450, new
            {
                Sender = (object?)fe,
                EventArgs = new FromEventPattern.TestEventArgs { Id = 5 },
            })
        );
    }

    [TestMethod]
    public void FromEventPattern_Conversion_E5()
    {
        var scheduler = Scheduler;

        var fe = new FromEventPattern();

        scheduler.ScheduleAbsolute(50, () => fe.M5(1));
        scheduler.ScheduleAbsolute(150, () => fe.M5(2));
        scheduler.ScheduleAbsolute(250, () => fe.M5(3));
        scheduler.ScheduleAbsolute(350, () => fe.M5(4));
        scheduler.ScheduleAbsolute(450, () => fe.M5(5));
        scheduler.ScheduleAbsolute(1050, () => fe.M5(6));

        var results = scheduler.Start(() =>
            Seq.FromEventPattern(
                h => fe.E5 += h,
                h => fe.E5 -= h)
            .Select(evt => new
            {
                evt.Sender,
                EventArgs = (FromEventPattern.TestEventArgs)evt.EventArgs,
            }));

        results.Messages.AssertEqual(
            OnNext(250, new
            {
                Sender = (object?)fe,
                EventArgs = new FromEventPattern.TestEventArgs { Id = 3 },
            }),
            OnNext(350, new
            {
                Sender = (object?)fe,
                EventArgs = new FromEventPattern.TestEventArgs { Id = 4 },
            }),
            OnNext(450, new
            {
                Sender = (object?)fe,
                EventArgs = new FromEventPattern.TestEventArgs { Id = 5 },
            })
        );
    }

    [TestMethod]
    public void FromEventPattern_E2_WithSender()
    {
        var scheduler = Scheduler;

        var fe = new FromEventPattern();

        scheduler.ScheduleAbsolute(50, () => fe.M2(1));
        scheduler.ScheduleAbsolute(150, () => fe.M2(2));
        scheduler.ScheduleAbsolute(250, () => fe.M2(3));
        scheduler.ScheduleAbsolute(350, () => fe.M2(4));
        scheduler.ScheduleAbsolute(450, () => fe.M2(5));
        scheduler.ScheduleAbsolute(1050, () => fe.M2(6));

        var results = scheduler.Start(() =>
            Seq.FromEventPattern<
                EventHandler<FromEventPattern.TestEventArgs>,
                object,
                FromEventPattern.TestEventArgs>(
                h => fe.E2 += h,
                h => fe.E2 -= h)
            .Select(evt => new { evt.Sender, evt.EventArgs }));

        results.Messages.AssertEqual(
            OnNext(250, new
            {
                Sender = (object?)fe,
                EventArgs = new FromEventPattern.TestEventArgs { Id = 3 },
            }),
            OnNext(350, new
            {
                Sender = (object?)fe,
                EventArgs = new FromEventPattern.TestEventArgs { Id = 4 },
            }),
            OnNext(450, new
            {
                Sender = (object?)fe,
                EventArgs = new FromEventPattern.TestEventArgs { Id = 5 },
            })
        );
    }

    [TestMethod]
    public void FromEventPattern_AddRemove_E4()
    {
        var scheduler = Scheduler;

        var fe = new FromEventPattern();

        scheduler.ScheduleAbsolute(50, () => fe.M2(1));
        scheduler.ScheduleAbsolute(150, () => fe.M2(2));
        scheduler.ScheduleAbsolute(250, () => fe.M2(3));
        scheduler.ScheduleAbsolute(350, () => fe.M2(4));
        scheduler.ScheduleAbsolute(450, () => fe.M2(5));
        scheduler.ScheduleAbsolute(1050, () => fe.M2(6));

        var results = scheduler.Start(() =>
            Seq.FromEventPattern<FromEventPattern.TestEventArgs>(
                h => fe.E2 += h,
                h => fe.E2 -= h)
            .Select(evt => new { evt.Sender, evt.EventArgs }));

        results.Messages.AssertEqual(
            OnNext(250, new
            {
                Sender = (object?)fe,
                EventArgs = new FromEventPattern.TestEventArgs { Id = 3 },
            }),
            OnNext(350, new
            {
                Sender = (object?)fe,
                EventArgs = new FromEventPattern.TestEventArgs { Id = 4 },
            }),
            OnNext(450, new
            {
                Sender = (object?)fe,
                EventArgs = new FromEventPattern.TestEventArgs { Id = 5 },
            })
        );
    }

    [TestMethod]
    public void FromEventPattern_Reflection_Instance_InvalidVariance()
    {
        var scheduler = Scheduler;

        Assert.ThrowsExactly<InvalidOperationException>(() =>
            scheduler.Start(() =>
                Seq.FromEventPattern<CancelEventArgs>(new FromEventPattern_VarianceCheck(), "E1")));
    }

    [TestMethod]
    public async Task FromEventPattern_Reflection_Instance_VarianceArgs()
    {
        var src = new FromEventPattern_VarianceCheck();

        var es = Seq.FromEventPattern<EventArgs>(src, "E2");

        var e1 = new CancelEventArgs();
        var e2 = new CancelEventArgs();

        var lst = new List<EventPattern<EventArgs>>();
        await using (await es.SubscribeAsync(Scheduler, e => lst.Add(e)))
        {
            src.OnE2(e1);
            src.OnE2(e2);
        }

        src.OnE2(new CancelEventArgs());

        Assert.IsTrue(lst.Count == 2, "Count");
        Assert.IsTrue(ReferenceEquals(e1, lst[0].EventArgs), "First");
        Assert.IsTrue(ReferenceEquals(e2, lst[1].EventArgs), "Second");
    }

    [TestMethod]
    public async Task FromEventPattern_Reflection_Instance_VarianceSender()
    {
        var src = new FromEventPattern_VarianceCheck();

        var es = Seq.FromEventPattern<EventArgs>(src, "E3");

        var s1 = "Hello";
        var s2 = "World";

        var lst = new List<EventPattern<EventArgs>>();
        await using (await es.SubscribeAsync(Scheduler, e => lst.Add(e)))
        {
            src.OnE3(s1);
            src.OnE3(s2);
        }

        src.OnE3("Fail!");

        Assert.IsTrue(lst.Count == 2, "Count");
        Assert.IsTrue(ReferenceEquals(s1, lst[0].Sender), "First");
        Assert.IsTrue(ReferenceEquals(s2, lst[1].Sender), "Second");
    }

    [TestMethod]
    public async Task FromEventPattern_Reflection_Instance_NonGeneric()
    {
        var src = new FromEventPattern_VarianceCheck();

        var es = Seq.FromEventPattern(src, "E2");

        var e1 = new CancelEventArgs();
        var e2 = new CancelEventArgs();

        var lst = new List<EventPattern<object>>();
        await using (await es.SubscribeAsync(Scheduler, e => lst.Add(e)))
        {
            src.OnE2(e1);
            src.OnE2(e2);
        }

        src.OnE2(new CancelEventArgs());

        Assert.IsTrue(lst.Count == 2, "Count");
        Assert.IsTrue(ReferenceEquals(e1, lst[0].EventArgs), "First");
        Assert.IsTrue(ReferenceEquals(e2, lst[1].EventArgs), "Second");
    }

    [TestMethod]
    public void FromEventPattern_Reflection_Instance_E1()
    {
        var scheduler = Scheduler;

        var fe = new FromEventPattern();

        scheduler.ScheduleAbsolute(50, () => fe.M1(1));
        scheduler.ScheduleAbsolute(150, () => fe.M1(2));
        scheduler.ScheduleAbsolute(250, () => fe.M1(3));
        scheduler.ScheduleAbsolute(350, () => fe.M1(4));
        scheduler.ScheduleAbsolute(450, () => fe.M1(5));
        scheduler.ScheduleAbsolute(1050, () => fe.M1(6));

        var results = scheduler.Start(() =>
            Seq.FromEventPattern<FromEventPattern.TestEventArgs>(fe, "E1")
                .Select(evt => (evt.Sender, EventArgs: (object)evt.EventArgs))
        );

        results.Messages.AssertEqual(
            OnNext(250, (
                Sender: (object?)fe,
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 3 })),
            OnNext(350, (
                Sender: (object?)fe,
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 4 })),
            OnNext(450, (
                Sender: (object?)fe,
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 5 }))
        );
    }

    [TestMethod]
    public void FromEventPattern_Reflection_Instance_E2()
    {
        var scheduler = Scheduler;

        var fe = new FromEventPattern();

        scheduler.ScheduleAbsolute(50, () => fe.M2(1));
        scheduler.ScheduleAbsolute(150, () => fe.M2(2));
        scheduler.ScheduleAbsolute(250, () => fe.M2(3));
        scheduler.ScheduleAbsolute(350, () => fe.M2(4));
        scheduler.ScheduleAbsolute(450, () => fe.M2(5));
        scheduler.ScheduleAbsolute(1050, () => fe.M2(6));

        var results = scheduler.Start(() =>
            Seq.FromEventPattern<FromEventPattern.TestEventArgs>(fe, "E2")
                .Select(evt => (evt.Sender, EventArgs: (object)evt.EventArgs))
        );

        results.Messages.AssertEqual(
            OnNext(250, (
                Sender: (object?)fe,
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 3 })),
            OnNext(350, (
                Sender: (object?)fe,
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 4 })),
            OnNext(450, (
                Sender: (object?)fe,
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 5 }))
        );
    }

    [TestMethod]
    public void FromEventPattern_Reflection_Instance_E2_WithSender()
    {
        var scheduler = Scheduler;

        var fe = new FromEventPattern();

        scheduler.ScheduleAbsolute(50, () => fe.M2(1));
        scheduler.ScheduleAbsolute(150, () => fe.M2(2));
        scheduler.ScheduleAbsolute(250, () => fe.M2(3));
        scheduler.ScheduleAbsolute(350, () => fe.M2(4));
        scheduler.ScheduleAbsolute(450, () => fe.M2(5));
        scheduler.ScheduleAbsolute(1050, () => fe.M2(6));

        var results = scheduler.Start(() =>
            Seq.FromEventPattern<object, FromEventPattern.TestEventArgs>(fe, "E2")
                .Select(evt => (evt.Sender, EventArgs: (object)evt.EventArgs))
        );

        results.Messages.AssertEqual(
            OnNext(250, (
                Sender: (object?)fe,
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 3 })),
            OnNext(350, (
                Sender: (object?)fe,
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 4 })),
            OnNext(450, (
                Sender: (object?)fe,
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 5 }))
        );
    }

    [TestMethod]
    public void FromEventPattern_Reflection_Instance_E3()
    {
        var scheduler = Scheduler;

        var fe = new FromEventPattern();

        scheduler.ScheduleAbsolute(50, () => fe.M3(1));
        scheduler.ScheduleAbsolute(150, () => fe.M3(2));
        scheduler.ScheduleAbsolute(250, () => fe.M3(3));
        scheduler.ScheduleAbsolute(350, () => fe.M3(4));
        scheduler.ScheduleAbsolute(450, () => fe.M3(5));
        scheduler.ScheduleAbsolute(1050, () => fe.M3(6));

        var results = scheduler.Start(() =>
            Seq.FromEventPattern<FromEventPattern.TestEventArgs>(fe, "E3")
                .Select(evt => (evt.Sender, EventArgs: (object)evt.EventArgs))
        );

        results.Messages.AssertEqual(
            OnNext(250, (
                Sender: (object?)fe,
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 3 })),
            OnNext(350, (
                Sender: (object?)fe,
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 4 })),
            OnNext(450, (
                Sender: (object?)fe,
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 5 }))
        );
    }

    [TestMethod]
    [DoNotParallelize]
    public void FromEventPattern_Reflection_Static_E6()
    {
        var scheduler = Scheduler;

        scheduler.ScheduleAbsolute(50, () => FromEventPattern.M6(1));
        scheduler.ScheduleAbsolute(150, () => FromEventPattern.M6(2));
        scheduler.ScheduleAbsolute(250, () => FromEventPattern.M6(3));
        scheduler.ScheduleAbsolute(350, () => FromEventPattern.M6(4));
        scheduler.ScheduleAbsolute(450, () => FromEventPattern.M6(5));
        scheduler.ScheduleAbsolute(1050, () => FromEventPattern.M6(6));

        var results = scheduler.Start(() =>
            Seq.FromEventPattern<FromEventPattern.TestEventArgs>(typeof(FromEventPattern), "E6")
                .Select(evt => (evt.Sender, EventArgs: (object)evt.EventArgs))
        );

        results.Messages.AssertEqual(
            OnNext(250, (
                Sender: default(object?),
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 3 })),
            OnNext(350, (
                Sender: default(object?),
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 4 })),
            OnNext(450, (
                Sender: default(object?),
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 5 }))
        );
    }

    [TestMethod]
    [DoNotParallelize]
    public void FromEventPattern_Reflection_Static_E6_WithSender()
    {
        var scheduler = Scheduler;

        scheduler.ScheduleAbsolute(50, () => FromEventPattern.M6(1));
        scheduler.ScheduleAbsolute(150, () => FromEventPattern.M6(2));
        scheduler.ScheduleAbsolute(250, () => FromEventPattern.M6(3));
        scheduler.ScheduleAbsolute(350, () => FromEventPattern.M6(4));
        scheduler.ScheduleAbsolute(450, () => FromEventPattern.M6(5));
        scheduler.ScheduleAbsolute(1050, () => FromEventPattern.M6(6));

        var results = scheduler.Start(() =>
            Seq.FromEventPattern<object, FromEventPattern.TestEventArgs>(
                typeof(FromEventPattern),
                "E6")
                .Select(evt => (evt.Sender, EventArgs: (object)evt.EventArgs))
        );

        results.Messages.AssertEqual(
            OnNext(250, (
                Sender: default(object?),
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 3 })),
            OnNext(350, (
                Sender: default(object?),
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 4 })),
            OnNext(450, (
                Sender: default(object?),
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 5 }))
        );
    }

    [TestMethod]
    [DoNotParallelize]
    public void FromEventPattern_Reflection_Static_NonGeneric_E6()
    {
        var scheduler = Scheduler;

        scheduler.ScheduleAbsolute(50, () => FromEventPattern.M6(1));
        scheduler.ScheduleAbsolute(150, () => FromEventPattern.M6(2));
        scheduler.ScheduleAbsolute(250, () => FromEventPattern.M6(3));
        scheduler.ScheduleAbsolute(350, () => FromEventPattern.M6(4));
        scheduler.ScheduleAbsolute(450, () => FromEventPattern.M6(5));
        scheduler.ScheduleAbsolute(1050, () => FromEventPattern.M6(6));

        var results = scheduler.Start(() =>
            Seq.FromEventPattern(typeof(FromEventPattern), "E6")
                .Select(evt => (evt.Sender, evt.EventArgs))
        );

        results.Messages.AssertEqual(
            OnNext(250, (
                Sender: default(object?),
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 3 })),
            OnNext(350, (
                Sender: default(object?),
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 4 })),
            OnNext(450, (
                Sender: default(object?),
                EventArgs: (object)new FromEventPattern.TestEventArgs { Id = 5 }))
        );
    }

    [TestMethod]
    public async Task FromEventPattern_Scheduler1()
    {
        await RunWithScheduler((s, add, remove) =>
            Seq.FromEventPattern<MyEventArgs>(h => { add(); }, h => { remove(); }, s));
    }

    [TestMethod]
    [DoNotParallelize]
    public async Task FromEventPattern_Scheduler2()
    {
        await RunWithScheduler((s, add, remove) =>
        {
            MyEventObject.Add = add;
            MyEventObject.Remove = remove;
            return Seq.FromEventPattern<MyEventArgs>(typeof(MyEventObject), "S", s);
        });
    }

    [TestMethod]
    [DoNotParallelize]
    public async Task FromEventPattern_Scheduler3()
    {
        await RunWithScheduler((s, add, remove) =>
        {
            MyEventObject.Add = add;
            MyEventObject.Remove = remove;
            return Seq.FromEventPattern<MyEventArgs>(new MyEventObject(), "I", s);
        });
    }

    [TestMethod]
    public async Task FromEventPattern_Scheduler4()
    {
        await RunWithScheduler((s, add, remove) =>
            Seq.FromEventPattern(h => { add(); }, h => { remove(); }, s));
    }

    [TestMethod]
    [DoNotParallelize]
    public async Task FromEventPattern_Scheduler5()
    {
        await RunWithScheduler((s, add, remove) =>
        {
            MyEventObject.Add = add;
            MyEventObject.Remove = remove;
            return Seq.FromEventPattern(typeof(MyEventObject), "S", s);
        });
    }

    [TestMethod]
    [DoNotParallelize]
    public async Task FromEventPattern_Scheduler6()
    {
        await RunWithScheduler((s, add, remove) =>
        {
            MyEventObject.Add = add;
            MyEventObject.Remove = remove;
            return Seq.FromEventPattern(new MyEventObject(), "I", s);
        });
    }

    [TestMethod]
    public async Task FromEventPattern_Scheduler7()
    {
        await RunWithScheduler((s, add, remove) =>
            Seq.FromEventPattern<EventHandler<MyEventArgs>, MyEventArgs>(
                h => { add(); },
                h => { remove(); },
                s));
    }

    [TestMethod]
    public async Task FromEventPattern_Scheduler8()
    {
        await RunWithScheduler((s, add, remove) =>
            Seq.FromEventPattern<EventHandler<MyEventArgs>, MyEventArgs>(
                h => h,
                h => { add(); },
                h => { remove(); },
                s));
    }

    [TestMethod]
    [DoNotParallelize]
    public async Task FromEventPattern_Scheduler9()
    {
        await RunWithScheduler((s, add, remove) =>
        {
            MyEventObject.Add = add;
            MyEventObject.Remove = remove;
            return Seq.FromEventPattern<object, MyEventArgs>(typeof(MyEventObject), "S", s);
        });
    }

    [TestMethod]
    [DoNotParallelize]
    public async Task FromEventPattern_Scheduler10()
    {
        await RunWithScheduler((s, add, remove) =>
        {
            MyEventObject.Add = add;
            MyEventObject.Remove = remove;
            return Seq.FromEventPattern<object, MyEventArgs>(new MyEventObject(), "I", s);
        });
    }

    [TestMethod]
    public async Task FromEventPattern_Scheduler11()
    {
        await RunWithScheduler((s, add, remove) =>
            Seq.FromEventPattern<EventHandler<MyEventArgs>, object, MyEventArgs>(
                h => { add(); },
                h => { remove(); },
                s));
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
