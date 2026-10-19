// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.FromEvent(addHandler, removeHandler[, scheduler])</c> over an <c>Action</c>
/// event, producing <c>Unit</c>.
/// </summary>
/// <remarks>
/// Built by the <c>Seq.FromEvent</c> overloads in <c>Seq.FromEvent.cs</c>; materialized by each
/// target through <see cref="ISeqVisitor.FromEventUnit(FromEventUnitSeq)"/>.
/// </remarks>
public sealed class FromEventUnitSeq(Action<Action> addHandler, Action<Action> removeHandler,
        SchedulerRef? scheduler)
    : Seq<Unit>
{
    /// <summary>
    /// Action that attaches the given event handler to the underlying .NET event.
    /// </summary>
    public Action<Action> AddHandler => addHandler;

    /// <summary>
    /// Action that detaches the given event handler from the underlying .NET event.
    /// </summary>
    public Action<Action> RemoveHandler => removeHandler;

    /// <summary>
    /// Scheduler to run the event conversion on, or null for the target's default.
    /// </summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<Unit>> AcceptCore(ISeqVisitor visitor) =>
        visitor.FromEventUnit(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? "Seq.FromEvent(addHandler, removeHandler)"
            : $"Seq.FromEvent(addHandler, removeHandler, {scheduler})";
}
