// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.If(condition, thenSource[, elseSource | scheduler])</c>.</summary>
/// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
/// <remarks>
/// Built by the <c>Seq.If</c> overloads in <c>Seq.If.cs</c>; materialized by each target through
/// <see cref="ISeqVisitor.If{TResult}(IfSeq{TResult})"/>. At most one of <see cref="ElseSource"/>
/// and <see cref="Scheduler"/> is set; with neither, the else branch is the target's own empty
/// sequence on its default scheduler.
/// </remarks>
public sealed class IfSeq<TResult>(
    Func<bool> condition,
    Seq<TResult> thenSource,
    Seq<TResult>? elseSource,
    SchedulerRef? scheduler,
    string text)
    : Seq<TResult>
{
    /// <summary>Condition evaluated to decide which of the different sources to return.</summary>
    public Func<bool> Condition => condition;

    /// <summary>Sequence returned in case the condition evaluates true.</summary>
    public Seq<TResult> ThenSource => thenSource;

    /// <summary>Sequence returned in case the condition evaluates false, or null.</summary>
    public Seq<TResult>? ElseSource => elseSource;

    /// <summary>
    /// Scheduler to generate an empty sequence on in case the condition evaluates false, or null.
    /// </summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) => visitor.If(this);

    /// <inheritdoc/>
    public override string ToString() =>
        elseSource is not null ? $"Seq.If({text}, {thenSource}, {elseSource})"
        : scheduler is not null ? $"Seq.If({text}, {thenSource}, {scheduler})"
        : $"Seq.If({text}, {thenSource})";
}
