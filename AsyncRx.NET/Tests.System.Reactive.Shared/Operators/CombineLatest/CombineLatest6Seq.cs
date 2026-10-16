// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.CombineLatest(source1, ..., source6, resultSelector)</c>.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="Seq.CombineLatest{T1, T2, T3, T4, T5, T6, TResult}(Seq{T1}, Seq{T2}, Seq{T3}, Seq{T4}, Seq{T5}, Seq{T6}, Func{T1, T2, T3, T4, T5, T6, TResult}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.CombineLatest6{T1, T2, T3, T4, T5, T6, TResult}(CombineLatest6Seq{T1, T2, T3, T4, T5, T6, TResult})"/>.
/// </remarks>
public sealed class CombineLatest6Seq<T1, T2, T3, T4, T5, T6, TResult>(
    Seq<T1> source1,
    Seq<T2> source2,
    Seq<T3> source3,
    Seq<T4> source4,
    Seq<T5> source5,
    Seq<T6> source6,
    Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector,
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

    /// <summary>Function to invoke whenever any of the sources produces an element.</summary>
    public Func<T1, T2, T3, T4, T5, T6, TResult> ResultSelector => resultSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.CombineLatest6(this);

    /// <inheritdoc/>
    public override string ToString() =>
        "Seq.CombineLatest("
        + string.Join(
            ", ",
            source1, source2, source3, source4, source5, source6)
        + $", {text})";
}
