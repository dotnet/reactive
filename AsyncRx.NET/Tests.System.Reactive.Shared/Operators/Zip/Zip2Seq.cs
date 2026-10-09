// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.Zip(source1, ..., source2, resultSelector)</c>.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="Seq.Zip{T1, T2, TResult}(Seq{T1}, Seq{T2}, Func{T1, T2, TResult}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.Zip2{T1, T2, TResult}(Zip2Seq{T1, T2, TResult})"/>.
/// </remarks>
public sealed class Zip2Seq<T1, T2, TResult>(
    Seq<T1> source1,
    Seq<T2> source2,
    Func<T1, T2, TResult> resultSelector,
    string text) : Seq<TResult>
{
    /// <summary>First observable source.</summary>
    public Seq<T1> Source1 => source1;

    /// <summary>Second observable source.</summary>
    public Seq<T2> Source2 => source2;

    /// <summary>
    /// Function to invoke for each consecutive pair of elements from the first and second source.
    /// </summary>
    public Func<T1, T2, TResult> ResultSelector => resultSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Zip2(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"Seq.Zip({source1}, {source2}, {text})";
}
