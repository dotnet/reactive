// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Window(windowBoundaries)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="WindowExtensions.Window{T, TWindowBoundary}(Seq{T}, Seq{TWindowBoundary})"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.WindowBoundaries{T, TWindowBoundary}(WindowBoundariesSeq{T, TWindowBoundary})"/>.
/// </remarks>
public sealed class WindowBoundariesSeq<T, TWindowBoundary>(Seq<T> source, Seq<TWindowBoundary> windowBoundaries) : Seq<Seq<T>>
{
    /// <summary>Source sequence to produce windows over.</summary>
    public Seq<T> Source => source;

    /// <summary>Sequence of window boundary markers.</summary>
    /// <remarks>
    /// The current window is closed and a new window is opened upon receiving a boundary marker.
    /// </remarks>
    public Seq<TWindowBoundary> WindowBoundaries => windowBoundaries;

    /// <inheritdoc/>
    public override Realized<Seq<Seq<T>>> Accept(ISeqVisitor visitor) =>
        visitor.WindowBoundaries(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Window({windowBoundaries})";
}
