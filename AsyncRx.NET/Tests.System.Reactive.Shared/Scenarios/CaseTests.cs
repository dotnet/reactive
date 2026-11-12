// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Case</c> scenarios, from Rx.NET's <c>CaseTest.cs</c>.</summary>
/// <remarks>
/// 10 of the file's 11 tests (<c>Case_ArgumentChecking</c> is the code-generated stratum). Nine
/// transcribe as they are: the selector picking each branch of a dictionary of sources, a
/// selector that throws, an erroring branch, and the default-source and scheduler forms (the
/// scheduled empty default's tick as <c>ScheduledAt(200)</c>). The dictionary is one of
/// descriptions, <c>Dictionary&lt;int, Seq&lt;int&gt;&gt;</c>, which the bridge converts into
/// one of the target's own sequences. <c>CaseWithDefault_CheckDefault</c> compares the default
/// scheduler with the explicit one through <see cref="SharedReactiveTest.ToListAsync{T}"/>
/// where the original compares the two sequences with <c>ReactiveAssert.AssertEqual</c>.
/// </remarks>
public abstract class CaseTests : SharedReactiveTest
{
    [TestMethod]
    public void Case_One()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(270, 3),
            OnCompleted<int>(300)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(220, 11),
            OnNext(250, 12),
            OnNext(280, 13),
            OnCompleted<int>(310)
        );

        var zs = scheduler.CreateHotObservable(
            OnNext(230, 21),
            OnNext(240, 22),
            OnNext(290, 23),
            OnCompleted<int>(320)
        );

        var map = new Dictionary<int, Seq<int>>
        {
            { 1, xs },
            { 2, ys }
        };

        var results = scheduler.Start(() => Seq.Case(() => 1, map, zs));

        results.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(270, 3),
            OnCompleted<int>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
        );

        zs.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Case_Two()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(270, 3),
            OnCompleted<int>(300)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(220, 11),
            OnNext(250, 12),
            OnNext(280, 13),
            OnCompleted<int>(310)
        );

        var zs = scheduler.CreateHotObservable(
            OnNext(230, 21),
            OnNext(240, 22),
            OnNext(290, 23),
            OnCompleted<int>(320)
        );

        var map = new Dictionary<int, Seq<int>>
        {
            { 1, xs },
            { 2, ys }
        };

        var results = scheduler.Start(() => Seq.Case(() => 2, map, zs));

        results.Messages.AssertEqual(
            OnNext(220, 11),
            OnNext(250, 12),
            OnNext(280, 13),
            OnCompleted<int>(310)
        );

        xs.Subscriptions.AssertEqual(
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );

        zs.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Case_Three()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(270, 3),
            OnCompleted<int>(300)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(220, 11),
            OnNext(250, 12),
            OnNext(280, 13),
            OnCompleted<int>(310)
        );

        var zs = scheduler.CreateHotObservable(
            OnNext(230, 21),
            OnNext(240, 22),
            OnNext(290, 23),
            OnCompleted<int>(320)
        );

        var map = new Dictionary<int, Seq<int>>
        {
            { 1, xs },
            { 2, ys }
        };

        var results = scheduler.Start(() => Seq.Case(() => 3, map, zs));

        results.Messages.AssertEqual(
            OnNext(230, 21),
            OnNext(240, 22),
            OnNext(290, 23),
            OnCompleted<int>(320)
        );

        xs.Subscriptions.AssertEqual(
        );

        ys.Subscriptions.AssertEqual(
        );

        zs.Subscriptions.AssertEqual(
            Subscribe(200, 320)
        );
    }

    [TestMethod]
    public void Case_Throw()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(270, 3),
            OnCompleted<int>(300)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(220, 11),
            OnNext(250, 12),
            OnNext(280, 13),
            OnCompleted<int>(310)
        );

        var zs = scheduler.CreateHotObservable(
            OnNext(230, 21),
            OnNext(240, 22),
            OnNext(290, 23),
            OnCompleted<int>(320)
        );

        var map = new Dictionary<int, Seq<int>>
        {
            { 1, xs },
            { 2, ys }
        };

        var ex = new Exception();

        var results = scheduler.Start(() => Seq.Case(() => Throw<int>(ex), map, zs));

        results.Messages.AssertEqual(
            OnError<int>(200, ex)
        );

        xs.Subscriptions.AssertEqual(
        );

        ys.Subscriptions.AssertEqual(
        );

        zs.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void CaseWithDefault_One()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(270, 3),
            OnCompleted<int>(300)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(220, 11),
            OnNext(250, 12),
            OnNext(280, 13),
            OnCompleted<int>(310)
        );

        var map = new Dictionary<int, Seq<int>>
        {
            { 1, xs },
            { 2, ys }
        };

        var results = scheduler.Start(() => Seq.Case(() => 1, map, scheduler));

        results.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(270, 3),
            OnCompleted<int>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void CaseWithDefault_Two()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(270, 3),
            OnCompleted<int>(300)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(220, 11),
            OnNext(250, 12),
            OnNext(280, 13),
            OnCompleted<int>(310)
        );

        var map = new Dictionary<int, Seq<int>>
        {
            { 1, xs },
            { 2, ys }
        };

        var results = scheduler.Start(() => Seq.Case(() => 2, map, scheduler));

        results.Messages.AssertEqual(
            OnNext(220, 11),
            OnNext(250, 12),
            OnNext(280, 13),
            OnCompleted<int>(310)
        );

        xs.Subscriptions.AssertEqual(
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );
    }

    [TestMethod]
    public void CaseWithDefault_Three()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(270, 3),
            OnCompleted<int>(300)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(220, 11),
            OnNext(250, 12),
            OnNext(280, 13),
            OnCompleted<int>(310)
        );

        var map = new Dictionary<int, Seq<int>>
        {
            { 1, xs },
            { 2, ys }
        };

        var results = scheduler.Start(() => Seq.Case(() => 3, map, scheduler));

        results.Messages.AssertEqual(
            OnCompleted<int>(ScheduledAt(200))
        );

        xs.Subscriptions.AssertEqual(
        );

        ys.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void CaseWithDefault_Throw()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(270, 3),
            OnCompleted<int>(300)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(220, 11),
            OnNext(250, 12),
            OnNext(280, 13),
            OnCompleted<int>(310)
        );

        var map = new Dictionary<int, Seq<int>>
        {
            { 1, xs },
            { 2, ys }
        };

        var ex = new Exception();

        var results = scheduler.Start(() => Seq.Case(() => Throw<int>(ex), map, scheduler));

        results.Messages.AssertEqual(
            OnError<int>(200, ex)
        );

        xs.Subscriptions.AssertEqual(
        );

        ys.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public async Task CaseWithDefault_CheckDefault()
    {
    Assert.IsTrue(
        (await ToListAsync(Seq.Case(() => 1, new Dictionary<int, Seq<int>>(), DefaultScheduler)))
            .SequenceEqual(await ToListAsync(Seq.Case(() => 1, new Dictionary<int, Seq<int>>()))));
    }

    [TestMethod]
    public void Case_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(270, 3),
            OnError<int>(300, ex)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(220, 11),
            OnNext(250, 12),
            OnNext(280, 13),
            OnCompleted<int>(310)
        );

        var map = new Dictionary<int, Seq<int>>
        {
            { 1, xs },
            { 2, ys }
        };

        var results = scheduler.Start(() => Seq.Case(() => 1, map, scheduler));

        results.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(270, 3),
            OnError<int>(300, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
        );
    }

    private static T Throw<T>(Exception ex)
    {
        throw ex;
    }
}
