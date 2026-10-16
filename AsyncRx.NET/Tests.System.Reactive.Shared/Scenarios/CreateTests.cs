// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Create</c> scenarios, from Rx.NET's <c>CreateTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// The 14 behavioural tests of the two <c>Create</c> overloads (the callback returns an action,
/// or a disposable). Deliberately not here: the two <c>*_ArgumentChecking</c> tests (the
/// code-generated stratum), and the 14 <c>Iterate*</c> and <c>IteratorScenario*</c> tests, which
/// exercise <c>ObservableEx.Create</c>, the iterator-based form from
/// <c>System.Reactive.Experimental</c>, a surface AsyncRx.NET does not have.
/// </para>
/// <para>
/// This is the one operator whose scenario text drives an observer itself. The callback is
/// <c>async</c>, receives an <see cref="ObserverRef{T}"/> and awaits its <c>OnNextAsync</c>
/// where the original calls <c>OnNext</c>; it returns <c>() =&gt; { }</c>,
/// <c>default(Action)</c>, <see cref="EmptyAsyncDisposable.Instance"/>, a
/// <see cref="BooleanAsyncDisposable"/> or <c>default(IAsyncDisposable)</c> where the original
/// returns the synchronous equivalents, and schedules through <c>Scheduler.ScheduleRelative</c>
/// where the original calls <c>scheduler.Schedule(TimeSpan, ...)</c>. The six real-time tests
/// subscribe through the raw surface and await the subscription where the original's
/// <c>Subscribe</c> throws synchronously; the two <c>*_Exception</c> tests subscribe a single
/// element handler, as the original's bare <c>Subscribe()</c> does, so that the error stub
/// rethrows the callback's exception.
/// </para>
/// </remarks>
public abstract class CreateTests : SharedReactiveTest
{
    [TestMethod]
    public async Task Create_NullCoalescingAction()
    {
        var xs = Seq.Create<int>(async o =>
        {
            await o.OnNextAsync(42);
            return default(Action);
        });

        var lst = new List<int>();

        var d = await xs.SubscribeAsync(Scheduler, lst.Add);

        await d.DisposeAsync();

        Assert.IsTrue(lst.SequenceEqual([42]));
    }

    [TestMethod]
    public void Create_Next()
    {
        var res = Scheduler.Start(() =>
            Seq.Create<int>(async o =>
            {
                await o.OnNextAsync(1);
                await o.OnNextAsync(2);
                return () => { };
            })
        );

        res.Messages.AssertEqual(
            OnNext(200, 1),
            OnNext(200, 2)
        );
    }

    [TestMethod]
    public void Create_Completed()
    {
        var res = Scheduler.Start(() =>
            Seq.Create<int>(async o =>
            {
                await o.OnCompletedAsync();
                await o.OnNextAsync(100);
                await o.OnErrorAsync(new Exception());
                await o.OnCompletedAsync();
                return () => { };
            })
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(200)
        );
    }

