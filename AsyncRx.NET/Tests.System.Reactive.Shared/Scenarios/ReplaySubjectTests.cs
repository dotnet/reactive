// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Rx.NET's <c>ReplaySubjectTest.cs</c>, shared.</summary>
/// <remarks>
/// 29 of the file's 46 tests, over the <c>CreateReplaySubject</c> forms where the original writes
/// <c>new ReplaySubject&lt;int&gt;(...)</c>. The six whose subject delivers on the test scheduler
/// (<c>*_ReplayByTime</c>, <c>Infinite2</c>, <c>ReplaySubjectDiesOut</c>) spell their ticks with
/// <c>ScheduledAt</c>: Rx.NET's scheduled observer delivers each notification one virtual tick
/// after it was scheduled, one hop per replayed element, which the pump delivers within the tick.
/// The eight <c>*_to_late_subscriber_*</c> tests assert the whole recorded log where the originals
/// index into it. The three argument-checking tests are in each runner over its own subjects;
/// <c>ReplaySubject_Reentrant</c> pushes into the subject from its own handler, which
/// AsyncRx.NET's observers refuse by the maintainer's decision (Step 81), so it runs against
/// Rx.NET only; the six <c>FastImmediateObserver_*</c> tests are over an internal type of Rx.NET
/// and are not migrated; and the <c>SubjectDisposed</c> and <c>HasObservers_Dispose*</c> originals
/// dispose the subject itself and assert <c>IsDisposed</c> and <c>ObjectDisposedException</c>,
/// which AsyncRx.NET's subjects do not have (a deferred item, Step 82), so they run against Rx.NET
/// only.
/// </remarks>
public abstract class ReplaySubjectTests : SharedReactiveTest
{
    [TestMethod]
    public void Infinite_ReplayByTime()
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

