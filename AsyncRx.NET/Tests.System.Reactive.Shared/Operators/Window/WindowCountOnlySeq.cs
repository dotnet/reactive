// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Window(count)</c>.</summary>
/// <remarks>
/// Built by <see cref="WindowExtensions.Window{T}(Seq{T}, int)"/>; materialized by each
/// target through <see cref="ISeqVisitor.WindowCountOnly{T}(WindowCountOnlySeq{T})"/>.
/// </remarks>
public sealed class WindowCountOnlySeq<T>(Seq<T> source, int count) : Seq<Seq<T>>
{
    /// <summary>Source sequence to produce windows over.</summary>
    public Seq<T> Source => source;

    /// <summary>Length of each window.</summary>
    public int Count => count;

    /// <inheritdoc/>
    public override Realized<Seq<Seq<T>>> Accept(ISeqVisitor visitor) => visitor.WindowCountOnly(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Window({count})";
}
