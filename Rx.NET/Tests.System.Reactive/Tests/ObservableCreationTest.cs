// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveTests.Dummies;

#if !NO_TPL
using System.Threading.Tasks;
#endif

namespace ReactiveTests.Tests
{
    [TestClass]
    public partial class ObservableTest : ReactiveTest
    {
        #region - Create -

        [TestMethod]
        public void Create_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create<int>(default(Func<IObserver<int>, Action>)));

            //
            // BREAKING CHANGE v2.0 > v1.x - Returning null from Subscribe means "nothing to do upon unsubscription"
            //                               all null-coalesces to Disposable.Empty.
            //
            //ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create<int>(o => default(Action)).Subscribe(DummyObserver<int>.Instance));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create<int>(o => () => { }).Subscribe(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create<int>(o =>
            {
                o.OnError(null);
                return () => { };
            }).Subscribe(null));
        }

        [TestMethod]
        public void Create_NullCoalescingAction()
        {
            var xs = Observable.Create<int>(o =>
            {
                o.OnNext(42);
                return default(Action);
            });

            var lst = new List<int>();
            var d = xs.Subscribe(lst.Add);
            d.Dispose();

            Assert.IsTrue(lst.SequenceEqual(new[] {42}));
        }

        [TestMethod]
        public void Create_Next()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Create<int>(o =>
                {
                    o.OnNext(1);
                    o.OnNext(2);
                    return () => { };
                })
            );

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(200, 2)
            );
        }

        [TestMethod]
        public void Create_Completed()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Create<int>(o =>
                {
                    o.OnCompleted();
                    o.OnNext(100);
                    o.OnError(new Exception());
                    o.OnCompleted();
                    return () => { };
                })
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(200)
            );
        }

        [TestMethod]
        public void Create_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Create<int>(o =>
                {
                    o.OnError(ex);
                    o.OnNext(100);
                    o.OnError(new Exception());
                    o.OnCompleted();
                    return () => { };
                })
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        [TestMethod]
        public void Create_Exception()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Create<int>(new Func<IObserver<int>, Action>(o => { throw new InvalidOperationException(); })).Subscribe());
        }

        [TestMethod]
        public void Create_Dispose()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Create<int>(o =>
                {
                    var stopped = false;

                    o.OnNext(1);
                    o.OnNext(2);
                    scheduler.Schedule(TimeSpan.FromTicks(600), () =>
                    {
                        if (!stopped)
                            o.OnNext(3);
                    });
                    scheduler.Schedule(TimeSpan.FromTicks(700), () =>
                    {
                        if (!stopped)
                            o.OnNext(4);
                    });
                    scheduler.Schedule(TimeSpan.FromTicks(900), () =>
                    {
                        if (!stopped)
                            o.OnNext(5);
                    });
                    scheduler.Schedule(TimeSpan.FromTicks(1100), () =>
                    {
                        if (!stopped)
                            o.OnNext(6);
                    });

                    return () => { stopped = true; };
                })
            );

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(200, 2),
                OnNext(800, 3),
                OnNext(900, 4)
            );
        }

        [TestMethod]
        public void Create_ObserverThrows()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Create<int>(o =>
                {
                    o.OnNext(1);
                    return () => { };
                }).Subscribe(x => { throw new InvalidOperationException(); }));
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Create<int>(o =>
                {
                    o.OnError(new Exception());
                    return () => { };
                }).Subscribe(x => { }, ex => { throw new InvalidOperationException(); }));
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Create<int>(o =>
                {
                    o.OnCompleted();
                    return () => { };
                }).Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); }));
        }

        [TestMethod]
        public void CreateWithDisposable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create<int>(default(Func<IObserver<int>, IDisposable>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create<int>(o => DummyDisposable.Instance).Subscribe(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create<int>(o =>
            {
                o.OnError(null);
                return DummyDisposable.Instance;
            }).Subscribe(null));
        }

        [TestMethod]
        public void CreateWithDisposable_NullCoalescingAction()
        {
            var xs = Observable.Create<int>(o =>
            {
                o.OnNext(42);
                return default(IDisposable);
            });

            var lst = new List<int>();
            var d = xs.Subscribe(lst.Add);
            d.Dispose();

            Assert.IsTrue(lst.SequenceEqual(new[] { 42 }));
        }

        [TestMethod]
        public void CreateWithDisposable_Next()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Create<int>(o =>
                {
                    o.OnNext(1);
                    o.OnNext(2);
                    return Disposable.Empty;
                })
            );

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(200, 2)
            );
        }

        [TestMethod]
        public void CreateWithDisposable_Completed()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Create<int>(o =>
                {
                    o.OnCompleted();
                    o.OnNext(100);
                    o.OnError(new Exception());
                    o.OnCompleted();
                    return Disposable.Empty;
                })
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(200)
            );
        }

        [TestMethod]
        public void CreateWithDisposable_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Create<int>(o =>
                {
                    o.OnError(ex);
                    o.OnNext(100);
                    o.OnError(new Exception());
                    o.OnCompleted();
                    return Disposable.Empty;
                })
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        [TestMethod]
        public void CreateWithDisposable_Exception()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Create<int>(new Func<IObserver<int>, IDisposable>(o => { throw new InvalidOperationException(); })).Subscribe());
        }

        [TestMethod]
        public void CreateWithDisposable_Dispose()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Create<int>(o =>
                {
                    var d = new BooleanDisposable();

                    o.OnNext(1);
                    o.OnNext(2);
                    scheduler.Schedule(TimeSpan.FromTicks(600), () =>
                    {
                        if (!d.IsDisposed)
                            o.OnNext(3);
                    });
                    scheduler.Schedule(TimeSpan.FromTicks(700), () =>
                    {
                        if (!d.IsDisposed)
                            o.OnNext(4);
                    });
                    scheduler.Schedule(TimeSpan.FromTicks(900), () =>
                    {
                        if (!d.IsDisposed)
                            o.OnNext(5);
                    });
                    scheduler.Schedule(TimeSpan.FromTicks(1100), () =>
                    {
                        if (!d.IsDisposed)
                            o.OnNext(6);
                    });

                    return d;
                })
            );

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(200, 2),
                OnNext(800, 3),
                OnNext(900, 4)
            );
        }

        [TestMethod]
        public void CreateWithDisposable_ObserverThrows()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Create<int>(o =>
                {
                    o.OnNext(1);
                    return Disposable.Empty;
                }).Subscribe(x => { throw new InvalidOperationException(); }));
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Create<int>(o =>
                {
                    o.OnError(new Exception());
                    return Disposable.Empty;
                }).Subscribe(x => { }, ex => { throw new InvalidOperationException(); }));
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Create<int>(o =>
                {
                    o.OnCompleted();
                    return Disposable.Empty;
                }).Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); }));
        }

        #endregion

        #region - CreateAsync -

