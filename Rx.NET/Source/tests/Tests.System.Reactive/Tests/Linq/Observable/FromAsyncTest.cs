// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Xunit;
using ReactiveTests.Dummies;
using System.Threading;

namespace ReactiveTests.Tests
{
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

        [Fact]
        public void FromAsync_Func_ArgumentChecking()
        {
            var s = Scheduler.Immediate;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync<int>(default(Func<Task<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync<int>(default(Func<CancellationToken, Task<int>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync<int>(default(Func<Task<int>>), s));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync<int>(() => _doneTask, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync<int>(default(Func<CancellationToken, Task<int>>), s));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync<int>(ct => _doneTask, default(IScheduler)));
        }

        [Fact]
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

        [Fact]
        public void FromAsync_Func_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync<int>(() =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
        public void FromAsync_Func_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync<int>(() =>
                Task.Factory.StartNew<int>(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
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

        [Fact]
        public void FromAsync_FuncWithCancel_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync<int>(ct =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
        public void FromAsync_FuncWithCancel_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync<int>(ct =>
                Task.Factory.StartNew<int>(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
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
                            ct.ThrowIfCancellationRequested();
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
                ;
        }

#if DESKTOPCLR
        [Fact]
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
                t = Thread.CurrentThread.ManagedThreadId;
                e.Set();
            });

            tcs.SetResult(42);

            e.WaitOne();

            Assert.Equal(42, x);
            Assert.Equal(Thread.CurrentThread.ManagedThreadId, t);
        }

        [Fact]
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
                t = Thread.CurrentThread.ManagedThreadId;
                e.Set();
            });

            tcs.SetResult(42);

            e.WaitOne();

            Assert.Equal(42, x);
            Assert.Equal(Thread.CurrentThread.ManagedThreadId, t);
        }
#endif

        #endregion

        #region Action

        [Fact]
        public void FromAsync_Action_ArgumentChecking()
        {
            var s = Scheduler.Immediate;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(default(Func<Task>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(default(Func<CancellationToken, Task>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(default(Func<Task>), s));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(() => (Task)_doneTask, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(default(Func<CancellationToken, Task>), s));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FromAsync(ct => (Task)_doneTask, default(IScheduler)));
        }

        [Fact]
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

        [Fact]
        public void FromAsync_Action_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync(() =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
        public void FromAsync_Action_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync(() =>
                Task.Factory.StartNew(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
        public void FromAsync_ActionWithCancel_Success()
        {
            var i = 0;
            var xs = Observable.FromAsync(ct =>
            {
                i++;
                return Task.Factory.StartNew(() => { });
            });

            Assert.Equal(Unit.Default, xs.Single());
            Assert.Equal(1, i);

            Assert.Equal(Unit.Default, xs.Single());
            Assert.Equal(2, i);
        }

        [Fact]
        public void FromAsync_ActionWithCancel_Throw_Synchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync(ct =>
            {
                throw ex;
            });

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
        public void FromAsync_ActionWithCancel_Throw_Asynchronous()
        {
            var ex = new Exception();

            var xs = Observable.FromAsync(ct =>
                Task.Factory.StartNew(() => { throw ex; })
            );

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [Fact]
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
                            ct.ThrowIfCancellationRequested();
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
                ;
        }

#if DESKTOPCLR
        [Fact]
        public void FromAsync_Action_Scheduler1()
        {
            var e = new ManualResetEvent(false);
            var t = default(int);

            var tcs = new TaskCompletionSource<int>();

            var xs = Observable.FromAsync(() => (Task)tcs.Task, Scheduler.Immediate);
            xs.Subscribe(res =>
            {
                t = Thread.CurrentThread.ManagedThreadId;
                e.Set();
            });

            tcs.SetResult(42);

            e.WaitOne();

            Assert.Equal(Thread.CurrentThread.ManagedThreadId, t);
        }

        [Fact]
        public void FromAsync_Action_Scheduler2()
        {
            var e = new ManualResetEvent(false);
            var t = default(int);

            var tcs = new TaskCompletionSource<int>();

            var xs = Observable.FromAsync(ct => (Task)tcs.Task, Scheduler.Immediate);
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

    }
}
