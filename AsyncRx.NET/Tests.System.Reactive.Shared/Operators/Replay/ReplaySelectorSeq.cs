// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Replay(selector)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="ReplayExtensions.Replay{T, TResult}(Seq{T}, Func{Seq{T}, Seq{TResult}}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.ReplaySelector{T, TResult}(ReplaySelectorSeq{T, TResult})"/>.
/// </remarks>
public sealed class ReplaySelectorSeq<T, TResult>(
    Seq<T> source,
    Func<Seq<T>, Seq<TResult>> selector,
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

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ReplaySelector(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Replay({text})";
}
