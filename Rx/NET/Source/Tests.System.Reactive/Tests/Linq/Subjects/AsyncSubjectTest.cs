// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public partial class AsyncSubjectTest : ReactiveTest
    {
        [TestMethod]
        public void Subscribe_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new AsyncSubject<int>().Subscribe(null));
        }

        [TestMethod]
        public void OnError_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new AsyncSubject<int>().OnError(null));
        }

        [TestMethod]
        public void Infinite()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 1),
                OnNext(110, 2),
                OnNext(220, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7),
                OnNext(630, 8),
                OnNext(710, 9),
                OnNext(870, 10),
                OnNext(940, 11),
                OnNext(1020, 12)
            );

            var subject = default(AsyncSubject<int>);
            var subscription = default(IDisposable);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new AsyncSubject<int>());
            scheduler.ScheduleAbsolute(200, () => subscription = xs.Subscribe(subject));
            scheduler.ScheduleAbsolute(1000, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => subscription1 = subject.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subscription2 = subject.Subscribe(results2));
            scheduler.ScheduleAbsolute(900, () => subscription3 = subject.Subscribe(results3));

            scheduler.ScheduleAbsolute(600, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
            scheduler.ScheduleAbsolute(800, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(950, () => subscription3.Dispose());

            scheduler.Start();

            results1.Messages.AssertEqual(
            );

            results2.Messages.AssertEqual(
            );

            results3.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void Finite()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 1),
                OnNext(110, 2),
                OnNext(220, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7),
                OnCompleted<int>(630),
                OnNext(640, 9),
                OnCompleted<int>(650),
                OnError<int>(660, new Exception())
            );

            var subject = default(AsyncSubject<int>);
            var subscription = default(IDisposable);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new AsyncSubject<int>());
            scheduler.ScheduleAbsolute(200, () => subscription = xs.Subscribe(subject));
            scheduler.ScheduleAbsolute(1000, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => subscription1 = subject.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subscription2 = subject.Subscribe(results2));
            scheduler.ScheduleAbsolute(900, () => subscription3 = subject.Subscribe(results3));

            scheduler.ScheduleAbsolute(600, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
            scheduler.ScheduleAbsolute(800, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(950, () => subscription3.Dispose());

            scheduler.Start();

            results1.Messages.AssertEqual(
            );

            results2.Messages.AssertEqual(
                OnNext(630, 7),
                OnCompleted<int>(630)
            );

            results3.Messages.AssertEqual(
                OnNext(900, 7),
                OnCompleted<int>(900)
            );
        }

        [TestMethod]
        public void Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 1),
                OnNext(110, 2),
                OnNext(220, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7),
                OnError<int>(630, ex),
                OnNext(640, 9),
                OnCompleted<int>(650),
                OnError<int>(660, new Exception())
            );

            var subject = default(AsyncSubject<int>);
            var subscription = default(IDisposable);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new AsyncSubject<int>());
            scheduler.ScheduleAbsolute(200, () => subscription = xs.Subscribe(subject));
            scheduler.ScheduleAbsolute(1000, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => subscription1 = subject.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subscription2 = subject.Subscribe(results2));
            scheduler.ScheduleAbsolute(900, () => subscription3 = subject.Subscribe(results3));

            scheduler.ScheduleAbsolute(600, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
            scheduler.ScheduleAbsolute(800, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(950, () => subscription3.Dispose());

            scheduler.Start();

            results1.Messages.AssertEqual(
            );

            results2.Messages.AssertEqual(
                OnError<int>(630, ex)
            );

            results3.Messages.AssertEqual(
                OnError<int>(900, ex)
            );
        }

        [TestMethod]
        public void Canceled()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<int>(630),
                OnNext(640, 9),
                OnCompleted<int>(650),
                OnError<int>(660, new Exception())
            );

            var subject = default(AsyncSubject<int>);
            var subscription = default(IDisposable);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new AsyncSubject<int>());
            scheduler.ScheduleAbsolute(200, () => subscription = xs.Subscribe(subject));
            scheduler.ScheduleAbsolute(1000, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => subscription1 = subject.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subscription2 = subject.Subscribe(results2));
            scheduler.ScheduleAbsolute(900, () => subscription3 = subject.Subscribe(results3));

            scheduler.ScheduleAbsolute(600, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
            scheduler.ScheduleAbsolute(800, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(950, () => subscription3.Dispose());

            scheduler.Start();

            results1.Messages.AssertEqual(
            );

            results2.Messages.AssertEqual(
                OnCompleted<int>(630)
            );

            results3.Messages.AssertEqual(
                OnCompleted<int>(900)
            );
        }

        [TestMethod]
        public void SubjectDisposed()
        {
            var scheduler = new TestScheduler();

            var subject = default(AsyncSubject<int>);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new AsyncSubject<int>());
            scheduler.ScheduleAbsolute(200, () => subscription1 = subject.Subscribe(results1));
            scheduler.ScheduleAbsolute(300, () => subscription2 = subject.Subscribe(results2));
            scheduler.ScheduleAbsolute(400, () => subscription3 = subject.Subscribe(results3));
            scheduler.ScheduleAbsolute(500, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(600, () => subject.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
            scheduler.ScheduleAbsolute(800, () => subscription3.Dispose());

            scheduler.ScheduleAbsolute(150, () => subject.OnNext(1));
            scheduler.ScheduleAbsolute(250, () => subject.OnNext(2));
            scheduler.ScheduleAbsolute(350, () => subject.OnNext(3));
            scheduler.ScheduleAbsolute(450, () => subject.OnNext(4));
            scheduler.ScheduleAbsolute(550, () => subject.OnNext(5));
            scheduler.ScheduleAbsolute(650, () => ReactiveAssert.Throws<ObjectDisposedException>(() => subject.OnNext(6)));
            scheduler.ScheduleAbsolute(750, () => ReactiveAssert.Throws<ObjectDisposedException>(() => subject.OnCompleted()));
            scheduler.ScheduleAbsolute(850, () => ReactiveAssert.Throws<ObjectDisposedException>(() => subject.OnError(new Exception())));
            scheduler.ScheduleAbsolute(950, () => ReactiveAssert.Throws<ObjectDisposedException>(() => subject.Subscribe()));

            scheduler.Start();

            results1.Messages.AssertEqual(
            );

            results2.Messages.AssertEqual(
            );

            results3.Messages.AssertEqual(
            );
        }

#if HAS_AWAIT
        [TestMethod]
        public void Await_Blocking()
        {
            var s = new AsyncSubject<int>();
            GetResult_Blocking(s.GetAwaiter());
        }

        [TestMethod]
        public void Await_Throw()
        {
            var s = new AsyncSubject<int>();
            GetResult_Blocking_Throw(s.GetAwaiter());
        }
#endif

        [TestMethod]
        public void GetResult_Empty()
        {
            var s = new AsyncSubject<int>();
            s.OnCompleted();
            ReactiveAssert.Throws<InvalidOperationException>(() => s.GetResult());
        }

        [TestMethod]
        public void GetResult_Blocking()
        {
            GetResult_Blocking(new AsyncSubject<int>());
        }

        private void GetResult_Blocking(AsyncSubject<int> s)
        {
            Assert.IsFalse(s.IsCompleted);

            var e = new ManualResetEvent(false);

            new Thread(() => { e.WaitOne(); s.OnNext(42); s.OnCompleted(); }).Start();

            var y = default(int);
            var t = new Thread(() => { y = s.GetResult(); });
            t.Start();

            while (t.ThreadState != ThreadState.WaitSleepJoin)
                ;

            e.Set();
            t.Join();

            Assert.AreEqual(42, y);
            Assert.IsTrue(s.IsCompleted);
        }

        [TestMethod]
        public void GetResult_Blocking_Throw()
        {
            GetResult_Blocking_Throw(new AsyncSubject<int>());
        }

        private void GetResult_Blocking_Throw(AsyncSubject<int> s)
        {
            Assert.IsFalse(s.IsCompleted);

            var e = new ManualResetEvent(false);

            var ex = new Exception();

            new Thread(() => { e.WaitOne(); s.OnError(ex); }).Start();

            var y = default(Exception);
            var t = new Thread(() =>
            {
                try
                {
                    s.GetResult();
                }
                catch (Exception ex_)
                {
                    y = ex_;
                }
            });
            t.Start();

            while (t.ThreadState != ThreadState.WaitSleepJoin)
                ;

            e.Set();
            t.Join();

            Assert.AreSame(ex, y);
            Assert.IsTrue(s.IsCompleted);
        }

#if HAS_AWAIT
        [TestMethod]
        public void GetResult_Context()
        {
            var x = new AsyncSubject<int>();

            var ctx = new MyContext();
            var e = new ManualResetEvent(false);

            new Thread(() =>
            {
                SynchronizationContext.SetSynchronizationContext(ctx);

                var a = x.GetAwaiter();
                a.OnCompleted(() =>
                {
                    e.Set();
                });
            }).Start();

            x.OnNext(42);
            x.OnCompleted();

            e.WaitOne();

            Assert.IsTrue(ctx.ran);
        }

        class MyContext : SynchronizationContext
        {
            public bool ran;

            public override void Post(SendOrPostCallback d, object state)
            {
                ran = true;
                d(state);
            }
        }
#endif

        [TestMethod]
        public void HasObservers()
        {
            var s = new AsyncSubject<int>();
            Assert.IsFalse(s.HasObservers);

            var d1 = s.Subscribe(_ => { });
            Assert.IsTrue(s.HasObservers);

            d1.Dispose();
            Assert.IsFalse(s.HasObservers);

            var d2 = s.Subscribe(_ => { });
            Assert.IsTrue(s.HasObservers);

            var d3 = s.Subscribe(_ => { });
            Assert.IsTrue(s.HasObservers);

            d2.Dispose();
            Assert.IsTrue(s.HasObservers);

            d3.Dispose();
            Assert.IsFalse(s.HasObservers);
        }

        [TestMethod]
        public void HasObservers_Dispose1()
        {
            var s = new AsyncSubject<int>();
            Assert.IsFalse(s.HasObservers);

            var d = s.Subscribe(_ => { });
            Assert.IsTrue(s.HasObservers);

            s.Dispose();
            Assert.IsFalse(s.HasObservers);

            d.Dispose();
            Assert.IsFalse(s.HasObservers);
        }

        [TestMethod]
        public void HasObservers_Dispose2()
        {
            var s = new AsyncSubject<int>();
            Assert.IsFalse(s.HasObservers);

            var d = s.Subscribe(_ => { });
            Assert.IsTrue(s.HasObservers);

            d.Dispose();
            Assert.IsFalse(s.HasObservers);

            s.Dispose();
            Assert.IsFalse(s.HasObservers);
        }

        [TestMethod]
        public void HasObservers_Dispose3()
        {
            var s = new AsyncSubject<int>();
            Assert.IsFalse(s.HasObservers);

            s.Dispose();
            Assert.IsFalse(s.HasObservers);
        }

        [TestMethod]
        public void HasObservers_OnCompleted()
        {
            var s = new AsyncSubject<int>();
            Assert.IsFalse(s.HasObservers);

            var d = s.Subscribe(_ => { });
            Assert.IsTrue(s.HasObservers);

            s.OnNext(42);
            Assert.IsTrue(s.HasObservers);

            s.OnCompleted();
            Assert.IsFalse(s.HasObservers);
        }

        [TestMethod]
        public void HasObservers_OnError()
        {
            var s = new AsyncSubject<int>();
            Assert.IsFalse(s.HasObservers);

            var d = s.Subscribe(_ => { }, ex => { });
            Assert.IsTrue(s.HasObservers);

            s.OnNext(42);
            Assert.IsTrue(s.HasObservers);

            s.OnError(new Exception());
            Assert.IsFalse(s.HasObservers);
        }
    }
}
