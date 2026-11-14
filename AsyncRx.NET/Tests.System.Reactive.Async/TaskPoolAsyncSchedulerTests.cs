// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using System.Reactive.Concurrency;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Reactive.Async;

[TestClass]
public sealed class TaskPoolAsyncSchedulerTests
{
    private static readonly MethodInfo DelayMethod = typeof(TaskPoolAsyncScheduler).GetMethod(
        "Delay",
        BindingFlags.Instance | BindingFlags.NonPublic)!;

    [TestMethod]
    public async Task Delay_SubMillisecondDueTime_DoesNotCompleteSynchronously()
    {
        var scheduler = TaskPoolAsyncScheduler.Default;
        var delayTask = (ValueTask)DelayMethod.Invoke(scheduler, [TimeSpan.FromTicks(1), CancellationToken.None])!;

        Assert.IsFalse(delayTask.IsCompleted);
        await delayTask;
    }

    [TestMethod]
    public async Task Delay_ZeroDueTime_CompletesSynchronously()
    {
        var scheduler = TaskPoolAsyncScheduler.Default;
        var delayTask = (ValueTask)DelayMethod.Invoke(scheduler, [TimeSpan.Zero, CancellationToken.None])!;

        Assert.IsTrue(delayTask.IsCompleted);
        await delayTask;
    }
}
