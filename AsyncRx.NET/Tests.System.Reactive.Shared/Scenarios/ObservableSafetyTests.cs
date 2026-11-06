// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared subscription-safety scenarios, from Rx.NET's <c>ObservableSafetyTest.cs</c>.
/// </summary>
/// <remarks>
/// 2 of the file's 7 tests: a source whose subscribe call throws, under operators and under
/// <c>Merge</c>, delivers the exception as the error, and <c>Merge</c> releases the other source
/// at once. The original's <c>RogueObservable</c> is <c>Seq.ThrowOnSubscribe</c>, a raw observable
/// on each target that <c>SubscribeSafe</c> guards (AsyncRx.NET's <c>SubscribeSafeAsync</c> guards
/// a raw source the same way and leaves its own base class unguarded, as Rx.NET's does). Not here:
/// <c>SubscribeSafe_ArgumentChecking</c>, the code-generated stratum, and the four
/// <c>*_ObserverThrows</c> tests, which claim that a handler's exception thrown through the
/// observer comes back out to the producer and disposes the subscription; that is the synchronous
/// <c>Create</c> and producer behaviour the <c>Create</c> scenarios already keep Rx.NET-only (Step
/// 28), so they are the Rx.NET runner's.
/// </remarks>
public abstract class ObservableSafetyTests : SharedReactiveTest
{
    [TestMethod]
    public async Task Safety_Subscription1()
    {
        var ex = new Exception();

        var xs = Seq.ThrowOnSubscribe<int>(ex);

        var res = xs.Where(x => true).Select(x => x);

        var err = default(Exception);
        var d = await res.SubscribeAsync(
            Scheduler, x => Assert.Fail(), ex_ => err = ex_, () => Assert.Fail());

        Assert.AreSame(ex, err);

        await d.DisposeAsync();
    }

    [TestMethod]
    public void Safety_Subscription2()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 42),
            OnNext(220, 43),
            OnNext(230, 44),
            OnNext(240, 45),
            OnCompleted<int>(250)
        );

        var ys = Seq.ThrowOnSubscribe<int>(ex);

        var res = scheduler.Start(() =>
            xs.Merge(ys)
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 200)
        );
    }
}
