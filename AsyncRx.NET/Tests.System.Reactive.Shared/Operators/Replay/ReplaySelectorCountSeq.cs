// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Replay(selector, bufferSize)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="ReplayExtensions.Replay{T, TResult}(Seq{T}, Func{Seq{T}, Seq{TResult}}, int, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.ReplaySelectorCount{T, TResult}(ReplaySelectorCountSeq{T, TResult})"/>.
/// </remarks>
public sealed class ReplaySelectorCountSeq<T, TResult>(
    Seq<T> source,
    Func<Seq<T>, Seq<TResult>> selector,
    int bufferSize,
    string text) : Seq<TResult>
{
    /// <summary>
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </summary>
    public Seq<T> Source => source;

    /// <summary>
    /// Selector function which can use the multicasted source sequence as many times as needed,
    /// without causing multiple subscriptions to the source sequence. Subscribers to the given
    /// source will receive all the notifications of the source.
    /// </summary>
    public Func<Seq<T>, Seq<TResult>> Selector => selector;

    /// <summary>Maximum element count of the replay buffer.</summary>
    public int BufferSize => bufferSize;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ReplaySelectorCount(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Replay({text}, {bufferSize})";
}
