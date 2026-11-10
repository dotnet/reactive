// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

using Microsoft.Reactive.Testing;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>TaskToObservableTests</c> on Rx.NET, plus the Rx.NET-only originals.
/// </summary>
/// <remarks>
/// The 20 originals that stay Rx.NET-only, as they stand but for MSTest's assertions and
/// <c>null!</c> where the nullable context would warn: the two argument checks over
/// <c>TaskObservableExtensions</c>, a static class the code-generated stratum does not cover, and
/// eighteen of the twenty <c>ToTask</c> tests (the two <c>*_Cancel</c> tests over the one-shot
/// scheduler call <c>ContinueOnScheduler</c>, an internal helper the original reaches through
/// <c>InternalsVisibleTo</c>, and are not migrated). <c>ToTask</c> is Rx.NET's bridge from a
/// sequence to a <c>Task</c> carrying the last element, with a cancellation token, an
/// <c>AsyncState</c> and, since Rx.NET 6, a scheduler the task's continuation resumes on (the
/// <c>OneshotScheduler</c> tests, over the original's nested one-shot scheduler double).
/// AsyncRx.NET's bridge is its awaiter, <c>GetAwaiter</c> and <c>RunAsync</c>, whose scenarios are
/// shared; it has no <c>ToTask</c>, so what these claim about the task itself has no counterpart
/// there. The <c>_doneTask</c> field and the constructor that completes it are the original's.
/// </remarks>
[TestClass]
public sealed class RxTaskObservableExtensionsTests : TaskToObservableTests
{
    private readonly Task<int> _doneTask;

    public RxTaskObservableExtensionsTests()
    {
        var tcs = new TaskCompletionSource<int>();
        tcs.SetResult(42);
        _doneTask = tcs.Task;
    }

    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void TaskToObservable_NonVoid_ArgumentChecking()
    {
        var s = global::System.Reactive.Concurrency.Scheduler.Immediate;

        Assert.ThrowsExactly<ArgumentNullException>(() => TaskObservableExtensions.ToObservable((Task<int>)null!));

        Assert.ThrowsExactly<ArgumentNullException>(() => TaskObservableExtensions.ToObservable((Task<int>)null!, s));
        Assert.ThrowsExactly<ArgumentNullException>(() => TaskObservableExtensions.ToObservable(_doneTask, default(IScheduler)!));

        var tcs = new TaskCompletionSource<int>();
        var task = tcs.Task;
        Assert.ThrowsExactly<ArgumentNullException>(() => task.ToObservable().Subscribe(null!));
    }

    [TestMethod]
    public void TaskToObservable_Void_ArgumentChecking()
    {
        var s = global::System.Reactive.Concurrency.Scheduler.Immediate;

        Assert.ThrowsExactly<ArgumentNullException>(() => TaskObservableExtensions.ToObservable(null!));

        Assert.ThrowsExactly<ArgumentNullException>(() => TaskObservableExtensions.ToObservable(null!, s));
        Assert.ThrowsExactly<ArgumentNullException>(() => TaskObservableExtensions.ToObservable((Task)_doneTask, default(IScheduler)!));

        var tcs = new TaskCompletionSource<int>();
        Task task = tcs.Task;
        Assert.ThrowsExactly<ArgumentNullException>(() => task.ToObservable().Subscribe(null!));
    }