#if !NO_TPL

        [TestMethod]
        public void CreateAsync_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create<int>(default(Func<IObserver<int>, Task>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create<int>(default(Func<IObserver<int>, CancellationToken, Task>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create<int>(default(Func<IObserver<int>, Task<IDisposable>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create<int>(default(Func<IObserver<int>, CancellationToken, Task<IDisposable>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create<int>(default(Func<IObserver<int>, Task<Action>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create<int>(default(Func<IObserver<int>, CancellationToken, Task<Action>>)));
        }

        [TestMethod]
        public void CreateAsync_NullCoalescingAction1()
        {
            var xs = Observable.Create<int>(o =>
            {
                o.OnNext(42);
                return Task.Factory.StartNew(() => default(Action));
            });

            var lst = new List<int>();
            var d = xs.Subscribe(lst.Add);
            d.Dispose();

            Assert.IsTrue(lst.SequenceEqual(new[] { 42 }));
        }

        [TestMethod]
        public void CreateAsync_NullCoalescingAction2()
        {
            var xs = Observable.Create<int>((o, ct) =>
            {
                o.OnNext(42);
                return Task.Factory.StartNew(() => default(Action));
            });

            var lst = new List<int>();
            var d = xs.Subscribe(lst.Add);
            d.Dispose();

            Assert.IsTrue(lst.SequenceEqual(new[] { 42 }));
        }

        [TestMethod]
        public void CreateAsync_NullCoalescingDisposable1()
        {
            var xs = Observable.Create<int>(o =>
            {
                o.OnNext(42);
                return Task.Factory.StartNew(() => default(IDisposable));
            });

            var lst = new List<int>();
            var d = xs.Subscribe(lst.Add);
            d.Dispose();

            Assert.IsTrue(lst.SequenceEqual(new[] { 42 }));
        }

        [TestMethod]
        public void CreateAsync_NullCoalescingDisposable2()
        {
            var xs = Observable.Create<int>((o, ct) =>
            {
                o.OnNext(42);
                return Task.Factory.StartNew(() => default(IDisposable));
            });

            var lst = new List<int>();
            var d = xs.Subscribe(lst.Add);
            d.Dispose();

            Assert.IsTrue(lst.SequenceEqual(new[] { 42 }));
        }

        Task Producer1(IObserver<int> results, CancellationToken token, IScheduler scheduler)
        {
            var tcs = new TaskCompletionSource<object>();

            var x = 0;

            var d = scheduler.Schedule(TimeSpan.FromTicks(100), self =>
            {
                results.OnNext(++x);
                self(TimeSpan.FromTicks(100));
            });

            token.Register(d.Dispose);

            return tcs.Task;
        }

        [TestMethod]
        public void CreateAsync_Never()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer1(observer, token, scheduler))
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(400, 2),
                    OnNext(500, 3),
                    OnNext(600, 4),
                    OnNext(700, 5),
                    OnNext(800, 6),
                    OnNext(900, 7)
                );
            });
        }

        Task Producer2(IObserver<int> results, CancellationToken token, IScheduler scheduler)
        {
            var tcs = new TaskCompletionSource<object>();

            var x = 0;

            var d = scheduler.Schedule(TimeSpan.FromTicks(100), self =>
            {
                if (x == 4)
                {
                    tcs.SetResult(null);
                }
                results.OnNext(++x);
                self(TimeSpan.FromTicks(100));
            });

            token.Register(d.Dispose);

            return tcs.Task;
        }

        [TestMethod]
        public void CreateAsync_Completed1()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer2(observer, token, scheduler))
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(400, 2),
                    OnNext(500, 3),
                    OnNext(600, 4),
                    OnCompleted<int>(700)
                );
            });
        }

        Task Producer3(IObserver<int> results, CancellationToken token, IScheduler scheduler)
        {
            var tcs = new TaskCompletionSource<object>();

            var x = 0;

            var d = scheduler.Schedule(TimeSpan.FromTicks(100), self =>
            {
                if (x == 4)
                {
                    results.OnCompleted();
                }
                results.OnNext(++x);
                self(TimeSpan.FromTicks(100));
            });

            token.Register(d.Dispose);

            return tcs.Task;
        }

        [TestMethod]
        public void CreateAsync_Completed2()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer3(observer, token, scheduler))
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(400, 2),
                    OnNext(500, 3),
                    OnNext(600, 4),
                    OnCompleted<int>(700)
                );
            });
        }

        Task Producer4(IObserver<int> results, CancellationToken token, IScheduler scheduler, Exception exception)
        {
            var tcs = new TaskCompletionSource<object>();

            var x = 0;

            var d = scheduler.Schedule(TimeSpan.FromTicks(100), self =>
            {
                if (x == 4)
                {
                    results.OnError(exception);
                }
                results.OnNext(++x);
                self(TimeSpan.FromTicks(100));
            });

            token.Register(d.Dispose);

            return tcs.Task;
        }

        [TestMethod]
        public void CreateAsync_Error1()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var exception = new Exception();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer4(observer, token, scheduler, exception))
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(400, 2),
                    OnNext(500, 3),
                    OnNext(600, 4),
                    OnError<int>(700, exception)
                );
            });
        }

        Task Producer5(IObserver<int> results, CancellationToken token, IScheduler scheduler, Exception exception)
        {
            var tcs = new TaskCompletionSource<object>();

            var x = 0;

            var d = scheduler.Schedule(TimeSpan.FromTicks(100), self =>
            {
                if (x == 4)
                {
                    tcs.SetException(exception);
                }
                results.OnNext(++x);
                self(TimeSpan.FromTicks(100));
            });

            token.Register(d.Dispose);

            return tcs.Task;
        }

        [TestMethod]
        public void CreateAsync_Error2()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var exception = new Exception();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer5(observer, token, scheduler, exception))
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(400, 2),
                    OnNext(500, 3),
                    OnNext(600, 4),
                    OnError<int>(700, exception)
                );
            });
        }


        Task Producer6(IObserver<int> results, CancellationToken token, Exception exception)
        {
            throw exception;
        }

        [TestMethod]
        public void CreateAsync_Error3()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var exception = new InvalidOperationException();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer6(observer, token, exception))
                );

                res.Messages.AssertEqual(
                    OnError<int>(200, exception)
                );
            });
        }

        Task Producer7(IObserver<int> results, CancellationToken token, IScheduler scheduler)
        {
            var tcs = new TaskCompletionSource<object>();

            var x = 0;

            var d = scheduler.Schedule(TimeSpan.FromTicks(100), self =>
            {
                if (x == 4)
                {
                    tcs.SetResult(null);
                }
                results.OnNext(++x);
                self(TimeSpan.FromTicks(100));
            });

            token.Register(d.Dispose);

            return tcs.Task;
        }

        [TestMethod]
        public void CreateAsync_Cancel1()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer7(observer, token, scheduler)),
                    650
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(400, 2),
                    OnNext(500, 3),
                    OnNext(600, 4)
                );
            });
        }

        Task Producer8(IObserver<int> results, CancellationToken token, IScheduler scheduler)
        {
            var tcs = new TaskCompletionSource<object>();

            var x = 0;

            var d = scheduler.Schedule(TimeSpan.FromTicks(100), self =>
            {
                if (x == 4)
                {
                    results.OnCompleted();
                }
                results.OnNext(++x);
                self(TimeSpan.FromTicks(100));
            });

            token.Register(d.Dispose);

            return tcs.Task;
        }

        [TestMethod]
        public void CreateAsync_Cancel2()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer8(observer, token, scheduler)),
                    650
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(400, 2),
                    OnNext(500, 3),
                    OnNext(600, 4)
                );
            });
        }

        Task Producer9(IObserver<int> results, CancellationToken token, IScheduler scheduler)
        {
            var tcs = new TaskCompletionSource<object>();

            var x = 0;

            var d = scheduler.Schedule(TimeSpan.FromTicks(100), self =>
            {
                if (x == 4)
                {
                    results.OnCompleted();
                }
                results.OnNext(++x);
                self(TimeSpan.FromTicks(100));
            });

            token.Register(d.Dispose);

            return tcs.Task;
        }

        [TestMethod]
        public void CreateAsync_Cancel3()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer9(observer, token, scheduler)),
                    750
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(400, 2),
                    OnNext(500, 3),
                    OnNext(600, 4),
                    OnCompleted<int>(700)
                );
            });
        }

        Task Producer10(IObserver<int> results, CancellationToken token, IScheduler scheduler)
        {
            var tcs = new TaskCompletionSource<object>();

            var x = 0;

            var d = scheduler.Schedule(TimeSpan.FromTicks(100), self =>
            {
                if (x == 4)
                {
                    tcs.SetCanceled();
                }
                results.OnNext(++x);
                self(TimeSpan.FromTicks(100));
            });

            token.Register(d.Dispose);

            return tcs.Task;
        }

        [TestMethod]
        public void CreateAsync_Cancel4()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer10(observer, token, scheduler))
                );

                res.Messages.Take(4).AssertEqual(
                    OnNext(300, 1),
                    OnNext(400, 2),
                    OnNext(500, 3),
                    OnNext(600, 4)
                );

                Assert.AreEqual(5, res.Messages.Count);

                Assert.AreEqual(700, res.Messages[4].Time);
                Assert.AreEqual(NotificationKind.OnError, res.Messages[4].Value.Kind);
                Assert.IsTrue(res.Messages[4].Value.Exception is OperationCanceledException);
            });
        }

        void RunSynchronously(Action action)
        {
            var t = new Task(action);
            t.RunSynchronously(new SynchronousScheduler());
            t.Wait();
        }

        class SynchronousScheduler : TaskScheduler
        {
            protected override IEnumerable<Task> GetScheduledTasks()
            {
                throw new NotImplementedException();
            }

            protected override void QueueTask(Task task)
            {
                TryExecuteTask(task);
            }

            protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
            {
                return TryExecuteTask(task);
            }
        }

        [TestMethod]
        public void CreateAsync_Task_Simple()
        {
            var xs = Observable.Create<int>(observer =>
            {
                return Task.Factory.StartNew(() =>
                {
                    observer.OnNext(42);
                    observer.OnCompleted();
                });
            });

            var lst = new List<int>();
            xs.ForEach(lst.Add);

            Assert.IsTrue(new[] { 42 }.SequenceEqual(lst));
        }

        [TestMethod]
        public void CreateAsync_Task_Token()
        {
            var e = new ManualResetEvent(false);

            var xs = Observable.Create<int>((observer, ct) =>
            {
                return Task.Factory.StartNew(() =>
                {
                    var i = 0;

                    while (!ct.IsCancellationRequested)
                    {
                        if (i++ == 10)
                            e.Set();

                        observer.OnNext(42);
                    }
                });
            });

            var lst = new List<int>();
            var d = xs.Subscribe(lst.Add);

            e.WaitOne();
            d.Dispose();

            Assert.IsTrue(lst.Take(10).SequenceEqual(Enumerable.Repeat(42, 10)));
        }

        [TestMethod]
        public void CreateAsync_IDisposable_Simple()
        {
            var stopped = new ManualResetEvent(false);
            var s = Disposable.Create(() => stopped.Set());

            var xs = Observable.Create<int>(observer =>
            {
                return Task.Factory.StartNew(() =>
                {
                    observer.OnNext(42);
                    observer.OnCompleted();

                    return s;
                });
            });

            var lst = new List<int>();
            xs.ForEach(lst.Add);

            stopped.WaitOne();

            Assert.IsTrue(new[] { 42 }.SequenceEqual(lst));
        }

        [TestMethod]
        public void CreateAsync_IDisposable_Token()
        {
            var stopped = new ManualResetEvent(false);
            var s = Disposable.Create(() => stopped.Set());

            var e = new ManualResetEvent(false);

            var xs = Observable.Create<int>((observer, ct) =>
            {
                return Task.Factory.StartNew(() =>
                {
                    var i = 0;

                    while (!ct.IsCancellationRequested)
                    {
                        if (i++ == 10)
                            e.Set();

                        observer.OnNext(42);
                    }

                    return s;
                });
            });

            var lst = new List<int>();
            var d = xs.Subscribe(lst.Add);

            e.WaitOne();
            d.Dispose();
            stopped.WaitOne();

            Assert.IsTrue(lst.Take(10).SequenceEqual(Enumerable.Repeat(42, 10)));
        }

        [TestMethod]
        public void CreateAsync_Action_Simple()
        {
            var stopped = new ManualResetEvent(false);
            var s = new Action(() => stopped.Set());

            var xs = Observable.Create<int>(observer =>
            {
                return Task.Factory.StartNew(() =>
                {
                    observer.OnNext(42);
                    observer.OnCompleted();

                    return s;
                });
            });

            var lst = new List<int>();
            xs.ForEach(lst.Add);

            stopped.WaitOne();

            Assert.IsTrue(new[] { 42 }.SequenceEqual(lst));
        }

        [TestMethod]
        public void CreateAsync_Action_Token()
        {
            var stopped = new ManualResetEvent(false);
            var s = new Action(() => stopped.Set());

            var e = new ManualResetEvent(false);

            var xs = Observable.Create<int>((observer, ct) =>
            {
                return Task.Factory.StartNew(() =>
                {
                    var i = 0;

                    while (!ct.IsCancellationRequested)
                    {
                        if (i++ == 10)
                            e.Set();

                        observer.OnNext(42);
                    }

                    return s;
                });
            });

            var lst = new List<int>();
            var d = xs.Subscribe(lst.Add);

            e.WaitOne();
            d.Dispose();
            stopped.WaitOne();

            Assert.IsTrue(lst.Take(10).SequenceEqual(Enumerable.Repeat(42, 10)));
        }

