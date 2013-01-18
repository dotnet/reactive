// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public partial class ObserverTest : ReactiveTest
    {
        [TestMethod]
        public void ToObserver_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.ToObserver<int>(default(Action<Notification<int>>)));
        }

        [TestMethod]
        public void ToObserver_NotificationOnNext()
        {
            int i = 0;
            Action<Notification<int>> next = n =>
            {
                Assert.AreEqual(i++, 0);
                Assert.AreEqual(n.Kind, NotificationKind.OnNext);
                Assert.AreEqual(n.Value, 42);
                Assert.AreEqual(n.Exception, null);
                Assert.IsTrue(n.HasValue);
            };
            next.ToObserver().OnNext(42);
        }

        [TestMethod]
        public void ToObserver_NotificationOnError()
        {
            var ex = new Exception();
            int i = 0;
            Action<Notification<int>> next = n =>
            {
                Assert.AreEqual(i++, 0);
                Assert.AreEqual(n.Kind, NotificationKind.OnError);
                Assert.AreSame(n.Exception, ex);
                Assert.IsFalse(n.HasValue);
            };
            next.ToObserver().OnError(ex);
        }

        [TestMethod]
        public void ToObserver_NotificationOnCompleted()
        {
            var ex = new Exception();
            int i = 0;
            Action<Notification<int>> next = n =>
            {
                Assert.AreEqual(i++, 0);
                Assert.AreEqual(n.Kind, NotificationKind.OnCompleted);
                Assert.IsFalse(n.HasValue);
            };
            next.ToObserver().OnCompleted();
        }

        [TestMethod]
        public void ToNotifier_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.ToNotifier<int>(default(IObserver<int>)));
        }

        [TestMethod]
        public void ToNotifier_Forwards()
        {
            var obsn = new MyObserver();
            obsn.ToNotifier()(Notification.CreateOnNext<int>(42));
            Assert.AreEqual(obsn.HasOnNext, 42);

            var ex = new Exception();
            var obse = new MyObserver();
            obse.ToNotifier()(Notification.CreateOnError<int>(ex));
            Assert.AreSame(ex, obse.HasOnError);

            var obsc = new MyObserver();
            obsc.ToNotifier()(Notification.CreateOnCompleted<int>());
            Assert.IsTrue(obsc.HasOnCompleted);
        }

        [TestMethod]
        public void Create_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Create<int>(default(Action<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Create<int>(default(Action<int>), () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Create<int>(_ => { }, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Create<int>(default(Action<int>), (Exception _) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Create<int>(_ => { }, default(Action<Exception>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Create<int>(default(Action<int>), (Exception _) => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Create<int>(_ => { }, default(Action<Exception>), () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Create<int>(_ => { }, (Exception _) => { }, default(Action)));
        }

        [TestMethod]
        public void Create_OnNext()
        {
            bool next = false;
            var res = Observer.Create<int>(x => { Assert.AreEqual(42, x); next = true; });
            res.OnNext(42);
            Assert.IsTrue(next);
            res.OnCompleted();
        }

        [TestMethod]
        public void Create_OnNext_HasError()
        {
            var ex = new Exception();
            var e_ = default(Exception);

            bool next = false;
            var res = Observer.Create<int>(x => { Assert.AreEqual(42, x); next = true; });
            res.OnNext(42);
            Assert.IsTrue(next);

            try
            {
                res.OnError(ex);
                Assert.Fail();
            }
            catch (Exception e)
            {
                e_ = e;
            }
            Assert.AreSame(ex, e_);
        }

        [TestMethod]
        public void Create_OnNextOnCompleted()
        {
            bool next = false;
            bool completed = false;
            var res = Observer.Create<int>(x => { Assert.AreEqual(42, x); next = true; }, () => { completed = true; });
            res.OnNext(42);
            Assert.IsTrue(next);
            Assert.IsFalse(completed);
            res.OnCompleted();
            Assert.IsTrue(completed);
        }

        [TestMethod]
        public void Create_OnNextOnCompleted_HasError()
        {
            var ex = new Exception();
            var e_ = default(Exception);

            bool next = false;
            bool completed = false;
            var res = Observer.Create<int>(x => { Assert.AreEqual(42, x); next = true; }, () => { completed = true; });
            res.OnNext(42);
            Assert.IsTrue(next);
            Assert.IsFalse(completed);
            try
            {
                res.OnError(ex);
                Assert.Fail();
            }
            catch (Exception e)
            {
                e_ = e;
            }
            Assert.AreSame(ex, e_);
            Assert.IsFalse(completed);
        }

        [TestMethod]
        public void Create_OnNextOnError()
        {
            var ex = new Exception();
            bool next = true;
            bool error = false;
            var res = Observer.Create<int>(x => { Assert.AreEqual(42, x); next = true; }, e => { Assert.AreSame(ex, e); error = true; });
            res.OnNext(42);
            Assert.IsTrue(next);
            Assert.IsFalse(error);
            res.OnError(ex);
            Assert.IsTrue(error);
        }

        [TestMethod]
        public void Create_OnNextOnError_HitCompleted()
        {
            var ex = new Exception();
            bool next = true;
            bool error = false;
            var res = Observer.Create<int>(x => { Assert.AreEqual(42, x); next = true; }, e => { Assert.AreSame(ex, e); error = true; });
            res.OnNext(42);
            Assert.IsTrue(next);
            Assert.IsFalse(error);
            res.OnCompleted();
            Assert.IsFalse(error);
        }

        [TestMethod]
        public void Create_OnNextOnErrorOnCompleted1()
        {
            var ex = new Exception();
            bool next = true;
            bool error = false;
            bool completed = false;
            var res = Observer.Create<int>(x => { Assert.AreEqual(42, x); next = true; }, e => { Assert.AreSame(ex, e); error = true; }, () => { completed = true; });
            res.OnNext(42);
            Assert.IsTrue(next);
            Assert.IsFalse(error);
            Assert.IsFalse(completed);
            res.OnCompleted();
            Assert.IsTrue(completed);
            Assert.IsFalse(error);
        }

        [TestMethod]
        public void Create_OnNextOnErrorOnCompleted2()
        {
            var ex = new Exception();
            bool next = true;
            bool error = false;
            bool completed = false;
            var res = Observer.Create<int>(x => { Assert.AreEqual(42, x); next = true; }, e => { Assert.AreSame(ex, e); error = true; }, () => { completed = true; });
            res.OnNext(42);
            Assert.IsTrue(next);
            Assert.IsFalse(error);
            Assert.IsFalse(completed);
            res.OnError(ex);
            Assert.IsTrue(error);
            Assert.IsFalse(completed);
        }

        [TestMethod]
        public void AsObserver_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.AsObserver<int>(default(IObserver<int>)));
        }

        [TestMethod]
        public void AsObserver_Hides()
        {
            var obs = new MyObserver();
            var res = obs.AsObserver();

            Assert.IsFalse(object.ReferenceEquals(obs, res));
        }

        [TestMethod]
        public void AsObserver_Forwards()
        {
            var obsn = new MyObserver();
            obsn.AsObserver().OnNext(42);
            Assert.AreEqual(obsn.HasOnNext, 42);

            var ex = new Exception();
            var obse = new MyObserver();
            obse.AsObserver().OnError(ex);
            Assert.AreSame(ex, obse.HasOnError);

            var obsc = new MyObserver();
            obsc.AsObserver().OnCompleted();
            Assert.IsTrue(obsc.HasOnCompleted);
        }

        class MyObserver : IObserver<int>
        {
            public void OnNext(int value)
            {
                HasOnNext = value;
            }

            public void OnError(Exception exception)
            {
                HasOnError = exception;
            }

            public void OnCompleted()
            {
                HasOnCompleted = true;
            }

            public int HasOnNext { get; set; }
            public Exception HasOnError { get; set; }
            public bool HasOnCompleted { get; set; }
        }

        [TestMethod]
        public void Observer_Checked_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Checked(default(IObserver<int>)));
        }

        [TestMethod]
        public void Observer_Checked_AlreadyTerminated_Completed()
        {
            var m = 0;
            var n = 0;

            var o = Observer.Create<int>(_ => { m++; }, ex => { Assert.Fail(); }, () => { n++; }).Checked();

            o.OnNext(1);
            o.OnNext(2);
            o.OnCompleted();

            ReactiveAssert.Throws<InvalidOperationException>(() => o.OnCompleted());
            ReactiveAssert.Throws<InvalidOperationException>(() => o.OnError(new Exception()));

            Assert.AreEqual(2, m);
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void Observer_Checked_AlreadyTerminated_Error()
        {
            var m = 0;
            var n = 0;

            var o = Observer.Create<int>(_ => { m++; }, ex => { n++; }, () => { Assert.Fail(); }).Checked();

            o.OnNext(1);
            o.OnNext(2);
            o.OnError(new Exception());

            ReactiveAssert.Throws<InvalidOperationException>(() => o.OnCompleted());
            ReactiveAssert.Throws<InvalidOperationException>(() => o.OnError(new Exception()));

            Assert.AreEqual(2, m);
            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void Observer_Checked_Reentrant_Next()
        {
            var n = 0;

            var o = default(IObserver<int>);
            o = Observer.Create<int>(
                _ =>
                {
                    n++;

                    ReactiveAssert.Throws<InvalidOperationException>(() => o.OnNext(9));
                    ReactiveAssert.Throws<InvalidOperationException>(() => o.OnError(new Exception()));
                    ReactiveAssert.Throws<InvalidOperationException>(() => o.OnCompleted());
                },
                ex =>
                {
                    Assert.Fail();
                },
                () =>
                {
                    Assert.Fail();
                })
                .Checked();

            o.OnNext(1);

            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void Observer_Checked_Reentrant_Error()
        {
            var n = 0;

            var o = default(IObserver<int>);
            o = Observer.Create<int>(
                _ =>
                {
                    Assert.Fail();
                },
                ex =>
                {
                    n++;

                    ReactiveAssert.Throws<InvalidOperationException>(() => o.OnNext(9));
                    ReactiveAssert.Throws<InvalidOperationException>(() => o.OnError(new Exception()));
                    ReactiveAssert.Throws<InvalidOperationException>(() => o.OnCompleted());
                },
                () =>
                {
                    Assert.Fail();
                })
                .Checked();

            o.OnError(new Exception());

            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void Observer_Checked_Reentrant_Completed()
        {
            var n = 0;

            var o = default(IObserver<int>);
            o = Observer.Create<int>(
                _ =>
                {
                    Assert.Fail();
                },
                ex =>
                {
                    Assert.Fail();
                },
                () =>
                {
                    n++;

                    ReactiveAssert.Throws<InvalidOperationException>(() => o.OnNext(9));
                    ReactiveAssert.Throws<InvalidOperationException>(() => o.OnError(new Exception()));
                    ReactiveAssert.Throws<InvalidOperationException>(() => o.OnCompleted());
                })
                .Checked();

            o.OnCompleted();

            Assert.AreEqual(1, n);
        }

        [TestMethod]
        public void Observer_Synchronize_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Synchronize(default(IObserver<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Synchronize(default(IObserver<int>), true));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Synchronize(default(IObserver<int>), new object()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Synchronize(new MyObserver(), default(object)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Synchronize(default(IObserver<int>), new AsyncLock()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Synchronize(new MyObserver(), default(AsyncLock)));
        }

        [TestMethod]
        public void Observer_Synchronize_Monitor_Reentrant1()
        {
            var res = false;
            var inOne = false;

            var s = default(IObserver<int>);

            var o = Observer.Create<int>(x =>
            {
                if (x == 1)
                {
                    inOne = true;
                    s.OnNext(2);
                    inOne = false;
                }
                else if (x == 2)
                {
                    res = inOne;
                }
            });

            s = Observer.Synchronize(o);

            s.OnNext(1);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Observer_Synchronize_Monitor_Reentrant2()
        {
            var res = false;
            var inOne = false;

            var s = default(IObserver<int>);

            var o = Observer.Create<int>(x =>
            {
                if (x == 1)
                {
                    inOne = true;
                    s.OnNext(2);
                    inOne = false;
                }
                else if (x == 2)
                {
                    res = inOne;
                }
            });

            s = Observer.Synchronize(o, new object());

            s.OnNext(1);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Observer_Synchronize_Monitor_Reentrant3()
        {
            var res = false;
            var inOne = false;

            var s = default(IObserver<int>);

            var o = Observer.Create<int>(x =>
            {
                if (x == 1)
                {
                    inOne = true;
                    s.OnNext(2);
                    inOne = false;
                }
                else if (x == 2)
                {
                    res = inOne;
                }
            });

            s = Observer.Synchronize(o, false);

            s.OnNext(1);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Observer_Synchronize_AsyncLock_NonReentrant1()
        {
            var res = false;
            var inOne = false;

            var s = default(IObserver<int>);

            var o = Observer.Create<int>(x =>
            {
                if (x == 1)
                {
                    inOne = true;
                    s.OnNext(2);
                    inOne = false;
                }
                else if (x == 2)
                {
                    res = !inOne;
                }
            });

            s = Observer.Synchronize(o, new AsyncLock());

            s.OnNext(1);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Observer_Synchronize_AsyncLock_NonReentrant2()
        {
            var res = false;
            var inOne = false;

            var s = default(IObserver<int>);

            var o = Observer.Create<int>(x =>
            {
                if (x == 1)
                {
                    inOne = true;
                    s.OnNext(2);
                    inOne = false;
                }
                else if (x == 2)
                {
                    res = !inOne;
                }
            });

            s = Observer.Synchronize(o, true);

            s.OnNext(1);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Observer_Synchronize_AsyncLock()
        {
            {
                var res = false;

                var s = default(IObserver<int>);

                var o = Observer.Create<int>(
                    x => { res = x == 1; },
                    ex => { Assert.Fail(); },
                    () => { Assert.Fail(); }
                );

                s = Observer.Synchronize(o, true);

                s.OnNext(1);

                Assert.IsTrue(res);
            }

            {
                var res = default(Exception);

                var err = new Exception();

                var s = default(IObserver<int>);

                var o = Observer.Create<int>(
                    x => { Assert.Fail(); },
                    ex => { res = ex; },
                    () => { Assert.Fail(); }
                );

                s = Observer.Synchronize(o, true);

                s.OnError(err);

                Assert.AreSame(err, res);
            }

            {
                var res = false;

                var s = default(IObserver<int>);

                var o = Observer.Create<int>(
                    x => { Assert.Fail(); },
                    ex => { Assert.Fail(); },
                    () => { res = true; }
                );

                s = Observer.Synchronize(o, true);

                s.OnCompleted();

                Assert.IsTrue(res);
            }
        }

#if !NO_CDS
        [TestMethod]
        public void Observer_Synchronize_OnCompleted()
        {
            Observer_Synchronize(true);
        }

        [TestMethod]
        public void Observer_Synchronize_OnError()
        {
            Observer_Synchronize(false);
        }

        private void Observer_Synchronize(bool success)
        {
            var busy = false;

            var n = 0;
            var ex = default(Exception);
            var done = false;

            var o = Observer.Create<int>(
                _ =>
                {
                    Assert.IsFalse(busy);
                    busy = true;
                    n++;
                    busy = false;
                },
                _ =>
                {
                    Assert.IsFalse(busy);
                    busy = true;
                    ex = _;
                    busy = false;
                },
                () =>
                {
                    Assert.IsFalse(busy);
                    busy = true;
                    done = true;
                    busy = false;
                }
            );

            var s = Observer.Synchronize(o);

            var N = 10;
            var M = 1000;

            var e = new CountdownEvent(N);
            for (int i = 0; i < N; i++)
            {
                Scheduler.Default.Schedule(() =>
                {
                    for (int j = 0; j < M; j++)
                        s.OnNext(j);
                    e.Signal();
                });
            }

            e.Wait();

            if (success)
            {
                s.OnCompleted();
                Assert.IsTrue(done);
            }
            else
            {
                var err = new Exception();
                s.OnError(err);
                Assert.AreSame(err, ex);
            }

            Assert.AreEqual(n, N * M);
        }
#endif

        [TestMethod]
        public void NotifyOn_Null()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.NotifyOn(default(IObserver<int>), Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.NotifyOn(new MyObserver(), default(IScheduler)));

#if !NO_SYNCCTX
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.NotifyOn(default(IObserver<int>), new MySyncCtx()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.NotifyOn(new MyObserver(), default(SynchronizationContext)));
#endif
        }

        [TestMethod]
        public void NotifyOn_Scheduler_OnCompleted()
        {
            NotifyOn_Scheduler(true);
        }

        [TestMethod]
        public void NotifyOn_Scheduler_OnError()
        {
            NotifyOn_Scheduler(false);
        }

        private void NotifyOn_Scheduler(bool success)
        {
            var e = new ManualResetEvent(false);
            var c = 0;
            var N = 100;
            var err = new Exception();

            var o = Observer.Create<int>(
                x =>
                {
                    c++;
#if DESKTOPCLR
                    Assert.IsTrue(Thread.CurrentThread.IsThreadPoolThread);
#endif
                },
                ex =>
                {
                    Assert.AreSame(err, ex);
                    e.Set();
                },
                () =>
                {
#if DESKTOPCLR
                    Assert.IsTrue(Thread.CurrentThread.IsThreadPoolThread);
#endif
                    e.Set();
                }
            );

            var s = ThreadPoolScheduler.Instance.DisableOptimizations(/* long-running creates a non-threadpool thread */);
            var n = Observer.NotifyOn(o, s);

            new Thread(() =>
            {
                for (int i = 0; i < N; i++)
                    n.OnNext(i);

                if (success)
                    n.OnCompleted();
                else
                    n.OnError(err);
            }).Start();

            e.WaitOne();
            Assert.AreEqual(N, c);
        }

        [TestMethod]
        public void NotifyOn_SyncCtx()
        {
            var lst = new List<int>();
            var don = new ManualResetEvent(false);
            var obs = Observer.Create<int>(x => { lst.Add(x); }, ex => { Assert.Fail(); }, () => { don.Set(); });
            var ctx = new MySyncCtx();
            var res = obs.NotifyOn(ctx);

            for (int i = 0; i < 100; i++)
                obs.OnNext(i);
            obs.OnCompleted();

            don.WaitOne();
            Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(0, 100)));
        }

        class MySyncCtx : SynchronizationContext
        {
            public override void Post(SendOrPostCallback d, object state)
            {
                ThreadPool.QueueUserWorkItem(_ => d(state), state);
            }
        }
    }
}
