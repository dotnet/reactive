// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Window(count, skip)</c>.</summary>
/// <remarks>Built by <see cref="WindowExtensions.Window{T}(Seq{T}, int, int)"/>; materialized by each target through <see cref="ISeqVisitor.WindowCount{T}(WindowCountSeq{T})"/>.</remarks>
public sealed class WindowCountSeq<T>(Seq<T> source, int count, int skip) : Nested<T>
{
    public Seq<T> Source => source;

    public int Count => count;

    public int Skip => skip;

    public override object Accept(ISeqVisitor visitor) => visitor.WindowCount(this);

    public override string ToString() => $"{source}.Window({count}, {skip})";
}
