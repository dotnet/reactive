// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

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
using Xunit;

namespace ReactiveTests.Tests
{
    public class FromEventPatternTest : ReactiveTest
    {

        #region Strongly typed

        [Fact]
        public void FromEventPattern_Conversion_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern(null, h => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern(h => { }, null));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern(null, h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern(h => { }, null, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern(h => { }, h => { }, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(null, h => { }, h => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(h => new EventHandler(h), null, h => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(h => new EventHandler(h), h => { }, null));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(null, h => { }, h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(h => new EventHandler(h), null, h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(h => new EventHandler(h), h => { }, null, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(h => new EventHandler(h), h => { }, h => { }, default));
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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
            res.Subscribe(_ => { Assert.True(false); }, ex_ => err = ex_, () => { Assert.True(false); });

            Assert.Same(ex, err);
        }

        [Fact]
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

        [Fact]
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
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(h => { }, h => { }, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(null, h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(h => { }, null, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, EventArgs>(h => { }, h => { }, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, object, EventArgs>(null, h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, object, EventArgs>(h => { }, null, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEventPattern<EventHandler, object, EventArgs>(h => { }, h => { }, default));
        }

        [Fact]
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

        [Fact]
        public void FromEventPattern_Reflection_Instance_ArgumentChecking()
        {
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(default(object), "foo"));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(new FromEventPattern_ArgCheck(), null));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(default(object), "foo", Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(new FromEventPattern_ArgCheck(), null, Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(new FromEventPattern_ArgCheck(), "foo", default));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern(new FromEventPattern_ArgCheck(), "E1"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern(new FromEventPattern_ArgCheck(), "E5"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern(new FromEventPattern_ArgCheck(), "E6"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern(new FromEventPattern_ArgCheck(), "foo"));

            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(default(object), "foo"));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(new FromEventPattern_ArgCheck(), null));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(default(object), "foo", Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(new FromEventPattern_ArgCheck(), null, Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(new FromEventPattern_ArgCheck(), "foo", default));
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
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(new FromEventPattern_ArgCheck(), "foo", default));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(new FromEventPattern_ArgCheck(), "E1"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(new FromEventPattern_ArgCheck(), "E2"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(new FromEventPattern_ArgCheck(), "E3"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(new FromEventPattern_ArgCheck(), "E4"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(new FromEventPattern_ArgCheck(), "E5"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(new FromEventPattern_ArgCheck(), "E6"));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<FromEventPattern_ArgCheck, EventArgs>(new FromEventPattern_ArgCheck(), "foo"));
        }

        [Fact]
        public void FromEventPattern_Reflection_Instance_InvalidVariance()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<CancelEventArgs>(new FromEventPattern_VarianceCheck(), "E1"));
        }

        [Fact]
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

            Assert.True(lst.Count == 2, "Count");
            Assert.True(ReferenceEquals(e1, lst[0].EventArgs), "First");
            Assert.True(ReferenceEquals(e2, lst[1].EventArgs), "Second");
        }

        [Fact]
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

            Assert.True(lst.Count == 2, "Count");
            Assert.True(ReferenceEquals(s1, lst[0].Sender), "First");
            Assert.True(ReferenceEquals(s2, lst[1].Sender), "Second");
        }

        [Fact]
        public void FromEventPattern_Reflection_Instance_NonGeneric()
        {
            var src = new FromEventPattern_VarianceCheck();

            var es = Observable.FromEventPattern(src, "E2");

            var e1 = new CancelEventArgs();
            var e2 = new CancelEventArgs();

            var lst = new List<EventPattern<object>>();
            using (es.Subscribe(e => lst.Add(e)))
            {
                src.OnE2(e1);
                src.OnE2(e2);
            }

            src.OnE2(new CancelEventArgs());

            Assert.True(lst.Count == 2, "Count");
            Assert.True(ReferenceEquals(e1, lst[0].EventArgs), "First");
            Assert.True(ReferenceEquals(e2, lst[1].EventArgs), "Second");
        }

        [Fact]
        public void FromEventPattern_Reflection_Instance_Throws()
        {
            //
            // BREAKING CHANGE v2.0 > v1.x - We no longer throw the exception synchronously as part of
            //                               the Subscribe, so it comes out through OnError now. Also,
            //                               we now unwrap TargetInvocationException objects.
            //
            var exAdd = default(Exception);
            var xs = Observable.FromEventPattern<FromEventPattern.TestEventArgs>(new FromEventPattern(), "AddThrows");
            xs.Subscribe(_ => { Assert.True(false); }, ex => exAdd = ex, () => { Assert.True(false); });
            Assert.True(exAdd is InvalidOperationException);

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
            var d = ys.Subscribe(_ => { Assert.True(false); }, ex => exRem = ex, () => { Assert.True(false); });
            ReactiveAssert.Throws<InvalidOperationException>(d.Dispose);
        }

        [Fact]
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
                Observable.FromEventPattern<FromEventPattern.TestEventArgs>(fe, "E1").Select(evt => new { Sender = evt.Sender, EventArgs = (object)evt.EventArgs })
            );

            results.Messages.AssertEqual(
                OnNext(250, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 3 } }),
                OnNext(350, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 4 } }),
                OnNext(450, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 5 } })
            );
        }

