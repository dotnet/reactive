// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>ToObservable</c>-over-a-task scenarios, from Rx.NET's
/// <c>TaskObservableExtensionsTest.cs</c>.
/// </summary>
/// <remarks>
/// 26 of the file's 48 tests: the <c>ToObservable</c> half, over a task with a result and a plain
/// task. Twenty-four are one shape, generated rather than transcribed since they differ only in
/// the task's kind, its outcome (result, exception, cancellation) and the tick it completes at:
/// before the subscription is created, before it is made, before it is disposed, or after. A task
/// already complete when subscribed delivers at the subscription; one completing later delivers
/// when it completes, on the thread completing it; one completing after disposal delivers nothing.
/// The originals run each inside a task factory over an inline task scheduler, because Rx.NET's
/// conversion without a scheduler queues its continuation to the ambient
/// <c>TaskScheduler.Current</c> rather than running it synchronously (its
/// <c>GetTaskContinuationOptions</c> adds <c>ExecuteSynchronously</c> only when a scheduler is
/// given); the Rx.NET target makes each subscription to the leaf under that scheduler instead
/// (<c>InlineTaskScheduler.Subscribing</c>), so the scenario completes the source from a scheduled
/// action and expects delivery in that tick. AsyncRx.NET's conversion runs its continuation
/// synchronously on the completing thread by itself when the scheduler is the immediate one, which
/// is its default, and needs no fixture. The two <c>*_Scheduler</c> tests (inside <c>#if
/// DESKTOPCLR</c> upstream, as the <c>FromAsync</c> ones were) take the
/// <c>FromAsync_Func_Scheduler1</c> shape: delivery on the thread completing the task under the
/// immediate scheduler. The other 22 are <c>ToTask</c>, Rx.NET's bridge to <c>Task</c>, whose
/// AsyncRx.NET form is the awaiter the shared <c>Await</c> and <c>RunAsync</c> scenarios cover,
/// and two argument checks (a static class outside the code-generated stratum, with AsyncRx.NET
/// equivalents in its runner): twenty in the Rx.NET runner, two over an internal helper not
/// migrated.
/// </remarks>
public abstract class TaskToObservableTests : SharedReactiveTest
{
    [TestMethod]
    public void TaskToObservable_NonVoid_Complete_BeforeCreate()
    {
        var scheduler = Scheduler;

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(10, () => taskSource.SetResult(42));

        var res = scheduler.Start(() =>
            taskSource.Task.ToSeq()
        );

        res.Messages.AssertEqual(
            OnNext(200, 42),
            OnCompleted<int>(200)
        );
    }

    [TestMethod]
    public void TaskToObservable_NonVoid_Complete_BeforeSubscribe()
    {
        var scheduler = Scheduler;

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(110, () => taskSource.SetResult(42));

        var res = scheduler.Start(() =>
            taskSource.Task.ToSeq()
        );

        res.Messages.AssertEqual(
            OnNext(200, 42),
            OnCompleted<int>(200)
        );
    }

    [TestMethod]
    public void TaskToObservable_NonVoid_Complete_BeforeDispose()
    {
        var scheduler = Scheduler;

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(300, () => taskSource.SetResult(42));

        var res = scheduler.Start(() =>
            taskSource.Task.ToSeq()
        );

        res.Messages.AssertEqual(
            OnNext(300, 42),
            OnCompleted<int>(300)
        );
    }

    [TestMethod]
    public void TaskToObservable_NonVoid_Complete_AfterDispose()
    {
        var scheduler = Scheduler;

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(1100, () => taskSource.SetResult(42));

        var res = scheduler.Start(() =>
            taskSource.Task.ToSeq()
        );

        res.Messages.AssertEqual();
    }

    [TestMethod]
    public void TaskToObservable_NonVoid_Exception_BeforeCreate()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(10, () => taskSource.SetException(ex));

