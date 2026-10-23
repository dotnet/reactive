// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>AsObservable</c> scenarios, from Rx.NET's <c>AsObservableTest.cs</c>.
/// </summary>
/// <remarks>
/// 7 of the file's 8 tests (<c>AsObservable_ArgumentChecking</c> is the code-generated
/// stratum), all transcribed as they are: the operator over hot, empty, failing and
/// never-ending sources, applied twice, and not subscribing until subscribed to. The two
/// identity claims compare what <see cref="SharedReactiveTest.Realize{T}(Seq{T})"/> returns
/// for the source and the result, the targets' own objects, since two descriptions are
/// always distinct. <c>AsObservable_IsNotEager</c>'s <c>Create</c> subscribes its hot source
/// through the kit's observer handle and returns the subscription, and realizes the
/// <c>AsObservable</c> it builds and discards, which is where the original's claim bites.
/// </remarks>
public abstract class AsObservableTests : SharedReactiveTest
{
    [TestMethod]
    public void AsObservable_AsObservable()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var ys = xs.AsObservable().AsObservable();

        Assert.AreNotSame(Realize(xs), Realize(ys));

        var res = Scheduler.Start(() =>
            ys
        );

        res.Messages.AssertEqual(
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void AsObservable_Hides()
    {
        var xs = Seq.Empty<int>();

        var res = xs.AsObservable();

        Assert.AreNotSame(Realize(xs), Realize(res));
    }

    [TestMethod]
    public void AsObservable_Never()
    {
        var xs = Seq.Never<int>();

        var res = Scheduler.Start(() =>
            xs.AsObservable()
        );

        res.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public void AsObservable_Empty()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.AsObservable()
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void AsObservable_Throw()
    {
        var ex = new Exception();
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(250, ex)
        );

        var res = Scheduler.Start(() =>
            xs.AsObservable()
        );

        res.Messages.AssertEqual(
            OnError<int>(250, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void AsObservable_Return()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.AsObservable()
        );

        res.Messages.AssertEqual(
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void AsObservable_IsNotEager()
    {
        var subscribed = false;
        var xs = Seq.Create<int>(async obs =>
        {
            subscribed = true;

            var disp = await Scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            ).SubscribeAsync(Scheduler, obs);

            return (IAsyncDisposable?)disp;
        });

        Realize(xs.AsObservable());
        Assert.IsFalse(subscribed);

        var res = Scheduler.Start(() =>
            xs.AsObservable()
        );
        Assert.IsTrue(subscribed);
    }
}
