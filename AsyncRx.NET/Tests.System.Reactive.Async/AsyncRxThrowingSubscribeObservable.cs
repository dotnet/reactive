// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

/// <summary>A raw asynchronous observable whose <c>SubscribeAsync</c> throws.</summary>
/// <remarks>
/// The AsyncRx.NET form of Rx.NET's <c>ObservableSafetyTest.RogueObservable</c>: it
/// implements the interface directly rather than through <c>AsyncObservableBase</c>, which
/// <c>SubscribeSafeAsync</c> would not guard, and throws synchronously from the call. Realizes
/// <see cref="ThrowOnSubscribeSeq{T}"/> on this target.
/// </remarks>
internal sealed class AsyncRxThrowingSubscribeObservable<T>(Exception error) : IAsyncObservable<T>
{
    public ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer) => throw error;
}