        var res = scheduler.Start(() =>
            taskSource.Task.ToSeq()
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex)
        );
    }

    [TestMethod]
    public void TaskToObservable_NonVoid_Exception_BeforeSubscribe()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(110, () => taskSource.SetException(ex));

        var res = scheduler.Start(() =>
            taskSource.Task.ToSeq()
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex)
        );
    }

    [TestMethod]
    public void TaskToObservable_NonVoid_Exception_BeforeDispose()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(300, () => taskSource.SetException(ex));

        var res = scheduler.Start(() =>
            taskSource.Task.ToSeq()
        );

        res.Messages.AssertEqual(
            OnError<int>(300, ex)
        );
    }

    [TestMethod]
    public void TaskToObservable_NonVoid_Exception_AfterDispose()
    {
        var scheduler = Scheduler;

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(1100, () => taskSource.SetException(new Exception()));

        var res = scheduler.Start(() =>
            taskSource.Task.ToSeq()
        );

        res.Messages.AssertEqual();
    }

    [TestMethod]
    public void TaskToObservable_NonVoid_Canceled_BeforeCreate()
    {
        var scheduler = Scheduler;

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(10, () => taskSource.SetCanceled());

        var res = scheduler.Start(() =>
            taskSource.Task.ToSeq()
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex => ex is TaskCanceledException)
        );
    }

    [TestMethod]
    public void TaskToObservable_NonVoid_Canceled_BeforeSubscribe()
    {
        var scheduler = Scheduler;

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(110, () => taskSource.SetCanceled());

        var res = scheduler.Start(() =>
            taskSource.Task.ToSeq()
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex => ex is TaskCanceledException)
        );
    }

    [TestMethod]
    public void TaskToObservable_NonVoid_Canceled_BeforeDispose()
    {
        var scheduler = Scheduler;

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(300, () => taskSource.SetCanceled());

        var res = scheduler.Start(() =>
            taskSource.Task.ToSeq()
        );

        res.Messages.AssertEqual(
            OnError<int>(300, ex => ex is TaskCanceledException)
        );
    }

    [TestMethod]
    public void TaskToObservable_NonVoid_Canceled_AfterDispose()
    {
        var scheduler = Scheduler;

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(1100, () => taskSource.SetCanceled());

        var res = scheduler.Start(() =>
            taskSource.Task.ToSeq()
        );

        res.Messages.AssertEqual();
    }

    [TestMethod]
    public async Task TaskToObservable_NonVoid_Scheduler()
    {
        var e = new ManualResetEvent(false);
        var x = default(int);
        var t = default(int);

        var cts = new TaskCompletionSource<int>();

        var xs = cts.Task.ToSeq(ImmediateScheduler);
        await xs.SubscribeAsync(Scheduler, res =>
        {
            x = res;
            t = Environment.CurrentManagedThreadId;
            e.Set();
        });

        cts.SetResult(42);

        e.WaitOne();

        Assert.AreEqual(42, x);
        Assert.AreEqual(Environment.CurrentManagedThreadId, t);
    }

    [TestMethod]
    public void TaskToObservable_Void_Complete_BeforeCreate()
    {
        var scheduler = Scheduler;

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(10, () => taskSource.SetResult(42));

        var res = scheduler.Start(() =>
            ((Task)taskSource.Task).ToSeq()
        );

        res.Messages.AssertEqual(
            OnNext(200, new Unit()),
            OnCompleted<Unit>(200)
        );
    }

    [TestMethod]
    public void TaskToObservable_Void_Complete_BeforeSubscribe()
    {
        var scheduler = Scheduler;

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(110, () => taskSource.SetResult(42));

        var res = scheduler.Start(() =>
            ((Task)taskSource.Task).ToSeq()
        );

        res.Messages.AssertEqual(
            OnNext(200, new Unit()),
            OnCompleted<Unit>(200)
        );
    }

    [TestMethod]
    public void TaskToObservable_Void_Complete_BeforeDispose()
    {
        var scheduler = Scheduler;

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(300, () => taskSource.SetResult(42));

        var res = scheduler.Start(() =>
            ((Task)taskSource.Task).ToSeq()
        );

        res.Messages.AssertEqual(
            OnNext(300, new Unit()),
            OnCompleted<Unit>(300)
        );
    }

    [TestMethod]
    public void TaskToObservable_Void_Complete_AfterDispose()
    {
        var scheduler = Scheduler;

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(1100, () => taskSource.SetResult(42));

        var res = scheduler.Start(() =>
            ((Task)taskSource.Task).ToSeq()
        );

        res.Messages.AssertEqual();
    }

    [TestMethod]
    public void TaskToObservable_Void_Exception_BeforeCreate()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(10, () => taskSource.SetException(ex));

        var res = scheduler.Start(() =>
            ((Task)taskSource.Task).ToSeq()
        );

        res.Messages.AssertEqual(
            OnError<Unit>(200, ex)
        );
    }

    [TestMethod]
    public void TaskToObservable_Void_Exception_BeforeSubscribe()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(110, () => taskSource.SetException(ex));

        var res = scheduler.Start(() =>
            ((Task)taskSource.Task).ToSeq()
        );

        res.Messages.AssertEqual(
            OnError<Unit>(200, ex)
        );
    }

    [TestMethod]
    public void TaskToObservable_Void_Exception_BeforeDispose()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(300, () => taskSource.SetException(ex));

        var res = scheduler.Start(() =>
            ((Task)taskSource.Task).ToSeq()
        );

        res.Messages.AssertEqual(
            OnError<Unit>(300, ex)
        );
    }

    [TestMethod]
    public void TaskToObservable_Void_Exception_AfterDispose()
    {
        var scheduler = Scheduler;

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(1100, () => taskSource.SetException(new Exception()));

        var res = scheduler.Start(() =>
            ((Task)taskSource.Task).ToSeq()
        );

        res.Messages.AssertEqual();
    }

    [TestMethod]
    public void TaskToObservable_Void_Canceled_BeforeCreate()
    {
        var scheduler = Scheduler;

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(10, () => taskSource.SetCanceled());

        var res = scheduler.Start(() =>
            ((Task)taskSource.Task).ToSeq()
        );

        res.Messages.AssertEqual(
            OnError<Unit>(200, ex => ex is TaskCanceledException)
        );
    }

    [TestMethod]
    public void TaskToObservable_Void_Canceled_BeforeSubscribe()
    {
        var scheduler = Scheduler;

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(110, () => taskSource.SetCanceled());

        var res = scheduler.Start(() =>
            ((Task)taskSource.Task).ToSeq()
        );

        res.Messages.AssertEqual(
            OnError<Unit>(200, ex => ex is TaskCanceledException)
        );
    }

    [TestMethod]
    public void TaskToObservable_Void_Canceled_BeforeDispose()
    {
        var scheduler = Scheduler;

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(300, () => taskSource.SetCanceled());

        var res = scheduler.Start(() =>
            ((Task)taskSource.Task).ToSeq()
        );

        res.Messages.AssertEqual(
            OnError<Unit>(300, ex => ex is TaskCanceledException)
        );
    }

    [TestMethod]
    public void TaskToObservable_Void_Canceled_AfterDispose()
    {
        var scheduler = Scheduler;

        var taskSource = new TaskCompletionSource<int>();
        taskSource.Task.ContinueWith(t => _ = t.Exception);

        scheduler.ScheduleAbsolute(1100, () => taskSource.SetCanceled());

        var res = scheduler.Start(() =>
            ((Task)taskSource.Task).ToSeq()
        );

        res.Messages.AssertEqual();
    }

    [TestMethod]
    public async Task TaskToObservable_Void_Scheduler()
    {
        var e = new ManualResetEvent(false);
        var t = default(int);

        var tcs = new TaskCompletionSource<int>();

        var xs = ((Task)tcs.Task).ToSeq(ImmediateScheduler);
        await xs.SubscribeAsync(Scheduler, res =>
        {
            t = Environment.CurrentManagedThreadId;
            e.Set();
        });

        tcs.SetResult(42);

        e.WaitOne();

        Assert.AreEqual(Environment.CurrentManagedThreadId, t);
    }
}
