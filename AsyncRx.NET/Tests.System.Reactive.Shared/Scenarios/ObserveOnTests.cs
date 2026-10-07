// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>ObserveOn</c> scenarios, from Rx.NET's <c>ObserveOnTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// Twelve of the file's 23 tests. The virtual-time tests run over the test scheduler and over a
/// synchronization context that posts to it; <c>ObserveOn</c> delivers every notification through
/// its scheduler, so where the original expects each one tick after the source's
/// (<c>OnNext(231, 3)</c>), and the source released when the completion is delivered
/// (<c>Subscribe(200, 531)</c>), the shared text writes <c>ScheduledAt</c>. The real-time tests
/// drive two scheduler doubles, both over <see cref="SchedulerDouble"/>: <c>MyScheduler</c>, which
/// runs work inline and records what it throws, and the shared
/// <see cref="TestLongRunningScheduler"/>, which runs each unit of work on its own thread; they are <c>async Task</c> methods that await the doubles' signals and a
/// <c>TaskCompletionSource</c> where the original blocks on events, keeping the original's
/// <c>ManualResetEvent</c> only where it stands between the test thread and the scheduler's.
/// </para>
/// <para>
/// Not here: the six Windows Forms and WPF tests, which Rx.NET's own runner compiles only under
/// <c>HAS_WINFORMS</c> and <c>HAS_WPF</c>; the two <c>*_ArgumentChecking</c> tests, the
/// code-generated stratum; and three tests over Rx.NET's own schedulers (<c>EventLoopScheduler</c>
/// and the long-running optimization of the task pool scheduler), which live in the Rx.NET
/// runner, <c>RxObserveOnTests</c>.
/// </para>
/// </remarks>
public abstract class ObserveOnTests : SharedReactiveTest
{
    /// <summary>A scheduler that runs work inline and records the exception it throws.</summary>
    /// <param name="target">The target under test.</param>
    /// <remarks>
    /// Rx.NET's <c>MyScheduler</c>, a private nested type of its <c>ObserveOnReactiveTest</c>: the
    /// one scenario that uses it checks that an exception thrown by the observer inside
    /// <c>ObserveOn</c>'s scheduled delivery reaches the scheduler, rather than being swallowed.
    /// </remarks>
    private sealed class MyScheduler(IRxTarget target) : SchedulerDouble(target, "MyScheduler")
    {
        private readonly TaskCompletionSource _faulted =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        /// <summary>Completes when a scheduled action has thrown.</summary>
        public Task Faulted => _faulted.Task;

        /// <summary>The exception a scheduled action threw, if any.</summary>
        public Exception? Exception { get; private set; }

        /// <inheritdoc/>
        public override DateTimeOffset Now => throw new NotImplementedException();

        /// <inheritdoc/>
        public override async ValueTask ScheduleAsync(
            ScheduledWork work,
            CancellationToken cancellationToken)
        {
            try
            {
                await work(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Exception = ex;
                _faulted.TrySetResult();
            }
        }
    }

    /// <summary>A synchronization context that posts its callbacks to the test scheduler.</summary>
    /// <remarks>
    /// Rx.NET's <c>MyCtx</c>, which schedules each <c>Post</c> on the scheduler it was given.
    /// Posting at the current clock is what Rx.NET's <c>TestScheduler</c> bumps by one tick and
    /// the async pump runs at the same tick, so the scenario reads the delivery ticks through
    /// <c>ScheduledAt</c>. A context is the same type on both targets, so one class serves both.
    /// </remarks>
    private sealed class MyCtx(TestSchedulerRef scheduler) : SynchronizationContext
    {
        public override void Post(SendOrPostCallback d, object? state) =>
            scheduler.ScheduleAbsolute(scheduler.Clock, () => d(state));
    }

    [TestMethod]
    public void ObserveOn_Scheduler_Completed()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(120, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnNext(310, 5),
            OnNext(470, 6),
            OnCompleted<int>(530)
        );

        var results = Scheduler.Start(() =>
            xs.ObserveOn(Scheduler)
        );

