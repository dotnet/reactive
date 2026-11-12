// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Rx;

/// <summary>A raw observable whose <c>Subscribe</c> throws.</summary>
/// <remarks>
/// Rx.NET's <c>ObservableSafetyTest.RogueObservable</c>, as it stands: it implements the
/// interface directly rather than through <c>ObservableBase</c>, so nothing in the library
/// catches the throw before the guard under test does. Realizes
/// <see cref="ThrowOnSubscribeSeq{T}"/> on this target.
/// </remarks>
internal sealed class RxThrowingSubscribeObservable<T>(Exception error) : IObservable<T>
{
    public IDisposable Subscribe(IObserver<T> observer) => throw error;
}
