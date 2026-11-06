// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Contains</c> scenarios, from Rx.NET's <c>ContainsTest.cs</c>.</summary>
/// <remarks>
/// 10 of the file's 11 tests (the <c>Contains_ArgumentChecking</c> test is the code-generated
/// stratum), all transcribed as they are: the default-comparer form over empty, single,
/// several, failing and never-ending sources, and the comparer form with the original's two
/// private comparers, one that throws and one that compares modulo two, nested here as there.
/// </remarks>
public abstract class ContainsTests : SharedReactiveTest
{
    [TestMethod]
    public void Contains_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Contains(42)
        );

        res.Messages.AssertEqual(
            OnNext(250, false),
            OnCompleted<bool>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Contains_ReturnPositive()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Contains(2)
        );

        res.Messages.AssertEqual(
            OnNext(210, true),
            OnCompleted<bool>(210)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Contains_ReturnNegative()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Contains(-2)
        );

        res.Messages.AssertEqual(
            OnNext(250, false),
            OnCompleted<bool>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Contains_SomePositive()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Contains(3)
        );

        res.Messages.AssertEqual(
            OnNext(220, true),
            OnCompleted<bool>(220)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void Contains_SomeNegative()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Contains(-3)
        );

        res.Messages.AssertEqual(
            OnNext(250, false),
            OnCompleted<bool>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Contains_Throw()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.Contains(42)
        );

        res.Messages.AssertEqual(
            OnError<bool>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Contains_Never()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            xs.Contains(42)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Contains_ComparerThrows()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2)
        );

        var res = scheduler.Start(() =>
            xs.Contains(42, new ContainsComparerThrows())
        );

        res.Messages.AssertEqual(
            OnError<bool>(210, e => e is NotImplementedException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Contains_ComparerContainsValue()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 3),
            OnNext(220, 4),
            OnNext(230, 8),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Contains(42, new ContainsComparerMod2())
        );

        res.Messages.AssertEqual(
            OnNext(220, true),
            OnCompleted<bool>(220)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void Contains_ComparerDoesNotContainValue()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 4),
            OnNext(230, 8),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Contains(21, new ContainsComparerMod2())
        );

        res.Messages.AssertEqual(
            OnNext(250, false),
            OnCompleted<bool>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    private sealed class ContainsComparerThrows : IEqualityComparer<int>
    {
        public bool Equals(int x, int y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(int obj)
        {
            throw new NotImplementedException();
        }
    }

    private sealed class ContainsComparerMod2 : IEqualityComparer<int>
    {
        public bool Equals(int x, int y)
        {
            return x % 2 == y % 2;
        }

        public int GetHashCode(int obj)
        {
            return obj.GetHashCode();
        }
    }
}
