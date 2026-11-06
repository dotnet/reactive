// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Rx.NET's <c>SubjectTest.cs</c>, shared.</summary>
/// <remarks>
/// 11 of the file's 21 tests, over <see cref="SharedReactiveTest.CreateSubject{T}()"/> where the
/// original writes <c>new Subject&lt;int&gt;()</c>, with the subscriptions, disposals and pushes
/// awaited inside the scheduled actions; <c>Subject_Create1</c> and <c>Subject_Create2</c> build
/// theirs from an observer and a sequence through <see
/// cref="SharedReactiveTest.CreateSubject{T}(ObserverRef{T}, Seq{T})"/>, and leave their two
/// null-argument checks to the runners' argument-checking tests. Of the rest,
/// <c>Subject_Synchronize1</c> and <c>Subject_Synchronize2</c> push from ten threads through
/// <c>Subject.Synchronize</c>, which AsyncRx.NET does not have; the four argument-checking tests
/// are in each runner over its own subjects; and the <c>SubjectDisposed</c> and
/// <c>HasObservers_Dispose*</c> originals dispose the subject itself and assert <c>IsDisposed</c>
/// and <c>ObjectDisposedException</c>, which AsyncRx.NET's subjects do not have (a deferred item,
/// Step 82), so they run against Rx.NET only.
/// </remarks>
public abstract class SubjectTests : SharedReactiveTest
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

        var s = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => s = CreateSubject<int>());
        Scheduler.ScheduleAbsolute(
            200, async () => subscription = await xs.SubscribeAsync(Scheduler, s!));
        Scheduler.ScheduleAbsolute(1000, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(
            300, async () => subscription1 = await s!.SubscribeAsync(results1));
        Scheduler.ScheduleAbsolute(
            400, async () => subscription2 = await s!.SubscribeAsync(results2));
        Scheduler.ScheduleAbsolute(
            900, async () => subscription3 = await s!.SubscribeAsync(results3));

        Scheduler.ScheduleAbsolute(600, async () => await subscription1!.DisposeAsync());
        Scheduler.ScheduleAbsolute(700, async () => await subscription2!.DisposeAsync());
        Scheduler.ScheduleAbsolute(800, async () => await subscription1!.DisposeAsync());
        Scheduler.ScheduleAbsolute(950, async () => await subscription3!.DisposeAsync());

        Scheduler.Start();

        results1.Messages.AssertEqual(
            OnNext(340, 5),
            OnNext(410, 6),
            OnNext(520, 7)
        );

        results2.Messages.AssertEqual(
            OnNext(410, 6),
            OnNext(520, 7),
            OnNext(630, 8)
        );

        results3.Messages.AssertEqual(
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

        var s = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => s = CreateSubject<int>());
        Scheduler.ScheduleAbsolute(
            200, async () => subscription = await xs.SubscribeAsync(Scheduler, s!));
        Scheduler.ScheduleAbsolute(1000, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(
            300, async () => subscription1 = await s!.SubscribeAsync(results1));
        Scheduler.ScheduleAbsolute(
            400, async () => subscription2 = await s!.SubscribeAsync(results2));
        Scheduler.ScheduleAbsolute(
            900, async () => subscription3 = await s!.SubscribeAsync(results3));

        Scheduler.ScheduleAbsolute(600, async () => await subscription1!.DisposeAsync());
        Scheduler.ScheduleAbsolute(700, async () => await subscription2!.DisposeAsync());
        Scheduler.ScheduleAbsolute(800, async () => await subscription1!.DisposeAsync());
        Scheduler.ScheduleAbsolute(950, async () => await subscription3!.DisposeAsync());

        Scheduler.Start();

        results1.Messages.AssertEqual(
            OnNext(340, 5),
            OnNext(410, 6),
            OnNext(520, 7)
        );

        results2.Messages.AssertEqual(
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

        var s = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => s = CreateSubject<int>());
        Scheduler.ScheduleAbsolute(
            200, async () => subscription = await xs.SubscribeAsync(Scheduler, s!));
        Scheduler.ScheduleAbsolute(1000, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(
            300, async () => subscription1 = await s!.SubscribeAsync(results1));
        Scheduler.ScheduleAbsolute(
            400, async () => subscription2 = await s!.SubscribeAsync(results2));
        Scheduler.ScheduleAbsolute(
            900, async () => subscription3 = await s!.SubscribeAsync(results3));

        Scheduler.ScheduleAbsolute(600, async () => await subscription1!.DisposeAsync());
        Scheduler.ScheduleAbsolute(700, async () => await subscription2!.DisposeAsync());
        Scheduler.ScheduleAbsolute(800, async () => await subscription1!.DisposeAsync());
        Scheduler.ScheduleAbsolute(950, async () => await subscription3!.DisposeAsync());

        Scheduler.Start();

        results1.Messages.AssertEqual(
            OnNext(340, 5),
            OnNext(410, 6),
            OnNext(520, 7)
        );

        results2.Messages.AssertEqual(
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

        var s = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => s = CreateSubject<int>());
        Scheduler.ScheduleAbsolute(
            200, async () => subscription = await xs.SubscribeAsync(Scheduler, s!));
        Scheduler.ScheduleAbsolute(1000, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(
            300, async () => subscription1 = await s!.SubscribeAsync(results1));
        Scheduler.ScheduleAbsolute(
            400, async () => subscription2 = await s!.SubscribeAsync(results2));
        Scheduler.ScheduleAbsolute(
            900, async () => subscription3 = await s!.SubscribeAsync(results3));

        Scheduler.ScheduleAbsolute(600, async () => await subscription1!.DisposeAsync());
        Scheduler.ScheduleAbsolute(700, async () => await subscription2!.DisposeAsync());
        Scheduler.ScheduleAbsolute(800, async () => await subscription1!.DisposeAsync());
        Scheduler.ScheduleAbsolute(950, async () => await subscription3!.DisposeAsync());

        Scheduler.Start();

        results1.Messages.AssertEqual(
        );

        results2.Messages.AssertEqual(
            OnCompleted<int>(630)
        );

        results3.Messages.AssertEqual(
            OnCompleted<int>(900)
        );
    }

    [TestMethod]
    public void Dispose()
    {
        var s = CreateSubject<int>();

        Scheduler.ScheduleAbsolute(300, async () => await s.OnNextAsync(1));
        Scheduler.ScheduleAbsolute(998, async () => await s.OnNextAsync(2));
        Scheduler.ScheduleAbsolute(999, async () => await s.OnNextAsync(3));
        Scheduler.ScheduleAbsolute(1001, async () => await s.OnNextAsync(3));

        var results = Scheduler.Start(() => s);

        results.Messages.AssertEqual(
            OnNext(300, 1),
            OnNext(998, 2),
            OnNext(999, 3)
        );
    }

    [TestMethod]
    public void PreComplete()
    {
        var s = CreateSubject<int>();

        Scheduler.ScheduleAbsolute(90, async () => await s.OnCompletedAsync());

        var results = Scheduler.Start(() => s);

        results.Messages.AssertEqual(
            OnCompleted<int>(200)
        );
    }

    [TestMethod]
    public async Task Subject_Create1()
    {
        var _x = default(int);
        var _ex = default(Exception);
        var done = false;

        var v = ObserverRef.Create<int>(x => _x = x, ex => _ex = ex, () => done = true);
        var o = Seq.Return(42);

        var s = CreateSubject(v, o);

        await s.SubscribeAsync(Scheduler, x => _x = x);
        Assert.AreEqual(42, _x);

        await s.OnNextAsync(21);
        Assert.AreEqual(21, _x);

        var e = new Exception();
        await s.OnErrorAsync(e);
        Assert.AreSame(e, _ex);

        await s.OnCompletedAsync();
        Assert.IsFalse(done); // already cut off
    }

    [TestMethod]
    public async Task Subject_Create2()
    {
        var _x = default(int);
        var _ex = default(Exception);
        var done = false;

        var v = ObserverRef.Create<int>(x => _x = x, ex => _ex = ex, () => done = true);
        var o = Seq.Return(42);

        var s = CreateSubject(v, o);

        await s.SubscribeAsync(Scheduler, x => _x = x);
        Assert.AreEqual(42, _x);

        await s.OnNextAsync(21);
        Assert.AreEqual(21, _x);

        var e = new Exception();
        await s.OnErrorAsync(e);
        Assert.AreSame(e, _ex);

        await s.OnCompletedAsync();
        Assert.IsFalse(done); // already cut off
    }

    [TestMethod]
    public async Task HasObservers()
    {
        var s = CreateSubject<int>();
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
        var s = CreateSubject<int>();
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
        var s = CreateSubject<int>();
        Assert.IsFalse(s.HasObservers);

        var d = await s.SubscribeAsync(Scheduler, _ => { }, ex => { });
        Assert.IsTrue(s.HasObservers);

        await s.OnNextAsync(42);
        Assert.IsTrue(s.HasObservers);

        await s.OnErrorAsync(new Exception());
        Assert.IsFalse(s.HasObservers);
    }
}
