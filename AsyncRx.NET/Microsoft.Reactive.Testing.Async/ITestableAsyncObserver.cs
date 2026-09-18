// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Microsoft.Reactive.Testing.Async;

/// <summary>
/// The async counterpart of <see cref="ITestableObserver{T}"/>: records each received
/// notification with its delivery start and completion virtual times.
/// </summary>
public interface ITestableAsyncObserver<T> : IAsyncObserver<T>
{
    /// <summary>Recorded notifications, each with delivery start and completion times.</summary>
    IReadOnlyList<AsyncRecorded<Notification<T>>> Messages { get; }
}
