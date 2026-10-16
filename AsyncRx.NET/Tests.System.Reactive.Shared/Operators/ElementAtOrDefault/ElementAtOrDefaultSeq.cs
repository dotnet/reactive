// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.ElementAtOrDefault(index)</c>.</summary>
/// <remarks>
/// Built by <see cref="ElementAtOrDefaultExtensions.ElementAtOrDefault{T}(Seq{T}, int)"/>;
/// materialized by each target through <see cref="ISeqVisitor.ElementAtOrDefault{T}(ElementAtOrDefaultSeq{T})"/>.
/// </remarks>
public sealed class ElementAtOrDefaultSeq<T>(Seq<T> source, int index) : Seq<T>
{
    /// <summary>Observable sequence to return the element from.</summary>
    public Seq<T> Source => source;

    /// <summary>The zero-based index of the element to retrieve.</summary>
    public int Index => index;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ElementAtOrDefault(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.ElementAtOrDefault({index})";
}
