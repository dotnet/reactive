// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>Cast</c> scenarios, from Rx.NET's <c>CastTest.cs</c>.
/// </summary>
/// <remarks>
/// 4 of the file's 5 tests (the <c>Cast_ArgumentChecking</c> test is the code-generated
/// stratum), all transcribed as they are: a source of objects cast to <see cref="B"/>, which
/// passes <see cref="D"/>s as <c>B</c>s, completing, failing, disposed part-way, and with an
/// element that is no <c>B</c>, which ends the result with <c>InvalidCastException</c>. The
/// original's <c>A</c> to <c>E</c> hierarchy, declared in its file for <c>OfType</c>'s tests
/// too, is top-level here.
/// </remarks>
public abstract class CastTests : SharedReactiveTest
{
    [TestMethod]
    public void Cast_Complete()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext<object>(210, new B(0)),
            OnNext<object>(220, new D(1)),
            OnNext<object>(240, new B(2)),
            OnNext<object>(270, new D(3)),
            OnCompleted<object>(300)
        );

        var res = scheduler.Start(() =>
            xs.Cast<B>()
        );

        res.Messages.AssertEqual(
            OnNext(210, new B(0)),
            OnNext<B>(220, new D(1)),
            OnNext(240, new B(2)),
            OnNext<B>(270, new D(3)),
            OnCompleted<B>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void Cast_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext<object>(210, new B(0)),
            OnNext<object>(220, new D(1)),
            OnNext<object>(240, new B(2)),
            OnNext<object>(270, new D(3)),
            OnError<object>(300, ex)
        );

        var res = scheduler.Start(() =>
            xs.Cast<B>()
        );

        res.Messages.AssertEqual(
            OnNext(210, new B(0)),
            OnNext<B>(220, new D(1)),
            OnNext(240, new B(2)),
            OnNext<B>(270, new D(3)),
            OnError<B>(300, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void Cast_Dispose()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext<object>(210, new B(0)),
            OnNext<object>(220, new D(1)),
            OnNext<object>(240, new B(2)),
            OnNext<object>(270, new D(3)),
            OnCompleted<object>(300)
        );

        var res = scheduler.Start(() =>
            xs.Cast<B>(),
            250
        );

        res.Messages.AssertEqual(
            OnNext(210, new B(0)),
            OnNext<B>(220, new D(1)),
            OnNext(240, new B(2))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Cast_NotValid()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext<object>(210, new B(0)),
            OnNext<object>(220, new D(1)),
            OnNext<object>(240, new B(2)),
            OnNext<object>(250, new A(-1)),
            OnNext<object>(270, new D(3)),
            OnCompleted<object>(300)
        );

        var res = scheduler.Start(() =>
            xs.Cast<B>()
        );

        res.Messages.AssertEqual(
            OnNext(210, new B(0)),
            OnNext<B>(220, new D(1)),
            OnNext(240, new B(2)),
            OnError<B>(250, e => e is InvalidCastException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }
}
