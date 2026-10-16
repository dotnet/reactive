// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Window(count, skip)</c>.</summary>
/// <remarks>
/// Built by <see cref="WindowExtensions.Window{T}(Seq{T}, int, int)"/>;
/// materialized by each target through <see cref="ISeqVisitor.WindowCount{T}(WindowCountSeq{T})"/>.
/// </remarks>
public sealed class WindowCountSeq<T>(Seq<T> source, int count, int skip) : Seq<Seq<T>>
{
    /// <summary>Source sequence to produce windows over.</summary>
    public Seq<T> Source => source;

    /// <summary>Length of each window.</summary>
    public int Count => count;

    /// <summary>Number of elements to skip between creation of consecutive windows.</summary>
    public int Skip => skip;

    /// <inheritdoc/>
    protected override Realized<Seq<Seq<T>>> AcceptCore(ISeqVisitor visitor) => visitor.WindowCount(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Window({count}, {skip})";
}
