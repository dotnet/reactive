// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>SelectMany</c> scenarios, from Rx.NET's <c>SelectManyTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// The first part of the file: its observable-selector families (<c>SelectMany_Then_*</c>, the
/// plain and <c>WithIndex</c> selector tests, and both <c>QueryOperator</c> families), 39 of its
/// 181 tests. The remaining families, over enumerable selectors, the three-selector form, and
/// task-returning selectors, are overloads AsyncRx.NET does not yet have and follow in later
/// steps; the five <c>*_ArgumentChecking</c> tests are the code-generated stratum.
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
}
