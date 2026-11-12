// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Publish(selector, initialValue)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Publish{T, TResult}(Seq{T}, Func{Seq{T}, Seq{TResult}}, T, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.PublishSelectorInitial{T, TResult}(PublishSelectorInitialSeq{T, TResult})"/>.
/// </remarks>
public sealed class PublishSelectorInitialSeq<T, TResult>(Seq<T> source, Func<Seq<T>, Seq<TResult>> selector, T initialValue, string text) : Seq<TResult>
{
    /// <summary>
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </summary>
    public Seq<T> Source => source;

    /// <summary>
    /// Selector function which can use the multicasted source sequence as many times as needed,
    /// without causing multiple subscriptions to the source sequence.
    /// </summary>
    public Func<Seq<T>, Seq<TResult>> Selector => selector;

    /// <summary>Initial value received by observers upon subscription.</summary>
    public T InitialValue => initialValue;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) => visitor.PublishSelectorInitial(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Publish({text}, {initialValue})";
}
