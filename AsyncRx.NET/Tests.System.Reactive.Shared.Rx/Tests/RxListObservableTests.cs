// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// Rx.NET's own <c>ListObservable</c> tests, from its <c>ListObservableTest.cs</c>.
/// </summary>
/// <remarks>
/// <c>ListObservable&lt;T&gt;</c> retains a sequence's elements as an <c>IList&lt;T&gt;</c>,
/// subscribing in its constructor, and signals the end of the sequence to its own subscribers.
/// Rx.NET marks it <c>[Experimental]</c>, in <c>System.Reactive</c>; AsyncRx.NET has nothing of
/// this shape, so these run here against Rx.NET only (the maintainer's decision, Step 87: the
/// suite keeps the upstream coverage). All 45 of the file's tests, as the originals but for
/// MSTest's assertions (<c>Assert.IsTrue</c> and <c>IsFalse</c> for equality with a boolean
/// literal, a discard for the original's unused local): the list surface over an empty, a
/// completed and a never-ending source, and the subscription surface in virtual time.
/// </remarks>
[TestClass]
public sealed class RxListObservableTests : ReactiveTest
{
    [TestMethod]
    public void Ctor_ArgumentChecking()
    {
#pragma warning disable CA1806 // (Unused new instance.) We expect the constructor to throw.
        Assert.ThrowsExactly<ArgumentNullException>(() => new ListObservable<int>(null!));
#pragma warning restore CA1806
    }

