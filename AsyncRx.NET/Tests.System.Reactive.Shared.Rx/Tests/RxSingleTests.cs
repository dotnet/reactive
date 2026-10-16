// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

// Rx.NET marks its blocking operators obsolete, pointing at the async forms; these tests exist
// to cover them all the same, as the originals do under a project-wide suppression.
#pragma warning disable CS0618

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Rx.NET's own <c>Single</c> tests, from its <c>SingleTest.cs</c>.</summary>
/// <remarks>
/// <c>Single</c> is the blocking operator: it waits for the result of <c>SingleAsync</c> and
/// returns it. AsyncRx.NET has no blocking operators, and what these tests claim about the
/// element chosen is what the shared <c>SingleAsync</c> scenarios claim in virtual time; what is
/// theirs alone is the blocking adapter, which only Rx.NET has, so they run here against
/// Rx.NET only (the maintainer's decision, Step 46). 7 of the file's 8 tests (the
/// argument-checking test is the code-generated stratum), as the originals but for MSTest's
/// assertions: <c>Assert.ThrowsExactly</c> for <c>ReactiveAssert.Throws</c>, and
/// <c>Assert.AreSame</c> on the thrown instance where the original asserts it is the one
/// expected.
/// </remarks>
[TestClass]
public sealed class RxSingleTests
{
    [TestMethod]
    public void Single_Empty()
    {
        Assert.ThrowsExactly<InvalidOperationException>(() => Observable.Empty<int>().Single());
    }

    [TestMethod]
    public void SinglePredicate_Empty()
    {
        Assert.ThrowsExactly<InvalidOperationException>(
            () => Observable.Empty<int>().Single(_ => true));
    }

    [TestMethod]
    public void Single_Return()
    {
        var value = 42;
        Assert.AreEqual(value, Observable.Return(value).Single());
    }

    [TestMethod]
    public void Single_Throw()
    {
        var ex = new Exception();

        var xs = Observable.Throw<int>(ex);

        Assert.AreSame(ex, Assert.ThrowsExactly<Exception>(() => xs.Single()));
    }

    [TestMethod]
    public void Single_Range()
    {
        var value = 42;
        Assert.ThrowsExactly<InvalidOperationException>(() => Observable.Range(value, 10).Single());
    }

    [TestMethod]
    public void SinglePredicate_Range()
    {
        var value = 42;
        Assert.ThrowsExactly<InvalidOperationException>(
            () => Observable.Range(value, 10).Single(i => i % 2 == 0));
    }

    [TestMethod]
    public void SinglePredicate_Range_ReducesToSingle()
    {
        var value = 42;
        Assert.AreEqual(45, Observable.Range(value, 10).Single(i => i == 45));
    }
}
