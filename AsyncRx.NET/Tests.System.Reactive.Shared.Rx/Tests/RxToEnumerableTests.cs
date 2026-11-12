// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Rx.NET's own <c>ToEnumerable</c> tests, from its <c>ToEnumerableTest.cs</c>.</summary>
/// <remarks>
/// <c>ToEnumerable</c> is the push-to-pull adapter over a buffer: every element is yielded, each
/// <c>MoveNext</c> blocking until one is there. AsyncRx.NET has no pull side (an
/// <c>IAsyncEnumerable</c> bridge would be its form, a separate question), so these run here
/// against Rx.NET only, under the Step 46 decision for the blocking operators. 5 of the file's 6
/// tests (the argument-checking test is the code-generated stratum), as the originals but for
/// MSTest's assertions.
/// </remarks>
[TestClass]
public sealed class RxToEnumerableTests
{
    [TestMethod]
    public void ToEnumerable_Generic()
    {
        Assert.IsTrue(Observable.Range(0, 10).ToEnumerable().SequenceEqual(Enumerable.Range(0, 10)));
    }

    [TestMethod]
    public void ToEnumerable_NonGeneric()
    {
        Assert.IsTrue(((IEnumerable)Observable.Range(0, 10).ToEnumerable()).Cast<int>().SequenceEqual(Enumerable.Range(0, 10)));
    }

    [TestMethod]
    public void ToEnumerable_ManualGeneric()
    {
        var res = Observable.Range(0, 10).ToEnumerable();
        var ieg = res.GetEnumerator();
        for (var i = 0; i < 10; i++)
        {
            Assert.IsTrue(ieg.MoveNext());
            Assert.AreEqual(i, ieg.Current);
        }
        Assert.IsFalse(ieg.MoveNext());
    }

    [TestMethod]
    public void ToEnumerable_ManualNonGeneric()
    {
        var res = (IEnumerable)Observable.Range(0, 10).ToEnumerable();
        var ien = res.GetEnumerator();
        for (var i = 0; i < 10; i++)
        {
            Assert.IsTrue(ien.MoveNext());
            Assert.AreEqual(i, ien.Current);
        }
        Assert.IsFalse(ien.MoveNext());
    }

    [TestMethod]
    public void ToEnumerable_ResetNotSupported()
    {
        Assert.ThrowsExactly<NotSupportedException>(() => Observable.Range(0, 10).ToEnumerable().GetEnumerator().Reset());
    }
}
