// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>FromAsync</c> scenarios, from Rx.NET's <c>FromAsyncTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 18 of the file's 36 tests. The function and action forms, with and without a cancellation
/// token: success (the function runs once per subscription), a synchronous and an asynchronous
/// throw, cancellation of a running task on disposal, and delivery on the thread completing
/// the task under the immediate scheduler (the four <c>*_Scheduler1/2</c> tests, inside
/// <c>#if DESKTOPCLR</c> upstream for a reason that no longer holds; see Step 59). Where the
/// original blocks on <c>Single()</c>, the scenario awaits
/// <see cref="SharedReactiveTest.ToListAsync{T}"/> and takes the single element; the handler
/// subscriptions go through the kit's handler form.
/// </para>
/// <para>
/// Not here: two argument-checking tests (the code-generated stratum), and the sixteen
/// <c>*UnsubscribeThenError*</c> tests, which exercise Rx.NET's <c>TaskObservationOptions</c>,
/// a choice AsyncRx.NET does not offer yet (a deferred work item); they run in
/// <c>Tests.System.Reactive.Shared.Rx</c> as the originals, with Rx.NET's
/// <c>TaskErrorObservation</c> helper.
/// </para>
/// </remarks>
public abstract class FromAsyncTests : SharedReactiveTest
{
    [TestMethod]
    public async Task FromAsync_Func_Success()
    {
        var n = 42;

        var i = 0;
        var xs = Seq.FromAsync(() =>
        {
            i++;
            return Task.Factory.StartNew(() => n);
        });

        Assert.AreEqual(n, (await ToListAsync(xs)).Single());
        Assert.AreEqual(1, i);

        Assert.AreEqual(n, (await ToListAsync(xs)).Single());
        Assert.AreEqual(2, i);
    }

