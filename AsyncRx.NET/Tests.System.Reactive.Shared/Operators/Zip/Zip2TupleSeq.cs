// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>SeqEx.Zip(source1, ..., source2)</c>, whose elements are tuples.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="SeqEx.Zip{T1, T2}(Seq{T1}, Seq{T2})"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.Zip2Tuple{T1, T2}(Zip2TupleSeq{T1, T2})"/>.
/// </remarks>
public sealed class Zip2TupleSeq<T1, T2>(
    Seq<T1> source1,
    Seq<T2> source2) : Seq<(T1, T2)>
{
    /// <summary>First observable source.</summary>
    public Seq<T1> Source1 => source1;

    /// <summary>Second observable source.</summary>
    public Seq<T2> Source2 => source2;

    /// <inheritdoc/>
    protected override Realized<Seq<(T1, T2)>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Zip2Tuple(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"SeqEx.Zip({source1}, {source2})";
}
