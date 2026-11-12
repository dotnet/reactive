// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.Zip(source1, ..., source3, resultSelector)</c>.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="Seq.Zip{T1, T2, T3, TResult}(Seq{T1}, Seq{T2}, Seq{T3}, Func{T1, T2, T3, TResult}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.Zip3{T1, T2, T3, TResult}(Zip3Seq{T1, T2, T3, TResult})"/>.
/// </remarks>
public sealed class Zip3Seq<T1, T2, T3, TResult>(
    Seq<T1> source1,
    Seq<T2> source2,
    Seq<T3> source3,
    Func<T1, T2, T3, TResult> resultSelector,
    string text) : Seq<TResult>
{
    /// <summary>First observable source.</summary>
    public Seq<T1> Source1 => source1;

    /// <summary>Second observable source.</summary>
    public Seq<T2> Source2 => source2;

    /// <summary>Third observable source.</summary>
    public Seq<T3> Source3 => source3;

    /// <summary>
    /// Function to invoke for each series of elements at corresponding indexes in the sources.
    /// </summary>
    public Func<T1, T2, T3, TResult> ResultSelector => resultSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Zip3(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"Seq.Zip({source1}, {source2}, {source3}, {text})";
}
