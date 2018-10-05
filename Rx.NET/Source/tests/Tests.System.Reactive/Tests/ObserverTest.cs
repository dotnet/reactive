// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{

    public partial class ObserverTest : ReactiveTest
    {
        [Fact]
        public void ToObserver_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.ToObserver(default(Action<Notification<int>>)));
        }

        [Fact]
        public void ToObserver_NotificationOnNext()
        {
            var i = 0;
            Action<Notification<int>> next = n =>
            {
                Assert.Equal(i++, 0);
                Assert.Equal(n.Kind, NotificationKind.OnNext);
                Assert.Equal(n.Value, 42);
                Assert.Equal(n.Exception, null);
                Assert.True(n.HasValue);
            };
            next.ToObserver().OnNext(42);
        }

        [Fact]
        public void ToObserver_NotificationOnError()
        {
            var ex = new Exception();
            var i = 0;
            Action<Notification<int>> next = n =>
            {
                Assert.Equal(i++, 0);
                Assert.Equal(n.Kind, NotificationKind.OnError);
                Assert.Same(n.Exception, ex);
                Assert.False(n.HasValue);
            };
            next.ToObserver().OnError(ex);
        }

        [Fact]
        public void ToObserver_NotificationOnCompleted()
        {
            var ex = new Exception();
            var i = 0;
            Action<Notification<int>> next = n =>
            {
                Assert.Equal(i++, 0);
                Assert.Equal(n.Kind, NotificationKind.OnCompleted);
                Assert.False(n.HasValue);
            };
            next.ToObserver().OnCompleted();
        }

        [Fact]
        public void ToNotifier_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.ToNotifier<int>(default));
        }

        [Fact]
        public void ToNotifier_Forwards()
        {
            var obsn = new MyObserver();
            obsn.ToNotifier()(Notification.CreateOnNext(42));
            Assert.Equal(obsn.HasOnNext, 42);

            var ex = new Exception();
            var obse = new MyObserver();
            obse.ToNotifier()(Notification.CreateOnError<int>(ex));
            Assert.Same(ex, obse.HasOnError);

            var obsc = new MyObserver();
            obsc.ToNotifier()(Notification.CreateOnCompleted<int>());
            Assert.True(obsc.HasOnCompleted);
        }

        [Fact]
        public void Create_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Create<int>(default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Create<int>(default, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Create<int>(_ => { }, default(Action)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Create<int>(default, (Exception _) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Create<int>(_ => { }, default(Action<Exception>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Create<int>(default, (Exception _) => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Create<int>(_ => { }, default, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Create<int>(_ => { }, (Exception _) => { }, default));
        }

        [Fact]
        public void Create_OnNext()
        {
            var next = false;
            var res = Observer.Create<int>(x => { Assert.Equal(42, x); next = true; });
            res.OnNext(42);
            Assert.True(next);
            res.OnCompleted();
        }

        [Fact]
        public void Create_OnNext_HasError()
        {
            var ex = new Exception();
            var e_ = default(Exception);

            var next = false;
            var res = Observer.Create<int>(x => { Assert.Equal(42, x); next = true; });
            res.OnNext(42);
            Assert.True(next);

            try
            {
                res.OnError(ex);
                Assert.True(false);
            }
            catch (Exception e)
            {
                e_ = e;
            }
            Assert.Same(ex, e_);
        }

        [Fact]
        public void Create_OnNextOnCompleted()
        {
            var next = false;
            var completed = false;
            var res = Observer.Create<int>(x => { Assert.Equal(42, x); next = true; }, () => { completed = true; });
            res.OnNext(42);
            Assert.True(next);
            Assert.False(completed);
            res.OnCompleted();
            Assert.True(completed);
        }

        [Fact]
        public void Create_OnNextOnCompleted_HasError()
        {
            var ex = new Exception();
            var e_ = default(Exception);

            var next = false;
            var completed = false;
            var res = Observer.Create<int>(x => { Assert.Equal(42, x); next = true; }, () => { completed = true; });
            res.OnNext(42);
            Assert.True(next);
            Assert.False(completed);
            try
            {
                res.OnError(ex);
                Assert.True(false);
            }
            catch (Exception e)
            {
                e_ = e;
            }
            Assert.Same(ex, e_);
            Assert.False(completed);
        }

        [Fact]
        public void Create_OnNextOnError()
        {
            var ex = new Exception();
            var next = true;
            var error = false;
            var res = Observer.Create<int>(x => { Assert.Equal(42, x); next = true; }, e => { Assert.Same(ex, e); error = true; });
            res.OnNext(42);
            Assert.True(next);
            Assert.False(error);
            res.OnError(ex);
            Assert.True(error);
        }

        [Fact]
        public void Create_OnNextOnError_HitCompleted()
        {
            var ex = new Exception();
            var next = true;
            var error = false;
            var res = Observer.Create<int>(x => { Assert.Equal(42, x); next = true; }, e => { Assert.Same(ex, e); error = true; });
            res.OnNext(42);
            Assert.True(next);
            Assert.False(error);
            res.OnCompleted();
            Assert.False(error);
        }

        [Fact]
        public void Create_OnNextOnErrorOnCompleted1()
        {
            var ex = new Exception();
            var next = true;
            var error = false;
            var completed = false;
            var res = Observer.Create<int>(x => { Assert.Equal(42, x); next = true; }, e => { Assert.Same(ex, e); error = true; }, () => { completed = true; });
            res.OnNext(42);
            Assert.True(next);
            Assert.False(error);
            Assert.False(completed);
            res.OnCompleted();
            Assert.True(completed);
            Assert.False(error);
        }

        [Fact]
        public void Create_OnNextOnErrorOnCompleted2()
        {
            var ex = new Exception();
            var next = true;
            var error = false;
            var completed = false;
            var res = Observer.Create<int>(x => { Assert.Equal(42, x); next = true; }, e => { Assert.Same(ex, e); error = true; }, () => { completed = true; });
            res.OnNext(42);
            Assert.True(next);
            Assert.False(error);
            Assert.False(completed);
            res.OnError(ex);
            Assert.True(error);
            Assert.False(completed);
        }

        [Fact]
        public void AsObserver_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.AsObserver<int>(default));
        }

        [Fact]
        public void AsObserver_Hides()
        {
            var obs = new MyObserver();
            var res = obs.AsObserver();

            Assert.False(ReferenceEquals(obs, res));
        }

        [Fact]
        public void AsObserver_Forwards()
        {
            var obsn = new MyObserver();
            obsn.AsObserver().OnNext(42);
            Assert.Equal(obsn.HasOnNext, 42);

            var ex = new Exception();
            var obse = new MyObserver();
            obse.AsObserver().OnError(ex);
            Assert.Same(ex, obse.HasOnError);

            var obsc = new MyObserver();
            obsc.AsObserver().OnCompleted();
            Assert.True(obsc.HasOnCompleted);
        }

        private class MyObserver : IObserver<int>
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

        [Fact]
        public void Observer_Checked_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Checked(default(IObserver<int>)));
        }

        [Fact]
        public void Observer_Checked_AlreadyTerminated_Completed()
        {
            var m = 0;
            var n = 0;

            var o = Observer.Create<int>(_ => { m++; }, ex => { Assert.True(false); }, () => { n++; }).Checked();

            o.OnNext(1);
            o.OnNext(2);
            o.OnCompleted();

            ReactiveAssert.Throws<InvalidOperationException>(() => o.OnCompleted());
            ReactiveAssert.Throws<InvalidOperationException>(() => o.OnError(new Exception()));

            Assert.Equal(2, m);
            Assert.Equal(1, n);
        }

        [Fact]
        public void Observer_Checked_AlreadyTerminated_Error()
        {
            var m = 0;
            var n = 0;

            var o = Observer.Create<int>(_ => { m++; }, ex => { n++; }, () => { Assert.True(false); }).Checked();

            o.OnNext(1);
            o.OnNext(2);
            o.OnError(new Exception());

            ReactiveAssert.Throws<InvalidOperationException>(() => o.OnCompleted());
            ReactiveAssert.Throws<InvalidOperationException>(() => o.OnError(new Exception()));

            Assert.Equal(2, m);
            Assert.Equal(1, n);
        }

        [Fact]
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
                    Assert.True(false);
                },
                () =>
                {
                    Assert.True(false);
                })
                .Checked();

            o.OnNext(1);

            Assert.Equal(1, n);
        }

        [Fact]
        public void Observer_Checked_Reentrant_Error()
        {
            var n = 0;

            var o = default(IObserver<int>);
            o = Observer.Create<int>(
                _ =>
                {
                    Assert.True(false);
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
                    Assert.True(false);
                })
                .Checked();

            o.OnError(new Exception());

            Assert.Equal(1, n);
        }

        [Fact]
        public void Observer_Checked_Reentrant_Completed()
        {
            var n = 0;

            var o = default(IObserver<int>);
            o = Observer.Create<int>(
                _ =>
                {
                    Assert.True(false);
                },
                ex =>
                {
                    Assert.True(false);
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

            Assert.Equal(1, n);
        }

        [Fact]
        public void Observer_Synchronize_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Synchronize(default(IObserver<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Synchronize(default(IObserver<int>), true));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Synchronize(default(IObserver<int>), new object()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Synchronize(new MyObserver(), default(object)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Synchronize(default(IObserver<int>), new AsyncLock()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.Synchronize(new MyObserver(), default(AsyncLock)));
        }

        [Fact]
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

            Assert.True(res);
        }

        [Fact]
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

            Assert.True(res);
        }

        [Fact]
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

            Assert.True(res);
        }

        [Fact]
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

            Assert.True(res);
        }

        [Fact]
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

            Assert.True(res);
        }

        [Fact]
        public void Observer_Synchronize_AsyncLock()
        {
            {
                var res = false;

                var s = default(IObserver<int>);

                var o = Observer.Create<int>(
                    x => { res = x == 1; },
                    ex => { Assert.True(false); },
                    () => { Assert.True(false); }
                );

                s = Observer.Synchronize(o, true);

                s.OnNext(1);

                Assert.True(res);
            }

            {
                var res = default(Exception);

                var err = new Exception();

                var s = default(IObserver<int>);

                var o = Observer.Create<int>(
                    x => { Assert.True(false); },
                    ex => { res = ex; },
                    () => { Assert.True(false); }
                );

                s = Observer.Synchronize(o, true);

                s.OnError(err);

                Assert.Same(err, res);
            }

            {
                var res = false;

                var s = default(IObserver<int>);

                var o = Observer.Create<int>(
                    x => { Assert.True(false); },
                    ex => { Assert.True(false); },
                    () => { res = true; }
                );

                s = Observer.Synchronize(o, true);

                s.OnCompleted();

                Assert.True(res);
            }
        }

        [Fact]
        public void Observer_Synchronize_OnCompleted()
        {
            Observer_Synchronize(true);
        }

        [Fact]
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
                    Assert.False(busy);
                    busy = true;
                    n++;
                    busy = false;
                },
                _ =>
                {
                    Assert.False(busy);
                    busy = true;
                    ex = _;
                    busy = false;
                },
                () =>
                {
                    Assert.False(busy);
                    busy = true;
                    done = true;
                    busy = false;
                }
            );

            var s = Observer.Synchronize(o);

            var N = 10;
            var M = 1000;

            var e = new CountdownEvent(N);
            for (var i = 0; i < N; i++)
            {
                Scheduler.Default.Schedule(() =>
                {
                    for (var j = 0; j < M; j++)
                    {
                        s.OnNext(j);
                    }

                    e.Signal();
                });
            }

            e.Wait();

            if (success)
            {
                s.OnCompleted();
                Assert.True(done);
            }
            else
            {
                var err = new Exception();
                s.OnError(err);
                Assert.Same(err, ex);
            }

            Assert.Equal(n, N * M);
        }

        [Fact]
        public void NotifyOn_Null()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.NotifyOn(default(IObserver<int>), Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.NotifyOn(new MyObserver(), default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.NotifyOn(default(IObserver<int>), new MySyncCtx()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.NotifyOn(new MyObserver(), default(SynchronizationContext)));
        }

#if !NO_THREAD
        [Fact]
        public void NotifyOn_Scheduler_OnCompleted()
        {
            NotifyOn_Scheduler(true);
        }

        [Fact]
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
                    Assert.True(Thread.CurrentThread.IsThreadPoolThread);
#endif
                },
                ex =>
                {
                    Assert.Same(err, ex);
                    e.Set();
                },
                () =>
                {
#if DESKTOPCLR
                    Assert.True(Thread.CurrentThread.IsThreadPoolThread);
#endif
                    e.Set();
                }
            );

            var s = ThreadPoolScheduler.Instance.DisableOptimizations(/* long-running creates a non-threadpool thread */);
            var n = Observer.NotifyOn(o, s);

            new Thread(() =>
            {
                for (var i = 0; i < N; i++)
                {
                    n.OnNext(i);
                }

                if (success)
                {
                    n.OnCompleted();
                }
                else
                {
                    n.OnError(err);
                }
            }).Start();

            e.WaitOne();
            Assert.Equal(N, c);
        }
