// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>Distinct</c> scenarios, from Rx.NET's <c>DistinctTest.cs</c>.
/// </summary>
/// <remarks>
/// 12 of the file's 13 tests (the <c>Distinct_ArgumentChecking</c> test is the code-generated
/// stratum). Eleven transcribe as they are: the four forms (default comparer, a comparer, a key
/// selector, both) over sources with and without duplicates, a key selector or comparer that
/// throws, and a failing source, with the original's two private comparers (one modulo
/// <c>n</c>, one throwing on a chosen value) nested here as there. <c>Distinct_Null</c> keeps a
/// <c>null</c> element among strings, which this project's nullable analysis makes it say so
/// about (<c>default(string)!</c>).
/// </remarks>
public abstract class DistinctTests : SharedReactiveTest
{
    [TestMethod]
    public void Distinct_DefaultComparer_AllDistinct()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 4),
            OnNext(300, 2),
            OnNext(350, 1),
            OnNext(380, 3),
            OnNext(400, 5),
            OnCompleted<int>(420)
        );

        var res = scheduler.Start(() =>
            xs.Distinct()
        );

        res.Messages.AssertEqual(
            OnNext(280, 4),
            OnNext(300, 2),
            OnNext(350, 1),
            OnNext(380, 3),
            OnNext(400, 5),
            OnCompleted<int>(420)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public void Distinct_DefaultComparer_SomeDuplicates()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 4),
            OnNext(300, 2),
            OnNext(350, 2),
            OnNext(380, 3),
            OnNext(400, 4),
            OnCompleted<int>(420)
        );

        var res = scheduler.Start(() =>
            xs.Distinct()
        );

        res.Messages.AssertEqual(
            OnNext(280, 4),
            OnNext(300, 2),
            OnNext(380, 3),
            OnCompleted<int>(420)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public void Distinct_CustomComparer_AllDistinct()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 4),
            OnNext(300, 2),
            OnNext(350, 1),
            OnNext(380, 3),
            OnNext(400, 5),
            OnCompleted<int>(420)
        );

        var res = scheduler.Start(() =>
            xs.Distinct(new ModComparer(10))
        );

        res.Messages.AssertEqual(
            OnNext(280, 4),
            OnNext(300, 2),
            OnNext(350, 1),
            OnNext(380, 3),
            OnNext(400, 5),
            OnCompleted<int>(420)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public void Distinct_CustomComparer_SomeDuplicates()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 4),
            OnNext(300, 2),
            OnNext(350, 12),
            OnNext(380, 3),
            OnNext(400, 24),
            OnCompleted<int>(420)
        );

        var res = scheduler.Start(() =>
            xs.Distinct(new ModComparer(10))
        );

        res.Messages.AssertEqual(
            OnNext(280, 4),
            OnNext(300, 2),
            OnNext(380, 3),
            OnCompleted<int>(420)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public void Distinct_KeySelector_DefaultComparer_AllDistinct()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 8),
            OnNext(300, 4),
            OnNext(350, 2),
            OnNext(380, 6),
            OnNext(400, 10),
            OnCompleted<int>(420)
        );

        var res = scheduler.Start(() =>
            xs.Distinct(x => x / 2)
        );

        res.Messages.AssertEqual(
            OnNext(280, 8),
            OnNext(300, 4),
            OnNext(350, 2),
            OnNext(380, 6),
            OnNext(400, 10),
            OnCompleted<int>(420)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public void Distinct_KeySelector_DefaultComparer_SomeDuplicates()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 4),
            OnNext(300, 2),
            OnNext(350, 3),
            OnNext(380, 7),
            OnNext(400, 5),
            OnCompleted<int>(420)
        );

        var res = scheduler.Start(() =>
            xs.Distinct(x => x / 2)
        );

        res.Messages.AssertEqual(
            OnNext(280, 4),
            OnNext(300, 2),
            OnNext(380, 7),
            OnCompleted<int>(420)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public void Distinct_KeySelector_CustomComparer_AllDistinct()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 8),
            OnNext(300, 4),
            OnNext(350, 2),
            OnNext(380, 6),
            OnNext(400, 10),
            OnCompleted<int>(420)
        );

        var res = scheduler.Start(() =>
            xs.Distinct(x => x / 2, new ModComparer(10))
        );

        res.Messages.AssertEqual(
            OnNext(280, 8),
            OnNext(300, 4),
            OnNext(350, 2),
            OnNext(380, 6),
            OnNext(400, 10),
            OnCompleted<int>(420)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public void Distinct_KeySelector_CustomComparer_SomeDuplicates()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 8),
            OnNext(300, 4),
            OnNext(350, 2),
            OnNext(380, 6),
            OnNext(400, 10),
            OnCompleted<int>(420)
        );

        var res = scheduler.Start(() =>
            xs.Distinct(x => x / 2, new ModComparer(3))
        );

        res.Messages.AssertEqual(
            OnNext(280, 8),
            OnNext(300, 4),
            OnNext(380, 6),
            OnCompleted<int>(420)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public void Distinct_KeySelector_Throws()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 3),
            OnNext(300, 2),
            OnNext(350, 1),
            OnNext(380, 0),
            OnNext(400, 4),
            OnCompleted<int>(420)
        );

        var ex = new Exception();

        var res = scheduler.Start(() =>
            xs.Distinct(x => { if (x == 0) { throw ex; } return x / 2; })
        );

        res.Messages.AssertEqual(
            OnNext(280, 3),
            OnNext(350, 1),
            OnError<int>(380, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 380)
        );
    }

    [TestMethod]
    public void Distinct_CustomComparer_Throws()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 1),
            OnNext(300, 2),
            OnNext(350, 3),
            OnNext(380, 4),
            OnNext(400, 5),
            OnCompleted<int>(420)
        );

        var ex = new Exception();

        var res = scheduler.Start(() =>
            xs.Distinct(new ThrowComparer(4, ex))
        );

        res.Messages.AssertEqual(
            OnNext(280, 1),
            OnNext(300, 2),
            OnNext(350, 3),
            OnError<int>(380, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 380)
        );
    }

    [TestMethod]
    public void Distinct_Throw()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 1),
            OnNext(300, 2),
            OnNext(350, 3),
            OnError<int>(380, ex)
        );

        var res = scheduler.Start(() =>
            xs.Distinct()
        );

        res.Messages.AssertEqual(
            OnNext(280, 1),
            OnNext(300, 2),
            OnNext(350, 3),
            OnError<int>(380, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 380)
        );
    }

    [TestMethod]
    public void Distinct_Null()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(220, "bar"),
            OnNext(240, default(string)!),
            OnNext(260, "bar"),
            OnNext(280, "foo"),
            OnNext(300, default(string)!),
            OnCompleted<string>(400)
        );

        var res = scheduler.Start(() =>
            xs.Distinct()
        );

        res.Messages.AssertEqual(
            OnNext(220, "bar"),
            OnNext(240, default(string)!),
            OnNext(280, "foo"),
            OnCompleted<string>(400)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    private sealed class ModComparer : IEqualityComparer<int>
    {
        private readonly int _mod;

        public ModComparer(int mod)
        {
            _mod = mod;
        }

        public bool Equals(int x, int y)
        {
            return EqualityComparer<int>.Default.Equals(x % _mod, y % _mod);
        }

        public int GetHashCode(int obj)
        {
            return EqualityComparer<int>.Default.GetHashCode(obj % _mod);
        }
    }

    private sealed class ThrowComparer : IEqualityComparer<int>
    {
        private readonly int _err;
        private readonly Exception _ex;

        public ThrowComparer(int err, Exception ex)
        {
            _err = err;
            _ex = ex;
        }

        public bool Equals(int x, int y)
        {
            return EqualityComparer<int>.Default.Equals(x, y);
        }

        public int GetHashCode(int obj)
        {
            if (obj == _err)
            {
                throw _ex;
            }

            return EqualityComparer<int>.Default.GetHashCode(obj);
        }
    }
}