    [TestMethod]
    public async Task FromAsync_Func_Throw_Synchronous()
    {
        var ex = new Exception();

        var xs = Seq.FromAsync<int>(() =>
        {
            throw ex;
        });

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(async () => await ToListAsync(xs)));
    }

    [TestMethod]
    public async Task FromAsync_Func_Throw_Asynchronous()
    {
        var ex = new Exception();

        var xs = Seq.FromAsync(() =>
            Task.Factory.StartNew<int>(() => { throw ex; })
        );

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(async () => await ToListAsync(xs)));
    }

    [TestMethod]
    public async Task FromAsync_FuncWithCancel_Success()
    {
        var n = 42;

        var i = 0;
        var xs = Seq.FromAsync(ct =>
        {
            i++;
            return Task.Factory.StartNew(() => n);
        });

        Assert.AreEqual(n, (await ToListAsync(xs)).Single());
        Assert.AreEqual(1, i);

        Assert.AreEqual(n, (await ToListAsync(xs)).Single());
        Assert.AreEqual(2, i);
    }

    [TestMethod]
    public async Task FromAsync_FuncWithCancel_Throw_Synchronous()
    {
        var ex = new Exception();

        var xs = Seq.FromAsync<int>(ct =>
        {
            throw ex;
        });

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(async () => await ToListAsync(xs)));
    }

    [TestMethod]
    public async Task FromAsync_FuncWithCancel_Throw_Asynchronous()
    {
        var ex = new Exception();

        var xs = Seq.FromAsync(ct =>
            Task.Factory.StartNew<int>(() => { throw ex; })
        );

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(async () => await ToListAsync(xs)));
    }

    [TestMethod]
    public async Task FromAsync_FuncWithCancel_Cancel()
    {
        var e = new ManualResetEvent(false);
        var f = new ManualResetEvent(false);

        Task<int>? t = null;
        var xs = Seq.FromAsync(ct =>
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

        var d = await xs.SubscribeAsync(Scheduler, _ => { });
        e.WaitOne();
        await d.DisposeAsync();

        f.WaitOne();
        while (!t!.IsCompleted)
        {
            ;
        }
    }

    [TestMethod]
    public async Task FromAsync_Func_Scheduler1()
    {
        var e = new ManualResetEvent(false);
        var x = default(int);
        var t = default(int);

        var tcs = new TaskCompletionSource<int>();

        var xs = Seq.FromAsync(() => tcs.Task, ImmediateScheduler);
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
    public async Task FromAsync_Func_Scheduler2()
    {
        var e = new ManualResetEvent(false);
        var x = default(int);
        var t = default(int);

        var tcs = new TaskCompletionSource<int>();

        var xs = Seq.FromAsync(ct => tcs.Task, ImmediateScheduler);
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
    public async Task FromAsync_Action_Success()
    {
        var i = 0;
        var xs = Seq.FromAsync(() =>
        {
            i++;
            return Task.Factory.StartNew(() => { });
        });

        Assert.AreEqual(Unit.Default, (await ToListAsync(xs)).Single());
        Assert.AreEqual(1, i);

        Assert.AreEqual(Unit.Default, (await ToListAsync(xs)).Single());
        Assert.AreEqual(2, i);
    }

    [TestMethod]
    public async Task FromAsync_Action_Throw_Synchronous()
    {
        var ex = new Exception();

        var xs = Seq.FromAsync(() =>
        {
            throw ex;
        });

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(async () => await ToListAsync(xs)));
    }

    [TestMethod]
    public async Task FromAsync_Action_Throw_Asynchronous()
    {
        var ex = new Exception();

        var xs = Seq.FromAsync(() =>
            Task.Factory.StartNew(() => { throw ex; })
        );

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(async () => await ToListAsync(xs)));
    }

    [TestMethod]
    public async Task FromAsync_ActionWithCancel_Success()
    {
        var i = 0;
        var xs = Seq.FromAsync(ct =>
        {
            i++;
            // Not forwarding ct because we want this task always to run and complete.
            return Task.Factory.StartNew(() => { }, CancellationToken.None);
        });

        Assert.AreEqual(Unit.Default, (await ToListAsync(xs)).Single());
        Assert.AreEqual(1, i);

        Assert.AreEqual(Unit.Default, (await ToListAsync(xs)).Single());
        Assert.AreEqual(2, i);
    }

    [TestMethod]
    public async Task FromAsync_ActionWithCancel_Throw_Synchronous()
    {
        var ex = new Exception();

        var xs = Seq.FromAsync(ct =>
        {
            throw ex;
        });

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(async () => await ToListAsync(xs)));
    }

    [TestMethod]
    public async Task FromAsync_ActionWithCancel_Throw_Asynchronous()
    {
        var ex = new Exception();

        var xs = Seq.FromAsync(ct =>
            // Not forwarding ct because we always want this task to run and fail
            Task.Factory.StartNew(() => { throw ex; }, CancellationToken.None)
        );

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(async () => await ToListAsync(xs)));
    }

    [TestMethod]
    public async Task FromAsync_ActionWithCancel_Cancel()
    {
        var e = new ManualResetEvent(false);
        var f = new ManualResetEvent(false);

        Task? t = null;
        var xs = Seq.FromAsync(ct =>
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
            // Not forwarding ct because we are testing the case where the task is already running
            // by the time cancellation is detected
            CancellationToken.None)
        );

        var d = await xs.SubscribeAsync(Scheduler, _ => { });
        e.WaitOne();
        await d.DisposeAsync();

        f.WaitOne();
        while (!t!.IsCompleted)
        {
            ;
        }
    }

    [TestMethod]
    public async Task FromAsync_Action_Scheduler1()
    {
        var e = new ManualResetEvent(false);
        var t = default(int);

        var tcs = new TaskCompletionSource<int>();

        var xs = Seq.FromAsync(() => (Task)tcs.Task, ImmediateScheduler);
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
    public async Task FromAsync_Action_Scheduler2()
    {
        var e = new ManualResetEvent(false);
        var t = default(int);

        var tcs = new TaskCompletionSource<int>();

        var xs = Seq.FromAsync(ct => (Task)tcs.Task, ImmediateScheduler);
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
