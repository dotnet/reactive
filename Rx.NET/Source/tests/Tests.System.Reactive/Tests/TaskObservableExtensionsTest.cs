// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{

    public class TaskObservableExtensionsTest : ReactiveTest
    {
        private readonly Task<int> _doneTask;

        public TaskObservableExtensionsTest()
        {
            var tcs = new TaskCompletionSource<int>();
            tcs.SetResult(42);
            _doneTask = tcs.Task;
        }

        #region ToObservable

        [Fact]
        public void TaskToObservable_NonVoid_ArgumentChecking()
        {
            var s = Scheduler.Immediate;

            ReactiveAssert.Throws<ArgumentNullException>(() => TaskObservableExtensions.ToObservable((Task<int>)null));

            ReactiveAssert.Throws<ArgumentNullException>(() => TaskObservableExtensions.ToObservable((Task<int>)null, s));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskObservableExtensions.ToObservable(_doneTask, default));

            var tcs = new TaskCompletionSource<int>();
            var task = tcs.Task;
            ReactiveAssert.Throws<ArgumentNullException>(() => task.ToObservable().Subscribe(null));
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

#if DESKTOPCLR
        [Fact]
        public void TaskToObservable_NonVoid_Scheduler()
        {
            var e = new ManualResetEvent(false);
            var x = default(int);
            var t = default(int);

            var cts = new TaskCompletionSource<int>();

            var xs = cts.Task.ToObservable(Scheduler.Immediate);
            xs.Subscribe(res =>
            {
                x = res;
                t = Thread.CurrentThread.ManagedThreadId;
                e.Set();
            });

            cts.SetResult(42);

            e.WaitOne();

            Assert.Equal(42, x);
            Assert.Equal(Thread.CurrentThread.ManagedThreadId, t);
        }
#endif

        [Fact]
        public void TaskToObservable_Void_ArgumentChecking()
        {
            var s = Scheduler.Immediate;

            ReactiveAssert.Throws<ArgumentNullException>(() => TaskObservableExtensions.ToObservable(null));

            ReactiveAssert.Throws<ArgumentNullException>(() => TaskObservableExtensions.ToObservable(null, s));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskObservableExtensions.ToObservable((Task)_doneTask, default));

            var tcs = new TaskCompletionSource<int>();
            Task task = tcs.Task;
            ReactiveAssert.Throws<ArgumentNullException>(() => task.ToObservable().Subscribe(null));
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

#if DESKTOPCLR
        [Fact]
        public void TaskToObservable_Void_Scheduler()
        {
            var e = new ManualResetEvent(false);
            var t = default(int);

            var tcs = new TaskCompletionSource<int>();

            var xs = ((Task)tcs.Task).ToObservable(Scheduler.Immediate);
            xs.Subscribe(res =>
            {
                t = Thread.CurrentThread.ManagedThreadId;
                e.Set();
            });

            tcs.SetResult(42);

            e.WaitOne();

            Assert.Equal(Thread.CurrentThread.ManagedThreadId, t);
        }
#endif

        #endregion

        #region ToTask

        [Fact]
        public void ObservableToTask_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskObservableExtensions.ToTask<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskObservableExtensions.ToTask<int>(null, new CancellationToken()));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskObservableExtensions.ToTask<int>(null, new object()));
            ReactiveAssert.Throws<ArgumentNullException>(() => TaskObservableExtensions.ToTask<int>(null, new CancellationToken(), new object()));
        }

        [Fact]
        public void ObservableToTaskNoValue()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Empty<int>(scheduler);

            var continuation = xs.ToTask();
            scheduler.Start();

            Assert.True(continuation.IsFaulted);
            Assert.True(continuation.Exception.InnerExceptions.Count == 1 && continuation.Exception.InnerExceptions[0] is InvalidOperationException);

            Assert.Equal(1, scheduler.Clock);
        }

        [Fact]
        public void ObservableToTaskSingleValue()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Return(5, scheduler);

            var continuation = xs.ToTask();
            scheduler.Start();

            Assert.True(continuation.IsCompleted);
            Assert.Equal(5, continuation.Result);

            Assert.Equal(1, scheduler.Clock);
        }

        [Fact]
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

            Assert.True(continuation.IsCompleted);
            Assert.Equal(3, continuation.Result);

            xs.Subscriptions.AssertEqual(
                Subscribe(0, 200)
            );
        }

        [Fact]
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

            Assert.True(continuation.IsFaulted);
            var ag = continuation.Exception;
            Assert.NotNull(ag);
            Assert.Equal(1, ag.InnerExceptions.Count);
            Assert.Equal(ex, ag.InnerExceptions[0]);

            xs.Subscriptions.AssertEqual(
                Subscribe(0, 200)
            );
        }

        [Fact]
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

            Assert.True(continuation.IsCanceled);

            xs.Subscriptions.AssertEqual(
                Subscribe(0, 125)
            );
        }

        [Fact]
        public void ObservableToTaskWithStateSingleValue()
        {
            var state = "bar";

            var scheduler = new TestScheduler();

            var xs = Observable.Return(5, scheduler);

            var continuation = xs.ToTask(state);
            Assert.Same(continuation.AsyncState, state);

            scheduler.Start();

            Assert.True(continuation.IsCompleted);
            Assert.Equal(5, continuation.Result);

            Assert.Equal(1, scheduler.Clock);
        }

        #endregion
    }
}
