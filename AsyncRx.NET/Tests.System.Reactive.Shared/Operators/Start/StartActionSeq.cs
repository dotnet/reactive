// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.Start(action[, scheduler])</c> over an action, run at the call.
/// </summary>
/// <remarks>
/// Built by the <c>Seq.Start</c> overloads in <c>Seq.Start.cs</c>; materialized by each
/// target through <see cref="ISeqVisitor.StartAction(StartActionSeq)"/>.
/// </remarks>
public sealed class StartActionSeq(Action action, SchedulerRef? scheduler)
    : Seq<Unit>
{
    /// <summary>Action to run asynchronously.</summary>
    public Action Action => action;

    /// <summary>Scheduler to run the function on. Null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<Unit>> AcceptCore(ISeqVisitor visitor) =>
        visitor.StartAction(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? "Seq.Start(action)"
            : $"Seq.Start(action, {scheduler})";
}
