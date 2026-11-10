// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.FromEventPattern(target, eventName[, scheduler])</c> over an instance event
/// found by name.
/// </summary>
/// <remarks>
/// Built by the <c>Seq.FromEventPattern</c> overloads in <c>Seq.FromEventPattern.cs</c>;
/// materialized by each
/// target through <see cref="ISeqVisitor.FromEventPatternInstanceObject(FromEventPatternInstanceObjectSeq)"/>.
/// </remarks>
public sealed class FromEventPatternInstanceObjectSeq(object target, string eventName,
        SchedulerRef? scheduler)
    : Seq<EventPattern<object>>
{
    /// <summary>Object instance that exposes the event to convert.</summary>
    public object Target => target;

    /// <summary>Name of the event to convert.</summary>
    public string EventName => eventName;

    /// <summary>
    /// Scheduler to run the event conversion on, or null for the target's default.
    /// </summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<EventPattern<object>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.FromEventPatternInstanceObject(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? "Seq.FromEventPattern(target, eventName)"
            : $"Seq.FromEventPattern(target, eventName, {scheduler})";
}
