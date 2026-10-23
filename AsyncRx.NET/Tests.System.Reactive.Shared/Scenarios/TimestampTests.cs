// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>Timestamp</c> scenarios, from Rx.NET's <c>TimestampTest.cs</c>.
/// </summary>
/// <remarks>
/// 5 of the file's 6 tests (the <c>Timestamp_ArgumentChecking</c> test is the code-generated
/// stratum). Four transcribe as they are, the virtual-time tests over the test
/// scheduler, whose clock stamps each element with its tick.
/// <c>Timestamp_DefaultScheduler</c> counts the one element of
/// <c>Seq.Return(1).Timestamp()</c> through
/// <see cref="SharedReactiveTest.ToListAsync{T}"/> where the original blocks on
/// <c>First()</c>.
/// </remarks>
public abstract class TimestampTests : SharedReactiveTest
{
    [TestMethod]
    public void Timestamp_Regular()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(230, 3),
            OnNext(260, 4),
            OnNext(300, 5),
            OnNext(350, 6),
            OnCompleted<int>(400)
        );

        var res = scheduler.Start(() =>
            xs.Timestamp(scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(210, new Timestamped<int>(2, new DateTimeOffset(210, TimeSpan.Zero))),
            OnNext(230, new Timestamped<int>(3, new DateTimeOffset(230, TimeSpan.Zero))),
            OnNext(260, new Timestamped<int>(4, new DateTimeOffset(260, TimeSpan.Zero))),
            OnNext(300, new Timestamped<int>(5, new DateTimeOffset(300, TimeSpan.Zero))),
            OnNext(350, new Timestamped<int>(6, new DateTimeOffset(350, TimeSpan.Zero))),
            OnCompleted<Timestamped<int>>(400)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Timestamp_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(400)
        );

        var res = scheduler.Start(() =>
            xs.Timestamp(scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<Timestamped<int>>(400)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Timestamp_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(400, ex)
        );

        var res = scheduler.Start(() =>
            xs.Timestamp(scheduler)
        );

        res.Messages.AssertEqual(
            OnError<Timestamped<int>>(400, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Timestamp_Never()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            xs.Timestamp(scheduler)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public async Task Timestamp_DefaultScheduler()
    {
        Assert.IsTrue((await ToListAsync(Seq.Return(1).Timestamp().Count())).Single() == 1);
    }
}
