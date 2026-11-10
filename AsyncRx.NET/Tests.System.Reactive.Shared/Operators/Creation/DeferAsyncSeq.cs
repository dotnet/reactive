// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.Defer(observableFactory)</c> with an asynchronous factory.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.Defer{T}(Func{ValueTask{Seq{T}}}, string)"/>; materialized by each
/// target through <see cref="ISeqVisitor.DeferAsync{T}(DeferAsyncSeq{T})"/>. The factory runs
/// for each subscription and its task's result is the sequence subscribed to.
/// </remarks>
public sealed class DeferAsyncSeq<T>(Func<ValueTask<Seq<T>>> observableFactory, string text) : Seq<T>
{
    /// <summary>Asynchronous factory to invoke for each observer that subscribes.</summary>
    public Func<ValueTask<Seq<T>>> ObservableFactory => observableFactory;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.DeferAsync(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.Defer({text})";
}