#endif

        [Fact]
        public void NotifyOn_SyncCtx()
        {
            var lst = new List<int>();
            var don = new ManualResetEvent(false);
            var obs = Observer.Create<int>(x => { lst.Add(x); }, ex => { Assert.True(false); }, () => { don.Set(); });
            var ctx = new MySyncCtx();
            var res = obs.NotifyOn(ctx);

            for (var i = 0; i < 100; i++)
            {
                obs.OnNext(i);
            }

            obs.OnCompleted();

            don.WaitOne();
            Assert.True(lst.SequenceEqual(Enumerable.Range(0, 100)));
        }

        private class MySyncCtx : SynchronizationContext
        {
            public override void Post(SendOrPostCallback d, object state)
            {
                Task.Run(() => d(state));
            }
        }

        [Fact]
        public void Observer_ToProgress_ArgumentChecking()
        {
            var s = Scheduler.Immediate;
            var o = Observer.Create<int>(_ => { });

            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.ToProgress<int>(default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.ToProgress<int>(default, s));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.ToProgress(o, default));
        }

        [Fact]
        public void Observer_ToProgress()
        {
            var xs = new List<int>();

            var p = Observer.Create<int>(xs.Add).ToProgress();

            p.Report(42);
            p.Report(43);

            Assert.True(xs.SequenceEqual(new[] { 42, 43 }));
        }

        [Fact]
        public void Observer_ToProgress_Scheduler()
        {
            var s = new TestScheduler();

            var o = s.CreateObserver<int>();
            var p = o.ToProgress(s);

            s.ScheduleAbsolute(200, () =>
            {
                p.Report(42);
                p.Report(43);
            });

            s.Start();

            o.Messages.AssertEqual(
                OnNext(201, 42),
                OnNext(202, 43)
            );
        }

        [Fact]
        public void Progress_ToObserver_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observer.ToObserver(default(IProgress<int>)));
        }

        [Fact]
        public void Progress_ToObserver()
        {
            var xs = new List<int>();

            var p = new MyProgress<int>(xs.Add);

            var o = p.ToObserver();

            o.OnNext(42);
            o.OnNext(43);

            Assert.True(xs.SequenceEqual(new[] { 42, 43 }));
        }

        private class MyProgress<T> : IProgress<T>
        {
            private readonly Action<T> _report;

            public MyProgress(Action<T> report)
            {
                _report = report;
            }

            public void Report(T value)
            {
                _report(value);
            }
        }
    }
}