        results.Messages.AssertEqual(
            OnNext(ScheduledAt(230), 3),
            OnNext(ScheduledAt(240), 4),
            OnNext(ScheduledAt(310), 5),
            OnNext(ScheduledAt(470), 6),
            OnCompleted<int>(ScheduledAt(530))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, ScheduledAt(530))
        );
    }

    [TestMethod]
    public void ObserveOn_Scheduler_Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(120, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnNext(310, 5),
            OnNext(470, 6),
            OnError<int>(530, ex)
        );

        var results = Scheduler.Start(() =>
            xs.ObserveOn(Scheduler)
        );

        results.Messages.AssertEqual(
            OnNext(ScheduledAt(230), 3),
            OnNext(ScheduledAt(240), 4),
            OnNext(ScheduledAt(310), 5),
            OnNext(ScheduledAt(470), 6),
            OnError<int>(ScheduledAt(530), ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, ScheduledAt(530))
        );
    }

    [TestMethod]
    public void ObserveOn_Scheduler_Dispose()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(120, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnNext(310, 5),
            OnNext(470, 6)
        );

        var results = Scheduler.Start(() =>
            xs.ObserveOn(Scheduler)
        );

        results.Messages.AssertEqual(
            OnNext(ScheduledAt(230), 3),
            OnNext(ScheduledAt(240), 4),
            OnNext(ScheduledAt(310), 5),
            OnNext(ScheduledAt(470), 6)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void ObserveOn_Scheduler_SameTime()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(210, 2)
        );

        var results = Scheduler.Start(() =>
            xs.ObserveOn(Scheduler)
        );

        results.Messages.AssertEqual(
            OnNext(ScheduledAt(210), 1),
            OnNext(ScheduledAt(210, 2), 2)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void ObserveOn_SynchronizationContext_Simple()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(120, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnNext(310, 5),
            OnNext(470, 6),
            OnCompleted<int>(530)
        );

        var results = Scheduler.Start(() =>
            xs.ObserveOn(new MyCtx(Scheduler))
        );

        results.Messages.AssertEqual(
            OnNext(ScheduledAt(230), 3),
            OnNext(ScheduledAt(240), 4),
            OnNext(ScheduledAt(310), 5),
            OnNext(ScheduledAt(470), 6),
            OnCompleted<int>(ScheduledAt(530))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, ScheduledAt(530))
        );
    }

    [TestMethod]
    public async Task ObserveOn_Scheduler_OnNextThrows()
    {
        var scheduler = new MyScheduler(Target);

        await Seq.Range(0, 10, DefaultScheduler).ObserveOn(scheduler).SubscribeAsync(
            Scheduler,
            x =>
            {
                if (x == 5)
                {
                    throw new Exception();
                }
            });

        await scheduler.Faulted;
        Assert.IsNotNull(scheduler.Exception);
    }

    [TestMethod]
    public async Task ObserveOn_LongRunning_Simple()
    {
        var scheduler = new TestLongRunningScheduler(Target);

        var s = CreateSubject<int>();

        var end = new TaskCompletionSource();
        var lst = new List<int>();
        await s.ObserveOn(scheduler).SubscribeAsync(
            Scheduler,
            lst.Add,
            ex => end.TrySetException(ex),
            () => end.TrySetResult());

        await s.OnNextAsync(1);
        await s.OnNextAsync(2);
        await s.OnNextAsync(3);
        await s.OnCompletedAsync();

        await end.Task;

        Assert.IsTrue(lst.SequenceEqual([1, 2, 3]));
    }

    [TestMethod]
    public async Task ObserveOn_LongRunning_Error()
    {
        var scheduler = new TestLongRunningScheduler(Target);

        var s = CreateSubject<int>();

        var end = new TaskCompletionSource();
        var err = default(Exception);
        await s.ObserveOn(scheduler).SubscribeAsync(
            Scheduler,
            _ => { },
            ex => { err = ex; end.TrySetResult(); });

        await s.OnNextAsync(1);
        await s.OnNextAsync(2);
        await s.OnNextAsync(3);
        var ex_ = new Exception();
        await s.OnErrorAsync(ex_);

        await end.Task;

        Assert.AreSame(ex_, err);
    }

    [TestMethod]
    public async Task ObserveOn_LongRunning_TimeVariance()
    {
        var scheduler = new TestLongRunningScheduler(Target);

        var s = CreateSubject<int>();

        var end = new TaskCompletionSource();
        await s.ObserveOn(scheduler).SubscribeAsync(
            Scheduler,
            _ => { },
            ex => end.TrySetException(ex),
            () => end.TrySetResult());

        await s.OnNextAsync(1); // Ensure active
        await scheduler.Started;

        Thread.Sleep(100); // Try to enter the dispatcher event wait state

        for (var i = 0; i < 1000; i++)
        {
            if (i % 100 == 0)
            {
                Thread.Sleep(10);
            }

            await s.OnNextAsync(i);
        }

        await s.OnCompletedAsync();

        await end.Task;
    }

    [TestMethod]
    public async Task ObserveOn_LongRunning_HoldUpDuringDispatchAndFail()
    {
        var scheduler = new TestLongRunningScheduler(Target);

        var s = CreateSubject<int>();

        var onNext = new ManualResetEvent(false);
        var resume = new ManualResetEvent(false);
        var lst = new List<int>();
        var err = default(Exception);
        var end = new TaskCompletionSource();
        await s.ObserveOn(scheduler).SubscribeAsync(
            Scheduler,
            x => { lst.Add(x); onNext.Set(); resume.WaitOne(); },
            ex_ => { err = ex_; end.TrySetResult(); });

        await s.OnNextAsync(1);
        onNext.WaitOne();

        await s.OnNextAsync(2);
        await s.OnNextAsync(3);
        var ex = new Exception();
        await s.OnErrorAsync(ex);

        resume.Set();

        await end.Task;

        Assert.IsTrue(lst.SequenceEqual([1, 2, 3]));
        Assert.AreSame(ex, err);
    }

    [TestMethod]
    public async Task ObserveOn_LongRunning_Cancel()
    {
        var scheduler = new TestLongRunningScheduler(Target);

        var s = CreateSubject<int>();

        var lst = new List<int>();
        var running = new ManualResetEvent(false);
        var d = await s.ObserveOn(scheduler).SubscribeAsync(
            Scheduler,
            x => { lst.Add(x); running.Set(); });

        await s.OnNextAsync(0);
        await scheduler.Started;

        await s.OnNextAsync(1);
        await s.OnNextAsync(2);
        await s.OnNextAsync(3);

        running.WaitOne();
        await d.DisposeAsync();
        await scheduler.Stopped;

        await s.OnNextAsync(4);

        Assert.IsTrue(lst.Count > 0 && !lst.Contains(4));
    }

    [TestMethod]
    public async Task ObserveOn_LongRunning_OnNextThrows()
    {
        var scheduler = new TestLongRunningScheduler(Target);

        var s = CreateSubject<int>();

        var lst = new List<int>();
        var running = new ManualResetEvent(false);
        var d = await s.ObserveOn(scheduler).SubscribeAsync(
            Scheduler,
            x => { lst.Add(x); running.Set(); if (x == 3) { throw new Exception(); } });

        await s.OnNextAsync(0);
        await scheduler.Started;

        await s.OnNextAsync(1);
        await s.OnNextAsync(2);
        await s.OnNextAsync(3);

        running.WaitOne();
        await s.OnNextAsync(4);

        // The original waits for the scheduler's "stopped" event: its one unit of work is the
        // whole dispatch loop, which the observer's exception ends. On AsyncRx.NET each burst is
        // a unit of its own, and the one that delivered 0 can end before 1, 2 and 3 arrive, so
        // the shared text waits for the signal that means what the original's does here.
        await scheduler.Faulted;

        Assert.IsNotNull(scheduler.Exception);
    }
}
