// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using Microsoft.Reactive.Testing.Async;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Microsoft.Reactive.Testing.Async;

/// <summary>
/// Tests for the virtual-time pump itself: clock semantics, the canonical
/// continuation-priority ordering, failure surfacing, and execution-shape equivalence.
/// </summary>
[TestClass]
public class TestAsyncSchedulerPumpTest
{
    [TestMethod]
    public void AsyncLocal_state_does_not_leak_between_work_items()
    {
        // The thread pool runs each work item under its own captured ExecutionContext, so an
        // AsyncLocal written inside one item is invisible to the next. The pump must do the
        // same: AsyncRx.NET's AsyncGate detects re-entrancy with an AsyncLocal, and a leaked
        // value makes a later, unrelated work item think it already holds the lock.
        var scheduler = new TestAsyncScheduler();
        var local = new AsyncLocal<int>();
        var seen = new List<int>();

        scheduler.ScheduleAbsolute(100, _ =>
        {
            seen.Add(local.Value);
            local.Value = 42;
            return default;
        });

        scheduler.ScheduleAbsolute(100, _ =>
        {
            seen.Add(local.Value);
            return default;
        });

        scheduler.ScheduleAbsolute(200, async _ =>
        {
            local.Value = 7;
            await scheduler.Delay(TimeSpan.FromTicks(10));
            seen.Add(local.Value); // survives the flow's own suspension...
        });

        scheduler.ScheduleAbsolute(220, _ =>
        {
            seen.Add(local.Value); // ...but not into another work item
            return default;
        });

        scheduler.Start();

        CollectionAssert.AreEqual(new[] { 0, 0, 7, 0 }, seen);
    }

    [TestMethod]
    public void Start_with_nothing_scheduled_returns_with_clock_unmoved()
    {
        var scheduler = new TestAsyncScheduler();

        scheduler.Start();

        Assert.AreEqual(0, scheduler.Clock);
    }

    [TestMethod]
    public void ScheduleAbsolute_runs_work_at_its_due_tick()
    {
        var scheduler = new TestAsyncScheduler();
        var log = new List<long>();

        scheduler.ScheduleAbsolute(300, _ => { log.Add(scheduler.Clock); return default; });
        scheduler.ScheduleAbsolute(100, _ => { log.Add(scheduler.Clock); return default; });

        scheduler.Start();

        CollectionAssert.AreEqual(new long[] { 100, 300 }, log);
        Assert.AreEqual(300, scheduler.Clock);
    }

    [TestMethod]
    public void Same_tick_timers_run_in_scheduling_order()
    {
        var scheduler = new TestAsyncScheduler();
        var log = new List<string>();

        scheduler.ScheduleAbsolute(100, _ => { log.Add("first"); return default; });
        scheduler.ScheduleAbsolute(100, _ => { log.Add("second"); return default; });

        scheduler.Start();

        CollectionAssert.AreEqual(new[] { "first", "second" }, log);
    }

    [TestMethod]
    public void Immediate_ScheduleAsync_completes_synchronously_and_runs_in_current_drain()
    {
        var scheduler = new TestAsyncScheduler();
        long? ranAt = null;

        var schedule = scheduler.ScheduleAsync(_ => { ranAt = scheduler.Clock; return default; });
        Assert.IsTrue(schedule.IsCompleted);

        scheduler.Start();

        Assert.AreEqual(0L, ranAt);
    }

    [TestMethod]
    public void Timed_ScheduleAsync_runs_at_virtual_due_time_on_the_pump_thread()
    {
        var scheduler = new TestAsyncScheduler();
        long? ranAt = null;
        Thread? ranOn = null;
        SynchronizationContext? context = null;

        var schedule = scheduler.ScheduleAsync(
            _ =>
            {
                ranAt = scheduler.Clock;
                ranOn = Thread.CurrentThread;
                context = SynchronizationContext.Current;
                return default;
            },
            TimeSpan.FromTicks(300));
        Assert.IsTrue(schedule.IsCompleted);

        scheduler.Start();

        Assert.AreEqual(300L, ranAt);
        Assert.AreSame(Thread.CurrentThread, ranOn);

        // The pump deliberately runs with no SynchronizationContext: a custom context would
        // stop the runtime inlining ConfigureAwait(false) continuations (punting them to the
        // thread pool), and contextless execution is what production AsyncRx code sees on
        // TaskPoolAsyncScheduler anyway.
        Assert.IsNull(context);
    }

    [TestMethod]
    public void Nested_relative_scheduling_accumulates_virtual_time()
    {
        var scheduler = new TestAsyncScheduler();
        long? innerRanAt = null;

        scheduler.ScheduleAbsolute(300, async _ =>
        {
            await scheduler.ScheduleAsync(_ => { innerRanAt = scheduler.Clock; return default; }, TimeSpan.FromTicks(200));
        });

        scheduler.Start();

        Assert.AreEqual(500L, innerRanAt);
    }

