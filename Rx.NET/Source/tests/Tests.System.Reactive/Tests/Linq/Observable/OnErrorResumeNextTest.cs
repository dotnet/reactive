// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class OnErrorResumeNextTest : ReactiveTest
    {

        [Fact]
        public void OnErrorResumeNext_ArgumentChecking()
        {
            var xs = DummyObservable<int>.Instance;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.OnErrorResumeNext<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.OnErrorResumeNext((IEnumerable<IObservable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.OnErrorResumeNext(null, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.OnErrorResumeNext(xs, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.OnErrorResumeNext(null, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.OnErrorResumeNext(xs, null));
        }

        [Fact]
        public void OnErrorResumeNext_IEofIO_GetEnumeratorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xss = new RogueEnumerable<IObservable<int>>(ex);

            var res = scheduler.Start(() =>
                Observable.OnErrorResumeNext(xss)
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        [Fact]
        public void OnErrorResumeNext_IEofIO()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var xs2 = scheduler.CreateColdObservable(
                OnNext(10, 4),
                OnNext(20, 5),
                OnError<int>(30, new Exception())
            );

            var xs3 = scheduler.CreateColdObservable(
                OnNext(10, 6),
                OnNext(20, 7),
                OnNext(30, 8),
                OnNext(40, 9),
                OnCompleted<int>(50)
            );

            var res = scheduler.Start(() =>
                Observable.OnErrorResumeNext(new[] { xs1, xs2, xs3 })
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(250, 4),
                OnNext(260, 5),
                OnNext(280, 6),
                OnNext(290, 7),
                OnNext(300, 8),
                OnNext(310, 9),
                OnCompleted<int>(320)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(240, 270)
            );

            xs3.Subscriptions.AssertEqual(
                Subscribe(270, 320)
            );
        }

        [Fact]
        public void OnErrorResumeNext_NoErrors()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.OnErrorResumeNext(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 250)
            );
        }

        [Fact]
        public void OnErrorResumeNext_Error()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, new Exception())
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.OnErrorResumeNext(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 250)
            );
        }

        [Fact]
        public void OnErrorResumeNext_ErrorMultiple()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(220, new Exception())
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(230, 3),
                OnError<int>(240, new Exception())
            );

            var o3 = scheduler.CreateHotObservable(
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                Observable.OnErrorResumeNext(o1, o2, o3)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(230, 3),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(220, 240)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(240, 250)
            );
        }

        [Fact]
        public void OnErrorResumeNext_EmptyReturnThrowAndMore()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(205)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var o3 = scheduler.CreateHotObservable(
                OnNext(225, 3),
                OnNext(230, 4),
                OnCompleted<int>(235)
            );

            var o4 = scheduler.CreateHotObservable(
                OnError<int>(240, new Exception())
            );

            var o5 = scheduler.CreateHotObservable(
                OnNext(245, 5),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                new[] { o1, o2, o3, o4, o5 }.OnErrorResumeNext()
            );

            res.Messages.AssertEqual(
                OnNext(215, 2),
                OnNext(225, 3),
                OnNext(230, 4),
                OnNext(245, 5),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 205)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(205, 220)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(220, 235)
            );

            o4.Subscriptions.AssertEqual(
                Subscribe(235, 240)
            );

            o5.Subscriptions.AssertEqual(
                Subscribe(240, 250)
            );
        }

        [Fact]
        public void OnErrorResumeNext_LastIsntSpecial()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(220)
            );

            var o2 = scheduler.CreateHotObservable(
                OnError<int>(230, ex)
            );

            var res = scheduler.Start(() =>
                o1.OnErrorResumeNext(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(230)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(220, 230)
            );
        }

        [Fact]
        public void OnErrorResumeNext_SingleSourceDoesntThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnError<int>(230, ex)
            );

            var res = scheduler.Start(() =>
                Observable.OnErrorResumeNext(o1)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void OnErrorResumeNext_EndWithNever()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(220)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                Observable.OnErrorResumeNext(o1, o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(220, 1000)
            );
        }

        [Fact]
        public void OnErrorResumeNext_StartWithNever()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(220)
            );

            var res = scheduler.Start(() =>
                Observable.OnErrorResumeNext(o1, o2)
            );

            res.Messages.AssertEqual(
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void OnErrorResumeNext_DefaultScheduler_Binary()
        {
            var evt = new ManualResetEvent(false);

            var sum = 0;
            Observable.Return(1).OnErrorResumeNext(Observable.Return(2)).Subscribe(x =>
            {
                sum += x;
            }, () => evt.Set());

            evt.WaitOne();
            Assert.Equal(3, sum);
        }

        [Fact]
        public void OnErrorResumeNext_DefaultScheduler_Nary()
        {
            var evt = new ManualResetEvent(false);

            var sum = 0;
            Observable.OnErrorResumeNext(Observable.Return(1), Observable.Return(2), Observable.Return(3)).Subscribe(x =>
            {
                sum += x;
            }, () => evt.Set());

            evt.WaitOne();
            Assert.Equal(6, sum);
        }

        [Fact]
        public void OnErrorResumeNext_DefaultScheduler_NaryEnumerable()
        {
            var evt = new ManualResetEvent(false);

            IEnumerable<IObservable<int>> sources = new[] { Observable.Return(1), Observable.Return(2), Observable.Return(3) };

            var sum = 0;
            Observable.OnErrorResumeNext(sources).Subscribe(x =>
            {
                sum += x;
            }, () => evt.Set());

            evt.WaitOne();
            Assert.Equal(6, sum);
        }

        [Fact]
        public void OnErrorResumeNext_IteratorThrows()
        {
            var scheduler = new TestScheduler();
            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.OnErrorResumeNext(Catch_IteratorThrows_Source(ex, true))
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        [Fact]
        public void OnErrorResumeNext_EnumerableThrows()
        {
            var scheduler = new TestScheduler();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(225, new Exception())
            );

            var ex = new Exception();
            var xss = new MockEnumerable<IObservable<int>>(scheduler, GetObservablesForOnErrorResumeNextThrow(o, ex));

            var res = scheduler.Start(() =>
                xss.OnErrorResumeNext()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(225, ex)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            xss.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        private IEnumerable<IObservable<int>> GetObservablesForOnErrorResumeNextThrow(IObservable<int> first, Exception ex)
        {
            yield return first;
            throw ex;
        }

        [Fact]
        public void OnErrorResumeNext_EnumerableTiming()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2), // !
                OnNext(220, 3), // !
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateColdObservable(
                OnNext(50, 4),  // !
                OnNext(60, 5),  // !
                OnNext(70, 6),  // !
                OnError<int>(80, new Exception())
            );

            var o3 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(200, 2),
                OnNext(210, 3),
                OnNext(220, 4),
                OnNext(230, 5),
                OnNext(270, 6),
                OnNext(320, 7), // !
                OnNext(330, 8), // !
                OnCompleted<int>(340)
            );

            var xss = new MockEnumerable<ITestableObservable<int>>(scheduler, new[] { o1, o2, o3, o2 });

            var res = scheduler.Start(() =>
                xss.Select(xs => (IObservable<int>)xs).OnErrorResumeNext()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 5),
                OnNext(300, 6),
                OnNext(320, 7),
                OnNext(330, 8),
                OnNext(390, 4),
                OnNext(400, 5),
                OnNext(410, 6),
                OnCompleted<int>(420)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 310),
                Subscribe(340, 420)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(310, 340)
            );

            xss.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [Fact]
        public void OnErrorResumeNext_Enumerable_Dispose()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, new Exception())
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(200, 2),
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(320, 6),
                OnNext(330, 7),
                OnCompleted<int>(340)
            );

            var xss = new MockEnumerable<ITestableObservable<int>>(scheduler, new[] { o1, o2 });

            var res = scheduler.Start(() =>
                xss.Select(xs => (IObservable<int>)xs).OnErrorResumeNext(),
                300
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(240, 4),
                OnNext(270, 5)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 300)
            );

            xss.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

