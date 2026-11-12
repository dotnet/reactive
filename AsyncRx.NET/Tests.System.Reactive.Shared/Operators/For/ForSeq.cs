// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.For(source, resultSelector)</c>.</summary>
/// <typeparam name="TSource">The type of the elements in the enumerable source.</typeparam>
/// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
/// <remarks>
/// Built by <see cref="Seq.For{TSource, TResult}(IEnumerable{TSource}, Func{TSource, Seq{TResult}}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.For{TSource, TResult}(ForSeq{TSource, TResult})"/>, with the selector
/// adapted to return the target's own sequences.
/// </remarks>
public sealed class ForSeq<TSource, TResult>(
    IEnumerable<TSource> source,
    Func<TSource, Seq<TResult>> resultSelector,
    string text)
    : Seq<TResult>
{
    /// <summary>Enumerable sequence for which each element will be mapped to a sequence.</summary>
    public IEnumerable<TSource> Source => source;

    /// <summary>Function to select a sequence for each element of the enumerable.</summary>
    public Func<TSource, Seq<TResult>> ResultSelector => resultSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.For(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.For(source, {text})";
}
