// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Timeout(firstTimeout, timeoutDurationSelector)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="TimeoutExtensions.Timeout{T, TTimeout}(Seq{T}, Seq{TTimeout}, Func{T, Seq{TTimeout}}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.TimeoutFirstDuration{T, TTimeout}(TimeoutFirstDurationSeq{T, TTimeout})"/>.
/// </remarks>
public sealed class TimeoutFirstDurationSeq<T, TTimeout>(
    Seq<T> source,
    Seq<TTimeout> firstTimeout,
    Func<T, Seq<TTimeout>> timeoutDurationSelector,
    string text)
    : Seq<T>
{
    /// <summary>Source sequence to perform a timeout for.</summary>
    public Seq<T> Source => source;

    /// <summary>Observable sequence that represents the timeout for the first element.</summary>
    public Seq<TTimeout> FirstTimeout => firstTimeout;

    /// <summary>
    /// Selector to retrieve an observable sequence that represents the timeout between the
    /// current element and the next element.
    /// </summary>
    public Func<T, Seq<TTimeout>> TimeoutDurationSelector => timeoutDurationSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.TimeoutFirstDuration(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"{source}.Timeout({firstTimeout}, {text})";
}