#if !NO_PERF
        [Fact]
        public void OnErrorResumeNext_TailRecursive1()
        {
            var create = 0L;
            var start = 200L;
            var end = 1000L;

            var scheduler = new TestScheduler();

            var o = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnError<int>(40, new Exception())
            );

            IObservable<int> f() => Observable.Defer(() => o.OnErrorResumeNext(f()));
            var res = scheduler.Start(() => f(), create, start, end);

            var expected = new List<Recorded<Notification<int>>>();

            var t = start;
            while (t <= end)
            {
                var n = (t - start) / 10;
                if (n % 4 != 0)
                {
                    expected.Add(OnNext(t, (int)(n % 4)));
                }

                t += 10;
            }

            res.Messages.AssertEqual(expected);
        }

#if HAS_STACKTRACE && !NO_THREAD
        [Fact]
        public void OnErrorResumeNext_TailRecursive2()
        {
            var f = default(Func<int, IObservable<int>>);
            f = x => Observable.Defer(() => Observable.Throw<int>(new Exception(), ThreadPoolScheduler.Instance).StartWith(x).OnErrorResumeNext(f(x + 1)));

            var lst = new List<int>();
            f(0).Select(x => new StackTrace().FrameCount).Take(10).ForEach(lst.Add);

            Assert.True(lst.Last() - lst.First() < 10);
        }
#endif

        [Fact]
        public void OnErrorResumeNext_TailRecursive3()
        {
            var ex = new Exception();

            var res =
                Observable.OnErrorResumeNext(
                    Observable.Return(1),
                    Observable.Defer(() =>
                    {
                        if (ex != null)
                        {
                            throw ex;
                        }

                        return Observable.Return(-2);
                    }),
                    Observable.Defer(() =>
                    {
                        if (ex != null)
                        {
                            throw ex;
                        }

                        return Observable.Return(-1);
                    }),
                    Observable.Return(2)
                )
                .SequenceEqual(new[] { 1, 2 });

            Assert.True(res.Wait());
        }
#endif

        private IEnumerable<IObservable<int>> Catch_IteratorThrows_Source(Exception ex, bool b)
        {
            if (b)
            {
                throw ex;
            }
            else
            {
                yield break;
            }
        }

    }
}
