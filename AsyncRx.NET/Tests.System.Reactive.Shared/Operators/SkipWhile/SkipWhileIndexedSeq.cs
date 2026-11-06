// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.SkipWhile((x, i) =&gt; ...)</c>, the indexed form.</summary>
/// <remarks>
/// Built by <see cref="SkipWhileExtensions.SkipWhile{T}(Seq{T}, Func{T, int, bool}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.SkipWhileIndexed{T}(SkipWhileIndexedSeq{T})"/>.
/// </remarks>
public sealed class SkipWhileIndexedSeq<T>(Seq<T> source, Func<T, int, bool> predicate, string text)
    : Seq<T>
{
    /// <summary>An observable sequence to return elements from.</summary>
    public Seq<T> Source => source;

    /// <summary>A function to test each element for a condition.</summary>
    /// <remarks>
    /// The second parameter of the function represents the index of the source element.
    /// </remarks>
    public Func<T, int, bool> Predicate => predicate;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SkipWhileIndexed(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.SkipWhile({text})";
}
