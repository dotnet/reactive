// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>SelectMany</c> scenarios, from Rx.NET's <c>SelectManyTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// The observable-selector families (<c>SelectMany_Then_*</c>, the plain and <c>WithIndex</c>
/// selector tests, and both <c>QueryOperator</c> families), the three-selector <c>Triple</c>
/// families, and the <c>Enumerable</c> families: 104 of the file's 181 tests. The task-returning
/// families remain, overloads AsyncRx.NET does not yet have; the nine <c>*_ArgumentChecking</c>
/// tests are the code-generated stratum. The <c>Enumerable</c> tests drive the shared
/// <see cref="MockEnumerable{T}"/> and <see cref="RogueEnumerable{T}"/>, and two throwing
/// enumerables declared below, as the originals are private to Rx.NET's test class. The
/// <c>Triple</c> tests build their inners from scheduled creation operators
/// (<c>Return(x, scheduler)</c>, <c>Empty(scheduler)</c>, <c>Range(1, 3, scheduler)</c>), whose
/// delivery the original expects one tick later, written here with <c>ScheduledAt</c>; those
/// that interleave several <c>Repeat(x, x, scheduler)</c> inners use the <see cref="Repeat"/>
/// helper below instead, for the reason it gives.
/// </para>
/// <para>
/// Where the original builds a hot sequence of cold sequences inline
/// (<c>OnNext(300, scheduler.CreateColdObservable(...))</c>) and then asserts on
/// <c>xs.Messages[2].Value.Value.Subscriptions</c>, the shared text hoists each inner into a
/// local (<c>ys1</c>, <c>ys2</c>, ...) and asserts on <c>ys3.Subscriptions</c>, as
/// <c>MergeTests</c> does, since a shared message log does not expose the target's recorded
/// values. The two <c>Index</c> tests project to anonymous types and use the vocabulary's
/// <c>OnCompleted(tick, witness)</c> as the original does.
/// </para>
/// </remarks>
public abstract class SelectManyTests : SharedReactiveTest
{
    private static T Throw<T>(Exception ex)
    {
        throw ex;
    }

