// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Rx.NET's <c>BehaviorSubjectTest.cs</c>, shared.</summary>
/// <remarks>
/// 12 of the file's 19 tests, over <see cref="SharedReactiveTest.CreateBehaviorSubject{T}"/> where
/// the original writes <c>new BehaviorSubject&lt;int&gt;(value)</c>; the five <c>Value_*</c> tests
/// read the handle's <c>Value</c> and <c>TryGetValue</c>, which are the target's own. The two
/// argument-checking tests are in each runner over its own subject; <c>Value_ThrowsOnDispose</c>,
/// the <c>SubjectDisposed</c> and <c>HasObservers_Dispose*</c> originals dispose the subject
/// itself and assert <c>IsDisposed</c> and <c>ObjectDisposedException</c>, which AsyncRx.NET's
/// subjects do not have (a deferred item, Step 82), so they run against Rx.NET only.
/// </remarks>
public abstract class BehaviorSubjectTests : SharedReactiveTest
{
    [TestMethod]
    public void Infinite()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(70, 1),
            OnNext(110, 2),
            OnNext(220, 3),
            OnNext(270, 4),
            OnNext(340, 5),
            OnNext(410, 6),
            OnNext(520, 7),
            OnNext(630, 8),
            OnNext(710, 9),
            OnNext(870, 10),
            OnNext(940, 11),
            OnNext(1020, 12)
        );

        var subject = default(BehaviorSubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateBehaviorSubject(100));
        Scheduler.ScheduleAbsolute(
            200, async () => subscription = await xs.SubscribeAsync(Scheduler, subject!));
        Scheduler.ScheduleAbsolute(1000, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(
            300, async () => subscription1 = await subject!.SubscribeAsync(results1));
        Scheduler.ScheduleAbsolute(
            400, async () => subscription2 = await subject!.SubscribeAsync(results2));
        Scheduler.ScheduleAbsolute(
            900, async () => subscription3 = await subject!.SubscribeAsync(results3));

        Scheduler.ScheduleAbsolute(600, async () => await subscription1!.DisposeAsync());
        Scheduler.ScheduleAbsolute(700, async () => await subscription2!.DisposeAsync());
        Scheduler.ScheduleAbsolute(800, async () => await subscription1!.DisposeAsync());
        Scheduler.ScheduleAbsolute(950, async () => await subscription3!.DisposeAsync());

        Scheduler.Start();

        results1.Messages.AssertEqual(
            OnNext(300, 4),
            OnNext(340, 5),
            OnNext(410, 6),
            OnNext(520, 7)
        );

        results2.Messages.AssertEqual(
            OnNext(400, 5),
            OnNext(410, 6),
            OnNext(520, 7),
            OnNext(630, 8)
        );

        results3.Messages.AssertEqual(
            OnNext(900, 10),
            OnNext(940, 11)
        );
    }

    [TestMethod]
    public void Finite()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(70, 1),
            OnNext(110, 2),
            OnNext(220, 3),
            OnNext(270, 4),
            OnNext(340, 5),
            OnNext(410, 6),
            OnNext(520, 7),
            OnCompleted<int>(630),
            OnNext(640, 9),
            OnCompleted<int>(650),
            OnError<int>(660, new Exception())
        );

        var subject = default(BehaviorSubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateBehaviorSubject(100));
        Scheduler.ScheduleAbsolute(
            200, async () => subscription = await xs.SubscribeAsync(Scheduler, subject!));
        Scheduler.ScheduleAbsolute(1000, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(
            300, async () => subscription1 = await subject!.SubscribeAsync(results1));
        Scheduler.ScheduleAbsolute(
            400, async () => subscription2 = await subject!.SubscribeAsync(results2));
        Scheduler.ScheduleAbsolute(
            900, async () => subscription3 = await subject!.SubscribeAsync(results3));

        Scheduler.ScheduleAbsolute(600, async () => await subscription1!.DisposeAsync());
        Scheduler.ScheduleAbsolute(700, async () => await subscription2!.DisposeAsync());
        Scheduler.ScheduleAbsolute(800, async () => await subscription1!.DisposeAsync());
        Scheduler.ScheduleAbsolute(950, async () => await subscription3!.DisposeAsync());

        Scheduler.Start();

        results1.Messages.AssertEqual(
            OnNext(300, 4),
            OnNext(340, 5),
            OnNext(410, 6),
            OnNext(520, 7)
        );

        results2.Messages.AssertEqual(
            OnNext(400, 5),
            OnNext(410, 6),
            OnNext(520, 7),
            OnCompleted<int>(630)
        );

        results3.Messages.AssertEqual(
            OnCompleted<int>(900)
        );
    }

    [TestMethod]
    public void Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(70, 1),
            OnNext(110, 2),
            OnNext(220, 3),
            OnNext(270, 4),
            OnNext(340, 5),
            OnNext(410, 6),
            OnNext(520, 7),
            OnError<int>(630, ex),
            OnNext(640, 9),
            OnCompleted<int>(650),
            OnError<int>(660, new Exception())
        );

        var subject = default(BehaviorSubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateBehaviorSubject(100));
        Scheduler.ScheduleAbsolute(
            200, async () => subscription = await xs.SubscribeAsync(Scheduler, subject!));
        Scheduler.ScheduleAbsolute(1000, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(
            300, async () => subscription1 = await subject!.SubscribeAsync(results1));
        Scheduler.ScheduleAbsolute(
            400, async () => subscription2 = await subject!.SubscribeAsync(results2));
        Scheduler.ScheduleAbsolute(
            900, async () => subscription3 = await subject!.SubscribeAsync(results3));

        Scheduler.ScheduleAbsolute(600, async () => await subscription1!.DisposeAsync());
        Scheduler.ScheduleAbsolute(700, async () => await subscription2!.DisposeAsync());
        Scheduler.ScheduleAbsolute(800, async () => await subscription1!.DisposeAsync());
        Scheduler.ScheduleAbsolute(950, async () => await subscription3!.DisposeAsync());

        Scheduler.Start();

        results1.Messages.AssertEqual(
            OnNext(300, 4),
            OnNext(340, 5),
            OnNext(410, 6),
            OnNext(520, 7)
        );

        results2.Messages.AssertEqual(
            OnNext(400, 5),
            OnNext(410, 6),
            OnNext(520, 7),
            OnError<int>(630, ex)
        );

        results3.Messages.AssertEqual(
            OnError<int>(900, ex)
        );
    }

    [TestMethod]
    public void Canceled()
    {
        var xs = Scheduler.CreateHotObservable(
            OnCompleted<int>(630),
            OnNext(640, 9),
            OnCompleted<int>(650),
            OnError<int>(660, new Exception())
        );

        var subject = default(BehaviorSubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateBehaviorSubject(100));
        Scheduler.ScheduleAbsolute(
            200, async () => subscription = await xs.SubscribeAsync(Scheduler, subject!));
        Scheduler.ScheduleAbsolute(1000, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(
            300, async () => subscription1 = await subject!.SubscribeAsync(results1));
        Scheduler.ScheduleAbsolute(
            400, async () => subscription2 = await subject!.SubscribeAsync(results2));
        Scheduler.ScheduleAbsolute(
            900, async () => subscription3 = await subject!.SubscribeAsync(results3));

        Scheduler.ScheduleAbsolute(600, async () => await subscription1!.DisposeAsync());
        Scheduler.ScheduleAbsolute(700, async () => await subscription2!.DisposeAsync());
        Scheduler.ScheduleAbsolute(800, async () => await subscription1!.DisposeAsync());
        Scheduler.ScheduleAbsolute(950, async () => await subscription3!.DisposeAsync());

        Scheduler.Start();

        results1.Messages.AssertEqual(
            OnNext(300, 100)
        );

        results2.Messages.AssertEqual(
            OnNext(400, 100),
            OnCompleted<int>(630)
        );

        results3.Messages.AssertEqual(
            OnCompleted<int>(900)
        );
    }

    [TestMethod]
    public async Task HasObservers()
    {
        var s = CreateBehaviorSubject(42);
        Assert.IsFalse(s.HasObservers);

        var d1 = await s.SubscribeAsync(Scheduler, _ => { });
        Assert.IsTrue(s.HasObservers);

        await d1.DisposeAsync();
        Assert.IsFalse(s.HasObservers);

        var d2 = await s.SubscribeAsync(Scheduler, _ => { });
        Assert.IsTrue(s.HasObservers);

        var d3 = await s.SubscribeAsync(Scheduler, _ => { });
        Assert.IsTrue(s.HasObservers);

        await d2.DisposeAsync();
        Assert.IsTrue(s.HasObservers);

        await d3.DisposeAsync();
        Assert.IsFalse(s.HasObservers);
    }

    [TestMethod]
    public async Task HasObservers_OnCompleted()
    {
        var s = CreateBehaviorSubject(42);
        Assert.IsFalse(s.HasObservers);

        var d = await s.SubscribeAsync(Scheduler, _ => { });
        Assert.IsTrue(s.HasObservers);

        await s.OnNextAsync(42);
        Assert.IsTrue(s.HasObservers);

        await s.OnCompletedAsync();
        Assert.IsFalse(s.HasObservers);
    }

    [TestMethod]
    public async Task HasObservers_OnError()
    {
        var s = CreateBehaviorSubject(42);
        Assert.IsFalse(s.HasObservers);

        var d = await s.SubscribeAsync(Scheduler, _ => { }, ex => { });
        Assert.IsTrue(s.HasObservers);

        await s.OnNextAsync(42);
        Assert.IsTrue(s.HasObservers);

        await s.OnErrorAsync(new Exception());
        Assert.IsFalse(s.HasObservers);
    }

    [TestMethod]
    public async Task Value_Initial()
    {
        var s = CreateBehaviorSubject(42);
        Assert.AreEqual(42, s.Value);

        Assert.IsTrue(s.TryGetValue(out var x));
        Assert.AreEqual(42, x);
    }

    [TestMethod]
    public async Task Value_First()
    {
        var s = CreateBehaviorSubject(42);
        Assert.AreEqual(42, s.Value);

        Assert.IsTrue(s.TryGetValue(out var x));
        Assert.AreEqual(42, x);

        await s.OnNextAsync(43);
        Assert.AreEqual(43, s.Value);

        Assert.IsTrue(s.TryGetValue(out x));
        Assert.AreEqual(43, x);
    }

    [TestMethod]
    public async Task Value_Second()
    {
        var s = CreateBehaviorSubject(42);
        Assert.AreEqual(42, s.Value);

        Assert.IsTrue(s.TryGetValue(out var x));
        Assert.AreEqual(42, x);

        await s.OnNextAsync(43);
        Assert.AreEqual(43, s.Value);

        Assert.IsTrue(s.TryGetValue(out x));
        Assert.AreEqual(43, x);

        await s.OnNextAsync(44);
        Assert.AreEqual(44, s.Value);

        Assert.IsTrue(s.TryGetValue(out x));
        Assert.AreEqual(44, x);
    }

    [TestMethod]
    public async Task Value_FrozenAfterOnCompleted()
    {
        var s = CreateBehaviorSubject(42);
        Assert.AreEqual(42, s.Value);

        Assert.IsTrue(s.TryGetValue(out var x));
        Assert.AreEqual(42, x);

        await s.OnNextAsync(43);
        Assert.AreEqual(43, s.Value);

        Assert.IsTrue(s.TryGetValue(out x));
        Assert.AreEqual(43, x);

        await s.OnNextAsync(44);
        Assert.AreEqual(44, s.Value);

        Assert.IsTrue(s.TryGetValue(out x));
        Assert.AreEqual(44, x);

        await s.OnCompletedAsync();
        Assert.AreEqual(44, s.Value);

        Assert.IsTrue(s.TryGetValue(out x));
        Assert.AreEqual(44, x);

        await s.OnNextAsync(1234);
        Assert.AreEqual(44, s.Value);

        Assert.IsTrue(s.TryGetValue(out x));
        Assert.AreEqual(44, x);
    }

    [TestMethod]
    public async Task Value_ThrowsAfterOnError()
    {
        var s = CreateBehaviorSubject(42);
        Assert.AreEqual(42, s.Value);

        await s.OnErrorAsync(new InvalidOperationException());

        Assert.ThrowsExactly<InvalidOperationException>(() =>
        {
            var ignored = s.Value;
        });

        Assert.ThrowsExactly<InvalidOperationException>(() =>
        {
            s.TryGetValue(out var x);
        });
    }
}
