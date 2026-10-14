// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Throttle(throttleDurationSelector)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="ThrottleExtensions.Throttle{T, TThrottle}(Seq{T}, Func{T, Seq{TThrottle}}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.ThrottleSelector{T, TThrottle}(ThrottleSelectorSeq{T, TThrottle})"/>.
/// </remarks>
public sealed class ThrottleSelectorSeq<T, TThrottle>(
    Seq<T> source,
    Func<T, Seq<TThrottle>> throttleDurationSelector,
    string text)
    : Seq<T>
{
    /// <summary>Source sequence to throttle.</summary>
    public Seq<T> Source => source;

    /// <summary>
    /// Selector function to retrieve a sequence indicating the throttle duration for each given
    /// element.
    /// </summary>
    public Func<T, Seq<TThrottle>> ThrottleDurationSelector => throttleDurationSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ThrottleSelector(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Throttle({text})";
}