#endif

        #endregion

        #region + Defer +

        [TestMethod]
        public void Defer_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Defer<int>(default(Func<IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Defer(() => DummyObservable<int>.Instance).Subscribe(null));
            ReactiveAssert.Throws</*some*/Exception>(() => Observable.Defer<int>(() => default(IObservable<int>)).Subscribe());
        }

        [TestMethod]
        public void Defer_Complete()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;
            var xs = default(ITestableObservable<long>);

            var res = scheduler.Start(() =>
                Observable.Defer(() =>
                {
                    invoked++;
                    xs = scheduler.CreateColdObservable(
                        OnNext<long>(100, scheduler.Clock),
                        OnCompleted<long>(200));
                    return xs;
                })
            );

            res.Messages.AssertEqual(
                OnNext(300, 200L),
                OnCompleted<long>(400)
            );

            Assert.AreEqual(1, invoked);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Defer_Error()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;
            var xs = default(ITestableObservable<long>);
            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Defer(() =>
                {
                    invoked++;
                    xs = scheduler.CreateColdObservable(
                        OnNext<long>(100, scheduler.Clock),
                        OnError<long>(200, ex));
                    return xs;
                })
            );

            res.Messages.AssertEqual(
                OnNext(300, 200L),
                OnError<long>(400, ex)
            );

            Assert.AreEqual(1, invoked);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Defer_Dispose()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;
            var xs = default(ITestableObservable<long>);

            var res = scheduler.Start(() =>
                Observable.Defer(() =>
                {
                    invoked++;
                    xs = scheduler.CreateColdObservable(
                        OnNext<long>(100, scheduler.Clock),
                        OnNext<long>(200, invoked),
                        OnNext<long>(1100, 1000));
                    return xs;
                })
            );

            res.Messages.AssertEqual(
                OnNext(300, 200L),
                OnNext(400, 1L)
            );

            Assert.AreEqual(1, invoked);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Defer_Throw()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;
            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Defer<int>(new Func<IObservable<int>>(() =>
                {
                    invoked++;
                    throw ex;
                }))
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            Assert.AreEqual(1, invoked);
        }

        #endregion

        #region - DeferAsync -