    [TestMethod]
    public void Create_Error()
    {
        var ex = new Exception();

        var res = Scheduler.Start(() =>
            Seq.Create<int>(async o =>
            {
                await o.OnErrorAsync(ex);
                await o.OnNextAsync(100);
                await o.OnErrorAsync(new Exception());
                await o.OnCompletedAsync();
                return () => { };
            })
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex)
        );
    }

    [TestMethod]
    public async Task Create_Exception()
    {
        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await Seq.Create(new Func<ObserverRef<int>, ValueTask<Action?>>(o => throw new InvalidOperationException())).SubscribeAsync(Scheduler, x => { }));
    }

    [TestMethod]
    public void Create_Dispose()
    {
        var res = Scheduler.Start(() =>
            Seq.Create<int>(async o =>
            {
                var stopped = false;

                await o.OnNextAsync(1);
                await o.OnNextAsync(2);
                Scheduler.ScheduleRelative(600, async () =>
                {
                    if (!stopped)
                    {
                        await o.OnNextAsync(3);
                    }
                });
                Scheduler.ScheduleRelative(700, async () =>
                {
                    if (!stopped)
                    {
                        await o.OnNextAsync(4);
                    }
                });
                Scheduler.ScheduleRelative(900, async () =>
                {
                    if (!stopped)
                    {
                        await o.OnNextAsync(5);
                    }
                });
                Scheduler.ScheduleRelative(1100, async () =>
                {
                    if (!stopped)
                    {
                        await o.OnNextAsync(6);
                    }
                });

                return () => { stopped = true; };
            })
        );

        res.Messages.AssertEqual(
            OnNext(200, 1),
            OnNext(200, 2),
            OnNext(800, 3),
            OnNext(900, 4)
        );
    }

    [TestMethod]
    public async Task Create_ObserverThrows()
    {
        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await Seq.Create<int>(async o =>
            {
                await o.OnNextAsync(1);
                return () => { };
            }).SubscribeAsync(Scheduler, new Action<int>(x => throw new InvalidOperationException())));

        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await Seq.Create<int>(async o =>
            {
                await o.OnErrorAsync(new Exception());
                return () => { };
            }).SubscribeAsync(Scheduler, x => { }, ex => throw new InvalidOperationException()));

        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await Seq.Create<int>(async o =>
            {
                await o.OnCompletedAsync();
                return () => { };
            }).SubscribeAsync(Scheduler, x => { }, ex => { }, () => throw new InvalidOperationException()));
    }

    [TestMethod]
    public async Task CreateWithDisposable_NullCoalescingAction()
    {
        var xs = Seq.Create<int>(async o =>
        {
            await o.OnNextAsync(42);
            return default(IAsyncDisposable);
        });

        var lst = new List<int>();

        var d = await xs.SubscribeAsync(Scheduler, lst.Add);

        await d.DisposeAsync();

        Assert.IsTrue(lst.SequenceEqual([42]));
    }

    [TestMethod]
    public void CreateWithDisposable_Next()
    {
        var res = Scheduler.Start(() =>
            Seq.Create<int>(async o =>
            {
                await o.OnNextAsync(1);
                await o.OnNextAsync(2);
                return EmptyAsyncDisposable.Instance;
            })
        );

        res.Messages.AssertEqual(
            OnNext(200, 1),
            OnNext(200, 2)
        );
    }

    [TestMethod]
    public void CreateWithDisposable_Completed()
    {
        var res = Scheduler.Start(() =>
            Seq.Create<int>(async o =>
            {
                await o.OnCompletedAsync();
                await o.OnNextAsync(100);
                await o.OnErrorAsync(new Exception());
                await o.OnCompletedAsync();
                return EmptyAsyncDisposable.Instance;
            })
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(200)
        );
    }

    [TestMethod]
    public void CreateWithDisposable_Error()
    {
        var ex = new Exception();

        var res = Scheduler.Start(() =>
            Seq.Create<int>(async o =>
            {
                await o.OnErrorAsync(ex);
                await o.OnNextAsync(100);
                await o.OnErrorAsync(new Exception());
                await o.OnCompletedAsync();
                return EmptyAsyncDisposable.Instance;
            })
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex)
        );
    }

    [TestMethod]
    public async Task CreateWithDisposable_Exception()
    {
        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await Seq.Create(new Func<ObserverRef<int>, ValueTask<IAsyncDisposable?>>(o => throw new InvalidOperationException())).SubscribeAsync(Scheduler, x => { }));
    }

    [TestMethod]
    public void CreateWithDisposable_Dispose()
    {
        var res = Scheduler.Start(() =>
            Seq.Create<int>(async o =>
            {
                var d = new BooleanAsyncDisposable();

                await o.OnNextAsync(1);
                await o.OnNextAsync(2);
                Scheduler.ScheduleRelative(600, async () =>
                {
                    if (!d.IsDisposed)
                    {
                        await o.OnNextAsync(3);
                    }
                });
                Scheduler.ScheduleRelative(700, async () =>
                {
                    if (!d.IsDisposed)
                    {
                        await o.OnNextAsync(4);
                    }
                });
                Scheduler.ScheduleRelative(900, async () =>
                {
                    if (!d.IsDisposed)
                    {
                        await o.OnNextAsync(5);
                    }
                });
                Scheduler.ScheduleRelative(1100, async () =>
                {
                    if (!d.IsDisposed)
                    {
                        await o.OnNextAsync(6);
                    }
                });

                return d;
            })
        );

        res.Messages.AssertEqual(
            OnNext(200, 1),
            OnNext(200, 2),
            OnNext(800, 3),
            OnNext(900, 4)
        );
    }

    [TestMethod]
    public async Task CreateWithDisposable_ObserverThrows()
    {
        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await Seq.Create<int>(async o =>
            {
                await o.OnNextAsync(1);
                return EmptyAsyncDisposable.Instance;
            }).SubscribeAsync(Scheduler, new Action<int>(x => throw new InvalidOperationException())));

        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await Seq.Create<int>(async o =>
            {
                await o.OnErrorAsync(new Exception());
                return EmptyAsyncDisposable.Instance;
            }).SubscribeAsync(Scheduler, x => { }, ex => throw new InvalidOperationException()));

        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await Seq.Create<int>(async o =>
            {
                await o.OnCompletedAsync();
                return EmptyAsyncDisposable.Instance;
            }).SubscribeAsync(Scheduler, x => { }, ex => { }, () => throw new InvalidOperationException()));
    }
}
