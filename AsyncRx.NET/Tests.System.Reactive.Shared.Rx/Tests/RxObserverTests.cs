// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;

using Microsoft.Reactive.Testing;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>ObserverTests</c> on Rx.NET, plus the Rx.NET-only originals.
/// </summary>
/// <remarks>
/// The 38 originals that stay Rx.NET-only, as they stand but for MSTest's assertions, <c>null!</c>
/// and <c>default!</c> where the nullable context would warn, and the static schedulers named in
/// full (the shared base uses their names for its own members); the <c>DESKTOPCLR</c> assertions
/// are left out, as they are from the upstream build. Eleven are argument checks over
/// <c>Observer</c> (not in the code-generated stratum, which covers the operator surface). Four
/// are the two-handler <c>Create</c> forms, which AsyncRx.NET's <c>AsyncObserver.Create</c> does
/// not offer. The rest are over members AsyncRx.NET does not have: <c>ToObserver</c> and
/// <c>ToNotifier</c> over <c>Notification</c>, <c>AsObserver</c>, <c>Checked</c> (AsyncRx.NET's
/// observer refuses and drops by itself, which the AsyncRx.NET runner claims in its own terms),
/// <c>Synchronize</c> over a monitor, an <c>AsyncLock</c> or ten threads (the <c>AsyncGate</c>
/// form has Step 80's tests), <c>NotifyOn</c> over a scheduler or a synchronization context, and
/// <c>ToProgress</c> with its inverse.
/// </remarks>
[TestClass]
public sealed class RxObserverTests : ObserverTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void ToObserver_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.ToObserver(default(Action<Notification<int>>)!));
    }

    [TestMethod]
    public void ToObserver_NotificationOnNext()
    {
        var i = 0;
        Action<Notification<int>> next = n =>
        {
            Assert.AreEqual(0, i++);
            Assert.AreEqual(NotificationKind.OnNext, n.Kind);
            Assert.AreEqual(42, n.Value);
            Assert.IsNull(n.Exception);
            Assert.IsTrue(n.HasValue);
        };
        next.ToObserver().OnNext(42);
    }

    [TestMethod]
    public void ToObserver_NotificationOnError()
    {
        var ex = new Exception();
        var i = 0;
        Action<Notification<int>> next = n =>
        {
            Assert.AreEqual(0, i++);
            Assert.AreEqual(NotificationKind.OnError, n.Kind);
            Assert.AreSame(n.Exception, ex);
            Assert.IsFalse(n.HasValue);
        };
        next.ToObserver().OnError(ex);
    }

    [TestMethod]
    public void ToObserver_NotificationOnCompleted()
    {
        var ex = new Exception();
        var i = 0;
        Action<Notification<int>> next = n =>
        {
            Assert.AreEqual(0, i++);
            Assert.AreEqual(NotificationKind.OnCompleted, n.Kind);
            Assert.IsFalse(n.HasValue);
        };
        next.ToObserver().OnCompleted();
    }

    [TestMethod]
    public void ToNotifier_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.ToNotifier<int>(default!));
    }

    [TestMethod]
    public void ToNotifier_Forwards()
    {
        var obsn = new MyObserver();
        obsn.ToNotifier()(Notification.CreateOnNext(42));
        Assert.AreEqual(42, obsn.HasOnNext);

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
#pragma warning disable IDE0350 // Use implicitly typed lambda - we want to be explicit here for clarity
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.Create<int>(default!));
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.Create<int>(default!, () => { }));
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.Create<int>(_ => { }, default(Action)!));
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.Create<int>(default!, (Exception _) => { }));
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.Create<int>(_ => { }, default(Action<Exception>)!));
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.Create<int>(default!, (Exception _) => { }, () => { }));
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.Create<int>(_ => { }, default!, () => { }));
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.Create<int>(_ => { }, (Exception _) => { }, default!));
#pragma warning restore IDE0350
    }

    [TestMethod]
    public void Create_OnNextOnCompleted()
    {
        var next = false;
        var completed = false;
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

        var next = false;
        var completed = false;
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
        var next = true;
        var error = false;
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
        var next = true;
        var error = false;
        var res = Observer.Create<int>(x => { Assert.AreEqual(42, x); next = true; }, e => { Assert.AreSame(ex, e); error = true; });
        res.OnNext(42);
        Assert.IsTrue(next);
        Assert.IsFalse(error);
        res.OnCompleted();
        Assert.IsFalse(error);
    }

    [TestMethod]
    public void AsObserver_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.AsObserver<int>(default!));
    }

    [TestMethod]
    public void AsObserver_Hides()
    {
        var obs = new MyObserver();
        var res = obs.AsObserver();

        Assert.AreNotSame(obs, res);
    }

    [TestMethod]
    public void AsObserver_Forwards()
    {
        var obsn = new MyObserver();
        obsn.AsObserver().OnNext(42);
        Assert.AreEqual(42, obsn.HasOnNext);

        var ex = new Exception();
        var obse = new MyObserver();
        obse.AsObserver().OnError(ex);
        Assert.AreSame(ex, obse.HasOnError);

        var obsc = new MyObserver();
        obsc.AsObserver().OnCompleted();
        Assert.IsTrue(obsc.HasOnCompleted);
    }

    [TestMethod]
    public void Observer_Checked_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.Checked(default(IObserver<int>)!));
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

        Assert.ThrowsExactly<InvalidOperationException>(() => o.OnCompleted());
        Assert.ThrowsExactly<InvalidOperationException>(() => o.OnError(new Exception()));

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

        Assert.ThrowsExactly<InvalidOperationException>(() => o.OnCompleted());
        Assert.ThrowsExactly<InvalidOperationException>(() => o.OnError(new Exception()));

        Assert.AreEqual(2, m);
        Assert.AreEqual(1, n);
    }

    [TestMethod]
    public void Observer_Checked_Reentrant_Next()
    {
        var n = 0;

        IObserver<int> o = null!;
        o = Observer.Create<int>(
            _ =>
            {
                n++;

                Assert.ThrowsExactly<InvalidOperationException>(() => o.OnNext(9));
                Assert.ThrowsExactly<InvalidOperationException>(() => o.OnError(new Exception()));
                Assert.ThrowsExactly<InvalidOperationException>(() => o.OnCompleted());
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

        IObserver<int> o = null!;
        o = Observer.Create<int>(
            _ =>
            {
                Assert.Fail();
            },
            ex =>
            {
                n++;

                Assert.ThrowsExactly<InvalidOperationException>(() => o.OnNext(9));
                Assert.ThrowsExactly<InvalidOperationException>(() => o.OnError(new Exception()));
                Assert.ThrowsExactly<InvalidOperationException>(() => o.OnCompleted());
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

        IObserver<int> o = null!;
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

                Assert.ThrowsExactly<InvalidOperationException>(() => o.OnNext(9));
                Assert.ThrowsExactly<InvalidOperationException>(() => o.OnError(new Exception()));
                Assert.ThrowsExactly<InvalidOperationException>(() => o.OnCompleted());
            })
            .Checked();

        o.OnCompleted();

        Assert.AreEqual(1, n);
    }

    [TestMethod]
    public void Observer_Synchronize_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.Synchronize(default(IObserver<int>)!));

        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.Synchronize(default(IObserver<int>)!, true));

        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.Synchronize(default(IObserver<int>)!, new object()));
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.Synchronize(new MyObserver(), default(object)!));

        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.Synchronize(default(IObserver<int>)!, new AsyncLock()));
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.Synchronize(new MyObserver(), default(AsyncLock)!));
    }

    [TestMethod]
    public void Observer_Synchronize_Monitor_Reentrant1()
    {
        var res = false;
        var inOne = false;

        IObserver<int> s = null!;

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

        IObserver<int> s = null!;

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

        IObserver<int> s = null!;

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

        IObserver<int> s = null!;

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

        IObserver<int> s = null!;

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

            IObserver<int> s = null!;

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

            IObserver<int> s = null!;

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

            IObserver<int> s = null!;

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

    [TestMethod]
    public void NotifyOn_Null()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.NotifyOn(default(IObserver<int>)!, global::System.Reactive.Concurrency.Scheduler.Immediate));
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.NotifyOn(new MyObserver(), default(IScheduler)!));
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.NotifyOn(default(IObserver<int>)!, new MySyncCtx()));
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.NotifyOn(new MyObserver(), default(SynchronizationContext)!));
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

    [TestMethod]
    public void NotifyOn_SyncCtx()
    {
        var lst = new List<int>();
        var don = new ManualResetEvent(false);
        var obs = Observer.Create<int>(x => { lst.Add(x); }, ex => { Assert.Fail(); }, () => { don.Set(); });
        var ctx = new MySyncCtx();
        var res = obs.NotifyOn(ctx);

        for (var i = 0; i < 100; i++)
        {
            obs.OnNext(i);
        }

        obs.OnCompleted();

        don.WaitOne();
        Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(0, 100)));
    }

    [TestMethod]
    public void Observer_ToProgress_ArgumentChecking()
    {
        var s = global::System.Reactive.Concurrency.Scheduler.Immediate;
        var o = Observer.Create<int>(_ => { });

        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.ToProgress<int>(default!));

        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.ToProgress<int>(default!, s));
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.ToProgress(o, default!));
    }

    [TestMethod]
    public void Observer_ToProgress()
    {
        var xs = new List<int>();

        var p = Observer.Create<int>(xs.Add).ToProgress();

        p.Report(42);
        p.Report(43);

        Assert.IsTrue(xs.SequenceEqual([42, 43]));
    }

    [TestMethod]
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

    [TestMethod]
    public void Progress_ToObserver_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => Observer.ToObserver(default(IProgress<int>)!));
    }

    [TestMethod]
    public void Progress_ToObserver()
    {
        var xs = new List<int>();

        var p = new MyProgress<int>(xs.Add);

        var o = p.ToObserver();

        o.OnNext(42);
        o.OnNext(43);

        Assert.IsTrue(xs.SequenceEqual([42, 43]));
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
        for (var i = 0; i < N; i++)
        {
            global::System.Reactive.Concurrency.Scheduler.Default.Schedule(() =>
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
            Assert.IsTrue(done);
        }
        else
        {
            var err = new Exception();
            s.OnError(err);
            Assert.AreSame(err, ex);
        }

        Assert.AreEqual(N * M, n);
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
            },
            ex =>
            {
                Assert.AreSame(err, ex);
                e.Set();
            },
            () =>
            {
                e.Set();
            }
        );

        var s = global::System.Reactive.Concurrency.ThreadPoolScheduler.Instance.DisableOptimizations();
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
        Assert.AreEqual(N, c);
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
        public Exception? HasOnError { get; set; }
        public bool HasOnCompleted { get; set; }
    }

    private class MySyncCtx : SynchronizationContext
    {
        public override void Post(SendOrPostCallback d, object? state)
        {
            Task.Run(() => d(state));
        }
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
