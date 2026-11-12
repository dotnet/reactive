// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.FromEventPattern(addHandler, removeHandler[, scheduler])</c> over an
/// <c>EventHandler</c> event.
/// </summary>
/// <remarks>
/// Built by the <c>Seq.FromEventPattern</c> overloads in <c>Seq.FromEventPattern.cs</c>;
/// materialized by each
/// target through <see cref="ISeqVisitor.FromEventPatternHandler(FromEventPatternHandlerSeq)"/>.
/// </remarks>
public sealed class FromEventPatternHandlerSeq(Action<EventHandler> addHandler,
        Action<EventHandler> removeHandler, SchedulerRef? scheduler)
    : Seq<EventPattern<object>>
{
    /// <summary>
    /// Action that attaches the given event handler to the underlying .NET event.
    /// </summary>
    public Action<EventHandler> AddHandler => addHandler;

    /// <summary>
    /// Action that detaches the given event handler from the underlying .NET event.
    /// </summary>
    public Action<EventHandler> RemoveHandler => removeHandler;

    /// <summary>
    /// Scheduler to run the event conversion on, or null for the target's default.
    /// </summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<EventPattern<object>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.FromEventPatternHandler(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? "Seq.FromEventPattern(addHandler, removeHandler)"
            : $"Seq.FromEventPattern(addHandler, removeHandler, {scheduler})";
}
