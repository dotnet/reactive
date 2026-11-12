// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>OfType</c> scenarios, from Rx.NET's <c>OfTypeTest.cs</c>.
/// </summary>
/// <remarks>
/// 4 of the file's 5 tests (the <c>OfType_ArgumentChecking</c> test is the code-generated
/// stratum): a source of objects of several types, among them an <c>int</c>, a string and a
/// <c>bool</c>, filtered to <see cref="B"/>, completing, failing and disposed part-way, and a
/// source with a null element filtered to <see cref="A"/>, which drops it and types the result
/// non-null. All four declare their sources as <c>object?</c> elements where the originals
/// write <c>object</c> or <c>A?</c>: <c>OfType</c> takes a <c>Seq&lt;object?&gt;</c>, as Rx.NET's
/// takes an <c>IObservable&lt;object?&gt;</c>, and where Rx.NET's covariance accepts the
/// originals' sources, the model's <see cref="Seq{T}"/> is invariant. What the tests claim is
/// unchanged.
/// </remarks>
public abstract class OfTypeTests : SharedReactiveTest
{
    [TestMethod]
    public void OfType_Complete()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext<object?>(210, new B(0)),
            OnNext<object?>(220, new A(1)),
            OnNext<object?>(230, new E(2)),
            OnNext<object?>(240, new D(3)),
            OnNext<object?>(250, new C(4)),
            OnNext<object?>(260, new B(5)),
            OnNext<object?>(270, new B(6)),
            OnNext<object?>(280, new D(7)),
            OnNext<object?>(290, new A(8)),
            OnNext<object?>(300, new E(9)),
            OnNext<object?>(310, 3),
            OnNext<object?>(320, "foo"),
            OnNext<object?>(330, true),
            OnNext<object?>(340, new B(10)),
            OnCompleted<object?>(350)
        );

        var res = scheduler.Start(() =>
            xs.OfType<B>()
        );

        res.Messages.AssertEqual(
            OnNext(210, new B(0)),
            OnNext<B>(240, new D(3)),
            OnNext(260, new B(5)),
            OnNext(270, new B(6)),
            OnNext<B>(280, new D(7)),
            OnNext(340, new B(10)),
            OnCompleted<B>(350)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );
    }

    [TestMethod]
    public void OfType_NullableSourceOfTypeNonNull()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext<object?>(210, new B(0)),
            OnNext<object?>(220, new A(1)),
            OnNext<object?>(230, default(A?)),
            OnNext<object?>(240, new D(3)),
            OnNext<object?>(250, new C(4)),
            OnNext<object?>(260, new B(5)),
            OnNext<object?>(270, new B(6)),
            OnNext<object?>(280, new D(7)),
            OnNext<object?>(290, new A(8)),
            OnNext<object?>(340, new B(10)),
            OnCompleted<object?>(350)
        );

        var res = scheduler.Start(() =>
            xs.OfType<A>()
        );

        res.Messages.AssertEqual(
            OnNext<A>(210, new B(0)),
            OnNext<A>(220, new A(1)),
            OnNext<A>(240, new D(3)),
            OnNext<A>(250, new C(4)),
            OnNext<A>(260, new B(5)),
            OnNext<A>(270, new B(6)),
            OnNext<A>(280, new D(7)),
            OnNext<A>(290, new A(8)),
            OnNext<A>(340, new B(10)),
            OnCompleted<A>(350)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );
    }

    [TestMethod]
    public void OfType_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext<object?>(210, new B(0)),
            OnNext<object?>(220, new A(1)),
            OnNext<object?>(230, new E(2)),
            OnNext<object?>(240, new D(3)),
            OnNext<object?>(250, new C(4)),
            OnNext<object?>(260, new B(5)),
            OnNext<object?>(270, new B(6)),
            OnNext<object?>(280, new D(7)),
            OnNext<object?>(290, new A(8)),
            OnNext<object?>(300, new E(9)),
            OnNext<object?>(310, 3),
            OnNext<object?>(320, "foo"),
            OnNext<object?>(330, true),
            OnNext<object?>(340, new B(10)),
            OnError<object?>(350, ex)
        );

        var res = scheduler.Start(() =>
            xs.OfType<B>()
        );

        res.Messages.AssertEqual(
            OnNext(210, new B(0)),
            OnNext<B>(240, new D(3)),
            OnNext(260, new B(5)),
            OnNext(270, new B(6)),
            OnNext<B>(280, new D(7)),
            OnNext(340, new B(10)),
            OnError<B>(350, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );
    }

    [TestMethod]
    public void OfType_Dispose()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext<object?>(210, new B(0)),
            OnNext<object?>(220, new A(1)),
            OnNext<object?>(230, new E(2)),
            OnNext<object?>(240, new D(3)),
            OnNext<object?>(250, new C(4)),
            OnNext<object?>(260, new B(5)),
            OnNext<object?>(270, new B(6)),
            OnNext<object?>(280, new D(7)),
            OnNext<object?>(290, new A(8)),
            OnNext<object?>(300, new E(9)),
            OnNext<object?>(310, 3),
            OnNext<object?>(320, "foo"),
            OnNext<object?>(330, true),
            OnNext<object?>(340, new B(10)),
            OnError<object?>(350, new Exception())
        );

        var res = scheduler.Start(() =>
            xs.OfType<B>(),
            275
        );

        res.Messages.AssertEqual(
            OnNext(210, new B(0)),
            OnNext<B>(240, new D(3)),
            OnNext(260, new B(5)),
            OnNext(270, new B(6))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 275)
        );
    }
}
