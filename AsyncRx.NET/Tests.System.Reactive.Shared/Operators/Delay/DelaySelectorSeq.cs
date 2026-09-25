// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Delay(delayDurationSelector)</c>.</summary>
/// <remarks>Built by <see cref="DelayExtensions.Delay{T, TDelay}(Seq{T}, Func{T, Seq{TDelay}}, string)"/>; materialized by each target through <see cref="ISeqVisitor.DelaySelector{T, TDelay}(DelaySelectorSeq{T, TDelay})"/>.</remarks>
public sealed class DelaySelectorSeq<T, TDelay>(Seq<T> source, Func<T, Seq<TDelay>> delayDurationSelector, string text) : Seq<T>
{
    public Seq<T> Source => source;

    public Func<T, Seq<TDelay>> DelayDurationSelector => delayDurationSelector;

    public override object Accept(ISeqVisitor visitor) => visitor.DelaySelector(this);

    public override string ToString() => $"{source}.Delay({text})";
}
