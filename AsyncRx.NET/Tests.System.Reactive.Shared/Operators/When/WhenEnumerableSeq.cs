// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>plans.When()</c> over an enumerable of plans.</summary>
/// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
/// <remarks>
/// Built by <see cref="Seq.When{TResult}(IEnumerable{SeqPlan{TResult}})"/>; materialized by each
/// target through <see cref="ISeqVisitor.WhenEnumerable{TResult}(WhenEnumerableSeq{TResult})"/>.
/// The plans are enumerated by the target's operator, not here, since a scenario may check what
/// an enumeration that throws does.
/// </remarks>
public sealed class WhenEnumerableSeq<TResult>(IEnumerable<SeqPlan<TResult>> plans) : Seq<TResult>
{
    /// <summary>A series of plans created by use of the Then operator on patterns.</summary>
    public IEnumerable<SeqPlan<TResult>> Plans => plans;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.WhenEnumerable(this);

    /// <inheritdoc/>
    public override string ToString() => "plans.When()";
}