        var subject = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(
            100, () => subject = CreateReplaySubject<int>(3, TimeSpan.FromTicks(100), Scheduler));
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
            OnNext(ScheduledAt(300), 3),
            OnNext(ScheduledAt(300, 2), 4),
            OnNext(ScheduledAt(340), 5),
            OnNext(ScheduledAt(410), 6),
            OnNext(ScheduledAt(520), 7)
        );

        results2.Messages.AssertEqual(
            OnNext(ScheduledAt(400), 5),
            OnNext(ScheduledAt(410), 6),
            OnNext(ScheduledAt(520), 7),
            OnNext(ScheduledAt(630), 8)
        );

        results3.Messages.AssertEqual(
            OnNext(ScheduledAt(900), 10),
            OnNext(ScheduledAt(940), 11)
        );
    }

    [TestMethod]
    public void Infinite_ReplayOne()
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

        var subject = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        var results4 = Scheduler.CreateObserver<int>();
        var subscription4 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateReplaySubject<int>(1));
        Scheduler.ScheduleAbsolute(
            200, async () => subscription = await xs.SubscribeAsync(Scheduler, subject!));
        Scheduler.ScheduleAbsolute(1200, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(
            300, async () => subscription1 = await subject!.SubscribeAsync(results1));
        Scheduler.ScheduleAbsolute(
            400, async () => subscription2 = await subject!.SubscribeAsync(results2));
        Scheduler.ScheduleAbsolute(
            900, async () => subscription3 = await subject!.SubscribeAsync(results3));
        Scheduler.ScheduleAbsolute(
            1100, async () => subscription4 = await subject!.SubscribeAsync(results4));

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

        results4.Messages.AssertEqual(
            OnNext(1100, 12)
        );
    }

    [TestMethod]
    public void Infinite_ReplayMany()
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

        var subject = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateReplaySubject<int>(3));
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
            OnNext(300, 3),
            OnNext(300, 4),
            OnNext(340, 5),
            OnNext(410, 6),
            OnNext(520, 7)
        );

        results2.Messages.AssertEqual(
            OnNext(400, 3),
            OnNext(400, 4),
            OnNext(400, 5),
            OnNext(410, 6),
            OnNext(520, 7),
            OnNext(630, 8)
        );

        results3.Messages.AssertEqual(
            OnNext(900, 8),
            OnNext(900, 9),
            OnNext(900, 10),
            OnNext(940, 11)
        );
    }

    [TestMethod]
    public void Infinite_ReplayAll()
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

        var subject = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateReplaySubject<int>());
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
            OnNext(300, 3),
            OnNext(300, 4),
            OnNext(340, 5),
            OnNext(410, 6),
            OnNext(520, 7)
        );

        results2.Messages.AssertEqual(
            OnNext(400, 3),
            OnNext(400, 4),
            OnNext(400, 5),
            OnNext(410, 6),
            OnNext(520, 7),
            OnNext(630, 8)
        );

        results3.Messages.AssertEqual(
            OnNext(900, 3),
            OnNext(900, 4),
            OnNext(900, 5),
            OnNext(900, 6),
            OnNext(900, 7),
            OnNext(900, 8),
            OnNext(900, 9),
            OnNext(900, 10),
            OnNext(940, 11)
        );
    }

    [TestMethod]
    public void Infinite2()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(70, 1),
            OnNext(110, 2),
            OnNext(220, 3),
            OnNext(270, 4),
            OnNext(280, -1),
            OnNext(290, -2),
            OnNext(340, 5),
            OnNext(410, 6),
            OnNext(520, 7),
            OnNext(630, 8),
            OnNext(710, 9),
            OnNext(870, 10),
            OnNext(940, 11),
            OnNext(1020, 12)
        );

        var subject = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(
            100, () => subject = CreateReplaySubject<int>(3, TimeSpan.FromTicks(100), Scheduler));
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
            OnNext(ScheduledAt(300), 4),
            OnNext(ScheduledAt(300, 2), -1),
            OnNext(ScheduledAt(300, 3), -2),
            OnNext(ScheduledAt(340), 5),
            OnNext(ScheduledAt(410), 6),
            OnNext(ScheduledAt(520), 7)
        );

        results2.Messages.AssertEqual(
            OnNext(ScheduledAt(400), 5),
            OnNext(ScheduledAt(410), 6),
            OnNext(ScheduledAt(520), 7),
            OnNext(ScheduledAt(630), 8)
        );

        results3.Messages.AssertEqual(
            OnNext(ScheduledAt(900), 10),
            OnNext(ScheduledAt(940), 11)
        );
    }

    [TestMethod]
    public void Finite_ReplayByTime()
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

        var subject = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(
            100, () => subject = CreateReplaySubject<int>(3, TimeSpan.FromTicks(100), Scheduler));
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
        Scheduler.ScheduleAbsolute(950, async () => await subscription3!.DisposeAsync());

        Scheduler.Start();

        results1.Messages.AssertEqual(
            OnNext(ScheduledAt(300), 3),
            OnNext(ScheduledAt(300, 2), 4),
            OnNext(ScheduledAt(340), 5),
            OnNext(ScheduledAt(410), 6),
            OnNext(ScheduledAt(520), 7)
        );

        results2.Messages.AssertEqual(
            OnNext(ScheduledAt(400), 5),
            OnNext(ScheduledAt(410), 6),
            OnNext(ScheduledAt(520), 7),
            OnCompleted<int>(ScheduledAt(630))
        );

        results3.Messages.AssertEqual(
            OnCompleted<int>(ScheduledAt(900))
        );
    }

    [TestMethod]
    public void Finite_ReplayOne()
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

        var subject = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateReplaySubject<int>(1));
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
            OnNext(900, 7),
            OnCompleted<int>(900)
        );
    }

    [TestMethod]
    public void Finite_ReplayMany()
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

        var subject = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateReplaySubject<int>(3));
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
        Scheduler.ScheduleAbsolute(950, async () => await subscription3!.DisposeAsync());

        Scheduler.Start();

        results1.Messages.AssertEqual(
            OnNext(300, 3),
            OnNext(300, 4),
            OnNext(340, 5),
            OnNext(410, 6),
            OnNext(520, 7)
        );

        results2.Messages.AssertEqual(
            OnNext(400, 3),
            OnNext(400, 4),
            OnNext(400, 5),
            OnNext(410, 6),
            OnNext(520, 7),
            OnCompleted<int>(630)
        );

        results3.Messages.AssertEqual(
            OnNext(900, 5),
            OnNext(900, 6),
            OnNext(900, 7),
            OnCompleted<int>(900)
        );
    }

    [TestMethod]
    public void Finite_ReplayAll()
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

        var subject = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateReplaySubject<int>());
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
        Scheduler.ScheduleAbsolute(950, async () => await subscription3!.DisposeAsync());

        Scheduler.Start();

        results1.Messages.AssertEqual(
            OnNext(300, 3),
            OnNext(300, 4),
            OnNext(340, 5),
            OnNext(410, 6),
            OnNext(520, 7)
        );

        results2.Messages.AssertEqual(
            OnNext(400, 3),
            OnNext(400, 4),
            OnNext(400, 5),
            OnNext(410, 6),
            OnNext(520, 7),
            OnCompleted<int>(630)
        );

        results3.Messages.AssertEqual(
            OnNext(900, 3),
            OnNext(900, 4),
            OnNext(900, 5),
            OnNext(900, 6),
            OnNext(900, 7),
            OnCompleted<int>(900)
        );
    }

    [TestMethod]
    public void Error_ReplayByTime()
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

        var subject = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(
            100, () => subject = CreateReplaySubject<int>(3, TimeSpan.FromTicks(100), Scheduler));
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
            OnNext(ScheduledAt(300), 3),
            OnNext(ScheduledAt(300, 2), 4),
            OnNext(ScheduledAt(340), 5),
            OnNext(ScheduledAt(410), 6),
            OnNext(ScheduledAt(520), 7)
        );

        results2.Messages.AssertEqual(
            OnNext(ScheduledAt(400), 5),
            OnNext(ScheduledAt(410), 6),
            OnNext(ScheduledAt(520), 7),
            OnError<int>(ScheduledAt(630), ex)
        );

        results3.Messages.AssertEqual(
            OnError<int>(ScheduledAt(900), ex)
        );
    }

    [TestMethod]
    public void Error_ReplayOne()
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

        var subject = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateReplaySubject<int>(1));
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
            OnNext(900, 7),
            OnError<int>(900, ex)
        );
    }

    [TestMethod]
    public void Error_ReplayMany()
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

        var subject = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateReplaySubject<int>(3));
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
        Scheduler.ScheduleAbsolute(950, async () => await subscription3!.DisposeAsync());

        Scheduler.Start();

        results1.Messages.AssertEqual(
            OnNext(300, 3),
            OnNext(300, 4),
            OnNext(340, 5),
            OnNext(410, 6),
            OnNext(520, 7)
        );

        results2.Messages.AssertEqual(
            OnNext(400, 3),
            OnNext(400, 4),
            OnNext(400, 5),
            OnNext(410, 6),
            OnNext(520, 7),
            OnError<int>(630, ex)
        );

        results3.Messages.AssertEqual(
            OnNext(900, 5),
            OnNext(900, 6),
            OnNext(900, 7),
            OnError<int>(900, ex)
        );
    }

    [TestMethod]
    public void Error_ReplayAll()
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

        var subject = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateReplaySubject<int>());
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
        Scheduler.ScheduleAbsolute(950, async () => await subscription3!.DisposeAsync());

        Scheduler.Start();

        results1.Messages.AssertEqual(
            OnNext(300, 3),
            OnNext(300, 4),
            OnNext(340, 5),
            OnNext(410, 6),
            OnNext(520, 7)
        );

        results2.Messages.AssertEqual(
            OnNext(400, 3),
            OnNext(400, 4),
            OnNext(400, 5),
            OnNext(410, 6),
            OnNext(520, 7),
            OnError<int>(630, ex)
        );

        results3.Messages.AssertEqual(
            OnNext(900, 3),
            OnNext(900, 4),
            OnNext(900, 5),
            OnNext(900, 6),
            OnNext(900, 7),
            OnError<int>(900, ex)
        );
    }

    [TestMethod]
    public void Canceled_ReplayByTime()
    {
        var xs = Scheduler.CreateHotObservable(
            OnCompleted<int>(630),
            OnNext(640, 9),
            OnCompleted<int>(650),
            OnError<int>(660, new Exception())
        );

        var subject = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(
            100, () => subject = CreateReplaySubject<int>(3, TimeSpan.FromTicks(100), Scheduler));
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
            OnCompleted<int>(ScheduledAt(630))
        );

        results3.Messages.AssertEqual(
            OnCompleted<int>(ScheduledAt(900))
        );
    }

    [TestMethod]
    public void Canceled_ReplayOne()
    {
        var xs = Scheduler.CreateHotObservable(
            OnCompleted<int>(630),
            OnNext(640, 9),
            OnCompleted<int>(650),
            OnError<int>(660, new Exception())
        );

        var subject = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateReplaySubject<int>(1));
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
    public void Canceled_ReplayMany()
    {
        var xs = Scheduler.CreateHotObservable(
            OnCompleted<int>(630),
            OnNext(640, 9),
            OnCompleted<int>(650),
            OnError<int>(660, new Exception())
        );

        var subject = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateReplaySubject<int>(3));
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
    public void Canceled_ReplayAll()
    {
        var xs = Scheduler.CreateHotObservable(
            OnCompleted<int>(630),
            OnNext(640, 9),
            OnCompleted<int>(650),
            OnError<int>(660, new Exception())
        );

        var subject = default(SubjectSeq<int>);
        var subscription = default(IAsyncDisposable);

        var results1 = Scheduler.CreateObserver<int>();
        var subscription1 = default(IAsyncDisposable);

        var results2 = Scheduler.CreateObserver<int>();
        var subscription2 = default(IAsyncDisposable);

        var results3 = Scheduler.CreateObserver<int>();
        var subscription3 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(100, () => subject = CreateReplaySubject<int>());
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
    public void ReplaySubjectDiesOut()
    {
        //
        // Tests v1.x behavior as documented in ReplaySubject.cs (Subscribe method).
        //

        var xs = Scheduler.CreateHotObservable(
            OnNext(70, 1),
            OnNext(110, 2),
            OnNext(220, 3),
            OnNext(270, 4),
            OnNext(340, 5),
            OnNext(410, 6),
            OnNext(520, 7),
            OnCompleted<int>(580)
        );

        var subject = default(SubjectSeq<int>);

        var results1 = Scheduler.CreateObserver<int>();
        var results2 = Scheduler.CreateObserver<int>();
        var results3 = Scheduler.CreateObserver<int>();
        var results4 = Scheduler.CreateObserver<int>();

        Scheduler.ScheduleAbsolute(
            100, () => subject = CreateReplaySubject<int>(int.MaxValue, TimeSpan.FromTicks(100), Scheduler));
        Scheduler.ScheduleAbsolute(
            200, async () => await xs.SubscribeAsync(Scheduler, subject!));

        Scheduler.ScheduleAbsolute(300, async () => await subject!.SubscribeAsync(results1));
        Scheduler.ScheduleAbsolute(400, async () => await subject!.SubscribeAsync(results2));
        Scheduler.ScheduleAbsolute(600, async () => await subject!.SubscribeAsync(results3));
        Scheduler.ScheduleAbsolute(900, async () => await subject!.SubscribeAsync(results4));

        Scheduler.Start();

        results1.Messages.AssertEqual(
            OnNext(ScheduledAt(300), 3),
            OnNext(ScheduledAt(300, 2), 4),
            OnNext(ScheduledAt(340), 5),
            OnNext(ScheduledAt(410), 6),
            OnNext(ScheduledAt(520), 7),
            OnCompleted<int>(ScheduledAt(580))
        );

        results2.Messages.AssertEqual(
            OnNext(ScheduledAt(400), 5),
            OnNext(ScheduledAt(410), 6),
            OnNext(ScheduledAt(520), 7),
            OnCompleted<int>(ScheduledAt(580))
        );

        results3.Messages.AssertEqual(
            OnNext(ScheduledAt(600), 7),
            OnCompleted<int>(ScheduledAt(600, 2))
        );

        results4.Messages.AssertEqual(
            OnCompleted<int>(ScheduledAt(900))
        );
    }

    [TestMethod]
    public async Task HasObservers()
    {
        await HasObserversImpl(CreateReplaySubject<int>());
        await HasObserversImpl(CreateReplaySubject<int>(1));
        await HasObserversImpl(CreateReplaySubject<int>(3));
        await HasObserversImpl(CreateReplaySubject<int>(TimeSpan.FromSeconds(1)));
    }


    private async Task HasObserversImpl(SubjectSeq<int> s)
    {
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
        await HasObservers_OnCompletedImpl(CreateReplaySubject<int>());
        await HasObservers_OnCompletedImpl(CreateReplaySubject<int>(1));
        await HasObservers_OnCompletedImpl(CreateReplaySubject<int>(3));
        await HasObservers_OnCompletedImpl(CreateReplaySubject<int>(TimeSpan.FromSeconds(1)));
    }


    private async Task HasObservers_OnCompletedImpl(SubjectSeq<int> s)
    {
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
        await HasObservers_OnErrorImpl(CreateReplaySubject<int>());
        await HasObservers_OnErrorImpl(CreateReplaySubject<int>(1));
        await HasObservers_OnErrorImpl(CreateReplaySubject<int>(3));
        await HasObservers_OnErrorImpl(CreateReplaySubject<int>(TimeSpan.FromSeconds(1)));
    }


    private async Task HasObservers_OnErrorImpl(SubjectSeq<int> s)
    {
        Assert.IsFalse(s.HasObservers);

        var d = await s.SubscribeAsync(Scheduler, _ => { }, ex => { });
        Assert.IsTrue(s.HasObservers);

        await s.OnNextAsync(42);
        Assert.IsTrue(s.HasObservers);

        await s.OnErrorAsync(new Exception());
        Assert.IsFalse(s.HasObservers);
    }

    [TestMethod]
    public async Task Completed_to_late_subscriber_ReplayAll()
    {
        var s = CreateReplaySubject<int>();
        await s.OnNextAsync(1);
        await s.OnNextAsync(2);
        await s.OnCompletedAsync();

        var observer = Scheduler.CreateObserver<int>();
        await s.SubscribeAsync(observer);

        observer.Messages.AssertEqual(
            OnNext(0, 1),
            OnNext(0, 2),
            OnCompleted<int>(0)
        );
    }

    [TestMethod]
    public async Task Completed_to_late_subscriber_ReplayOne()
    {
        var s = CreateReplaySubject<int>(1);
        await s.OnNextAsync(1);
        await s.OnNextAsync(2);
        await s.OnCompletedAsync();

        var observer = Scheduler.CreateObserver<int>();
        await s.SubscribeAsync(observer);

        observer.Messages.AssertEqual(
            OnNext(0, 2),
            OnCompleted<int>(0)
        );
    }

    [TestMethod]
    public async Task Completed_to_late_subscriber_ReplayMany()
    {
        var s = CreateReplaySubject<int>(2);
        await s.OnNextAsync(1);
        await s.OnNextAsync(2);
        await s.OnNextAsync(3);
        await s.OnCompletedAsync();

        var observer = Scheduler.CreateObserver<int>();
        await s.SubscribeAsync(observer);

        observer.Messages.AssertEqual(
            OnNext(0, 2),
            OnNext(0, 3),
            OnCompleted<int>(0)
        );
    }

    [TestMethod]
    public async Task Completed_to_late_subscriber_ReplayByTime()
    {
        var s = CreateReplaySubject<int>(TimeSpan.FromMinutes(1));
        await s.OnNextAsync(1);
        await s.OnNextAsync(2);
        await s.OnNextAsync(3);
        await s.OnCompletedAsync();

        var observer = Scheduler.CreateObserver<int>();
        await s.SubscribeAsync(observer);

        observer.Messages.AssertEqual(
            OnNext(0, 1),
            OnNext(0, 2),
            OnNext(0, 3),
            OnCompleted<int>(0)
        );
    }

    [TestMethod]
    public async Task Errored_to_late_subscriber_ReplayAll()
    {
        var expectedException = new Exception("Test");
        var s = CreateReplaySubject<int>();
        await s.OnNextAsync(1);
        await s.OnNextAsync(2);
        await s.OnErrorAsync(expectedException);

        var observer = Scheduler.CreateObserver<int>();
        await s.SubscribeAsync(observer);

        observer.Messages.AssertEqual(
            OnNext(0, 1),
            OnNext(0, 2),
            OnError<int>(0, expectedException)
        );
    }

    [TestMethod]
    public async Task Errored_to_late_subscriber_ReplayOne()
    {
        var expectedException = new Exception("Test");
        var s = CreateReplaySubject<int>(1);
        await s.OnNextAsync(1);
        await s.OnNextAsync(2);
        await s.OnErrorAsync(expectedException);

        var observer = Scheduler.CreateObserver<int>();
        await s.SubscribeAsync(observer);

        observer.Messages.AssertEqual(
            OnNext(0, 2),
            OnError<int>(0, expectedException)
        );
    }

    [TestMethod]
    public async Task Errored_to_late_subscriber_ReplayMany()
    {
        var expectedException = new Exception("Test");
        var s = CreateReplaySubject<int>(2);
        await s.OnNextAsync(1);
        await s.OnNextAsync(2);
        await s.OnNextAsync(3);
        await s.OnErrorAsync(expectedException);

        var observer = Scheduler.CreateObserver<int>();
        await s.SubscribeAsync(observer);

        observer.Messages.AssertEqual(
            OnNext(0, 2),
            OnNext(0, 3),
            OnError<int>(0, expectedException)
        );
    }

    [TestMethod]
    public async Task Errored_to_late_subscriber_ReplayByTime()
    {
        var expectedException = new Exception("Test");
        var s = CreateReplaySubject<int>(TimeSpan.FromMinutes(1));
        await s.OnNextAsync(1);
        await s.OnNextAsync(2);
        await s.OnNextAsync(3);
        await s.OnErrorAsync(expectedException);

        var observer = Scheduler.CreateObserver<int>();
        await s.SubscribeAsync(observer);

        observer.Messages.AssertEqual(
            OnNext(0, 1),
            OnNext(0, 2),
            OnNext(0, 3),
            OnError<int>(0, expectedException)
        );
    }
}
