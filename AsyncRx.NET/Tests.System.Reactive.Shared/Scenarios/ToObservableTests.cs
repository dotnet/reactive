// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>ToObservable</c> scenarios, from Rx.NET's <c>ToObservableTest.cs</c>.
/// </summary>
/// <remarks>
/// 8 of the file's 10 tests (the two argument-checking tests are the code-generated stratum):
/// an enumerable enumerated on the test scheduler to completion, to an iterator's failure, and
/// until disposed; compared across the default and an explicit scheduler; enumerated on the
/// shared <see cref="TestLongRunningScheduler"/> in the <c>Range</c> scenarios' shapes; and a
/// <see cref="RogueEnumerable{T}"/> whose <c>GetEnumerator</c> throws. Where Rx.NET writes
/// <c>ToObservable</c> the kit writes <c>ToSeq</c>, its name for the enumerable conversion
/// since the start. The scheduled ticks are <c>ScheduledAt</c> hops, as the <c>Range</c>
/// scenarios spell them, and the disposal test disposes from the second element's handler
/// where the original disposes at tick 203, since on the pump the whole enumeration runs within
/// tick 200. The long-running tests await the double's <c>Started</c> and <c>Stopped</c> where
/// the originals wait on its events, and the disposal one sets an event at the hundredth
/// element where the original spins.
/// </remarks>
public abstract class ToObservableTests : SharedReactiveTest
{
    [TestMethod]
    public void EnumerableToObservable_Complete()
    {
        var e = new MockEnumerable<int>(Scheduler,
            [3, 1, 2, 4]
        );

        var results = Scheduler.Start(() =>
            e.ToSeq(Scheduler)
        );

        results.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), 3),
            OnNext(ScheduledAt(200, 2), 1),
            OnNext(ScheduledAt(200, 3), 2),
            OnNext(ScheduledAt(200, 4), 4),
            OnCompleted<int>(ScheduledAt(200, 5))
        );

        e.Enumerations.AssertEqual(
            new Enumeration(200, ScheduledAt(200, 5))
        );
    }

    [TestMethod]
    public async Task EnumerableToObservable_Dispose()
    {
        var scheduler = Scheduler;

        IAsyncDisposable? d = null;

        var e = new MockEnumerable<int>(scheduler,
            [3, 1, 2, 4]
        );

        var results = scheduler.CreateObserver<int>();

        scheduler.ScheduleAbsolute(200, async () =>
            d = await e.ToSeq(scheduler).SubscribeAsync(
                scheduler,
                async x =>
                {
                    await results.OnNextAsync(x);

                    if (x == 1)
                    {
                        await d!.DisposeAsync();
                    }
                },
                results.OnErrorAsync,
                results.OnCompletedAsync));

        scheduler.Start();

        results.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), 3),
            OnNext(ScheduledAt(200, 2), 1)
        );

        e.Enumerations.AssertEqual(
            new Enumeration(200, ScheduledAt(200, 3))
        );
    }

    [TestMethod]
    public void EnumerableToObservable_Error()
    {
        var ex = new Exception();

        var e = new MockEnumerable<int>(Scheduler,
            EnumerableToObservable_Error_Core(ex)
        );

        var results = Scheduler.Start(() =>
            e.ToSeq(Scheduler)
        );

        results.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), 1),
            OnNext(ScheduledAt(200, 2), 2),
            OnError<int>(ScheduledAt(200, 3), ex)
        );

        e.Enumerations.AssertEqual(
            new Enumeration(200, ScheduledAt(200, 3))
        );
    }

    [TestMethod]
    public async Task EnumerableToObservable_Default()
    {
        var xs = new[] { 4, 3, 1, 5, 9, 2 };

        Assert.IsTrue(
            (await ToListAsync(xs.ToSeq()))
                .SequenceEqual(await ToListAsync(xs.ToSeq(DefaultScheduler))));
    }

    [TestMethod]
    public async Task EnumerableToObservable_LongRunning_Complete()
    {
        var scheduler = new TestLongRunningScheduler(Target);

        var e = new[] { 3, 1, 2, 4 };

        var results = e.ToSeq(scheduler);

        var lst = new List<int>();
        await results.SubscribeAsync(Scheduler, lst.Add);

        await scheduler.Started;
        await scheduler.Stopped;

        Assert.IsTrue(e.SequenceEqual(lst));
    }

    [TestMethod]
    public async Task EnumerableToObservable_LongRunning_Dispose()
    {
        var scheduler = new TestLongRunningScheduler(Target);

        var e = Enumerable.Range(0, int.MaxValue);

        var results = e.ToSeq(scheduler);

        var lst = new List<int>();
        var hundred = new ManualResetEvent(false);
        var d = await results.SubscribeAsync(
            Scheduler,
            x => { lst.Add(x); if (lst.Count == 100) { hundred.Set(); } });

        await scheduler.Started;

        hundred.WaitOne();

        await d.DisposeAsync();
        await scheduler.Stopped;

        Assert.IsTrue(e.Take(100).SequenceEqual(lst.Take(100)));
    }

    [TestMethod]
    public async Task EnumerableToObservable_LongRunning_Error()
    {
        var scheduler = new TestLongRunningScheduler(Target);

        var ex = new Exception();
        var e = EnumerableToObservable_Error_Core(ex);

        var results = e.ToSeq(scheduler);

        var lst = new List<int>();
        var err = default(Exception);
        await results.SubscribeAsync(Scheduler, lst.Add, ex_ => err = ex_);

        await scheduler.Started;
        await scheduler.Stopped;

        Assert.IsTrue(new[] { 1, 2 }.SequenceEqual(lst));
        Assert.AreSame(ex, err);
    }

    [TestMethod]
    public void EnumerableToObservable_GetEnumeratorThrows()
    {
        var ex = new Exception();
        var xs = new RogueEnumerable<int>(ex);

        var res = Scheduler.Start(() =>
            xs.ToSeq(Scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex)
        );
    }

    private static IEnumerable<int> EnumerableToObservable_Error_Core(Exception ex)
    {
        yield return 1;
        yield return 2;
        throw ex;
    }
}