    [TestMethod]
    public void Task_Yield_escapes_to_the_thread_pool_and_fails_informatively()
    {
        // With no SynchronizationContext current, Task.Yield resumes on the thread pool.
        // That is an escape from the virtual-time pump; the harness reports it (as an escape
        // if the pool thread has resumed the continuation by the time the pump runs out of
        // work, otherwise as work still incomplete) instead of hanging, producing a corrupt
        // trace, or — if the pool thread completes the work before the pump looks at it —
        // passing silently.
        var scheduler = new TestAsyncScheduler();

        scheduler.ScheduleAbsolute(100, async _ =>
        {
            await Task.Yield();
        });

        Assert.ThrowsExactly<TestAsyncSchedulerException>(scheduler.Start);
    }

    [TestMethod]
    public void Forced_yield_resumption_runs_ahead_of_other_work_queued_at_the_same_tick()
    {
        var scheduler = new TestAsyncScheduler(ExecutionShape.ForcedYield);
        var log = new List<string>();

        scheduler.ScheduleAbsolute(100, async _ =>
        {
            log.Add("A1");

            // Queues same-tick work at the back of the ready deque...
            await scheduler.ScheduleAsync(_ => { log.Add("C"); return default; });

            // ...then suspends; the resumption is a continuation, so it front-queues.
            await scheduler.YieldPoint();
            log.Add("A2");
        });

        scheduler.Start();

        CollectionAssert.AreEqual(new[] { "A1", "A2", "C" }, log);
    }

    [TestMethod]
    public void ConfigureAwait_false_awaiter_resumes_inline_when_the_pump_completes_its_task()
    {
        var scheduler = new TestAsyncScheduler();
        var log = new List<string>();
        var tcs = new TaskCompletionSource();

        scheduler.ScheduleAbsolute(100, async _ =>
        {
            log.Add("A1");
            await tcs.Task.ConfigureAwait(false);
            log.Add("A2");
        });

        scheduler.ScheduleAbsolute(200, _ =>
        {
            log.Add("B1");
            tcs.TrySetResult();

            // Inline (depth-first) propagation: A's continuation has already run by here.
            log.Add("B2");
            return default;
        });

        scheduler.Start();

        CollectionAssert.AreEqual(new[] { "A1", "B1", "A2", "B2" }, log);
    }

    [TestMethod]
    public void Unconfigured_awaiter_behaves_identically_because_no_context_is_current()
    {
        // Awaits without ConfigureAwait(false) capture nothing under the pump (no context is
        // installed), so configured and unconfigured awaits resume the same way: inline.
        var scheduler = new TestAsyncScheduler();
        var log = new List<string>();
        var tcs = new TaskCompletionSource();

        scheduler.ScheduleAbsolute(100, async _ =>
        {
            log.Add("A1");
            await tcs.Task;
            log.Add("A2");
        });

        scheduler.ScheduleAbsolute(200, _ =>
        {
            log.Add("B1");
            tcs.TrySetResult();
            log.Add("B2");
            return default;
        });

        scheduler.Start();

        CollectionAssert.AreEqual(new[] { "A1", "B1", "A2", "B2" }, log);
    }

    [TestMethod]
    public void Disposing_a_timed_schedule_cancels_it_without_failing_the_pump()
    {
        var scheduler = new TestAsyncScheduler();
        var ran = false;
        IAsyncDisposable? subscription = null;

        scheduler.ScheduleAbsolute(50, async _ =>
        {
            subscription = await scheduler.ScheduleAsync(_ => { ran = true; return default; }, TimeSpan.FromTicks(500));
        });

        scheduler.ScheduleAbsolute(100, async _ =>
        {
            await subscription!.DisposeAsync();
        });

        scheduler.Start();

        Assert.IsFalse(ran);
    }

    [TestMethod]
    public void Synchronous_exception_from_scheduled_work_fails_Start_with_the_original_exception()
    {
        var scheduler = new TestAsyncScheduler();
        var error = new InvalidOperationException("boom");

        scheduler.ScheduleAbsolute(100, _ => throw error);

        var thrown = Assert.ThrowsExactly<InvalidOperationException>(scheduler.Start);
        Assert.AreSame(error, thrown);
    }

    [TestMethod]
    public void Exception_after_a_genuine_suspension_fails_Start_with_the_original_exception()
    {
        var scheduler = new TestAsyncScheduler();
        var error = new InvalidOperationException("boom");

        scheduler.ScheduleAbsolute(100, async _ =>
        {
            await scheduler.Delay(TimeSpan.FromTicks(50));
            throw error;
        });

        var thrown = Assert.ThrowsExactly<InvalidOperationException>(scheduler.Start);
        Assert.AreSame(error, thrown);
    }

