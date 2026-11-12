// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

// Rx.NET marks its blocking operators obsolete, pointing at the async forms; these tests exist
// to cover them all the same, as the originals do under a project-wide suppression.
#pragma warning disable CS0618

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Rx.NET's own <c>ForEach</c> tests, from its <c>ForEachTest.cs</c>.</summary>
/// <remarks>
/// <c>ForEach</c> is the blocking operator: it waits on <c>ForEachAsync</c>, whose scenarios are
/// shared (Step 64). AsyncRx.NET has no blocking operators, so what is these tests' alone is the
/// blocking adapter, which only Rx.NET has, and they run here against Rx.NET only (the
/// maintainer's decision, Step 46). 10 of the file's 11 tests (the argument-checking test is the
/// code-generated stratum), as the originals but for MSTest's assertions: <c>Assert.AreSame</c> on
/// the thrown instance where the original asserts it is the one expected, and <c>Assert.Fail</c>
/// for a constant-false assert.
/// </remarks>
[TestClass]
public sealed class RxForEachTests
{
    [TestMethod]
    public void ForEach_Empty()
    {
        var lst = new List<int>();
        Observable.Empty<int>().ForEach(x => lst.Add(x));
        Assert.IsTrue(lst.SequenceEqual([]));
    }

    [TestMethod]
    public void ForEach_Index_Empty()
    {
        var lstX = new List<int>();
        Observable.Empty<int>().ForEach((x, i) => lstX.Add(x));
        Assert.IsTrue(lstX.SequenceEqual([]));
    }

    [TestMethod]
    public void ForEach_Return()
    {
        var lst = new List<int>();
        Observable.Return(42).ForEach(x => lst.Add(x));
        Assert.IsTrue(lst.SequenceEqual([42]));
    }

    [TestMethod]
    public void ForEach_Index_Return()
    {
        var lstX = new List<int>();
        var lstI = new List<int>();
        Observable.Return(42).ForEach((x, i) => { lstX.Add(x); lstI.Add(i); });
        Assert.IsTrue(lstX.SequenceEqual([42]));
        Assert.IsTrue(lstI.SequenceEqual([0]));
    }

    [TestMethod]
    public void ForEach_Throw()
    {
        var ex = new Exception();

        var xs = Observable.Throw<int>(ex);

        Assert.AreSame(ex, Assert.ThrowsExactly<Exception>(() => xs.ForEach(x => { Assert.Fail(); })));
    }

    [TestMethod]
    public void ForEach_Index_Throw()
    {
        var ex = new Exception();

        var xs = Observable.Throw<int>(ex);

        Assert.AreSame(ex, Assert.ThrowsExactly<Exception>(() => xs.ForEach((x, i) => { Assert.Fail(); })));
    }

    [TestMethod]
    public void ForEach_SomeData()
    {
        var lstX = new List<int>();
        Observable.Range(10, 10).ForEach(x => lstX.Add(x));
        Assert.IsTrue(lstX.SequenceEqual(Enumerable.Range(10, 10)));
    }

    [TestMethod]
    public void ForEach_Index_SomeData()
    {
        var lstX = new List<int>();
        var lstI = new List<int>();
        Observable.Range(10, 10).ForEach((x, i) => { lstX.Add(x); lstI.Add(i); });
        Assert.IsTrue(lstX.SequenceEqual(Enumerable.Range(10, 10)));
        Assert.IsTrue(lstI.SequenceEqual(Enumerable.Range(0, 10)));
    }

    [TestMethod]
    public void ForEach_OnNextThrows()
    {
        var ex = new Exception();

        var xs = Observable.Range(0, 10);

        Assert.AreSame(ex, Assert.ThrowsExactly<Exception>(() => xs.ForEach(x => { throw ex; })));
    }

    [TestMethod]
    public void ForEach_Index_OnNextThrows()
    {
        var ex = new Exception();

        var xs = Observable.Range(0, 10);

        Assert.AreSame(ex, Assert.ThrowsExactly<Exception>(() => xs.ForEach((x, i) => { throw ex; })));
    }
}
