// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Subscribe</c> scenarios, from Rx.NET's <c>SubscribeTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 13 of the file's 24 tests. The three virtual-time enumerable subscriptions
/// (<c>SubscribeToEnumerable_*</c>) go through the kit's subscribe over an
/// <c>IEnumerable</c>, with <see cref="MockEnumerable{T}"/> recording the enumeration where
/// the original's records a subscription, and <c>ScheduledAt(200, n)</c> for the test
/// scheduler's tick per element; <c>SubscribeToEnumerable_Infinite</c> disposes from the ninth
/// element's handler, through an <see cref="ObserverRef{T}"/> over the testable observer, where
/// the original disposes at tick 210, since on the pump the enumeration runs within its tick.
/// <c>SubscribeToEnumerable_DefaultScheduler</c> compares the default scheduler with the explicit
/// one through <see cref="ObserverRef.Create{T}"/>, as the original does with
/// <c>Observer.Create</c>. The nine <c>Subscribe_*</c> tests subscribe through the kit's handler
/// forms; <c>Subscribe()</c> with no handlers, which AsyncRx.NET does not have, becomes the
/// element-handler form with an empty handler, and the error a handler-less subscription throws
/// is awaited from the subscription.
/// </para>
/// <para>
/// Not here: three argument-checking tests (the code-generated stratum), and the eight
/// <c>Subscribe_CT_*</c> tests over Rx.NET's <c>Subscribe(..., CancellationToken)</c>
/// overloads, which AsyncRx.NET does not have (a deferred work item); they run in
/// <c>Tests.System.Reactive.Shared.Rx</c> as the originals.
/// </para>
/// </remarks>
public abstract class SubscribeTests : SharedReactiveTest
{
    [TestMethod]
    public async Task SubscribeToEnumerable_Finite()
    {
        var scheduler = Scheduler;

        var results = scheduler.CreateObserver<int>();
        IAsyncDisposable? d = null;
        MockEnumerable<int>? xs = null;

        scheduler.ScheduleAbsolute(
            ReactiveTest.Created,
            () => xs = new MockEnumerable<int>(scheduler, Enumerable_Finite()));
        scheduler.ScheduleAbsolute(
            ReactiveTest.Subscribed,
            async () => d = await xs!.SubscribeAsync(results, scheduler));
        scheduler.ScheduleAbsolute(ReactiveTest.Disposed, async () => await d!.DisposeAsync());

        scheduler.Start();

        results.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), 1),
            OnNext(ScheduledAt(200, 2), 2),
            OnNext(ScheduledAt(200, 3), 3),
            OnNext(ScheduledAt(200, 4), 4),
            OnNext(ScheduledAt(200, 5), 5),
            OnCompleted<int>(ScheduledAt(200, 6))
        );

        xs!.Enumerations.AssertEqual(
            new Enumeration(200, ScheduledAt(200, 6))
        );
    }

    [TestMethod]
    public async Task SubscribeToEnumerable_Infinite()
    {
        var scheduler = Scheduler;

        var results = scheduler.CreateObserver<int>();
        IAsyncDisposable? d = null;
        MockEnumerable<int>? xs = null;

        // The original disposes at 210, after nine elements a tick apart; on the pump the
        // enumeration runs within its tick, so the ninth element's handler disposes instead.
        // Rx.NET's recursive loop still ends the enumeration a tick later, at 210, as it does
        // for the original, which is ScheduledAt(200, 10) on both targets.
        var n = 0;
        var observer = new ObserverRef<int>(
            async x =>
            {
                await results.OnNextAsync(x);

                if (++n == 9)
                {
                    await d!.DisposeAsync();
                }
            },
            results.OnErrorAsync,
            results.OnCompletedAsync);

        scheduler.ScheduleAbsolute(
            ReactiveTest.Created,
            () => xs = new MockEnumerable<int>(scheduler, Enumerable_Infinite()));
        scheduler.ScheduleAbsolute(
            ReactiveTest.Subscribed,
            async () => d = await xs!.SubscribeAsync(scheduler, observer, scheduler));

        scheduler.Start();

        results.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), 1),
            OnNext(ScheduledAt(200, 2), 1),
            OnNext(ScheduledAt(200, 3), 1),
            OnNext(ScheduledAt(200, 4), 1),
            OnNext(ScheduledAt(200, 5), 1),
            OnNext(ScheduledAt(200, 6), 1),
            OnNext(ScheduledAt(200, 7), 1),
            OnNext(ScheduledAt(200, 8), 1),
            OnNext(ScheduledAt(200, 9), 1)
        );

        xs!.Enumerations.AssertEqual(
            new Enumeration(200, ScheduledAt(200, 10))
        );
    }

    [TestMethod]
    public async Task SubscribeToEnumerable_Error()
    {
        var scheduler = Scheduler;

        var results = scheduler.CreateObserver<int>();
        IAsyncDisposable? d = null;
        MockEnumerable<int>? xs = null;
        var ex = new Exception();

        scheduler.ScheduleAbsolute(
            ReactiveTest.Created,
            () => xs = new MockEnumerable<int>(scheduler, Enumerable_Error(ex)));
        scheduler.ScheduleAbsolute(
            ReactiveTest.Subscribed,
            async () => d = await xs!.SubscribeAsync(results, scheduler));
        scheduler.ScheduleAbsolute(ReactiveTest.Disposed, async () => await d!.DisposeAsync());

        scheduler.Start();

        results.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), 1),
            OnNext(ScheduledAt(200, 2), 2),
            OnNext(ScheduledAt(200, 3), 3),
            OnError<int>(ScheduledAt(200, 4), ex)
        );

        xs!.Enumerations.AssertEqual(
            new Enumeration(200, ScheduledAt(200, 4))
        );
    }

    [TestMethod]
    public async Task SubscribeToEnumerable_DefaultScheduler()
    {
        for (var i = 0; i < 100; i++)
        {
            var results1 = new List<int>();
            var results2 = new List<int>();

            var s1 = new Semaphore(0, 1);
            var s2 = new Semaphore(0, 1);

            await Enumerable_Finite().SubscribeAsync(
                Scheduler,
                ObserverRef.Create<int>(
                    x => results1.Add(x),
                    ex => { throw ex; },
                    () => s1.Release()),
                null);
            await Enumerable_Finite().SubscribeAsync(
                Scheduler,
                ObserverRef.Create<int>(
                    x => results2.Add(x),
                    ex => { throw ex; },
                    () => s2.Release()),
                DefaultScheduler);

            s1.WaitOne();
            s2.WaitOne();

            Assert.IsTrue(results1.SequenceEqual(results2));
        }
    }

    private IEnumerable<int> Enumerable_Finite()
    {
        yield return 1;
        yield return 2;
        yield return 3;
        yield return 4;
        yield return 5;
        yield break;
    }

    private IEnumerable<int> Enumerable_Infinite()
    {
        while (true)
        {
            yield return 1;
        }
    }

    private IEnumerable<int> Enumerable_Error(Exception exception)
    {
        yield return 1;
        yield return 2;
        yield return 3;
        throw exception;
    }

    [TestMethod]
    public void Subscribe_None_Return()
    {
        Seq.Return(1, ImmediateScheduler).SubscribeAsync(Scheduler, _ => { });
    }

    [TestMethod]
    public async Task Subscribe_None_Throw()
    {
        var ex = new Exception();

        var xs = Seq.Throw<int>(ex, ImmediateScheduler);

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(
                async () => await xs.SubscribeAsync(Scheduler, _ => { })));
    }

    [TestMethod]
    public async Task Subscribe_None_Empty()
    {
        await Seq.Empty<int>(
            ImmediateScheduler).SubscribeAsync(Scheduler,
            (int _) => { Assert.Fail(); });
    }

    [TestMethod]
    public async Task Subscribe_OnNext_Return()
    {
        var _x = -1;
        await Seq.Return(42, ImmediateScheduler).SubscribeAsync(Scheduler, (int x) => { _x = x; });
        Assert.AreEqual(42, _x);
    }

    [TestMethod]
    public async Task Subscribe_OnNext_Throw()
    {
        var ex = new Exception();

        var xs = Seq.Throw<int>(ex, ImmediateScheduler);

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(
                async () => await xs.SubscribeAsync(Scheduler, _ => { Assert.Fail(); })));
    }

    [TestMethod]
    public async Task Subscribe_OnNext_Empty()
    {
        await Seq.Empty<int>(
            ImmediateScheduler).SubscribeAsync(Scheduler,
            (int _) => { Assert.Fail(); });
    }

    [TestMethod]
    public async Task Subscribe_OnNextOnCompleted_Return()
    {
        var finished = false;
        var _x = -1;
        await Seq.Return(
            42,
            ImmediateScheduler).SubscribeAsync(Scheduler,
            (int x) => { _x = x; },
            () => { finished = true; });
        Assert.AreEqual(42, _x);
        Assert.IsTrue(finished);
    }

    [TestMethod]
    public async Task Subscribe_OnNextOnCompleted_Throw()
    {
        var ex = new Exception();

        var xs = Seq.Throw<int>(ex, ImmediateScheduler);

        Assert.AreSame(
            ex,
            await Assert.ThrowsExactlyAsync<Exception>(
                async () => await xs.SubscribeAsync(
                    Scheduler,
                    _ => { Assert.Fail(); },
                    () => { Assert.Fail(); })));
    }

    [TestMethod]
    public async Task Subscribe_OnNextOnCompleted_Empty()
    {
        var finished = false;
        await Seq.Empty<int>(
            ImmediateScheduler).SubscribeAsync(Scheduler,
            (int _) => { Assert.Fail(); },
            () => { finished = true; });
        Assert.IsTrue(finished);
    }
}
