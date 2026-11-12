// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>A task's completion as a sequence of one <see cref="Unit"/>, as a leaf.</summary>
/// <remarks>
/// Rx.NET's <c>task.ToObservable([scheduler])</c> over a plain <c>Task</c>; AsyncRx.NET's
/// <c>ToAsyncObservable</c>. As <see cref="TaskSeq{TResult}"/>, with <see cref="Unit.Default"/>
/// for the element.
/// </remarks>
public sealed class VoidTaskSeq(Task task, SchedulerRef? scheduler) : Seq<Unit>
{
    /// <summary>Task to convert to an observable sequence.</summary>
    public Task Task => task;

    /// <summary>Scheduler on which to notify observers. Null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<Unit>> AcceptCore(ISeqVisitor visitor) =>
        visitor.FromVoidTask(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null ? "task.ToSeq()" : $"task.ToSeq({scheduler})";
}