    [TestMethod]
    public void Subscribe_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new ListObservable<int>(Observable.Never<int>()).Subscribe(null!));
    }

    [TestMethod]
    public void Value_None()
    {
        var o = new ListObservable<int>(Observable.Empty<int>());
        Assert.ThrowsExactly<InvalidOperationException>(() => { var t = o.Value; });
    }

    [TestMethod]
    public void Value_Some()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        Assert.AreEqual(9, o.Value);
    }

    [TestMethod]
    public void IndexOf_None()
    {
        var o = new ListObservable<int>(Observable.Empty<int>());
        Assert.AreEqual(-1, o.IndexOf(0));
    }

    [TestMethod]
    public void IndexOf_Some_NotFound()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        Assert.AreEqual(-1, o.IndexOf(100));
    }

    [TestMethod]
    public void IndexOf_Some_Found()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        Assert.AreEqual(3, o.IndexOf(3));
    }

    [TestMethod]
    public void RemoveAt_Some_NotFound()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => o.RemoveAt(100));
        o.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
    }

    [TestMethod]
    public void RemoveAt_Some_Found()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        o.RemoveAt(3);
        o.AssertEqual(0, 1, 2, 4, 5, 6, 7, 8, 9);
    }

    [TestMethod]
    public void Insert_Invalid()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => o.Insert(100, 100));
        o.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
    }

    [TestMethod]
    public void Insert_Invalid_2()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => o.Insert(-1, 100));
        o.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
    }

    [TestMethod]
    public void Insert_Beginning()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        o.Insert(0, -1);
        o.AssertEqual(-1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
    }

    [TestMethod]
    public void Insert_Middle()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        o.Insert(3, -1);
        o.AssertEqual(0, 1, 2, -1, 3, 4, 5, 6, 7, 8, 9);
    }

    [TestMethod]
    public void Change_Beginning()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10))
        {
            [0] = -1
        };
        o.AssertEqual(-1, 1, 2, 3, 4, 5, 6, 7, 8, 9);
    }

    [TestMethod]
    public void Change_Middle()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10))
        {
            [5] = -1
        };
        o.AssertEqual(0, 1, 2, 3, 4, -1, 6, 7, 8, 9);
    }

    [TestMethod]
    public void Change_End()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10))
        {
            [9] = -1
        };
        o.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, -1);
    }

    [TestMethod]
    public void Change_Error()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => o[100] = -1);
    }

    [TestMethod]
    public void Insert_End()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        o.Insert(10, -1);
        o.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, -1);
    }

    [TestMethod]
    public void Contains_None()
    {
        var o = new ListObservable<int>(Observable.Empty<int>());
        Assert.IsFalse(o.Contains(0));
    }

    [TestMethod]
    public void Contains_Some_NotFound()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        Assert.IsFalse(o.Contains(100));
    }

    [TestMethod]
    public void Contains_Some_Found()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        Assert.IsTrue(o.Contains(3));
    }

    [TestMethod]
    public void Clear()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        o.Clear();
        o.AssertEqual();
    }

    [TestMethod]
    public void IsReadOnly()
    {
        var o = new ListObservable<int>(Observable.Never<int>());
        Assert.IsFalse(o.IsReadOnly);
    }

    [TestMethod]
    public void This_None()
    {
        var o = new ListObservable<int>(Observable.Empty<int>());
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => { var t = o[0]; });
    }

    [TestMethod]
    public void This_Some_NotFound()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => { var t = o[100]; });
    }

    [TestMethod]
    public void This_Some_Found()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        Assert.AreEqual(5, o[5]);
    }

    [TestMethod]
    public void CopyTo_RightSize()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        var array = new int[10];
        o.CopyTo(array, 0);
        array.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
    }

    [TestMethod]
    public void CopyTo_RightSize_Offset()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        var array = new int[10];
        Assert.ThrowsExactly<ArgumentException>(() => o.CopyTo(array, 3));
    }

    [TestMethod]
    public void CopyTo_Bigger()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        var array = new int[15];
        o.CopyTo(array, 0);
        array.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 0, 0, 0, 0);
    }

    [TestMethod]
    public void CopyTo_Bigger_Offset()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        var array = new int[15];
        o.CopyTo(array, 3);
        array.AssertEqual(0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 0);
    }

    [TestMethod]
    public void CopyTo_Smaller()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        var array = new int[5];
        Assert.ThrowsExactly<ArgumentException>(() => o.CopyTo(array, 0));
    }

    [TestMethod]
    public void CopyTo_Smaller_Offset()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        var array = new int[5];
        Assert.ThrowsExactly<ArgumentException>(() => o.CopyTo(array, 3));
    }

    [TestMethod]
    public void Add_Empty()
    {
        var o = new ListObservable<int>(Observable.Empty<int>())
        {
            100
        };
        o.AssertEqual(100);
    }

    [TestMethod]
    public void Add_Some()
    {
        var o = new ListObservable<int>(Observable.Return(200))
        {
            100
        };
        o.AssertEqual(200, 100);
    }

    [TestMethod]
    public void Remove_None()
    {
        var o = new ListObservable<int>(Observable.Empty<int>());
        Assert.IsFalse(o.Remove(0));
        o.AssertEqual();
    }

    [TestMethod]
    public void Remove_Some_NotFound()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        Assert.IsFalse(o.Remove(100));
        o.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
    }

    [TestMethod]
    public void Remove_Some_Found()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        Assert.IsTrue(o.Remove(3));
        o.AssertEqual(0, 1, 2, 4, 5, 6, 7, 8, 9);
    }

    [TestMethod]
    public void ForEach()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        var l = new List<int>();

        foreach (var x in o)
        {
            l.Add(x);
        }

        l.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
    }

    [TestMethod]
    public void ForEach_Old()
    {
        var o = new ListObservable<int>(Observable.Range(0, 10));
        var l = new List<int>();

        foreach (int x in (IEnumerable)o)
        {
            l.Add(x);
        }

        l.AssertEqual(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);
    }

    [TestMethod]
    public void Subscribe_Never()
    {
        var s = new TestScheduler();

        var xs = s.CreateHotObservable<int>(
        );

        var results = s.Start(() => new ListObservable<int>(xs));

        results.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public void Subscribe_Infinite()
    {
        var s = new TestScheduler();

        var xs = s.CreateHotObservable(
            OnNext(300, 1)
        );

        var results = s.Start(() => new ListObservable<int>(xs));

        results.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public void Subscribe_Error()
    {
        var s = new TestScheduler();

        var ex = new Exception();

        var xs = s.CreateHotObservable(
            OnNext(300, 1),
            OnError<int>(400, ex)
        );

        var results = s.Start(() => new ListObservable<int>(xs));

        results.Messages.AssertEqual(
            OnError<object>(400, ex)
        );
    }

    [TestMethod]
    public void Subscribe_Completed()
    {
        var s = new TestScheduler();

        var xs = s.CreateHotObservable(
            OnNext(300, 1),
            OnCompleted<int>(400)
        );

        var results = s.Start(() => new ListObservable<int>(xs));

        results.Messages.AssertEqual(
            OnCompleted<object>(400)
        );
    }

    [TestMethod]
    public void Subscribe_Disposed()
    {
        var s = new TestScheduler();

        var xs = s.CreateHotObservable(
            OnNext(300, 1),
            OnCompleted<int>(1100)
        );

        var results = s.Start(() => new ListObservable<int>(xs));

        results.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public void Subscribe_Disposed_Multi()
    {
        var s = new TestScheduler();

        var xs = s.CreateHotObservable(
            OnNext(300, 1),
            OnCompleted<int>(400)
        );

        var o = new ListObservable<int>(xs);

        var results1 = s.CreateObserver<object>();
        var results2 = s.CreateObserver<object>();

        var d1 = o.Subscribe(results1);
        var d2 = o.Subscribe(results2);

        s.ScheduleAbsolute(350, () => d1.Dispose());
        s.ScheduleAbsolute(500, () => d2.Dispose());

        s.Start();

        results1.Messages.AssertEqual(
        );

        results2.Messages.AssertEqual(
        );
    }
}
