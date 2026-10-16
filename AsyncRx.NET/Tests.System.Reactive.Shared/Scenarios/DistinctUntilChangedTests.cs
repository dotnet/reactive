// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>DistinctUntilChanged</c> scenarios, from Rx.NET's <c>DistinctUntilChangedTest.cs</c>.
/// </summary>
/// <remarks>
/// 13 of the file's 14 tests (the <c>DistinctUntilChanged_ArgumentChecking</c> test is the
/// code-generated stratum), all transcribed as they are (with <c>Seq.Never</c> for
/// <c>Observable.Never</c>): the four forms (default comparer, a comparer, a key selector,
/// both) over never-ending, empty, single, failing and changing sources, and a key selector or
/// comparer that throws, with the original's two private comparers (one over a delegate, one
/// throwing) nested here as there.
/// </remarks>
public abstract class DistinctUntilChangedTests : SharedReactiveTest
{
    [TestMethod]
    public void DistinctUntilChanged_Never()
    {
        var scheduler = Scheduler;

        var xs = Seq.Never<int>();

        var res = scheduler.Start(() =>
            xs.DistinctUntilChanged()
        );

        res.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public void DistinctUntilChanged_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.DistinctUntilChanged()
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void DistinctUntilChanged_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.DistinctUntilChanged()
        );

        res.Messages.AssertEqual(
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void DistinctUntilChanged_Throw()
    {
        var scheduler = Scheduler;

        var ex = new Exception();
        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(250, ex)
        );

        var res = scheduler.Start(() =>
            xs.DistinctUntilChanged()
        );

        res.Messages.AssertEqual(
            OnError<int>(250, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void DistinctUntilChanged_AllChanges()
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
            xs.DistinctUntilChanged()
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void DistinctUntilChanged_AllSame()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 2),
            OnNext(230, 2),
            OnNext(240, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.DistinctUntilChanged()
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void DistinctUntilChanged_SomeChanges()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2), //*
            OnNext(215, 3), //*
            OnNext(220, 3),
            OnNext(225, 2), //*
            OnNext(230, 2),
            OnNext(230, 1), //*
            OnNext(240, 2), //*
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.DistinctUntilChanged()
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(215, 3),
            OnNext(225, 2),
            OnNext(230, 1),
            OnNext(240, 2),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void DistinctUntilChanged_Comparer_AllEqual()
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
            xs.DistinctUntilChanged(new FuncComparer<int>((x, y) => true))
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void DistinctUntilChanged_Comparer_AllDifferent()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 2),
            OnNext(230, 2),
            OnNext(240, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.DistinctUntilChanged(new FuncComparer<int>((x, y) => false))
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 2),
            OnNext(230, 2),
            OnNext(240, 2),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void DistinctUntilChanged_KeySelector_Div2()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2), //*
            OnNext(220, 4),
            OnNext(230, 3), //*
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.DistinctUntilChanged(x => x % 2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(230, 3),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void DistinctUntilChanged_KeySelectorThrows()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.DistinctUntilChanged(new Func<int, int>(x => { throw ex; }))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }

    [TestMethod]
    public void DistinctUntilChanged_ComparerThrows()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.DistinctUntilChanged(new ThrowComparer<int>(ex))
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnError<int>(220, ex)
        );
    }

    [TestMethod]
    public void DistinctUntilChanged_KeySelector_Comparer()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2), // * key = 1    % 3 = 1
            OnNext(220, 8), //   key = 4    % 3 = 1   same
            OnNext(230, 2), //   key = 1    % 3 = 1   same
            OnNext(240, 5), // * key = 2    % 3 = 2
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.DistinctUntilChanged(x => x / 2, new FuncComparer<int>((x, y) => x % 3 == y % 3))
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    private sealed class FuncComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _equals;

        public FuncComparer(Func<T, T, bool> equals)
        {
            _equals = equals;
        }

        public bool Equals(T? x, T? y)
        {
            return _equals(x!, y!);
        }

        public int GetHashCode(T obj)
        {
            return 0;
        }
    }

    private sealed class ThrowComparer<T> : IEqualityComparer<T>
    {
        private readonly Exception _ex;

        public ThrowComparer(Exception ex)
        {
            _ex = ex;
        }

        public bool Equals(T? x, T? y)
        {
            throw _ex;
        }

        public int GetHashCode(T obj)
        {
            return 0;
        }
    }
}
