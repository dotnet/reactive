// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>CreateAsync</c> scenarios, from Rx.NET's <c>CreateAsyncTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 22 of the file's 23 tests (<c>CreateAsync_ArgumentChecking</c> is the code-generated
/// stratum): Rx.NET's six task-based <c>Create</c> overloads, which the six <c>Seq.Create</c>
/// overloads describe with the same semantics on both targets (see <c>Seq.Create.cs</c>). The
/// callbacks are the originals' with the observer calls awaited: a body that ran its work on a
/// started task does so through <c>Task.Run</c>, or <c>Task.Factory.StartNew(…).Unwrap()</c>
/// where the original names <c>StartNew</c>; <c>default(IDisposable)</c> and
/// <c>Disposable.Create</c> become their async counterparts; and <c>Subscribe</c>,
/// <c>ForEach</c> and <c>Wait</c> become <c>SubscribeAsync</c>, <c>ForEachAsync</c> and
/// <c>ToListAsync</c>, awaited.
/// </para>
/// <para>
/// The ten <c>Producer*</c> helpers reschedule themselves through a <c>SerialDisposable</c>
/// and a local <c>Tick</c> where the originals recurse through the scheduler's <c>self</c>
/// callback; the token's registration disposes whichever tick is pending, as the original's
/// disposes the recursion. The virtual-time scenarios keep the original's
/// <c>RunSynchronously</c>, now the target's (see <see cref="IRxTarget.RunSynchronously"/>),
/// which Rx.NET needs for the continuations it hangs off the callback's task and AsyncRx.NET
/// must not run under. <c>CreateAsync_Cancel4</c> asserts its
/// five messages in one <c>AssertEqual</c>, the error by predicate, where the original takes
/// four and then indexes the fifth.
/// </para>
/// </remarks>
public abstract class CreateAsyncTests : SharedReactiveTest
{
    [TestMethod]
    public async Task CreateAsync_NullCoalescingAction1()
    {
        var xs = Seq.Create<int>(async o =>
        {
            await o.OnNextAsync(42);
            return await Task.Factory.StartNew(() => default(Action));
        });

        var lst = new List<int>();
        var d = await xs.SubscribeAsync(Scheduler, lst.Add);
        await d.DisposeAsync();

        Assert.IsTrue(lst.SequenceEqual([42]));
    }

    [TestMethod]
    public async Task CreateAsync_NullCoalescingAction2()
    {
        var xs = Seq.Create<int>(async (o, ct) =>
        {
            await o.OnNextAsync(42);
            return await Task.Factory.StartNew(() => default(Action));
        });

        var lst = new List<int>();
        var d = await xs.SubscribeAsync(Scheduler, lst.Add);
        await d.DisposeAsync();

        Assert.IsTrue(lst.SequenceEqual([42]));
    }

    [TestMethod]
    public async Task CreateAsync_NullCoalescingDisposable1()
    {
        var xs = Seq.Create<int>(async o =>
        {
            await o.OnNextAsync(42);
            return await Task.Factory.StartNew(() => default(IAsyncDisposable));
        });

        var lst = new List<int>();
        var d = await xs.SubscribeAsync(Scheduler, lst.Add);
        await d.DisposeAsync();

        Assert.IsTrue(lst.SequenceEqual([42]));
    }

    [TestMethod]
    public async Task CreateAsync_NullCoalescingDisposable2()
    {
        var xs = Seq.Create<int>(async (o, ct) =>
        {
            await o.OnNextAsync(42);
            return await Task.Factory.StartNew(() => default(IAsyncDisposable));
        });

        var lst = new List<int>();
        var d = await xs.SubscribeAsync(Scheduler, lst.Add);
        await d.DisposeAsync();

        Assert.IsTrue(lst.SequenceEqual([42]));
    }

    private static ValueTask Producer1(ObserverRef<int> results, TestSchedulerRef scheduler, CancellationToken token)
    {
        var tcs = new TaskCompletionSource<object?>();

        var x = 0;

        var d = new SerialDisposable();
        void Tick() => d.Disposable = scheduler.ScheduleRelative(100, async () =>
        {
            await results.OnNextAsync(++x);
            Tick();
        });
        Tick();

        token.Register(d.Dispose);

        return new ValueTask(tcs.Task);
    }

