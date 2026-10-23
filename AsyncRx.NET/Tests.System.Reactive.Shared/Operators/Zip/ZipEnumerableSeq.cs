// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>first.Zip(second, resultSelector)</c> where the second source is an
/// enumerable.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="Seq.Zip{T1, T2, TResult}(Seq{T1}, IEnumerable{T2}, Func{T1, T2, TResult}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.ZipEnumerable{T1, T2, TResult}(ZipEnumerableSeq{T1, T2, TResult})"/>.
/// An enumerable is the same type on both targets, so it passes through the bridge unchanged.
/// </remarks>
public sealed class ZipEnumerableSeq<T1, T2, TResult>(
    Seq<T1> first,
    IEnumerable<T2> second,
    Func<T1, T2, TResult> resultSelector,
    string text) : Seq<TResult>
{
    /// <summary>First observable source.</summary>
    public Seq<T1> First => first;

    /// <summary>Second enumerable source.</summary>
    public IEnumerable<T2> Second => second;

    /// <summary>
    /// Function to invoke for each consecutive pair of elements from the first and second source.
    /// </summary>
    public Func<T1, T2, TResult> ResultSelector => resultSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ZipEnumerable(this);

    /// <inheritdoc/>
    public override string ToString() => $"{first}.Zip({second.GetType().Name}, {text})";
}
