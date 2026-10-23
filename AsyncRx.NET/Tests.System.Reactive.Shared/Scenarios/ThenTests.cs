// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Then</c> scenarios, from Rx.NET's <c>ThenTest.cs</c>.</summary>
/// <remarks>
/// 18 of the file's 19 tests (<c>Then_ArgumentChecking</c> is the code-generated stratum), all
/// transcribed as they are: a one-source plan over a value, an error and a throwing selector,
/// then a throwing selector over patterns of two to sixteen sources. The <c>Then&lt;int&gt;</c>
/// and <c>Then&lt;int, int&gt;</c> spellings with explicit type arguments carry over, since the
/// pattern's <c>Then</c> and the single-sequence extension take the same type parameters as
/// Rx.NET's.
/// </remarks>
public abstract class ThenTests : SharedReactiveTest
{
    [TestMethod]
    public void Then1()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnCompleted<int>(220)
        );

        var res = scheduler.Start(() =>
            Seq.When(xs.Then(a => a))
        );

        res.Messages.AssertEqual(
            OnNext(210, 1),
            OnCompleted<int>(220)
        );
    }

    [TestMethod]
    public void Then1Error()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            Seq.When(xs.Then(a => a))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }

    [TestMethod]
    public void Then1Throws()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnCompleted<int>(220)
        );

        var res = scheduler.Start(() =>
            Seq.When(xs.Then<int, int>(a => { throw ex; }))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }

    [TestMethod]
    public void Then2Throws()
    {
        var scheduler = Scheduler;
        var ex = new Exception();

        const int N = 2;

        var obs = new List<Seq<int>>();
        for (var i = 0; i < N; i++)
        {
            obs.Add(scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(220)
            ));
        }

        var res = scheduler.Start(() =>
            Seq.When(obs[0].And(obs[1]).Then<int>((a, b) => { throw ex; }))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }

    [TestMethod]
    public void Then3Throws()
    {
        var scheduler = Scheduler;
        var ex = new Exception();

        const int N = 3;

        var obs = new List<Seq<int>>();
        for (var i = 0; i < N; i++)
        {
            obs.Add(scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(220)
            ));
        }

        var res = scheduler.Start(() =>
            Seq.When(obs[0].And(obs[1]).And(obs[2]).Then<int>((a, b, c) => { throw ex; }))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }

    [TestMethod]
    public void Then4Throws()
    {
        var scheduler = Scheduler;
        var ex = new Exception();

        const int N = 4;

        var obs = new List<Seq<int>>();
        for (var i = 0; i < N; i++)
        {
            obs.Add(scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(220)
            ));
        }

        var res = scheduler.Start(() =>
            Seq.When(
                obs[0].And(obs[1]).And(obs[2]).And(obs[3])
                    .Then<int>((a, b, c, d) => { throw ex; }))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }

    [TestMethod]
    public void Then5Throws()
    {
        var scheduler = Scheduler;
        var ex = new Exception();

        const int N = 5;

        var obs = new List<Seq<int>>();
        for (var i = 0; i < N; i++)
        {
            obs.Add(scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(220)
            ));
        }

        var res = scheduler.Start(() =>
            Seq.When(
                obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4])
                    .Then<int>((a, b, c, d, e) => { throw ex; }))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }

    [TestMethod]
    public void Then6Throws()
    {
        var scheduler = Scheduler;
        var ex = new Exception();

        const int N = 6;

        var obs = new List<Seq<int>>();
        for (var i = 0; i < N; i++)
        {
            obs.Add(scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(220)
            ));
        }

        var res = scheduler.Start(() =>
            Seq.When(
                obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5])
                    .Then<int>((a, b, c, d, e, f) => { throw ex; }))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }

    [TestMethod]
    public void Then7Throws()
    {
        var scheduler = Scheduler;
        var ex = new Exception();

        const int N = 7;

        var obs = new List<Seq<int>>();
        for (var i = 0; i < N; i++)
        {
            obs.Add(scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(220)
            ));
        }

        var res = scheduler.Start(() =>
            Seq.When(
                obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                    .Then<int>((a, b, c, d, e, f, g) => { throw ex; }))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }

    [TestMethod]
    public void Then8Throws()
    {
        var scheduler = Scheduler;
        var ex = new Exception();

        const int N = 8;

        var obs = new List<Seq<int>>();
        for (var i = 0; i < N; i++)
        {
            obs.Add(scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(220)
            ));
        }

        var res = scheduler.Start(() =>
            Seq.When(
                obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                    .And(obs[7])
                    .Then<int>((a, b, c, d, e, f, g, h) => { throw ex; }))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }

    [TestMethod]
    public void Then9Throws()
    {
        var scheduler = Scheduler;
        var ex = new Exception();

        const int N = 9;

        var obs = new List<Seq<int>>();
        for (var i = 0; i < N; i++)
        {
            obs.Add(scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(220)
            ));
        }

        var res = scheduler.Start(() =>
            Seq.When(
                obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                    .And(obs[7]).And(obs[8])
                    .Then<int>((a, b, c, d, e, f, g, h, i_) => { throw ex; }))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }

    [TestMethod]
    public void Then10Throws()
    {
        var scheduler = Scheduler;
        var ex = new Exception();

        const int N = 10;

        var obs = new List<Seq<int>>();
        for (var i = 0; i < N; i++)
        {
            obs.Add(scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(220)
            ));
        }

        var res = scheduler.Start(() =>
            Seq.When(
                obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                    .And(obs[7]).And(obs[8]).And(obs[9])
                    .Then<int>((a, b, c, d, e, f, g, h, i_, j) => { throw ex; }))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }

    [TestMethod]
    public void Then11Throws()
    {
        var scheduler = Scheduler;
        var ex = new Exception();

        const int N = 11;

        var obs = new List<Seq<int>>();
        for (var i = 0; i < N; i++)
        {
            obs.Add(scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(220)
            ));
        }

        var res = scheduler.Start(() =>
            Seq.When(
                obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                    .And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10])
                    .Then<int>((a, b, c, d, e, f, g, h, i_, j, k) => { throw ex; }))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }

    [TestMethod]
    public void Then12Throws()
    {
        var scheduler = Scheduler;
        var ex = new Exception();

        const int N = 12;

        var obs = new List<Seq<int>>();
        for (var i = 0; i < N; i++)
        {
            obs.Add(scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(220)
            ));
        }

        var res = scheduler.Start(() =>
            Seq.When(
                obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                    .And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11])
                    .Then<int>((a, b, c, d, e, f, g, h, i_, j, k, l) => { throw ex; }))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }

    [TestMethod]
    public void Then13Throws()
    {
        var scheduler = Scheduler;
        var ex = new Exception();

        const int N = 13;

        var obs = new List<Seq<int>>();
        for (var i = 0; i < N; i++)
        {
            obs.Add(scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(220)
            ));
        }

        var res = scheduler.Start(() =>
            Seq.When(
                obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                    .And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12])
                    .Then<int>((a, b, c, d, e, f, g, h, i_, j, k, l, m) => { throw ex; }))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }

    [TestMethod]
    public void Then14Throws()
    {
        var scheduler = Scheduler;
        var ex = new Exception();

        const int N = 14;

        var obs = new List<Seq<int>>();
        for (var i = 0; i < N; i++)
        {
            obs.Add(scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(220)
            ));
        }

        var res = scheduler.Start(() =>
            Seq.When(
                obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                    .And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12])
                    .And(obs[13])
                    .Then<int>((a, b, c, d, e, f, g, h, i_, j, k, l, m, n) => { throw ex; }))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }

    [TestMethod]
    public void Then15Throws()
    {
        var scheduler = Scheduler;
        var ex = new Exception();

        const int N = 15;

        var obs = new List<Seq<int>>();
        for (var i = 0; i < N; i++)
        {
            obs.Add(scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(220)
            ));
        }

        var res = scheduler.Start(() =>
            Seq.When(
                obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                    .And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12])
                    .And(obs[13]).And(obs[14])
                    .Then<int>((a, b, c, d, e, f, g, h, i_, j, k, l, m, n, o) => { throw ex; }))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }

    [TestMethod]
    public void Then16Throws()
    {
        var scheduler = Scheduler;
        var ex = new Exception();

        const int N = 16;

        var obs = new List<Seq<int>>();
        for (var i = 0; i < N; i++)
        {
            obs.Add(scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(220)
            ));
        }

        var res = scheduler.Start(() =>
            Seq.When(
                obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                    .And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12])
                    .And(obs[13]).And(obs[14]).And(obs[15])
                    .Then<int>((a, b, c, d, e, f, g, h, i_, j, k, l, m, n, o, p) => { throw ex; }))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }
}
