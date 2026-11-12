// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.SkipWhile(predicate)</c>.</summary>
/// <remarks>
/// Built by <see cref="SkipWhileExtensions.SkipWhile{T}(Seq{T}, Func{T, bool}, string)"/>;
/// materialized by each target through <see cref="ISeqVisitor.SkipWhile{T}(SkipWhileSeq{T})"/>.
/// </remarks>
public sealed class SkipWhileSeq<T>(Seq<T> source, Func<T, bool> predicate, string text)
    : Seq<T>
{
    /// <summary>An observable sequence to return elements from.</summary>
    public Seq<T> Source => source;

    /// <summary>A function to test each element for a condition.</summary>
    public Func<T, bool> Predicate => predicate;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SkipWhile(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.SkipWhile({text})";
}
