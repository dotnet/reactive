// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

// Rx.NET marks its blocking operators obsolete, pointing at the async forms; these tests exist
// to cover them all the same, as the originals do under a project-wide suppression.
#pragma warning disable CS0618

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Rx.NET's own <c>First</c> tests, from its <c>FirstTest.cs</c>.</summary>
/// <remarks>
/// <c>First</c> is the blocking operator: it waits for the result of <c>FirstAsync</c> and
/// returns it. AsyncRx.NET has no blocking operators, and what these tests claim about the
/// element chosen is what the shared <c>FirstAsync</c> scenarios claim in virtual time; what is
/// theirs alone is the blocking adapter, which only Rx.NET has, so they run here against
/// Rx.NET only (the maintainer's decision, Step 46). 9 of the file's 10 tests (the
/// argument-checking test is the code-generated stratum), as the originals but for MSTest's
/// assertions: <c>Assert.ThrowsExactly</c> for <c>ReactiveAssert.Throws</c>, and
/// <c>Assert.AreSame</c> on the thrown instance where the original asserts it is the one
/// expected.
/// </remarks>
[TestClass]
public sealed class RxFirstTests
{
    [TestMethod]
    public void First_Empty()
    {
        Assert.ThrowsExactly<InvalidOperationException>(() => Observable.Empty<int>().First());
    }

    [TestMethod]
    public void FirstPredicate_Empty()
    {
        Assert.ThrowsExactly<InvalidOperationException>(
            () => Observable.Empty<int>().First(_ => true));
    }

    [TestMethod]
    public void First_Return()
    {
        var value = 42;
        Assert.AreEqual(value, Observable.Return(value).First());
    }

    [TestMethod]
    public void FirstPredicate_Return()
    {
        var value = 42;
        Assert.AreEqual(value, Observable.Return(value).First(i => i % 2 == 0));
    }

    [TestMethod]
    public void FirstPredicate_Return_NoMatch()
    {
        var value = 42;
        Assert.ThrowsExactly<InvalidOperationException>(
            () => Observable.Return(value).First(i => i % 2 != 0));
    }

    [TestMethod]
    public void First_Throw()
    {
        var ex = new Exception();

        var xs = Observable.Throw<int>(ex);

        Assert.AreSame(ex, Assert.ThrowsExactly<Exception>(() => xs.First()));
    }

    [TestMethod]
    public void FirstPredicate_Throw()
    {
        var ex = new Exception();

        var xs = Observable.Throw<int>(ex);

        Assert.AreSame(ex, Assert.ThrowsExactly<Exception>(() => xs.First(_ => true)));
    }

    [TestMethod]
    public void First_Range()
    {
        var value = 42;
        Assert.AreEqual(value, Observable.Range(value, 10).First());
    }

    [TestMethod]
    public void FirstPredicate_Range()
    {
        var value = 42;
        Assert.AreEqual(46, Observable.Range(value, 10).First(i => i > 45));
    }
}
