// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.Zip(sources, resultSelector)</c> over a sequence of sources.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="Seq.Zip{T, TResult}(IEnumerable{Seq{T}}, Func{IList{T}, TResult}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.ZipListSelector{T, TResult}(ZipListSelectorSeq{T, TResult})"/>.
/// </remarks>
public sealed class ZipListSelectorSeq<T, TResult>(
    IEnumerable<Seq<T>> sources,
    Func<IList<T>, TResult> resultSelector,
    string text) : Seq<TResult>
{
    /// <summary>Observable sources.</summary>
    public IEnumerable<Seq<T>> Sources => sources;

    /// <summary>
    /// Function to invoke for each series of elements at corresponding indexes in the sources.
    /// </summary>
    public Func<IList<T>, TResult> ResultSelector => resultSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ZipListSelector(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"Seq.Zip([{string.Join(", ", sources)}], {text})";
}
