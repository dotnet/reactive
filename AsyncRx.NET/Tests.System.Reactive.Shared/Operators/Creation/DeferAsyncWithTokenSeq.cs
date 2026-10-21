// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.DeferAsync(observableFactory)</c>: an asynchronous factory that takes a
/// token.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.DeferAsync{T}(Func{CancellationToken, ValueTask{Seq{T}}}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.DeferAsyncWithToken{T}(DeferAsyncWithTokenSeq{T})"/>. The factory runs
/// for each subscription, its token cancelled if the subscriber unsubscribes while it runs, and
/// its task's result is the sequence subscribed to.
/// </remarks>
public sealed class DeferAsyncWithTokenSeq<T>(
    Func<CancellationToken, ValueTask<Seq<T>>> observableFactory,
    string text) : Seq<T>
{
    /// <summary>Asynchronous factory to invoke for each observer that subscribes.</summary>
    public Func<CancellationToken, ValueTask<Seq<T>>> ObservableFactory => observableFactory;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.DeferAsyncWithToken(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.DeferAsync({text})";
}
