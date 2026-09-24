// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Microsoft.Reactive.Testing.Async;

/// <summary>
/// Observable asynchronous sequence that records subscription lifetimes and timestamped
/// notification messages sent to observers.
/// </summary>
/// <typeparam name="T">The type of the elements in the sequence.</typeparam>
/// <remarks>
/// This is the async counterpart of <see cref="ITestableObservable{T}"/>. In addition to deriving
/// from <see cref="IAsyncObservable{T}"/> (instead of <see cref="IObservable{T}"/>), it records
/// subscriptions using <see cref="AsyncSubscription"/>, which can report when an asynchronous
/// operation did not complete instantaneously, because it records start and end times separately.
/// </remarks>
public interface ITestableAsyncObservable<T> : IAsyncObservable<T>
{
    /// <summary>
    /// Gets a list of all the subscriptions to the observable sequence, including their lifetimes.
    /// </summary>
    public IReadOnlyList<AsyncSubscription> Subscriptions { get; }

    /// <summary>
    /// Gets the recorded timestamped notification messages that were sent by the observable
    /// sequence to its observers.
    /// </summary>
    public IReadOnlyList<Recorded<Notification<T>>> Messages { get; }
}
