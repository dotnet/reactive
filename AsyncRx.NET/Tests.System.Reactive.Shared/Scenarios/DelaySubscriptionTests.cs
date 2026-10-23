// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>DelaySubscription</c> scenarios, from Rx.NET's <c>DelaySubscriptionTest.cs</c>.
/// </summary>
/// <remarks>
/// 6 of the file's 7 tests (<c>DelaySubscription_ArgumentChecking</c> is the code-generated
/// stratum), all transcribed as they are: a relative and an absolute due time on the test
/// scheduler, each over a completing and a failing cold source, and each on the default
/// scheduler in real time, where the original blocks in <c>ForEach</c> and the shared text
/// awaits <see cref="SharedReactiveTest.ForEachAsync{T}(Seq{T}, Action{T})"/>.
/// </remarks>
public abstract class DelaySubscriptionTests : SharedReactiveTest
{
    [TestMethod]
    public async Task DelaySubscription_TimeSpan_Default()
    {
        var lst = new List<int>();
        await ForEachAsync(
            Seq.Range(0, 10).DelaySubscription(TimeSpan.FromMilliseconds(1)),
            lst.Add);
        Assert.IsTrue(Enumerable.Range(0, 10).SequenceEqual(lst));
    }

    [TestMethod]
    public void DelaySubscription_TimeSpan_Simple()
    {
        var xs = Scheduler.CreateColdObservable(
            OnNext(50, 42),
            OnNext(60, 43),
            OnCompleted<int>(70)
        );

        var res = Scheduler.Start(() =>
            xs.DelaySubscription(TimeSpan.FromTicks(30), Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(280, 42),
            OnNext(290, 43),
            OnCompleted<int>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(230, 300)
        );
    }

    [TestMethod]
    public void DelaySubscription_TimeSpan_Error()
    {
        var ex = new Exception();
        var xs = Scheduler.CreateColdObservable(
            OnNext(50, 42),
            OnNext(60, 43),
            OnError<int>(70, ex)
        );

        var res = Scheduler.Start(() =>
            xs.DelaySubscription(TimeSpan.FromTicks(30), Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(280, 42),
            OnNext(290, 43),
            OnError<int>(300, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(230, 300)
        );
    }

    [TestMethod]
    public async Task DelaySubscription_DateTimeOffset_Default()
    {
        var lst = new List<int>();
        await ForEachAsync(
            Seq.Range(0, 10).DelaySubscription(DateTimeOffset.UtcNow.AddMilliseconds(1)),
            lst.Add);
        Assert.IsTrue(Enumerable.Range(0, 10).SequenceEqual(lst));
    }

    [TestMethod]
    public void DelaySubscription_DateTimeOffset_Simple()
    {
        var xs = Scheduler.CreateColdObservable(
            OnNext(50, 42),
            OnNext(60, 43),
            OnCompleted<int>(70)
        );

        var res = Scheduler.Start(() =>
            xs.DelaySubscription(new DateTimeOffset(230, TimeSpan.Zero), Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(280, 42),
            OnNext(290, 43),
            OnCompleted<int>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(230, 300)
        );
    }

    [TestMethod]
    public void DelaySubscription_DateTimeOffset_Error()
    {
        var ex = new Exception();
        var xs = Scheduler.CreateColdObservable(
            OnNext(50, 42),
            OnNext(60, 43),
            OnError<int>(70, ex)
        );

        var res = Scheduler.Start(() =>
            xs.DelaySubscription(new DateTimeOffset(230, TimeSpan.Zero), Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(280, 42),
            OnNext(290, 43),
            OnError<int>(300, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(230, 300)
        );
    }
}
