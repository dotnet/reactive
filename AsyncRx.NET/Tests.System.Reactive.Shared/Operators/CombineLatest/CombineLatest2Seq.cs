// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.CombineLatest(source1, ..., source2, resultSelector)</c>.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="Seq.CombineLatest{T1, T2, TResult}(Seq{T1}, Seq{T2}, Func{T1, T2, TResult}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.CombineLatest2{T1, T2, TResult}(CombineLatest2Seq{T1, T2, TResult})"/>.
/// </remarks>
public sealed class CombineLatest2Seq<T1, T2, TResult>(
    Seq<T1> source1,
    Seq<T2> source2,
    Func<T1, T2, TResult> resultSelector,
    string text) : Seq<TResult>
{
    /// <summary>First observable source.</summary>
    public Seq<T1> Source1 => source1;

    /// <summary>Second observable source.</summary>
    public Seq<T2> Source2 => source2;

    /// <summary>Function to invoke whenever any of the sources produces an element.</summary>
    public Func<T1, T2, TResult> ResultSelector => resultSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.CombineLatest2(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"Seq.CombineLatest({source1}, {source2}, {text})";
}
