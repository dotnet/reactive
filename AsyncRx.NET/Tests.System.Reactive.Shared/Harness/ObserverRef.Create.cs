// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Builds an <see cref="ObserverRef{T}"/> from handlers.</summary>
/// <remarks>
/// Where an Rx.NET test writes <c>Observer.Create&lt;int&gt;(onNext, onError, onCompleted)</c>
/// to hand an observer to an operator (<c>Do(observer)</c>), a shared scenario writes
/// <c>ObserverRef.Create&lt;int&gt;(...)</c> with the same handlers: <see cref="ObserverRef{T}"/>
/// stands for the observer as <see cref="Seq{T}"/> stands for the sequence. The bridge turns the
/// result into the target's own observer when it passes it to the library.
/// </remarks>
public static class ObserverRef
{
    /// <summary>Describes <c>Observer.Create(onNext, onError, onCompleted)</c>.</summary>
    /// <typeparam name="T">The type of the elements received by the observer.</typeparam>
    /// <param name="onNext">Observer's OnNext action implementation.</param>
    /// <param name="onError">Observer's OnError action implementation.</param>
    /// <param name="onCompleted">Observer's OnCompleted action implementation.</param>
    /// <remarks>
    /// The handlers are synchronous, as the Rx.NET test's are; each completes its call at once.
    /// </remarks>
    public static ObserverRef<T> Create<T>(
        Action<T> onNext,
        Action<Exception> onError,
        Action onCompleted) =>
        new(
            x =>
            {
                onNext(x);
                return default;
            },
            ex =>
            {
                onError(ex);
                return default;
            },
            () =>
            {
                onCompleted();
                return default;
            });
}