    [TestMethod]
    public void CreateAsync_Never()
    {
        RunSynchronously(() =>
        {
            var res = Scheduler.Start(() =>
                Seq.Create<int>((observer, token) => Producer1(observer, Scheduler, token))
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

    private static ValueTask Producer2(ObserverRef<int> results, TestSchedulerRef scheduler, CancellationToken token)
    {
        var tcs = new TaskCompletionSource<object?>();

        var x = 0;

        var d = new SerialDisposable();
        void Tick() => d.Disposable = scheduler.ScheduleRelative(100, async () =>
        {
            if (x == 4)
            {
                tcs.SetResult(null);
            }
            await results.OnNextAsync(++x);
            Tick();
        });
        Tick();

        token.Register(d.Dispose);

        return new ValueTask(tcs.Task);
    }

    [TestMethod]
    public void CreateAsync_Completed1()
    {
        RunSynchronously(() =>
        {
            var res = Scheduler.Start(() =>
                Seq.Create<int>((observer, token) => Producer2(observer, Scheduler, token))
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

    private static ValueTask Producer3(ObserverRef<int> results, TestSchedulerRef scheduler, CancellationToken token)
    {
        var tcs = new TaskCompletionSource<object?>();

        var x = 0;

        var d = new SerialDisposable();
        void Tick() => d.Disposable = scheduler.ScheduleRelative(100, async () =>
        {
            if (x == 4)
            {
                await results.OnCompletedAsync();
            }
            await results.OnNextAsync(++x);
            Tick();
        });
        Tick();

        token.Register(d.Dispose);

        return new ValueTask(tcs.Task);
    }

    [TestMethod]
    public void CreateAsync_Completed2()
    {
        RunSynchronously(() =>
        {
            var res = Scheduler.Start(() =>
                Seq.Create<int>((observer, token) => Producer3(observer, Scheduler, token))
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

    private static ValueTask Producer4(ObserverRef<int> results, TestSchedulerRef scheduler, Exception exception, CancellationToken token)
    {
        var tcs = new TaskCompletionSource<object?>();

        var x = 0;

        var d = new SerialDisposable();
        void Tick() => d.Disposable = scheduler.ScheduleRelative(100, async () =>
        {
            if (x == 4)
            {
                await results.OnErrorAsync(exception);
            }
            await results.OnNextAsync(++x);
            Tick();
        });
        Tick();

        token.Register(d.Dispose);

        return new ValueTask(tcs.Task);
    }

    [TestMethod]
    public void CreateAsync_Error1()
    {
        RunSynchronously(() =>
        {
            var exception = new Exception();

            var res = Scheduler.Start(() =>
                Seq.Create<int>((observer, token) => Producer4(observer, Scheduler, exception, token))
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

    private static ValueTask Producer5(ObserverRef<int> results, TestSchedulerRef scheduler, Exception exception, CancellationToken token)
    {
        var tcs = new TaskCompletionSource<object?>();

        var x = 0;

        var d = new SerialDisposable();
        void Tick() => d.Disposable = scheduler.ScheduleRelative(100, async () =>
        {
            if (x == 4)
            {
                tcs.SetException(exception);
            }
            await results.OnNextAsync(++x);
            Tick();
        });
        Tick();

        token.Register(d.Dispose);

        return new ValueTask(tcs.Task);
    }

    [TestMethod]
    public void CreateAsync_Error2()
    {
        RunSynchronously(() =>
        {
            var exception = new Exception();

            var res = Scheduler.Start(() =>
                Seq.Create<int>((observer, token) => Producer5(observer, Scheduler, exception, token))
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

    private static ValueTask Producer6(ObserverRef<int> results, Exception exception, CancellationToken token)
    {
        throw exception;
    }

    [TestMethod]
    public void CreateAsync_Error3()
    {
        RunSynchronously(() =>
        {
            var exception = new InvalidOperationException();

            var res = Scheduler.Start(() =>
                Seq.Create<int>((observer, token) => Producer6(observer, exception, token))
            );

            res.Messages.AssertEqual(
                OnError<int>(200, exception)
            );
        });
    }

    private static ValueTask Producer7(ObserverRef<int> results, TestSchedulerRef scheduler, CancellationToken token)
    {
        var tcs = new TaskCompletionSource<object?>();

        var x = 0;

        var d = new SerialDisposable();
        void Tick() => d.Disposable = scheduler.ScheduleRelative(100, async () =>
        {
            if (x == 4)
            {
                tcs.SetResult(null);
            }
            await results.OnNextAsync(++x);
            Tick();
        });
        Tick();

        token.Register(d.Dispose);

        return new ValueTask(tcs.Task);
    }

    [TestMethod]
    public void CreateAsync_Cancel1()
    {
        RunSynchronously(() =>
        {
            var res = Scheduler.Start(() =>
                Seq.Create<int>((observer, token) => Producer7(observer, Scheduler, token)),
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

    private static ValueTask Producer8(ObserverRef<int> results, TestSchedulerRef scheduler, CancellationToken token)
    {
        var tcs = new TaskCompletionSource<object?>();

        var x = 0;

        var d = new SerialDisposable();
        void Tick() => d.Disposable = scheduler.ScheduleRelative(100, async () =>
        {
            if (x == 4)
            {
                await results.OnCompletedAsync();
            }
            await results.OnNextAsync(++x);
            Tick();
        });
        Tick();

        token.Register(d.Dispose);

        return new ValueTask(tcs.Task);
    }

    [TestMethod]
    public void CreateAsync_Cancel2()
    {
        RunSynchronously(() =>
        {
            var res = Scheduler.Start(() =>
                Seq.Create<int>((observer, token) => Producer8(observer, Scheduler, token)),
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

    private static ValueTask Producer9(ObserverRef<int> results, TestSchedulerRef scheduler, CancellationToken token)
    {
        var tcs = new TaskCompletionSource<object?>();

        var x = 0;

        var d = new SerialDisposable();
        void Tick() => d.Disposable = scheduler.ScheduleRelative(100, async () =>
        {
            if (x == 4)
            {
                await results.OnCompletedAsync();
            }
            await results.OnNextAsync(++x);
            Tick();
        });
        Tick();

        token.Register(d.Dispose);

        return new ValueTask(tcs.Task);
    }

    [TestMethod]
    public void CreateAsync_Cancel3()
    {
        RunSynchronously(() =>
        {
            var res = Scheduler.Start(() =>
                Seq.Create<int>((observer, token) => Producer9(observer, Scheduler, token)),
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

    private static ValueTask Producer10(ObserverRef<int> results, TestSchedulerRef scheduler, CancellationToken token)
    {
        var tcs = new TaskCompletionSource<object?>();

        var x = 0;

        var d = new SerialDisposable();
        void Tick() => d.Disposable = scheduler.ScheduleRelative(100, async () =>
        {
            if (x == 4)
            {
                tcs.SetCanceled();
            }
            await results.OnNextAsync(++x);
            Tick();
        });
        Tick();

        token.Register(d.Dispose);

        return new ValueTask(tcs.Task);
    }

    [TestMethod]
    public void CreateAsync_Cancel4()
    {
        RunSynchronously(() =>
        {
            var res = Scheduler.Start(() =>
                Seq.Create<int>((observer, token) => Producer10(observer, Scheduler, token))
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(400, 2),
                OnNext(500, 3),
                OnNext(600, 4),
                OnError<int>(700, ex => ex is OperationCanceledException)
            );
        });
    }

    [TestMethod]
    public async Task CreateAsync_Task_Simple()
    {
        var xs = Seq.Create<int>(async observer =>
        {
            await Task.Run(async () =>
            {
                await observer.OnNextAsync(42);
                await observer.OnCompletedAsync();
            });
        });

        var lst = new List<int>();
        await ForEachAsync(xs, lst.Add);

        Assert.IsTrue(new[] { 42 }.SequenceEqual(lst));
    }

    [TestMethod]
    public async Task CreateAsync_Task_Token()
    {
        var e = new ManualResetEvent(false);

        var xs = Seq.Create<int>(async (observer, ct) =>
        {
            await Task.Factory.StartNew(async () =>
            {
                var i = 0;

                while (!ct.IsCancellationRequested)
                {
                    if (i++ == 10)
                    {
                        e.Set();
                    }

                    await observer.OnNextAsync(42);
                }
            },
            CancellationToken.None).Unwrap();
        });

        var lst = new List<int>();
        var d = await xs.SubscribeAsync(Scheduler, i => { lock (lst) { lst.Add(i); } });

        e.WaitOne();
        await d.DisposeAsync();

        // Although Dispose will set the _isStopped gate in the AutoDetachObserver that our
        // observer gets wrapped in, it's possible that the thread we kicked off had just
        // made one of its calls to observer.OnNext, and that this had just got past the
        // _isStopped gate when we called Dispose, meaning that it might right now be inside
        // List<int>.Add. We're synchronizing access to the list to ensure that any such
        // call has completed by the time we try to inspect the list.

        lock (lst)
        {
            Assert.IsTrue(lst.Take(10).SequenceEqual(Enumerable.Repeat(42, 10)));
        }
    }

    [TestMethod]
    public async Task CreateAsync_IDisposable_Simple()
    {
        var stopped = new ManualResetEvent(false);
        IAsyncDisposable s = new AnonymousAsyncDisposable(() =>
        {
            stopped.Set();
            return default;
        });

        var xs = Seq.Create<int>(async observer =>
        {
            return await Task.Run(async () =>
            {
                await observer.OnNextAsync(42);
                await observer.OnCompletedAsync();

                return s;
            });
        });

        var lst = new List<int>();
        await ForEachAsync(xs, lst.Add);

        stopped.WaitOne();

        Assert.IsTrue(new[] { 42 }.SequenceEqual(lst));
    }

    [TestMethod]
    public async Task CreateAsync_IDisposable_Token()
    {
        var stopped = new ManualResetEvent(false);
        IAsyncDisposable s = new AnonymousAsyncDisposable(() =>
        {
            stopped.Set();
            return default;
        });

        var e = new ManualResetEvent(false);

        var xs = Seq.Create<int>(async (observer, ct) =>
        {
            return await Task.Factory.StartNew(async () =>
            {
                var i = 0;

                while (!ct.IsCancellationRequested)
                {
                    if (i++ == 10)
                    {
                        e.Set();
                    }

                    await observer.OnNextAsync(42);
                }

                return s;
            }).Unwrap();
        });

        var lst = new List<int>();
        var d = await xs.SubscribeAsync(Scheduler, lst.Add);

        e.WaitOne();
        await d.DisposeAsync();
        stopped.WaitOne();

        Assert.IsTrue(lst.Take(10).SequenceEqual(Enumerable.Repeat(42, 10)));
    }

    [TestMethod]
    public async Task CreateAsync_Action_Simple()
    {
        var stopped = new ManualResetEvent(false);
        var s = new Action(() => stopped.Set());

        var xs = Seq.Create<int>(async observer =>
        {
            return await Task.Run(async () =>
            {
                await observer.OnNextAsync(42);
                await observer.OnCompletedAsync();

                return s;
            });
        });

        var lst = new List<int>();
        await ForEachAsync(xs, lst.Add);

        stopped.WaitOne();

        Assert.IsTrue(new[] { 42 }.SequenceEqual(lst));
    }

    [TestMethod]
    public async Task CreateAsync_Action_Token()
    {
        var stopped = new ManualResetEvent(false);
        var s = new Action(() => stopped.Set());

        var e = new ManualResetEvent(false);

        var xs = Seq.Create<int>(async (observer, ct) =>
        {
            return await Task.Factory.StartNew(async () =>
            {
                var i = 0;

                while (!ct.IsCancellationRequested)
                {
                    if (i++ == 10)
                    {
                        e.Set();
                    }

                    await observer.OnNextAsync(42);
                }

                return s;
            }).Unwrap();
        });

        var lst = new List<int>();
        var d = await xs.SubscribeAsync(Scheduler, lst.Add);

        e.WaitOne();
        await d.DisposeAsync();
        stopped.WaitOne();

        Assert.IsTrue(lst.Take(10).SequenceEqual(Enumerable.Repeat(42, 10)));
    }

    [TestMethod]
    public async Task CreateWithTaskDisposable_NoPrematureTermination()
    {
        var obs = Seq.Create<int>(async o =>
        {
            // avoid warning on async o due to no await
            await Task.CompletedTask;

            var inner = Seq.Range(1, 3);

            return await inner.SubscribeAsync(Scheduler, x => o.OnNextAsync(x));
        });

        var result = await ToListAsync(obs.Take(1));
    }

    [TestMethod]
    public async Task CreateWithTaskAction_NoPrematureTermination()
    {
        var obs = Seq.Create<int>(async o =>
        {
            // avoid warning on async o due to no await
            await Task.CompletedTask;

            var inner = Seq.Range(1, 3);

            var d = await inner.SubscribeAsync(Scheduler, x => o.OnNextAsync(x));

#pragma warning disable IDE0039 // (Use local function.) We are testing for a returned Action, and want to be explicit about that.
            Action a = () => d.DisposeAsync().AsTask().Wait();
#pragma warning restore IDE0039
            return a;
        });

        var result = await ToListAsync(obs.Take(1));
    }
}
