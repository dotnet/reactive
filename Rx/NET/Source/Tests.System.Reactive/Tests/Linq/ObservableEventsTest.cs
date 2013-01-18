// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public partial class ObservableEventsTest : ReactiveTest
    {
        #region + FromEventPattern +

        #region Strongly typed

        [TestMethod]
        public void FromEventPattern_Conversion_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern(null, h => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern(h => { }, null));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern(null, h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern(h => { }, null, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern(h => { }, h => { }, default(IScheduler)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(null, h => { }, h => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(h => new EventHandler(h), null, h => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(h => new EventHandler(h), h => { }, null));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(null, h => { }, h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(h => new EventHandler(h), null, h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(h => new EventHandler(h), h => { }, null, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(h => new EventHandler(h), h => { }, h => { }, default(IScheduler)));
        }

        [TestMethod]
        public void FromEventPattern_E2()
        {
            var scheduler = new TestScheduler();

            var fe = new FromEventPattern();

            scheduler.ScheduleAbsolute(50, () => fe.M2(1));
            scheduler.ScheduleAbsolute(150, () => fe.M2(2));
            scheduler.ScheduleAbsolute(250, () => fe.M2(3));
            scheduler.ScheduleAbsolute(350, () => fe.M2(4));
            scheduler.ScheduleAbsolute(450, () => fe.M2(5));
            scheduler.ScheduleAbsolute(1050, () => fe.M2(6));

            var results = scheduler.Start(() =>
                Observable.FromEventPattern<EventHandler<FromEventPattern.TestEventArgs>, FromEventPattern.TestEventArgs>(
                    h => fe.E2 += h,
                    h => fe.E2 -= h)
                .Select(evt => new { evt.Sender, evt.EventArgs }));

            results.Messages.AssertEqual(
                OnNext(250, new { Sender = (object)fe, EventArgs = new FromEventPattern.TestEventArgs { Id = 3 } }),
                OnNext(350, new { Sender = (object)fe, EventArgs = new FromEventPattern.TestEventArgs { Id = 4 } }),
                OnNext(450, new { Sender = (object)fe, EventArgs = new FromEventPattern.TestEventArgs { Id = 5 } })
            );
        }

        [TestMethod]
        public void FromEventPattern_Conversion_E4()
        {
            var scheduler = new TestScheduler();

            var fe = new FromEventPattern();

            scheduler.ScheduleAbsolute(50, () => fe.M4(1));
            scheduler.ScheduleAbsolute(150, () => fe.M4(2));
            scheduler.ScheduleAbsolute(250, () => fe.M4(3));
            scheduler.ScheduleAbsolute(350, () => fe.M4(4));
            scheduler.ScheduleAbsolute(450, () => fe.M4(5));
            scheduler.ScheduleAbsolute(1050, () => fe.M4(6));

            var results = scheduler.Start(() =>
                Observable.FromEventPattern<Action<int>, FromEventPattern.TestEventArgs>(
                    h => new Action<int>(x => h(fe, new FromEventPattern.TestEventArgs { Id = x })),
                    h => fe.E4 += h,
                    h => fe.E4 -= h)
                .Select(evt => new { evt.Sender, evt.EventArgs }));

            results.Messages.AssertEqual(
                OnNext(250, new { Sender = (object)fe, EventArgs = new FromEventPattern.TestEventArgs { Id = 3 } }),
                OnNext(350, new { Sender = (object)fe, EventArgs = new FromEventPattern.TestEventArgs { Id = 4 } }),
                OnNext(450, new { Sender = (object)fe, EventArgs = new FromEventPattern.TestEventArgs { Id = 5 } })
            );
        }

        [TestMethod]
        public void FromEventPattern_Conversion_E5()
        {
            var scheduler = new TestScheduler();

            var fe = new FromEventPattern();

            scheduler.ScheduleAbsolute(50, () => fe.M5(1));
            scheduler.ScheduleAbsolute(150, () => fe.M5(2));
            scheduler.ScheduleAbsolute(250, () => fe.M5(3));
            scheduler.ScheduleAbsolute(350, () => fe.M5(4));
            scheduler.ScheduleAbsolute(450, () => fe.M5(5));
            scheduler.ScheduleAbsolute(1050, () => fe.M5(6));

            var results = scheduler.Start(() =>
                Observable.FromEventPattern(
                    h => fe.E5 += h,
                    h => fe.E5 -= h)
                .Select(evt => new { evt.Sender, EventArgs = (FromEventPattern.TestEventArgs)evt.EventArgs }));

            results.Messages.AssertEqual(
                OnNext(250, new { Sender = (object)fe, EventArgs = new FromEventPattern.TestEventArgs { Id = 3 } }),
                OnNext(350, new { Sender = (object)fe, EventArgs = new FromEventPattern.TestEventArgs { Id = 4 } }),
                OnNext(450, new { Sender = (object)fe, EventArgs = new FromEventPattern.TestEventArgs { Id = 5 } })
            );
        }

        [TestMethod]
        public void FromEventPattern_ConversionThrows()
        {
            var ex = new Exception();

            var fe = new FromEventPattern();

            var res =
                Observable.FromEventPattern<Action<int>, FromEventPattern.TestEventArgs>(
                    h => { throw ex; },
                    h => fe.E4 += h,
                    h => fe.E4 -= h
                );

            var err = default(Exception);
            res.Subscribe(_ => { Assert.Fail(); }, ex_ => err = ex_, () => { Assert.Fail(); });

            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void FromEventPattern_E2_WithSender()
        {
            var scheduler = new TestScheduler();

            var fe = new FromEventPattern();

            scheduler.ScheduleAbsolute(50, () => fe.M2(1));
            scheduler.ScheduleAbsolute(150, () => fe.M2(2));
            scheduler.ScheduleAbsolute(250, () => fe.M2(3));
            scheduler.ScheduleAbsolute(350, () => fe.M2(4));
            scheduler.ScheduleAbsolute(450, () => fe.M2(5));
            scheduler.ScheduleAbsolute(1050, () => fe.M2(6));

            var results = scheduler.Start(() =>
                Observable.FromEventPattern<EventHandler<FromEventPattern.TestEventArgs>, object, FromEventPattern.TestEventArgs>(
                    h => fe.E2 += h,
                    h => fe.E2 -= h)
                .Select(evt => new { evt.Sender, evt.EventArgs }));

            results.Messages.AssertEqual(
                OnNext(250, new { Sender = (object)fe, EventArgs = new FromEventPattern.TestEventArgs { Id = 3 } }),
                OnNext(350, new { Sender = (object)fe, EventArgs = new FromEventPattern.TestEventArgs { Id = 4 } }),
                OnNext(450, new { Sender = (object)fe, EventArgs = new FromEventPattern.TestEventArgs { Id = 5 } })
            );
        }

        [TestMethod]
        public void FromEventPattern_AddRemove_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(null, h => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(h => { }, null));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(null, h => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(h => { }, null));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, object, EventArgs>(null, h => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, object, EventArgs>(h => { }, null));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(null, h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(h => { }, null, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(h => { }, h => { }, default(IScheduler)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(null, h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(h => { }, null, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(h => { }, h => { }, default(IScheduler)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, object, EventArgs>(null, h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, object, EventArgs>(h => { }, null, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, object, EventArgs>(h => { }, h => { }, default(IScheduler)));
        }

        [TestMethod]
        public void FromEventPattern_AddRemove_E4()
        {
            var scheduler = new TestScheduler();

            var fe = new FromEventPattern();

            scheduler.ScheduleAbsolute(50, () => fe.M2(1));
            scheduler.ScheduleAbsolute(150, () => fe.M2(2));
            scheduler.ScheduleAbsolute(250, () => fe.M2(3));
            scheduler.ScheduleAbsolute(350, () => fe.M2(4));
            scheduler.ScheduleAbsolute(450, () => fe.M2(5));
            scheduler.ScheduleAbsolute(1050, () => fe.M2(6));

            var results = scheduler.Start(() =>
                Observable.FromEventPattern<FromEventPattern.TestEventArgs>(
                    h => fe.E2 += h,
                    h => fe.E2 -= h)
                .Select(evt => new { evt.Sender, evt.EventArgs }));

            results.Messages.AssertEqual(
                OnNext(250, new { Sender = (object)fe, EventArgs = new FromEventPattern.TestEventArgs { Id = 3 } }),
                OnNext(350, new { Sender = (object)fe, EventArgs = new FromEventPattern.TestEventArgs { Id = 4 } }),
                OnNext(450, new { Sender = (object)fe, EventArgs = new FromEventPattern.TestEventArgs { Id = 5 } })
            );
        }

        #endregion

        #region Reflection

        #region Instance events

        [TestMethod]
        public void FromEventPattern_Reflection_Instance_ArgumentChecking()
        {
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(default(object), "foo"));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(new FromEventPattern_ArgCheck(), null));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(default(object), "foo", Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(new FromEventPattern_ArgCheck(), null, Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(new FromEventPattern_ArgCheck(), "foo", default(IScheduler)));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern(new FromEventPattern_ArgCheck(), "E1"));
#if !NO_EVENTARGS_CONSTRAINT
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern(new FromEventPattern_ArgCheck(), "E2"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern(new FromEventPattern_ArgCheck(), "E3"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern(new FromEventPattern_ArgCheck(), "E4"));
#endif
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern(new FromEventPattern_ArgCheck(), "E5"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern(new FromEventPattern_ArgCheck(), "E6"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern(new FromEventPattern_ArgCheck(), "foo"));

            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(default(object), "foo"));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(new FromEventPattern_ArgCheck(), null));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(default(object), "foo", Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(new FromEventPattern_ArgCheck(), null, Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(new FromEventPattern_ArgCheck(), "foo", default(IScheduler)));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<EventArgs>(new FromEventPattern_ArgCheck(), "E1"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<EventArgs>(new FromEventPattern_ArgCheck(), "E2"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<EventArgs>(new FromEventPattern_ArgCheck(), "E3"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<EventArgs>(new FromEventPattern_ArgCheck(), "E4"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<EventArgs>(new FromEventPattern_ArgCheck(), "E5"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<EventArgs>(new FromEventPattern_ArgCheck(), "E6"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<EventArgs>(new FromEventPattern_ArgCheck(), "foo"));

            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(default(object), "foo"));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(new FromEventPattern_ArgCheck(), null));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(default(object), "foo", Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(new FromEventPattern_ArgCheck(), null, Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(new FromEventPattern_ArgCheck(), "foo", default(IScheduler)));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(new FromEventPattern_ArgCheck(), "E1"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(new FromEventPattern_ArgCheck(), "E2"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(new FromEventPattern_ArgCheck(), "E3"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(new FromEventPattern_ArgCheck(), "E4"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(new FromEventPattern_ArgCheck(), "E5"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(new FromEventPattern_ArgCheck(), "E6"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(new FromEventPattern_ArgCheck(), "foo"));
        }

        [TestMethod]
        public void FromEventPattern_Reflection_Instance_InvalidVariance()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<CancelEventArgs>(new FromEventPattern_VarianceCheck(), "E1"));
        }

        [TestMethod]
        public void FromEventPattern_Reflection_Instance_VarianceArgs()
        {
            var src = new FromEventPattern_VarianceCheck();

            var es = Observable.FromEventPattern<EventArgs>(src, "E2");

            var e1 = new CancelEventArgs();
            var e2 = new CancelEventArgs();

            var lst = new List<EventPattern<EventArgs>>();
            using (es.Subscribe(e => lst.Add(e)))
            {
                src.OnE2(e1);
                src.OnE2(e2);
            }

            src.OnE2(new CancelEventArgs());

            Assert.IsTrue(lst.Count == 2, "Count");
            Assert.AreSame(e1, lst[0].EventArgs, "First");
            Assert.AreSame(e2, lst[1].EventArgs, "Second");
        }

        [TestMethod]
        public void FromEventPattern_Reflection_Instance_VarianceSender()
        {
            var src = new FromEventPattern_VarianceCheck();

            var es = Observable.FromEventPattern<EventArgs>(src, "E3");

            var s1 = "Hello";
            var s2 = "World";

            var lst = new List<EventPattern<EventArgs>>();
            using (es.Subscribe(e => lst.Add(e)))
            {
                src.OnE3(s1);
                src.OnE3(s2);
            }

            src.OnE3("Fail!");

            Assert.IsTrue(lst.Count == 2, "Count");
            Assert.AreSame(s1, lst[0].Sender, "First");
            Assert.AreSame(s2, lst[1].Sender, "Second");
        }

        [TestMethod]
        public void FromEventPattern_Reflection_Instance_NonGeneric()
        {
            var src = new FromEventPattern_VarianceCheck();

            var es = Observable.FromEventPattern(src, "E2");

            var e1 = new CancelEventArgs();
            var e2 = new CancelEventArgs();

#if !NO_EVENTARGS_CONSTRAINT
            var lst = new List<EventPattern<EventArgs>>();
#else
            var lst = new List<EventPattern<object>>();
#endif
            using (es.Subscribe(e => lst.Add(e)))
            {
                src.OnE2(e1);
                src.OnE2(e2);
            }

            src.OnE2(new CancelEventArgs());

            Assert.IsTrue(lst.Count == 2, "Count");
            Assert.AreSame(e1, lst[0].EventArgs, "First");
            Assert.AreSame(e2, lst[1].EventArgs, "Second");
        }

        [TestMethod]
        public void FromEventPattern_Reflection_Instance_Throws()
        {
            //
            // BREAKING CHANGE v2.0 > v1.x - We no longer throw the exception synchronously as part of
            //                               the Subscribe, so it comes out through OnError now. Also,
            //                               we now unwrap TargetInvocationException objects.
            //
            var exAdd = default(Exception);
            var xs = Observable.FromEventPattern<FromEventPattern.TestEventArgs>(new FromEventPattern(), "AddThrows");
            xs.Subscribe(_ => { Assert.Fail(); }, ex => exAdd = ex, () => { Assert.Fail(); });
            Assert.IsTrue(exAdd is InvalidOperationException);

            //
            // Notice the exception propgation behavior is asymmetric by design. Below, the Dispose
            // call will throw synchronously, merely because we happen to use FromEventPattern from a
            // thread with no SynchronizationContext, causing the Immediate scheduler to be used.
            //
            // See AddHandler in FromEvent.cs for more information on the design rationale of the
            // asymmetry you see here.
            //
            var exRem = default(Exception);
            var ys = Observable.FromEventPattern<FromEventPattern.TestEventArgs>(new FromEventPattern(), "RemoveThrows");
            var d = ys.Subscribe(_ => { Assert.Fail(); }, ex => exRem = ex, () => { Assert.Fail(); });
            ReactiveAssert.Throws<InvalidOperationException>(d.Dispose);
        }

        [TestMethod]
        public void FromEventPattern_Reflection_Instance_E1()
        {
            var scheduler = new TestScheduler();

            var fe = new FromEventPattern();

            scheduler.ScheduleAbsolute(50, () => fe.M1(1));
            scheduler.ScheduleAbsolute(150, () => fe.M1(2));
            scheduler.ScheduleAbsolute(250, () => fe.M1(3));
            scheduler.ScheduleAbsolute(350, () => fe.M1(4));
            scheduler.ScheduleAbsolute(450, () => fe.M1(5));
            scheduler.ScheduleAbsolute(1050, () => fe.M1(6));

            var results = scheduler.Start(() =>
                Observable.FromEventPattern<FromEventPattern.TestEventArgs>(fe, "E1").Select(evt => new { Sender = (object)evt.Sender, EventArgs = (object)evt.EventArgs })
            );

            results.Messages.AssertEqual(
                OnNext(250, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 3 } }),
                OnNext(350, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 4 } }),
                OnNext(450, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 5 } })
            );
        }

        [TestMethod]
        public void FromEventPattern_Reflection_Instance_E2()
        {
            var scheduler = new TestScheduler();

            var fe = new FromEventPattern();

            scheduler.ScheduleAbsolute(50, () => fe.M2(1));
            scheduler.ScheduleAbsolute(150, () => fe.M2(2));
            scheduler.ScheduleAbsolute(250, () => fe.M2(3));
            scheduler.ScheduleAbsolute(350, () => fe.M2(4));
            scheduler.ScheduleAbsolute(450, () => fe.M2(5));
            scheduler.ScheduleAbsolute(1050, () => fe.M2(6));

            var results = scheduler.Start(() =>
                Observable.FromEventPattern<FromEventPattern.TestEventArgs>(fe, "E2").Select(evt => new { Sender = (object)evt.Sender, EventArgs = (object)evt.EventArgs })
            );

            results.Messages.AssertEqual(
                OnNext(250, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 3 } }),
                OnNext(350, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 4 } }),
                OnNext(450, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 5 } })
            );
        }

        [TestMethod]
        public void FromEventPattern_Reflection_Instance_E2_WithSender()
        {
            var scheduler = new TestScheduler();

            var fe = new FromEventPattern();

            scheduler.ScheduleAbsolute(50, () => fe.M2(1));
            scheduler.ScheduleAbsolute(150, () => fe.M2(2));
            scheduler.ScheduleAbsolute(250, () => fe.M2(3));
            scheduler.ScheduleAbsolute(350, () => fe.M2(4));
            scheduler.ScheduleAbsolute(450, () => fe.M2(5));
            scheduler.ScheduleAbsolute(1050, () => fe.M2(6));

            var results = scheduler.Start(() =>
                Observable.FromEventPattern<object, FromEventPattern.TestEventArgs>(fe, "E2").Select(evt => new { Sender = (object)evt.Sender, EventArgs = (object)evt.EventArgs })
            );

            results.Messages.AssertEqual(
                OnNext(250, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 3 } }),
                OnNext(350, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 4 } }),
                OnNext(450, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 5 } })
            );
        }

        [TestMethod]
        public void FromEventPattern_Reflection_Instance_E3()
        {
            var scheduler = new TestScheduler();

            var fe = new FromEventPattern();

            scheduler.ScheduleAbsolute(50, () => fe.M3(1));
            scheduler.ScheduleAbsolute(150, () => fe.M3(2));
            scheduler.ScheduleAbsolute(250, () => fe.M3(3));
            scheduler.ScheduleAbsolute(350, () => fe.M3(4));
            scheduler.ScheduleAbsolute(450, () => fe.M3(5));
            scheduler.ScheduleAbsolute(1050, () => fe.M3(6));

            var results = scheduler.Start(() =>
                Observable.FromEventPattern<FromEventPattern.TestEventArgs>(fe, "E3").Select(evt => new { Sender = (object)evt.Sender, EventArgs = (object)evt.EventArgs })
            );

            results.Messages.AssertEqual(
                OnNext(250, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 3 } }),
                OnNext(350, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 4 } }),
                OnNext(450, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 5 } })
            );
        }

