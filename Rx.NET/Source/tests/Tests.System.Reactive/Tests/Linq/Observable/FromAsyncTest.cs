// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tests.System.Reactive;

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class FromAsyncTest : ReactiveTest
    {
        private readonly Task<int> _doneTask;

        public FromAsyncTest()
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.SetResult(42);
            _doneTask = tcs.Task;
        }

        #region Func

        [TestMethod]
        public void FromAsync_Func_ArgumentChecking()
        {
            var s = Scheduler.Immediate;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(default(Func<Task<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(default(Func<CancellationToken, Task<int>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(default(Func<Task<int>>), s));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(() => _doneTask, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(default(Func<CancellationToken, Task<int>>), s));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(ct => _doneTask, default(IScheduler)));
        }

        [TestMethod]
        public void FromAsync_Func_Success()
        {
            var n = 42;

            var i = 0;
            var xs = Observable.FromAsync(() =>
            {
                i++;
                return Task.Factory.StartNew(() => n);
            });

            Assert.Equal(n, xs.Single());
            Assert.Equal(1, i);

            Assert.Equal(n, xs.Single());
            Assert.Equal(2, i);
        }

        [TestMethod]
        public void FromAsync_Func_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync<int>(() =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void FromAsync_Func_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync(() =>
                Task.Factory.StartNew<int>(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void FromAsync_FuncWithCancel_Success()
        {
            var n = 42;

            var i = 0;
            var xs = Observable.FromAsync(ct =>
            {
                i++;
                return Task.Factory.StartNew(() => n);
            });

            Assert.Equal(n, xs.Single());
            Assert.Equal(1, i);

            Assert.Equal(n, xs.Single());
            Assert.Equal(2, i);
        }

        [TestMethod]
        public void FromAsync_FuncWithCancel_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync<int>(ct =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void FromAsync_FuncWithCancel_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync(ct =>
                Task.Factory.StartNew<int>(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void FromAsync_FuncWithCancel_Cancel()
        {
            var e = new ManualResetEvent(false);
            var f = new ManualResetEvent(false);

            var t = default(Task<int>);
            var xs = Observable.FromAsync(ct =>
                t = Task.Factory.StartNew<int>(() =>
                {
                    try
                    {
                        e.Set();
                        while (true)
                        {
                            ct.ThrowIfCancellationRequested();
                        }
                    }
                    finally
                    {
                        f.Set();
                    }
                })
            );

            var d = xs.Subscribe(_ => { });
            e.WaitOne();
            d.Dispose();

            f.WaitOne();
            while (!t.IsCompleted)
            {
                ;
            }
        }

        [TestMethod]
        public void FromAsync_Func_UnsubscribeThenError_ErrorReportedAsUnobserved()
        {
            FromAsync_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.FromAsync(createTask),
                errorObservation =>
                {
                    errorObservation.AssertExceptionReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void FromAsync_FuncWithCancel_UnsubscribeThenError_ErrorReportedAsUnobserved()
        {
            FromAsync_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.FromAsync(_ => createTask()),
                errorObservation =>
                {
                    errorObservation.AssertExceptionReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void FromAsync_Func_WithScheduler_UnsubscribeThenError_ErrorReportedAsUnobserved()
        {
            FromAsync_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.FromAsync(createTask, TaskPoolScheduler.Default),
                errorObservation =>
                {
                    errorObservation.AssertExceptionReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void FromAsync_FuncWithCancel_WithScheduler_UnsubscribeThenError_ErrorReportedAsUnobserved()
        {
            FromAsync_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.FromAsync(_ => createTask(), TaskPoolScheduler.Default),
                errorObservation =>
                {
                    errorObservation.AssertExceptionReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void FromAsync_Func_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
        {
            FromAsync_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.FromAsync(createTask, new TaskObservationOptions(null, ignoreExceptionsAfterUnsubscribe: true)),
                errorObservation =>
                {
                    errorObservation.AssertExceptionNotReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void FromAsync_FuncWithCancel_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
        {
            FromAsync_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.FromAsync(_ => createTask(), new TaskObservationOptions(null, ignoreExceptionsAfterUnsubscribe: true)),
                errorObservation =>
                {
                    errorObservation.AssertExceptionNotReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void FromAsync_Func_WithScheduler_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
        {
            FromAsync_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.FromAsync(createTask, new TaskObservationOptions(TaskPoolScheduler.Default, ignoreExceptionsAfterUnsubscribe: true)),
                errorObservation =>
                {
                    errorObservation.AssertExceptionNotReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void FromAsync_FuncWithCancel_WithScheduler_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
        {
            FromAsync_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.FromAsync(_ => createTask(), new TaskObservationOptions(TaskPoolScheduler.Default, ignoreExceptionsAfterUnsubscribe: true)),
                errorObservation =>
                {
                    errorObservation.AssertExceptionNotReportedAsUnobserved();
                });
        }

#if DESKTOPCLR
        [TestMethod]
        public void FromAsync_Func_Scheduler1()
        {
            var e = new ManualResetEvent(false);
            var x = default(int);
            var t = default(int);

            var tcs = new TaskCompletionSource<int>();

            var xs = Observable.FromAsync(() => tcs.Task, Scheduler.Immediate);
            xs.Subscribe(res =>
            {
                x = res;
                t = Environment.CurrentManagedThreadId;
                e.Set();
            });

            tcs.SetResult(42);

            e.WaitOne();

            Assert.Equal(42, x);
            Assert.Equal(Environment.CurrentManagedThreadId, t);
        }

        [TestMethod]
        public void FromAsync_Func_Scheduler2()
        {
            var e = new ManualResetEvent(false);
            var x = default(int);
            var t = default(int);

            var tcs = new TaskCompletionSource<int>();

            var xs = Observable.FromAsync(ct => tcs.Task, Scheduler.Immediate);
            xs.Subscribe(res =>
            {
                x = res;
                t = Environment.CurrentManagedThreadId;
                e.Set();
            });

            tcs.SetResult(42);

            e.WaitOne();

            Assert.Equal(42, x);
            Assert.Equal(Environment.CurrentManagedThreadId, t);
        }
#endif

        #endregion

        #region Action

        [TestMethod]
        public void FromAsync_Action_ArgumentChecking()
        {
            var s = Scheduler.Immediate;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(default(Func<Task>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(default(Func<CancellationToken, Task>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(default(Func<Task>), s));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(default(Func<CancellationToken, Task>), s));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(ct => (Task)_doneTask, default(IScheduler)));
        }

        [TestMethod]
        public void FromAsync_Action_Success()
        {
            var i = 0;
            var xs = Observable.FromAsync(() =>
            {
                i++;
                return Task.Factory.StartNew(() => { });
            });

            Assert.Equal(Unit.Default, xs.Single());
            Assert.Equal(1, i);

            Assert.Equal(Unit.Default, xs.Single());
            Assert.Equal(2, i);
        }

        [TestMethod]
        public void FromAsync_Action_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync(() =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void FromAsync_Action_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync(() =>
                Task.Factory.StartNew(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void FromAsync_ActionWithCancel_Success()
        {
            var i = 0;
            var xs = Observable.FromAsync(ct =>
            {
                i++;
                return Task.Factory.StartNew(() => { }, CancellationToken.None); // Not forwarding ct because we want this task always to run and complete.
            });

            Assert.Equal(Unit.Default, xs.Single());
            Assert.Equal(1, i);

            Assert.Equal(Unit.Default, xs.Single());
            Assert.Equal(2, i);
        }

        [TestMethod]
        public void FromAsync_ActionWithCancel_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync(ct =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void FromAsync_ActionWithCancel_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync(ct =>
                Task.Factory.StartNew(() => { throw ex; }, CancellationToken.None) // Not forwarding ct because we always want this task to run and fail
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void FromAsync_ActionWithCancel_Cancel()
        {
            var e = new ManualResetEvent(false);
            var f = new ManualResetEvent(false);

            var t = default(Task);
            var xs = Observable.FromAsync(ct =>
                t = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        e.Set();
                        while (true)
                        {
                            ct.ThrowIfCancellationRequested();
                        }
                    }
                    finally
                    {
                        f.Set();
                    }
                },
                CancellationToken.None) // Not forwarding ct because we are testing the case where the task is already running by the time cancellation is detected
            );

            var d = xs.Subscribe(_ => { });
            e.WaitOne();
            d.Dispose();

            f.WaitOne();
            while (!t.IsCompleted)
            {
                ;
            }
        }


        [TestMethod]
        public void FromAsync_Action_UnsubscribeThenError_ErrorReportedAsUnobserved()
        {
            FromAsync_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.FromAsync(createTask),
                errorObservation =>
                {
                    errorObservation.AssertExceptionReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void FromAsync_ActionWithCancel_UnsubscribeThenError_ErrorReportedAsUnobserved()
        {
            FromAsync_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.FromAsync(_ => createTask()),
                errorObservation =>
                {
                    errorObservation.AssertExceptionReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void FromAsync_Action_WithScheduler_UnsubscribeThenError_ErrorReportedAsUnobserved()
        {
            FromAsync_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.FromAsync(createTask, TaskPoolScheduler.Default),
                errorObservation =>
                {
                    errorObservation.AssertExceptionReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void FromAsync_ActionWithCancel_WithScheduler_UnsubscribeThenError_ErrorReportedAsUnobserved()
        {
            FromAsync_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.FromAsync(_ => createTask(), TaskPoolScheduler.Default),
                errorObservation =>
                {
                    errorObservation.AssertExceptionReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void FromAsync_Action_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
        {
            FromAsync_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.FromAsync(createTask, new TaskObservationOptions(null, ignoreExceptionsAfterUnsubscribe: true)),
                errorObservation =>
                {
                    errorObservation.AssertExceptionNotReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void FromAsync_ActionWithCancel_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
        {
            FromAsync_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.FromAsync(_ => createTask(), new TaskObservationOptions(scheduler: null, ignoreExceptionsAfterUnsubscribe: true)),
                errorObservation =>
                {
                    errorObservation.AssertExceptionNotReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void FromAsync_Action_WithScheduler_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
        {
            FromAsync_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.FromAsync(createTask, new TaskObservationOptions(TaskPoolScheduler.Default, ignoreExceptionsAfterUnsubscribe: true)),
                errorObservation =>
                {
                    errorObservation.AssertExceptionNotReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void FromAsync_ActionWithCancel_WithScheduler_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
        {
            FromAsync_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.FromAsync(_ => createTask(), new TaskObservationOptions(TaskPoolScheduler.Default, ignoreExceptionsAfterUnsubscribe: true)),
                errorObservation =>
                {
                    errorObservation.AssertExceptionNotReportedAsUnobserved();
                });
        }
#if DESKTOPCLR
        [TestMethod]
        public void FromAsync_Action_Scheduler1()
        {
            var e = new ManualResetEvent(false);
            var t = default(int);

            var tcs = new TaskCompletionSource<int>();

            var xs = Observable.FromAsync(() => (Task)tcs.Task, Scheduler.Immediate);
            xs.Subscribe(res =>
            {
                t = Environment.CurrentManagedThreadId;
                e.Set();
            });

            tcs.SetResult(42);

            e.WaitOne();

            Assert.Equal(Environment.CurrentManagedThreadId, t);
        }

        [TestMethod]
        public void FromAsync_Action_Scheduler2()
        {
            var e = new ManualResetEvent(false);
            var t = default(int);

            var tcs = new TaskCompletionSource<int>();

            var xs = Observable.FromAsync(ct => (Task)tcs.Task, Scheduler.Immediate);
            xs.Subscribe(res =>
            {
                t = Environment.CurrentManagedThreadId;
                e.Set();
            });

            tcs.SetResult(42);

            e.WaitOne();

            Assert.Equal(Environment.CurrentManagedThreadId, t);
        }
#endif

        #endregion

        private void FromAsync_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            Func<Func<Task<int>>, IObservable<int>> createObservable,
            Action<TaskErrorObservation> testResults)
        {
            FromAsync_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core<int>(createObservable, testResults);
        }

        private void FromAsync_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            Func<Func<Task>, IObservable<Unit>> createObservable,
            Action<TaskErrorObservation> testResults)
        {
            FromAsync_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core<Unit>(createObservable, testResults);
        }

        private void FromAsync_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core<T>(
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

            // 2: Notify that unsubscribe Dispose has returned
            gate.SignalAndWait();

            testResults(errorObservation);
        }
    }
}
