// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.CombineLatest(source1, ..., source3, resultSelector)</c>.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="Seq.CombineLatest{T1, T2, T3, TResult}(Seq{T1}, Seq{T2}, Seq{T3}, Func{T1, T2, T3, TResult}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.CombineLatest3{T1, T2, T3, TResult}(CombineLatest3Seq{T1, T2, T3, TResult})"/>.
/// </remarks>
public sealed class CombineLatest3Seq<T1, T2, T3, TResult>(
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

    /// <summary>Function to invoke whenever any of the sources produces an element.</summary>
    public Func<T1, T2, T3, TResult> ResultSelector => resultSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.CombineLatest3(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"Seq.CombineLatest({source1}, {source2}, {source3}, {text})";
}
