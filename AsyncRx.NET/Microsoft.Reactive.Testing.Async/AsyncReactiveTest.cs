// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Microsoft.Reactive.Testing.Async;

/// <summary>
/// Base class for AsyncRx virtual-time tests. Inherits the sync <see cref="ReactiveTest"/>
/// vocabulary — <c>OnNext(ticks, value)</c>, <c>OnError</c>, <c>OnCompleted</c>,
/// <c>Subscribe(start, end)</c>, and the <c>Created</c>/<c>Subscribed</c>/<c>Disposed</c>
/// conventions — so scenarios read identically on both platforms, and adds the extended
/// factories for the async-only forms: notifications whose delivery start and completion
/// ticks differ (prolonged completion), and four-timestamp subscription records.
/// </summary>
#pragma warning disable CA1052 // Tests inherit from this to bring static members into scope
public class AsyncReactiveTest : ReactiveTest
#pragma warning restore CA1052
{
    /// <summary>
    /// Factory for an extended OnNext expectation whose delivery started and completed at
    /// the given (start, end) virtual times, e.g. <c>OnNext((210, 250), 5)</c>. Use only
    /// when prolonged completion (delivery whose completion depends on something logically
    /// in the future) is the point of the test; the compact <c>OnNext(ticks, value)</c>
    /// form asserts both instants at once. (The times are a tuple rather than two leading
    /// <c>long</c> parameters so these can never bind against the sync witness overloads,
    /// e.g. <c>OnCompleted&lt;int&gt;(ticks, witness)</c>.)
    /// </summary>
    public static AsyncRecorded<Notification<T>> OnNext<T>((long Start, long End) delivery, T value) =>
        new(delivery.Start, delivery.End, Notification.CreateOnNext(value));

    /// <summary>Factory for an extended OnError expectation with distinct delivery start and completion times.</summary>
    public static AsyncRecorded<Notification<T>> OnError<T>((long Start, long End) delivery, Exception exception)
    {
        if (exception == null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        return new(delivery.Start, delivery.End, Notification.CreateOnError<T>(exception));
    }

    /// <summary>Factory for an extended OnCompleted expectation with distinct delivery start and completion times.</summary>
    public static AsyncRecorded<Notification<T>> OnCompleted<T>((long Start, long End) delivery) =>
        new(delivery.Start, delivery.End, Notification.CreateOnCompleted<T>());

    /// <summary>
    /// Factory for a broken-down four-timestamp subscription expectation. Use only when the
    /// ticks genuinely diverge — that divergence is semantically meaningful (an operator
    /// establishing or tearing down subscriptions across ticks); the compact
    /// <c>Subscribe(start, end)</c> form asserts call and completion together.
    /// </summary>
    public static AsyncSubscription Subscribe(long subscribeCalled, long subscribeCompleted, long disposeCalled, long disposeCompleted) =>
        new(subscribeCalled, subscribeCompleted, disposeCalled, disposeCompleted);
}
