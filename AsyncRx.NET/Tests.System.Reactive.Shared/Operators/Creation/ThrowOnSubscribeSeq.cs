// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>A sequence whose subscribe call itself throws, as a leaf.</summary>
/// <remarks>
/// Rx.NET's <c>ObservableSafetyTest</c> writes a <c>RogueObservable</c> for this: a raw
/// <c>IObservable&lt;T&gt;</c>, not an <c>ObservableBase</c>, whose <c>Subscribe</c> throws the
/// given exception, so that what guards the subscription (<c>SubscribeSafe</c>, an operator
/// subscribing to its source) is what delivers the error. Each target realizes it as its own raw
/// implementation of the observable interface.
/// </remarks>
public sealed class ThrowOnSubscribeSeq<T>(Exception error) : Seq<T>
{
    /// <summary>The exception thrown by every subscribe call.</summary>
    public Exception Error => error;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ThrowOnSubscribe(this);

    /// <inheritdoc/>
    public override string ToString() => "Seq.ThrowOnSubscribe(error)";
}
