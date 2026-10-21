// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>first.SequenceEqual(second)</c> against an enumerable.</summary>
/// <remarks>
/// Built by <see cref="SequenceEqualExtensions.SequenceEqual{T}(Seq{T}, IEnumerable{T})"/>; materialized by each target through <see cref="ISeqVisitor.SequenceEqualEnumerable{T}(SequenceEqualEnumerableSeq{T})"/>.
/// </remarks>
public sealed class SequenceEqualEnumerableSeq<T>(
    Seq<T> first,
    IEnumerable<T> second) : Seq<bool>
{
    /// <summary>The first sequence.</summary>
    public Seq<T> First => first;

    /// <summary>What the first sequence is compared with.</summary>
    /// <remarks>
    /// Printed only when it is a collection: an iterator is not enumerated for the query text,
    /// since the scenarios check when, and whether, the operator enumerates it.
    /// </remarks>
    public IEnumerable<T> Second => second;

    /// <inheritdoc/>
    protected override Realized<Seq<bool>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SequenceEqualEnumerable(this);

    /// <inheritdoc/>
    public override string ToString() => $"{first}.SequenceEqual({SecondText})";

    private string SecondText =>
        second is ICollection<T> ? "[" + string.Join(", ", second) + "]" : second.GetType().Name;
}
