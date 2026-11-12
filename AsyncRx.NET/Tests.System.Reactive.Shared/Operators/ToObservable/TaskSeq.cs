// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>A task's result as a sequence, as a leaf.</summary>
/// <remarks>
/// Rx.NET's <c>task.ToObservable([scheduler])</c> over a <c>Task&lt;TResult&gt;</c>; AsyncRx.NET's
/// <c>ToAsyncObservable</c>. One element and a completion when the task completes, its error
/// when it faults, a <see cref="TaskCanceledException"/> when it is cancelled; at subscription
/// if the task is already complete, otherwise when it completes.
/// </remarks>
public sealed class TaskSeq<TResult>(Task<TResult> task, SchedulerRef? scheduler) : Seq<TResult>
{
    /// <summary>Task to convert to an observable sequence.</summary>
    public Task<TResult> Task => task;

    /// <summary>Scheduler on which to notify observers. Null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.FromTask(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null ? "task.ToSeq()" : $"task.ToSeq({scheduler})";
}
