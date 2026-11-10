// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>StartAsync</c> scenarios on Rx.NET, plus its <c>TaskObservationOptions</c> tests.
/// </summary>
/// <remarks>
/// The sixteen <c>*UnsubscribeThenError*</c> tests are the originals: a task that faults after
/// its subscription was disposed has its error reported as unobserved by default, and ignored
/// when <c>TaskObservationOptions</c> says so. The option was added to Rx.NET after the
/// AsyncRx.NET prototype, and an equivalent is a deferred work item; until then the tests run
/// here, with Rx.NET's <c>TaskErrorObservation</c> helper, which waits on the runtime's
/// unobserved-task-exception event across garbage collections.
/// </remarks>
[TestClass]
public sealed class RxStartAsyncTests : StartAsyncTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void Start_Func_UnsubscribeThenError_ErrorReportedAsUnobserved()
    {
        Start_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            createTask => Observable.StartAsync(createTask),
            errorObservation =>
            {
                errorObservation.AssertExceptionReportedAsUnobserved();
            });
    }

    [TestMethod]
    public void Start_FuncWithCancel_UnsubscribeThenError_ErrorReportedAsUnobserved()
    {
        Start_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            createTask => Observable.StartAsync(_ => createTask()),
            errorObservation =>
            {
                errorObservation.AssertExceptionReportedAsUnobserved();
            });
    }

    [TestMethod]
    public void Start_Func_WithScheduler_UnsubscribeThenError_ErrorReportedAsUnobserved()
    {
        Start_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            createTask => Observable.StartAsync(createTask, TaskPoolScheduler.Default),
            errorObservation =>
            {
                errorObservation.AssertExceptionReportedAsUnobserved();
            });
    }

    [TestMethod]
    public void Start_FuncWithCancel_WithScheduler_UnsubscribeThenError_ErrorReportedAsUnobserved()
    {
        Start_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            createTask => Observable.StartAsync(_ => createTask(), TaskPoolScheduler.Default),
            errorObservation =>
            {
                errorObservation.AssertExceptionReportedAsUnobserved();
            });
    }

    [TestMethod]
    public void Start_Func_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
    {
        Start_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            createTask => Observable.StartAsync(
                createTask,
                new TaskObservationOptions(null, ignoreExceptionsAfterUnsubscribe: true)),
            errorObservation =>
            {
                errorObservation.AssertExceptionNotReportedAsUnobserved();
            });
    }

    [TestMethod]
    public void Start_FuncWithCancel_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
    {
        Start_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            createTask => Observable.StartAsync(
                _ => createTask(),
                new TaskObservationOptions(null, ignoreExceptionsAfterUnsubscribe: true)),
            errorObservation =>
            {
                errorObservation.AssertExceptionNotReportedAsUnobserved();
            });
    }

    [TestMethod]
    public void Start_Func_WithScheduler_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
    {
        Start_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            createTask => Observable.StartAsync(
                createTask,
                new TaskObservationOptions(
                    TaskPoolScheduler.Default,
                    ignoreExceptionsAfterUnsubscribe: true)),
            errorObservation =>
            {
                errorObservation.AssertExceptionNotReportedAsUnobserved();
            });
    }

    [TestMethod]
    public void Start_FuncWithCancel_WithScheduler_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
    {
        Start_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            createTask => Observable.StartAsync(
                _ => createTask(),
                new TaskObservationOptions(
                    TaskPoolScheduler.Default,
                    ignoreExceptionsAfterUnsubscribe: true)),
            errorObservation =>
            {
                errorObservation.AssertExceptionNotReportedAsUnobserved();
            });
    }

    [TestMethod]
    public void Start_Action_UnsubscribeThenError_ErrorReportedAsUnobserved()
    {
        Start_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            createTask => Observable.StartAsync(createTask),
            errorObservation =>
            {
                errorObservation.AssertExceptionReportedAsUnobserved();
            });
    }

    [TestMethod]
    public void Start_ActionWithCancel_UnsubscribeThenError_ErrorReportedAsUnobserved()
    {
        Start_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            createTask => Observable.StartAsync(_ => createTask()),
            errorObservation =>
            {
                errorObservation.AssertExceptionReportedAsUnobserved();
            });
    }

    [TestMethod]
    public void Start_Action_WithScheduler_UnsubscribeThenError_ErrorReportedAsUnobserved()
    {
        Start_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            createTask => Observable.StartAsync(createTask, TaskPoolScheduler.Default),
            errorObservation =>
            {
                errorObservation.AssertExceptionReportedAsUnobserved();
            });
    }

    [TestMethod]
    public void Start_ActionWithCancel_WithScheduler_UnsubscribeThenError_ErrorReportedAsUnobserved()
    {
        Start_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            createTask => Observable.StartAsync(_ => createTask(), TaskPoolScheduler.Default),
            errorObservation =>
            {
                errorObservation.AssertExceptionReportedAsUnobserved();
            });
    }

    [TestMethod]
    public void Start_Action_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
    {
        Start_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            createTask => Observable.StartAsync(
                createTask,
                new TaskObservationOptions(null, ignoreExceptionsAfterUnsubscribe: true)),
            errorObservation =>
            {
                errorObservation.AssertExceptionNotReportedAsUnobserved();
            });
    }

    [TestMethod]
    public void Start_ActionWithCancel_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
    {
        Start_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            createTask => Observable.StartAsync(
                _ => createTask(),
                new TaskObservationOptions(null, ignoreExceptionsAfterUnsubscribe: true)),
            errorObservation =>
            {
                errorObservation.AssertExceptionNotReportedAsUnobserved();
            });
    }

    [TestMethod]
    public void Start_Action_WithScheduler_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
    {
        Start_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            createTask => Observable.StartAsync(
                createTask,
                new TaskObservationOptions(
                    TaskPoolScheduler.Default,
                    ignoreExceptionsAfterUnsubscribe: true)),
            errorObservation =>
            {
                errorObservation.AssertExceptionNotReportedAsUnobserved();
            });
    }

    [TestMethod]
    public void Start_ActionWithCancel_WithScheduler_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
    {
        Start_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            createTask => Observable.StartAsync(
                _ => createTask(),
                new TaskObservationOptions(
                    TaskPoolScheduler.Default,
                    ignoreExceptionsAfterUnsubscribe: true)),
            errorObservation =>
            {
                errorObservation.AssertExceptionNotReportedAsUnobserved();
            });
    }

    private void Start_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
        Func<Func<Task<int>>, IObservable<int>> createObservable,
        Action<TaskErrorObservation> testResults)
    {
        Start_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core<int>(
            createObservable,
            testResults);
    }

    private void Start_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
        Func<Func<Task>, IObservable<Unit>> createObservable,
        Action<TaskErrorObservation> testResults)
    {
        Start_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core<Unit>(
            createObservable,
            testResults);
    }

    private void Start_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core<T>(
        Func<Func<Task<T>>, IObservable<T>> createObservable,
        Action<TaskErrorObservation> testResults)
    {
        using Barrier gate = new(2);
        using TaskErrorObservation errorObservation = new();

        var sub = errorObservation.SuscribeWithoutKeepingSourceReachable<T>(
            (setTask, exception) => createObservable(
                () => setTask(Task.Factory.StartNew<T>(
                    () =>
                    {
                        // 1: Notify that task execution has begun
                        gate.SignalAndWait();
                        // 2: Wait until unsubscribe Dispose has returned
                        gate.SignalAndWait();
                        throw exception;
                    })))
                .Subscribe());

        // 1: wait until task execution has begun
        gate.SignalAndWait();

        sub.Dispose();
        //sub = null;

        // 2: Notify that unsubscribe Dispose has returned
        gate.SignalAndWait();

        testResults(errorObservation);
    }
}
