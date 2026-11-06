// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.RetryWhen(handler)</c>.</summary>
/// <remarks>
/// Built by <see cref="RetryWhenExtensions.RetryWhen{T, TSignal}"/>; materialized by each target
/// through <see cref="ISeqVisitor.RetryWhen{T, TSignal}(RetryWhenSeq{T, TSignal})"/>. The handler
/// receives the sequence of the source's error signals as a description and returns the
/// sequence whose elements retrie the source; the bridge hands it the target's own signal
/// sequence and materializes what it returns.
/// </remarks>
public sealed class RetryWhenSeq<T, TSignal>(
    Seq<T> source,
    Func<Seq<Exception>, Seq<TSignal>> handler,
    string text) : Seq<T>
{
    /// <summary>The sequence to retrie.</summary>
    public Seq<T> Source => source;

    /// <summary>The handler, from the error signals to the sequence that retries.</summary>
    public Func<Seq<Exception>, Seq<TSignal>> Handler => handler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.RetryWhen(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.RetryWhen({text})";
}
