// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Rx;

/// <summary>
/// Rx.NET's <c>SynchronousScheduler</c> from <c>CreateAsyncTest.cs</c>: a task scheduler that
/// runs every task inline on the queueing thread.
/// </summary>
/// <remarks>
/// <see cref="RxTarget.RunSynchronously"/> runs a scenario body on it, as the original tests
/// do, so that the continuations Rx.NET's task-based <c>Create</c> hangs off the callback's
/// task, which follow the current task scheduler, run inline on the test scheduler's thread.
/// </remarks>
internal sealed class SynchronousScheduler : TaskScheduler
{
    protected override IEnumerable<Task> GetScheduledTasks()
    {
        throw new NotImplementedException();
    }

    protected override void QueueTask(Task task)
    {
        TryExecuteTask(task);
    }

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        return TryExecuteTask(task);
    }
}