#if !NO_TPL

        [TestMethod]
        public void DeferAsync_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Defer(default(Func<Task<IObservable<int>>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DeferAsync(default(Func<CancellationToken, Task<IObservable<int>>>)));
        }

        [TestMethod]
        public void DeferAsync_Simple()
        {
            var xs = Observable.Defer<int>(() => Task.Factory.StartNew(() => Observable.Return(42)));

            var res = xs.ToEnumerable().ToList();

            Assert.IsTrue(new[] { 42 }.SequenceEqual(res));
        }

        [TestMethod]
        public void DeferAsync_WithCancel_Simple()
        {
            var xs = Observable.DeferAsync<int>(ct => Task.Factory.StartNew(() => Observable.Return(42)));

            var res = xs.ToEnumerable().ToList();

            Assert.IsTrue(new[] { 42 }.SequenceEqual(res));
        }

        [TestMethod]
        public void DeferAsync_WithCancel_Cancel()
        {
            var N = 10;// 0000;
            for (int i = 0; i < N; i++)
            {
                var e = new ManualResetEvent(false);
                var called = false;

                var xs = Observable.DeferAsync<int>(ct => Task.Factory.StartNew(() =>
                {
                    e.Set();

                    while (!ct.IsCancellationRequested)
                        ;

                    return Observable.Defer(() => { called = true; return Observable.Return(42); });
                }));

                var d = xs.Subscribe(_ => { });

                e.WaitOne();
                d.Dispose();

                Assert.IsFalse(called);
            }
        }

