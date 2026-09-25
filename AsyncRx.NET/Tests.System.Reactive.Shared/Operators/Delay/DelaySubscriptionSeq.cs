// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Delay(subscriptionDelay, delayDurationSelector)</c>.</summary>
/// <remarks>Built by <see cref="DelayExtensions.Delay{T, TDelay}(Seq{T}, Seq{TDelay}, Func{T, Seq{TDelay}}, string)"/>; materialized by each target through <see cref="ISeqVisitor.DelaySubscription{T, TDelay}(DelaySubscriptionSeq{T, TDelay})"/>.</remarks>
public sealed class DelaySubscriptionSeq<T, TDelay>(Seq<T> source, Seq<TDelay> subscriptionDelay, Func<T, Seq<TDelay>> delayDurationSelector, string text) : Seq<T>
{
    public Seq<T> Source => source;

    public Seq<TDelay> SubscriptionDelay => subscriptionDelay;

    public Func<T, Seq<TDelay>> DelayDurationSelector => delayDurationSelector;

    public override object Accept(ISeqVisitor visitor) => visitor.DelaySubscription(this);

    public override string ToString() => $"{source}.Delay({subscriptionDelay}, {text})";
}
