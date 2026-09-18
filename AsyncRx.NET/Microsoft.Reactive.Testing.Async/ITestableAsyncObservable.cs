// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Microsoft.Reactive.Testing.Async;

/// <summary>
/// The async counterpart of <see cref="ITestableObservable{T}"/>: an observable sequence
/// with recorded four-timestamp subscriptions and a scripted message list.
/// </summary>
public interface ITestableAsyncObservable<T> : IAsyncObservable<T>
{
    /// <summary>Recorded subscriptions, in subscription order.</summary>
    IReadOnlyList<AsyncSubscription> Subscriptions { get; }

    /// <summary>The scripted notifications this observable plays back.</summary>
    IReadOnlyList<Recorded<Notification<T>>> Messages { get; }
}
