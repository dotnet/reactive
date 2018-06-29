// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class ForEachAsyncTest : ReactiveTest
    {

        [Fact]
        public void ForEachAsync_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEachAsync(default(IObservable<int>), x => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEachAsync(Observable.Never<int>(), default(Action<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEachAsync(default(IObservable<int>), x => { }, CancellationToken.None));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEachAsync(Observable.Never<int>(), default(Action<int>), CancellationToken.None));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEachAsync(default(IObservable<int>), (x, i) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEachAsync(Observable.Never<int>(), default(Action<int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEachAsync(default(IObservable<int>), (x, i) => { }, CancellationToken.None));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEachAsync(Observable.Never<int>(), default(Action<int, int>), CancellationToken.None));
        }

        [Fact]
        public void ForEachAsync_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5)
            );

            var task = default(Task);
            var cts = new CancellationTokenSource();
            var list = new List<Recorded<int>>();

            scheduler.ScheduleAbsolute(150, () => task = xs.ForEachAsync(x => list.Add(new Recorded<int>(scheduler.Clock, x)), cts.Token));

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(150)
            );

            list.AssertEqual(
                new Recorded<int>(200, 2),
                new Recorded<int>(300, 3),
                new Recorded<int>(400, 4),
                new Recorded<int>(500, 5)
            );

            Assert.Equal(TaskStatus.WaitingForActivation, task.Status);
        }

        [Fact]
        public void ForEachAsync_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnCompleted<int>(600)
            );

            var task = default(Task);
            var cts = new CancellationTokenSource();
            var list = new List<Recorded<int>>();

            scheduler.ScheduleAbsolute(150, () => task = xs.ForEachAsync(x => list.Add(new Recorded<int>(scheduler.Clock, x)), cts.Token));

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(150, 600)
            );

            list.AssertEqual(
                new Recorded<int>(200, 2),
                new Recorded<int>(300, 3),
                new Recorded<int>(400, 4),
                new Recorded<int>(500, 5)
            );

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void ForEachAsync_Error()
        {
            var scheduler = new TestScheduler();

            var exception = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnError<int>(600, exception)
            );

            var task = default(Task);
            var cts = new CancellationTokenSource();
            var list = new List<Recorded<int>>();

            scheduler.ScheduleAbsolute(150, () => task = xs.ForEachAsync(x => list.Add(new Recorded<int>(scheduler.Clock, x)), cts.Token));

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(150, 600)
            );

            list.AssertEqual(
                new Recorded<int>(200, 2),
                new Recorded<int>(300, 3),
                new Recorded<int>(400, 4),
                new Recorded<int>(500, 5)
            );

            Assert.Equal(TaskStatus.Faulted, task.Status);
            Assert.Same(exception, task.Exception.InnerException);
        }

        [Fact]
        public void ForEachAsync_Throw()
        {
            var scheduler = new TestScheduler();

            var exception = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnCompleted<int>(600)
            );

            var task = default(Task);
            var cts = new CancellationTokenSource();
            var list = new List<Recorded<int>>();

            scheduler.ScheduleAbsolute(150, () => task = xs.ForEachAsync(x =>
            {
                if (scheduler.Clock > 400)
                {
                    throw exception;
                }

                list.Add(new Recorded<int>(scheduler.Clock, x));
            }, cts.Token));

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(150, 500)
            );

            list.AssertEqual(
                new Recorded<int>(200, 2),
                new Recorded<int>(300, 3),
                new Recorded<int>(400, 4)
            );

            Assert.Equal(TaskStatus.Faulted, task.Status);
            Assert.Same(exception, task.Exception.InnerException);
        }

        [Fact]
        public void ForEachAsync_CancelDuring()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnCompleted<int>(600)
            );

            var task = default(Task);
            var cts = new CancellationTokenSource();
            var list = new List<Recorded<int>>();

            scheduler.ScheduleAbsolute(150, () => task = xs.ForEachAsync(x => list.Add(new Recorded<int>(scheduler.Clock, x)), cts.Token));
            scheduler.ScheduleAbsolute(350, () => cts.Cancel());

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(150, 350)
            );

            list.AssertEqual(
                new Recorded<int>(200, 2),
                new Recorded<int>(300, 3)
            );

            Assert.Equal(TaskStatus.Canceled, task.Status);
        }

        [Fact]
        public void ForEachAsync_CancelBefore()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnCompleted<int>(600)
            );

            var task = default(Task);
            var cts = new CancellationTokenSource();
            var list = new List<Recorded<int>>();

            cts.Cancel();

            scheduler.ScheduleAbsolute(150, () => task = xs.ForEachAsync(x => list.Add(new Recorded<int>(scheduler.Clock, x)), cts.Token));

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
            );

            list.AssertEqual(
            );

            Assert.Equal(TaskStatus.Canceled, task.Status);
        }

        [Fact]
        public void ForEachAsync_CancelAfter()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnCompleted<int>(600)
            );

            var task = default(Task);
            var cts = new CancellationTokenSource();
            var list = new List<Recorded<int>>();

            scheduler.ScheduleAbsolute(150, () => task = xs.ForEachAsync(x => list.Add(new Recorded<int>(scheduler.Clock, x)), cts.Token));
            scheduler.ScheduleAbsolute(700, () => cts.Cancel());

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(150, 600)
            );

            list.AssertEqual(
                new Recorded<int>(200, 2),
                new Recorded<int>(300, 3),
                new Recorded<int>(400, 4),
                new Recorded<int>(500, 5)
            );

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void ForEachAsync_Default()
        {
            var list = new List<int>();
            Observable.Range(1, 10).ForEachAsync(list.Add).Wait();
            list.AssertEqual(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        }

        [Fact]
        public void ForEachAsync_Index()
        {
            var list = new List<int>();
            Observable.Range(3, 5).ForEachAsync((x, i) => list.Add(x * i)).Wait();
            list.AssertEqual(3 * 0, 4 * 1, 5 * 2, 6 * 3, 7 * 4);
        }

        [Fact]
        public void ForEachAsync_Default_Cancel()
        {
            var N = 10;

            for (var n = 0; n < N; n++)
            {
                var cts = new CancellationTokenSource();
                var done = false;

                var xs = Observable.Create<int>(observer =>
                {
                    return new CompositeDisposable(
                        Observable.Repeat(42, Scheduler.Default).Subscribe(observer),
                        Disposable.Create(() => done = true)
                    );
                });

                var lst = new List<int>();

                var t = xs.ForEachAsync(
                    x =>
                    {
                        lock (lst)
                        {
                            lst.Add(x);
                        }
                    },
                    cts.Token
                );

                while (true)
                {
                    lock (lst)
                    {
                        if (lst.Count >= 10)
                        {
                            break;
                        }
                    }
                }

                cts.Cancel();

                while (!t.IsCompleted)
                {
                    ;
                }

                for (var i = 0; i < 10; i++)
                {
                    Assert.Equal(42, lst[i]);
                }

                Assert.True(done);
                Assert.True(t.IsCanceled);
            }
        }

        [Fact]
        public void ForEachAsync_Index_Cancel()
        {
            var N = 10;

            for (var n = 0; n < N; n++)
            {
                var cts = new CancellationTokenSource();
                var done = false;

                var xs = Observable.Create<int>(observer =>
                {
                    return new CompositeDisposable(
                        Observable.Repeat(42, Scheduler.Default).Subscribe(observer),
                        Disposable.Create(() => done = true)
                    );
                });

                var lst = new List<int>();

                var t = xs.ForEachAsync(
                    (x, i) =>
                    {
                        lock (lst)
                        {
                            lst.Add(x * i);
                        }
                    },
                    cts.Token
                );

                while (true)
                {
                    lock (lst)
                    {
                        if (lst.Count >= 10)
                        {
                            break;
                        }
                    }
                }

                cts.Cancel();

                while (!t.IsCompleted)
                {
                    ;
                }

                for (var i = 0; i < 10; i++)
                {
                    Assert.Equal(i * 42, lst[i]);
                }

                Assert.True(done);
                Assert.True(t.IsCanceled);
            }
        }

        [Fact]
        public void ForEachAsync_DisposeThrows1()
        {
            var cts = new CancellationTokenSource();
            var ex = new Exception();

            var xs = Observable.Create<int>(observer =>
            {
                return new CompositeDisposable(
                    Observable.Range(0, 10, Scheduler.CurrentThread).Subscribe(observer),
                    Disposable.Create(() => { throw ex; })
                );
            });

            var lst = new List<int>();
            var t = xs.ForEachAsync(lst.Add, cts.Token);

            //
            // Unfortunately, this doesn't throw for CurrentThread scheduling. The
            // subscription completes prior to assignment of the disposable, so we
            // succeed calling the TrySetResult method for the OnCompleted handler
            // prior to observing the exception of the Dispose operation, which is
            // surfacing upon assignment to the SingleAssignmentDisposable. As a
            // result, the exception evaporates.
            //
            // It'd be a breaking change at this point to rethrow the exception in
            // that case, so we're merely asserting regressions here.
            //
            try
            {
                t.Wait();
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void ForEachAsync_DisposeThrows2()
        {
            var cts = new CancellationTokenSource();
            var ex = new Exception();

            var xs = Observable.Create<int>(observer =>
            {
                return new CompositeDisposable(
                    Observable.Range(0, 10, Scheduler.CurrentThread).Subscribe(observer),
                    Disposable.Create(() => { throw ex; })
                );
            });

            var lst = new List<int>();

            var t = default(Task);

            Scheduler.CurrentThread.Schedule(() =>
            {
                t = xs.ForEachAsync(lst.Add, cts.Token);
            });

            //
            // If the trampoline of the CurrentThread has been installed higher
            // up the stack, the assignment of the subscription's disposable to
            // the SingleAssignmentDisposable can complete prior to the Dispose
            // method being called from the OnCompleted handler. In this case,
            // the OnCompleted handler's invocation of Dispose will cause the
            // exception to occur, and it bubbles out through TrySetException.
            //
            try
            {
                t.Wait();
            }
            catch (AggregateException err)
            {
                Assert.Equal(1, err.InnerExceptions.Count);
                Assert.Same(ex, err.InnerExceptions[0]);
            }
        }

#if !NO_THREAD
        [Fact]
        [Trait("SkipCI", "true")]
        public void ForEachAsync_DisposeThrows()
        {
            //
            // Unfortunately, this test is non-deterministic due to the race
            // conditions described above in the tests using the CurrentThread
            // scheduler. The exception can come out through the OnCompleted
            // handler but can equally well get swallowed if the main thread
            // hasn't reached the assignment of the disposable yet, causing
            // the OnCompleted handler to win the race. The user can deal with
            // this by hooking an exception handler to the scheduler, so we
            // assert this behavior here.
            //
            // It'd be a breaking change at this point to change rethrowing
            // behavior, so we're merely asserting regressions here.
            //

            var hasCaughtEscapingException = 0;

            var cts = new CancellationTokenSource();
            var ex = new Exception();

            var s = Scheduler.Default.Catch<Exception>(err =>
            {
                Volatile.Write(ref hasCaughtEscapingException, 1);
                return ex == err;
            });

            while (Volatile.Read(ref hasCaughtEscapingException) == 0)
            {
                var xs = Observable.Create<int>(observer =>
                {
                    return new CompositeDisposable(
                        Observable.Range(0, 10, s).Subscribe(observer),
                        Disposable.Create(() => { throw ex; })
                    );
                });

                var lst = new List<int>();
                var t = xs.ForEachAsync(lst.Add, cts.Token);

                try
                {
                    t.Wait();
                }
                catch (AggregateException err)
                {
                    Assert.Equal(1, err.InnerExceptions.Count);
                    Assert.Same(ex, err.InnerExceptions[0]);
                }
            }
        }

        [Fact]
        public void ForEachAsync_SubscribeThrows()
        {
            var ex = new Exception();

            var x = 42;
            var xs = Observable.Create<int>(observer =>
            {
                if (x == 42)
                {
                    throw ex;
                }

                return Disposable.Empty;
            });

            var t = xs.ForEachAsync(_ => { });

            try
            {
                t.Wait();
                Assert.True(false);
            }
            catch (AggregateException err)
            {
                Assert.Equal(1, err.InnerExceptions.Count);
                Assert.Same(ex, err.InnerExceptions[0]);
            }
        }
#endif

    }
}
