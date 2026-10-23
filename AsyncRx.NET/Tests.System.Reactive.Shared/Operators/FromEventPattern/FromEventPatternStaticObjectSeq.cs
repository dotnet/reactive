// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.FromEventPattern(type, eventName[, scheduler])</c> over a static event found by
/// name.
/// </summary>
/// <remarks>
/// Built by the <c>Seq.FromEventPattern</c> overloads in <c>Seq.FromEventPattern.cs</c>;
/// materialized by each
/// target through <see cref="ISeqVisitor.FromEventPatternStaticObject(FromEventPatternStaticObjectSeq)"/>.
/// </remarks>
public sealed class FromEventPatternStaticObjectSeq(Type type, string eventName,
        SchedulerRef? scheduler)
    : Seq<EventPattern<object>>
{
    /// <summary>Type that exposes the static event to convert.</summary>
    public Type Type => type;

    /// <summary>Name of the event to convert.</summary>
    public string EventName => eventName;

    /// <summary>
    /// Scheduler to run the event conversion on, or null for the target's default.
    /// </summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<EventPattern<object>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.FromEventPatternStaticObject(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? "Seq.FromEventPattern(type, eventName)"
            : $"Seq.FromEventPattern(type, eventName, {scheduler})";
}
