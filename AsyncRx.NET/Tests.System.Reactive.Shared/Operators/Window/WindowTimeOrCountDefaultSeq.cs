// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Window(timeSpan, count)</c> on the default scheduler.</summary>
/// <remarks>
/// Built by <see cref="WindowExtensions.Window{T}(Seq{T}, TimeSpan, int)"/>; materialized by each
/// target through
/// <see cref="ISeqVisitor.WindowTimeOrCountDefault{T}(WindowTimeOrCountDefaultSeq{T})"/>.
/// Only a real-time scenario can use this form, since a virtual-time one must name the
/// scheduler.
/// </remarks>
public sealed class WindowTimeOrCountDefaultSeq<T>(Seq<T> source, TimeSpan timeSpan, int count) : Seq<Seq<T>>
{
    /// <summary>Source sequence to produce windows over.</summary>
    public Seq<T> Source => source;

    /// <summary>Maximum time length of a window.</summary>
    public TimeSpan TimeSpan => timeSpan;

    /// <summary>Maximum element count of a window.</summary>
    public int Count => count;

    /// <inheritdoc/>
    protected override Realized<Seq<Seq<T>>> AcceptCore(ISeqVisitor visitor) => visitor.WindowTimeOrCountDefault(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Window({timeSpan.Ticks} ticks, {count})";
}
