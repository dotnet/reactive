// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.Zip(source1, ..., source7, resultSelector)</c>.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="Seq.Zip{T1, T2, T3, T4, T5, T6, T7, TResult}(Seq{T1}, Seq{T2}, Seq{T3}, Seq{T4}, Seq{T5}, Seq{T6}, Seq{T7}, Func{T1, T2, T3, T4, T5, T6, T7, TResult}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.Zip7{T1, T2, T3, T4, T5, T6, T7, TResult}(Zip7Seq{T1, T2, T3, T4, T5, T6, T7, TResult})"/>.
/// </remarks>
public sealed class Zip7Seq<T1, T2, T3, T4, T5, T6, T7, TResult>(
    Seq<T1> source1,
    Seq<T2> source2,
    Seq<T3> source3,
    Seq<T4> source4,
    Seq<T5> source5,
    Seq<T6> source6,
    Seq<T7> source7,
    Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector,
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

    /// <summary>
    /// Function to invoke for each series of elements at corresponding indexes in the sources.
    /// </summary>
    public Func<T1, T2, T3, T4, T5, T6, T7, TResult> ResultSelector => resultSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Zip7(this);

    /// <inheritdoc/>
    public override string ToString() =>
        "Seq.Zip("
        + string.Join(
            ", ",
            source1, source2, source3, source4, source5, source6, source7)
        + $", {text})";
}
