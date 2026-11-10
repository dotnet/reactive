// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.CombineLatest(source1, ..., source11, resultSelector)</c>.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="Seq.CombineLatest{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}(Seq{T1}, Seq{T2}, Seq{T3}, Seq{T4}, Seq{T5}, Seq{T6}, Seq{T7}, Seq{T8}, Seq{T9}, Seq{T10}, Seq{T11}, Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.CombineLatest11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}(CombineLatest11Seq{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult})"/>.
/// </remarks>
public sealed class CombineLatest11Seq<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
    Seq<T1> source1,
    Seq<T2> source2,
    Seq<T3> source3,
    Seq<T4> source4,
    Seq<T5> source5,
    Seq<T6> source6,
    Seq<T7> source7,
    Seq<T8> source8,
    Seq<T9> source9,
    Seq<T10> source10,
    Seq<T11> source11,
    Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> resultSelector,
    string text) : Seq<TResult>
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

    /// <summary>Seventh observable source.</summary>
    public Seq<T7> Source7 => source7;

    /// <summary>Eighth observable source.</summary>
    public Seq<T8> Source8 => source8;

    /// <summary>Ninth observable source.</summary>
    public Seq<T9> Source9 => source9;

    /// <summary>Tenth observable source.</summary>
    public Seq<T10> Source10 => source10;

    /// <summary>Eleventh observable source.</summary>
    public Seq<T11> Source11 => source11;

    /// <summary>Function to invoke whenever any of the sources produces an element.</summary>
    public Func<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult
    > ResultSelector => resultSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.CombineLatest11(this);

    /// <inheritdoc/>
    public override string ToString() =>
        "Seq.CombineLatest("
        + string.Join(
            ", ",
            source1, source2, source3, source4, source5, source6, source7, source8, source9,
            source10, source11)
        + $", {text})";
}
