// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>The shared <c>Delay</c> scenarios on Rx.NET, plus its two long-running tests.</summary>
/// <remarks>
/// <c>Delay_LongRunning_CancelEarly</c> and <c>Delay_LongRunning_CancelLate</c> are the originals,
/// with their doubles: each starts Rx.NET's long-running dispatch loop on a thread, then disposes
/// the subscription while the loop waits (for input; for a due time) and checks that the loop
/// ends. AsyncRx.NET has no such loop, so they are written against Rx.NET directly.
/// </remarks>
[TestClass]
public sealed class RxDelayTests : DelayTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void Delay_LongRunning_CancelEarly()
    {
        var xs = Observable.Create<int>(observer =>
        {
            return global::System.Reactive.Concurrency.Scheduler.Default.Schedule(
                TimeSpan.FromHours(1),
                () =>
                {
                    observer.OnNext(42);
                });
        });

        var s = new ManualResetEvent(false);
        var e = new ManualResetEvent(false);

        var ys = xs.Delay(TimeSpan.Zero, new MyLongRunning1(s, e));

        var d = ys.Subscribe(_ => { });

        s.WaitOne();
        d.Dispose();
        e.WaitOne();
    }

    [TestMethod]
    public void Delay_LongRunning_CancelLate()
    {
        var xs = Observable.Return(42);

        var s = new ManualResetEvent(false);
        var e = new ManualResetEvent(false);

        var ys = xs.Delay(TimeSpan.FromHours(1), new MyLongRunning2(s, e));

        var d = ys.Subscribe(_ => { });

        s.WaitOne();
        d.Dispose();
        e.WaitOne();
    }

    private sealed class MyLongRunning1(ManualResetEvent start, ManualResetEvent stop)
        : LocalScheduler, ISchedulerLongRunning
    {
        public IDisposable ScheduleLongRunning<TState>(
            TState state,
            Action<TState, ICancelable> action)
        {
            var b = new BooleanDisposable();
            Task.Run(() =>
            {
                start.Set();
                action(state, b);
                stop.Set();
            });
            return b;
        }

        public override IDisposable Schedule<TState>(
            TState state,
            TimeSpan dueTime,
            Func<IScheduler, TState, IDisposable> action)
        {
            throw new NotImplementedException();
        }
    }

    private sealed class MyLongRunning2(ManualResetEvent start, ManualResetEvent stop)
        : LocalScheduler, ISchedulerLongRunning
    {
        public IDisposable ScheduleLongRunning<TState>(
            TState state,
            Action<TState, ICancelable> action)
        {
            var b = new BooleanDisposable();
            Task.Run(() =>
            {
                action(state, b);
                stop.Set();
            });
            return b;
        }

        public override IDisposable Schedule<TState>(
            TState state,
            TimeSpan dueTime,
            Func<IScheduler, TState, IDisposable> action)
        {
            start.Set();
            return Disposable.Empty;
        }
    }
}
