// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>ObservableSafetyTests</c> on Rx.NET, plus the Rx.NET-only originals.
/// </summary>
/// <remarks>
/// The four <c>*_ObserverThrows</c> originals, as they stand but for MSTest's assertions and the
/// static scheduler named in full: a handler that throws, through an anonymous observer or the
/// original's <c>ObserverBase</c> double, has its exception come back out to whoever called
/// <c>OnNext</c> (the <c>Create</c> callback's scheduled work, or the test scheduler's
/// <c>Start</c> under a <c>CombineLatest</c> producer, which stops at that tick) and the
/// subscription disposed. The originals' two comments on the mechanism are left out.
/// </remarks>
[TestClass]
public sealed class RxObservableSafetyTests : ObservableSafetyTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void ObservableBase_ObserverThrows()
    {
        var ex = new Exception();

        var failed = new ManualResetEvent(false);
        var disposed = new ManualResetEvent(false);
        var err = default(Exception);

        var xs = Observable.Create<int>(observer =>
        {
            global::System.Reactive.Concurrency.Scheduler.Default.Schedule(() =>
            {
                try
                {
                    observer.OnNext(42);
                }
                catch (Exception ex_)
                {
                    err = ex_;
                    failed.Set();
                }
            });

            return () => { disposed.Set(); };
        });

        xs.Subscribe(x =>
        {
            throw ex;
        });

        disposed.WaitOne();
        failed.WaitOne();

        Assert.AreSame(ex, err);
    }

    [TestMethod]
    public void ObservableBase_ObserverThrows_CustomObserver()
    {
        var ex = new Exception();

        var failed = new ManualResetEvent(false);
        var disposed = new ManualResetEvent(false);
        var err = default(Exception);

        var xs = Observable.Create<int>(observer =>
        {
            global::System.Reactive.Concurrency.Scheduler.Default.Schedule(() =>
            {
                try
                {
                    observer.OnNext(42);
                }
                catch (Exception ex_)
                {
                    err = ex_;
                    failed.Set();
                }
            });

            return () => { disposed.Set(); };
        });

        xs.Subscribe(new MyObserver(_ => true, ex));

        disposed.WaitOne();
        failed.WaitOne();

        Assert.AreSame(ex, err);
    }

    [TestMethod]
    public void Producer_ObserverThrows()
    {
        var ex = new Exception();

        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(215, 1),
            OnNext(225, 2),
            OnNext(235, 3)
        );

        var res = xs.CombineLatest(ys, (x, y) => x + y);

        scheduler.ScheduleAbsolute(200, () =>
        {
            res.Subscribe(z =>
            {
                if (z == 4)
                {
                    throw ex;
                }
            });
        });

        try
        {
            scheduler.Start();
            Assert.Fail();
        }
        catch (Exception err)
        {
            Assert.AreSame(ex, err);
        }

        Assert.AreEqual(225, scheduler.Clock);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void Producer_ObserverThrows_CustomObserver()
    {
        var ex = new Exception();

        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(215, 1),
            OnNext(225, 2),
            OnNext(235, 3)
        );

        var res = xs.CombineLatest(ys, (x, y) => x + y);

        scheduler.ScheduleAbsolute(200, () =>
        {
            res.Subscribe(new MyObserver(x => x == 4, ex));
        });

        try
        {
            scheduler.Start();
            Assert.Fail();
        }
        catch (Exception err)
        {
            Assert.AreSame(ex, err);
        }

        Assert.AreEqual(225, scheduler.Clock);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    private class MyObserver : ObserverBase<int>
    {
        private readonly Func<int, bool> _predicate;
        private readonly Exception _exception;

        public MyObserver(Func<int, bool> predicate, Exception exception)
        {
            _predicate = predicate;
            _exception = exception;
        }

        protected override void OnNextCore(int value)
        {
            if (_predicate(value))
            {
                throw _exception;
            }
        }

        protected override void OnErrorCore(Exception error)
        {
        }

        protected override void OnCompletedCore()
        {
        }
    }
}
