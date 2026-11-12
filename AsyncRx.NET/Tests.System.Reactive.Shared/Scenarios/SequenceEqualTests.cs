// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>SequenceEqual</c> scenarios, from Rx.NET's <c>SequenceEqualTest.cs</c>.</summary>
/// <remarks>
/// 27 of the file's 28 tests (<c>SequenceEqual_ArgumentChecking</c> is the code-generated
/// stratum), all transcribed as they are: the two-sequence form in both orders, with and
/// without a comparer, and the enumerable form with the second sequence as a collection, an
/// iterator that throws part-way (the file's own <c>Throw</c>) and a
/// <see cref="RogueEnumerable{T}"/>. The file's three comparers are top-level doubles under
/// their names, but for its <c>ThrowingComparer</c>, which is
/// <see cref="ThrowingEqualityComparer"/> here beside the suite's earlier
/// <see cref="ThrowingComparer{T}"/>.
/// </remarks>
public abstract class SequenceEqualTests : SharedReactiveTest
{
    [TestMethod]
    public void SequenceEqual_Observable_Equal()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(450, 7),
            OnCompleted<int>(510)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(270, 3),
            OnNext(280, 4),
            OnNext(300, 5),
            OnNext(330, 6),
            OnNext(340, 7),
            OnCompleted<int>(720)
        );

        var res = Scheduler.Start(() =>
            xs.SequenceEqual(ys)
        );

        res.Messages.AssertEqual(
            OnNext(720, true),
            OnCompleted<bool>(720)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 720)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 720)
        );
    }

    [TestMethod]
    public void SequenceEqual_Observable_Equal_Sym()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(450, 7),
            OnCompleted<int>(510)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(270, 3),
            OnNext(280, 4),
            OnNext(300, 5),
            OnNext(330, 6),
            OnNext(340, 7),
            OnCompleted<int>(720)
        );

        var res = Scheduler.Start(() =>
            ys.SequenceEqual(xs)
        );

        res.Messages.AssertEqual(
            OnNext(720, true),
            OnCompleted<bool>(720)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 720)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 720)
        );
    }

    [TestMethod]
    public void SequenceEqual_Observable_NotEqual_Left()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(310, 0),
            OnNext(340, 6),
            OnNext(450, 7),
            OnCompleted<int>(510)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(270, 3),
            OnNext(280, 4),
            OnNext(300, 5),
            OnNext(330, 6),
            OnNext(340, 7),
            OnCompleted<int>(720)
        );

        var res = Scheduler.Start(() =>
            xs.SequenceEqual(ys)
        );

        res.Messages.AssertEqual(
            OnNext(310, false),
            OnCompleted<bool>(310)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );
    }

    [TestMethod]
    public void SequenceEqual_Observable_NotEqual_Left_Sym()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(310, 0),
            OnNext(340, 6),
            OnNext(450, 7),
            OnCompleted<int>(510)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(270, 3),
            OnNext(280, 4),
            OnNext(300, 5),
            OnNext(330, 6),
            OnNext(340, 7),
            OnCompleted<int>(720)
        );

        var res = Scheduler.Start(() =>
            ys.SequenceEqual(xs)
        );

        res.Messages.AssertEqual(
            OnNext(310, false),
            OnCompleted<bool>(310)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );
    }

    [TestMethod]
    public void SequenceEqual_Observable_NotEqual_Right()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(450, 7),
            OnCompleted<int>(510)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(270, 3),
            OnNext(280, 4),
            OnNext(300, 5),
            OnNext(330, 6),
            OnNext(340, 7),
            OnNext(350, 8)
        );

        var res = Scheduler.Start(() =>
            xs.SequenceEqual(ys)
        );

        res.Messages.AssertEqual(
            OnNext(510, false),
            OnCompleted<bool>(510)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 510)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 510)
        );
    }

    [TestMethod]
    public void SequenceEqual_Observable_NotEqual_Right_Sym()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(450, 7),
            OnCompleted<int>(510)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(270, 3),
            OnNext(280, 4),
            OnNext(300, 5),
            OnNext(330, 6),
            OnNext(340, 7),
            OnNext(350, 8)
        );

        var res = Scheduler.Start(() =>
            ys.SequenceEqual(xs)
        );

        res.Messages.AssertEqual(
            OnNext(510, false),
            OnCompleted<bool>(510)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 510)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 510)
        );
    }

    [TestMethod]
    public void SequenceEqual_Observable_NotEqual_2()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(450, 7),
            OnNext(490, 8),
            OnNext(520, 9),
            OnNext(580, 10),
            OnNext(600, 11)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(270, 3),
            OnNext(280, 4),
            OnNext(300, 5),
            OnNext(330, 6),
            OnNext(340, 7),
            OnNext(350, 9),
            OnNext(400, 9),
            OnNext(410, 10),
            OnNext(490, 11),
            OnNext(550, 12),
            OnNext(560, 13)
        );

        var res = Scheduler.Start(() =>
            xs.SequenceEqual(ys)
        );

        res.Messages.AssertEqual(
            OnNext(490, false),
            OnCompleted<bool>(490)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 490)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 490)
        );
    }

    [TestMethod]
    public void SequenceEqual_Observable_NotEqual_2_Sym()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(450, 7),
            OnNext(490, 8),
            OnNext(520, 9),
            OnNext(580, 10),
            OnNext(600, 11)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(270, 3),
            OnNext(280, 4),
            OnNext(300, 5),
            OnNext(330, 6),
            OnNext(340, 7),
            OnNext(350, 9),
            OnNext(400, 9),
            OnNext(410, 10),
            OnNext(490, 11),
            OnNext(550, 12),
            OnNext(560, 13)
        );

        var res = Scheduler.Start(() =>
            ys.SequenceEqual(xs)
        );

        res.Messages.AssertEqual(
            OnNext(490, false),
            OnCompleted<bool>(490)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 490)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 490)
        );
    }

    [TestMethod]
    public void SequenceEqual_Observable_NotEqual_3()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(310, 5),
            OnCompleted<int>(330)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(270, 3),
            OnNext(400, 4),
            OnCompleted<int>(420)
        );

        var res = Scheduler.Start(() =>
            xs.SequenceEqual(ys)
        );

        res.Messages.AssertEqual(
            OnNext(420, false),
            OnCompleted<bool>(420)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public void SequenceEqual_Observable_NotEqual_3_Sym()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(310, 5),
            OnCompleted<int>(330)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(270, 3),
            OnNext(400, 4),
            OnCompleted<int>(420)
        );

        var res = Scheduler.Start(() =>
            ys.SequenceEqual(xs)
        );

        res.Messages.AssertEqual(
            OnNext(420, false),
            OnCompleted<bool>(420)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public void SequenceEqual_Observable_ComparerThrows()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(310, 5),
            OnCompleted<int>(330)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(270, 3),
            OnNext(400, 4),
            OnCompleted<int>(420)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SequenceEqual(ys, new ThrowComparer(ex))
        );

        res.Messages.AssertEqual(
            OnError<bool>(270, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 270)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 270)
        );
    }

    [TestMethod]
    public void SequenceEqual_Observable_ComparerThrows_Sym()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(310, 5),
            OnCompleted<int>(330)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(270, 3),
            OnNext(400, 4),
            OnCompleted<int>(420)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            ys.SequenceEqual(xs, new ThrowComparer(ex))
        );

        res.Messages.AssertEqual(
            OnError<bool>(270, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 270)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 270)
        );
    }

    [TestMethod]
    public void SequenceEqual_Observable_NotEqual_4()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(250, 1),
            OnCompleted<int>(300)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(290, 1),
            OnNext(310, 2),
            OnCompleted<int>(350)
        );

        var res = Scheduler.Start(() =>
            xs.SequenceEqual(ys)
        );

        res.Messages.AssertEqual(
            OnNext(310, false),
            OnCompleted<bool>(310)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );
    }

    [TestMethod]
    public void SequenceEqual_Observable_NotEqual_4_Sym()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(250, 1),
            OnCompleted<int>(300)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(290, 1),
            OnNext(310, 2),
            OnCompleted<int>(350)
        );

        var res = Scheduler.Start(() =>
            ys.SequenceEqual(xs)
        );

        res.Messages.AssertEqual(
            OnNext(310, false),
            OnCompleted<bool>(310)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );
    }

    [TestMethod]
    public void SequenceEqual_Observable_Left_Throw()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(250, 1),
            OnError<int>(300, ex)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(290, 1),
            OnNext(310, 2),
            OnCompleted<int>(350)
        );

        var res = Scheduler.Start(() =>
            ys.SequenceEqual(xs)
        );

        res.Messages.AssertEqual(
            OnError<bool>(300, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void SequenceEqual_Observable_Right_Throw()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(290, 1),
            OnNext(310, 2),
            OnCompleted<int>(350)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(250, 1),
            OnError<int>(300, ex)
        );

        var res = Scheduler.Start(() =>
            ys.SequenceEqual(xs)
        );

        res.Messages.AssertEqual(
            OnError<bool>(300, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void SequenceEqual_Enumerable_Equal()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(450, 7),
            OnCompleted<int>(510)
        );

        var res = Scheduler.Start(() =>
            xs.SequenceEqual([3, 4, 5, 6, 7])
        );

        res.Messages.AssertEqual(
            OnNext(510, true),
            OnCompleted<bool>(510)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 510)
        );
    }

    [TestMethod]
    public void SequenceEqual_Enumerable_NotEqual_Elements()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(450, 7),
            OnCompleted<int>(510)
        );

        var res = Scheduler.Start(() =>
            xs.SequenceEqual([3, 4, 9, 6, 7])
        );

        res.Messages.AssertEqual(
            OnNext(310, false),
            OnCompleted<bool>(310)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );
    }

    [TestMethod]
    public void SequenceEqual_Enumerable_Comparer_Equal()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(450, 7),
            OnCompleted<int>(510)
        );

        var res = Scheduler.Start(() =>
            xs.SequenceEqual([3 - 2, 4, 5, 6 + 42, 7 - 6], new OddEvenComparer())
        );

        res.Messages.AssertEqual(
            OnNext(510, true),
            OnCompleted<bool>(510)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 510)
        );
    }

    [TestMethod]
    public void SequenceEqual_Enumerable_Comparer_NotEqual()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(450, 7),
            OnCompleted<int>(510)
        );

        var res = Scheduler.Start(() =>
            xs.SequenceEqual([3 - 2, 4, 5 + 9, 6 + 42, 7 - 6], new OddEvenComparer())
        );

        res.Messages.AssertEqual(
            OnNext(310, false),
            OnCompleted<bool>(310)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );
    }

    [TestMethod]
    public void SequenceEqual_Enumerable_Comparer_Throws()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(450, 7),
            OnCompleted<int>(510)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.SequenceEqual([3, 4, 5, 6, 7], new ThrowingEqualityComparer(5, ex))
        );

        res.Messages.AssertEqual(
            OnError<bool>(310, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );
    }

    [TestMethod]
    public void SequenceEqual_Enumerable_NotEqual_TooLong()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(450, 7),
            OnCompleted<int>(510)
        );

        var res = Scheduler.Start(() =>
            xs.SequenceEqual([3, 4, 5, 6, 7, 8])
        );

        res.Messages.AssertEqual(
            OnNext(510, false),
            OnCompleted<bool>(510)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 510)
        );
    }

    [TestMethod]
    public void SequenceEqual_Enumerable_NotEqual_TooShort()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(450, 7),
            OnCompleted<int>(510)
        );

        var res = Scheduler.Start(() =>
            xs.SequenceEqual([3, 4, 5, 6])
        );

        res.Messages.AssertEqual(
            OnNext(450, false),
            OnCompleted<bool>(450)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450)
        );
    }

    [TestMethod]
    public void SequenceEqual_Enumerable_OnError()
    {
        var ex = new Exception();
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnError<int>(310, ex)
        );

        var res = Scheduler.Start(() =>
            xs.SequenceEqual([3, 4])
        );

        res.Messages.AssertEqual(
            OnError<bool>(310, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );
    }

    [TestMethod]
    public void SequenceEqual_Enumerable_IteratorThrows1()
    {
        var ex = new Exception();
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnCompleted<int>(310)
        );

        var res = Scheduler.Start(() =>
            xs.SequenceEqual(Throw(ex))
        );

        res.Messages.AssertEqual(
            OnError<bool>(290, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 290)
        );
    }

    [TestMethod]
    public void SequenceEqual_Enumerable_IteratorThrows2()
    {
        var ex = new Exception();
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnCompleted<int>(310)
        );

        var res = Scheduler.Start(() =>
            xs.SequenceEqual(Throw(ex))
        );

        res.Messages.AssertEqual(
            OnError<bool>(310, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );
    }

    [TestMethod]
    public void SequenceEqual_Enumerable_GetEnumeratorThrows()
    {
        var ex = new Exception();
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 1),
            OnNext(190, 2),
            OnNext(240, 3),
            OnCompleted<int>(310)
        );

        var res = Scheduler.Start(() =>
            xs.SequenceEqual(new RogueEnumerable<int>(ex))
        );

        res.Messages.AssertEqual(
            OnError<bool>(200, ex)
        );

        xs.Subscriptions.AssertEqual(
        );
    }

    private static IEnumerable<int> Throw(Exception ex)
    {
        yield return 3;
        throw ex;
    }
}
