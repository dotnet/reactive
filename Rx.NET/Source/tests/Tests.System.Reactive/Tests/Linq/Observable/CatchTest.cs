// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class CatchTest : ReactiveTest
    {

        [Fact]
        public void Catch_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Catch<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Catch((IEnumerable<IObservable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Catch(DummyObservable<int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Catch(null, DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Catch<int, Exception>(null, _ => DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Catch<int, Exception>(DummyObservable<int>.Instance, null));
        }

        [Fact]
        public void Catch_IEofIO_GetEnumeratorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xss = new RogueEnumerable<IObservable<int>>(ex);

            var res = scheduler.Start(() =>
                Observable.Catch(xss)
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        [Fact]
        public void Catch_IEofIO()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnError<int>(40, new Exception())
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
                Observable.Catch(new[] { xs1, xs2, xs3 })
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
        public void Catch_NoErrors()
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
                o1.Catch(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(230)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void Catch_Never()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Catch(o2)
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
        public void Catch_Empty()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Catch(o2)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void Catch_Return()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Catch(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(230)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void Catch_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Catch(o2)
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
        public void Catch_Error_Never()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                o1.Catch(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 1000)
            );
        }

        [Fact]
        public void Catch_Error_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, new Exception())
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                o1.Catch(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(240, 4),
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
        public void Catch_Multiple()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(215, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(220, 3),
                OnError<int>(225, ex)
            );

            var o3 = scheduler.CreateHotObservable(
                OnNext(230, 4),
                OnCompleted<int>(235)
            );

            var res = scheduler.Start(() =>
                Observable.Catch(o1, o2, o3)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnCompleted<int>(235)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(215, 225)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(225, 235)
            );
        }

        [Fact]
        public void Catch_ErrorSpecific_Caught()
        {
            var scheduler = new TestScheduler();

            var ex = new ArgumentException("x");

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var handlerCalled = default(long?);

            var res = scheduler.Start(() =>
                o1.Catch((ArgumentException ex_) => { handlerCalled = scheduler.Clock; return o2; })
            );

            Assert.Equal(230, handlerCalled);

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
        public void Catch_ErrorSpecific_Uncaught()
        {
            var scheduler = new TestScheduler();

            var ex = new InvalidOperationException("x");

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var handlerCalled = default(long?);

            var res = scheduler.Start(() =>
                o1.Catch((ArgumentException ex_) => { handlerCalled = scheduler.Clock; return o2; })
            );

            Assert.Equal(null, handlerCalled);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void Catch_HandlerThrows()
        {
            var scheduler = new TestScheduler();

            var ex1 = new ArgumentException("x");
            var ex2 = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, ex1)
            );

            var handlerCalled = default(long?);

            var res = scheduler.Start(() =>
                o1.Catch((ArgumentException ex_) => { handlerCalled = scheduler.Clock; throw ex2; })
            );

            Assert.Equal(230, handlerCalled);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, ex2)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [Fact]
        public void Catch_Nested_OuterCatches()
        {
            var scheduler = new TestScheduler();

            var ex = new ArgumentException("x");

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(215, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(220, 3),
                OnCompleted<int>(225)
            );

            var o3 = scheduler.CreateHotObservable(
                OnNext(220, 4), //!
                OnCompleted<int>(225)
            );

            var firstHandlerCalled = default(long?);
            var secondHandlerCalled = default(long?);

            var res = scheduler.Start(() =>
                o1
                .Catch((InvalidOperationException ex_) => { firstHandlerCalled = scheduler.Clock; return o2; })
                .Catch((ArgumentException ex_) => { secondHandlerCalled = scheduler.Clock; return o3; })
            );

            Assert.Equal(null, firstHandlerCalled);
            Assert.Equal(215, secondHandlerCalled);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 4),
                OnCompleted<int>(225)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            o2.Subscriptions.AssertEqual(
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(215, 225)
            );
        }

        [Fact]
        public void Catch_Nested_InnerCatches()
        {
            var scheduler = new TestScheduler();

            var ex = new ArgumentException("x");

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(215, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(220, 3), //!
                OnCompleted<int>(225)
            );

            var o3 = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnCompleted<int>(225)
            );

            var firstHandlerCalled = default(long?);
            var secondHandlerCalled = default(long?);

            var res = scheduler.Start(() =>
                o1
                .Catch((ArgumentException ex_) => { firstHandlerCalled = scheduler.Clock; return o2; })
                .Catch((InvalidOperationException ex_) => { secondHandlerCalled = scheduler.Clock; return o3; })
            );

            Assert.Equal(215, firstHandlerCalled);
            Assert.Equal(null, secondHandlerCalled);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(225)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(215, 225)
            );

            o3.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void Catch_ThrowFromNestedCatch()
        {
            var scheduler = new TestScheduler();

            var ex1 = new ArgumentException("x1");
            var ex2 = new ArgumentException("x2");

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(215, ex1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(220, 3), //!
                OnError<int>(225, ex2)
            );

            var o3 = scheduler.CreateHotObservable(
                OnNext(230, 4),
                OnCompleted<int>(235)
            );

            var firstHandlerCalled = default(long?);
            var secondHandlerCalled = default(long?);

            var res = scheduler.Start(() =>
                o1
                .Catch((ArgumentException ex_) => { firstHandlerCalled = scheduler.Clock; Assert.True(ex1 == ex_, "Expected ex1"); return o2; })
                .Catch((ArgumentException ex_) => { secondHandlerCalled = scheduler.Clock; Assert.True(ex2 == ex_, "Expected ex2"); return o3; })
            );

            Assert.Equal(215, firstHandlerCalled);
            Assert.Equal(225, secondHandlerCalled);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnCompleted<int>(235)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(215, 225)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(225, 235)
            );
        }

        [Fact]
        public void Catch_DefaultScheduler_Binary()
        {
            var evt = new ManualResetEvent(false);

            var res = 0;
            Observable.Return(1).Catch(Observable.Return(2)).Subscribe(x =>
            {
                res = x;
                evt.Set();
            });

            evt.WaitOne();
            Assert.Equal(1, res);
        }

        [Fact]
        public void Catch_DefaultScheduler_Nary()
        {
            var evt = new ManualResetEvent(false);

            var res = 0;
            Observable.Catch(Observable.Return(1), Observable.Return(2), Observable.Return(3)).Subscribe(x =>
            {
                res = x;
                evt.Set();
            });

            evt.WaitOne();
            Assert.Equal(1, res);
        }

        [Fact]
        public void Catch_DefaultScheduler_NaryEnumerable()
        {
            var evt = new ManualResetEvent(false);

            IEnumerable<IObservable<int>> sources = new[] { Observable.Return(1), Observable.Return(2), Observable.Return(3) };

            var res = 0;
            Observable.Catch(sources).Subscribe(x =>
            {
                res = x;
                evt.Set();
            });

            evt.WaitOne();
            Assert.Equal(1, res);
        }

        [Fact]
        public void Catch_EmptyIterator()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Catch((IEnumerable<IObservable<int>>)new IObservable<int>[0])
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(200)
            );
        }

        [Fact]
        public void Catch_IteratorThrows()
        {
            var scheduler = new TestScheduler();
            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Catch(Catch_IteratorThrows_Source(ex, true))
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

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

        [Fact]
        public void Catch_EnumerableThrows()
        {
            var scheduler = new TestScheduler();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(225)
            );

            var ex = new Exception();
            var xss = new MockEnumerable<IObservable<int>>(scheduler, GetObservablesForCatchThrow(o, ex));

            var res = scheduler.Start(() =>
                xss.Catch()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(225)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            xss.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        private IEnumerable<IObservable<int>> GetObservablesForCatchThrow(IObservable<int> first, Exception ex)
        {
            yield return first;
            throw ex;
        }

        [Fact]
        public void Catch_EnumerableTiming()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2), // !
                OnNext(220, 3), // !
                OnError<int>(230, new Exception())
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
                xss.Select(xs => (IObservable<int>)xs).Catch()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 5),
                OnNext(300, 6),
                OnNext(320, 7),
                OnNext(330, 8),
                OnCompleted<int>(340)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 310)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(310, 340)
            );

            xss.Subscriptions.AssertEqual(
                Subscribe(200, 340)
            );
        }

        [Fact]
        public void Catch_Enumerable_Dispose()
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
                xss.Select(xs => (IObservable<int>)xs).Catch(),
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
        public void Catch_TailRecursive1()
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

            IObservable<int> f() => Observable.Defer(() => o.Catch(f()));
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
        public void Catch_TailRecursive2()
        {
            var f = default(Func<int, IObservable<int>>);
            f = x => Observable.Defer(() => Observable.Throw<int>(new Exception(), ThreadPoolScheduler.Instance).StartWith(x).Catch(f(x + 1)));

            var lst = new List<int>();
            f(0).Select(x => new StackTrace().FrameCount).Take(10).ForEach(lst.Add);

            Assert.True(lst.Last() - lst.First() < 10);
        }
#endif

        [Fact]
        public void Catch_TailRecursive3()
        {
            var ex = new Exception();

            var res =
                Observable.Catch(
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
                    Observable.Return(42)
                );

            Assert.Equal(42, res.Wait());
        }
#endif

    }
}
