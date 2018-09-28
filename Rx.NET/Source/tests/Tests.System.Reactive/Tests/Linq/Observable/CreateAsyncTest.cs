// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class CreateAsyncTest : ReactiveTest
    {

        [Fact]
        public void CreateAsync_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create(default(Func<IObserver<int>, Task>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create(default(Func<IObserver<int>, CancellationToken, Task>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create(default(Func<IObserver<int>, Task<IDisposable>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create(default(Func<IObserver<int>, CancellationToken, Task<IDisposable>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create(default(Func<IObserver<int>, Task<Action>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Create(default(Func<IObserver<int>, CancellationToken, Task<Action>>)));
        }

        [Fact]
        public void CreateAsync_NullCoalescingAction1()
        {
            var xs = Observable.Create<int>(o =>
            {
                o.OnNext(42);
                return Task.Factory.StartNew(() => default(Action));
            });

            var lst = new List<int>();
            var d = xs.Subscribe(lst.Add);
            d.Dispose();

            Assert.True(lst.SequenceEqual(new[] { 42 }));
        }

        [Fact]
        public void CreateAsync_NullCoalescingAction2()
        {
            var xs = Observable.Create<int>((o, ct) =>
            {
                o.OnNext(42);
                return Task.Factory.StartNew(() => default(Action));
            });

            var lst = new List<int>();
            var d = xs.Subscribe(lst.Add);
            d.Dispose();

            Assert.True(lst.SequenceEqual(new[] { 42 }));
        }

        [Fact]
        public void CreateAsync_NullCoalescingDisposable1()
        {
            var xs = Observable.Create<int>(o =>
            {
                o.OnNext(42);
                return Task.Factory.StartNew(() => default(IDisposable));
            });

            var lst = new List<int>();
            var d = xs.Subscribe(lst.Add);
            d.Dispose();

            Assert.True(lst.SequenceEqual(new[] { 42 }));
        }

        [Fact]
        public void CreateAsync_NullCoalescingDisposable2()
        {
            var xs = Observable.Create<int>((o, ct) =>
            {
                o.OnNext(42);
                return Task.Factory.StartNew(() => default(IDisposable));
            });

            var lst = new List<int>();
            var d = xs.Subscribe(lst.Add);
            d.Dispose();

            Assert.True(lst.SequenceEqual(new[] { 42 }));
        }

        private Task Producer1(IObserver<int> results, CancellationToken token, IScheduler scheduler)
        {
            var tcs = new TaskCompletionSource<object>();

            var x = 0;

            var d = scheduler.Schedule(TimeSpan.FromTicks(100), self =>
            {
                results.OnNext(++x);
                self(TimeSpan.FromTicks(100));
            });

            token.Register(d.Dispose);

            return tcs.Task;
        }

        [Fact]
        public void CreateAsync_Never()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer1(observer, token, scheduler))
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(400, 2),
                    OnNext(500, 3),
                    OnNext(600, 4),
                    OnNext(700, 5),
                    OnNext(800, 6),
                    OnNext(900, 7)
                );
            });
        }

        private Task Producer2(IObserver<int> results, CancellationToken token, IScheduler scheduler)
        {
            var tcs = new TaskCompletionSource<object>();

            var x = 0;

            var d = scheduler.Schedule(TimeSpan.FromTicks(100), self =>
            {
                if (x == 4)
                {
                    tcs.SetResult(null);
                }
                results.OnNext(++x);
                self(TimeSpan.FromTicks(100));
            });

            token.Register(d.Dispose);

            return tcs.Task;
        }

        [Fact]
        public void CreateAsync_Completed1()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer2(observer, token, scheduler))
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(400, 2),
                    OnNext(500, 3),
                    OnNext(600, 4),
                    OnCompleted<int>(700)
                );
            });
        }

        private Task Producer3(IObserver<int> results, CancellationToken token, IScheduler scheduler)
        {
            var tcs = new TaskCompletionSource<object>();

            var x = 0;

            var d = scheduler.Schedule(TimeSpan.FromTicks(100), self =>
            {
                if (x == 4)
                {
                    results.OnCompleted();
                }
                results.OnNext(++x);
                self(TimeSpan.FromTicks(100));
            });

            token.Register(d.Dispose);

            return tcs.Task;
        }

        [Fact]
        public void CreateAsync_Completed2()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer3(observer, token, scheduler))
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(400, 2),
                    OnNext(500, 3),
                    OnNext(600, 4),
                    OnCompleted<int>(700)
                );
            });
        }

        private Task Producer4(IObserver<int> results, CancellationToken token, IScheduler scheduler, Exception exception)
        {
            var tcs = new TaskCompletionSource<object>();

            var x = 0;

            var d = scheduler.Schedule(TimeSpan.FromTicks(100), self =>
            {
                if (x == 4)
                {
                    results.OnError(exception);
                }
                results.OnNext(++x);
                self(TimeSpan.FromTicks(100));
            });

            token.Register(d.Dispose);

            return tcs.Task;
        }

        [Fact]
        public void CreateAsync_Error1()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var exception = new Exception();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer4(observer, token, scheduler, exception))
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(400, 2),
                    OnNext(500, 3),
                    OnNext(600, 4),
                    OnError<int>(700, exception)
                );
            });
        }

        private Task Producer5(IObserver<int> results, CancellationToken token, IScheduler scheduler, Exception exception)
        {
            var tcs = new TaskCompletionSource<object>();

            var x = 0;

            var d = scheduler.Schedule(TimeSpan.FromTicks(100), self =>
            {
                if (x == 4)
                {
                    tcs.SetException(exception);
                }
                results.OnNext(++x);
                self(TimeSpan.FromTicks(100));
            });

            token.Register(d.Dispose);

            return tcs.Task;
        }

        [Fact]
        public void CreateAsync_Error2()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var exception = new Exception();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer5(observer, token, scheduler, exception))
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(400, 2),
                    OnNext(500, 3),
                    OnNext(600, 4),
                    OnError<int>(700, exception)
                );
            });
        }

        private Task Producer6(IObserver<int> results, CancellationToken token, Exception exception)
        {
            throw exception;
        }

        [Fact]
        public void CreateAsync_Error3()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var exception = new InvalidOperationException();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer6(observer, token, exception))
                );

                res.Messages.AssertEqual(
                    OnError<int>(200, exception)
                );
            });
        }

        private Task Producer7(IObserver<int> results, CancellationToken token, IScheduler scheduler)
        {
            var tcs = new TaskCompletionSource<object>();

            var x = 0;

            var d = scheduler.Schedule(TimeSpan.FromTicks(100), self =>
            {
                if (x == 4)
                {
                    tcs.SetResult(null);
                }
                results.OnNext(++x);
                self(TimeSpan.FromTicks(100));
            });

            token.Register(d.Dispose);

            return tcs.Task;
        }

        [Fact]
        public void CreateAsync_Cancel1()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer7(observer, token, scheduler)),
                    650
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(400, 2),
                    OnNext(500, 3),
                    OnNext(600, 4)
                );
            });
        }

        private Task Producer8(IObserver<int> results, CancellationToken token, IScheduler scheduler)
        {
            var tcs = new TaskCompletionSource<object>();

            var x = 0;

            var d = scheduler.Schedule(TimeSpan.FromTicks(100), self =>
            {
                if (x == 4)
                {
                    results.OnCompleted();
                }
                results.OnNext(++x);
                self(TimeSpan.FromTicks(100));
            });

            token.Register(d.Dispose);

            return tcs.Task;
        }

        [Fact]
        public void CreateAsync_Cancel2()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer8(observer, token, scheduler)),
                    650
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(400, 2),
                    OnNext(500, 3),
                    OnNext(600, 4)
                );
            });
        }

        private Task Producer9(IObserver<int> results, CancellationToken token, IScheduler scheduler)
        {
            var tcs = new TaskCompletionSource<object>();

            var x = 0;

            var d = scheduler.Schedule(TimeSpan.FromTicks(100), self =>
            {
                if (x == 4)
                {
                    results.OnCompleted();
                }
                results.OnNext(++x);
                self(TimeSpan.FromTicks(100));
            });

            token.Register(d.Dispose);

            return tcs.Task;
        }

        [Fact]
        public void CreateAsync_Cancel3()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer9(observer, token, scheduler)),
                    750
                );

                res.Messages.AssertEqual(
                    OnNext(300, 1),
                    OnNext(400, 2),
                    OnNext(500, 3),
                    OnNext(600, 4),
                    OnCompleted<int>(700)
                );
            });
        }

        private Task Producer10(IObserver<int> results, CancellationToken token, IScheduler scheduler)
        {
            var tcs = new TaskCompletionSource<object>();

            var x = 0;

            var d = scheduler.Schedule(TimeSpan.FromTicks(100), self =>
            {
                if (x == 4)
                {
                    tcs.SetCanceled();
                }
                results.OnNext(++x);
                self(TimeSpan.FromTicks(100));
            });

            token.Register(d.Dispose);

            return tcs.Task;
        }

        [Fact]
        public void CreateAsync_Cancel4()
        {
            RunSynchronously(() =>
            {
                var scheduler = new TestScheduler();

                var res = scheduler.Start(() =>
                    Observable.Create<int>((observer, token) => Producer10(observer, token, scheduler))
                );

                res.Messages.Take(4).AssertEqual(
                    OnNext(300, 1),
                    OnNext(400, 2),
                    OnNext(500, 3),
                    OnNext(600, 4)
                );

                Assert.Equal(5, res.Messages.Count);

                Assert.Equal(700, res.Messages[4].Time);
                Assert.Equal(NotificationKind.OnError, res.Messages[4].Value.Kind);
                Assert.True(res.Messages[4].Value.Exception is OperationCanceledException);
            });
        }

        private void RunSynchronously(Action action)
        {
            var t = new Task(action);
            t.RunSynchronously(new SynchronousScheduler());
            t.Wait();
        }

        private class SynchronousScheduler : TaskScheduler
        {
            protected override IEnumerable<Task> GetScheduledTasks()
            {
                throw new NotImplementedException();
            }

            protected override void QueueTask(Task task)
            {
                TryExecuteTask(task);
            }

            protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
            {
                return TryExecuteTask(task);
            }
        }

        [Fact]
        public void CreateAsync_Task_Simple()
        {
            var xs = Observable.Create<int>(observer =>
            {
                return Task.Factory.StartNew(() =>
                {
                    observer.OnNext(42);
                    observer.OnCompleted();
                });
            });

            var lst = new List<int>();
            xs.ForEach(lst.Add);

            Assert.True(new[] { 42 }.SequenceEqual(lst));
        }

        [Fact]
        public void CreateAsync_Task_Token()
        {
            var e = new ManualResetEvent(false);

            var xs = Observable.Create<int>((observer, ct) =>
            {
                return Task.Factory.StartNew(() =>
                {
                    var i = 0;

                    while (!ct.IsCancellationRequested)
                    {
                        if (i++ == 10)
                        {
                            e.Set();
                        }

                        observer.OnNext(42);
                    }
                });
            });

            var lst = new List<int>();
            var d = xs.Subscribe(lst.Add);

            e.WaitOne();
            d.Dispose();

            Assert.True(lst.Take(10).SequenceEqual(Enumerable.Repeat(42, 10)));
        }

        [Fact]
        public void CreateAsync_IDisposable_Simple()
        {
            var stopped = new ManualResetEvent(false);
            var s = Disposable.Create(() => stopped.Set());

            var xs = Observable.Create<int>(observer =>
            {
                return Task.Factory.StartNew(() =>
                {
                    observer.OnNext(42);
                    observer.OnCompleted();

                    return s;
                });
            });

            var lst = new List<int>();
            xs.ForEach(lst.Add);

            stopped.WaitOne();

            Assert.True(new[] { 42 }.SequenceEqual(lst));
        }

        [Fact]
        public void CreateAsync_IDisposable_Token()
        {
            var stopped = new ManualResetEvent(false);
            var s = Disposable.Create(() => stopped.Set());

            var e = new ManualResetEvent(false);

            var xs = Observable.Create<int>((observer, ct) =>
            {
                return Task.Factory.StartNew(() =>
                {
                    var i = 0;

                    while (!ct.IsCancellationRequested)
                    {
                        if (i++ == 10)
                        {
                            e.Set();
                        }

                        observer.OnNext(42);
                    }

                    return s;
                });
            });

            var lst = new List<int>();
            var d = xs.Subscribe(lst.Add);

            e.WaitOne();
            d.Dispose();
            stopped.WaitOne();

            Assert.True(lst.Take(10).SequenceEqual(Enumerable.Repeat(42, 10)));
        }

        [Fact]
        public void CreateAsync_Action_Simple()
        {
            var stopped = new ManualResetEvent(false);
            var s = new Action(() => stopped.Set());

            var xs = Observable.Create<int>(observer =>
            {
                return Task.Factory.StartNew(() =>
                {
                    observer.OnNext(42);
                    observer.OnCompleted();

                    return s;
                });
            });

            var lst = new List<int>();
            xs.ForEach(lst.Add);

            stopped.WaitOne();

            Assert.True(new[] { 42 }.SequenceEqual(lst));
        }

        [Fact]
        public void CreateAsync_Action_Token()
        {
            var stopped = new ManualResetEvent(false);
            var s = new Action(() => stopped.Set());

            var e = new ManualResetEvent(false);

            var xs = Observable.Create<int>((observer, ct) =>
            {
                return Task.Factory.StartNew(() =>
                {
                    var i = 0;

                    while (!ct.IsCancellationRequested)
                    {
                        if (i++ == 10)
                        {
                            e.Set();
                        }

                        observer.OnNext(42);
                    }

                    return s;
                });
            });

            var lst = new List<int>();
            var d = xs.Subscribe(lst.Add);

            e.WaitOne();
            d.Dispose();
            stopped.WaitOne();

            Assert.True(lst.Take(10).SequenceEqual(Enumerable.Repeat(42, 10)));
        }


        [Fact]
        public void CreateWithTaskDisposable_NoPrematureTermination()
        {
            var obs = Observable.Create<int>(async o =>
            {
                // avoid warning on async o due to no await
                await Task.CompletedTask;

                var inner = Observable.Range(1, 3);

                return inner.Subscribe(x =>
                {
                    o.OnNext(x);
                });
            });

            var result = obs.Take(1).Wait();
        }

        [Fact]
        public void CreateWithTaskAction_NoPrematureTermination()
        {
            var obs = Observable.Create<int>(async o =>
            {
                // avoid warning on async o due to no await
                await Task.CompletedTask;

                var inner = Observable.Range(1, 3);

                var d = inner.Subscribe(x =>
                {
                    o.OnNext(x);
                });

                Action a = () => d.Dispose();
                return a;
            });

            var result = obs.Take(1).Wait();
        }
    }
}
