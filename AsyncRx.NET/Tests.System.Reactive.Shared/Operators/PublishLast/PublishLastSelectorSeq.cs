// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.PublishLast(selector)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="PublishLastExtensions.PublishLast{T, TResult}(Seq{T}, Func{Seq{T}, Seq{TResult}}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.PublishLastSelector{T, TResult}(PublishLastSelectorSeq{T, TResult})"/>.
/// </remarks>
public sealed class PublishLastSelectorSeq<T, TResult>(
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
    /// source will only receive the last notification of the source.
    /// </summary>
    public Func<Seq<T>, Seq<TResult>> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<Seq<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.PublishLastSelector(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.PublishLast({text})";
}
