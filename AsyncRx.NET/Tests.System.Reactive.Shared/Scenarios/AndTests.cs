// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>And</c> scenarios, from Rx.NET's <c>AndTest.cs</c>.</summary>
/// <remarks>
/// 30 of the file's 31 tests (<c>And_ArgumentChecking</c> is the code-generated stratum), all
/// transcribed as they are: <c>And2</c> to <c>And16</c> join that many hot sources through one
/// plan whose selector sums them, and <c>And2Error</c> to <c>And16Error</c> let each source in
/// turn fail, on a scheduler of its own per case. The pattern chain
/// <c>obs[0].And(obs[1]).And(obs[2])...Then(...)</c> is the same text over descriptions, one
/// pattern type per arity; the longer chains are wrapped for width and nothing else.
/// </remarks>
public abstract class AndTests : SharedReactiveTest
{
    [TestMethod]
    public void And2()
    {
        var scheduler = Scheduler;

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
            Seq.When(obs[0].And(obs[1]).Then((a, b) => a + b))
        );

        res.Messages.AssertEqual(
            OnNext(210, N),
            OnCompleted<int>(220)
        );
    }

    [TestMethod]
    public void And2Error()
    {
        var ex = new Exception();

        const int N = 2;

        for (var i = 0; i < N; i++)
        {
            var scheduler = new TestSchedulerRef(Target);
            var obs = new List<Seq<int>>();

            for (var j = 0; j < N; j++)
            {
                if (j == i)
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnError<int>(210, ex)
                    ));
                }
                else
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnNext(210, 1),
                        OnCompleted<int>(220)
                    ));
                }
            }

            var res = scheduler.Start(() =>
                Seq.When(obs[0].And(obs[1]).Then((a, b) => 0))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }
    }

    [TestMethod]
    public void And3()
    {
        var scheduler = Scheduler;

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
            Seq.When(obs[0].And(obs[1]).And(obs[2]).Then((a, b, c) => a + b + c))
        );

        res.Messages.AssertEqual(
            OnNext(210, N),
            OnCompleted<int>(220)
        );
    }

    [TestMethod]
    public void And3Error()
    {
        var ex = new Exception();

        const int N = 3;

        for (var i = 0; i < N; i++)
        {
            var scheduler = new TestSchedulerRef(Target);
            var obs = new List<Seq<int>>();

            for (var j = 0; j < N; j++)
            {
                if (j == i)
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnError<int>(210, ex)
                    ));
                }
                else
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnNext(210, 1),
                        OnCompleted<int>(220)
                    ));
                }
            }

            var res = scheduler.Start(() =>
                Seq.When(obs[0].And(obs[1]).And(obs[2]).Then((a, b, c) => 0))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }
    }

    [TestMethod]
    public void And4()
    {
        var scheduler = Scheduler;

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
            Seq.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).Then((a, b, c, d) => a + b + c + d))
        );

        res.Messages.AssertEqual(
            OnNext(210, N),
            OnCompleted<int>(220)
        );
    }

    [TestMethod]
    public void And4Error()
    {
        var ex = new Exception();

        const int N = 4;

        for (var i = 0; i < N; i++)
        {
            var scheduler = new TestSchedulerRef(Target);
            var obs = new List<Seq<int>>();

            for (var j = 0; j < N; j++)
            {
                if (j == i)
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnError<int>(210, ex)
                    ));
                }
                else
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnNext(210, 1),
                        OnCompleted<int>(220)
                    ));
                }
            }

            var res = scheduler.Start(() =>
                Seq.When(obs[0].And(obs[1]).And(obs[2]).And(obs[3]).Then((a, b, c, d) => 0))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }
    }

    [TestMethod]
    public void And5()
    {
        var scheduler = Scheduler;

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
                    .Then((a, b, c, d, e) => a + b + c + d + e))
        );

        res.Messages.AssertEqual(
            OnNext(210, N),
            OnCompleted<int>(220)
        );
    }

    [TestMethod]
    public void And5Error()
    {
        var ex = new Exception();

        const int N = 5;

        for (var i = 0; i < N; i++)
        {
            var scheduler = new TestSchedulerRef(Target);
            var obs = new List<Seq<int>>();

            for (var j = 0; j < N; j++)
            {
                if (j == i)
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnError<int>(210, ex)
                    ));
                }
                else
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnNext(210, 1),
                        OnCompleted<int>(220)
                    ));
                }
            }

            var res = scheduler.Start(() =>
                Seq.When(
                    obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4])
                        .Then((a, b, c, d, e) => 0))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }
    }

    [TestMethod]
    public void And6()
    {
        var scheduler = Scheduler;

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
                    .Then((a, b, c, d, e, f) => a + b + c + d + e + f))
        );

        res.Messages.AssertEqual(
            OnNext(210, N),
            OnCompleted<int>(220)
        );
    }

    [TestMethod]
    public void And6Error()
    {
        var ex = new Exception();

        const int N = 6;

        for (var i = 0; i < N; i++)
        {
            var scheduler = new TestSchedulerRef(Target);
            var obs = new List<Seq<int>>();

            for (var j = 0; j < N; j++)
            {
                if (j == i)
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnError<int>(210, ex)
                    ));
                }
                else
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnNext(210, 1),
                        OnCompleted<int>(220)
                    ));
                }
            }

            var res = scheduler.Start(() =>
                Seq.When(
                    obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5])
                        .Then((a, b, c, d, e, f) => 0))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }
    }

    [TestMethod]
    public void And7()
    {
        var scheduler = Scheduler;

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
                    .Then((a, b, c, d, e, f, g) => a + b + c + d + e + f + g))
        );

        res.Messages.AssertEqual(
            OnNext(210, N),
            OnCompleted<int>(220)
        );
    }

    [TestMethod]
    public void And7Error()
    {
        var ex = new Exception();

        const int N = 7;

        for (var i = 0; i < N; i++)
        {
            var scheduler = new TestSchedulerRef(Target);
            var obs = new List<Seq<int>>();

            for (var j = 0; j < N; j++)
            {
                if (j == i)
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnError<int>(210, ex)
                    ));
                }
                else
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnNext(210, 1),
                        OnCompleted<int>(220)
                    ));
                }
            }

            var res = scheduler.Start(() =>
                Seq.When(
                    obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                        .Then((a, b, c, d, e, f, g) => 0))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }
    }

    [TestMethod]
    public void And8()
    {
        var scheduler = Scheduler;

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
                    .Then((a, b, c, d, e, f, g, h) => a + b + c + d + e + f + g + h))
        );

        res.Messages.AssertEqual(
            OnNext(210, N),
            OnCompleted<int>(220)
        );
    }

    [TestMethod]
    public void And8Error()
    {
        var ex = new Exception();

        const int N = 8;

        for (var i = 0; i < N; i++)
        {
            var scheduler = new TestSchedulerRef(Target);
            var obs = new List<Seq<int>>();

            for (var j = 0; j < N; j++)
            {
                if (j == i)
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnError<int>(210, ex)
                    ));
                }
                else
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnNext(210, 1),
                        OnCompleted<int>(220)
                    ));
                }
            }

            var res = scheduler.Start(() =>
                Seq.When(
                    obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                        .And(obs[7])
                        .Then((a, b, c, d, e, f, g, h) => 0))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }
    }

    [TestMethod]
    public void And9()
    {
        var scheduler = Scheduler;

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
                    .Then((a, b, c, d, e, f, g, h, i) => a + b + c + d + e + f + g + h + i))
        );

        res.Messages.AssertEqual(
            OnNext(210, N),
            OnCompleted<int>(220)
        );
    }

    [TestMethod]
    public void And9Error()
    {
        var ex = new Exception();

        const int N = 9;

        for (var i = 0; i < N; i++)
        {
            var scheduler = new TestSchedulerRef(Target);
            var obs = new List<Seq<int>>();

            for (var j = 0; j < N; j++)
            {
                if (j == i)
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnError<int>(210, ex)
                    ));
                }
                else
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnNext(210, 1),
                        OnCompleted<int>(220)
                    ));
                }
            }

            var res = scheduler.Start(() =>
                Seq.When(
                    obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                        .And(obs[7]).And(obs[8])
                        .Then((a, b, c, d, e, f, g, h, i_) => 0))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }
    }

    [TestMethod]
    public void And10()
    {
        var scheduler = Scheduler;

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
                    .Then((a, b, c, d, e, f, g, h, i, j) => a + b + c + d + e + f + g + h + i + j))
        );

        res.Messages.AssertEqual(
            OnNext(210, N),
            OnCompleted<int>(220)
        );
    }

    [TestMethod]
    public void And10Error()
    {
        var ex = new Exception();

        const int N = 10;

        for (var i = 0; i < N; i++)
        {
            var scheduler = new TestSchedulerRef(Target);
            var obs = new List<Seq<int>>();

            for (var j = 0; j < N; j++)
            {
                if (j == i)
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnError<int>(210, ex)
                    ));
                }
                else
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnNext(210, 1),
                        OnCompleted<int>(220)
                    ));
                }
            }

            var res = scheduler.Start(() =>
                Seq.When(
                    obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                        .And(obs[7]).And(obs[8]).And(obs[9])
                        .Then((a, b, c, d, e, f, g, h, i_, j) => 0))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }
    }

    [TestMethod]
    public void And11()
    {
        var scheduler = Scheduler;

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
                    .Then((a, b, c, d, e, f, g, h, i, j, k) =>
                        a + b + c + d + e + f + g + h + i + j + k))
        );

        res.Messages.AssertEqual(
            OnNext(210, N),
            OnCompleted<int>(220)
        );
    }

    [TestMethod]
    public void And11Error()
    {
        var ex = new Exception();

        const int N = 11;

        for (var i = 0; i < N; i++)
        {
            var scheduler = new TestSchedulerRef(Target);
            var obs = new List<Seq<int>>();

            for (var j = 0; j < N; j++)
            {
                if (j == i)
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnError<int>(210, ex)
                    ));
                }
                else
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnNext(210, 1),
                        OnCompleted<int>(220)
                    ));
                }
            }

            var res = scheduler.Start(() =>
                Seq.When(
                    obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                        .And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10])
                        .Then((a, b, c, d, e, f, g, h, i_, j, k) => 0))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }
    }

    [TestMethod]
    public void And12()
    {
        var scheduler = Scheduler;

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
                    .Then((a, b, c, d, e, f, g, h, i, j, k, l) =>
                        a + b + c + d + e + f + g + h + i + j + k + l))
        );

        res.Messages.AssertEqual(
            OnNext(210, N),
            OnCompleted<int>(220)
        );
    }

    [TestMethod]
    public void And12Error()
    {
        var ex = new Exception();

        const int N = 12;

        for (var i = 0; i < N; i++)
        {
            var scheduler = new TestSchedulerRef(Target);
            var obs = new List<Seq<int>>();

            for (var j = 0; j < N; j++)
            {
                if (j == i)
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnError<int>(210, ex)
                    ));
                }
                else
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnNext(210, 1),
                        OnCompleted<int>(220)
                    ));
                }
            }

            var res = scheduler.Start(() =>
                Seq.When(
                    obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                        .And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11])
                        .Then((a, b, c, d, e, f, g, h, i_, j, k, l) => 0))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }
    }

    [TestMethod]
    public void And13()
    {
        var scheduler = Scheduler;

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
                    .Then((a, b, c, d, e, f, g, h, i, j, k, l, m) =>
                        a + b + c + d + e + f + g + h + i + j + k + l + m))
        );

        res.Messages.AssertEqual(
            OnNext(210, N),
            OnCompleted<int>(220)
        );
    }

    [TestMethod]
    public void And13Error()
    {
        var ex = new Exception();

        const int N = 13;

        for (var i = 0; i < N; i++)
        {
            var scheduler = new TestSchedulerRef(Target);
            var obs = new List<Seq<int>>();

            for (var j = 0; j < N; j++)
            {
                if (j == i)
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnError<int>(210, ex)
                    ));
                }
                else
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnNext(210, 1),
                        OnCompleted<int>(220)
                    ));
                }
            }

            var res = scheduler.Start(() =>
                Seq.When(
                    obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                        .And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12])
                        .Then((a, b, c, d, e, f, g, h, i_, j, k, l, m) => 0))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }
    }

    [TestMethod]
    public void And14()
    {
        var scheduler = Scheduler;

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
                    .Then((a, b, c, d, e, f, g, h, i, j, k, l, m, n) =>
                        a + b + c + d + e + f + g + h + i + j + k + l + m + n))
        );

        res.Messages.AssertEqual(
            OnNext(210, N),
            OnCompleted<int>(220)
        );
    }

    [TestMethod]
    public void And14Error()
    {
        var ex = new Exception();

        const int N = 14;

        for (var i = 0; i < N; i++)
        {
            var scheduler = new TestSchedulerRef(Target);
            var obs = new List<Seq<int>>();

            for (var j = 0; j < N; j++)
            {
                if (j == i)
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnError<int>(210, ex)
                    ));
                }
                else
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnNext(210, 1),
                        OnCompleted<int>(220)
                    ));
                }
            }

            var res = scheduler.Start(() =>
                Seq.When(
                    obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                        .And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12])
                        .And(obs[13])
                        .Then((a, b, c, d, e, f, g, h, i_, j, k, l, m, n) => 0))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }
    }

    [TestMethod]
    public void And15()
    {
        var scheduler = Scheduler;

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
                    .Then((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o) =>
                        a + b + c + d + e + f + g + h + i + j + k + l + m + n + o))
        );

        res.Messages.AssertEqual(
            OnNext(210, N),
            OnCompleted<int>(220)
        );
    }

    [TestMethod]
    public void And15Error()
    {
        var ex = new Exception();

        const int N = 15;

        for (var i = 0; i < N; i++)
        {
            var scheduler = new TestSchedulerRef(Target);
            var obs = new List<Seq<int>>();

            for (var j = 0; j < N; j++)
            {
                if (j == i)
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnError<int>(210, ex)
                    ));
                }
                else
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnNext(210, 1),
                        OnCompleted<int>(220)
                    ));
                }
            }

            var res = scheduler.Start(() =>
                Seq.When(
                    obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                        .And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12])
                        .And(obs[13]).And(obs[14])
                        .Then((a, b, c, d, e, f, g, h, i_, j, k, l, m, n, o) => 0))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }
    }

    [TestMethod]
    public void And16()
    {
        var scheduler = Scheduler;

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
                    .Then((a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p) =>
                        a + b + c + d + e + f + g + h + i + j + k + l + m + n + o + p))
        );

        res.Messages.AssertEqual(
            OnNext(210, N),
            OnCompleted<int>(220)
        );
    }

    [TestMethod]
    public void And16Error()
    {
        var ex = new Exception();

        const int N = 16;

        for (var i = 0; i < N; i++)
        {
            var scheduler = new TestSchedulerRef(Target);
            var obs = new List<Seq<int>>();

            for (var j = 0; j < N; j++)
            {
                if (j == i)
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnError<int>(210, ex)
                    ));
                }
                else
                {
                    obs.Add(scheduler.CreateHotObservable(
                        OnNext(210, 1),
                        OnCompleted<int>(220)
                    ));
                }
            }

            var res = scheduler.Start(() =>
                Seq.When(
                    obs[0].And(obs[1]).And(obs[2]).And(obs[3]).And(obs[4]).And(obs[5]).And(obs[6])
                        .And(obs[7]).And(obs[8]).And(obs[9]).And(obs[10]).And(obs[11]).And(obs[12])
                        .And(obs[13]).And(obs[14]).And(obs[15])
                        .Then((a, b, c, d, e, f, g, h, i_, j, k, l, m, n, o, p) => 0))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }
    }
}
