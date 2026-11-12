// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Runtime.CompilerServices;

using Microsoft.Reactive.Testing.Async;

namespace Tests.Microsoft.Reactive.Testing.Async;

/// <summary>
/// Tests for the state-passing shape of <see cref="IAsyncScheduler"/>: the state reaches the
/// action on every overload and every scheduler, the returned handle still cancels, and the
/// stateless extension overloads route through the same path.
/// </summary>
[TestClass]
public class AsyncSchedulerStateTest
{
    private sealed class Observed
    {
        public string? State;
        public long RanAt = -1;
        public bool WasCancelled;
    }

    [TestMethod]
    public void Immediate_ScheduleAsync_passes_state_to_the_action()
    {
        var scheduler = new TestAsyncScheduler();
        var observed = new Observed();

        var schedule = scheduler.ScheduleAsync((observed, scheduler, "payload"), static (s, _) =>
        {
            s.observed.State = s.Item3;
            s.observed.RanAt = s.scheduler.Clock;
            return default;
        });
        Assert.IsTrue(schedule.IsCompleted);

        scheduler.Start();

        Assert.AreEqual("payload", observed.State);
        Assert.AreEqual(0L, observed.RanAt);
    }

    [TestMethod]
    public void Relative_ScheduleAsync_passes_state_to_the_action_at_the_due_time()
    {
        var scheduler = new TestAsyncScheduler();
        var observed = new Observed();

        var schedule = scheduler.ScheduleAsync((observed, scheduler, "payload"), TimeSpan.FromTicks(300), static (s, _) =>
        {
            s.observed.State = s.Item3;
            s.observed.RanAt = s.scheduler.Clock;
            return default;
        });
        Assert.IsTrue(schedule.IsCompleted);

        scheduler.Start();

        Assert.AreEqual("payload", observed.State);
        Assert.AreEqual(300L, observed.RanAt);
    }

    [TestMethod]
    public void Absolute_ScheduleAsync_passes_state_to_the_action_at_the_due_time()
    {
        var scheduler = new TestAsyncScheduler();
        var observed = new Observed();

        var schedule = scheduler.ScheduleAsync((observed, scheduler, "payload"), new DateTimeOffset(300, TimeSpan.Zero), static (s, _) =>
        {
            s.observed.State = s.Item3;
            s.observed.RanAt = s.scheduler.Clock;
            return default;
        });
        Assert.IsTrue(schedule.IsCompleted);

        scheduler.Start();

        Assert.AreEqual("payload", observed.State);
        Assert.AreEqual(300L, observed.RanAt);
    }

    [TestMethod]
    public void Disposing_the_handle_of_stateful_timed_work_cancels_it()
    {
        var scheduler = new TestAsyncScheduler();
        var observed = new Observed();

        scheduler.ScheduleAbsolute(100, async _ =>
        {
            var handle = await scheduler.ScheduleAsync(observed, TimeSpan.FromTicks(500), static (o, ct) =>
            {
                o.RanAt = 0;
                o.WasCancelled = ct.IsCancellationRequested;
                return default;
            });

            await scheduler.Delay(TimeSpan.FromTicks(100));
            await handle.DisposeAsync();
        });

        scheduler.Start();

        Assert.AreEqual(-1L, observed.RanAt, "cancelled work must not run");
    }

    [TestMethod]
    public void Stateless_absolute_extension_overload_runs_at_the_due_time()
    {
        var scheduler = new TestAsyncScheduler();
        long? ranAt = null;

        var schedule = scheduler.ScheduleAsync(_ => { ranAt = scheduler.Clock; return default; }, new DateTimeOffset(250, TimeSpan.Zero));
        Assert.IsTrue(schedule.IsCompleted);

        scheduler.Start();

        Assert.AreEqual(250L, ranAt);
    }

    [TestMethod]
    public void Stateless_extension_overload_passes_the_cancellation_token_and_honours_the_handle()
    {
        var scheduler = new TestAsyncScheduler();
        bool? cancelledWhenRun = null;
        var disposedWorkRan = false;

        scheduler.ScheduleAbsolute(100, async _ =>
        {
            // Immediate work joins the back of the ready queue, so it runs after this item
            // finishes: this one is left alone and must see an uncancelled token...
            await scheduler.ScheduleAsync(ct => { cancelledWhenRun = ct.IsCancellationRequested; return default; });

            // ...and this one is cancelled through its handle before it gets to run.
            var handle = await scheduler.ScheduleAsync(_ => { disposedWorkRan = true; return default; });
            await handle.DisposeAsync();
        });

        scheduler.Start();

        Assert.IsFalse(cancelledWhenRun, "value of IsCancellationRequested observed when callback ran");
        Assert.IsFalse(disposedWorkRan, "cancelled work must not run");
    }

    [TestMethod]
    public async Task ExecuteAsync_passes_state_to_the_action()
    {
        var box = new StrongBox<int>();

        await ImmediateAsyncScheduler.Instance.ExecuteAsync(box, static (b, _) => { b.Value = 7; return default; });

        Assert.AreEqual(7, box.Value);
    }

    [TestMethod]
    public async Task ExecuteAsync_with_result_passes_state_to_the_action()
    {
        var result = await ImmediateAsyncScheduler.Instance.ExecuteAsync(20, static (n, _) => new ValueTask<int>(n + 22));

        Assert.AreEqual(42, result);
    }

    [TestMethod]
    public void ImmediateAsyncScheduler_passes_state_to_the_action()
    {
        var box = new StrongBox<int>();

        var schedule = ImmediateAsyncScheduler.Instance.ScheduleAsync((box, 42), static (s, _) => { s.box.Value = s.Item2; return default; });

        Assert.IsTrue(schedule.IsCompleted);
        Assert.AreEqual(42, box.Value);
    }

    [TestMethod]
    public async Task TaskPoolAsyncScheduler_passes_state_to_the_action()
    {
        var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);

        await TaskPoolAsyncScheduler.Default.ScheduleAsync((tcs, 42), static (s, _) => { s.tcs.SetResult(s.Item2); return default; });

        Assert.AreEqual(42, await tcs.Task.WaitAsync(TimeSpan.FromSeconds(10)));
    }

    [TestMethod]
    public async Task TaskPoolAsyncScheduler_passes_state_to_timed_action()
    {
        var tcs = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);

        await TaskPoolAsyncScheduler.Default.ScheduleAsync((tcs, 42), TimeSpan.FromMilliseconds(10), static (s, _) => { s.tcs.SetResult(s.Item2); return default; });

        Assert.AreEqual(42, await tcs.Task.WaitAsync(TimeSpan.FromSeconds(10)));
    }

    [TestMethod]
    public async Task TaskPoolAsyncScheduler_disposing_the_handle_cancels_timed_work()
    {
        var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

        var handle = await TaskPoolAsyncScheduler.Default.ScheduleAsync(tcs, TimeSpan.FromSeconds(30), static (t, ct) => { t.TrySetResult(ct.IsCancellationRequested); return default; });
        await handle.DisposeAsync();

        // The scheduler's own Delay is cancelled by the handle; the action itself never runs, so
        // the source stays pending. Give it a moment to prove that, then move on.
        var completed = await Task.WhenAny(tcs.Task, Task.Delay(200));
        Assert.AreNotSame(tcs.Task, completed, "cancelled work must not run");
    }
}
