// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.CombineLatest(sources, resultSelector)</c> over a sequence of sources.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="Seq.CombineLatest{T, TResult}(IEnumerable{Seq{T}}, Func{IList{T}, TResult}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.CombineLatestListSelector{T, TResult}(CombineLatestListSelectorSeq{T, TResult})"/>.
/// </remarks>
public sealed class CombineLatestListSelectorSeq<T, TResult>(
    IEnumerable<Seq<T>> sources,
    Func<IList<T>, TResult> resultSelector,
    string text) : Seq<TResult>
{
    /// <summary>Observable sources.</summary>
    public IEnumerable<Seq<T>> Sources => sources;

    /// <summary>Function to invoke whenever any of the sources produces an element.</summary>
    public Func<IList<T>, TResult> ResultSelector => resultSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.CombineLatestListSelector(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"Seq.CombineLatest([{string.Join(", ", sources)}], {text})";
}
