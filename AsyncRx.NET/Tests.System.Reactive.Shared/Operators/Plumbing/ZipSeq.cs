// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>first.Zip(second, resultSelector)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="SeqExtensions.Zip{TFirst, TSecond, TResult}(Seq{TFirst}, Seq{TSecond}, Func{TFirst, TSecond, TResult}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.Zip{TFirst, TSecond, TResult}(ZipSeq{TFirst, TSecond, TResult})"/>.
/// </remarks>
public sealed class ZipSeq<TFirst, TSecond, TResult>(
    Seq<TFirst> first,
    Seq<TSecond> second,
    Func<TFirst, TSecond, TResult> resultSelector,
    string text) : Seq<TResult>
{
    /// <summary>First observable source.</summary>
    public Seq<TFirst> First => first;

    /// <summary>Second observable source.</summary>
    public Seq<TSecond> Second => second;

    /// <summary>Function to invoke for each consecutive pair of elements.</summary>
    public Func<TFirst, TSecond, TResult> ResultSelector => resultSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) => visitor.Zip(this);

    /// <inheritdoc/>
    public override string ToString() => $"{first}.Zip({second}, {text})";
}
