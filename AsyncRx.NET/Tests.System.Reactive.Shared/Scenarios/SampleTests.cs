// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Sample</c> scenarios, from Rx.NET's <c>SampleTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 12 of the file's 19 tests. Eleven transcribe as they are: the timed form over the test
/// scheduler, where the source's completion is forwarded with the next sample and ends the
/// source's subscription at once, and the sampler form, where the sampler's completion flushes
/// the latest element and ends the sampler's subscription while the source goes on, and either
/// side's error ends the result. <c>Sample_DefaultScheduler_Periodic</c> awaits the result
/// through <see cref="SharedReactiveTest.ToListAsync{T}"/> where the original blocks on
/// <c>ToEnumerable</c>.
/// </para>
/// <para>
/// Not here: <c>Sample_ArgumentChecking</c> (the code-generated stratum); the five
/// <c>Sample_Periodic_*</c> tests, which repeat their twins' expectations over Rx.NET's
/// <c>PeriodicTestScheduler</c> and add an assertion on the periodic timer runs it records, a
/// sync-scheduler feature with no async counterpart, as the <c>Window</c> and <c>Buffer</c>
/// periodic tests were left; and <c>Sample_DefaultScheduler_PeriodicDisabled</c>, which runs
/// on a scheduler stripped of its periodic interface with <c>DisableOptimizations</c>, which
/// only Rx.NET has, and so sits in its runner.
/// </para>
/// </remarks>
public abstract class SampleTests : SharedReactiveTest
{
    [TestMethod]
    public void Sample_Regular()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(230, 3),
            OnNext(260, 4),
            OnNext(300, 5),
            OnNext(350, 6),
            OnNext(380, 7),
            OnCompleted<int>(390)
        );

        var res = scheduler.Start(() =>
            xs.Sample(TimeSpan.FromTicks(50), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(250, 3),
            OnNext(300, 5), /* CHECK: boundary of sampling */
            OnNext(350, 6),
            OnNext(400, 7), /* Sample in last bucket */
            OnCompleted<int>(400)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 390)
        );
    }

    [TestMethod]
    public void Sample_ErrorInFlight()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(230, 3),
            OnNext(260, 4),
            OnNext(300, 5),
            OnNext(310, 6),
            OnError<int>(330, ex)
        );

        var res = scheduler.Start(() =>
            xs.Sample(TimeSpan.FromTicks(50), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(250, 3),
            OnNext(300, 5), /* CHECK: boundary of sampling */
            OnError<int>(330, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 330)
        );
    }

    [TestMethod]
    public void Sample_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(300)
        );

        var res = scheduler.Start(() =>
            xs.Sample(TimeSpan.FromTicks(50), scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void Sample_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(300, ex)
        );

        var res = scheduler.Start(() =>
            xs.Sample(TimeSpan.FromTicks(50), scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(300, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void Sample_Never()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            xs.Sample(TimeSpan.FromTicks(50), scheduler)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public async Task Sample_DefaultScheduler_Periodic()
    {
        var res = (await ToListAsync(Seq.Return(42).Sample(TimeSpan.FromMilliseconds(1)))).Single();

        Assert.AreEqual(42, res);
    }

    [TestMethod]
    public void Sample_Sampler_Simple1()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(300, 5),
            OnNext(310, 6),
            OnCompleted<int>(400)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(150, ""),
            OnNext(210, "bar"),
            OnNext(250, "foo"),
            OnNext(260, "qux"),
            OnNext(320, "baz"),
            OnCompleted<string>(500)
        );

        var res = scheduler.Start(() =>
            xs.Sample(ys)
        );

        res.Messages.AssertEqual(
            OnNext(250, 3),
            OnNext(320, 6),
            OnCompleted<int>(500 /* on sampling boundaries only */)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 500)
        );
    }

    [TestMethod]
    public void Sample_Sampler_Simple2()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(300, 5),
            OnNext(310, 6),
            OnNext(360, 7),
            OnCompleted<int>(400)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(150, ""),
            OnNext(210, "bar"),
            OnNext(250, "foo"),
            OnNext(260, "qux"),
            OnNext(320, "baz"),
            OnCompleted<string>(500)
        );

        var res = scheduler.Start(() =>
            xs.Sample(ys)
        );

        res.Messages.AssertEqual(
            OnNext(250, 3),
            OnNext(320, 6),
            OnNext(500, 7),
            OnCompleted<int>(500 /* on sampling boundaries only */)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 500)
        );
    }

    [TestMethod]
    public void Sample_Sampler_Simple3()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnCompleted<int>(300)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(150, ""),
            OnNext(210, "bar"),
            OnNext(250, "foo"),
            OnNext(260, "qux"),
            OnNext(320, "baz"),
            OnCompleted<string>(500)
        );

        var res = scheduler.Start(() =>
            xs.Sample(ys)
        );

        res.Messages.AssertEqual(
            OnNext(250, 3),
            OnNext(320, 4),
            OnCompleted<int>(320 /* on sampling boundaries only */)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 320)
        );
    }

    [TestMethod]
    public void Sample_Sampler_completes_first()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnCompleted<int>(600)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(150, ""),
            OnNext(210, "bar"),
            OnNext(250, "foo"),
            OnNext(260, "qux"),
            OnNext(320, "baz"),
            OnCompleted<string>(500)
        );

        var res = scheduler.Start(() =>
            xs.Sample(ys)
        );

        res.Messages.AssertEqual(
            OnNext(250, 3),
            OnNext(320, 4),
            OnCompleted<int>(600 /* on sampling boundaries only */)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 500)
        );
    }

    [TestMethod]
    public void Sample_Sampler_SourceThrows()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(300, 5),
            OnNext(310, 6),
            OnError<int>(320, ex)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(150, ""),
            OnNext(210, "bar"),
            OnNext(250, "foo"),
            OnNext(260, "qux"),
            OnNext(330, "baz"),
            OnCompleted<string>(400)
        );

        var res = scheduler.Start(() =>
            xs.Sample(ys)
        );

        res.Messages.AssertEqual(
            OnNext(250, 3),
            OnError<int>(320, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 320)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 320)
        );
    }

    [TestMethod]
    public void Sample_Sampler_SamplerThrows()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnNext(240, 3),
            OnNext(290, 4),
            OnNext(300, 5),
            OnNext(310, 6),
            OnCompleted<int>(400)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(150, ""),
            OnNext(210, "bar"),
            OnNext(250, "foo"),
            OnNext(260, "qux"),
            OnError<string>(320, ex)
        );

        var res = scheduler.Start(() =>
            xs.Sample(ys)
        );

        res.Messages.AssertEqual(
            OnNext(250, 3),
            OnError<int>(320, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 320)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 320)
        );
    }
}
