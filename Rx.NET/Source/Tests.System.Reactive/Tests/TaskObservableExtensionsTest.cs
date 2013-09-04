// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_TPL
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class TaskObservableExtensionsTest : ReactiveTest
    {
        #region ToObservable

        [TestMethod]
        public void TaskToObservable_NonVoid_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskObservableExtensions.ToObservable((System.Threading.Tasks.Task<int>)null));
            var tcs = new System.Threading.Tasks.TaskCompletionSource<int>();
            var task = tcs.Task;
            ReactiveAssert.Throws<ArgumentNullException>(() => task.ToObservable().Subscribe(null));
        }

        [TestMethod]
        public void TaskToObservable_NonVoid_Complete_BeforeCreate()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<int>);

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(10, () => taskSource.SetResult(42));

                res = scheduler.Start(() =>
                    taskSource.Task.ToObservable()
                );
            });

            res.Messages.AssertEqual(
                OnNext(200, 42),
                OnCompleted<int>(200)
            );
        }

        [TestMethod]
        public void TaskToObservable_NonVoid_Complete_BeforeSubscribe()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<int>);

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(110, () => taskSource.SetResult(42));

                res = scheduler.Start(() =>
                    taskSource.Task.ToObservable()
                );
            });

            res.Messages.AssertEqual(
                OnNext(200, 42),
                OnCompleted<int>(200)
            );
        }

        [TestMethod]
        public void TaskToObservable_NonVoid_Complete_BeforeDispose()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<int>);

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(300, () => taskSource.SetResult(42));

                res = scheduler.Start(() =>
                    taskSource.Task.ToObservable()
                );
            });

            res.Messages.AssertEqual(
                OnNext(300, 42),
                OnCompleted<int>(300)
            );
        }

        [TestMethod]
        public void TaskToObservable_NonVoid_Complete_AfterDispose()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<int>);

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(1100, () => taskSource.SetResult(42));

                res = scheduler.Start(() =>
                    taskSource.Task.ToObservable()
                );
            });

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void TaskToObservable_NonVoid_Exception_BeforeCreate()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<int>);

            var ex = new Exception();

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(10, () => taskSource.SetException(ex));

                res = scheduler.Start(() =>
                    taskSource.Task.ToObservable()
                );
            });

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        [TestMethod]
        public void TaskToObservable_NonVoid_Exception_BeforeSubscribe()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<int>);

            var ex = new Exception();

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(110, () => taskSource.SetException(ex));

                res = scheduler.Start(() =>
                    taskSource.Task.ToObservable()
                );
            });

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        [TestMethod]
        public void TaskToObservable_NonVoid_Exception_BeforeDispose()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<int>);

            var ex = new Exception();

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(300, () => taskSource.SetException(ex));

                res = scheduler.Start(() =>
                    taskSource.Task.ToObservable()
                );
            });

            res.Messages.AssertEqual(
                OnError<int>(300, ex)
            );
        }

        [TestMethod]
        public void TaskToObservable_NonVoid_Exception_AfterDispose()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<int>);

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(1100, () => taskSource.SetException(new Exception()));

                res = scheduler.Start(() =>
                    taskSource.Task.ToObservable()
                );
            });

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void TaskToObservable_NonVoid_Canceled_BeforeCreate()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<int>);

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(10, () => taskSource.SetCanceled());

                res = scheduler.Start(() =>
                    taskSource.Task.ToObservable()
                );
            });

            res.Messages.AssertEqual(
                OnError<int>(200, ex => ex is TaskCanceledException)
            );
        }

        [TestMethod]
        public void TaskToObservable_NonVoid_Canceled_BeforeSubscribe()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<int>);

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(110, () => taskSource.SetCanceled());

                res = scheduler.Start(() =>
                    taskSource.Task.ToObservable()
                );
            });

            res.Messages.AssertEqual(
                OnError<int>(200, ex => ex is TaskCanceledException)
            );
        }

        [TestMethod]
        public void TaskToObservable_NonVoid_Canceled_BeforeDispose()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<int>);

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(300, () => taskSource.SetCanceled());

                res = scheduler.Start(() =>
                    taskSource.Task.ToObservable()
                );
            });

            res.Messages.AssertEqual(
                OnError<int>(300, ex => ex is TaskCanceledException)
            );
        }

        [TestMethod]
        public void TaskToObservable_NonVoid_Canceled_AfterDispose()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<int>);

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(1100, () => taskSource.SetCanceled());

                res = scheduler.Start(() =>
                    taskSource.Task.ToObservable()
                );
            });

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void TaskToObservable_Void_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskObservableExtensions.ToObservable((System.Threading.Tasks.Task)null));
            var tcs = new System.Threading.Tasks.TaskCompletionSource<int>();
            System.Threading.Tasks.Task task = tcs.Task;
            ReactiveAssert.Throws<ArgumentNullException>(() => task.ToObservable().Subscribe(null));
        }

        [TestMethod]
        public void TaskToObservable_Void_Complete_BeforeCreate()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<Unit>);

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(10, () => taskSource.SetResult(42));

                res = scheduler.Start(() =>
                    ((Task)taskSource.Task).ToObservable()
                );
            });

            res.Messages.AssertEqual(
                OnNext(200, new Unit()),
                OnCompleted<Unit>(200)
            );
        }

        [TestMethod]
        public void TaskToObservable_Void_Complete_BeforeSubscribe()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<Unit>);

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(110, () => taskSource.SetResult(42));

                res = scheduler.Start(() =>
                    ((Task)taskSource.Task).ToObservable()
                );
            });

            res.Messages.AssertEqual(
                OnNext(200, new Unit()),
                OnCompleted<Unit>(200)
            );
        }

        [TestMethod]
        public void TaskToObservable_Void_Complete_BeforeDispose()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<Unit>);

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(300, () => taskSource.SetResult(42));

                res = scheduler.Start(() =>
                    ((Task)taskSource.Task).ToObservable()
                );
            });

            res.Messages.AssertEqual(
                OnNext(300, new Unit()),
                OnCompleted<Unit>(300)
            );
        }

        [TestMethod]
        public void TaskToObservable_Void_Complete_AfterDispose()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<Unit>);

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(1100, () => taskSource.SetResult(42));

                res = scheduler.Start(() =>
                    ((Task)taskSource.Task).ToObservable()
                );
            });

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void TaskToObservable_Void_Exception_BeforeCreate()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<Unit>);

            var ex = new Exception();

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(10, () => taskSource.SetException(ex));

                res = scheduler.Start(() =>
                    ((Task)taskSource.Task).ToObservable()
                );
            });

            res.Messages.AssertEqual(
                OnError<Unit>(200, ex)
            );
        }

        [TestMethod]
        public void TaskToObservable_Void_Exception_BeforeSubscribe()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<Unit>);

            var ex = new Exception();

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(110, () => taskSource.SetException(ex));

                res = scheduler.Start(() =>
                    ((Task)taskSource.Task).ToObservable()
                );
            });

            res.Messages.AssertEqual(
                OnError<Unit>(200, ex)
            );
        }

        [TestMethod]
        public void TaskToObservable_Void_Exception_BeforeDispose()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<Unit>);

            var ex = new Exception();

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(300, () => taskSource.SetException(ex));

                res = scheduler.Start(() =>
                    ((Task)taskSource.Task).ToObservable()
                );
            });

            res.Messages.AssertEqual(
                OnError<Unit>(300, ex)
            );
        }

        [TestMethod]
        public void TaskToObservable_Void_Exception_AfterDispose()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<Unit>);

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(1100, () => taskSource.SetException(new Exception()));

                res = scheduler.Start(() =>
                    ((Task)taskSource.Task).ToObservable()
                );
            });

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void TaskToObservable_Void_Canceled_BeforeCreate()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<Unit>);

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(10, () => taskSource.SetCanceled());

                res = scheduler.Start(() =>
                    ((Task)taskSource.Task).ToObservable()
                );
            });

            res.Messages.AssertEqual(
                OnError<Unit>(200, ex => ex is TaskCanceledException)
            );
        }

        [TestMethod]
        public void TaskToObservable_Void_Canceled_BeforeSubscribe()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<Unit>);

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(110, () => taskSource.SetCanceled());

                res = scheduler.Start(() =>
                    ((Task)taskSource.Task).ToObservable()
                );
            });

            res.Messages.AssertEqual(
                OnError<Unit>(200, ex => ex is TaskCanceledException)
            );
        }

        [TestMethod]
        public void TaskToObservable_Void_Canceled_BeforeDispose()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<Unit>);

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(300, () => taskSource.SetCanceled());

                res = scheduler.Start(() =>
                    ((Task)taskSource.Task).ToObservable()
                );
            });

            res.Messages.AssertEqual(
                OnError<Unit>(300, ex => ex is TaskCanceledException)
            );
        }

        [TestMethod]
        public void TaskToObservable_Void_Canceled_AfterDispose()
        {
            var taskScheduler = new TestTaskScheduler();
            var taskFactory = new TaskFactory(taskScheduler);
            var res = default(ITestableObserver<Unit>);

            taskFactory.StartNew(() =>
            {
                var scheduler = new TestScheduler();

                var taskSource = new TaskCompletionSource<int>();
                taskSource.Task.ContinueWith(t => { var e = t.Exception; });

                scheduler.ScheduleAbsolute(1100, () => taskSource.SetCanceled());

                res = scheduler.Start(() =>
                    ((Task)taskSource.Task).ToObservable()
                );
            });

            res.Messages.AssertEqual(
            );
        }

        #endregion

        #region ToTask

        [TestMethod]
        public void ObservableToTask_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskObservableExtensions.ToTask<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskObservableExtensions.ToTask<int>(null, new CancellationToken()));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskObservableExtensions.ToTask<int>(null, new object()));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskObservableExtensions.ToTask<int>(null, new CancellationToken(), new object()));
        }

        [TestMethod]
        public void ObservableToTaskNoValue()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Empty<int>(scheduler);

            var continuation = xs.ToTask();
            scheduler.Start();

            Assert.IsTrue(continuation.IsFaulted);
            Assert.IsTrue(continuation.Exception.InnerExceptions.Count == 1 && continuation.Exception.InnerExceptions[0] is InvalidOperationException);

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

        #endregion
    }
}
#endif