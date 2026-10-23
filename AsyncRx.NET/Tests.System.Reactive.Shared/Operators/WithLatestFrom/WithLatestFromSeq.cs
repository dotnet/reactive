// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>first.WithLatestFrom(second, resultSelector)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="Seq.WithLatestFrom{TFirst, TSecond, TResult}(Seq{TFirst}, Seq{TSecond}, Func{TFirst, TSecond, TResult}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.WithLatestFrom{TFirst, TSecond, TResult}(WithLatestFromSeq{TFirst, TSecond, TResult})"/>.
/// </remarks>
public sealed class WithLatestFromSeq<TFirst, TSecond, TResult>(
    Seq<TFirst> first,
    Seq<TSecond> second,
    Func<TFirst, TSecond, TResult> resultSelector,
    string text)
    : Seq<TResult>
{
    /// <summary>First observable source.</summary>
    public Seq<TFirst> First => first;

    /// <summary>Second observable source.</summary>
    public Seq<TSecond> Second => second;

    /// <summary>
    /// Function to invoke for each element from the first source combined with the latest
    /// element from the second source, if any.
    /// </summary>
    public Func<TFirst, TSecond, TResult> ResultSelector => resultSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.WithLatestFrom(this);

    /// <inheritdoc/>
    public override string ToString() => $"{first}.WithLatestFrom({second}, {text})";
}
