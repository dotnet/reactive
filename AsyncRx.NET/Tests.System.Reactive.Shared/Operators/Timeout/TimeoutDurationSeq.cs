// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Timeout(timeoutDurationSelector)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="TimeoutExtensions.Timeout{T, TTimeout}(Seq{T}, Func{T, Seq{TTimeout}}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.TimeoutDuration{T, TTimeout}(TimeoutDurationSeq{T, TTimeout})"/>.
/// </remarks>
public sealed class TimeoutDurationSeq<T, TTimeout>(
    Seq<T> source,
    Func<T, Seq<TTimeout>> timeoutDurationSelector,
    string text)
    : Seq<T>
{
    /// <summary>Source sequence to perform a timeout for.</summary>
    public Seq<T> Source => source;

    /// <summary>
    /// Selector to retrieve an observable sequence that represents the timeout between the
    /// current element and the next element.
    /// </summary>
    public Func<T, Seq<TTimeout>> TimeoutDurationSelector => timeoutDurationSelector;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.TimeoutDuration(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"{source}.Timeout({text})";
}
