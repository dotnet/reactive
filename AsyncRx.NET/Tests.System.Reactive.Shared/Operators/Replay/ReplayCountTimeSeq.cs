// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Replay(bufferSize, window)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="ReplayExtensions.Replay{T}(Seq{T}, int, TimeSpan)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.ReplayCountTime{T}(ReplayCountTimeSeq{T})"/>.
/// </remarks>
public sealed class ReplayCountTimeSeq<T>(
    Seq<T> source,
    int bufferSize,
    TimeSpan window) : ConnectableSeq<T>
{
    /// <summary>
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </summary>
    public Seq<T> Source => source;

    /// <summary>Maximum element count of the replay buffer.</summary>
    public int BufferSize => bufferSize;

    /// <summary>Maximum time length of the replay buffer.</summary>
    public TimeSpan Window => window;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ReplayCountTime(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Replay({bufferSize}, {window})";
}
