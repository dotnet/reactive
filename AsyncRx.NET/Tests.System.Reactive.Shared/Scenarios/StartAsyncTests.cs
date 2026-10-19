// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>StartAsync</c> scenarios, from Rx.NET's <c>StartAsyncTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 18 of the file's 36 tests, the twins of the <c>FromAsync</c> ones with the function run at
/// the call rather than per subscription. The two <c>*_Cancel</c> tests wait for the task to
/// start before subscribing, so they realize the description first through
/// <see cref="SharedReactiveTest.Realize{T}"/>, where the original's call starts the task;
/// the rest are as the <c>FromAsync</c> scenarios.
/// </para>
/// <para>
/// Not here: two argument-checking tests, and the sixteen <c>*UnsubscribeThenError*</c> tests
/// over <c>TaskObservationOptions</c>, in <c>Tests.System.Reactive.Shared.Rx</c> as the
/// originals.
/// </para>
/// </remarks>
public abstract class StartAsyncTests : SharedReactiveTest
{
    [TestMethod]
    public async Task StartAsync_Func_Success()
    {
        var n = 42;

        var i = 0;

        var xs = Seq.StartAsync(() =>
        {
            i++;
            return Task.Factory.StartNew(() => n);
        });

        Assert.AreEqual(n, (await ToListAsync(xs)).Single());
        Assert.AreEqual(1, i);

        Assert.AreEqual(n, (await ToListAsync(xs)).Single());
        Assert.AreEqual(1, i);
    }

