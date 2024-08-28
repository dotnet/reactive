// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Tests.System.Reactive;

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class StartAsyncTest : ReactiveTest
    {

        private readonly Task<int> _doneTask;

        public StartAsyncTest()
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.SetResult(42);
            _doneTask = tcs.Task;
        }

        #region Func

        [TestMethod]
        public void StartAsync_Func_ArgumentChecking()
        {
            var s = Scheduler.Immediate;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(default(Func<Task<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(default(Func<CancellationToken, Task<int>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(default(Func<Task<int>>), s));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(default(Func<CancellationToken, Task<int>>), s));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(() => _doneTask, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(ct => _doneTask, default(IScheduler)));
        }

        [TestMethod]
        public void StartAsync_Func_Success()
        {
            var n = 42;

            var i = 0;

            var xs = Observable.StartAsync(() =>
            {
                i++;
                return Task.Factory.StartNew(() => n);
            });

            Assert.Equal(n, xs.Single());
            Assert.Equal(1, i);

            Assert.Equal(n, xs.Single());
            Assert.Equal(1, i);
        }

        [TestMethod]
        public void StartAsync_Func_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync<int>(() =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void StartAsync_Func_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync(() =>
                Task.Factory.StartNew<int>(() =>
                {
                    throw ex;
                })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void StartAsync_FuncWithCancel_Success()
        {
            var n = 42;

            var i = 0;
            var xs = Observable.StartAsync(ct =>
            {
                i++;
                return Task.Factory.StartNew(() => n);
            });

            Assert.Equal(n, xs.Single());
            Assert.Equal(1, i);

            Assert.Equal(n, xs.Single());
            Assert.Equal(1, i);
        }

        [TestMethod]
        public void StartAsync_FuncWithCancel_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync<int>(ct =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void StartAsync_FuncWithCancel_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync(ct =>
                Task.Factory.StartNew<int>(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void StartAsync_FuncWithCancel_Cancel()
        {
            var N = 10;

            for (var n = 0; n < N; n++)
            {
                var e = new ManualResetEvent(false);
                var f = new ManualResetEvent(false);

                var t = default(Task<int>);
                var xs = Observable.StartAsync(ct =>
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

                e.WaitOne();

                var d = xs.Subscribe(_ => { });
                d.Dispose();

                f.WaitOne();
                while (!t.IsCompleted)
                {
                    ;
                }

                ReactiveAssert.Throws<OperationCanceledException>(() => xs.Single());
            }
        }

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
                createTask => Observable.StartAsync(createTask, new TaskObservationOptions(null, ignoreExceptionsAfterUnsubscribe: true)),
                errorObservation =>
                {
                    errorObservation.AssertExceptionNotReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void Start_FuncWithCancel_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
        {
            Start_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.StartAsync(_ => createTask(), new TaskObservationOptions(null, ignoreExceptionsAfterUnsubscribe: true)),
                errorObservation =>
                {
                    errorObservation.AssertExceptionNotReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void Start_Func_WithScheduler_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
        {
            Start_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.StartAsync(createTask, new TaskObservationOptions(TaskPoolScheduler.Default, ignoreExceptionsAfterUnsubscribe: true)),
                errorObservation =>
                {
                    errorObservation.AssertExceptionNotReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void Start_FuncWithCancel_WithScheduler_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
        {
            Start_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.StartAsync(_ => createTask(), new TaskObservationOptions(TaskPoolScheduler.Default, ignoreExceptionsAfterUnsubscribe: true)),
                errorObservation =>
                {
                    errorObservation.AssertExceptionNotReportedAsUnobserved();
                });
        }

#if DESKTOPCLR
        [TestMethod]
        public void StartAsync_Func_Scheduler1()
        {
            var tcs = new TaskCompletionSource<int>();

            var e = new ManualResetEvent(false);
            var x = default(int);
            var t = default(int);

            var xs = Observable.StartAsync(() => tcs.Task, Scheduler.Immediate);
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
        public void StartAsync_Func_Scheduler2()
        {
            var tcs = new TaskCompletionSource<int>();

            var e = new ManualResetEvent(false);
            var x = default(int);
            var t = default(int);

            var xs = Observable.StartAsync(ct => tcs.Task, Scheduler.Immediate);
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
        public void StartAsync_Action_ArgumentChecking()
        {
            var s = Scheduler.Immediate;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(default(Func<Task>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(default(Func<CancellationToken, Task>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(default(Func<Task>), s));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(default(Func<CancellationToken, Task>), s));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(() => (Task)_doneTask, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartAsync(ct => (Task)_doneTask, default(IScheduler)));
        }

        [TestMethod]
        public void StartAsync_Action_Success()
        {
            var i = 0;
            var xs = Observable.StartAsync(() =>
            {
                i++;
                return Task.Factory.StartNew(() => { });
            });

            Assert.Equal(Unit.Default, xs.Single());
            Assert.Equal(1, i);

            Assert.Equal(Unit.Default, xs.Single());
            Assert.Equal(1, i);
        }

        [TestMethod]
        public void StartAsync_Action_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync(() =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void StartAsync_Action_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync(() =>
                Task.Factory.StartNew(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void StartAsync_ActionWithCancel_Success()
        {
            var i = 0;
            var xs = Observable.StartAsync(ct =>
            {
                i++;
                return Task.Factory.StartNew(() => { }, CancellationToken.None); // Not forwarding ct because we always want this task to run to completion in this test.
            });

            Assert.Equal(Unit.Default, xs.Single());
            Assert.Equal(1, i);

            Assert.Equal(Unit.Default, xs.Single());
            Assert.Equal(1, i);
        }

        [TestMethod]
        public void StartAsync_ActionWithCancel_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync(ct =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void StartAsync_ActionWithCancel_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.StartAsync(ct =>
                Task.Factory.StartNew(() => { throw ex; }, CancellationToken.None) // Not forwarding ct because we always want this task to run and then fail in this test
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void StartAsync_ActionWithCancel_Cancel()
        {
            var N = 10;

            for (var n = 0; n < N; n++)
            {
                var e = new ManualResetEvent(false);
                var f = new ManualResetEvent(false);

                var t = default(Task);
                var xs = Observable.StartAsync(ct =>
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

                e.WaitOne();

                var d = xs.Subscribe(_ => { });
                d.Dispose();

                f.WaitOne();
                while (!t.IsCompleted)
                {
                    ;
                }

                ReactiveAssert.Throws<OperationCanceledException>(() => xs.Single());
            }
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
                createTask => Observable.StartAsync(createTask, new TaskObservationOptions(null, ignoreExceptionsAfterUnsubscribe: true)),
                errorObservation =>
                {
                    errorObservation.AssertExceptionNotReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void Start_ActionWithCancel_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
        {
            Start_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.StartAsync(_ => createTask(), new TaskObservationOptions(null, ignoreExceptionsAfterUnsubscribe: true)),
                errorObservation =>
                {
                    errorObservation.AssertExceptionNotReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void Start_Action_WithScheduler_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
        {
            Start_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.StartAsync(createTask, new TaskObservationOptions(TaskPoolScheduler.Default, ignoreExceptionsAfterUnsubscribe: true)),
                errorObservation =>
                {
                    errorObservation.AssertExceptionNotReportedAsUnobserved();
                });
        }

        [TestMethod]
        public void Start_ActionWithCancel_WithScheduler_IgnorePostUnsubscribeErrors_UnsubscribeThenError_ErrorNotReportedAsUnobserved()
        {
            Start_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
                createTask => Observable.StartAsync(_ => createTask(), new TaskObservationOptions(TaskPoolScheduler.Default, ignoreExceptionsAfterUnsubscribe: true)),
                errorObservation =>
                {
                    errorObservation.AssertExceptionNotReportedAsUnobserved();
                });
        }

#if DESKTOPCLR
        [TestMethod]
        public void StartAsync_Action_Scheduler1()
        {
            var tcs = new TaskCompletionSource<int>();

            var e = new ManualResetEvent(false);
            var t = default(int);

            var xs = Observable.StartAsync(() => (Task)tcs.Task, Scheduler.Immediate);
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
        public void StartAsync_Action_Scheduler2()
        {
            var tcs = new TaskCompletionSource<int>();

            var e = new ManualResetEvent(false);
            var t = default(int);

            var xs = Observable.StartAsync(ct => (Task)tcs.Task, Scheduler.Immediate);
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

        private void Start_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            Func<Func<Task<int>>, IObservable<int>> createObservable,
            Action<TaskErrorObservation> testResults)
        {
            Start_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core<int>(createObservable, testResults);
        }

        private void Start_Action_ErrorAfterUnsubscribeReportedAsUnobserved_Core(
            Func<Func<Task>, IObservable<Unit>> createObservable,
            Action<TaskErrorObservation> testResults)
        {
            Start_Func_ErrorAfterUnsubscribeReportedAsUnobserved_Core<Unit>(createObservable, testResults);
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
}