    [TestMethod]
    public void Work_awaiting_something_that_never_completes_fails_informatively_instead_of_hanging()
    {
        var scheduler = new TestAsyncScheduler();
        var never = new TaskCompletionSource();

        scheduler.ScheduleAbsolute(100, async _ =>
        {
            await never.Task;
        });

        var thrown = Assert.ThrowsExactly<TestAsyncSchedulerException>(scheduler.Start);
        StringAssert.Contains(thrown.Message, "never completed");
    }

    [TestMethod]
    public void Work_escaping_to_a_real_thread_fails_informatively()
    {
        var scheduler = new TestAsyncScheduler();

        scheduler.ScheduleAbsolute(100, async _ =>
        {
            await Task.Run(() => 42);
        });

        // Depending on timing this surfaces as an escape or as work that had not completed
        // when the pump ran out of virtual-time events; both are the harness's informative
        // failure, never a hang and never a silent pass.
        Assert.ThrowsExactly<TestAsyncSchedulerException>(scheduler.Start);
    }

    [TestMethod]
    public void Escaped_code_that_never_touches_the_harness_is_still_reported()
    {
        // The escape is detected when the escaped code's ExecutionContext is restored onto
        // the foreign thread, before it runs — not by inspecting the work item's task
        // afterwards, which a fast pool thread could already have completed. To prove that,
        // the escaped code does nothing harness-related, and the pump is made to wait until
        // it has run, so the only possible outcome is the escape diagnosis (never "work
        // never completed", and never a silent pass). It also shows the escaped work's own
        // completion, which follows on the same foreign thread, is folded into that one
        // failure rather than reported as a second one.
        var scheduler = new TestAsyncScheduler();
        using var escapedCodeRan = new ManualResetEventSlim();

        scheduler.ScheduleAbsolute(100, async _ =>
        {
            await Task.Run(() => escapedCodeRan.Set());
        });

        scheduler.ScheduleAbsolute(200, _ =>
        {
            escapedCodeRan.Wait();
            return default;
        });

        var thrown = Assert.ThrowsExactly<TestAsyncSchedulerException>(scheduler.Start);
        StringAssert.Contains(thrown.Message, "escaped the virtual-time pump");
    }

    [TestMethod]
    public void YieldPoint_completes_synchronously_in_synchronous_completion_shape()
    {
        var scheduler = new TestAsyncScheduler(ExecutionShape.SynchronousCompletion);

        Assert.IsTrue(scheduler.YieldPoint().GetAwaiter().IsCompleted);
    }

    [TestMethod]
    public void YieldPoint_suspends_in_forced_yield_shape()
    {
        var scheduler = new TestAsyncScheduler(ExecutionShape.ForcedYield);
        var log = new List<string>();

        scheduler.ScheduleAbsolute(100, async _ =>
        {
            Assert.IsFalse(scheduler.YieldPoint().GetAwaiter().IsCompleted);

            log.Add("before");
            await scheduler.YieldPoint();
            log.Add("after");
        });

        scheduler.Start();

        CollectionAssert.AreEqual(new[] { "before", "after" }, log);
    }

    [TestMethod]
    [DataRow(ExecutionShape.SynchronousCompletion)]
    [DataRow(ExecutionShape.ForcedYield)]
    public void Execution_shapes_produce_identical_traces(ExecutionShape shape)
    {
        // The trace-equivalence meta-property: the same scenario, with yield points at every
        // step, logs the same events at the same virtual times in both shapes.
        var log = RunShapeScenario(shape);

        CollectionAssert.AreEqual(RunShapeScenario(ExecutionShape.SynchronousCompletion), log);
    }

    private static List<string> RunShapeScenario(ExecutionShape shape)
    {
        var scheduler = new TestAsyncScheduler(shape);
        var log = new List<string>();

        scheduler.ScheduleAbsolute(100, async _ =>
        {
            log.Add($"A1@{scheduler.Clock}");
            await scheduler.YieldPoint();
            log.Add($"A2@{scheduler.Clock}");

            await scheduler.ScheduleAsync(async _ =>
            {
                log.Add($"C1@{scheduler.Clock}");
                await scheduler.YieldPoint();
                log.Add($"C2@{scheduler.Clock}");
            });

            await scheduler.YieldPoint();
            log.Add($"A3@{scheduler.Clock}");
        });

        scheduler.ScheduleAbsolute(100, _ => { log.Add($"B@{scheduler.Clock}"); return default; });
        scheduler.ScheduleAbsolute(200, _ => { log.Add($"D@{scheduler.Clock}"); return default; });

        scheduler.Start();

        return log;
    }
}
