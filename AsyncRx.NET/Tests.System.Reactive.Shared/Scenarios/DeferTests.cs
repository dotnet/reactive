// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Defer</c> scenarios, from Rx.NET's <c>DeferTest.cs</c>.</summary>
/// <remarks>
/// 4 of the file's 5 tests (<c>Defer_ArgumentChecking</c> is the code-generated stratum), all
/// transcribed as they are: the factory, invoked once per subscription, builds a cold testable
/// source at the clock it is invoked at, and the scenarios check the elements, the count of
/// invocations and the source's subscription. The model's <c>Defer</c> node has served other
/// scenarios since Step 2; these are its own tests.
/// </remarks>
public abstract class DeferTests : SharedReactiveTest
{
    [TestMethod]
    public void Defer_Complete()
    {
        var invoked = 0;
        var xs = default(TestableSeq<long>);

        var res = Scheduler.Start(() =>
            Seq.Defer(() =>
            {
                invoked++;
                xs = Scheduler.CreateColdObservable(
                    OnNext(100, Scheduler.Clock),
                    OnCompleted<long>(200));
                return xs;
            })
        );

        res.Messages.AssertEqual(
            OnNext(300, 200L),
            OnCompleted<long>(400)
        );

        Assert.AreEqual(1, invoked);

        xs!.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Defer_Error()
    {
        var invoked = 0;
        var xs = default(TestableSeq<long>);
        var ex = new Exception();

        var res = Scheduler.Start(() =>
            Seq.Defer(() =>
            {
                invoked++;
                xs = Scheduler.CreateColdObservable(
                    OnNext(100, Scheduler.Clock),
                    OnError<long>(200, ex));
                return xs;
            })
        );

        res.Messages.AssertEqual(
            OnNext(300, 200L),
            OnError<long>(400, ex)
        );

        Assert.AreEqual(1, invoked);

        xs!.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Defer_Dispose()
    {
        var invoked = 0;
        var xs = default(TestableSeq<long>);

        var res = Scheduler.Start(() =>
            Seq.Defer(() =>
            {
                invoked++;
                xs = Scheduler.CreateColdObservable(
                    OnNext(100, Scheduler.Clock),
                    OnNext<long>(200, invoked),
                    OnNext<long>(1100, 1000));
                return xs;
            })
        );

        res.Messages.AssertEqual(
            OnNext(300, 200L),
            OnNext(400, 1L)
        );

        Assert.AreEqual(1, invoked);

        xs!.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Defer_Throw()
    {
        var invoked = 0;
        var ex = new Exception();

        var res = Scheduler.Start(() =>
            Seq.Defer(new Func<Seq<int>>(() =>
            {
                invoked++;
                throw ex;
            }))
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex)
        );

        Assert.AreEqual(1, invoked);
    }
}