#endif

        #endregion

        #region + Empty +

        [TestMethod]
        public void Empty_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Empty<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Empty<int>(null, 42));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Empty<int>(DummyScheduler.Instance).Subscribe(null));
        }

        [TestMethod]
        public void Empty_Basic()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Empty<int>(scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(201)
            );
        }

        [TestMethod]
        public void Empty_Disposed()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Empty<int>(scheduler),
                200
            );

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void Empty_ObserverThrows()
        {
            var scheduler1 = new TestScheduler();

            var xs = Observable.Empty<int>(scheduler1);

            xs.Subscribe(x => { }, exception => { }, () => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler1.Start());
        }

        [TestMethod]
        public void Empty_DefaultScheduler()
        {
            Observable.Empty<int>().AssertEqual(Observable.Empty<int>(DefaultScheduler.Instance));
        }

        [TestMethod]
        public void Empty_Basic_Witness()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Empty<int>(scheduler, 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(201)
            );
        }

        #endregion

        #region + Generate +

        [TestMethod]
        public void Generate_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, (IScheduler)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, (Func<int, bool>)null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, (Func<int, int>)null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, (Func<int, int>)null, DummyFunc<int, int>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyScheduler.Instance).Subscribe(null));
        }

        [TestMethod]
        public void Generate_Finite()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => x <= 3, x => x + 1, x => x, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 0),
                OnNext(202, 1),
                OnNext(203, 2),
                OnNext(204, 3),
                OnCompleted<int>(205)
            );
        }

        [TestMethod]
        public void Generate_Throw_Condition()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, new Func<int, bool>(x => { throw ex; }), x => x + 1, x => x, scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [TestMethod]
        public void Generate_Throw_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, x => x + 1, new Func<int, int>(x => { throw ex; }), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [TestMethod]
        public void Generate_Throw_Iterate()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, new Func<int, int>(x => { throw ex; }), x => x, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 0),
                OnError<int>(202, ex)
            );
        }

        [TestMethod]
        public void Generate_Dispose()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, x => x + 1, x => x, scheduler),
                203
            );

            res.Messages.AssertEqual(
                OnNext(201, 0),
                OnNext(202, 1)
            );
        }

        [TestMethod]
        public void Generate_DefaultScheduler_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, (Func<int, bool>)null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, (Func<int, int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, (Func<int, int>)null, DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void Generate_DefaultScheduler()
        {
            Observable.Generate(0, x => x < 10, x => x + 1, x => x).AssertEqual(Observable.Generate(0, x => x < 10, x => x + 1, x => x, DefaultScheduler.Instance));
        }

#if !NO_PERF
        [TestMethod]
        public void Generate_LongRunning1()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var s = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Generate(0, x => x < 100, x => x + 1, x => x, s);

            var lst = new List<int>();
            var done = false;
            xs.Subscribe(x => { lst.Add(x); }, () => done = true);

            end.WaitOne();

            Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(0, 100)));
            Assert.IsTrue(done);
        }

        [TestMethod]
        public void Generate_LongRunning2()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var s = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Generate(0, _ => true, x => x + 1, x => x, s);

            var lst = new List<int>();
            var d = xs.Subscribe(x => { lst.Add(x); });

            start.WaitOne();

            while (lst.Count < 100)
                ;

            d.Dispose();
            end.WaitOne();

            Assert.IsTrue(lst.Take(100).SequenceEqual(Enumerable.Range(0, 100)));
        }

        [TestMethod]
        public void Generate_LongRunning_Throw()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var s = new TestLongRunningScheduler(x => start = x, x => end = x);

            var ex = new Exception();
            var xs = Observable.Generate(0, x => { if (x < 100) return true; throw ex; }, x => x + 1, x => x, s);

            var lst = new List<int>();
            var e = default(Exception);
            var done = false;
            xs.Subscribe(x => { lst.Add(x); }, e_ => e = e_, () => done = true);

            end.WaitOne();

            Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(0, 100)));
            Assert.AreSame(ex, e);
            Assert.IsFalse(done);
        }
#endif

        #endregion

        #region + Never +

        [TestMethod]
        public void Never_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Never<int>().Subscribe(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Never<int>(42).Subscribe(null));
        }

        [TestMethod]
        public void Never_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Never<int>();

            var res = scheduler.CreateObserver<int>();

            xs.Subscribe(res);

            scheduler.Start();

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void Never_Basic_Witness()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Never<int>(42);

            var res = scheduler.CreateObserver<int>();

            xs.Subscribe(res);

            scheduler.Start();

            res.Messages.AssertEqual(
            );
        }

        #endregion

        #region + Range +

        [TestMethod]
        public void Range_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Range(0, 0, null));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Range(0, -1, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Range(int.MaxValue, 2, DummyScheduler.Instance));
        }

        [TestMethod]
        public void Range_Zero()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Range(0, 0, scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(201)
            );
        }

        [TestMethod]
        public void Range_One()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Range(0, 1, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 0),
                OnCompleted<int>(202)
            );
        }

        [TestMethod]
        public void Range_Five()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Range(10, 5, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 10),
                OnNext(202, 11),
                OnNext(203, 12),
                OnNext(204, 13),
                OnNext(205, 14),
                OnCompleted<int>(206)
            );
        }

        [TestMethod]
        public void Range_Boundaries()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Range(int.MaxValue, 1, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, int.MaxValue),
                OnCompleted<int>(202)
            );
        }

        [TestMethod]
        public void Range_Dispose()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Range(-10, 5, scheduler),
                204
            );

            res.Messages.AssertEqual(
                OnNext(201, -10),
                OnNext(202, -9),
                OnNext(203, -8)
            );
        }

        [TestMethod]
        public void Range_Default_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Range(0, -1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Range(int.MaxValue, 2));
        }

        [TestMethod]
        public void Range_Default()
        {
            for (int i = 0; i < 100; i++)
                Observable.Range(100, 100).AssertEqual(Observable.Range(100, 100, DefaultScheduler.Instance));
        }

