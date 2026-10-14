// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Throttle</c> scenarios, from Rx.NET's <c>ThrottleTest.cs</c>.</summary>
/// <remarks>
/// 17 of the file's 19 tests (the two <c>*_ArgumentChecking</c> tests are the code-generated
/// stratum). Sixteen transcribe as they are: the timed form over the test scheduler, and the
/// duration-selector form with cold throttle sequences, in which a throttle that fires or
/// completes lets the pending element through, a later element supersedes a pending one and
/// ends its throttle, and a selector that throws or a throttle that fails ends the result.
/// <c>Throttle_DefaultScheduler</c> awaits the result through
/// <see cref="SharedReactiveTest.ToListAsync{T}"/> where the original blocks on
/// <c>ToEnumerable</c>, as the <c>Delay</c> scenarios do.
/// </remarks>
public abstract class ThrottleTests : SharedReactiveTest
{
    [TestMethod]
    public void Throttle_TimeSpan_AllPass()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 0),
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(270, 3),
            OnNext(300, 4),
            OnCompleted<int>(400)
        );

        var res = scheduler.Start(() =>
            xs.Throttle(TimeSpan.FromTicks(20), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(230, 1),
            OnNext(260, 2),
            OnNext(290, 3),
            OnNext(320, 4),
            OnCompleted<int>(400)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Throttle_TimeSpan_AllPass_ErrorEnd()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 0),
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(270, 3),
            OnNext(300, 4),
            OnError<int>(400, ex)
        );

        var res = scheduler.Start(() =>
            xs.Throttle(TimeSpan.FromTicks(20), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(230, 1),
            OnNext(260, 2),
            OnNext(290, 3),
            OnNext(320, 4),
            OnError<int>(400, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Throttle_TimeSpan_AllDrop()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 0),
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(270, 3),
            OnNext(300, 4),
            OnNext(330, 5),
            OnNext(360, 6),
            OnNext(390, 7),
            OnCompleted<int>(400)
        );

        var res = scheduler.Start(() =>
            xs.Throttle(TimeSpan.FromTicks(40), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(400, 7),
            OnCompleted<int>(400)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Throttle_TimeSpan_AllDrop_ErrorEnd()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 0),
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(270, 3),
            OnNext(300, 4),
            OnNext(330, 5),
            OnNext(360, 6),
            OnNext(390, 7),
            OnError<int>(400, ex)
        );

        var res = scheduler.Start(() =>
            xs.Throttle(TimeSpan.FromTicks(40), scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(400, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Throttle_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 0),
            OnCompleted<int>(300)
        );

        var res = scheduler.Start(() =>
            xs.Throttle(TimeSpan.FromTicks(10), scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void Throttle_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 0),
            OnError<int>(300, ex)
        );

        var res = scheduler.Start(() =>
            xs.Throttle(TimeSpan.FromTicks(10), scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(300, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void Throttle_Never()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 0)
        );

        var res = scheduler.Start(() =>
            xs.Throttle(TimeSpan.FromTicks(10), scheduler)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Throttle_Simple()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 0),
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(250, 3),
            OnNext(280, 4),
            OnCompleted<int>(300)
        );

        var res = scheduler.Start(() =>
            xs.Throttle(TimeSpan.FromTicks(20), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(230, 1),
            OnNext(270, 3),
            OnNext(300, 4),
            OnCompleted<int>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public async Task Throttle_DefaultScheduler()
    {
        Assert.IsTrue(
            (await ToListAsync(Seq.Return(1).Throttle(TimeSpan.FromMilliseconds(1))))
                .SequenceEqual([1]));
    }

    [TestMethod]
    public void Throttle_Duration_DelayBehavior()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, -1),
            OnNext(250, 0),
            OnNext(280, 1),
            OnNext(310, 2),
            OnNext(350, 3),
            OnNext(400, 4),
            OnCompleted<int>(550)
        );

        var ys = new[] {
            scheduler.CreateColdObservable(
                OnNext(20, 42),
                OnNext(25, 99)
            ),
            scheduler.CreateColdObservable(
                OnNext(20, 42),
                OnNext(25, 99)
            ),
            scheduler.CreateColdObservable(
                OnNext(20, 42),
                OnNext(25, 99)
            ),
            scheduler.CreateColdObservable(
                OnNext(20, 42),
                OnNext(25, 99)
            ),
            scheduler.CreateColdObservable(
                OnNext(20, 42),
                OnNext(25, 99)
            ),
        };

        var res = scheduler.Start(() =>
            xs.Throttle(x => ys[x])
        );

        res.Messages.AssertEqual(
            OnNext(250 + 20, 0),
            OnNext(280 + 20, 1),
            OnNext(310 + 20, 2),
            OnNext(350 + 20, 3),
            OnNext(400 + 20, 4),
            OnCompleted<int>(550)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );

        ys[0].Subscriptions.AssertEqual(Subscribe(250, 250 + 20));
        ys[1].Subscriptions.AssertEqual(Subscribe(280, 280 + 20));
        ys[2].Subscriptions.AssertEqual(Subscribe(310, 310 + 20));
        ys[3].Subscriptions.AssertEqual(Subscribe(350, 350 + 20));
        ys[4].Subscriptions.AssertEqual(Subscribe(400, 400 + 20));
    }

    [TestMethod]
    public void Throttle_Duration_ThrottleBehavior()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, -1),
            OnNext(250, 0),
            OnNext(280, 1),
            OnNext(310, 2),
            OnNext(350, 3),
            OnNext(400, 4),
            OnCompleted<int>(550)
        );

        var ys = new[] {
            scheduler.CreateColdObservable(
                OnNext(20, 42),
                OnNext(25, 99)
            ),
            scheduler.CreateColdObservable(
                OnNext(40, 42),
                OnNext(45, 99)
            ),
            scheduler.CreateColdObservable(
                OnNext(20, 42),
                OnNext(25, 99)
            ),
            scheduler.CreateColdObservable(
                OnNext(60, 42),
                OnNext(65, 99)
            ),
            scheduler.CreateColdObservable(
                OnNext(20, 42),
                OnNext(25, 99)
            ),
        };

        var res = scheduler.Start(() =>
            xs.Throttle(x => ys[x])
        );

        res.Messages.AssertEqual(
            OnNext(250 + 20, 0),
            OnNext(310 + 20, 2),
            OnNext(400 + 20, 4),
            OnCompleted<int>(550)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );

        ys[0].Subscriptions.AssertEqual(Subscribe(250, 250 + 20));
        ys[1].Subscriptions.AssertEqual(Subscribe(280, 310));
        ys[2].Subscriptions.AssertEqual(Subscribe(310, 310 + 20));
        ys[3].Subscriptions.AssertEqual(Subscribe(350, 400));
        ys[4].Subscriptions.AssertEqual(Subscribe(400, 400 + 20));
    }

    [TestMethod]
    public void Throttle_Duration_EarlyCompletion()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, -1),
            OnNext(250, 0),
            OnNext(280, 1),
            OnNext(310, 2),
            OnNext(350, 3),
            OnNext(400, 4),
            OnCompleted<int>(410)
        );

        var ys = new[] {
            scheduler.CreateColdObservable(
                OnNext(20, 42),
                OnNext(25, 99)
            ),
            scheduler.CreateColdObservable(
                OnNext(40, 42),
                OnNext(45, 99)
            ),
            scheduler.CreateColdObservable(
                OnNext(20, 42),
                OnNext(25, 99)
            ),
            scheduler.CreateColdObservable(
                OnNext(60, 42),
                OnNext(65, 99)
            ),
            scheduler.CreateColdObservable(
                OnNext(20, 42),
                OnNext(25, 99)
            ),
        };

        var res = scheduler.Start(() =>
            xs.Throttle(x => ys[x])
        );

        res.Messages.AssertEqual(
            OnNext(250 + 20, 0),
            OnNext(310 + 20, 2),
            OnNext(410, 4),
            OnCompleted<int>(410)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 410)
        );

        ys[0].Subscriptions.AssertEqual(Subscribe(250, 250 + 20));
        ys[1].Subscriptions.AssertEqual(Subscribe(280, 310));
        ys[2].Subscriptions.AssertEqual(Subscribe(310, 310 + 20));
        ys[3].Subscriptions.AssertEqual(Subscribe(350, 400));
        ys[4].Subscriptions.AssertEqual(Subscribe(400, 410));
    }

    [TestMethod]
    public void Throttle_Duration_InnerError()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnCompleted<int>(550)
        );

        var ex = new Exception();

        var res = scheduler.Start(() =>
            xs.Throttle(x =>
                x < 4 ? scheduler.CreateColdObservable(
                            OnNext(x * 10, "Ignore"),
                            OnNext(x * 10 + 5, "Aargh!")
                        )
                      : scheduler.CreateColdObservable(
                            OnError<string>(x * 10, ex)
                        )
            )
        );

        res.Messages.AssertEqual(
            OnNext(250 + 2 * 10, 2),
            OnNext(350 + 3 * 10, 3),
            OnError<int>(450 + 4 * 10, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 490)
        );
    }

    [TestMethod]
    public void Throttle_Duration_OuterError()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnError<int>(460, ex)
        );

        var res = scheduler.Start(() =>
            xs.Throttle(x =>
                scheduler.CreateColdObservable(
                    OnNext(x * 10, "Ignore"),
                    OnNext(x * 10 + 5, "Aargh!")
                )
            )
        );

        res.Messages.AssertEqual(
            OnNext(250 + 2 * 10, 2),
            OnNext(350 + 3 * 10, 3),
            OnError<int>(460, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 460)
        );
    }

    [TestMethod]
    public void Throttle_Duration_SelectorThrows()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnCompleted<int>(550)
        );

        var ex = new Exception();

        var res = scheduler.Start(() =>
            xs.Throttle(x =>
            {
                if (x < 4)
                {
                    return scheduler.CreateColdObservable(
                            OnNext(x * 10, "Ignore"),
                            OnNext(x * 10 + 5, "Aargh!")
                        );
                }
                else
                {
                    throw ex;
                }
            })
        );

        res.Messages.AssertEqual(
            OnNext(250 + 2 * 10, 2),
            OnNext(350 + 3 * 10, 3),
            OnError<int>(450, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450)
        );
    }

    [TestMethod]
    public void Throttle_Duration_InnerDone_DelayBehavior()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnCompleted<int>(550)
        );

        var ex = new Exception();

        var res = scheduler.Start(() =>
            xs.Throttle(x =>
                scheduler.CreateColdObservable(
                    OnCompleted<string>(x * 10)
                )
            )
        );

        res.Messages.AssertEqual(
            OnNext(250 + 2 * 10, 2),
            OnNext(350 + 3 * 10, 3),
            OnNext(450 + 4 * 10, 4),
            OnCompleted<int>(550)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );
    }

    [TestMethod]
    public void Throttle_Duration_InnerDone_ThrottleBehavior()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(280, 3),
            OnNext(300, 4),
            OnNext(400, 5),
            OnNext(410, 6),
            OnCompleted<int>(550)
        );

        var ex = new Exception();

        var res = scheduler.Start(() =>
            xs.Throttle(x =>
                scheduler.CreateColdObservable(
                    OnCompleted<string>(x * 10)
                )
            )
        );

        res.Messages.AssertEqual(
            OnNext(250 + 2 * 10, 2),
            OnNext(300 + 4 * 10, 4),
            OnNext(410 + 6 * 10, 6),
            OnCompleted<int>(550)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );
    }
}
