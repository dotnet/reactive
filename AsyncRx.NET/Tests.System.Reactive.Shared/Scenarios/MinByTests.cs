// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>MinBy</c> scenarios, from Rx.NET's <c>MinByTest.cs</c>.</summary>
/// <remarks>
/// 13 of the file's 14 tests (<c>MinBy_ArgumentChecking</c> is the code-generated stratum),
/// all transcribed as they are: the key-selector form and the comparer form over empty,
/// single, several, tied, failing and never-ending sources, and a selector and a comparer
/// that throw. The comparers are the shared <see cref="ReverseComparer{T}"/> and
/// <see cref="ThrowingComparer{T}"/>.
/// </remarks>
public abstract class MinByTests : SharedReactiveTest
{
    [TestMethod]
    public void MinBy_Empty()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, new KeyValuePair<int, string>(1, "z")),
            OnCompleted<KeyValuePair<int, string>>(250)
        );

        var res = Scheduler.Start(() =>
            xs.MinBy(x => x.Key)
        );

        res.Messages.AssertEqual(
            OnNext<IList<KeyValuePair<int, string>>>(250, x => x.Count == 0),
            OnCompleted<IList<KeyValuePair<int, string>>>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void MinBy_Return()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, new KeyValuePair<int, string>(1, "z")),
            OnNext(210, new KeyValuePair<int, string>(2, "a")),
            OnCompleted<KeyValuePair<int, string>>(250)
        );

        var res = Scheduler.Start(() =>
            xs.MinBy(x => x.Key)
        );

        res.Messages.AssertEqual(
            OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual([
                new KeyValuePair<int, string>(2, "a"),
            ])),
            OnCompleted<IList<KeyValuePair<int, string>>>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void MinBy_Some()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, new KeyValuePair<int, string>(1, "z")),
            OnNext(210, new KeyValuePair<int, string>(3, "b")),
            OnNext(220, new KeyValuePair<int, string>(2, "c")),
            OnNext(230, new KeyValuePair<int, string>(4, "a")),
            OnCompleted<KeyValuePair<int, string>>(250)
        );

        var res = Scheduler.Start(() =>
            xs.MinBy(x => x.Key)
        );

        res.Messages.AssertEqual(
            OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual([
                new KeyValuePair<int, string>(2, "c"),
            ])),
            OnCompleted<IList<KeyValuePair<int, string>>>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void MinBy_Multiple()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, new KeyValuePair<int, string>(1, "z")),
            OnNext(210, new KeyValuePair<int, string>(3, "b")),
            OnNext(215, new KeyValuePair<int, string>(2, "d")),
            OnNext(220, new KeyValuePair<int, string>(3, "c")),
            OnNext(225, new KeyValuePair<int, string>(2, "y")),
            OnNext(230, new KeyValuePair<int, string>(4, "a")),
            OnNext(235, new KeyValuePair<int, string>(4, "r")),
            OnCompleted<KeyValuePair<int, string>>(250)
        );

        var res = Scheduler.Start(() =>
            xs.MinBy(x => x.Key)
        );

        res.Messages.AssertEqual(
            OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual([
                new KeyValuePair<int, string>(2, "d"),
                new KeyValuePair<int, string>(2, "y"),
            ])),
            OnCompleted<IList<KeyValuePair<int, string>>>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void MinBy_Throw()
    {
        var ex = new Exception();
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, new KeyValuePair<int, string>(1, "z")),
            OnError<KeyValuePair<int, string>>(210, ex)
        );

        var res = Scheduler.Start(() =>
            xs.MinBy(x => x.Key)
        );

        res.Messages.AssertEqual(
            OnError<IList<KeyValuePair<int, string>>>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void MinBy_Never()
    {
        var ex = new Exception();
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, new KeyValuePair<int, string>(1, "z"))
        );

        var res = Scheduler.Start(() =>
            xs.MinBy(x => x.Key)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void MinBy_Comparer_Empty()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, new KeyValuePair<int, string>(1, "z")),
            OnCompleted<KeyValuePair<int, string>>(250)
        );

        var res = Scheduler.Start(() =>
            xs.MinBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
        );

        res.Messages.AssertEqual(
            OnNext<IList<KeyValuePair<int, string>>>(250, x => x.Count == 0),
            OnCompleted<IList<KeyValuePair<int, string>>>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void MinBy_Comparer_Return()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, new KeyValuePair<int, string>(1, "z")),
            OnNext(210, new KeyValuePair<int, string>(2, "a")),
            OnCompleted<KeyValuePair<int, string>>(250)
        );

        var res = Scheduler.Start(() =>
            xs.MinBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
        );

        res.Messages.AssertEqual(
            OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual([
                new KeyValuePair<int, string>(2, "a"),
            ])),
            OnCompleted<IList<KeyValuePair<int, string>>>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void MinBy_Comparer_Some()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, new KeyValuePair<int, string>(1, "z")),
            OnNext(210, new KeyValuePair<int, string>(3, "b")),
            OnNext(220, new KeyValuePair<int, string>(20, "c")),
            OnNext(230, new KeyValuePair<int, string>(4, "a")),
            OnCompleted<KeyValuePair<int, string>>(250)
        );

        var res = Scheduler.Start(() =>
            xs.MinBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
        );

        res.Messages.AssertEqual(
            OnNext<IList<KeyValuePair<int, string>>>(250, x => x.SequenceEqual([
                new KeyValuePair<int, string>(20, "c"),
            ])),
            OnCompleted<IList<KeyValuePair<int, string>>>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void MinBy_Comparer_Throw()
    {
        var ex = new Exception();
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, new KeyValuePair<int, string>(1, "z")),
            OnError<KeyValuePair<int, string>>(210, ex)
        );

        var res = Scheduler.Start(() =>
            xs.MinBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
        );

        res.Messages.AssertEqual(
            OnError<IList<KeyValuePair<int, string>>>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void MinBy_Comparer_Never()
    {
        var ex = new Exception();
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, new KeyValuePair<int, string>(1, "z"))
        );

        var res = Scheduler.Start(() =>
            xs.MinBy(x => x.Key, new ReverseComparer<int>(Comparer<int>.Default))
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void MinBy_SelectorThrows()
    {
        var ex = new Exception();
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, new KeyValuePair<int, string>(1, "z")),
            OnNext(210, new KeyValuePair<int, string>(3, "b")),
            OnNext(220, new KeyValuePair<int, string>(2, "c")),
            OnNext(230, new KeyValuePair<int, string>(4, "a")),
            OnCompleted<KeyValuePair<int, string>>(250)
        );

        var res = Scheduler.Start(() =>
            xs.MinBy<KeyValuePair<int, string>, int>(x => { throw ex; })
        );

        res.Messages.AssertEqual(
            OnError<IList<KeyValuePair<int, string>>>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void MinBy_ComparerThrows()
    {
        var ex = new Exception();
        var xs = Scheduler.CreateHotObservable(
            OnNext(150, new KeyValuePair<int, string>(1, "z")),
            OnNext(210, new KeyValuePair<int, string>(3, "b")),
            OnNext(220, new KeyValuePair<int, string>(2, "c")),
            OnNext(230, new KeyValuePair<int, string>(4, "a")),
            OnCompleted<KeyValuePair<int, string>>(250)
        );

        var res = Scheduler.Start(() =>
            xs.MinBy(x => x.Key, new ThrowingComparer<int>(ex))
        );

        res.Messages.AssertEqual(
            OnError<IList<KeyValuePair<int, string>>>(220, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }
}
