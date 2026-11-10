// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>ForEachAsync</c> scenarios, from Rx.NET's <c>ForEachAsyncTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 12 of the file's 16 tests: <c>ForEachAsync_ArgumentChecking</c> is the code-generated
/// stratum, and the three <c>ForEachAsync_DisposeThrows*</c> tests are Rx.NET's own, in its
/// runner, since they document a race in its current-thread trampoline. The terminal is called
/// as <c>ForEachAsync(xs, onNext, token)</c>, the task it returns is the target's own, and the
/// scenarios inspect that task as the originals do: its status after the scheduler has run,
/// the error it faulted with, whether it was cancelled.
/// </para>
/// <para>
/// The two real-time <c>*_Cancel</c> scenarios build their source with <c>Seq.Create</c> over a
/// subscription to an infinite <c>Repeat</c> on the default scheduler, with an
/// <see cref="AnonymousAsyncDisposable"/> standing in for the original's
/// <c>CompositeDisposable</c> of the subscription and a flag-setting disposable;
/// <c>ForEachAsync_SubscribeThrows</c> throws from the subscribe function itself, as the
/// original does.
/// </para>
/// </remarks>
public abstract class ForEachAsyncTests : SharedReactiveTest
{
    [TestMethod]
    public void ForEachAsync_Never()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 4),
            OnNext(500, 5)
        );

        var task = default(Task);
        var cts = new CancellationTokenSource();
        var list = new List<Recorded<int>>();

        Scheduler.ScheduleAbsolute(150, () => task = ForEachAsync(xs, x => list.Add(new Recorded<int>(Scheduler.Clock, x)), cts.Token));

        Scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(150)
        );

        list.AssertEqual(
            new Recorded<int>(200, 2),
            new Recorded<int>(300, 3),
            new Recorded<int>(400, 4),
            new Recorded<int>(500, 5)
        );

        Assert.AreEqual(TaskStatus.WaitingForActivation, task!.Status);
    }

    [TestMethod]
    public void ForEachAsync_Completed()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 4),
            OnNext(500, 5),
            OnCompleted<int>(600)
        );

        var task = default(Task);
        var cts = new CancellationTokenSource();
        var list = new List<Recorded<int>>();

        Scheduler.ScheduleAbsolute(150, () => task = ForEachAsync(xs, x => list.Add(new Recorded<int>(Scheduler.Clock, x)), cts.Token));

        Scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(150, 600)
        );

        list.AssertEqual(
            new Recorded<int>(200, 2),
            new Recorded<int>(300, 3),
            new Recorded<int>(400, 4),
            new Recorded<int>(500, 5)
        );

        Assert.AreEqual(TaskStatus.RanToCompletion, task!.Status);
    }

    [TestMethod]
    public void ForEachAsync_Error()
    {
        var exception = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 4),
            OnNext(500, 5),
            OnError<int>(600, exception)
        );

        var task = default(Task);
        var cts = new CancellationTokenSource();
        var list = new List<Recorded<int>>();

        Scheduler.ScheduleAbsolute(150, () => task = ForEachAsync(xs, x => list.Add(new Recorded<int>(Scheduler.Clock, x)), cts.Token));

        Scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(150, 600)
        );

        list.AssertEqual(
            new Recorded<int>(200, 2),
            new Recorded<int>(300, 3),
            new Recorded<int>(400, 4),
            new Recorded<int>(500, 5)
        );

        Assert.AreEqual(TaskStatus.Faulted, task!.Status);
        Assert.AreSame(exception, task!.Exception!.InnerException);
    }

    [TestMethod]
    public void ForEachAsync_Throw()
    {
        var exception = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 4),
            OnNext(500, 5),
            OnCompleted<int>(600)
        );

        var task = default(Task);
        var cts = new CancellationTokenSource();
        var list = new List<Recorded<int>>();

        Scheduler.ScheduleAbsolute(150, () => task = ForEachAsync(xs, x =>
        {
            if (Scheduler.Clock > 400)
            {
                throw exception;
            }

            list.Add(new Recorded<int>(Scheduler.Clock, x));
        }, cts.Token));

        Scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(150, 500)
        );

        list.AssertEqual(
            new Recorded<int>(200, 2),
            new Recorded<int>(300, 3),
            new Recorded<int>(400, 4)
        );

        Assert.AreEqual(TaskStatus.Faulted, task!.Status);
        Assert.AreSame(exception, task!.Exception!.InnerException);
    }

    [TestMethod]
    public void ForEachAsync_CancelDuring()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 4),
            OnNext(500, 5),
            OnCompleted<int>(600)
        );

        var task = default(Task);
        var cts = new CancellationTokenSource();
        var list = new List<Recorded<int>>();

        Scheduler.ScheduleAbsolute(150, () => task = ForEachAsync(xs, x => list.Add(new Recorded<int>(Scheduler.Clock, x)), cts.Token));
        Scheduler.ScheduleAbsolute(350, () => cts.Cancel());

        Scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(150, 350)
        );

        list.AssertEqual(
            new Recorded<int>(200, 2),
            new Recorded<int>(300, 3)
        );

        Assert.AreEqual(TaskStatus.Canceled, task!.Status);
    }

    [TestMethod]
    public void ForEachAsync_CancelBefore()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 4),
            OnNext(500, 5),
            OnCompleted<int>(600)
        );

        var task = default(Task);
        var cts = new CancellationTokenSource();
        var list = new List<Recorded<int>>();

        cts.Cancel();

        Scheduler.ScheduleAbsolute(150, () => task = ForEachAsync(xs, x => list.Add(new Recorded<int>(Scheduler.Clock, x)), cts.Token));

        Scheduler.Start();

        xs.Subscriptions.AssertEqual(
        );

        list.AssertEqual(
        );

        Assert.AreEqual(TaskStatus.Canceled, task!.Status);
    }

    [TestMethod]
    public void ForEachAsync_CancelAfter()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 4),
            OnNext(500, 5),
            OnCompleted<int>(600)
        );

        var task = default(Task);
        var cts = new CancellationTokenSource();
        var list = new List<Recorded<int>>();

        Scheduler.ScheduleAbsolute(150, () => task = ForEachAsync(xs, x => list.Add(new Recorded<int>(Scheduler.Clock, x)), cts.Token));
        Scheduler.ScheduleAbsolute(700, () => cts.Cancel());

        Scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(150, 600)
        );

        list.AssertEqual(
            new Recorded<int>(200, 2),
            new Recorded<int>(300, 3),
            new Recorded<int>(400, 4),
            new Recorded<int>(500, 5)
        );

        Assert.AreEqual(TaskStatus.RanToCompletion, task!.Status);
    }

    [TestMethod]
    public void ForEachAsync_Default()
    {
        var list = new List<int>();
        ForEachAsync(Seq.Range(1, 10), list.Add).Wait();
        list.AssertEqual(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
    }

    [TestMethod]
    public void ForEachAsync_Index()
    {
        var list = new List<int>();
        ForEachAsync(Seq.Range(3, 5), (x, i) => list.Add(x * i)).Wait();
        list.AssertEqual(3 * 0, 4 * 1, 5 * 2, 6 * 3, 7 * 4);
    }

    [TestMethod]
    public void ForEachAsync_Default_Cancel()
    {
        var N = 10;

        for (var n = 0; n < N; n++)
        {
            var cts = new CancellationTokenSource();
            var done = false;

            var xs = Seq.Create<int>(async observer =>
            {
                var subscription = await Seq.Repeat(42, DefaultScheduler).SubscribeAsync(Scheduler, observer);

                return new AnonymousAsyncDisposable(async () =>
                {
                    await subscription.DisposeAsync();
                    done = true;
                });
            });

            var lst = new List<int>();

            var t = ForEachAsync(xs,
                x =>
                {
                    lock (lst)
                    {
                        lst.Add(x);
                    }
                },
                cts.Token
            );

            while (true)
            {
                lock (lst)
                {
                    if (lst.Count >= 10)
                    {
                        break;
                    }
                }
            }

            cts.Cancel();

            while (!t.IsCompleted)
            {
                ;
            }

            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(42, lst[i]);
            }

            Assert.IsTrue(done);
            Assert.IsTrue(t.IsCanceled);
        }
    }

    [TestMethod]
    public void ForEachAsync_Index_Cancel()
    {
        var N = 10;

        for (var n = 0; n < N; n++)
        {
            var cts = new CancellationTokenSource();
            var done = false;

            var xs = Seq.Create<int>(async observer =>
            {
                var subscription = await Seq.Repeat(42, DefaultScheduler).SubscribeAsync(Scheduler, observer);

                return new AnonymousAsyncDisposable(async () =>
                {
                    await subscription.DisposeAsync();
                    done = true;
                });
            });

            var lst = new List<int>();

            var t = ForEachAsync(xs,
                (x, i) =>
                {
                    lock (lst)
                    {
                        lst.Add(x * i);
                    }
                },
                cts.Token
            );

            while (true)
            {
                lock (lst)
                {
                    if (lst.Count >= 10)
                    {
                        break;
                    }
                }
            }

            cts.Cancel();

            while (!t.IsCompleted)
            {
                ;
            }

            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(i * 42, lst[i]);
            }

            Assert.IsTrue(done);
            Assert.IsTrue(t.IsCanceled);
        }
    }

    [TestMethod]
    public void ForEachAsync_SubscribeThrows()
    {
        var ex = new Exception();

        var x = 42;
        var xs = Seq.Create<int>(observer =>
        {
            if (x == 42)
            {
                throw ex;
            }

            return ValueTask.FromResult<IAsyncDisposable?>(EmptyAsyncDisposable.Instance);
        });

        var t = ForEachAsync(xs, _ => { });

        try
        {
            t.Wait();
            Assert.Fail();
        }
        catch (AggregateException err)
        {
            Assert.AreEqual(1, err.InnerExceptions.Count);
            Assert.AreSame(ex, err.InnerExceptions[0]);
        }
    }
}
