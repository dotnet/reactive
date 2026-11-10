// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>SingleAsync</c> scenarios, from Rx.NET's <c>SingleAsyncTest.cs</c>.
/// </summary>
/// <remarks>
/// <para>
/// 10 of the file's 11 tests (the argument-checking test is the code-generated stratum).
/// Nine transcribe as they are: the plain and predicate forms over
/// empty, single, several and failing sources, where more than one element or match fails as
/// an empty source does, and a predicate that throws.
/// </para>
/// <para>
/// <c>MeaningfulStackTrace</c> is the original's claim in one shape: the error the operator
/// raises for a two-element source carries a stack trace that names the operator. The original
/// awaits the sequence three ways (directly, through <c>ToTask</c>, and through a subscription)
/// and asserts the same on each; this version awaits through
/// <see cref="SharedReactiveTest.ToListAsync{T}"/>, expects the exception rather than passing
/// silently without one, and looks for a frame of the library's (in
/// <c>System.Reactive.Linq</c>) that names <c>Single</c>, which both targets' operators are
/// called (Rx.NET's <c>SingleAsync</c>, AsyncRx.NET's <c>Single</c> behind its
/// <c>SingleAsync</c>). The original's bare search for <c>SingleAsync</c> is satisfied by
/// the test class's own frame, <c>SingleAsyncTest</c>, so it would pass whether or not the
/// operator's error carried a trace; this one does not.
/// </para>
/// </remarks>
public abstract class SingleAsyncTests : SharedReactiveTest
{
    [TestMethod]
    public void SingleAsync_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.SingleAsync()
        );

        res.Messages.AssertEqual(
            OnError<int>(250, e => e is InvalidOperationException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void SingleAsync_One()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.SingleAsync()
        );

        res.Messages.AssertEqual(
            OnNext(250, 2),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void SingleAsync_Many()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.SingleAsync()
        );

        res.Messages.AssertEqual(
            OnError<int>(220, e => e is InvalidOperationException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void SingleAsync_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.SingleAsync()
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void SingleAsync_Predicate()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.SingleAsync(x => x % 2 == 1)
        );

        res.Messages.AssertEqual(
            OnError<int>(240, e => e is InvalidOperationException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );
    }

    [TestMethod]
    public void SingleAsync_Predicate_Empty()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.SingleAsync(x => x % 2 == 1)
        );

        res.Messages.AssertEqual(
            OnError<int>(250, e => e is InvalidOperationException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void SingleAsync_Predicate_One()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.SingleAsync(x => x == 4)
        );

        res.Messages.AssertEqual(
            OnNext(250, 4),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void SingleAsync_Predicate_Throw()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.SingleAsync(x => x > 10)
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void SingleAsync_PredicateThrows()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.SingleAsync(x => { if (x < 4) { return false; } throw ex; })
        );

        res.Messages.AssertEqual(
            OnError<int>(230, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public async Task MeaningfulStackTrace()
    {
        var xs = Seq.Range(0, 2).SingleAsync();

        var e = await Assert.ThrowsExactlyAsync<InvalidOperationException>(
            async () => await ToListAsync(xs));

        Assert.IsNotNull(e.StackTrace);
        Assert.AreNotEqual("", e.StackTrace);

        // A frame of the library's that names the operator; the test's own frames name it
        // too (this class is SingleAsyncTests), so a bare Contains would prove nothing.
        Assert.IsTrue(
            e.StackTrace.Split('\n').Any(
                frame => frame.Contains("System.Reactive.Linq") && frame.Contains("Single")),
            $"expected a library frame naming Single, got:\n{e.StackTrace}");
    }
}
