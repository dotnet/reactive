// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Microsoft.Reactive.Testing.Async;

/// <summary>
/// Base class for AsyncRx virtual-time tests.
/// </summary>
/// <remarks>
/// This inherits the sync <see cref="ReactiveTest"/> vocabulary—<c>OnNext(ticks, value)</c>,
/// <c>OnError</c>, <c>OnCompleted</c>, <c>Subscribe(start, end)</c>, and the
/// <c>Created</c>/<c>Subscribed</c>/<c>Disposed</c> conventions—so scenarios read identically on
/// both platforms. It adds extended factories for the async-only forms: notifications whose
/// delivery start and completion ticks differ (prolonged completion), and four-timestamp subscription records.
/// </remarks>
public class AsyncReactiveTest : ReactiveTest
{
    /// <summary>
    /// Factory method for an OnNext notification record with a given time range and a given value.
    /// </summary>
    /// <typeparam name="T">The element type for the resulting notification object.</typeparam>
    /// <param name="delivery">
    /// The OnNext notification's recorded virtual start and end time. The end time can be
    /// <see cref="OperationTime.Infinite"/> to indicate that handling has not completed (i.e.,
    /// the task returned by <see cref="IAsyncObserver{T}.OnNextAsync(T)"/> has not completed).
    /// </param>
    /// <param name="value">Recorded value stored in the OnNext notification.</param>
    /// <returns>Recorded OnNext notification.</returns>
    /// <remarks>
    /// <para>
    /// This is for scenarios in which we expected prolonged handling (that is, the
    /// <see cref="IAsyncObserver{T}.OnNextAsync(T)"/> returns a task that completes at a later
    /// virtual time than the time at which the notification was delivered). For example:
    /// </para>
    /// <code><![CDATA[
    /// OnNext((210, 260), 1)
    /// ]]></code>
    /// <para>
    /// Describes an OnNext notification delivered at virtual time 210, where the handler completed
    /// the returned task at virtual time 260.
    /// </para>
    /// <para>
    /// If a test does not expect any prolonged completions, it should typically use the compact
    /// <see cref="ReactiveTest.OnNext{T}(long, T)"/> form instead. The comparisons in
    /// <see cref="AsyncReactiveAssert"/> accept the compact form, and <c>OnNext(210, 5)</c> is
    /// effectively equivalent to <c>OnNext((210, 210), 5)</c>.
    /// </para>
    /// </remarks>
    public static AsyncRecorded<Notification<T>> OnNext<T>((long Start, long End) delivery, T value) =>
        new(delivery.Start, delivery.End, Notification.CreateOnNext(value));

    /// <summary>
    /// Factory method for an OnError notification record with a given time range and a given error.
    /// </summary>
    /// <typeparam name="T">
    /// The element type for the resulting notification object. For <c>OnError</c> notifications,
    /// there is no value of this type, but any <see cref="Notification{T}"/> has a type argument
    /// matching the source stream element type even for notifications with no associated element.
    /// </typeparam>
    /// <param name="delivery">
    /// The OnError notification's recorded virtual start and end time. The end time can be
    /// <see cref="OperationTime.Infinite"/> to represent an operation that has not completed.
    /// </param>
    /// <param name="exception">Recorded exception stored in the OnError notification.</param>
    /// <returns>Recorded OnError notification.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="exception"/> is null.</exception>
    /// <remarks>
    /// <para>
    /// This is for scenarios in which we expected prolonged handling. See
    /// <see cref="OnNext{T}(ValueTuple{long, long}, T)"/> for details.
    /// </para>
    /// </remarks>
    public static AsyncRecorded<Notification<T>> OnError<T>((long Start, long End) delivery, Exception exception)
    {
        if (exception == null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        return new(delivery.Start, delivery.End, Notification.CreateOnError<T>(exception));
    }

    /// <summary>
    /// Factory method for an OnCompleted notification record with a given time range.
    /// </summary>
    /// <typeparam name="T">
    /// The element type for the resulting notification object. For <c>OnCompleted</c>
    /// notifications, there is no value of this type, but any <see cref="Notification{T}"/> has a
    /// type argument matching the source stream element type even for notifications with no
    /// associated element.
    /// </typeparam>
    /// <param name="delivery">
    /// The OnCompleted notification's recorded virtual start and end time. The end time can be
    /// <see cref="OperationTime.Infinite"/> to represent an operation that has not completed.
    /// </param>
    /// <returns>Recorded OnCompleted notification.</returns>
    /// <remarks>
    /// <para>
    /// This is for scenarios in which we expected prolonged handling. See
    /// <see cref="OnNext{T}(ValueTuple{long, long}, T)"/> for details.
    /// </para>
    /// </remarks>
    public static AsyncRecorded<Notification<T>> OnCompleted<T>((long Start, long End) delivery) =>
        new(delivery.Start, delivery.End, Notification.CreateOnCompleted<T>());

    /// <summary>
    /// Factory method for a subscription record based on given subscription and disposal time
    /// ranges.
    /// </summary>
    /// <param name="subscribeCalled">
    /// Virtual time of the call to <see cref="IAsyncObservable{T}.SubscribeAsync(IAsyncObserver{T})"/>.
    /// </param>
    /// <param name="subscribeCompleted">
    /// Virtual time of the completion of the task returned by
    /// <see cref="IAsyncObservable{T}.SubscribeAsync(IAsyncObserver{T})"/>.
    /// </param>
    /// <param name="disposeCalled">
    /// Virtual time of the call to <see cref="IAsyncDisposable.DisposeAsync"/> on the subscription
    /// returned by <see cref="IAsyncObservable{T}.SubscribeAsync(IAsyncObserver{T})"/>.
    /// </param>
    /// <param name="disposeCompleted">
    /// Virtual time of the completion of the task returned by the call to
    /// <see cref="IAsyncDisposable.DisposeAsync"/>.
    /// </param>
    /// <returns><see cref="AsyncSubscription"/> object.</returns>
    /// <remarks>
    /// This is for scenarios in which we expect either subscription or disposal of a subscription
    /// to be prolonged (that is, one or both of the tasks returned by
    /// <see cref="IAsyncObservable{T}.SubscribeAsync(IAsyncObserver{T})"/> or
    /// <see cref="IAsyncDisposable.DisposeAsync"/> do not complete logically instantaneously).
    /// Tests that do not expect prolonged subscription or disposal should typically use the compact
    /// form, <see cref="ReactiveTest.Subscribe(long, long)"/>.
    /// </remarks>
    public static AsyncSubscription Subscribe(long subscribeCalled, long subscribeCompleted, long disposeCalled, long disposeCompleted) =>
        new(subscribeCalled, subscribeCompleted, disposeCalled, disposeCompleted);
}
