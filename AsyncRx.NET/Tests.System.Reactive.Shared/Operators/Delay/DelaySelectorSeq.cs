// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Delay(delayDurationSelector)</c>.</summary>
/// <remarks>
/// Built by <see cref="DelayExtensions.Delay{T, TDelay}(Seq{T}, Func{T, Seq{TDelay}}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.DelaySelector{T, TDelay}(DelaySelectorSeq{T, TDelay})"/>.
/// </remarks>
public sealed class DelaySelectorSeq<T, TDelay>(Seq<T> source, Func<T, Seq<TDelay>> delayDurationSelector, string text) : Seq<T>
{
    /// <summary>Source sequence to delay values for.</summary>
    public Seq<T> Source => source;

    /// <summary>
    /// Selector function to retrieve a sequence indicating the delay for each given element.
    /// </summary>
    public Func<T, Seq<TDelay>> DelayDurationSelector => delayDurationSelector;

    /// <inheritdoc/>
    public override RSeq<T> Accept(ISeqVisitor visitor) => visitor.DelaySelector(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Delay({text})";
}