#if DESKTOPCLR
        [TestMethod]
        public void FromEventPattern_Reflection_Instance_MissingAccessors()
        {
            var asm = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("EventsTest"), System.Reflection.Emit.AssemblyBuilderAccess.RunAndSave);
            var mod = asm.DefineDynamicModule("Events");
            var tpe = mod.DefineType("FromEvent");

            var ev1 = tpe.DefineEvent("Bar", (EventAttributes)MethodAttributes.Public, typeof(Action));
            var add = tpe.DefineMethod("add_Bar", MethodAttributes.Public, CallingConventions.Standard, typeof(void), new Type[0]);
            var ge1 = add.GetILGenerator();
            ge1.Emit(System.Reflection.Emit.OpCodes.Ret);
            ev1.SetAddOnMethod(add);

            var ev2 = tpe.DefineEvent("Foo", (EventAttributes)MethodAttributes.Public, typeof(Action));
            var rem = tpe.DefineMethod("remove_Foo", MethodAttributes.Public, CallingConventions.Standard, typeof(void), new Type[0]);
            var ge2 = rem.GetILGenerator();
            ge2.Emit(System.Reflection.Emit.OpCodes.Ret);
            ev2.SetRemoveOnMethod(rem);

            var evt = tpe.DefineEvent("Evt", (EventAttributes)MethodAttributes.Public, typeof(Action));
            evt.SetAddOnMethod(add);
            evt.SetRemoveOnMethod(rem);

            var res = tpe.CreateType();
            var obj = Activator.CreateInstance(res);

            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<EventArgs>(obj, "Bar"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<EventArgs>(obj, "Foo"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<EventArgs>(obj, "Evt"));
        }
