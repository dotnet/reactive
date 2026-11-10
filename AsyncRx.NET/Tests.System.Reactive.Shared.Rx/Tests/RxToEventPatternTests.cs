// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>ToEventPattern</c> scenarios on Rx.NET, plus five of its own.
/// </summary>
/// <remarks>
/// <c>ToEvent_SourceCompletes</c>, <c>ToEvent_SourceFails</c> and <c>ToEvent_DoneImmediately</c>
/// are the originals: each reflects into the event source's private subscription table to show
/// it empty once the source has ended, which is Rx.NET's own implementation. The two
/// <c>EventPattern_*</c> tests exercise the <c>EventPattern</c> type of System.Reactive, which
/// both libraries share, so one run is enough and it is this one. As the originals but for
/// MSTest's assertions.
/// </remarks>
[TestClass]
public sealed class RxToEventPatternTests : ToEventPatternTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void ToEvent_SourceCompletes()
    {
        var src = new Subject<Unit>();
        var evt = src.ToEvent();

        var num = 0;
        var hnd = new Action<Unit>(e => num++);

        evt.OnNext += hnd;

        Assert.AreEqual(0, num);

        src.OnNext(new Unit());
        Assert.AreEqual(1, num);

        src.OnNext(new Unit());
        Assert.AreEqual(2, num);

        src.OnCompleted();
        Assert.AreEqual(2, num);

        var tbl = GetSubscriptionTable(evt);
        Assert.IsTrue(tbl.Count == 0);
    }

    [TestMethod]
    public void ToEvent_SourceFails()
    {
        var src = new Subject<Unit>();
        var evt = src.ToEvent();

        var num = 0;
        var hnd = new Action<Unit>(e => num++);

        evt.OnNext += hnd;

        Assert.AreEqual(0, num);

        src.OnNext(new Unit());
        Assert.AreEqual(1, num);

        src.OnNext(new Unit());
        Assert.AreEqual(2, num);

        var ex = new Exception();

        Assert.AreSame(ex, Assert.ThrowsExactly<Exception>(() => src.OnError(ex)));

        var tbl = GetSubscriptionTable(evt);
        Assert.IsTrue(tbl.Count == 0);
    }

    [TestMethod]
    public void ToEvent_DoneImmediately()
    {
        var src = Observable.Empty<Unit>();
        var evt = src.ToEvent();

        var num = 0;
        var hnd = new Action<Unit>(e => num++);

        for (var i = 0; i < 2; i++)
        {
            evt.OnNext += hnd;

            Assert.AreEqual(0, num);

            var tbl = GetSubscriptionTable(evt);
            Assert.IsTrue(tbl.Count == 0);
        }
    }

    [TestMethod]
    public void EventPattern_Equality()
    {
        var e1 = new EventPattern<string, EventArgs>("Bart", EventArgs.Empty);
        var e2 = new EventPattern<string, EventArgs>("Bart", EventArgs.Empty);

        Assert.IsTrue(e1.Equals(e1));
        Assert.IsTrue(e1.Equals(e2));
        Assert.IsTrue(e2.Equals(e1));
        Assert.IsTrue(e1 == e2);
        Assert.IsTrue(!(e1 != e2));
        Assert.IsTrue(e1.GetHashCode() == e2.GetHashCode());

        Assert.IsFalse(e1.Equals(null));
        Assert.IsFalse(e1.Equals("xy"));
        Assert.IsFalse(e1 == null!);
    }

    [TestMethod]
    public void EventPattern_Inequality()
    {
        var a1 = new MyEventArgs();
        var a2 = new MyEventArgs();

        var e1 = new EventPattern<string, MyEventArgs>("Bart", a1);
        var e2 = new EventPattern<string, MyEventArgs>("John", a1);
        var e3 = new EventPattern<string, MyEventArgs>("Bart", a2);

        Assert.IsTrue(!e1.Equals(e2));
        Assert.IsTrue(!e2.Equals(e1));
        Assert.IsTrue(!(e1 == e2));
        Assert.IsTrue(e1 != e2);
        Assert.IsTrue(e1.GetHashCode() != e2.GetHashCode());

        Assert.IsTrue(!e1.Equals(e3));
        Assert.IsTrue(!e3.Equals(e1));
        Assert.IsTrue(!(e1 == e3));
        Assert.IsTrue(e1 != e3);
        Assert.IsTrue(e1.GetHashCode() != e3.GetHashCode());
    }

    private static Dictionary<Delegate, Stack<IDisposable>> GetSubscriptionTable(object evt)
    {
        return (Dictionary<Delegate, Stack<IDisposable>>)evt.GetType()
            .GetField("_subscriptions", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(evt)!;
    }

    private class MyEventArgs : EventArgs
    {
    }
}
