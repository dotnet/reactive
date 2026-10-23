// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Defer(observableFactory)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Defer{T}(Func{Seq{T}}, string)"/>; materialized by each target
/// through <see cref="ISeqVisitor.Defer{T}(DeferSeq{T})"/>. The factory returns a description,
/// which the target materializes each time it runs the factory, so a scenario that counts
/// factory runs counts subscriptions as the Rx.NET test does.
/// </remarks>
public sealed class DeferSeq<T>(Func<Seq<T>> observableFactory, string text) : Seq<T>
{
    /// <summary>Observable factory function to invoke for each observer that subscribes.</summary>
    public Func<Seq<T>> ObservableFactory => observableFactory;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.Defer(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.Defer({text})";
}
