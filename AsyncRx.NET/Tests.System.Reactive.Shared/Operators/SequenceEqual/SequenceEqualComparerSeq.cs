// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>first.SequenceEqual(second, comparer)</c> over two sequences.</summary>
/// <remarks>
/// Built by <see cref="SequenceEqualExtensions.SequenceEqual{T}(Seq{T}, Seq{T}, IEqualityComparer{T})"/>; materialized by each target through <see cref="ISeqVisitor.SequenceEqualComparer{T}(SequenceEqualComparerSeq{T})"/>.
/// </remarks>
public sealed class SequenceEqualComparerSeq<T>(
    Seq<T> first,
    Seq<T> second,
    IEqualityComparer<T> comparer) : Seq<bool>
{
    /// <summary>The first sequence.</summary>
    public Seq<T> First => first;

    /// <summary>What the first sequence is compared with.</summary>
    public Seq<T> Second => second;

    /// <summary>The comparer for the elements.</summary>
    public IEqualityComparer<T> Comparer => comparer;

    /// <inheritdoc/>
    protected override Realized<Seq<bool>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SequenceEqualComparer(this);

    /// <inheritdoc/>
    public override string ToString() => $"{first}.SequenceEqual({SecondText})";

    private string SecondText =>
        (second.ToString())
        + ", " + comparer.GetType().Name;
}