#endif

        #endregion

        #region Static events

        [TestMethod]
        public void FromEventPattern_Reflection_Static_ArgumentChecking()
        {
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(default(Type), "foo"));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(typeof(FromEventPattern_ArgCheck), null));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(default(Type), "foo", Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(typeof(FromEventPattern_ArgCheck), null, Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(typeof(FromEventPattern_ArgCheck), "foo", default(IScheduler)));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern(typeof(FromEventPattern_ArgCheck), "foo"));

            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(default(Type), "foo"));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(typeof(FromEventPattern_ArgCheck), null));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(default(Type), "foo", Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(typeof(FromEventPattern_ArgCheck), null, Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(typeof(FromEventPattern_ArgCheck), "foo", default(IScheduler)));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<EventArgs>(typeof(FromEventPattern_ArgCheck), "foo"));

            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<object, EventArgs>(default(Type), "foo"));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<object, EventArgs>(typeof(FromEventPattern_ArgCheck), null));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<object, EventArgs>(default(Type), "foo", Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<object, EventArgs>(typeof(FromEventPattern_ArgCheck), null, Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<object, EventArgs>(typeof(FromEventPattern_ArgCheck), "foo", default(IScheduler)));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<object, EventArgs>(typeof(FromEventPattern_ArgCheck), "foo"));
        }

        [TestMethod]
        public void FromEventPattern_Reflection_Static_E6()
        {
            var scheduler = new TestScheduler();

            scheduler.ScheduleAbsolute(50, () => FromEventPattern.M6(1));
            scheduler.ScheduleAbsolute(150, () => FromEventPattern.M6(2));
            scheduler.ScheduleAbsolute(250, () => FromEventPattern.M6(3));
            scheduler.ScheduleAbsolute(350, () => FromEventPattern.M6(4));
            scheduler.ScheduleAbsolute(450, () => FromEventPattern.M6(5));
            scheduler.ScheduleAbsolute(1050, () => FromEventPattern.M6(6));

            var results = scheduler.Start(() =>
                Observable.FromEventPattern<FromEventPattern.TestEventArgs>(typeof(FromEventPattern), "E6").Select(evt => new { Sender = (object)evt.Sender, EventArgs = (object)evt.EventArgs })
            );

            results.Messages.AssertEqual(
                OnNext(250, new { Sender = default(object), EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 3 } }),
                OnNext(350, new { Sender = default(object), EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 4 } }),
                OnNext(450, new { Sender = default(object), EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 5 } })
            );
        }

        [TestMethod]
        public void FromEventPattern_Reflection_Static_E6_WithSender()
        {
            var scheduler = new TestScheduler();

            scheduler.ScheduleAbsolute(50, () => FromEventPattern.M6(1));
            scheduler.ScheduleAbsolute(150, () => FromEventPattern.M6(2));
            scheduler.ScheduleAbsolute(250, () => FromEventPattern.M6(3));
            scheduler.ScheduleAbsolute(350, () => FromEventPattern.M6(4));
            scheduler.ScheduleAbsolute(450, () => FromEventPattern.M6(5));
            scheduler.ScheduleAbsolute(1050, () => FromEventPattern.M6(6));

            var results = scheduler.Start(() =>
                Observable.FromEventPattern<object, FromEventPattern.TestEventArgs>(typeof(FromEventPattern), "E6").Select(evt => new { Sender = (object)evt.Sender, EventArgs = (object)evt.EventArgs })
            );

            results.Messages.AssertEqual(
                OnNext(250, new { Sender = default(object), EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 3 } }),
                OnNext(350, new { Sender = default(object), EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 4 } }),
                OnNext(450, new { Sender = default(object), EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 5 } })
            );
        }

        [TestMethod]
        public void FromEventPattern_Reflection_Static_NonGeneric_E6()
        {
            var scheduler = new TestScheduler();

            scheduler.ScheduleAbsolute(50, () => FromEventPattern.M6(1));
            scheduler.ScheduleAbsolute(150, () => FromEventPattern.M6(2));
            scheduler.ScheduleAbsolute(250, () => FromEventPattern.M6(3));
            scheduler.ScheduleAbsolute(350, () => FromEventPattern.M6(4));
            scheduler.ScheduleAbsolute(450, () => FromEventPattern.M6(5));
            scheduler.ScheduleAbsolute(1050, () => FromEventPattern.M6(6));

            var results = scheduler.Start(() =>
                Observable.FromEventPattern(typeof(FromEventPattern), "E6").Select(evt => new { Sender = (object)evt.Sender, EventArgs = (object)evt.EventArgs })
            );

            results.Messages.AssertEqual(
                OnNext(250, new { Sender = default(object), EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 3 } }),
                OnNext(350, new { Sender = default(object), EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 4 } }),
                OnNext(450, new { Sender = default(object), EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 5 } })
            );
        }

        #endregion

        #endregion

        #endregion

        #region + FromEvent +

        [TestMethod]
        public void FromEvent_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(default(Func<Action<int>, Action<int>>), h => { }, h => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(h => h, default(Action<Action<int>>), h => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(h => h, h => { }, default(Action<Action<int>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(default(Action<Action<int>>), h => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(h => { }, default(Action<Action<int>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<int>(default(Action<Action<int>>), h => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<int>(h => { }, default(Action<Action<int>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent(default(Action<Action>), h => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent(h => { }, default(Action<Action>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(default(Func<Action<int>, Action<int>>), h => { }, h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(h => h, default(Action<Action<int>>), h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(h => h, h => { }, default(Action<Action<int>>), Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(h => h, h => { }, h => { }, default(IScheduler)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(default(Action<Action<int>>), h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(h => { }, default(Action<Action<int>>), Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(h => { }, h => { }, default(IScheduler)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<int>(default(Action<Action<int>>), h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<int>(h => { }, default(Action<Action<int>>), Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<int>(h => { }, h => { }, default(IScheduler)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent(default(Action<Action>), h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent(h => { }, default(Action<Action>), Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent(h => { }, h => { }, default(IScheduler)));
        }

        [TestMethod]
        public void FromEvent_Action()
        {
            var fe = new FromEvent();

            var xs = Observable.FromEvent(h => fe.A += h, h => fe.A -= h);

            fe.OnA();

            var n = 0;
            var d = xs.Subscribe(_ => n++);

            fe.OnA();
            fe.OnA();

            d.Dispose();

            fe.OnA();

            Assert.AreEqual(2, n);
        }

        [TestMethod]
        public void FromEvent_ActionOfInt()
        {
            var fe = new FromEvent();

            var xs = Observable.FromEvent<int>(h => fe.B += h, h => fe.B -= h);

            fe.OnB(1);

            var n = 0;
            var d = xs.Subscribe(x => n += x);

            fe.OnB(2);
            fe.OnB(3);

            d.Dispose();

            fe.OnB(4);

            Assert.AreEqual(2 + 3, n);
        }

        [TestMethod]
        public void FromEvent_ActionOfInt_SpecifiedExplicitly()
        {
            var fe = new FromEvent();

            var xs = Observable.FromEvent<Action<int>, int>(h => fe.B += h, h => fe.B -= h);

            fe.OnB(1);

            var n = 0;
            var d = xs.Subscribe(x => n += x);

            fe.OnB(2);
            fe.OnB(3);

            d.Dispose();

            fe.OnB(4);

            Assert.AreEqual(2 + 3, n);
        }

        [TestMethod]
        public void FromEvent_ActionOfInt_SpecifiedExplicitly_TrivialConversion()
        {
            var fe = new FromEvent();

            var xs = Observable.FromEvent<Action<int>, int>(h => h, h => fe.B += h, h => fe.B -= h);

            fe.OnB(1);

            var n = 0;
            var d = xs.Subscribe(x => n += x);

            fe.OnB(2);
            fe.OnB(3);

            d.Dispose();

            fe.OnB(4);

            Assert.AreEqual(2 + 3, n);
        }

        [TestMethod]
        public void FromEvent_MyAction()
        {
            var fe = new FromEvent();

            var xs = Observable.FromEvent<MyAction, int>(h => new MyAction(h), h => fe.C += h, h => fe.C -= h);

            fe.OnC(1);

            var n = 0;
            var d = xs.Subscribe(x => n += x);

            fe.OnC(2);
            fe.OnC(3);

            d.Dispose();

            fe.OnC(4);

            Assert.AreEqual(2 + 3, n);
        }

        #endregion

        #region Rx v2.0 behavior

        [TestMethod]
        public void FromEvent_ImplicitPublish()
        {
            var src = new MyEventSource();

            var addCount = 0;
            var remCount = 0;

            var xs = Observable.FromEventPattern<MyEventArgs>(h => { addCount++; src.Bar += h; }, h => { src.Bar -= h; remCount++; }, Scheduler.Immediate);

            Assert.AreEqual(0, addCount);
            Assert.AreEqual(0, remCount);

            src.OnBar(41);

            var fst = new List<int>();
            var d1 = xs.Subscribe(e => fst.Add(e.EventArgs.Value));

            Assert.AreEqual(1, addCount);
            Assert.AreEqual(0, remCount);

            src.OnBar(42);

            Assert.IsTrue(fst.SequenceEqual(new[] { 42 }));

            d1.Dispose();

            Assert.AreEqual(1, addCount);
            Assert.AreEqual(1, remCount);

            var snd = new List<int>();
            var d2 = xs.Subscribe(e => snd.Add(e.EventArgs.Value));

            Assert.AreEqual(2, addCount);
            Assert.AreEqual(1, remCount);

            src.OnBar(43);

            Assert.IsTrue(fst.SequenceEqual(new[] { 42 }));
            Assert.IsTrue(snd.SequenceEqual(new[] { 43 }));

            var thd = new List<int>();
            var d3 = xs.Subscribe(e => thd.Add(e.EventArgs.Value));

            Assert.AreEqual(2, addCount);
            Assert.AreEqual(1, remCount);

            src.OnBar(44);

            Assert.IsTrue(fst.SequenceEqual(new[] { 42 }));
            Assert.IsTrue(snd.SequenceEqual(new[] { 43, 44 }));
            Assert.IsTrue(thd.SequenceEqual(new[] { 44 }));

            d2.Dispose();

            Assert.AreEqual(2, addCount);
            Assert.AreEqual(1, remCount);

            src.OnBar(45);

            Assert.IsTrue(fst.SequenceEqual(new[] { 42 }));
            Assert.IsTrue(snd.SequenceEqual(new[] { 43, 44 }));
            Assert.IsTrue(thd.SequenceEqual(new[] { 44, 45 }));

            d3.Dispose();

            Assert.AreEqual(2, addCount);
            Assert.AreEqual(2, remCount);

            src.OnBar(46);

            Assert.IsTrue(fst.SequenceEqual(new[] { 42 }));
            Assert.IsTrue(snd.SequenceEqual(new[] { 43, 44 }));
            Assert.IsTrue(thd.SequenceEqual(new[] { 44, 45 }));
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

                var xs = Observable.FromEventPattern<MyEventArgs>(h => { addCtx = SynchronizationContext.Current; src.Bar += h; addEvt.Set(); }, h => { remCtx = SynchronizationContext.Current; src.Bar -= h; remEvt.Set(); });

                Assert.IsNull(addCtx);
                Assert.IsNull(remCtx);

                var d = default(IDisposable);
                var res = new List<int>();

                var s = new Thread(() =>
                {
                    beforeSubscribeNull = object.ReferenceEquals(SynchronizationContext.Current, null);
                    d = xs.Subscribe(e => res.Add(e.EventArgs.Value));
                    afterSubscribeNull = object.ReferenceEquals(SynchronizationContext.Current, null);
                });

                s.Start();
                s.Join();

                addEvt.WaitOne();

                subscribeOnCtx = object.ReferenceEquals(addCtx, ctx);

                src.OnBar(42);
                fstNext = res.SequenceEqual(new[] { 42 });

                src.OnBar(43);
                sndNext = res.SequenceEqual(new[] { 42, 43 });

                var u = new Thread(() =>
                {
                    beforeDisposeNull = object.ReferenceEquals(SynchronizationContext.Current, null);
                    d.Dispose();
                    afterDisposeNull = object.ReferenceEquals(SynchronizationContext.Current, null);
                });

                u.Start();
                u.Join();

                remEvt.WaitOne();

                disposeOnCtx = object.ReferenceEquals(remCtx, ctx);

                src.OnBar(44);
                thdNext = res.SequenceEqual(new[] { 42, 43 });
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

        [TestMethod]
        public void FromEvent_Scheduler1()
        {
            RunWithScheduler((s, add, remove) => Observable.FromEvent<MyEventArgs>(h => { add(); }, h => { remove(); }, s));
        }

        [TestMethod]
        public void FromEvent_Scheduler2()
        {
            RunWithScheduler((s, add, remove) => Observable.FromEvent(h => { add(); }, h => { remove(); }, s));
        }

        [TestMethod]
        public void FromEvent_Scheduler3()
        {
            RunWithScheduler((s, add, remove) => Observable.FromEvent<Action<MyEventArgs>, MyEventArgs>(h => { add(); }, h => { remove(); }, s));
        }

        [TestMethod]
        public void FromEvent_Scheduler4()
        {
            RunWithScheduler((s, add, remove) => Observable.FromEvent<Action, MyEventArgs>(h => () => { }, h => { add(); }, h => { remove(); }, s));
        }

        [TestMethod]
        public void FromEventPattern_Scheduler1()
        {
            RunWithScheduler((s, add, remove) => Observable.FromEventPattern<MyEventArgs>(h => { add(); }, h => { remove(); }, s));
        }

        [TestMethod]
        public void FromEventPattern_Scheduler2()
        {
            RunWithScheduler((s, add, remove) =>
            {
                MyEventObject.Add = add;
                MyEventObject.Remove = remove;
                return Observable.FromEventPattern<MyEventArgs>(typeof(MyEventObject), "S", s);
            });
        }

        [TestMethod]
        public void FromEventPattern_Scheduler3()
        {
            RunWithScheduler((s, add, remove) =>
            {
                MyEventObject.Add = add;
                MyEventObject.Remove = remove;
                return Observable.FromEventPattern<MyEventArgs>(new MyEventObject(), "I", s);
            });
        }

        [TestMethod]
        public void FromEventPattern_Scheduler4()
        {
            RunWithScheduler((s, add, remove) => Observable.FromEventPattern(h => { add(); }, h => { remove(); }, s));
        }

        [TestMethod]
        public void FromEventPattern_Scheduler5()
        {
            RunWithScheduler((s, add, remove) =>
            {
                MyEventObject.Add = add;
                MyEventObject.Remove = remove;
                return Observable.FromEventPattern(typeof(MyEventObject), "S", s);
            });
        }

        [TestMethod]
        public void FromEventPattern_Scheduler6()
        {
            RunWithScheduler((s, add, remove) =>
            {
                MyEventObject.Add = add;
                MyEventObject.Remove = remove;
                return Observable.FromEventPattern(new MyEventObject(), "I", s);
            });
        }

        [TestMethod]
        public void FromEventPattern_Scheduler7()
        {
            RunWithScheduler((s, add, remove) => Observable.FromEventPattern<EventHandler<MyEventArgs>, MyEventArgs>(h => { add(); }, h => { remove(); }, s));
        }

        [TestMethod]
        public void FromEventPattern_Scheduler8()
        {
            RunWithScheduler((s, add, remove) => Observable.FromEventPattern<EventHandler<MyEventArgs>, MyEventArgs>(h => h, h => { add(); }, h => { remove(); }, s));
        }

        [TestMethod]
        public void FromEventPattern_Scheduler9()
        {
            RunWithScheduler((s, add, remove) =>
            {
                MyEventObject.Add = add;
                MyEventObject.Remove = remove;
                return Observable.FromEventPattern<object, MyEventArgs>(typeof(MyEventObject), "S", s);
            });
        }

        [TestMethod]
        public void FromEventPattern_Scheduler10()
        {
            RunWithScheduler((s, add, remove) =>
            {
                MyEventObject.Add = add;
                MyEventObject.Remove = remove;
                return Observable.FromEventPattern<object, MyEventArgs>(new MyEventObject(), "I", s);
            });
        }

        [TestMethod]
        public void FromEventPattern_Scheduler11()
        {
            RunWithScheduler((s, add, remove) => Observable.FromEventPattern<EventHandler<MyEventArgs>, object, MyEventArgs>(h => { add(); }, h => { remove(); }, s));
        }

        class MyEventObject
        {
            public static Action Add;
            public static Action Remove;

            public event EventHandler<MyEventArgs> I
            {
                add { Add(); }
                remove { Remove(); }
            }

            public static event EventHandler<MyEventArgs> S
            {
                add { Add(); }
                remove { Remove(); }
            }
        }

        private void RunWithScheduler<T>(Func<IScheduler, Action, Action, IObservable<T>> run)
        {
            var n = 0;
            var a = 0;
            var r = 0;

            var s = new MyEventScheduler(() => n++);

            var add = new Action(() => a++);
            var rem = new Action(() => r++);

            var xs = run(s, add, rem);

            Assert.AreEqual(0, n);
            Assert.AreEqual(0, a);
            Assert.AreEqual(0, r);

            var d1 = xs.Subscribe();
            Assert.AreEqual(1, n);
            Assert.AreEqual(1, a);
            Assert.AreEqual(0, r);

            var d2 = xs.Subscribe();
            Assert.AreEqual(1, n);
            Assert.AreEqual(1, a);
            Assert.AreEqual(0, r);

            d1.Dispose();
            Assert.AreEqual(1, n);
            Assert.AreEqual(1, a);
            Assert.AreEqual(0, r);

            d2.Dispose();
            Assert.AreEqual(2, n);
            Assert.AreEqual(1, a);
            Assert.AreEqual(1, r);
        }

        class MyEventSource
        {
            public event EventHandler<MyEventArgs> Bar;

            public void OnBar(int value)
            {
                var bar = Bar;
                if (bar != null)
                    bar(this, new MyEventArgs(value));
            }
        }

        class MyEventArgs : EventArgs
        {
            public MyEventArgs(int value)
            {
                Value = value;
            }

            public int Value { get; private set; }
        }

        class MyEventSyncCtx : SynchronizationContext
        {
            public int PostCount { get; private set; }

            public override void Post(SendOrPostCallback d, object state)
            {
                var old = SynchronizationContext.Current;
                SynchronizationContext.SetSynchronizationContext(this);
                try
                {
                    PostCount++;
                    d(state);
                }
                finally
                {
                    SynchronizationContext.SetSynchronizationContext(old);
                }
            }
        }

        class MyEventScheduler : LocalScheduler
        {
            private Action _schedule;

            public MyEventScheduler(Action schedule)
            {
                _schedule = schedule;
            }

            public override IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
            {
                _schedule();

                return action(this, state);
            }

            public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                throw new NotImplementedException();
            }
        }


        #endregion

        #region <| Helpers |>

        class FromEventPattern_ArgCheck
        {
#pragma warning disable 67
            public event Action E1;
            public event Action<int, int> E2;
            public event Action<object, object> E3;
            public event Action<object, int> E4;
            public event Func<object, EventArgs, int> E5;
            public event Action<EventArgs> E6;
#pragma warning restore 67
        }

        public class FromEventPattern_VarianceCheck
        {
#pragma warning disable 67
            public event EventHandler<EventArgs> E1;
            public event EventHandler<CancelEventArgs> E2;
            public event Action<string, EventArgs> E3;
#pragma warning restore 67

            public void OnE2(CancelEventArgs args)
            {
                var e = E2;
                if (e != null)
                    e(this, args);
            }

            public void OnE3(string sender)
            {
                var e = E3;
                if (e != null)
                    e(sender, EventArgs.Empty);
            }
        }

        public class FromEventPattern
        {
            [DebuggerDisplay("{Id}")]
            public class TestEventArgs : EventArgs, IEquatable<TestEventArgs>
            {
                public int Id { get; set; }

                public override string ToString()
                {
                    return Id.ToString();
                }

                public bool Equals(TestEventArgs other)
                {
                    if (other == this)
                        return true;
                    if (other == null)
                        return false;
                    return other.Id == Id;
                }

                public override bool Equals(object obj)
                {
                    return Equals(obj as TestEventArgs);
                }

                public override int GetHashCode()
                {
                    return Id;
                }
            }

            public delegate void TestEventHandler(object sender, TestEventArgs eventArgs);

            public event TestEventHandler E1;

            public void M1(int i)
            {
                var e = E1;
                if (e != null)
                    e(this, new TestEventArgs { Id = i });
            }

            public event EventHandler<TestEventArgs> E2;

            public void M2(int i)
            {
                var e = E2;
                if (e != null)
                    e(this, new TestEventArgs { Id = i });
            }

            public event Action<object, TestEventArgs> E3;

            public void M3(int i)
            {
                var e = E3;
                if (e != null)
                    e(this, new TestEventArgs { Id = i });
            }

            public event Action<int> E4;

            public void M4(int i)
            {
                var e = E4;
                if (e != null)
                    e(i);
            }

            public event TestEventHandler AddThrows
            {
                add { throw new InvalidOperationException(); }
                remove { }
            }

            public event TestEventHandler RemoveThrows
            {
                add { }
                remove { throw new InvalidOperationException(); }
            }

            public event EventHandler E5;
            public void M5(int i)
            {
                var e = E5;
                if (e != null)
                    e(this, new FromEventPattern.TestEventArgs { Id = i });
            }

            public static event EventHandler<TestEventArgs> E6;

            public static void M6(int i)
            {
                var e = E6;
                if (e != null)
                    e(null, new TestEventArgs { Id = i });
            }
        }

        public delegate void MyAction(int x);

        public class FromEvent
        {
            public event Action A;

            public void OnA()
            {
                var a = A;
                if (a != null)
                    a();
            }

            public event Action<int> B;

            public void OnB(int x)
            {
                var b = B;
                if (b != null)
                    b(x);
            }

            public event MyAction C;

            public void OnC(int x)
            {
                var c = C;
                if (c != null)
                    c(x);
            }
        }

        #endregion
    }
}
