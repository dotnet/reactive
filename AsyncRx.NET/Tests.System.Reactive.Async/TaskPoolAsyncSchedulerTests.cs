// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Reactive.Concurrency;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Reactive.Async;

[TestClass]
public sealed class TaskPoolAsyncSchedulerTests
{
    [TestMethod]
    public async Task ScheduleAsync_SubMillisecondDueTime_DoesNotExecuteBeforeDueTime()
    {
        var tickAtScheduled = Environment.TickCount64;
        TaskCompletionSource<long> tickAtExecutedTcs = new();

        await TaskPoolAsyncScheduler.Default.ScheduleAsync(
            ct =>
            {
                tickAtExecutedTcs.SetResult(Environment.TickCount64);
                return ValueTask.CompletedTask;
            },
            TimeSpan.FromTicks(1));

        var tickAtExecuted = await tickAtExecutedTcs.Task;
        Assert.AreNotEqual(tickAtScheduled, tickAtExecuted);
    }
}
