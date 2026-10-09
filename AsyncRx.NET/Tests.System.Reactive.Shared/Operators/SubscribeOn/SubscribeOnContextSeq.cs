// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.SubscribeOn(context)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="SubscribeOnExtensions.SubscribeOn{T}(Seq{T}, SynchronizationContext)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.SubscribeOnContext{T}(SubscribeOnContextSeq{T})"/>. A synchronization
/// context is the same type on both targets, so it passes through the bridge unchanged.
/// </remarks>
public sealed class SubscribeOnContextSeq<T>(Seq<T> source, SynchronizationContext context)
    : Seq<T>
{
    /// <summary>Source sequence.</summary>
    public Seq<T> Source => source;

    /// <summary>
    /// Synchronization context to perform subscription and unsubscription actions on.
    /// </summary>
    public SynchronizationContext Context => context;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SubscribeOnContext(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.SubscribeOn({context.GetType().Name})";
}
