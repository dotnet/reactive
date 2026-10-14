// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

// Rx.NET marks its blocking operators obsolete, pointing at the async forms; these tests exist
// to cover them all the same, as the originals do under a project-wide suppression.
#pragma warning disable CS0618

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Rx.NET's own <c>SingleOrDefault</c> tests, from its <c>SingleOrDefaultTest.cs</c>.</summary>
/// <remarks>
/// <c>SingleOrDefault</c> is the blocking operator: it waits for the result of <c>SingleOrDefaultAsync</c> and
/// returns it. AsyncRx.NET has no blocking operators, and what these tests claim about the
/// element chosen is what the shared <c>SingleOrDefaultAsync</c> scenarios claim in virtual time; what is
/// theirs alone is the blocking adapter, which only Rx.NET has, so they run here against
/// Rx.NET only (the maintainer's decision, Step 46). 8 of the file's 9 tests (the
/// argument-checking test is the code-generated stratum), as the originals but for MSTest's
/// assertions: <c>Assert.ThrowsExactly</c> for <c>ReactiveAssert.Throws</c>, and
/// <c>Assert.AreSame</c> on the thrown instance where the original asserts it is the one
/// expected.
/// </remarks>
[TestClass]
public sealed class RxSingleOrDefaultTests
{
    [TestMethod]
    public void SingleOrDefault_Empty()
    {
        Assert.AreEqual(default, Observable.Empty<int>().SingleOrDefault());
    }

    [TestMethod]
    public void SingleOrDefaultPredicate_Empty()
    {
        Assert.AreEqual(default, Observable.Empty<int>().SingleOrDefault(_ => true));
    }

    [TestMethod]
    public void SingleOrDefault_Return()
    {
        var value = 42;
        Assert.AreEqual(value, Observable.Return(value).SingleOrDefault());
    }

    [TestMethod]
    public void SingleOrDefault_Throw()
    {
        var ex = new Exception();

        var xs = Observable.Throw<int>(ex);

        Assert.AreSame(ex, Assert.ThrowsExactly<Exception>(() => xs.SingleOrDefault()));
    }

    [TestMethod]
    public void SingleOrDefault_Range()
    {
        var value = 42;
        Assert.ThrowsExactly<InvalidOperationException>(
            () => Observable.Range(value, 10).SingleOrDefault());
    }

    [TestMethod]
    public void SingleOrDefaultPredicate_Range()
    {
        var value = 42;
        Assert.ThrowsExactly<InvalidOperationException>(
            () => Observable.Range(value, 10).SingleOrDefault(i => i % 2 == 0));
    }

    [TestMethod]
    public void SingleOrDefault_Range_ReducesToSingle()
    {
        var value = 42;
        Assert.AreEqual(45, Observable.Range(value, 10).SingleOrDefault(i => i == 45));
    }

    [TestMethod]
    public void SingleOrDefault_Range_ReducesToNone()
    {
        var value = 42;
        Assert.AreEqual(0, Observable.Range(value, 10).SingleOrDefault(i => i > 100));
    }
}