#if !NO_PERF
        [TestMethod]
        public void Range_LongRunning1()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var s = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Range(0, 100, s);

            var lst = new List<int>();
            var done = false;
            xs.Subscribe(x => { lst.Add(x); }, () => done = true);

            end.WaitOne();

            Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(0, 100)));
            Assert.IsTrue(done);
        }

        [TestMethod]
        public void Range_LongRunning2()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var s = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Range(0, int.MaxValue, s);

            var lst = new List<int>();
            var d = xs.Subscribe(x => { lst.Add(x); });

            start.WaitOne();

            while (lst.Count < 100)
                ;

            d.Dispose();
            end.WaitOne();

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Range_LongRunning_Empty()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var scheduler = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Range(5, 0, scheduler);

            var lst = new List<int>();
            xs.ForEach(lst.Add);

            Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(5, 0)));
        }

        [TestMethod]
        public void Range_LongRunning_Regular()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var scheduler = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Range(5, 17, scheduler);

            var lst = new List<int>();
            xs.ForEach(lst.Add);

            Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(5, 17)));
        }

        [TestMethod]
        public void Range_LongRunning_Boundaries()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var scheduler = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Range(int.MaxValue, 1, scheduler);

            var lst = new List<int>();
            xs.ForEach(lst.Add);

            Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(int.MaxValue, 1)));
        }
#endif

        #endregion

        #region + Repeat +

        [TestMethod]
        public void Repeat_Value_Count_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat(1, 0, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Repeat(1, -1, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat(1, 1, DummyScheduler.Instance).Subscribe(null));
        }

        [TestMethod]
        public void Repeat_Value_Count_Zero()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Repeat(42, 0, scheduler)
            );

#if !NO_PERF
            res.Messages.AssertEqual(
                OnCompleted<int>(201)
            );
#else
            res.Messages.AssertEqual(
                OnCompleted<int>(200)
            );
#endif
        }

        [TestMethod]
        public void Repeat_Value_Count_One()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Repeat(42, 1, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 42),
                OnCompleted<int>(201)
            );
        }

        [TestMethod]
        public void Repeat_Value_Count_Ten()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Repeat(42, 10, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 42),
                OnNext(202, 42),
                OnNext(203, 42),
                OnNext(204, 42),
                OnNext(205, 42),
                OnNext(206, 42),
                OnNext(207, 42),
                OnNext(208, 42),
                OnNext(209, 42),
                OnNext(210, 42),
                OnCompleted<int>(210)
            );
        }

        [TestMethod]
        public void Repeat_Value_Count_Dispose()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Repeat(42, 10, scheduler),
                207
            );

            res.Messages.AssertEqual(
                OnNext(201, 42),
                OnNext(202, 42),
                OnNext(203, 42),
                OnNext(204, 42),
                OnNext(205, 42),
                OnNext(206, 42)
            );
        }

        [TestMethod]
        public void Repeat_Value_Count_Default_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Repeat(1, -1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat(1, 1).Subscribe(null));
        }

        [TestMethod]
        public void Repeat_Value_Count_Default()
        {
            Observable.Repeat(42, 10).AssertEqual(Observable.Repeat(42, 10, DefaultScheduler.Instance));
        }

        [TestMethod]
        public void Repeat_Value_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat(1, (IScheduler)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat(DummyScheduler.Instance, 1).Subscribe(null));
        }

        [TestMethod]
        public void Repeat_Value()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Repeat(42, scheduler),
                207
            );

            res.Messages.AssertEqual(
                OnNext(201, 42),
                OnNext(202, 42),
                OnNext(203, 42),
                OnNext(204, 42),
                OnNext(205, 42),
                OnNext(206, 42)
            );
        }

        [TestMethod]
        public void Repeat_Value_Default_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat(1).Subscribe(null));
        }

        [TestMethod]
        public void Repeat_Value_Default()
        {
            Observable.Repeat(42).Take(100).AssertEqual(Observable.Repeat(42, DefaultScheduler.Instance).Take(100));
        }

#if !NO_PERF
        [TestMethod]
        public void Repeat_Count_LongRunning1()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var s = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Repeat(42, 100, s);

            var lst = new List<int>();
            var done = false;
            xs.Subscribe(x => { lst.Add(x); }, () => done = true);

            end.WaitOne();

            Assert.IsTrue(lst.SequenceEqual(Enumerable.Repeat(42, 100)));
            Assert.IsTrue(done);
        }

        [TestMethod]
        public void Repeat_Count_LongRunning2()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var s = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Repeat(42, int.MaxValue, s);

            var lst = new List<int>();
            var d = xs.Subscribe(x => { lst.Add(x); });

            start.WaitOne();

            while (lst.Count < 100)
                ;

            d.Dispose();
            end.WaitOne();

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Repeat_Inf_LongRunning()
        {
            var start = default(ManualResetEvent);
            var end = default(ManualResetEvent);
            var s = new TestLongRunningScheduler(x => start = x, x => end = x);

            var xs = Observable.Repeat(42, s);

            var lst = new List<int>();
            var d = xs.Subscribe(x => { lst.Add(x); });

            start.WaitOne();

            while (lst.Count < 100)
                ;

            d.Dispose();
            end.WaitOne();

            Assert.IsTrue(true);
        }
