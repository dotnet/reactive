// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.When(plans)</c> over an array of plans.</summary>
/// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
/// <remarks>
/// Built by <see cref="Seq.When{TResult}(SeqPlan{TResult}[])"/>; materialized by each target
/// through <see cref="ISeqVisitor.When{TResult}(WhenSeq{TResult})"/>.
/// </remarks>
public sealed class WhenSeq<TResult>(SeqPlan<TResult>[] plans) : Seq<TResult>
{
    /// <summary>A series of plans created by use of the Then operator on patterns.</summary>
    public SeqPlan<TResult>[] Plans => plans;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.When(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"Seq.When({string.Join(", ", plans.Select(p => p.ToString()))})";
}
