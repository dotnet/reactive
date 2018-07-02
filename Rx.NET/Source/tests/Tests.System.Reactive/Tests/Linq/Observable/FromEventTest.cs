// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class FromEventTest : ReactiveTest
    {

        [Fact]
        public void FromEvent_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(default, h => { }, h => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(h => h, default, h => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(h => h, h => { }, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(default, h => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(h => { }, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<int>(default, h => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<int>(h => { }, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent(default, h => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent(h => { }, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(default, h => { }, h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(h => h, default, h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(h => h, h => { }, default, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(h => h, h => { }, h => { }, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(default, h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(h => { }, default, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<Action<int>, int>(h => { }, h => { }, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<int>(default, h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<int>(h => { }, default, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent<int>(h => { }, h => { }, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent(default, h => { }, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent(h => { }, default, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromEvent(h => { }, h => { }, default));
        }

        [Fact]
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

            Assert.Equal(2, n);
        }

        [Fact]
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

            Assert.Equal(2 + 3, n);
        }

        [Fact]
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

            Assert.Equal(2 + 3, n);
        }

        [Fact]
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

            Assert.Equal(2 + 3, n);
        }

        [Fact]
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

            Assert.Equal(2 + 3, n);
        }

        #region Rx v2.0 behavior

        [Fact]
        public void FromEvent_ImplicitPublish()
        {
            var src = new MyEventSource();

            var addCount = 0;
            var remCount = 0;

            var xs = Observable.FromEventPattern<MyEventArgs>(h => { addCount++; src.Bar += h; }, h => { src.Bar -= h; remCount++; }, Scheduler.Immediate);

            Assert.Equal(0, addCount);
            Assert.Equal(0, remCount);

            src.OnBar(41);

            var fst = new List<int>();
            var d1 = xs.Subscribe(e => fst.Add(e.EventArgs.Value));

            Assert.Equal(1, addCount);
            Assert.Equal(0, remCount);

            src.OnBar(42);

            Assert.True(fst.SequenceEqual(new[] { 42 }));

            d1.Dispose();

            Assert.Equal(1, addCount);
            Assert.Equal(1, remCount);

            var snd = new List<int>();
            var d2 = xs.Subscribe(e => snd.Add(e.EventArgs.Value));

            Assert.Equal(2, addCount);
            Assert.Equal(1, remCount);

            src.OnBar(43);

            Assert.True(fst.SequenceEqual(new[] { 42 }));
            Assert.True(snd.SequenceEqual(new[] { 43 }));

            var thd = new List<int>();
            var d3 = xs.Subscribe(e => thd.Add(e.EventArgs.Value));

            Assert.Equal(2, addCount);
            Assert.Equal(1, remCount);

            src.OnBar(44);

            Assert.True(fst.SequenceEqual(new[] { 42 }));
            Assert.True(snd.SequenceEqual(new[] { 43, 44 }));
            Assert.True(thd.SequenceEqual(new[] { 44 }));

            d2.Dispose();

            Assert.Equal(2, addCount);
            Assert.Equal(1, remCount);

            src.OnBar(45);

            Assert.True(fst.SequenceEqual(new[] { 42 }));
            Assert.True(snd.SequenceEqual(new[] { 43, 44 }));
            Assert.True(thd.SequenceEqual(new[] { 44, 45 }));

            d3.Dispose();

            Assert.Equal(2, addCount);
            Assert.Equal(2, remCount);

            src.OnBar(46);

            Assert.True(fst.SequenceEqual(new[] { 42 }));
            Assert.True(snd.SequenceEqual(new[] { 43, 44 }));
            Assert.True(thd.SequenceEqual(new[] { 44, 45 }));
        }
#if !NO_THREAD
        [Fact]
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

                Assert.Null(addCtx);
                Assert.Null(remCtx);

                var d = default(IDisposable);
                var res = new List<int>();

                var s = new Thread(() =>
                {
                    beforeSubscribeNull = SynchronizationContext.Current is null;
                    d = xs.Subscribe(e => res.Add(e.EventArgs.Value));
                    afterSubscribeNull = SynchronizationContext.Current is null;
                });

                s.Start();
                s.Join();

                addEvt.WaitOne();

                subscribeOnCtx = ReferenceEquals(addCtx, ctx);

                src.OnBar(42);
                fstNext = res.SequenceEqual(new[] { 42 });

                src.OnBar(43);
                sndNext = res.SequenceEqual(new[] { 42, 43 });

                var u = new Thread(() =>
                {
                    beforeDisposeNull = SynchronizationContext.Current is null;
                    d.Dispose();
                    afterDisposeNull = SynchronizationContext.Current is null;
                });

                u.Start();
                u.Join();

                remEvt.WaitOne();

                disposeOnCtx = ReferenceEquals(remCtx, ctx);

                src.OnBar(44);
                thdNext = res.SequenceEqual(new[] { 42, 43 });
            });

            Assert.True(beforeSubscribeNull);
            Assert.True(subscribeOnCtx);
            Assert.True(afterSubscribeNull);

            Assert.True(fstNext);
            Assert.True(sndNext);
            Assert.True(thdNext);

            Assert.True(beforeDisposeNull);
            Assert.True(disposeOnCtx);
            Assert.True(afterDisposeNull);
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
#endif

        [Fact]
        public void FromEvent_Scheduler1()
        {
            RunWithScheduler((s, add, remove) => Observable.FromEvent<MyEventArgs>(h => { add(); }, h => { remove(); }, s));
        }

        [Fact]
        public void FromEvent_Scheduler2()
        {
            RunWithScheduler((s, add, remove) => Observable.FromEvent(h => { add(); }, h => { remove(); }, s));
        }

        [Fact]
        public void FromEvent_Scheduler3()
        {
            RunWithScheduler((s, add, remove) => Observable.FromEvent<Action<MyEventArgs>, MyEventArgs>(h => { add(); }, h => { remove(); }, s));
        }

        [Fact]
        public void FromEvent_Scheduler4()
        {
            RunWithScheduler((s, add, remove) => Observable.FromEvent<Action, MyEventArgs>(h => () => { }, h => { add(); }, h => { remove(); }, s));
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
}
