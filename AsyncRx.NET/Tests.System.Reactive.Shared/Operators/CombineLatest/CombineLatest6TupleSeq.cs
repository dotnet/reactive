// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>SeqEx.CombineLatest(source1, ..., source6)</c>, whose elements are tuples.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="SeqEx.CombineLatest{T1, T2, T3, T4, T5, T6}(Seq{T1}, Seq{T2}, Seq{T3}, Seq{T4}, Seq{T5}, Seq{T6})"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.CombineLatest6Tuple{T1, T2, T3, T4, T5, T6}(CombineLatest6TupleSeq{T1, T2, T3, T4, T5, T6})"/>.
/// </remarks>
public sealed class CombineLatest6TupleSeq<T1, T2, T3, T4, T5, T6>(
    Seq<T1> source1,
    Seq<T2> source2,
    Seq<T3> source3,
    Seq<T4> source4,
    Seq<T5> source5,
    Seq<T6> source6) : Seq<(T1, T2, T3, T4, T5, T6)>
{
    /// <summary>First observable source.</summary>
    public Seq<T1> Source1 => source1;

    /// <summary>Second observable source.</summary>
    public Seq<T2> Source2 => source2;

    /// <summary>Third observable source.</summary>
    public Seq<T3> Source3 => source3;

    /// <summary>Fourth observable source.</summary>
    public Seq<T4> Source4 => source4;

    /// <summary>Fifth observable source.</summary>
    public Seq<T5> Source5 => source5;

    /// <summary>Sixth observable source.</summary>
    public Seq<T6> Source6 => source6;

    /// <inheritdoc/>
    protected override Realized<Seq<(T1, T2, T3, T4, T5, T6)>> AcceptCore(ISeqVisitor visitor) =>
        visitor.CombineLatest6Tuple(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"SeqEx.CombineLatest({source1}, {source2}, {source3}, {source4}, {source5}, {source6})";
}
