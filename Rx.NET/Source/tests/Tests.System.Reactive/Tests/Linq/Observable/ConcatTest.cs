// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class ConcatTest : ReactiveTest
    {

        [Fact]
        public void Concat_ArgumentChecking()
        {
            var xs = DummyObservable<int>.Instance;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Concat(xs, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Concat(null, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Concat((IObservable<int>[])null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Concat((IEnumerable<IObservable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Concat((IObservable<int>[])null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Concat((IEnumerable<IObservable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Concat(xs, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Concat(null, xs));
        }

        [Fact]
        public void Concat_DefaultScheduler()
        {
            var evt = new ManualResetEvent(false);

            var sum = 0;
            Observable.Concat(Observable.Return(1), Observable.Return(2), Observable.Return(3)).Subscribe(n =>
            {
                sum += n;
            }, () => evt.Set());

            evt.WaitOne();

            Assert.Equal(6, sum);
        }

        [Fact]
        public void Concat_IEofIO_DefaultScheduler()
        {
            var evt = new ManualResetEvent(false);

            IEnumerable<IObservable<int>> sources = new[] { Observable.Return(1), Observable.Return(2), Observable.Return(3) };

            var sum = 0;
            Observable.Concat(sources).Subscribe(n =>
            {
                sum += n;
            }, () => evt.Set());

            evt.WaitOne();

            Assert.Equal(6, sum);
        }

        [Fact]
        public void Concat_IEofIO_GetEnumeratorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xss = new RogueEnumerable<IObservable<int>>(ex);

            var res = scheduler.Start(() =>
                Observable.Concat(xss)
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        [Fact]
        public void Concat_IEofIO()
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
                OnCompleted<int>(30)
            );

            var xs3 = scheduler.CreateColdObservable(
                OnNext(10, 6),
                OnNext(20, 7),
                OnNext(30, 8),
                OnNext(40, 9),
                OnCompleted<int>(50)
            );

            var res = scheduler.Start(() =>
                Observable.Concat(new[] { xs1, xs2, xs3 })
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
        public void Concat_EmptyEmpty()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
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
        public void Concat_EmptyNever()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 1000)
            );
        }

        [Fact]
        public void Concat_NeverEmpty()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
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
        public void Concat_NeverNever()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
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
        public void Concat_EmptyThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 250)
            );
        }

        [Fact]
        public void Concat_ThrowEmpty()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void Concat_ThrowThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(250, new Exception())
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void Concat_ReturnEmpty()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
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
        public void Concat_EmptyReturn()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(240, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnNext(240, 2),
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
        public void Concat_ReturnNever()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 1000)
            );
        }

        [Fact]
        public void Concat_NeverReturn()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
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
        public void Concat_ReturnReturn()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(240, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnNext(220, 2),
                OnNext(240, 3),
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
        public void Concat_ThrowReturn()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(240, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void Concat_ReturnThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnNext(220, 2),
                OnError<int>(250, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 250)
            );
        }

        [Fact]
        public void Concat_SomeDataSomeData()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(225)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(225, 250)
            );
        }

        [Fact]
        public void Concat_EnumerableThrows()
        {
            var scheduler = new TestScheduler();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(225)
            );

            var ex = new Exception();
            var xss = new MockEnumerable<IObservable<int>>(scheduler, GetObservablesForConcatThrow(o, ex));

            var res = scheduler.Start(() =>
                xss.Concat()
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

        private IEnumerable<IObservable<int>> GetObservablesForConcatThrow(IObservable<int> first, Exception ex)
        {
            yield return first;
            throw ex;
        }

        [Fact]
        public void Concat_EnumerableTiming()
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
                OnCompleted<int>(80)
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
                xss.Select(xs => (IObservable<int>)xs).Concat()
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
        public void Concat_Enumerable_Dispose()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(230)
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
                xss.Select(xs => (IObservable<int>)xs).Concat(),
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

        [Fact]
        public void Concat_Optimization_DeferEvalTiming()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateColdObservable(
                OnNext(10, 4),
                OnNext(20, 5),
                OnNext(30, 6),
                OnCompleted<int>(40)
            );

            var invoked = default(long);

            var xs = o1;
            var ys = Observable.Defer(() => { invoked = scheduler.Clock; return o2; });

            var res = scheduler.Start(() =>
                xs.Concat(ys)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 270)
            );

            Assert.Equal(230, invoked);
        }

        [Fact]
        public void Concat_Optimization_DeferExceptionPropagation()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(220)
            );

            var ex = new Exception();
            var invoked = default(long);

            var xs = o1;
            var ys = Observable.Defer(new Func<IObservable<int>>(() => { invoked = scheduler.Clock; throw ex; }));

            var res = scheduler.Start(() =>
                xs.Concat(ys)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            Assert.Equal(220, invoked);
        }

#if !NO_PERF
        [Fact]
        public void Concat_TailRecursive1()
        {
            var create = 0L;
            var start = 200L;
            var end = 1000L;

            var scheduler = new TestScheduler();

            var o = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            IObservable<int> f() => Observable.Defer(() => o.Concat(f()));
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

#if !NO_THREAD && !NETCOREAPP1_1 && !NETCOREAPP1_0
        [Fact]
        public void Concat_TailRecursive2()
        {
            IObservable<int> f(int x) => Observable.Defer(() => Observable.Return(x, ThreadPoolScheduler.Instance).Concat(f(x + 1)));
            var lst = new List<int>();
            f(0).Select(x => new StackTrace().FrameCount).Take(10).ForEach(lst.Add);

            Assert.True(lst.Last() - lst.First() < 10);
        }
#endif
#endif

        [Fact]
        public void Concat_Task()
        {
            var tss = Observable.Concat(new[] { Task.Factory.StartNew(() => 1), Task.Factory.StartNew(() => 2), Task.Factory.StartNew(() => 3) }.ToObservable());

            var res = tss.ToArray().Single();

            Assert.True(res.SequenceEqual(new[] { 1, 2, 3 }));
        }

    }
}
