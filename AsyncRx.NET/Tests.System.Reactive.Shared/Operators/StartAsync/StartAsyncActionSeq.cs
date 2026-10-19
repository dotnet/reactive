// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.StartAsync(actionAsync[, scheduler])</c> over an asynchronous action, run at
/// the call.
/// </summary>
/// <remarks>
/// Built by the <c>Seq.StartAsync</c> overloads in <c>Seq.StartAsync.cs</c>; materialized by each
/// target through <see cref="ISeqVisitor.StartAsyncAction(StartAsyncActionSeq)"/>.
/// </remarks>
public sealed class StartAsyncActionSeq(Func<Task> actionAsync, SchedulerRef? scheduler)
    : Seq<Unit>
{
    /// <summary>Asynchronous action to run.</summary>
    public Func<Task> ActionAsync => actionAsync;

    /// <summary>Scheduler on which to notify observers. Null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<Unit>> AcceptCore(ISeqVisitor visitor) =>
        visitor.StartAsyncAction(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? "Seq.StartAsync(actionAsync)"
            : $"Seq.StartAsync(actionAsync, {scheduler})";
}