    [TestMethod]
    public async Task StartAsync_Func_Throw_Synchronous()
    {
        var ex = new Exception();

        var xs = Seq.StartAsync<int>(() =>
        {
            throw ex;
        });

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(async () => await ToListAsync(xs)));
    }

    [TestMethod]
    public async Task StartAsync_Func_Throw_Asynchronous()
    {
        var ex = new Exception();

        var xs = Seq.StartAsync(() =>
            Task.Factory.StartNew<int>(() =>
            {
                throw ex;
            })
        );

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(async () => await ToListAsync(xs)));
    }

    [TestMethod]
    public async Task StartAsync_FuncWithCancel_Success()
    {
        var n = 42;

        var i = 0;
        var xs = Seq.StartAsync(ct =>
        {
            i++;
            return Task.Factory.StartNew(() => n);
        });

        Assert.AreEqual(n, (await ToListAsync(xs)).Single());
        Assert.AreEqual(1, i);

        Assert.AreEqual(n, (await ToListAsync(xs)).Single());
        Assert.AreEqual(1, i);
    }

    [TestMethod]
    public async Task StartAsync_FuncWithCancel_Throw_Synchronous()
    {
        var ex = new Exception();

        var xs = Seq.StartAsync<int>(ct =>
        {
            throw ex;
        });

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(async () => await ToListAsync(xs)));
    }

    [TestMethod]
    public async Task StartAsync_FuncWithCancel_Throw_Asynchronous()
    {
        var ex = new Exception();

        var xs = Seq.StartAsync(ct =>
            Task.Factory.StartNew<int>(() => { throw ex; })
        );

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(async () => await ToListAsync(xs)));
    }

    [TestMethod]
    public async Task StartAsync_FuncWithCancel_Cancel()
    {
        var N = 10;

        for (var n = 0; n < N; n++)
        {
            var e = new ManualResetEvent(false);
            var f = new ManualResetEvent(false);

            Task<int>? t = null;
            var xs = Seq.StartAsync(ct =>
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

            Realize(xs);

            e.WaitOne();

            var d = await xs.SubscribeAsync(Scheduler, _ => { });
            await d.DisposeAsync();

            f.WaitOne();
            while (!t!.IsCompleted)
            {
                ;
            }

            await Assert.ThrowsAsync<OperationCanceledException>(async () => await ToListAsync(xs));
        }
    }

    [TestMethod]
    public async Task StartAsync_Func_Scheduler1()
    {
        var tcs = new TaskCompletionSource<int>();

        var e = new ManualResetEvent(false);
        var x = default(int);
        var t = default(int);

        var xs = Seq.StartAsync(() => tcs.Task, ImmediateScheduler);
        await xs.SubscribeAsync(Scheduler, res =>
        {
            x = res;
            t = Environment.CurrentManagedThreadId;
            e.Set();
        });

        tcs.SetResult(42);

        e.WaitOne();

        Assert.AreEqual(42, x);
        Assert.AreEqual(Environment.CurrentManagedThreadId, t);
    }

    [TestMethod]
    public async Task StartAsync_Func_Scheduler2()
    {
        var tcs = new TaskCompletionSource<int>();

        var e = new ManualResetEvent(false);
        var x = default(int);
        var t = default(int);

        var xs = Seq.StartAsync(ct => tcs.Task, ImmediateScheduler);
        await xs.SubscribeAsync(Scheduler, res =>
        {
            x = res;
            t = Environment.CurrentManagedThreadId;
            e.Set();
        });

        tcs.SetResult(42);

        e.WaitOne();

        Assert.AreEqual(42, x);
        Assert.AreEqual(Environment.CurrentManagedThreadId, t);
    }

    [TestMethod]
    public async Task StartAsync_Action_Success()
    {
        var i = 0;
        var xs = Seq.StartAsync(() =>
        {
            i++;
            return Task.Factory.StartNew(() => { });
        });

        Assert.AreEqual(Unit.Default, (await ToListAsync(xs)).Single());
        Assert.AreEqual(1, i);

        Assert.AreEqual(Unit.Default, (await ToListAsync(xs)).Single());
        Assert.AreEqual(1, i);
    }

    [TestMethod]
    public async Task StartAsync_Action_Throw_Synchronous()
    {
        var ex = new Exception();

        var xs = Seq.StartAsync(() =>
        {
            throw ex;
        });

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(async () => await ToListAsync(xs)));
    }

    [TestMethod]
    public async Task StartAsync_Action_Throw_Asynchronous()
    {
        var ex = new Exception();

        var xs = Seq.StartAsync(() =>
            Task.Factory.StartNew(() => { throw ex; })
        );

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(async () => await ToListAsync(xs)));
    }

    [TestMethod]
    public async Task StartAsync_ActionWithCancel_Success()
    {
        var i = 0;
        var xs = Seq.StartAsync(ct =>
        {
            i++;
            // Not forwarding ct because we always want this task to run to completion in this test.
            return Task.Factory.StartNew(() => { }, CancellationToken.None);
        });

        Assert.AreEqual(Unit.Default, (await ToListAsync(xs)).Single());
        Assert.AreEqual(1, i);

        Assert.AreEqual(Unit.Default, (await ToListAsync(xs)).Single());
        Assert.AreEqual(1, i);
    }

    [TestMethod]
    public async Task StartAsync_ActionWithCancel_Throw_Synchronous()
    {
        var ex = new Exception();

        var xs = Seq.StartAsync(ct =>
        {
            throw ex;
        });

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(async () => await ToListAsync(xs)));
    }

    [TestMethod]
    public async Task StartAsync_ActionWithCancel_Throw_Asynchronous()
    {
        var ex = new Exception();

        var xs = Seq.StartAsync(ct =>
            // Not forwarding ct because we always want this task to run and then fail in this test
            Task.Factory.StartNew(() => { throw ex; }, CancellationToken.None)
        );

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(async () => await ToListAsync(xs)));
    }

    [TestMethod]
    public async Task StartAsync_ActionWithCancel_Cancel()
    {
        var N = 10;

        for (var n = 0; n < N; n++)
        {
            var e = new ManualResetEvent(false);
            var f = new ManualResetEvent(false);

            Task? t = null;
            var xs = Seq.StartAsync(ct =>
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
                // Not forwarding ct because we are testing the case where the task is already
                // running by the time cancellation is detected
                CancellationToken.None)
            );

            Realize(xs);

            e.WaitOne();

            var d = await xs.SubscribeAsync(Scheduler, _ => { });
            await d.DisposeAsync();

            f.WaitOne();
            while (!t!.IsCompleted)
            {
                ;
            }

            await Assert.ThrowsAsync<OperationCanceledException>(async () => await ToListAsync(xs));
        }
    }

    [TestMethod]
    public async Task StartAsync_Action_Scheduler1()
    {
        var tcs = new TaskCompletionSource<int>();

        var e = new ManualResetEvent(false);
        var t = default(int);

        var xs = Seq.StartAsync(() => (Task)tcs.Task, ImmediateScheduler);
        await xs.SubscribeAsync(Scheduler, res =>
        {
            t = Environment.CurrentManagedThreadId;
            e.Set();
        });

        tcs.SetResult(42);

        e.WaitOne();

        Assert.AreEqual(Environment.CurrentManagedThreadId, t);
    }

    [TestMethod]
    public async Task StartAsync_Action_Scheduler2()
    {
        var tcs = new TaskCompletionSource<int>();

        var e = new ManualResetEvent(false);
        var t = default(int);

        var xs = Seq.StartAsync(ct => (Task)tcs.Task, ImmediateScheduler);
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
