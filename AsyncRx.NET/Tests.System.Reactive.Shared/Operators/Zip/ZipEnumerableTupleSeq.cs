// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>SeqEx.Zip(first, second)</c> where the second source is an enumerable, whose
/// elements are pairs.
/// </summary>
/// <remarks>
/// Built by <see cref="SeqEx.Zip{T1, T2}(Seq{T1}, IEnumerable{T2})"/>; materialized by
/// each target through
/// <see cref="ISeqVisitor.ZipEnumerableTuple{T1, T2}(ZipEnumerableTupleSeq{T1, T2})"/>.
/// </remarks>
public sealed class ZipEnumerableTupleSeq<T1, T2>(Seq<T1> first, IEnumerable<T2> second)
    : Seq<(T1, T2)>
{
    /// <summary>First observable source.</summary>
    public Seq<T1> First => first;

    /// <summary>Second enumerable source.</summary>
    public IEnumerable<T2> Second => second;

    /// <inheritdoc/>
    protected override Realized<Seq<(T1, T2)>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ZipEnumerableTuple(this);

    /// <inheritdoc/>
    public override string ToString() => $"SeqEx.Zip({first}, {second.GetType().Name})";
}
