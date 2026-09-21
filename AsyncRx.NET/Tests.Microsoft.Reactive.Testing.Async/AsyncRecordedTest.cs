// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using Microsoft.Reactive.Testing.Async;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Microsoft.Reactive.Testing.Async;

[TestClass]
public class AsyncRecordedTest
{
    [TestMethod]
    public void Equal_when_time_and_value_match()
    {
        var a = new AsyncRecorded<string>(100, 110, "x");
        var b = new AsyncRecorded<string>(100, 110, "x");

        Assert.AreEqual(a, b);
        Assert.IsTrue(a == b);
        Assert.IsFalse(a != b);
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }

    [TestMethod]
    public void Not_equal_when_value_differs()
    {
        var a = new AsyncRecorded<string>(100, 110, "x");
        var b = new AsyncRecorded<string>(100, 110, "y");

        Assert.AreNotEqual(a, b);
        Assert.IsTrue(a != b);
    }

    [TestMethod]
    public void Not_equal_when_time_differs()
    {
        var a = new AsyncRecorded<string>(100, 110, "x");
        var b = new AsyncRecorded<string>(100, 120, "x");

        Assert.AreNotEqual(a, b);
    }

    [TestMethod]
    public void Null_values_compare_equal_without_throwing()
    {
        // T is unconstrained, so a null value must be a legitimate recorded value, as it is
        // for the sync Recorded<T>.
        var a = new AsyncRecorded<string?>(100, 110, null);
        var b = new AsyncRecorded<string?>(100, 110, null);

        Assert.AreEqual(a, b);
        Assert.IsTrue(a == b);
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }

    [TestMethod]
    public void Null_and_non_null_values_are_not_equal_in_either_order()
    {
        var withNull = new AsyncRecorded<string?>(100, 110, null);
        var withValue = new AsyncRecorded<string?>(100, 110, "x");

        Assert.AreNotEqual(withNull, withValue);
        Assert.AreNotEqual(withValue, withNull);
    }
}
