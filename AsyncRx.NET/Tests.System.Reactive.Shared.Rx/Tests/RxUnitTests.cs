// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// Rx.NET's own <c>Unit</c> test, from its <c>UnitTest.cs</c>.
/// </summary>
/// <remarks>
/// The file's one test, as the original but for MSTest's assertions, against Rx.NET only:
/// <c>Unit</c> is Rx.NET's struct, which AsyncRx.NET references rather than redefines, so its test
/// holds for both by running once, here.
/// </remarks>
[TestClass]
public sealed class RxUnitTests
{
    [TestMethod]
    public void Unit()
    {
        var u1 = new Unit();
        var u2 = new Unit();
        Assert.IsTrue(u1.Equals(u2));
        Assert.IsFalse(u1.Equals(""));
        Assert.IsFalse(u1.Equals(null));
        Assert.IsTrue(u1 == u2);
        Assert.IsFalse(u1 != u2);
        Assert.AreEqual(0, u1.GetHashCode());
        Assert.AreEqual("()", u1.ToString());
    }
}