    /// <summary>An enumerable whose enumerator throws from <c>Current</c>.</summary>
    /// <remarks>Rx.NET's, a private nested type of its <c>SelectManyTest</c>.</remarks>
    private sealed class CurrentThrowsEnumerable<T>(IEnumerable<T> e, Exception ex)
        : IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator() => new Enumerator(e.GetEnumerator(), ex);

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        private sealed class Enumerator(IEnumerator<T> e, Exception ex) : IEnumerator<T>
        {
            public T Current => throw ex;

            object? IEnumerator.Current => Current;

            public void Dispose() => e.Dispose();

            public bool MoveNext() => e.MoveNext();

            public void Reset() => e.Reset();
        }
    }

    /// <summary>An enumerable whose enumerator throws from <c>MoveNext</c>.</summary>
    /// <remarks>Rx.NET's, a private nested type of its <c>SelectManyTest</c>.</remarks>
    private sealed class MoveNextThrowsEnumerable<T>(IEnumerable<T> e, Exception ex)
        : IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator() => new Enumerator(e.GetEnumerator(), ex);

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        private sealed class Enumerator(IEnumerator<T> e, Exception ex) : IEnumerator<T>
        {
            public T Current => e.Current;

            object? IEnumerator.Current => Current;

            public void Dispose() => e.Dispose();

            public bool MoveNext() => throw ex;

            public void Reset() => e.Reset();
        }
    }

    /// <summary>
    /// A sequence with the timing of Rx.NET's <c>Observable.Repeat(value, count, scheduler)</c>
    /// on both targets' test schedulers.
    /// </summary>
    /// <remarks>
    /// Rx.NET's scheduled <c>Repeat</c> schedules each value only when the previous one has been
    /// delivered, and completes in the action that delivers the last (or in its first action,
    /// for a count of zero); on <c>TestScheduler</c>, which moves one tick per scheduling, that
    /// is one tick per value, and the <c>Triple</c> tests interleave several such inners by
    /// those ticks and by the order their actions were scheduled. AsyncRx.NET's <c>Repeat</c>
    /// loops within one scheduled action, so on the pump every value lands at the tick of
    /// subscription and the interleaving differs, which <c>ScheduledAt</c> cannot express. This
    /// sequence schedules each value one tick after the previous through the scenario's
    /// scheduler, which both test schedulers order the same way, so those tests keep every
    /// expectation of the original.
    /// </remarks>
    private Seq<int> Repeat(int value, int count) =>
        Seq.Create<int>(observer =>
        {
            var disposed = false;
            var remaining = count;

            void ScheduleNext() =>
                Scheduler.ScheduleRelative(1, async () =>
                {
                    if (disposed)
                    {
                        return;
                    }

                    if (remaining > 0)
                    {
                        await observer.OnNextAsync(value);
                        remaining--;
                    }

                    if (remaining == 0)
                    {
                        await observer.OnCompletedAsync();
                    }
                    else
                    {
                        ScheduleNext();
                    }
                });

            ScheduleNext();
            return new ValueTask<Action?>(() => disposed = true);
        });

    [TestMethod]
    public void SelectMany_Then_Complete_Complete()
    {
        var xs = Scheduler.CreateColdObservable(
            OnNext(100, 4),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 1),
            OnCompleted<int>(500)
        );

        var ys = Scheduler.CreateColdObservable(
            OnNext(50, "foo"),
            OnNext(100, "bar"),
            OnNext(150, "baz"),
            OnNext(200, "qux"),
            OnCompleted<string>(250)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(ys)
        );

        res.Messages.AssertEqual(
            OnNext(350, "foo"),
            OnNext(400, "bar"),
            OnNext(450, "baz"),
            OnNext(450, "foo"),
            OnNext(500, "qux"),
            OnNext(500, "bar"),
            OnNext(550, "baz"),
            OnNext(550, "foo"),
            OnNext(600, "qux"),
            OnNext(600, "bar"),
            OnNext(650, "baz"),
            OnNext(650, "foo"),
            OnNext(700, "qux"),
            OnNext(700, "bar"),
            OnNext(750, "baz"),
            OnNext(800, "qux"),
            OnCompleted<string>(850)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 700)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(300, 550),
            Subscribe(400, 650),
            Subscribe(500, 750),
            Subscribe(600, 850)
        );
    }

    [TestMethod]
    public void SelectMany_Then_Complete_Complete_2()
    {
        var xs = Scheduler.CreateColdObservable(
            OnNext(100, 4),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 1),
            OnCompleted<int>(700)
        );

        var ys = Scheduler.CreateColdObservable(
            OnNext(50, "foo"),
            OnNext(100, "bar"),
            OnNext(150, "baz"),
            OnNext(200, "qux"),
            OnCompleted<string>(250)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(ys)
        );

        res.Messages.AssertEqual(
            OnNext(350, "foo"),
            OnNext(400, "bar"),
            OnNext(450, "baz"),
            OnNext(450, "foo"),
            OnNext(500, "qux"),
            OnNext(500, "bar"),
            OnNext(550, "baz"),
            OnNext(550, "foo"),
            OnNext(600, "qux"),
            OnNext(600, "bar"),
            OnNext(650, "baz"),
            OnNext(650, "foo"),
            OnNext(700, "qux"),
            OnNext(700, "bar"),
            OnNext(750, "baz"),
            OnNext(800, "qux"),
            OnCompleted<string>(900)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 900)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(300, 550),
            Subscribe(400, 650),
            Subscribe(500, 750),
            Subscribe(600, 850)
        );
    }

    [TestMethod]
    public void SelectMany_Then_Never_Complete()
    {
        var xs = Scheduler.CreateColdObservable(
            OnNext(100, 4),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 1),
            OnNext(500, 5),
            OnNext(700, 0)
        );

        var ys = Scheduler.CreateColdObservable(
            OnNext(50, "foo"),
            OnNext(100, "bar"),
            OnNext(150, "baz"),
            OnNext(200, "qux"),
            OnCompleted<string>(250)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(ys)
        );

        res.Messages.AssertEqual(
            OnNext(350, "foo"),
            OnNext(400, "bar"),
            OnNext(450, "baz"),
            OnNext(450, "foo"),
            OnNext(500, "qux"),
            OnNext(500, "bar"),
            OnNext(550, "baz"),
            OnNext(550, "foo"),
            OnNext(600, "qux"),
            OnNext(600, "bar"),
            OnNext(650, "baz"),
            OnNext(650, "foo"),
            OnNext(700, "qux"),
            OnNext(700, "bar"),
            OnNext(750, "baz"),
            OnNext(750, "foo"),
            OnNext(800, "qux"),
            OnNext(800, "bar"),
            OnNext(850, "baz"),
            OnNext(900, "qux"),
            OnNext(950, "foo")
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(300, 550),
            Subscribe(400, 650),
            Subscribe(500, 750),
            Subscribe(600, 850),
            Subscribe(700, 950),
            Subscribe(900, 1000)
        );
    }

    [TestMethod]
    public void SelectMany_Then_Complete_Never()
    {
        var xs = Scheduler.CreateColdObservable(
            OnNext(100, 4),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 1),
            OnCompleted<int>(500)
        );

        var ys = Scheduler.CreateColdObservable(
            OnNext(50, "foo"),
            OnNext(100, "bar"),
            OnNext(150, "baz"),
            OnNext(200, "qux")
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(ys)
        );

        res.Messages.AssertEqual(
            OnNext(350, "foo"),
            OnNext(400, "bar"),
            OnNext(450, "baz"),
            OnNext(450, "foo"),
            OnNext(500, "qux"),
            OnNext(500, "bar"),
            OnNext(550, "baz"),
            OnNext(550, "foo"),
            OnNext(600, "qux"),
            OnNext(600, "bar"),
            OnNext(650, "baz"),
            OnNext(650, "foo"),
            OnNext(700, "qux"),
            OnNext(700, "bar"),
            OnNext(750, "baz"),
            OnNext(800, "qux")
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 700)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(300, 1000),
            Subscribe(400, 1000),
            Subscribe(500, 1000),
            Subscribe(600, 1000)
        );
    }

    [TestMethod]
    public void SelectMany_Then_Complete_Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateColdObservable(
            OnNext(100, 4),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 1),
            OnCompleted<int>(500)
        );

        var ys = Scheduler.CreateColdObservable(
            OnNext(50, "foo"),
            OnNext(100, "bar"),
            OnNext(150, "baz"),
            OnNext(200, "qux"),
            OnError<string>(300, ex)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(ys)
        );

        res.Messages.AssertEqual(
            OnNext(350, "foo"),
            OnNext(400, "bar"),
            OnNext(450, "baz"),
            OnNext(450, "foo"),
            OnNext(500, "qux"),
            OnNext(500, "bar"),
            OnNext(550, "baz"),
            OnNext(550, "foo"),
            OnError<string>(600, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(300, 600),
            Subscribe(400, 600),
            Subscribe(500, 600),
            Subscribe(600, 600)
        );
    }

    [TestMethod]
    public void SelectMany_Then_Error_Complete()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateColdObservable(
            OnNext(100, 4),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 1),
            OnError<int>(500, ex)
        );

        var ys = Scheduler.CreateColdObservable(
            OnNext(50, "foo"),
            OnNext(100, "bar"),
            OnNext(150, "baz"),
            OnNext(200, "qux"),
            OnCompleted<string>(250)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(ys)
        );

        res.Messages.AssertEqual(
            OnNext(350, "foo"),
            OnNext(400, "bar"),
            OnNext(450, "baz"),
            OnNext(450, "foo"),
            OnNext(500, "qux"),
            OnNext(500, "bar"),
            OnNext(550, "baz"),
            OnNext(550, "foo"),
            OnNext(600, "qux"),
            OnNext(600, "bar"),
            OnNext(650, "baz"),
            OnNext(650, "foo"),
            OnError<string>(700, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 700)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(300, 550),
            Subscribe(400, 650),
            Subscribe(500, 700),
            Subscribe(600, 700)
        );
    }

    [TestMethod]
    public void SelectMany_Then_Error_Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateColdObservable(
            OnNext(100, 4),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 1),
            OnError<int>(500, new Exception())
        );

        var ys = Scheduler.CreateColdObservable(
            OnNext(50, "foo"),
            OnNext(100, "bar"),
            OnNext(150, "baz"),
            OnNext(200, "qux"),
            OnError<string>(250, ex)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(ys)
        );

        res.Messages.AssertEqual(
            OnNext(350, "foo"),
            OnNext(400, "bar"),
            OnNext(450, "baz"),
            OnNext(450, "foo"),
            OnNext(500, "qux"),
            OnNext(500, "bar"),
            OnError<string>(550, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(300, 550),
            Subscribe(400, 550),
            Subscribe(500, 550)
        );
    }

    [TestMethod]
    public void SelectMany_Complete()
    {
        var ys1 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 102),
            OnNext(90, 103),
            OnNext(110, 104),
            OnNext(190, 105),
            OnNext(440, 106),
            OnCompleted<int>(460)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(180, 202),
            OnNext(190, 203),
            OnCompleted<int>(205)
        );

        var ys5 = Scheduler.CreateColdObservable(
            OnNext(10, 301),
            OnNext(50, 302),
            OnNext(70, 303),
            OnNext(260, 304),
            OnNext(310, 305),
            OnCompleted<int>(410)
        );

        var ys6 = Scheduler.CreateColdObservable(
            OnCompleted<int>(40)
        );

        var ys7 = Scheduler.CreateColdObservable(
            OnNext(80, 401),
            OnNext(90, 402),
            OnCompleted<int>(100)
        );

        var xs = Scheduler.CreateHotObservable(
                OnNext<Seq<int>>(5, ys1),
                OnNext<Seq<int>>(105, ys2),
                OnNext<Seq<int>>(300, ys3),
                OnNext<Seq<int>>(400, ys4),
                OnNext<Seq<int>>(550, ys5),
                OnNext<Seq<int>>(750, ys6),
                OnNext<Seq<int>>(850, ys7),
                OnCompleted<Seq<int>>(900)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(x => x)
        );

        res.Messages.AssertEqual(
            OnNext(310, 102),
            OnNext(390, 103),
            OnNext(410, 104),
            OnNext(490, 105),
            OnNext(560, 301),
            OnNext(580, 202),
            OnNext(590, 203),
            OnNext(600, 302),
            OnNext(620, 303),
            OnNext(740, 106),
            OnNext(810, 304),
            OnNext(860, 305),
            OnNext(930, 401),
            OnNext(940, 402),
            OnCompleted<int>(960)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 900));

        ys3.Subscriptions.AssertEqual(
            Subscribe(300, 760));

        ys4.Subscriptions.AssertEqual(
            Subscribe(400, 605));

        ys5.Subscriptions.AssertEqual(
            Subscribe(550, 960));

        ys6.Subscriptions.AssertEqual(
            Subscribe(750, 790));

        ys7.Subscriptions.AssertEqual(
            Subscribe(850, 950));
    }

    [TestMethod]
    public void SelectMany_Complete_InnerNotComplete()
    {
        var ys1 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 102),
            OnNext(90, 103),
            OnNext(110, 104),
            OnNext(190, 105),
            OnNext(440, 106),
            OnCompleted<int>(460)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(180, 202),
            OnNext(190, 203)
        );

        var ys5 = Scheduler.CreateColdObservable(
            OnNext(10, 301),
            OnNext(50, 302),
            OnNext(70, 303),
            OnNext(260, 304),
            OnNext(310, 305),
            OnCompleted<int>(410)
        );

        var ys6 = Scheduler.CreateColdObservable(
            OnCompleted<int>(40)
        );

        var ys7 = Scheduler.CreateColdObservable(
            OnNext(80, 401),
            OnNext(90, 402),
            OnCompleted<int>(100)
        );

        var xs = Scheduler.CreateHotObservable(
                OnNext<Seq<int>>(5, ys1),
                OnNext<Seq<int>>(105, ys2),
                OnNext<Seq<int>>(300, ys3),
                OnNext<Seq<int>>(400, ys4),
                OnNext<Seq<int>>(550, ys5),
                OnNext<Seq<int>>(750, ys6),
                OnNext<Seq<int>>(850, ys7),
                OnCompleted<Seq<int>>(900)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(x => x)
        );

        res.Messages.AssertEqual(
            OnNext(310, 102),
            OnNext(390, 103),
            OnNext(410, 104),
            OnNext(490, 105),
            OnNext(560, 301),
            OnNext(580, 202),
            OnNext(590, 203),
            OnNext(600, 302),
            OnNext(620, 303),
            OnNext(740, 106),
            OnNext(810, 304),
            OnNext(860, 305),
            OnNext(930, 401),
            OnNext(940, 402)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 900));

        ys3.Subscriptions.AssertEqual(
            Subscribe(300, 760));

        ys4.Subscriptions.AssertEqual(
            Subscribe(400, 1000));

        ys5.Subscriptions.AssertEqual(
            Subscribe(550, 960));

        ys6.Subscriptions.AssertEqual(
            Subscribe(750, 790));

        ys7.Subscriptions.AssertEqual(
            Subscribe(850, 950));
    }

    [TestMethod]
    public void SelectMany_Complete_OuterNotComplete()
    {
        var ys1 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 102),
            OnNext(90, 103),
            OnNext(110, 104),
            OnNext(190, 105),
            OnNext(440, 106),
            OnCompleted<int>(460)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(180, 202),
            OnNext(190, 203),
            OnCompleted<int>(205)
        );

        var ys5 = Scheduler.CreateColdObservable(
            OnNext(10, 301),
            OnNext(50, 302),
            OnNext(70, 303),
            OnNext(260, 304),
            OnNext(310, 305),
            OnCompleted<int>(410)
        );

        var ys6 = Scheduler.CreateColdObservable(
            OnCompleted<int>(40)
        );

        var ys7 = Scheduler.CreateColdObservable(
            OnNext(80, 401),
            OnNext(90, 402),
            OnCompleted<int>(100)
        );

        var xs = Scheduler.CreateHotObservable(
                OnNext<Seq<int>>(5, ys1),
                OnNext<Seq<int>>(105, ys2),
                OnNext<Seq<int>>(300, ys3),
                OnNext<Seq<int>>(400, ys4),
                OnNext<Seq<int>>(550, ys5),
                OnNext<Seq<int>>(750, ys6),
                OnNext<Seq<int>>(850, ys7)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(x => x)
        );

        res.Messages.AssertEqual(
            OnNext(310, 102),
            OnNext(390, 103),
            OnNext(410, 104),
            OnNext(490, 105),
            OnNext(560, 301),
            OnNext(580, 202),
            OnNext(590, 203),
            OnNext(600, 302),
            OnNext(620, 303),
            OnNext(740, 106),
            OnNext(810, 304),
            OnNext(860, 305),
            OnNext(930, 401),
            OnNext(940, 402)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000));

        ys3.Subscriptions.AssertEqual(
            Subscribe(300, 760));

        ys4.Subscriptions.AssertEqual(
            Subscribe(400, 605));

        ys5.Subscriptions.AssertEqual(
            Subscribe(550, 960));

        ys6.Subscriptions.AssertEqual(
            Subscribe(750, 790));

        ys7.Subscriptions.AssertEqual(
            Subscribe(850, 950));
    }

    [TestMethod]
    public void SelectMany_Error_Outer()
    {
        var ex = new Exception();

        var ys1 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 102),
            OnNext(90, 103),
            OnNext(110, 104),
            OnNext(190, 105),
            OnNext(440, 106),
            OnCompleted<int>(460)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(180, 202),
            OnNext(190, 203),
            OnCompleted<int>(205)
        );

        var ys5 = Scheduler.CreateColdObservable(
            OnNext(10, 301),
            OnNext(50, 302),
            OnNext(70, 303),
            OnNext(260, 304),
            OnNext(310, 305),
            OnCompleted<int>(410)
        );

        var ys6 = Scheduler.CreateColdObservable(
            OnCompleted<int>(40)
        );

        var ys7 = Scheduler.CreateColdObservable(
            OnNext(80, 401),
            OnNext(90, 402),
            OnCompleted<int>(100)
        );

        var xs = Scheduler.CreateHotObservable(
                OnNext<Seq<int>>(5, ys1),
                OnNext<Seq<int>>(105, ys2),
                OnNext<Seq<int>>(300, ys3),
                OnNext<Seq<int>>(400, ys4),
                OnNext<Seq<int>>(550, ys5),
                OnNext<Seq<int>>(750, ys6),
                OnNext<Seq<int>>(850, ys7),
                OnError<Seq<int>>(900, ex)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(x => x)
        );

        res.Messages.AssertEqual(
            OnNext(310, 102),
            OnNext(390, 103),
            OnNext(410, 104),
            OnNext(490, 105),
            OnNext(560, 301),
            OnNext(580, 202),
            OnNext(590, 203),
            OnNext(600, 302),
            OnNext(620, 303),
            OnNext(740, 106),
            OnNext(810, 304),
            OnNext(860, 305),
            OnError<int>(900, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 900));

        ys3.Subscriptions.AssertEqual(
            Subscribe(300, 760));

        ys4.Subscriptions.AssertEqual(
            Subscribe(400, 605));

        ys5.Subscriptions.AssertEqual(
            Subscribe(550, 900));

        ys6.Subscriptions.AssertEqual(
            Subscribe(750, 790));

        ys7.Subscriptions.AssertEqual(
            Subscribe(850, 900));
    }

    [TestMethod]
    public void SelectMany_Error_Inner()
    {
        var ex = new Exception();

        var ys1 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 102),
            OnNext(90, 103),
            OnNext(110, 104),
            OnNext(190, 105),
            OnNext(440, 106),
            OnError<int>(460, ex)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(180, 202),
            OnNext(190, 203),
            OnCompleted<int>(205)
        );

        var ys5 = Scheduler.CreateColdObservable(
            OnNext(10, 301),
            OnNext(50, 302),
            OnNext(70, 303),
            OnNext(260, 304),
            OnNext(310, 305),
            OnCompleted<int>(410)
        );

        var ys6 = Scheduler.CreateColdObservable(
            OnCompleted<int>(40)
        );

        var ys7 = Scheduler.CreateColdObservable(
            OnNext(80, 401),
            OnNext(90, 402),
            OnCompleted<int>(100)
        );

        var xs = Scheduler.CreateHotObservable(
                OnNext<Seq<int>>(5, ys1),
                OnNext<Seq<int>>(105, ys2),
                OnNext<Seq<int>>(300, ys3),
                OnNext<Seq<int>>(400, ys4),
                OnNext<Seq<int>>(550, ys5),
                OnNext<Seq<int>>(750, ys6),
                OnNext<Seq<int>>(850, ys7),
                OnCompleted<Seq<int>>(900)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(x => x)
        );

        res.Messages.AssertEqual(
            OnNext(310, 102),
            OnNext(390, 103),
            OnNext(410, 104),
            OnNext(490, 105),
            OnNext(560, 301),
            OnNext(580, 202),
            OnNext(590, 203),
            OnNext(600, 302),
            OnNext(620, 303),
            OnNext(740, 106),
            OnError<int>(760, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 760));

        ys3.Subscriptions.AssertEqual(
            Subscribe(300, 760));

        ys4.Subscriptions.AssertEqual(
            Subscribe(400, 605));

        ys5.Subscriptions.AssertEqual(
            Subscribe(550, 760));

        ys6.Subscriptions.AssertEqual(
            Subscribe(750, 760));

        ys7.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void SelectMany_Dispose()
    {
        var ys1 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 102),
            OnNext(90, 103),
            OnNext(110, 104),
            OnNext(190, 105),
            OnNext(440, 106),
            OnCompleted<int>(460)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(180, 202),
            OnNext(190, 203),
            OnCompleted<int>(205)
        );

        var ys5 = Scheduler.CreateColdObservable(
            OnNext(10, 301),
            OnNext(50, 302),
            OnNext(70, 303),
            OnNext(260, 304),
            OnNext(310, 305),
            OnCompleted<int>(410)
        );

        var ys6 = Scheduler.CreateColdObservable(
            OnCompleted<int>(40)
        );

        var ys7 = Scheduler.CreateColdObservable(
            OnNext(80, 401),
            OnNext(90, 402),
            OnCompleted<int>(100)
        );

        var xs = Scheduler.CreateHotObservable(
                OnNext<Seq<int>>(5, ys1),
                OnNext<Seq<int>>(105, ys2),
                OnNext<Seq<int>>(300, ys3),
                OnNext<Seq<int>>(400, ys4),
                OnNext<Seq<int>>(550, ys5),
                OnNext<Seq<int>>(750, ys6),
                OnNext<Seq<int>>(850, ys7),
                OnCompleted<Seq<int>>(900)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(x => x),
            700
        );

        res.Messages.AssertEqual(
            OnNext(310, 102),
            OnNext(390, 103),
            OnNext(410, 104),
            OnNext(490, 105),
            OnNext(560, 301),
            OnNext(580, 202),
            OnNext(590, 203),
            OnNext(600, 302),
            OnNext(620, 303)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 700));

        ys3.Subscriptions.AssertEqual(
            Subscribe(300, 700));

        ys4.Subscriptions.AssertEqual(
            Subscribe(400, 605));

        ys5.Subscriptions.AssertEqual(
            Subscribe(550, 700));

        ys6.Subscriptions.AssertEqual(
        );

        ys7.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void SelectMany_Throw()
    {
        var ys1 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 102),
            OnNext(90, 103),
            OnNext(110, 104),
            OnNext(190, 105),
            OnNext(440, 106),
            OnCompleted<int>(460)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(180, 202),
            OnNext(190, 203),
            OnCompleted<int>(205)
        );

        var ys5 = Scheduler.CreateColdObservable(
            OnNext(10, 301),
            OnNext(50, 302),
            OnNext(70, 303),
            OnNext(260, 304),
            OnNext(310, 305),
            OnCompleted<int>(410)
        );

        var ys6 = Scheduler.CreateColdObservable(
            OnCompleted<int>(40)
        );

        var ys7 = Scheduler.CreateColdObservable(
            OnNext(80, 401),
            OnNext(90, 402),
            OnCompleted<int>(100)
        );

        var xs = Scheduler.CreateHotObservable(
                OnNext<Seq<int>>(5, ys1),
                OnNext<Seq<int>>(105, ys2),
                OnNext<Seq<int>>(300, ys3),
                OnNext<Seq<int>>(400, ys4),
                OnNext<Seq<int>>(550, ys5),
                OnNext<Seq<int>>(750, ys6),
                OnNext<Seq<int>>(850, ys7),
                OnCompleted<Seq<int>>(900)
        );

        var invoked = 0;

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany(x =>
            {
                invoked++;
                if (invoked == 3)
                {
                    throw ex;
                }

                return x;
            })
        );

        res.Messages.AssertEqual(
            OnNext(310, 102),
            OnNext(390, 103),
            OnNext(410, 104),
            OnNext(490, 105),
            OnError<int>(550, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550));

        ys3.Subscriptions.AssertEqual(
            Subscribe(300, 550));

        ys4.Subscriptions.AssertEqual(
            Subscribe(400, 550));

        ys5.Subscriptions.AssertEqual(
        );

        ys6.Subscriptions.AssertEqual(
        );

        ys7.Subscriptions.AssertEqual(
        );

        Assert.AreEqual(3, invoked);
    }

    [TestMethod]
    public void SelectMany_UseFunction()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 4),
            OnNext(220, 3),
            OnNext(250, 5),
            OnNext(270, 1),
            OnCompleted<int>(290)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(x =>
                Seq.Interval(TimeSpan.FromTicks(10), Scheduler).Select(_ => x).Take(x))
        );

        res.Messages.AssertEqual(
            OnNext(220, 4),
            OnNext(230, 3),
            OnNext(230, 4),
            OnNext(240, 3),
            OnNext(240, 4),
            OnNext(250, 3),
            OnNext(250, 4),
            OnNext(260, 5),
            OnNext(270, 5),
            OnNext(280, 1),
            OnNext(280, 5),
            OnNext(290, 5),
            OnNext(300, 5),
            OnCompleted<int>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 290)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Index()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 4),
            OnNext(220, 3),
            OnNext(250, 5),
            OnNext(270, 1),
            OnCompleted<int>(290)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, i) => Seq.Return(new { x, i }))
        );

        var witness = new { x = 0, i = 0 };

        res.Messages.AssertEqual(
            OnNext(210, new { x = 4, i = 0 }),
            OnNext(220, new { x = 3, i = 1 }),
            OnNext(250, new { x = 5, i = 2 }),
            OnNext(270, new { x = 1, i = 3 }),
            OnCompleted(290, witness)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 290)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Complete()
    {
        var ys1 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 102),
            OnNext(90, 103),
            OnNext(110, 104),
            OnNext(190, 105),
            OnNext(440, 106),
            OnCompleted<int>(460)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(180, 202),
            OnNext(190, 203),
            OnCompleted<int>(205)
        );

        var ys5 = Scheduler.CreateColdObservable(
            OnNext(10, 301),
            OnNext(50, 302),
            OnNext(70, 303),
            OnNext(260, 304),
            OnNext(310, 305),
            OnCompleted<int>(410)
        );

        var ys6 = Scheduler.CreateColdObservable(
            OnCompleted<int>(40)
        );

        var ys7 = Scheduler.CreateColdObservable(
            OnNext(80, 401),
            OnNext(90, 402),
            OnCompleted<int>(100)
        );

        var xs = Scheduler.CreateHotObservable(
                OnNext<Seq<int>>(5, ys1),
                OnNext<Seq<int>>(105, ys2),
                OnNext<Seq<int>>(300, ys3),
                OnNext<Seq<int>>(400, ys4),
                OnNext<Seq<int>>(550, ys5),
                OnNext<Seq<int>>(750, ys6),
                OnNext<Seq<int>>(850, ys7),
                OnCompleted<Seq<int>>(900)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) => x)
        );

        res.Messages.AssertEqual(
            OnNext(310, 102),
            OnNext(390, 103),
            OnNext(410, 104),
            OnNext(490, 105),
            OnNext(560, 301),
            OnNext(580, 202),
            OnNext(590, 203),
            OnNext(600, 302),
            OnNext(620, 303),
            OnNext(740, 106),
            OnNext(810, 304),
            OnNext(860, 305),
            OnNext(930, 401),
            OnNext(940, 402),
            OnCompleted<int>(960)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 900));

        ys3.Subscriptions.AssertEqual(
            Subscribe(300, 760));

        ys4.Subscriptions.AssertEqual(
            Subscribe(400, 605));

        ys5.Subscriptions.AssertEqual(
            Subscribe(550, 960));

        ys6.Subscriptions.AssertEqual(
            Subscribe(750, 790));

        ys7.Subscriptions.AssertEqual(
            Subscribe(850, 950));
    }

    [TestMethod]
    public void SelectManyWithIndex_Complete_InnerNotComplete()
    {
        var ys1 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 102),
            OnNext(90, 103),
            OnNext(110, 104),
            OnNext(190, 105),
            OnNext(440, 106),
            OnCompleted<int>(460)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(180, 202),
            OnNext(190, 203)
        );

        var ys5 = Scheduler.CreateColdObservable(
            OnNext(10, 301),
            OnNext(50, 302),
            OnNext(70, 303),
            OnNext(260, 304),
            OnNext(310, 305),
            OnCompleted<int>(410)
        );

        var ys6 = Scheduler.CreateColdObservable(
            OnCompleted<int>(40)
        );

        var ys7 = Scheduler.CreateColdObservable(
            OnNext(80, 401),
            OnNext(90, 402),
            OnCompleted<int>(100)
        );

        var xs = Scheduler.CreateHotObservable(
                OnNext<Seq<int>>(5, ys1),
                OnNext<Seq<int>>(105, ys2),
                OnNext<Seq<int>>(300, ys3),
                OnNext<Seq<int>>(400, ys4),
                OnNext<Seq<int>>(550, ys5),
                OnNext<Seq<int>>(750, ys6),
                OnNext<Seq<int>>(850, ys7),
                OnCompleted<Seq<int>>(900)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) => x)
        );

        res.Messages.AssertEqual(
            OnNext(310, 102),
            OnNext(390, 103),
            OnNext(410, 104),
            OnNext(490, 105),
            OnNext(560, 301),
            OnNext(580, 202),
            OnNext(590, 203),
            OnNext(600, 302),
            OnNext(620, 303),
            OnNext(740, 106),
            OnNext(810, 304),
            OnNext(860, 305),
            OnNext(930, 401),
            OnNext(940, 402)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 900));

        ys3.Subscriptions.AssertEqual(
            Subscribe(300, 760));

        ys4.Subscriptions.AssertEqual(
            Subscribe(400, 1000));

        ys5.Subscriptions.AssertEqual(
            Subscribe(550, 960));

        ys6.Subscriptions.AssertEqual(
            Subscribe(750, 790));

        ys7.Subscriptions.AssertEqual(
            Subscribe(850, 950));
    }

    [TestMethod]
    public void SelectManyWithIndex_Complete_OuterNotComplete()
    {
        var ys1 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 102),
            OnNext(90, 103),
            OnNext(110, 104),
            OnNext(190, 105),
            OnNext(440, 106),
            OnCompleted<int>(460)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(180, 202),
            OnNext(190, 203),
            OnCompleted<int>(205)
        );

        var ys5 = Scheduler.CreateColdObservable(
            OnNext(10, 301),
            OnNext(50, 302),
            OnNext(70, 303),
            OnNext(260, 304),
            OnNext(310, 305),
            OnCompleted<int>(410)
        );

        var ys6 = Scheduler.CreateColdObservable(
            OnCompleted<int>(40)
        );

        var ys7 = Scheduler.CreateColdObservable(
            OnNext(80, 401),
            OnNext(90, 402),
            OnCompleted<int>(100)
        );

        var xs = Scheduler.CreateHotObservable(
                OnNext<Seq<int>>(5, ys1),
                OnNext<Seq<int>>(105, ys2),
                OnNext<Seq<int>>(300, ys3),
                OnNext<Seq<int>>(400, ys4),
                OnNext<Seq<int>>(550, ys5),
                OnNext<Seq<int>>(750, ys6),
                OnNext<Seq<int>>(850, ys7)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) => x)
        );

        res.Messages.AssertEqual(
            OnNext(310, 102),
            OnNext(390, 103),
            OnNext(410, 104),
            OnNext(490, 105),
            OnNext(560, 301),
            OnNext(580, 202),
            OnNext(590, 203),
            OnNext(600, 302),
            OnNext(620, 303),
            OnNext(740, 106),
            OnNext(810, 304),
            OnNext(860, 305),
            OnNext(930, 401),
            OnNext(940, 402)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000));

        ys3.Subscriptions.AssertEqual(
            Subscribe(300, 760));

        ys4.Subscriptions.AssertEqual(
            Subscribe(400, 605));

        ys5.Subscriptions.AssertEqual(
            Subscribe(550, 960));

        ys6.Subscriptions.AssertEqual(
            Subscribe(750, 790));

        ys7.Subscriptions.AssertEqual(
            Subscribe(850, 950));
    }

    [TestMethod]
    public void SelectManyWithIndex_Error_Outer()
    {
        var ex = new Exception();

        var ys1 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 102),
            OnNext(90, 103),
            OnNext(110, 104),
            OnNext(190, 105),
            OnNext(440, 106),
            OnCompleted<int>(460)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(180, 202),
            OnNext(190, 203),
            OnCompleted<int>(205)
        );

        var ys5 = Scheduler.CreateColdObservable(
            OnNext(10, 301),
            OnNext(50, 302),
            OnNext(70, 303),
            OnNext(260, 304),
            OnNext(310, 305),
            OnCompleted<int>(410)
        );

        var ys6 = Scheduler.CreateColdObservable(
            OnCompleted<int>(40)
        );

        var ys7 = Scheduler.CreateColdObservable(
            OnNext(80, 401),
            OnNext(90, 402),
            OnCompleted<int>(100)
        );

        var xs = Scheduler.CreateHotObservable(
                OnNext<Seq<int>>(5, ys1),
                OnNext<Seq<int>>(105, ys2),
                OnNext<Seq<int>>(300, ys3),
                OnNext<Seq<int>>(400, ys4),
                OnNext<Seq<int>>(550, ys5),
                OnNext<Seq<int>>(750, ys6),
                OnNext<Seq<int>>(850, ys7),
                OnError<Seq<int>>(900, ex)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) => x)
        );

        res.Messages.AssertEqual(
            OnNext(310, 102),
            OnNext(390, 103),
            OnNext(410, 104),
            OnNext(490, 105),
            OnNext(560, 301),
            OnNext(580, 202),
            OnNext(590, 203),
            OnNext(600, 302),
            OnNext(620, 303),
            OnNext(740, 106),
            OnNext(810, 304),
            OnNext(860, 305),
            OnError<int>(900, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 900));

        ys3.Subscriptions.AssertEqual(
            Subscribe(300, 760));

        ys4.Subscriptions.AssertEqual(
            Subscribe(400, 605));

        ys5.Subscriptions.AssertEqual(
            Subscribe(550, 900));

        ys6.Subscriptions.AssertEqual(
            Subscribe(750, 790));

        ys7.Subscriptions.AssertEqual(
            Subscribe(850, 900));
    }

    [TestMethod]
    public void SelectManyWithIndex_Error_Inner()
    {
        var ex = new Exception();

        var ys1 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 102),
            OnNext(90, 103),
            OnNext(110, 104),
            OnNext(190, 105),
            OnNext(440, 106),
            OnError<int>(460, ex)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(180, 202),
            OnNext(190, 203),
            OnCompleted<int>(205)
        );

        var ys5 = Scheduler.CreateColdObservable(
            OnNext(10, 301),
            OnNext(50, 302),
            OnNext(70, 303),
            OnNext(260, 304),
            OnNext(310, 305),
            OnCompleted<int>(410)
        );

        var ys6 = Scheduler.CreateColdObservable(
            OnCompleted<int>(40)
        );

        var ys7 = Scheduler.CreateColdObservable(
            OnNext(80, 401),
            OnNext(90, 402),
            OnCompleted<int>(100)
        );

        var xs = Scheduler.CreateHotObservable(
                OnNext<Seq<int>>(5, ys1),
                OnNext<Seq<int>>(105, ys2),
                OnNext<Seq<int>>(300, ys3),
                OnNext<Seq<int>>(400, ys4),
                OnNext<Seq<int>>(550, ys5),
                OnNext<Seq<int>>(750, ys6),
                OnNext<Seq<int>>(850, ys7),
                OnCompleted<Seq<int>>(900)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) => x)
        );

        res.Messages.AssertEqual(
            OnNext(310, 102),
            OnNext(390, 103),
            OnNext(410, 104),
            OnNext(490, 105),
            OnNext(560, 301),
            OnNext(580, 202),
            OnNext(590, 203),
            OnNext(600, 302),
            OnNext(620, 303),
            OnNext(740, 106),
            OnError<int>(760, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 760));

        ys3.Subscriptions.AssertEqual(
            Subscribe(300, 760));

        ys4.Subscriptions.AssertEqual(
            Subscribe(400, 605));

        ys5.Subscriptions.AssertEqual(
            Subscribe(550, 760));

        ys6.Subscriptions.AssertEqual(
            Subscribe(750, 760));

        ys7.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Dispose()
    {
        var ys1 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 102),
            OnNext(90, 103),
            OnNext(110, 104),
            OnNext(190, 105),
            OnNext(440, 106),
            OnCompleted<int>(460)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(180, 202),
            OnNext(190, 203),
            OnCompleted<int>(205)
        );

        var ys5 = Scheduler.CreateColdObservable(
            OnNext(10, 301),
            OnNext(50, 302),
            OnNext(70, 303),
            OnNext(260, 304),
            OnNext(310, 305),
            OnCompleted<int>(410)
        );

        var ys6 = Scheduler.CreateColdObservable(
            OnCompleted<int>(40)
        );

        var ys7 = Scheduler.CreateColdObservable(
            OnNext(80, 401),
            OnNext(90, 402),
            OnCompleted<int>(100)
        );

        var xs = Scheduler.CreateHotObservable(
                OnNext<Seq<int>>(5, ys1),
                OnNext<Seq<int>>(105, ys2),
                OnNext<Seq<int>>(300, ys3),
                OnNext<Seq<int>>(400, ys4),
                OnNext<Seq<int>>(550, ys5),
                OnNext<Seq<int>>(750, ys6),
                OnNext<Seq<int>>(850, ys7),
                OnCompleted<Seq<int>>(900)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) => x),
            700
        );

        res.Messages.AssertEqual(
            OnNext(310, 102),
            OnNext(390, 103),
            OnNext(410, 104),
            OnNext(490, 105),
            OnNext(560, 301),
            OnNext(580, 202),
            OnNext(590, 203),
            OnNext(600, 302),
            OnNext(620, 303)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 700));

        ys3.Subscriptions.AssertEqual(
            Subscribe(300, 700));

        ys4.Subscriptions.AssertEqual(
            Subscribe(400, 605));

        ys5.Subscriptions.AssertEqual(
            Subscribe(550, 700));

        ys6.Subscriptions.AssertEqual(
        );

        ys7.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Throw()
    {
        var ys1 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnError<int>(1, new InvalidOperationException())
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 102),
            OnNext(90, 103),
            OnNext(110, 104),
            OnNext(190, 105),
            OnNext(440, 106),
            OnCompleted<int>(460)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(180, 202),
            OnNext(190, 203),
            OnCompleted<int>(205)
        );

        var ys5 = Scheduler.CreateColdObservable(
            OnNext(10, 301),
            OnNext(50, 302),
            OnNext(70, 303),
            OnNext(260, 304),
            OnNext(310, 305),
            OnCompleted<int>(410)
        );

        var ys6 = Scheduler.CreateColdObservable(
            OnCompleted<int>(40)
        );

        var ys7 = Scheduler.CreateColdObservable(
            OnNext(80, 401),
            OnNext(90, 402),
            OnCompleted<int>(100)
        );

        var xs = Scheduler.CreateHotObservable(
                OnNext<Seq<int>>(5, ys1),
                OnNext<Seq<int>>(105, ys2),
                OnNext<Seq<int>>(300, ys3),
                OnNext<Seq<int>>(400, ys4),
                OnNext<Seq<int>>(550, ys5),
                OnNext<Seq<int>>(750, ys6),
                OnNext<Seq<int>>(850, ys7),
                OnCompleted<Seq<int>>(900)
        );

        var invoked = 0;

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) =>
            {
                invoked++;
                if (invoked == 3)
                {
                    throw ex;
                }

                return x;
            })
        );

        res.Messages.AssertEqual(
            OnNext(310, 102),
            OnNext(390, 103),
            OnNext(410, 104),
            OnNext(490, 105),
            OnError<int>(550, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550));

        ys3.Subscriptions.AssertEqual(
            Subscribe(300, 550));

        ys4.Subscriptions.AssertEqual(
            Subscribe(400, 550));

        ys5.Subscriptions.AssertEqual(
        );

        ys6.Subscriptions.AssertEqual(
        );

        ys7.Subscriptions.AssertEqual(
        );

        Assert.AreEqual(3, invoked);
    }

    [TestMethod]
    public void SelectManyWithIndex_UseFunction()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 4),
            OnNext(220, 3),
            OnNext(250, 5),
            OnNext(270, 1),
            OnCompleted<int>(290)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) =>
                Seq.Interval(TimeSpan.FromTicks(10), Scheduler).Select(__ => x).Take(x))
        );

        res.Messages.AssertEqual(
            OnNext(220, 4),
            OnNext(230, 3),
            OnNext(230, 4),
            OnNext(240, 3),
            OnNext(240, 4),
            OnNext(250, 3),
            OnNext(250, 4),
            OnNext(260, 5),
            OnNext(270, 5),
            OnNext(280, 1),
            OnNext(280, 5),
            OnNext(290, 5),
            OnNext(300, 5),
            OnCompleted<int>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 290)
        );
    }

    [TestMethod]
    public void SelectMany_QueryOperator_CompleteOuterFirst()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, 4),
            OnNext(221, 3),
            OnNext(222, 2),
            OnNext(223, 5),
            OnCompleted<int>(224)
        );

        var res = Scheduler.Start(() =>
            from x in xs
            from y in Seq.Interval(TimeSpan.FromTicks(1), Scheduler).Take(x)
            select x * 10 + (int)y
        );

        res.Messages.AssertEqual(
            OnNext(221, 40),
            OnNext(222, 30),
            OnNext(222, 41),
            OnNext(223, 20),
            OnNext(223, 31),
            OnNext(223, 42),
            OnNext(224, 50),
            OnNext(224, 21),
            OnNext(224, 32),
            OnNext(224, 43),
            OnNext(225, 51),
            OnNext(226, 52),
            OnNext(227, 53),
            OnNext(228, 54),
            OnCompleted<int>(228)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 224)
        );
    }

    [TestMethod]
    public void SelectMany_QueryOperator_CompleteInnerFirst()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, 4),
            OnNext(221, 3),
            OnNext(222, 2),
            OnNext(223, 5),
            OnCompleted<int>(300)
        );

        var res = Scheduler.Start(() =>
            from x in xs
            from y in Seq.Interval(TimeSpan.FromTicks(1), Scheduler).Take(x)
            select x * 10 + (int)y
        );

        res.Messages.AssertEqual(
            OnNext(221, 40),
            OnNext(222, 30),
            OnNext(222, 41),
            OnNext(223, 20),
            OnNext(223, 31),
            OnNext(223, 42),
            OnNext(224, 50),
            OnNext(224, 21),
            OnNext(224, 32),
            OnNext(224, 43),
            OnNext(225, 51),
            OnNext(226, 52),
            OnNext(227, 53),
            OnNext(228, 54),
            OnCompleted<int>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void SelectMany_QueryOperator_ErrorOuter()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(220, 4),
            OnNext(221, 3),
            OnNext(222, 2),
            OnNext(223, 5),
            OnError<int>(224, ex)
        );

        var res = Scheduler.Start(() =>
            from x in xs
            from y in Seq.Interval(TimeSpan.FromTicks(1), Scheduler).Take(x)
            select x * 10 + (int)y
        );

        res.Messages.AssertEqual(
            OnNext(221, 40),
            OnNext(222, 30),
            OnNext(222, 41),
            OnNext(223, 20),
            OnNext(223, 31),
            OnNext(223, 42),
            OnError<int>(224, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 224)
        );
    }

    [TestMethod]
    public void SelectMany_QueryOperator_ErrorInner()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, 4),
            OnNext(221, 3),
            OnNext(222, 2),
            OnNext(223, 5),
            OnCompleted<int>(224)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            from x in xs
            from y in x == 2 ? Scheduler.CreateColdObservable(OnError<long>(1, ex))
                             : Seq.Interval(TimeSpan.FromTicks(1), Scheduler).Take(x)
            select x * 10 + (int)y
        );

        res.Messages.AssertEqual(
            OnNext(221, 40),
            OnNext(222, 30),
            OnNext(222, 41),
            OnError<int>(223, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 223)
        );
    }

    [TestMethod]
    public void SelectMany_QueryOperator_Dispose()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, 4),
            OnNext(221, 3),
            OnNext(222, 2),
            OnNext(223, 5),
            OnCompleted<int>(224)
        );

        var res = Scheduler.Start(() =>
            from x in xs
            from y in Seq.Interval(TimeSpan.FromTicks(1), Scheduler).Take(x)
            select x * 10 + (int)y,
            223
        );

        res.Messages.AssertEqual(
            OnNext(221, 40),
            OnNext(222, 30),
            OnNext(222, 41)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 223)
        );
    }

    [TestMethod]
    public void SelectMany_QueryOperator_ThrowSelector()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, 4),
            OnNext(221, 3),
            OnNext(222, 2),
            OnNext(223, 5),
            OnCompleted<int>(224)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            from x in xs
            from y in Throw<Seq<long>>(ex)
            select x * 10 + (int)y
        );

        res.Messages.AssertEqual(
            OnError<int>(220, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void SelectMany_QueryOperator_ThrowResult()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, 4),
            OnNext(221, 3),
            OnNext(222, 2),
            OnNext(223, 5),
            OnCompleted<int>(224)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            from x in xs
            from y in Seq.Interval(TimeSpan.FromTicks(1), Scheduler).Take(x)
            select Throw<int>(ex)
        );

        res.Messages.AssertEqual(
            OnError<int>(221, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 221)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_QueryOperator_Index()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 4),
            OnNext(220, 3),
            OnNext(250, 5),
            OnNext(270, 1),
            OnCompleted<int>(290)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, i) => Seq.Range(10, i + 1), (x, i, y, j) => new { x, i, y, j })
        );

        var witness = new { x = 0, i = 0, y = 0, j = 0 };

        res.Messages.AssertEqual(
            OnNext(210, new { x = 4, i = 0, y = 10, j = 0 }),
            OnNext(220, new { x = 3, i = 1, y = 10, j = 0 }),
            OnNext(220, new { x = 3, i = 1, y = 11, j = 1 }),
            OnNext(250, new { x = 5, i = 2, y = 10, j = 0 }),
            OnNext(250, new { x = 5, i = 2, y = 11, j = 1 }),
            OnNext(250, new { x = 5, i = 2, y = 12, j = 2 }),
            OnNext(270, new { x = 1, i = 3, y = 10, j = 0 }),
            OnNext(270, new { x = 1, i = 3, y = 11, j = 1 }),
            OnNext(270, new { x = 1, i = 3, y = 12, j = 2 }),
            OnNext(270, new { x = 1, i = 3, y = 13, j = 3 }),
            OnCompleted(290, witness)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 290)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_QueryOperator_CompleteOuterFirst()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, 4),
            OnNext(221, 3),
            OnNext(222, 2),
            OnNext(223, 5),
            OnCompleted<int>(224)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => Seq.Interval(TimeSpan.FromTicks(1), Scheduler).Take(x),
                (x, _, y, __) => x * 10 + (int)y)
        );

        res.Messages.AssertEqual(
            OnNext(221, 40),
            OnNext(222, 30),
            OnNext(222, 41),
            OnNext(223, 20),
            OnNext(223, 31),
            OnNext(223, 42),
            OnNext(224, 50),
            OnNext(224, 21),
            OnNext(224, 32),
            OnNext(224, 43),
            OnNext(225, 51),
            OnNext(226, 52),
            OnNext(227, 53),
            OnNext(228, 54),
            OnCompleted<int>(228)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 224)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_QueryOperator_CompleteInnerFirst()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, 4),
            OnNext(221, 3),
            OnNext(222, 2),
            OnNext(223, 5),
            OnCompleted<int>(300)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => Seq.Interval(TimeSpan.FromTicks(1), Scheduler).Take(x),
                (x, _, y, __) => x * 10 + (int)y)
        );

        res.Messages.AssertEqual(
            OnNext(221, 40),
            OnNext(222, 30),
            OnNext(222, 41),
            OnNext(223, 20),
            OnNext(223, 31),
            OnNext(223, 42),
            OnNext(224, 50),
            OnNext(224, 21),
            OnNext(224, 32),
            OnNext(224, 43),
            OnNext(225, 51),
            OnNext(226, 52),
            OnNext(227, 53),
            OnNext(228, 54),
            OnCompleted<int>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_QueryOperator_ErrorOuter()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(220, 4),
            OnNext(221, 3),
            OnNext(222, 2),
            OnNext(223, 5),
            OnError<int>(224, ex)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => Seq.Interval(TimeSpan.FromTicks(1), Scheduler).Take(x),
                (x, _, y, __) => x * 10 + (int)y)
        );

        res.Messages.AssertEqual(
            OnNext(221, 40),
            OnNext(222, 30),
            OnNext(222, 41),
            OnNext(223, 20),
            OnNext(223, 31),
            OnNext(223, 42),
            OnError<int>(224, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 224)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_QueryOperator_ErrorInner()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, 4),
            OnNext(221, 3),
            OnNext(222, 2),
            OnNext(223, 5),
            OnCompleted<int>(224)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => x == 2
                        ? Scheduler.CreateColdObservable(OnError<long>(1, ex))
                        : Seq.Interval(TimeSpan.FromTicks(1), Scheduler).Take(x),
                (x, _, y, __) => x * 10 + (int)y)
        );

        res.Messages.AssertEqual(
            OnNext(221, 40),
            OnNext(222, 30),
            OnNext(222, 41),
            OnError<int>(223, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 223)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_QueryOperator_Dispose()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, 4),
            OnNext(221, 3),
            OnNext(222, 2),
            OnNext(223, 5),
            OnCompleted<int>(224)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => Seq.Interval(TimeSpan.FromTicks(1), Scheduler).Take(x),
                (x, _, y, __) => x * 10 + (int)y),
            223
        );

        res.Messages.AssertEqual(
            OnNext(221, 40),
            OnNext(222, 30),
            OnNext(222, 41)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 223)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_QueryOperator_ThrowSelector()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, 4),
            OnNext(221, 3),
            OnNext(222, 2),
            OnNext(223, 5),
            OnCompleted<int>(224)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) => Throw<Seq<long>>(ex), (x, _, y, __) => x * 10 + (int)y)
        );

        res.Messages.AssertEqual(
            OnError<int>(220, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_QueryOperator_ThrowResult()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, 4),
            OnNext(221, 3),
            OnNext(222, 2),
            OnNext(223, 5),
            OnCompleted<int>(224)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => Seq.Interval(TimeSpan.FromTicks(1), Scheduler).Take(x),
                (x, _, y, __) => Throw<int>(ex))
        );

        res.Messages.AssertEqual(
            OnError<int>(221, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 221)
        );
    }

    [TestMethod]
    public void SelectMany_Triple_Identity()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x => Seq.Return(x, Scheduler),
                ex => Seq.Throw<int>(ex, Scheduler),
                () => Seq.Empty<int>(Scheduler)
            )
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(300), 0),
            OnNext(ScheduledAt(301), 1),
            OnNext(ScheduledAt(302), 2),
            OnNext(ScheduledAt(303), 3),
            OnNext(ScheduledAt(304), 4),
            OnCompleted<int>(ScheduledAt(305))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectMany_Triple_InnersWithTiming1()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var ysn = Scheduler.CreateColdObservable(
            OnNext(10, 10),
            OnNext(20, 11),
            OnNext(30, 12),
            OnCompleted<int>(40)
        );

        var yse = Scheduler.CreateColdObservable(
            OnNext(0, 99),
            OnCompleted<int>(10)
        );

        var ysc = Scheduler.CreateColdObservable(
            OnNext(10, 42),
            OnCompleted<int>(20)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x => ysn,
                ex => yse,
                () => ysc
            )
        );

        res.Messages.AssertEqual(
            OnNext(310, 10),
            OnNext(311, 10),
            OnNext(312, 10),
            OnNext(313, 10),
            OnNext(314, 10),
            OnNext(315, 42),
            OnNext(320, 11),
            OnNext(321, 11),
            OnNext(322, 11),
            OnNext(323, 11),
            OnNext(324, 11),
            OnNext(330, 12),
            OnNext(331, 12),
            OnNext(332, 12),
            OnNext(333, 12),
            OnNext(334, 12),
            OnCompleted<int>(344)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );

        ysn.Subscriptions.AssertEqual(
            Subscribe(300, 340),
            Subscribe(301, 341),
            Subscribe(302, 342),
            Subscribe(303, 343),
            Subscribe(304, 344)
        );

        yse.Subscriptions.AssertEqual(
        );

        ysc.Subscriptions.AssertEqual(
            Subscribe(305, 325)
        );
    }

    [TestMethod]
    public void SelectMany_Triple_InnersWithTiming2()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var ysn = Scheduler.CreateColdObservable(
            OnNext(10, 10),
            OnNext(20, 11),
            OnNext(30, 12),
            OnCompleted<int>(40)
        );

        var yse = Scheduler.CreateColdObservable(
            OnNext(0, 99),
            OnCompleted<int>(10)
        );

        var ysc = Scheduler.CreateColdObservable(
            OnNext(10, 42),
            OnCompleted<int>(50)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x => ysn,
                ex => yse,
                () => ysc
            )
        );

        res.Messages.AssertEqual(
            OnNext(310, 10),
            OnNext(311, 10),
            OnNext(312, 10),
            OnNext(313, 10),
            OnNext(314, 10),
            OnNext(315, 42),
            OnNext(320, 11),
            OnNext(321, 11),
            OnNext(322, 11),
            OnNext(323, 11),
            OnNext(324, 11),
            OnNext(330, 12),
            OnNext(331, 12),
            OnNext(332, 12),
            OnNext(333, 12),
            OnNext(334, 12),
            OnCompleted<int>(355)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );

        ysn.Subscriptions.AssertEqual(
            Subscribe(300, 340),
            Subscribe(301, 341),
            Subscribe(302, 342),
            Subscribe(303, 343),
            Subscribe(304, 344)
        );

        yse.Subscriptions.AssertEqual(
        );

        ysc.Subscriptions.AssertEqual(
            Subscribe(305, 355)
        );
    }

    [TestMethod]
    public void SelectMany_Triple_InnersWithTiming3()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(400, 1),
            OnNext(500, 2),
            OnNext(600, 3),
            OnNext(700, 4),
            OnCompleted<int>(800)
        );

        var ysn = Scheduler.CreateColdObservable(
            OnNext(10, 10),
            OnNext(20, 11),
            OnNext(30, 12),
            OnCompleted<int>(40)
        );

        var yse = Scheduler.CreateColdObservable(
            OnNext(0, 99),
            OnCompleted<int>(10)
        );

        var ysc = Scheduler.CreateColdObservable(
            OnNext(10, 42),
            OnCompleted<int>(100)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x => ysn,
                ex => yse,
                () => ysc
            )
        );

        res.Messages.AssertEqual(
            OnNext(310, 10),
            OnNext(320, 11),
            OnNext(330, 12),
            OnNext(410, 10),
            OnNext(420, 11),
            OnNext(430, 12),
            OnNext(510, 10),
            OnNext(520, 11),
            OnNext(530, 12),
            OnNext(610, 10),
            OnNext(620, 11),
            OnNext(630, 12),
            OnNext(710, 10),
            OnNext(720, 11),
            OnNext(730, 12),
            OnNext(810, 42),
            OnCompleted<int>(900)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 800)
        );

        ysn.Subscriptions.AssertEqual(
            Subscribe(300, 340),
            Subscribe(400, 440),
            Subscribe(500, 540),
            Subscribe(600, 640),
            Subscribe(700, 740)
        );

        yse.Subscriptions.AssertEqual(
        );

        ysc.Subscriptions.AssertEqual(
            Subscribe(800, 900)
        );
    }

    [TestMethod]
    public void SelectMany_Triple_Error_Identity()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnError<int>(305, ex)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x => Seq.Return(x, Scheduler),
                ex1 => Seq.Throw<int>(ex1, Scheduler),
                () => Seq.Empty<int>(Scheduler)
            )
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(300), 0),
            OnNext(ScheduledAt(301), 1),
            OnNext(ScheduledAt(302), 2),
            OnNext(ScheduledAt(303), 3),
            OnNext(ScheduledAt(304), 4),
            OnError<int>(ScheduledAt(305), ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectMany_Triple_SelectMany()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x => Repeat(x, x),
                ex => Seq.Throw<int>(ex, Scheduler),
                () => Seq.Empty<int>(Scheduler)
            )
        );

        res.Messages.AssertEqual(
            OnNext(302, 1),
            OnNext(303, 2),
            OnNext(304, 3),
            OnNext(304, 2),
            OnNext(305, 4),
            OnNext(305, 3),
            OnNext(306, 4),
            OnNext(306, 3),
            OnNext(307, 4),
            OnNext(308, 4),
            OnCompleted<int>(308)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectMany_Triple_Concat()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x => Seq.Return(x, Scheduler),
                ex => Seq.Throw<int>(ex, Scheduler),
                () => Seq.Range(1, 3, Scheduler)
            )
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(300), 0),
            OnNext(ScheduledAt(301), 1),
            OnNext(ScheduledAt(302), 2),
            OnNext(ScheduledAt(303), 3),
            OnNext(ScheduledAt(304), 4),
            OnNext(ScheduledAt(305), 1),
            OnNext(ScheduledAt(305, 2), 2),
            OnNext(ScheduledAt(305, 3), 3),
            OnCompleted<int>(ScheduledAt(305, 4))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectMany_Triple_Catch()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x => Seq.Return(x, Scheduler),
                ex => Seq.Range(1, 3, Scheduler),
                () => Seq.Empty<int>(Scheduler)
            )
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(300), 0),
            OnNext(ScheduledAt(301), 1),
            OnNext(ScheduledAt(302), 2),
            OnNext(ScheduledAt(303), 3),
            OnNext(ScheduledAt(304), 4),
            OnCompleted<int>(ScheduledAt(305))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectMany_Triple_Error_Catch()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnError<int>(305, new Exception())
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x => Seq.Return(x, Scheduler),
                ex => Seq.Range(1, 3, Scheduler),
                () => Seq.Empty<int>(Scheduler)
            )
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(300), 0),
            OnNext(ScheduledAt(301), 1),
            OnNext(ScheduledAt(302), 2),
            OnNext(ScheduledAt(303), 3),
            OnNext(ScheduledAt(304), 4),
            OnNext(ScheduledAt(305), 1),
            OnNext(ScheduledAt(305, 2), 2),
            OnNext(ScheduledAt(305, 3), 3),
            OnCompleted<int>(ScheduledAt(305, 4))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectMany_Triple_All()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x => Repeat(x, x),
                ex => Repeat(0, 2),
                () => Repeat(-1, 2)
            )
        );

        res.Messages.AssertEqual(
            OnNext(302, 1),
            OnNext(303, 2),
            OnNext(304, 3),
            OnNext(304, 2),
            OnNext(305, 4),
            OnNext(305, 3),
            OnNext(306, -1),
            OnNext(306, 4),
            OnNext(306, 3),
            OnNext(307, -1),
            OnNext(307, 4),
            OnNext(308, 4),
            OnCompleted<int>(308)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectMany_Triple_Error_All()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnError<int>(305, new Exception())
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x => Repeat(x, x),
                ex => Repeat(0, 2),
                () => Repeat(-1, 2)
            )
        );

        res.Messages.AssertEqual(
            OnNext(302, 1),
            OnNext(303, 2),
            OnNext(304, 3),
            OnNext(304, 2),
            OnNext(305, 4),
            OnNext(305, 3),
            OnNext(306, 0),
            OnNext(306, 4),
            OnNext(306, 3),
            OnNext(307, 0),
            OnNext(307, 4),
            OnNext(308, 4),
            OnCompleted<int>(308)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectMany_Triple_All_Dispose()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x => Repeat(x, x),
                ex => Repeat(0, 2),
                () => Repeat(-1, 2)
            ),
            307
        );

        res.Messages.AssertEqual(
            OnNext(302, 1),
            OnNext(303, 2),
            OnNext(304, 3),
            OnNext(304, 2),
            OnNext(305, 4),
            OnNext(305, 3),
            OnNext(306, -1),
            OnNext(306, 4),
            OnNext(306, 3)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectMany_Triple_All_Dispose_Before_First()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x => Repeat(x, x),
                ex => Repeat(0, 2),
                () => Repeat(-1, 2)
            ),
            304
        );

        res.Messages.AssertEqual(
            OnNext(302, 1),
            OnNext(303, 2)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 304)
        );
    }

    [TestMethod]
    public void SelectMany_Triple_OnNextThrow()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x => Throw<Seq<int>>(ex),
                ex1 => Repeat(0, 2),
                () => Repeat(-1, 2)
            )
        );

        res.Messages.AssertEqual(
            OnError<int>(300, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void SelectMany_Triple_OnErrorThrow()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnError<int>(305, new Exception())
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x => Repeat(x, x),
                ex1 => Throw<Seq<int>>(ex),
                () => Repeat(-1, 2)
            )
        );

        res.Messages.AssertEqual(
            OnNext(302, 1),
            OnNext(303, 2),
            OnNext(304, 3),
            OnNext(304, 2),
            OnError<int>(305, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectMany_Triple_OnCompletedThrow()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x => Repeat(x, x),
                ex1 => Repeat(0, 2),
                () => Throw<Seq<int>>(ex)
            )
        );

        res.Messages.AssertEqual(
            OnNext(302, 1),
            OnNext(303, 2),
            OnNext(304, 3),
            OnNext(304, 2),
            OnError<int>(305, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Triple_Index()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var witness = new { x = 0, i = 0 };

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, i) => Seq.Return(new { x, i }, Scheduler),
                ex => Seq.Throw(ex, Scheduler, witness),
                () => Seq.Empty(Scheduler, witness)
            )
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(300), new { x = 0, i = 0 }),
            OnNext(ScheduledAt(301), new { x = 1, i = 1 }),
            OnNext(ScheduledAt(302), new { x = 2, i = 2 }),
            OnNext(ScheduledAt(303), new { x = 3, i = 3 }),
            OnNext(ScheduledAt(304), new { x = 4, i = 4 }),
            OnCompleted(ScheduledAt(305), witness)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Triple_Identity()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => Seq.Return(x, Scheduler),
                ex => Seq.Throw<int>(ex, Scheduler),
                () => Seq.Empty<int>(Scheduler)
            )
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(300), 0),
            OnNext(ScheduledAt(301), 1),
            OnNext(ScheduledAt(302), 2),
            OnNext(ScheduledAt(303), 3),
            OnNext(ScheduledAt(304), 4),
            OnCompleted<int>(ScheduledAt(305))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Triple_InnersWithTiming1()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var ysn = Scheduler.CreateColdObservable(
            OnNext(10, 10),
            OnNext(20, 11),
            OnNext(30, 12),
            OnCompleted<int>(40)
        );

        var yse = Scheduler.CreateColdObservable(
            OnNext(0, 99),
            OnCompleted<int>(10)
        );

        var ysc = Scheduler.CreateColdObservable(
            OnNext(10, 42),
            OnCompleted<int>(20)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => ysn,
                ex => yse,
                () => ysc
            )
        );

        res.Messages.AssertEqual(
            OnNext(310, 10),
            OnNext(311, 10),
            OnNext(312, 10),
            OnNext(313, 10),
            OnNext(314, 10),
            OnNext(315, 42),
            OnNext(320, 11),
            OnNext(321, 11),
            OnNext(322, 11),
            OnNext(323, 11),
            OnNext(324, 11),
            OnNext(330, 12),
            OnNext(331, 12),
            OnNext(332, 12),
            OnNext(333, 12),
            OnNext(334, 12),
            OnCompleted<int>(344)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );

        ysn.Subscriptions.AssertEqual(
            Subscribe(300, 340),
            Subscribe(301, 341),
            Subscribe(302, 342),
            Subscribe(303, 343),
            Subscribe(304, 344)
        );

        yse.Subscriptions.AssertEqual(
        );

        ysc.Subscriptions.AssertEqual(
            Subscribe(305, 325)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Triple_InnersWithTiming2()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var ysn = Scheduler.CreateColdObservable(
            OnNext(10, 10),
            OnNext(20, 11),
            OnNext(30, 12),
            OnCompleted<int>(40)
        );

        var yse = Scheduler.CreateColdObservable(
            OnNext(0, 99),
            OnCompleted<int>(10)
        );

        var ysc = Scheduler.CreateColdObservable(
            OnNext(10, 42),
            OnCompleted<int>(50)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => ysn,
                ex => yse,
                () => ysc
            )
        );

        res.Messages.AssertEqual(
            OnNext(310, 10),
            OnNext(311, 10),
            OnNext(312, 10),
            OnNext(313, 10),
            OnNext(314, 10),
            OnNext(315, 42),
            OnNext(320, 11),
            OnNext(321, 11),
            OnNext(322, 11),
            OnNext(323, 11),
            OnNext(324, 11),
            OnNext(330, 12),
            OnNext(331, 12),
            OnNext(332, 12),
            OnNext(333, 12),
            OnNext(334, 12),
            OnCompleted<int>(355)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );

        ysn.Subscriptions.AssertEqual(
            Subscribe(300, 340),
            Subscribe(301, 341),
            Subscribe(302, 342),
            Subscribe(303, 343),
            Subscribe(304, 344)
        );

        yse.Subscriptions.AssertEqual(
        );

        ysc.Subscriptions.AssertEqual(
            Subscribe(305, 355)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Triple_InnersWithTiming3()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(400, 1),
            OnNext(500, 2),
            OnNext(600, 3),
            OnNext(700, 4),
            OnCompleted<int>(800)
        );

        var ysn = Scheduler.CreateColdObservable(
            OnNext(10, 10),
            OnNext(20, 11),
            OnNext(30, 12),
            OnCompleted<int>(40)
        );

        var yse = Scheduler.CreateColdObservable(
            OnNext(0, 99),
            OnCompleted<int>(10)
        );

        var ysc = Scheduler.CreateColdObservable(
            OnNext(10, 42),
            OnCompleted<int>(100)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => ysn,
                ex => yse,
                () => ysc
            )
        );

        res.Messages.AssertEqual(
            OnNext(310, 10),
            OnNext(320, 11),
            OnNext(330, 12),
            OnNext(410, 10),
            OnNext(420, 11),
            OnNext(430, 12),
            OnNext(510, 10),
            OnNext(520, 11),
            OnNext(530, 12),
            OnNext(610, 10),
            OnNext(620, 11),
            OnNext(630, 12),
            OnNext(710, 10),
            OnNext(720, 11),
            OnNext(730, 12),
            OnNext(810, 42),
            OnCompleted<int>(900)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 800)
        );

        ysn.Subscriptions.AssertEqual(
            Subscribe(300, 340),
            Subscribe(400, 440),
            Subscribe(500, 540),
            Subscribe(600, 640),
            Subscribe(700, 740)
        );

        yse.Subscriptions.AssertEqual(
        );

        ysc.Subscriptions.AssertEqual(
            Subscribe(800, 900)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Triple_Error_Identity()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnError<int>(305, ex)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => Seq.Return(x, Scheduler),
                ex1 => Seq.Throw<int>(ex1, Scheduler),
                () => Seq.Empty<int>(Scheduler)
            )
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(300), 0),
            OnNext(ScheduledAt(301), 1),
            OnNext(ScheduledAt(302), 2),
            OnNext(ScheduledAt(303), 3),
            OnNext(ScheduledAt(304), 4),
            OnError<int>(ScheduledAt(305), ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Triple_SelectMany()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => Repeat(x, x),
                ex => Seq.Throw<int>(ex, Scheduler),
                () => Seq.Empty<int>(Scheduler)
            )
        );

        res.Messages.AssertEqual(
            OnNext(302, 1),
            OnNext(303, 2),
            OnNext(304, 3),
            OnNext(304, 2),
            OnNext(305, 4),
            OnNext(305, 3),
            OnNext(306, 4),
            OnNext(306, 3),
            OnNext(307, 4),
            OnNext(308, 4),
            OnCompleted<int>(308)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Triple_Concat()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => Seq.Return(x, Scheduler),
                ex => Seq.Throw<int>(ex, Scheduler),
                () => Seq.Range(1, 3, Scheduler)
            )
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(300), 0),
            OnNext(ScheduledAt(301), 1),
            OnNext(ScheduledAt(302), 2),
            OnNext(ScheduledAt(303), 3),
            OnNext(ScheduledAt(304), 4),
            OnNext(ScheduledAt(305), 1),
            OnNext(ScheduledAt(305, 2), 2),
            OnNext(ScheduledAt(305, 3), 3),
            OnCompleted<int>(ScheduledAt(305, 4))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Triple_Catch()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => Seq.Return(x, Scheduler),
                ex => Seq.Range(1, 3, Scheduler),
                () => Seq.Empty<int>(Scheduler)
            )
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(300), 0),
            OnNext(ScheduledAt(301), 1),
            OnNext(ScheduledAt(302), 2),
            OnNext(ScheduledAt(303), 3),
            OnNext(ScheduledAt(304), 4),
            OnCompleted<int>(ScheduledAt(305))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Triple_Error_Catch()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnError<int>(305, new Exception())
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => Seq.Return(x, Scheduler),
                ex => Seq.Range(1, 3, Scheduler),
                () => Seq.Empty<int>(Scheduler)
            )
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(300), 0),
            OnNext(ScheduledAt(301), 1),
            OnNext(ScheduledAt(302), 2),
            OnNext(ScheduledAt(303), 3),
            OnNext(ScheduledAt(304), 4),
            OnNext(ScheduledAt(305), 1),
            OnNext(ScheduledAt(305, 2), 2),
            OnNext(ScheduledAt(305, 3), 3),
            OnCompleted<int>(ScheduledAt(305, 4))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Triple_All()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => Repeat(x, x),
                ex => Repeat(0, 2),
                () => Repeat(-1, 2)
            )
        );

        res.Messages.AssertEqual(
            OnNext(302, 1),
            OnNext(303, 2),
            OnNext(304, 3),
            OnNext(304, 2),
            OnNext(305, 4),
            OnNext(305, 3),
            OnNext(306, -1),
            OnNext(306, 4),
            OnNext(306, 3),
            OnNext(307, -1),
            OnNext(307, 4),
            OnNext(308, 4),
            OnCompleted<int>(308)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Triple_Error_All()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnError<int>(305, new Exception())
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => Repeat(x, x),
                ex => Repeat(0, 2),
                () => Repeat(-1, 2)
            )
        );

        res.Messages.AssertEqual(
            OnNext(302, 1),
            OnNext(303, 2),
            OnNext(304, 3),
            OnNext(304, 2),
            OnNext(305, 4),
            OnNext(305, 3),
            OnNext(306, 0),
            OnNext(306, 4),
            OnNext(306, 3),
            OnNext(307, 0),
            OnNext(307, 4),
            OnNext(308, 4),
            OnCompleted<int>(308)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Triple_All_Dispose()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => Repeat(x, x),
                ex => Repeat(0, 2),
                () => Repeat(-1, 2)
            ),
            307
        );

        res.Messages.AssertEqual(
            OnNext(302, 1),
            OnNext(303, 2),
            OnNext(304, 3),
            OnNext(304, 2),
            OnNext(305, 4),
            OnNext(305, 3),
            OnNext(306, -1),
            OnNext(306, 4),
            OnNext(306, 3)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Triple_All_Dispose_Before_First()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => Repeat(x, x),
                ex => Repeat(0, 2),
                () => Repeat(-1, 2)
            ),
            304
        );

        res.Messages.AssertEqual(
            OnNext(302, 1),
            OnNext(303, 2)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 304)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Triple_OnNextThrow()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => Throw<Seq<int>>(ex),
                ex1 => Repeat(0, 2),
                () => Repeat(-1, 2)
            )
        );

        res.Messages.AssertEqual(
            OnError<int>(300, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Triple_OnErrorThrow()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnError<int>(305, new Exception())
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => Repeat(x, x),
                ex1 => Throw<Seq<int>>(ex),
                () => Repeat(-1, 2)
            )
        );

        res.Messages.AssertEqual(
            OnNext(302, 1),
            OnNext(303, 2),
            OnNext(304, 3),
            OnNext(304, 2),
            OnError<int>(305, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Triple_OnCompletedThrow()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(300, 0),
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(303, 3),
            OnNext(304, 4),
            OnCompleted<int>(305)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => Repeat(x, x),
                ex1 => Repeat(0, 2),
                () => Throw<Seq<int>>(ex)
            )
        );

        res.Messages.AssertEqual(
            OnNext(302, 1),
            OnNext(303, 2),
            OnNext(304, 3),
            OnNext(304, 2),
            OnError<int>(305, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 305)
        );
    }

    [TestMethod]
    public void SelectMany_Enumerable_Complete()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var inners = new List<MockEnumerable<int>>();

        var res = Scheduler.Start(() =>
            xs.SelectMany(x =>
            {
                var ys = new MockEnumerable<int>(Scheduler, Enumerable.Repeat(x, x));
                inners.Add(ys);
                return ys;
            })
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(420, 3),
            OnNext(420, 3),
            OnNext(510, 2),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );

        Assert.AreEqual(4, inners.Count);

        inners[0].Enumerations.AssertEqual(
            new Enumeration(210, 210)
        );

        inners[1].Enumerations.AssertEqual(
            new Enumeration(340, 340)
        );

        inners[2].Enumerations.AssertEqual(
            new Enumeration(420, 420)
        );

        inners[3].Enumerations.AssertEqual(
            new Enumeration(510, 510)
        );
    }

    [TestMethod]
    public void SelectMany_Enumerable_Complete_ResultSelector()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(x => Enumerable.Repeat(x, x), (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnNext(210, 4),
            OnNext(210, 4),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(420, 6),
            OnNext(420, 6),
            OnNext(420, 6),
            OnNext(510, 4),
            OnNext(510, 4),
            OnCompleted<int>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void SelectMany_Enumerable_Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnError<int>(600, ex)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(x => Enumerable.Repeat(x, x))
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(420, 3),
            OnNext(420, 3),
            OnNext(510, 2),
            OnNext(510, 2),
            OnError<int>(600, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void SelectMany_Enumerable_Error_ResultSelector()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnError<int>(600, ex)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(x => Enumerable.Repeat(x, x), (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnNext(210, 4),
            OnNext(210, 4),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(420, 6),
            OnNext(420, 6),
            OnNext(420, 6),
            OnNext(510, 4),
            OnNext(510, 4),
            OnError<int>(600, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void SelectMany_Enumerable_Dispose()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(x => Enumerable.Repeat(x, x)),
            350
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(340, 4)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );
    }

    [TestMethod]
    public void SelectMany_Enumerable_Dispose_ResultSelector()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany(x => Enumerable.Repeat(x, x), (x, y) => x + y),
            350
        );

        res.Messages.AssertEqual(
            OnNext(210, 4),
            OnNext(210, 4),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );
    }

    [TestMethod]
    public void SelectMany_Enumerable_SelectorThrows()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var invoked = 0;
        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany(x =>
            {
                invoked++;
                if (invoked == 3)
                {
                    throw ex;
                }

                return Enumerable.Repeat(x, x);
            })
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(340, 4),
            OnError<int>(420, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );

        Assert.AreEqual(3, invoked);
    }

    [TestMethod]
    public void SelectMany_Enumerable_ResultSelectorThrows()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var ex = new Exception();

        var inners = new List<MockEnumerable<int>>();

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x =>
                {
                    var ys = new MockEnumerable<int>(Scheduler, Enumerable.Repeat(x, x));
                    inners.Add(ys);
                    return ys;
                },
                (x, y) =>
                {
                    if (x == 3)
                    {
                        throw ex;
                    }

                    return x + y;
                }
            )
        );

        res.Messages.AssertEqual(
            OnNext(210, 4),
            OnNext(210, 4),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8),
            OnError<int>(420, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );

        Assert.AreEqual(3, inners.Count);

        inners[0].Enumerations.AssertEqual(
            new Enumeration(210, 210)
        );

        inners[1].Enumerations.AssertEqual(
            new Enumeration(340, 340)
        );

        inners[2].Enumerations.AssertEqual(
            new Enumeration(420, 420)
        );
    }

    [TestMethod]
    public void SelectMany_Enumerable_ResultSelector_GetEnumeratorThrows()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany(x => new RogueEnumerable<int>(ex), (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void SelectMany_Enumerable_SelectorThrows_ResultSelector()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var invoked = 0;
        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x =>
                {
                    invoked++;
                    if (invoked == 3)
                    {
                        throw ex;
                    }

                    return Enumerable.Repeat(x, x);
                },
                (x, y) => x + y
            )
        );

        res.Messages.AssertEqual(
            OnNext(210, 4),
            OnNext(210, 4),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8),
            OnError<int>(420, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );

        Assert.AreEqual(3, invoked);
    }

    [TestMethod]
    public void SelectMany_Enumerable_CurrentThrows()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany(x => new CurrentThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void SelectMany_Enumerable_CurrentThrows_ResultSelector()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x => new CurrentThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex),
                (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void SelectMany_Enumerable_GetEnumeratorThrows()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany(x => new RogueEnumerable<int>(ex))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void SelectMany_Enumerable_MoveNextThrows()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany(x => new MoveNextThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void SelectMany_Enumerable_MoveNextThrows_ResultSelector()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                x => new MoveNextThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex),
                (x, y) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Enumerable_Index()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 4),
            OnNext(220, 3),
            OnNext(250, 5),
            OnNext(270, 1),
            OnCompleted<int>(290)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, i) => new[] { new { x, i } })
        );

        var witness = new { x = 0, i = 0 };

        res.Messages.AssertEqual(
            OnNext(210, new { x = 4, i = 0 }),
            OnNext(220, new { x = 3, i = 1 }),
            OnNext(250, new { x = 5, i = 2 }),
            OnNext(270, new { x = 1, i = 3 }),
            OnCompleted(290, witness)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 290)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Enumerable_ResultSelector_Index()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 4),
            OnNext(220, 3),
            OnNext(250, 5),
            OnNext(270, 1),
            OnCompleted<int>(290)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, i) => Enumerable.Range(10, i + 1), (x, i, y, j) => new { x, i, y, j })
        );

        var witness = new { x = 0, i = 0, y = 0, j = 0 };

        res.Messages.AssertEqual(
            OnNext(210, new { x = 4, i = 0, y = 10, j = 0 }),
            OnNext(220, new { x = 3, i = 1, y = 10, j = 0 }),
            OnNext(220, new { x = 3, i = 1, y = 11, j = 1 }),
            OnNext(250, new { x = 5, i = 2, y = 10, j = 0 }),
            OnNext(250, new { x = 5, i = 2, y = 11, j = 1 }),
            OnNext(250, new { x = 5, i = 2, y = 12, j = 2 }),
            OnNext(270, new { x = 1, i = 3, y = 10, j = 0 }),
            OnNext(270, new { x = 1, i = 3, y = 11, j = 1 }),
            OnNext(270, new { x = 1, i = 3, y = 12, j = 2 }),
            OnNext(270, new { x = 1, i = 3, y = 13, j = 3 }),
            OnCompleted(290, witness)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 290)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Enumerable_Complete()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var inners = new List<MockEnumerable<int>>();

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) =>
            {
                var ys = new MockEnumerable<int>(Scheduler, Enumerable.Repeat(x, x));
                inners.Add(ys);
                return ys;
            })
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(420, 3),
            OnNext(420, 3),
            OnNext(510, 2),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );

        Assert.AreEqual(4, inners.Count);

        inners[0].Enumerations.AssertEqual(
            new Enumeration(210, 210)
        );

        inners[1].Enumerations.AssertEqual(
            new Enumeration(340, 340)
        );

        inners[2].Enumerations.AssertEqual(
            new Enumeration(420, 420)
        );

        inners[3].Enumerations.AssertEqual(
            new Enumeration(510, 510)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Enumerable_Complete_ResultSelector()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) => Enumerable.Repeat(x, x), (x, _, y, __) => x + y)
        );

        res.Messages.AssertEqual(
            OnNext(210, 4),
            OnNext(210, 4),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(420, 6),
            OnNext(420, 6),
            OnNext(420, 6),
            OnNext(510, 4),
            OnNext(510, 4),
            OnCompleted<int>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Enumerable_Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnError<int>(600, ex)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) => Enumerable.Repeat(x, x))
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(420, 3),
            OnNext(420, 3),
            OnNext(510, 2),
            OnNext(510, 2),
            OnError<int>(600, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Enumerable_Error_ResultSelector()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnError<int>(600, ex)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) => Enumerable.Repeat(x, x), (x, _, y, __) => x + y)
        );

        res.Messages.AssertEqual(
            OnNext(210, 4),
            OnNext(210, 4),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(420, 6),
            OnNext(420, 6),
            OnNext(420, 6),
            OnNext(510, 4),
            OnNext(510, 4),
            OnError<int>(600, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Enumerable_Dispose()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) => Enumerable.Repeat(x, x)),
            350
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(340, 4)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Enumerable_Dispose_ResultSelector()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) => Enumerable.Repeat(x, x), (x, _, y, __) => x + y),
            350
        );

        res.Messages.AssertEqual(
            OnNext(210, 4),
            OnNext(210, 4),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Enumerable_SelectorThrows()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var invoked = 0;
        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) =>
            {
                invoked++;
                if (invoked == 3)
                {
                    throw ex;
                }

                return Enumerable.Repeat(x, x);
            })
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(340, 4),
            OnNext(340, 4),
            OnError<int>(420, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );

        Assert.AreEqual(3, invoked);
    }

    [TestMethod]
    public void SelectManyWithIndex_Enumerable_ResultSelectorThrows()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var ex = new Exception();

        var inners = new List<MockEnumerable<int>>();

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) =>
                {
                    var ys = new MockEnumerable<int>(Scheduler, Enumerable.Repeat(x, x));
                    inners.Add(ys);
                    return ys;
                },
                (x, _, y, __) =>
                {
                    if (x == 3)
                    {
                        throw ex;
                    }

                    return x + y;
                }
            )
        );

        res.Messages.AssertEqual(
            OnNext(210, 4),
            OnNext(210, 4),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8),
            OnError<int>(420, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );

        Assert.AreEqual(3, inners.Count);

        inners[0].Enumerations.AssertEqual(
            new Enumeration(210, 210)
        );

        inners[1].Enumerations.AssertEqual(
            new Enumeration(340, 340)
        );

        inners[2].Enumerations.AssertEqual(
            new Enumeration(420, 420)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Enumerable_ResultSelector_GetEnumeratorThrows()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) => new RogueEnumerable<int>(ex), (x, _, y, __) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Enumerable_SelectorThrows_ResultSelector()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var invoked = 0;
        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) =>
                {
                    invoked++;
                    if (invoked == 3)
                    {
                        throw ex;
                    }

                    return Enumerable.Repeat(x, x);
                },
                (x, _, y, __) => x + y
            )
        );

        res.Messages.AssertEqual(
            OnNext(210, 4),
            OnNext(210, 4),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8),
            OnNext(340, 8),
            OnError<int>(420, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );

        Assert.AreEqual(3, invoked);
    }

    [TestMethod]
    public void SelectManyWithIndex_Enumerable_CurrentThrows()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) => new CurrentThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Enumerable_CurrentThrows_ResultSelector()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => new CurrentThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex),
                (x, _, y, __) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Enumerable_GetEnumeratorThrows()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) => new RogueEnumerable<int>(ex))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Enumerable_MoveNextThrows()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany((x, _) => new MoveNextThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void SelectManyWithIndex_Enumerable_MoveNextThrows_ResultSelector()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 2),
            OnNext(340, 4),
            OnNext(420, 3),
            OnNext(510, 2),
            OnCompleted<int>(600)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SelectMany(
                (x, _) => new MoveNextThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex),
                (x, _, y, __) => x + y)
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }
}