        [Fact]
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
                Observable.FromEventPattern<FromEventPattern.TestEventArgs>(fe, "E2").Select(evt => new { Sender = evt.Sender, EventArgs = (object)evt.EventArgs })
            );

            results.Messages.AssertEqual(
                OnNext(250, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 3 } }),
                OnNext(350, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 4 } }),
                OnNext(450, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 5 } })
            );
        }

        [Fact]
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
                Observable.FromEventPattern<object, FromEventPattern.TestEventArgs>(fe, "E2").Select(evt => new { Sender = evt.Sender, EventArgs = (object)evt.EventArgs })
            );

            results.Messages.AssertEqual(
                OnNext(250, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 3 } }),
                OnNext(350, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 4 } }),
                OnNext(450, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 5 } })
            );
        }

        [Fact]
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
                Observable.FromEventPattern<FromEventPattern.TestEventArgs>(fe, "E3").Select(evt => new { Sender = evt.Sender, EventArgs = (object)evt.EventArgs })
            );

            results.Messages.AssertEqual(
                OnNext(250, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 3 } }),
                OnNext(350, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 4 } }),
                OnNext(450, new { Sender = (object)fe, EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 5 } })
            );
        }

#if DESKTOPCLR && NET46
        [Fact]
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

        [Fact]
        public void FromEventPattern_Reflection_Static_ArgumentChecking()
        {
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(default, "foo"));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(typeof(FromEventPattern_ArgCheck), null));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(default, "foo", Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(typeof(FromEventPattern_ArgCheck), null, Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern(typeof(FromEventPattern_ArgCheck), "foo", default));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern(typeof(FromEventPattern_ArgCheck), "foo"));

            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(default, "foo"));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(typeof(FromEventPattern_ArgCheck), null));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(default, "foo", Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(typeof(FromEventPattern_ArgCheck), null, Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<EventArgs>(typeof(FromEventPattern_ArgCheck), "foo", default));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<EventArgs>(typeof(FromEventPattern_ArgCheck), "foo"));

            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<object, EventArgs>(default, "foo"));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<object, EventArgs>(typeof(FromEventPattern_ArgCheck), null));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<object, EventArgs>(default, "foo", Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<object, EventArgs>(typeof(FromEventPattern_ArgCheck), null, Scheduler.Default));
            ReactiveAssert.Throws</**/ArgumentNullException>(() => Observable.FromEventPattern<object, EventArgs>(typeof(FromEventPattern_ArgCheck), "foo", default));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.FromEventPattern<object, EventArgs>(typeof(FromEventPattern_ArgCheck), "foo"));
        }

        [Fact]
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
                Observable.FromEventPattern<FromEventPattern.TestEventArgs>(typeof(FromEventPattern), "E6").Select(evt => new { Sender = evt.Sender, EventArgs = (object)evt.EventArgs })
            );

            results.Messages.AssertEqual(
                OnNext(250, new { Sender = default(object), EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 3 } }),
                OnNext(350, new { Sender = default(object), EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 4 } }),
                OnNext(450, new { Sender = default(object), EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 5 } })
            );
        }

        [Fact]
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
                Observable.FromEventPattern<object, FromEventPattern.TestEventArgs>(typeof(FromEventPattern), "E6").Select(evt => new { Sender = evt.Sender, EventArgs = (object)evt.EventArgs })
            );

            results.Messages.AssertEqual(
                OnNext(250, new { Sender = default(object), EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 3 } }),
                OnNext(350, new { Sender = default(object), EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 4 } }),
                OnNext(450, new { Sender = default(object), EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 5 } })
            );
        }

        [Fact]
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
                Observable.FromEventPattern(typeof(FromEventPattern), "E6").Select(evt => new { Sender = evt.Sender, EventArgs = evt.EventArgs })
            );

            results.Messages.AssertEqual(
                OnNext(250, new { Sender = default(object), EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 3 } }),
                OnNext(350, new { Sender = default(object), EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 4 } }),
                OnNext(450, new { Sender = default(object), EventArgs = (object)new FromEventPattern.TestEventArgs { Id = 5 } })
            );
        }

        #endregion

        #endregion


        #region Rx v2.0 behavior

        [Fact]
        public void FromEventPattern_Scheduler1()
        {
            RunWithScheduler((s, add, remove) => Observable.FromEventPattern<MyEventArgs>(h => { add(); }, h => { remove(); }, s));
        }

        [Fact]
        public void FromEventPattern_Scheduler2()
        {
            RunWithScheduler((s, add, remove) =>
            {
                MyEventObject.Add = add;
                MyEventObject.Remove = remove;
                return Observable.FromEventPattern<MyEventArgs>(typeof(MyEventObject), "S", s);
            });
        }

        [Fact]
        public void FromEventPattern_Scheduler3()
        {
            RunWithScheduler((s, add, remove) =>
            {
                MyEventObject.Add = add;
                MyEventObject.Remove = remove;
                return Observable.FromEventPattern<MyEventArgs>(new MyEventObject(), "I", s);
            });
        }

        [Fact]
        public void FromEventPattern_Scheduler4()
        {
            RunWithScheduler((s, add, remove) => Observable.FromEventPattern(h => { add(); }, h => { remove(); }, s));
        }

        [Fact]
        public void FromEventPattern_Scheduler5()
        {
            RunWithScheduler((s, add, remove) =>
            {
                MyEventObject.Add = add;
                MyEventObject.Remove = remove;
                return Observable.FromEventPattern(typeof(MyEventObject), "S", s);
            });
        }

        [Fact]
        public void FromEventPattern_Scheduler6()
        {
            RunWithScheduler((s, add, remove) =>
            {
                MyEventObject.Add = add;
                MyEventObject.Remove = remove;
                return Observable.FromEventPattern(new MyEventObject(), "I", s);
            });
        }

        [Fact]
        public void FromEventPattern_Scheduler7()
        {
            RunWithScheduler((s, add, remove) => Observable.FromEventPattern<EventHandler<MyEventArgs>, MyEventArgs>(h => { add(); }, h => { remove(); }, s));
        }

        [Fact]
        public void FromEventPattern_Scheduler8()
        {
            RunWithScheduler((s, add, remove) => Observable.FromEventPattern<EventHandler<MyEventArgs>, MyEventArgs>(h => h, h => { add(); }, h => { remove(); }, s));
        }

        [Fact]
        public void FromEventPattern_Scheduler9()
        {
            RunWithScheduler((s, add, remove) =>
            {
                MyEventObject.Add = add;
                MyEventObject.Remove = remove;
                return Observable.FromEventPattern<object, MyEventArgs>(typeof(MyEventObject), "S", s);
            });
        }

        [Fact]
        public void FromEventPattern_Scheduler10()
        {
            RunWithScheduler((s, add, remove) =>
            {
                MyEventObject.Add = add;
                MyEventObject.Remove = remove;
                return Observable.FromEventPattern<object, MyEventArgs>(new MyEventObject(), "I", s);
            });
        }

        [Fact]
        public void FromEventPattern_Scheduler11()
        {
            RunWithScheduler((s, add, remove) => Observable.FromEventPattern<EventHandler<MyEventArgs>, object, MyEventArgs>(h => { add(); }, h => { remove(); }, s));
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

            Assert.Equal(0, n);
            Assert.Equal(0, a);
            Assert.Equal(0, r);

            var d1 = xs.Subscribe();
            Assert.Equal(1, n);
            Assert.Equal(1, a);
            Assert.Equal(0, r);

            var d2 = xs.Subscribe();
            Assert.Equal(1, n);
            Assert.Equal(1, a);
            Assert.Equal(0, r);

            d1.Dispose();
            Assert.Equal(1, n);
            Assert.Equal(1, a);
            Assert.Equal(0, r);

            d2.Dispose();
            Assert.Equal(2, n);
            Assert.Equal(1, a);
            Assert.Equal(1, r);
        }

        #endregion
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
                {
                    return true;
                }

                if (other == null)
                {
                    return false;
                }

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
            E1?.Invoke(this, new TestEventArgs { Id = i });
        }

        public event EventHandler<TestEventArgs> E2;

        public void M2(int i)
        {
            E2?.Invoke(this, new TestEventArgs { Id = i });
        }

        public event Action<object, TestEventArgs> E3;

        public void M3(int i)
        {
            E3?.Invoke(this, new TestEventArgs { Id = i });
        }

        public event Action<int> E4;

        public void M4(int i)
        {
            E4?.Invoke(i);
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
            E5?.Invoke(this, new TestEventArgs { Id = i });
        }

        public static event EventHandler<TestEventArgs> E6;

        public static void M6(int i)
        {
            E6?.Invoke(null, new TestEventArgs { Id = i });
        }
    }

    public delegate void MyAction(int x);

    public class FromEvent
    {
        public event Action A;

        public void OnA()
        {
            A?.Invoke();
        }

        public event Action<int> B;

        public void OnB(int x)
        {
            B?.Invoke(x);
        }

        public event MyAction C;

        public void OnC(int x)
        {
            C?.Invoke(x);
        }
    }

    internal class FromEventPattern_ArgCheck
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
            E2?.Invoke(this, args);
        }

        public void OnE3(string sender)
        {
            E3?.Invoke(sender, EventArgs.Empty);
        }
    }

    internal class MyEventObject
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

    internal class MyEventSource
    {
        public event EventHandler<MyEventArgs> Bar;

        public void OnBar(int value)
        {
            Bar?.Invoke(this, new MyEventArgs(value));
        }
    }

    internal class MyEventArgs : EventArgs
    {
        public MyEventArgs(int value)
        {
            Value = value;
        }

        public int Value { get; private set; }
    }

    internal class MyEventSyncCtx : SynchronizationContext
    {
        public int PostCount { get; private set; }

        public override void Post(SendOrPostCallback d, object state)
        {
            var old = Current;
            SetSynchronizationContext(this);
            try
            {
                PostCount++;
                d(state);
            }
            finally
            {
                SetSynchronizationContext(old);
            }
        }
    }

    internal class MyEventScheduler : LocalScheduler
    {
        private readonly Action _schedule;

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
}
