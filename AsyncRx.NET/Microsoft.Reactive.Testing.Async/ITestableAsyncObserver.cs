// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Microsoft.Reactive.Testing.Async;

/// <summary>
/// Asynchronous observer that records and timestamps received notification messages.
/// </summary>
/// <typeparam name="T">The type of the elements in the sequence.</typeparam>
/// <summary>
/// <para>
/// The async counterpart of <see cref="ITestableObserver{T}"/>. In addition to deriving from
/// <see cref="IAsyncObserver{T}"/> (instead of <see cref="IObserver{T}"/>), this records the
/// start and end times of each notification delivery, because asynchronous operations don't
/// necessarily complete at the same virtual time as they start.
/// </para>
/// </summary>
public interface ITestableAsyncObserver<T> : IAsyncObserver<T>
{
    /// <summary>
    /// Gets recorded timestamped notification messages received by the observer.
    /// </summary>
    /// <remarks>
    /// In test observers can be set up to invoke a test-supplied handler with the
    /// <see cref="TestAsyncScheduler.CreateObserver{T}(Func{Notification{T}, ValueTask})"/>
    /// method. This enables tests to prolong consumption of notifications, which is why this
    /// property uses <see cref="AsyncRecorded{T}"/>. This enables recording of different start and
    /// end times.
    /// </remarks>
    public IReadOnlyList<AsyncRecorded<Notification<T>>> Messages { get; }
}
