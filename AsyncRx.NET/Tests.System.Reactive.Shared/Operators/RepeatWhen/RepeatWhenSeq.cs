// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.RepeatWhen(handler)</c>.</summary>
/// <remarks>
/// Built by <see cref="RepeatWhenExtensions.RepeatWhen{T, TSignal}"/>; materialized by each target
/// through <see cref="ISeqVisitor.RepeatWhen{T, TSignal}(RepeatWhenSeq{T, TSignal})"/>. The handler
/// receives the sequence of the source's completion signals as a description and returns the
/// sequence whose elements repeat the source; the bridge hands it the target's own signal
/// sequence and materializes what it returns.
/// </remarks>
public sealed class RepeatWhenSeq<T, TSignal>(
    Seq<T> source,
    Func<Seq<object>, Seq<TSignal>> handler,
    string text) : Seq<T>
{
    /// <summary>The sequence to repeat.</summary>
    public Seq<T> Source => source;

    /// <summary>The handler, from the completion signals to the sequence that repeats.</summary>
    public Func<Seq<object>, Seq<TSignal>> Handler => handler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.RepeatWhen(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.RepeatWhen({text})";
}
