// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Rx.NET's <c>AsyncSubjectTest.cs</c>, shared.</summary>
/// <remarks>
/// 8 of the file's 19 tests, over <see cref="SharedReactiveTest.CreateAsyncSubject{T}"/> where the
/// original writes <c>new AsyncSubject&lt;int&gt;()</c>, plus two of this suite's own.
/// <c>Await_Blocking</c>, <c>Await_Throw</c>, <c>GetResult_Blocking</c> and
/// <c>GetResult_Blocking_Throw</c> block a thread in <c>GetResult</c> until another thread
/// terminates the subject, and <c>GetResult_Context</c> registers the awaiter's continuation under
/// a synchronization context from a pool thread; they stay Rx.NET-only as they stand, and what
/// they claim that holds of any awaiter, that an await registered before the subject terminates
/// yields its last value or throws its error once it does, is
/// <c>Await_YieldsTheLastValueOnCompletion</c> and <c>Await_ThrowsTheError</c> here, in the
/// awaitable shape. The two argument-checking tests are in each runner over its own subject, and
/// the <c>SubjectDisposed</c> and <c>HasObservers_Dispose*</c> originals dispose the subject
/// itself and assert <c>IsDisposed</c> and <c>ObjectDisposedException</c>, which AsyncRx.NET's
/// subjects do not have (a deferred item, Step 82), so they run against Rx.NET only.
/// </remarks>
public abstract class AsyncSubjectTests : SharedReactiveTest
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

        var subject = default(AsyncSubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateAsyncSubject<int>());
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
        );

        results2.Messages.AssertEqual(
        );

        results3.Messages.AssertEqual(
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

        var subject = default(AsyncSubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateAsyncSubject<int>());
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
        );

        results2.Messages.AssertEqual(
            OnNext(630, 7),
            OnCompleted<int>(630)
        );

        results3.Messages.AssertEqual(
            OnNext(900, 7),
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

        var subject = default(AsyncSubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateAsyncSubject<int>());
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
        );

        results2.Messages.AssertEqual(
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

        var subject = default(AsyncSubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateAsyncSubject<int>());
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
        );

        results2.Messages.AssertEqual(
            OnCompleted<int>(630)
        );

        results3.Messages.AssertEqual(
            OnCompleted<int>(900)
        );
    }

    [TestMethod]
    public async Task GetResult_Empty()
    {
        var s = CreateAsyncSubject<int>();
        await s.OnCompletedAsync();
        Assert.ThrowsExactly<InvalidOperationException>(() => s.GetResult());
    }

    [TestMethod]
    public async Task Await_YieldsTheLastValueOnCompletion()
    {
        var s = CreateAsyncSubject<int>();
        Assert.IsFalse(s.IsCompleted);

        var awaiting = AwaitAsync(s);
        Assert.IsFalse(awaiting.IsCompleted);

        await s.OnNextAsync(42);
        await s.OnCompletedAsync();

        Assert.AreEqual(42, await awaiting);
        Assert.IsTrue(s.IsCompleted);
    }

    [TestMethod]
    public async Task Await_ThrowsTheError()
    {
        var s = CreateAsyncSubject<int>();
        Assert.IsFalse(s.IsCompleted);

        var awaiting = AwaitAsync(s);
        Assert.IsFalse(awaiting.IsCompleted);

        var ex = new Exception();
        await s.OnErrorAsync(ex);

        var thrown = await Assert.ThrowsExactlyAsync<Exception>(async () => await awaiting);
        Assert.AreSame(ex, thrown);
        Assert.IsTrue(s.IsCompleted);
    }

    // Registers the await before the subject terminates, as the originals' blocked thread
    // registered its GetResult.
    private static async Task<int> AwaitAsync(AsyncSubjectSeq<int> subject) => await subject;

    [TestMethod]
    public async Task HasObservers()
    {
        var s = CreateAsyncSubject<int>();
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
        var s = CreateAsyncSubject<int>();
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
        var s = CreateAsyncSubject<int>();
        Assert.IsFalse(s.HasObservers);

        var d = await s.SubscribeAsync(Scheduler, _ => { }, ex => { });
        Assert.IsTrue(s.HasObservers);

        await s.OnNextAsync(42);
        Assert.IsTrue(s.HasObservers);

        await s.OnErrorAsync(new Exception());
        Assert.IsFalse(s.HasObservers);
    }
}
