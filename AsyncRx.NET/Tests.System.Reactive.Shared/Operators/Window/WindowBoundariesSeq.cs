// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Window(windowBoundaries)</c>.</summary>
/// <remarks>Built by <see cref="WindowExtensions.Window{T, TWindowBoundary}(Seq{T}, Seq{TWindowBoundary})"/>; materialized by each target through <see cref="ISeqVisitor.WindowBoundaries{T, TWindowBoundary}(WindowBoundariesSeq{T, TWindowBoundary})"/>.</remarks>
public sealed class WindowBoundariesSeq<T, TWindowBoundary>(Seq<T> source, Seq<TWindowBoundary> windowBoundaries) : Nested<T>
{
    public Seq<T> Source => source;

    public Seq<TWindowBoundary> WindowBoundaries => windowBoundaries;

    public override object Accept(ISeqVisitor visitor) => visitor.WindowBoundaries(this);

    public override string ToString() => $"{source}.Window({windowBoundaries})";
}