#endif

        #endregion

        #region + Return +

        [TestMethod]
        public void Return_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Return(0, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Return(0, DummyScheduler.Instance).Subscribe(null));
        }

        [TestMethod]
        public void Return_Basic()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Return(42, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 42),
                OnCompleted<int>(201)
            );
        }

        [TestMethod]
        public void Return_Disposed()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Return(42, scheduler),
                200
            );

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void Return_DisposedAfterNext()
        {
            var scheduler = new TestScheduler();

            var d = new SerialDisposable();

            var xs = Observable.Return(42, scheduler);

            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(100, () =>
                d.Disposable = xs.Subscribe(
                    x =>
                    {
                        d.Dispose();
                        res.OnNext(x);
                    },
                    res.OnError,
                    res.OnCompleted
                )
            );

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(101, 42)
            );
        }

        [TestMethod]
        public void Return_ObserverThrows()
        {
            var scheduler1 = new TestScheduler();

            var xs = Observable.Return(1, scheduler1);

            xs.Subscribe(x => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler1.Start());

            var scheduler2 = new TestScheduler();

            var ys = Observable.Return(1, scheduler2);

            ys.Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler2.Start());
        }

        [TestMethod]
        public void Return_DefaultScheduler()
        {
            Observable.Return(42).AssertEqual(Observable.Return(42, DefaultScheduler.Instance));
        }

        #endregion

        #region + Throw +

        [TestMethod]
        public void Throw_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throw<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throw<int>(null, 42));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throw<int>(new Exception(), null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throw<int>(new Exception(), null, 42));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throw<int>(null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throw<int>(null, DummyScheduler.Instance, 42));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throw<int>(new Exception(), DummyScheduler.Instance).Subscribe(null));
        }

        [TestMethod]
        public void Throw_Basic()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Throw<int>(ex, scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [TestMethod]
        public void Throw_Disposed()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Throw<int>(new Exception(), scheduler),
                200
            );

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void Throw_ObserverThrows()
        {
            var scheduler1 = new TestScheduler();

            var xs = Observable.Throw<int>(new Exception(), scheduler1);

            xs.Subscribe(x => { }, ex => { throw new InvalidOperationException(); }, () => { });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler1.Start());
        }

        [TestMethod]
        public void Throw_DefaultScheduler()
        {
            var ex = new Exception();
            Observable.Throw<int>(ex).AssertEqual(Observable.Throw<int>(ex, DefaultScheduler.Instance));
        }

        [TestMethod]
        public void Throw_Witness_Basic()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Throw<int>(ex, scheduler, 42)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        #endregion

        #region + Using +

        [TestMethod]
        public void Using_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Using((Func<IDisposable>)null, DummyFunc<IDisposable, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Using(DummyFunc<IDisposable>.Instance, (Func<IDisposable, IObservable<int>>)null));
            ReactiveAssert.Throws</*some*/Exception>(() => Observable.Using(() => DummyDisposable.Instance, d => default(IObservable<int>)).Subscribe());
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Using(() => DummyDisposable.Instance, d => DummyObservable<int>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void Using_Null()
        {
            var scheduler = new TestScheduler();

            var disposeInvoked = 0L;
            var createInvoked = 0L;
            var xs = default(ITestableObservable<long>);
            var disposable = default(MockDisposable);
            var _d = default(MockDisposable);

            var res = scheduler.Start(() =>
                Observable.Using(
                    () =>
                    {
                        disposeInvoked++;
                        disposable = default(MockDisposable);
                        return disposable;
                    },
                    d =>
                    {
                        _d = d;
                        createInvoked++;
                        xs = scheduler.CreateColdObservable(
                            OnNext<long>(100, scheduler.Clock),
                            OnCompleted<long>(200));
                        return xs;
                    }
                )
            );

            Assert.AreSame(disposable, _d);

            res.Messages.AssertEqual(
                OnNext(300, 200L),
                OnCompleted<long>(400)
            );

            Assert.AreEqual(1, createInvoked);
            Assert.AreEqual(1, disposeInvoked);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            Assert.IsNull(disposable);
        }

        [TestMethod]
        public void Using_Complete()
        {
            var scheduler = new TestScheduler();

            var disposeInvoked = 0;
            var createInvoked = 0;
            var xs = default(ITestableObservable<long>);
            var disposable = default(MockDisposable);
            var _d = default(MockDisposable);

            var res = scheduler.Start(() =>
                Observable.Using(
                    () =>
                    {
                        disposeInvoked++;
                        disposable = new MockDisposable(scheduler);
                        return disposable;
                    },
                    d =>
                    {
                        _d = d;
                        createInvoked++;
                        xs = scheduler.CreateColdObservable(
                            OnNext<long>(100, scheduler.Clock),
                            OnCompleted<long>(200));
                        return xs;
                    }
                )
            );

            Assert.AreSame(disposable, _d);

            res.Messages.AssertEqual(
                OnNext(300, 200L),
                OnCompleted<long>(400)
            );

            Assert.AreEqual(1, createInvoked);
            Assert.AreEqual(1, disposeInvoked);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            disposable.AssertEqual(
                200,
                400
            );
        }

        [TestMethod]
        public void Using_Error()
        {
            var scheduler = new TestScheduler();

            var disposeInvoked = 0;
            var createInvoked = 0;
            var xs = default(ITestableObservable<long>);
            var disposable = default(MockDisposable);
            var _d = default(MockDisposable);
            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Using(
                    () =>
                    {
                        disposeInvoked++;
                        disposable = new MockDisposable(scheduler);
                        return disposable;
                    },
                    d =>
                    {
                        _d = d;
                        createInvoked++;
                        xs = scheduler.CreateColdObservable(
                            OnNext<long>(100, scheduler.Clock),
                            OnError<long>(200, ex));
                        return xs;
                    }
                )
            );

            Assert.AreSame(disposable, _d);

            res.Messages.AssertEqual(
                OnNext(300, 200L),
                OnError<long>(400, ex)
            );

            Assert.AreEqual(1, createInvoked);
            Assert.AreEqual(1, disposeInvoked);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            disposable.AssertEqual(
                200,
                400
            );
        }

        [TestMethod]
        public void Using_Dispose()
        {
            var scheduler = new TestScheduler();

            var disposeInvoked = 0;
            var createInvoked = 0;
            var xs = default(ITestableObservable<long>);
            var disposable = default(MockDisposable);
            var _d = default(MockDisposable);

            var res = scheduler.Start(() =>
                Observable.Using(
                    () =>
                    {
                        disposeInvoked++;
                        disposable = new MockDisposable(scheduler);
                        return disposable;
                    },
                    d =>
                    {
                        _d = d;
                        createInvoked++;
                        xs = scheduler.CreateColdObservable(
                            OnNext<long>(100, scheduler.Clock),
                            OnNext<long>(1000, scheduler.Clock + 1));
                        return xs;
                    }
                )
            );

            Assert.AreSame(disposable, _d);

            res.Messages.AssertEqual(
                OnNext(300, 200L)
            );

            Assert.AreEqual(1, createInvoked);
            Assert.AreEqual(1, disposeInvoked);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            disposable.AssertEqual(
                200,
                1000
            );
        }

        [TestMethod]
        public void Using_ThrowResourceSelector()
        {
            var scheduler = new TestScheduler();

            var disposeInvoked = 0;
            var createInvoked = 0;
            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Using<int, IDisposable>(
                    () =>
                    {
                        disposeInvoked++;
                        throw ex;
                    },
                    d =>
                    {
                        createInvoked++;
                        return Observable.Never<int>();
                    }
                )
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            Assert.AreEqual(0, createInvoked);
            Assert.AreEqual(1, disposeInvoked);
        }

        [TestMethod]
        public void Using_ThrowResourceUsage()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var disposeInvoked = 0;
            var createInvoked = 0;
            var disposable = default(MockDisposable);

            var res = scheduler.Start(() =>
                Observable.Using<int, IDisposable>(
                    () =>
                    {
                        disposeInvoked++;
                        disposable = new MockDisposable(scheduler);
                        return disposable;
                    },
                    d =>
                    {
                        createInvoked++;
                        throw ex;
                    }
                )
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            Assert.AreEqual(1, createInvoked);
            Assert.AreEqual(1, disposeInvoked);

            disposable.AssertEqual(
                200,
                200
            );
        }

        #endregion

        #region - UsingAsync -