    [TestMethod]
    public void ObservableToTask_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => TaskObservableExtensions.ToTask<int>(null!));
        Assert.ThrowsExactly<ArgumentNullException>(() => TaskObservableExtensions.ToTask<int>(null!, new CancellationToken()));
        Assert.ThrowsExactly<ArgumentNullException>(() => TaskObservableExtensions.ToTask<int>(null!, new object()));
        Assert.ThrowsExactly<ArgumentNullException>(() => TaskObservableExtensions.ToTask<int>(null!, new CancellationToken(), new object()));

        Assert.ThrowsExactly<ArgumentNullException>(() => TaskObservableExtensions.ToTask(Observable.Never<int>(), scheduler: null!));
        Assert.ThrowsExactly<ArgumentNullException>(() => TaskObservableExtensions.ToTask(Observable.Never<int>(), new CancellationToken(), scheduler: null!));
        Assert.ThrowsExactly<ArgumentNullException>(() => TaskObservableExtensions.ToTask(Observable.Never<int>(), new object(), scheduler: null!));
        Assert.ThrowsExactly<ArgumentNullException>(() => TaskObservableExtensions.ToTask(Observable.Never<int>(), new CancellationToken(), new object(), scheduler: null!));
    }

    [TestMethod]
    public void ObservableToTaskNoValue()
    {
        var scheduler = new TestScheduler();

        var xs = Observable.Empty<int>(scheduler);

        var continuation = xs.ToTask();
        scheduler.Start();

        Assert.IsTrue(continuation.IsFaulted);
        Assert.IsTrue(continuation.Exception!.InnerExceptions.Count == 1 && continuation.Exception!.InnerExceptions[0] is InvalidOperationException);

        Assert.AreEqual(1, scheduler.Clock);
    }

    [TestMethod]
    public void ObservableToTaskSingleValue()
    {
        var scheduler = new TestScheduler();

        var xs = Observable.Return(5, scheduler);

        var continuation = xs.ToTask();
        scheduler.Start();

        Assert.IsTrue(continuation.IsCompleted);
        Assert.AreEqual(5, continuation.Result);

        Assert.AreEqual(1, scheduler.Clock);
    }

    [TestMethod]
    public void ObservableToTaskMultipleValues()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnNext(200, 3),
            OnCompleted<int>(200)
        );

        var continuation = xs.ToTask();
        scheduler.Start();

        Assert.IsTrue(continuation.IsCompleted);
        Assert.AreEqual(3, continuation.Result);

        xs.Subscriptions.AssertEqual(
            Subscribe(0, 200)
        );
    }

    [TestMethod]
    public void ObservableToTaskException()
    {
        var scheduler = new TestScheduler();

        var ex = new InvalidOperationException();

        var xs = scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnError<int>(200, ex)
        );

        var continuation = xs.ToTask();
        scheduler.Start();

        Assert.IsTrue(continuation.IsFaulted);
        var ag = continuation.Exception;
        Assert.IsNotNull(ag);
        Assert.AreEqual(1, ag.InnerExceptions.Count);
        Assert.AreEqual(ex, ag.InnerExceptions[0]);

        xs.Subscriptions.AssertEqual(
            Subscribe(0, 200)
        );
    }

    [TestMethod]
    public void ObservableToTaskCancelled()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnCompleted<int>(200)
        );

        var cts = new CancellationTokenSource();
        var continuation = xs.ToTask(cts.Token);
        scheduler.ScheduleAbsolute(125, cts.Cancel);

        scheduler.Start();

        Assert.IsTrue(continuation.IsCanceled);

        xs.Subscriptions.AssertEqual(
            Subscribe(0, 125)
        );
    }

    [TestMethod]
    public void ObservableToTaskWithStateSingleValue()
    {
        var state = "bar";

        var scheduler = new TestScheduler();

        var xs = Observable.Return(5, scheduler);

        var continuation = xs.ToTask(state);
        Assert.AreSame(continuation.AsyncState, state);

        scheduler.Start();

        Assert.IsTrue(continuation.IsCompleted);
        Assert.AreEqual(5, continuation.Result);

        Assert.AreEqual(1, scheduler.Clock);
    }

    [TestMethod]
    public void ToTask_Scheduler_Resumed_On_Thread_Success()
    {
        var scheduler = new OneshotScheduler();

        var task = Observable.Return(1).ToTask(scheduler);

        Assert.IsTrue(scheduler.HasTask);

        scheduler.Run();

        Assert.IsFalse(scheduler.HasTask);

        Assert.IsTrue(task.IsCompleted);
    }

    [TestMethod]
    public void ToTask_Scheduler_Resumed_On_Thread_Failure()
    {
        var scheduler = new OneshotScheduler();

        var task = Observable.Throw<int>(new InvalidOperationException("failure")).ToTask(scheduler);

        Assert.IsTrue(scheduler.HasTask);

        scheduler.Run();

        Assert.IsFalse(scheduler.HasTask);

        Assert.IsTrue(task.IsFaulted);
        Assert.IsTrue(task.Exception!.InnerException is InvalidOperationException);
        Assert.AreEqual("failure", task.Exception!.InnerException!.Message);
    }

    [TestMethod]
    public void ToTask_Scheduler_Resumed_On_Thread_Success_With_State()
    {
        var scheduler = new OneshotScheduler();

        var task = Observable.Return(1).ToTask("state", scheduler);

        Assert.IsTrue(scheduler.HasTask);

        scheduler.Run();

        Assert.IsFalse(scheduler.HasTask);

        Assert.IsTrue(task.IsCompleted);

        Assert.AreEqual("state", task.AsyncState);
    }

    [TestMethod]
    public void ToTask_Scheduler_Resumed_On_Thread_Failure_With_State()
    {
        var scheduler = new OneshotScheduler();

        var task = Observable.Throw<int>(new InvalidOperationException("failure")).ToTask("state", scheduler);

        Assert.IsTrue(scheduler.HasTask);

        scheduler.Run();

        Assert.IsFalse(scheduler.HasTask);

        Assert.IsTrue(task.IsFaulted);
        Assert.IsTrue(task.Exception!.InnerException is InvalidOperationException);
        Assert.AreEqual("failure", task.Exception!.InnerException!.Message);
        Assert.AreEqual("state", task.AsyncState);
    }

    [TestMethod]
    public void ToTask_Scheduler_Resumed_On_Thread_Success_With_Cancellation()
    {
        var scheduler = new OneshotScheduler();

        var task = Observable.Return(1).ToTask(cancellationToken: default, scheduler: scheduler);

        Assert.IsTrue(scheduler.HasTask);

        scheduler.Run();

        Assert.IsFalse(scheduler.HasTask);

        Assert.IsTrue(task.IsCompleted);
    }

    [TestMethod]
    public void ToTask_Scheduler_Resumed_On_Thread_Failure_With_Cancellation()
    {
        var scheduler = new OneshotScheduler();

        var task = Observable.Throw<int>(new InvalidOperationException("failure")).ToTask(cancellationToken: default, scheduler: scheduler);

        Assert.IsTrue(scheduler.HasTask);

        scheduler.Run();

        Assert.IsFalse(scheduler.HasTask);

        Assert.IsTrue(task.IsFaulted);
        Assert.IsTrue(task.Exception!.InnerException is InvalidOperationException);
        Assert.AreEqual("failure", task.Exception!.InnerException!.Message);
    }

    [TestMethod]
    public void ToTask_Scheduler_Resumed_On_Thread_Cancel_With_Cancellation()
    {
        var scheduler = new OneshotScheduler();

        var cts = new CancellationTokenSource();
        var task = Observable.Never<int>().ToTask(cts.Token, scheduler);

        Assert.IsFalse(scheduler.HasTask);

        cts.Cancel();

        Assert.IsTrue(scheduler.HasTask);

        scheduler.Run();

        Assert.IsFalse(scheduler.HasTask);

        Assert.IsTrue(task.IsCanceled);
        Assert.AreEqual(new TaskCanceledException(task).CancellationToken, cts.Token);
    }

    [TestMethod]
    public void ToTask_Scheduler_Resumed_On_Thread_Success_With_State_And_Cancellation()
    {
        var scheduler = new OneshotScheduler();

        var task = Observable.Return(1).ToTask(default, "state", scheduler);

        Assert.IsTrue(scheduler.HasTask);

        scheduler.Run();

        Assert.IsFalse(scheduler.HasTask);

        Assert.IsTrue(task.IsCompleted);

        Assert.AreEqual("state", task.AsyncState);
    }

    [TestMethod]
    public void ToTask_Scheduler_Resumed_On_Thread_Failure_With_State_And_Cancellation()
    {
        var scheduler = new OneshotScheduler();

        var task = Observable.Throw<int>(new InvalidOperationException("failure")).ToTask(default, "state", scheduler);

        Assert.IsTrue(scheduler.HasTask);

        scheduler.Run();

        Assert.IsFalse(scheduler.HasTask);

        Assert.IsTrue(task.IsFaulted);
        Assert.IsTrue(task.Exception!.InnerException is InvalidOperationException);
        Assert.AreEqual("failure", task.Exception!.InnerException!.Message);
        Assert.AreEqual("state", task.AsyncState);
    }

    [TestMethod]
    public void ToTask_Scheduler_Resumed_On_Thread_Cancel_With_State_And_Cancellation()
    {
        var scheduler = new OneshotScheduler();

        var cts = new CancellationTokenSource();
        var task = Observable.Never<int>().ToTask(cts.Token, "state", scheduler);

        Assert.IsFalse(scheduler.HasTask);

        cts.Cancel();

        Assert.IsTrue(scheduler.HasTask);

        scheduler.Run();

        Assert.IsFalse(scheduler.HasTask);

        Assert.IsTrue(task.IsCanceled);
        Assert.AreEqual("state", task.AsyncState);
        Assert.AreEqual(new TaskCanceledException(task).CancellationToken, cts.Token);
    }

    [TestMethod]
    public async Task ToTask_Scheduler_Dispose_Can_Propagate()
    {
        async Task asyncMethod()
        {
            await Task.Delay(500, TestContext.CancellationToken);
            Console.WriteLine("Done");
        }

        var count = 0;

        var observable = Observable.Create<long>(observer =>
        {
            var d = Observable.Interval(TimeSpan.FromMilliseconds(200)).Subscribe(observer);
            return new CompositeDisposable(d, Disposable.Create(() =>
            {
                Interlocked.Increment(ref count);
            }));
        })
        .Select(_ => Observable.FromAsync(asyncMethod))
        .Concat()
        .Take(1);

        await observable.ToTask(global::System.Reactive.Concurrency.Scheduler.Default).ConfigureAwait(false);

        Thread.Sleep(500);

        Assert.AreEqual(1, Volatile.Read(ref count));
    }

    private sealed class OneshotScheduler : IScheduler
    {
        public DateTimeOffset Now => DateTimeOffset.Now;

        private volatile Action? _task;

        public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            var task = new Work<TState> { State = state, Action = action };
            _task = () =>
            {
                task.Action?.Invoke(this, task.State);
            };
            return task;
        }

        public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            throw new NotImplementedException("Not supported by this scheduler");
        }

        public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            throw new NotImplementedException("Not supported by this scheduler");
        }

        public void Run()
        {
            Interlocked.Exchange(ref _task, null)?.Invoke();
        }

        public bool HasTask => _task != null;

        private sealed class Work<TState> : IDisposable
        {
            internal required TState State;
            internal required Func<IScheduler, TState, IDisposable>? Action;

            public void Dispose()
            {
                Action = null;
            }
        }
    }

    public TestContext TestContext { get; set; } = null!;
}
