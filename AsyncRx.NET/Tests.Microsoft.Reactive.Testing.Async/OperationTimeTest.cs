// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing.Async;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Microsoft.Reactive.Testing.Async;

/// <summary>
/// Tests for the common start/end record: an operation that never began, one that began but
/// never completed, a logically instantaneous one, and a prolonged one.
/// </summary>
[TestClass]
public class OperationTimeTest
{
    [TestMethod]
    public void Never_has_not_started_and_is_not_complete()
    {
        var time = OperationTime.Never;

        Assert.IsFalse(time.HasStarted);
        Assert.IsFalse(time.IsComplete);
        Assert.IsFalse(time.IsInstantaneous);
        Assert.IsFalse(time.IsProlonged);
        Assert.AreEqual("never", time.ToString());
    }

    [TestMethod]
    public void A_started_operation_is_incomplete_until_completed()
    {
        var time = OperationTime.StartingAt(210);

        Assert.IsTrue(time.HasStarted);
        Assert.IsFalse(time.IsComplete);
        Assert.IsFalse(time.IsInstantaneous);
        Assert.IsFalse(time.IsProlonged);
        Assert.AreEqual("210→incomplete", time.ToString());
    }

    [TestMethod]
    public void An_operation_completing_at_its_start_time_is_logically_instantaneous()
    {
        var time = OperationTime.StartingAt(210).CompletedAt(210);

        Assert.IsTrue(time.IsComplete);
        Assert.IsTrue(time.IsInstantaneous);
        Assert.IsFalse(time.IsProlonged);
        Assert.AreEqual("210", time.ToString());
    }

    [TestMethod]
    public void An_operation_completing_later_than_it_started_is_prolonged()
    {
        var time = OperationTime.StartingAt(210).CompletedAt(250);

        Assert.IsTrue(time.IsComplete);
        Assert.IsFalse(time.IsInstantaneous);
        Assert.IsTrue(time.IsProlonged);
        Assert.AreEqual("210→250", time.ToString());
    }

    [TestMethod]
    public void Value_equality_covers_both_timestamps()
    {
        Assert.AreEqual(new OperationTime(210, 250), new OperationTime(210, 250));
        Assert.AreNotEqual(new OperationTime(210, 250), new OperationTime(210, 251));
        Assert.AreNotEqual(new OperationTime(210, 250), new OperationTime(211, 250));
        Assert.AreEqual(OperationTime.Never, new OperationTime(OperationTime.Infinite, OperationTime.Infinite));
    }
}