#if !NO_TPL

        [TestMethod]
        public void UsingAsync_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Using<int, IDisposable>(null, (res, ct) => null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Using<int, IDisposable>(ct => null, null));
        }

        [TestMethod]
        public void UsingAsync_Simple()
        {
            var done = false;

            var xs = Observable.Using<int, IDisposable>(
                ct => Task.Factory.StartNew<IDisposable>(() => Disposable.Create(() => done = true)),
                (_, ct) => Task.Factory.StartNew<IObservable<int>>(() => Observable.Return(42))
            );

            var res = xs.ToEnumerable().ToList();

            Assert.IsTrue(new[] { 42 }.SequenceEqual(res));
            Assert.IsTrue(done);
        }

        [TestMethod]
        public void UsingAsync_CancelResource()
        {
            var N = 10;// 0000;
            for (int i = 0; i < N; i++)
            {
                var called = false;

                var s = new ManualResetEvent(false);
                var e = new ManualResetEvent(false);
                var x = new ManualResetEvent(false);

                var xs = Observable.Using<int, IDisposable>(
                    ct => Task.Factory.StartNew<IDisposable>(() =>
                    {
                        s.Set();
                        e.WaitOne();
                        while (!ct.IsCancellationRequested)
                            ;
                        x.Set();
                        return Disposable.Empty;
                    }),
                    (_, ct) =>
                    {
                        called = true;
                        return Task.Factory.StartNew<IObservable<int>>(() =>
                            Observable.Return(42)
                        );
                    }
                );

                var d = xs.Subscribe(_ => { });

                s.WaitOne();
                d.Dispose();

                e.Set();
                x.WaitOne();

                Assert.IsFalse(called);
            }
        }

        [TestMethod]
        public void UsingAsync_CancelFactory()
        {
            var N = 10;// 0000;
            for (int i = 0; i < N; i++)
            {
                var gate = new object();
                var disposed = false;
                var called = false;

                var s = new ManualResetEvent(false);
                var e = new ManualResetEvent(false);
                var x = new ManualResetEvent(false);

                var xs = Observable.Using<int, IDisposable>(
                    ct => Task.Factory.StartNew<IDisposable>(() =>
                        Disposable.Create(() =>
                        {
                            lock (gate)
                                disposed = true;
                        })
                    ),
                    (_, ct) => Task.Factory.StartNew<IObservable<int>>(() =>
                    {
                        s.Set();
                        e.WaitOne();
                        while (!ct.IsCancellationRequested)
                            ;
                        x.Set();
                        return Observable.Defer<int>(() =>
                        {
                            called = true;
                            return Observable.Return(42);
                        });
                    })
                );

                var d = xs.Subscribe(_ => { });

                s.WaitOne();

                //
                // This will *eventually* set the CancellationToken. There's a fundamental race between observing the CancellationToken
                // and returning the IDisposable that will set the CancellationTokenSource. Notice this is reflected in the code above,
                // by looping until the CancellationToken is set.
                //
                d.Dispose();

                e.Set();
                x.WaitOne();

                while (true)
                {
                    lock (gate)
                        if (disposed)
                            break;
                }

                Assert.IsFalse(called, i.ToString());
            }
        }

#endif

        #endregion
    }
}
