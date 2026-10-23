// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The raw surface's subscribe, as extension methods so it reads as in the sync suite.
/// </summary>
public static class SubscribeExtensions
{
    /// <summary>
    /// Materializes <paramref name="source"/> on <paramref name="observer"/>'s target and
    /// subscribes the observer to it.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="source">The description to subscribe to.</param>
    /// <param name="observer">
    /// A recording observer from <see cref="TestSchedulerRef.CreateObserver{T}"/>.
    /// </param>
    /// <returns>The subscription; on the sync target the task is already complete.</returns>
    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(this Seq<T> source, TestableObserver<T> observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        return observer.Target.SubscribeAsync(source, observer);
    }

    /// <summary>
    /// Materializes <paramref name="source"/> on <paramref name="scheduler"/>'s target and
    /// subscribes a handler for each element.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="source">The description to subscribe to.</param>
    /// <param name="scheduler">The test's scheduler, which identifies the target.</param>
    /// <param name="onNext">
    /// Receives each element; async-shaped so the same scenario text runs on both targets.
    /// </param>
    /// <returns>The subscription; on the sync target the task is already complete.</returns>
    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(this Seq<T> source, TestSchedulerRef scheduler, Func<T, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(scheduler);

        return scheduler.Target.SubscribeAsync(scheduler, source, onNext);
    }

    /// <summary>
    /// As <see cref="SubscribeAsync{T}(Seq{T}, TestSchedulerRef, Func{T, ValueTask})"/>, with a
    /// synchronous handler.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="source">The description to subscribe to.</param>
    /// <param name="scheduler">The test's scheduler, which identifies the target.</param>
    /// <param name="onNext">Receives each element.</param>
    /// <returns>The subscription.</returns>
    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(this Seq<T> source, TestSchedulerRef scheduler, Action<T> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        return source.SubscribeAsync(scheduler, x =>
        {
            onNext(x);
            return default;
        });
    }

    /// <summary>
    /// Materializes a nested <paramref name="source"/> and subscribes a handler that receives each
    /// inner sequence (a window or group) as a <see cref="NativeSeq{T}"/> leaf, which the handler
    /// may subscribe to or build a further description over.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the inner sequences.</typeparam>
    /// <param name="source">The nested description to subscribe to.</param>
    /// <param name="scheduler">The test's scheduler, which identifies the target.</param>
    /// <param name="onNext">
    /// Receives each inner sequence; async-shaped so the same scenario text runs on both targets.
    /// </param>
    /// <returns>The subscription; on the sync target the task is already complete.</returns>
    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(this Seq<Seq<T>> source, TestSchedulerRef scheduler, Func<Seq<T>, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(scheduler);

        return scheduler.Target.SubscribeAsync(scheduler, source, onNext);
    }

    /// <summary>
    /// As <see cref="SubscribeAsync{T}(Seq{Seq{T}}, TestSchedulerRef, Func{Seq{T}, ValueTask})"/>,
    /// with a synchronous handler.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the inner sequences.</typeparam>
    /// <param name="source">The nested description to subscribe to.</param>
    /// <param name="scheduler">The test's scheduler, which identifies the target.</param>
    /// <param name="onNext">Receives each inner sequence.</param>
    /// <returns>The subscription.</returns>
    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(this Seq<Seq<T>> source, TestSchedulerRef scheduler, Action<Seq<T>> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        return source.SubscribeAsync(scheduler, w =>
        {
            onNext(w);
            return default;
        });
    }

    /// <summary>
    /// Subscribes <paramref name="onNext"/> and <paramref name="onError"/> to
    /// <paramref name="source"/>, the target's own <c>Subscribe(onNext, onError)</c>.
    /// </summary>
    /// <param name="source">The sequence to subscribe to.</param>
    /// <param name="scheduler">The test's scheduler, which carries the target.</param>
    /// <param name="onNext">Handler for each element, awaited before the next delivery.</param>
    /// <param name="onError">Handler for an error.</param>
    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(
        this Seq<T> source,
        TestSchedulerRef scheduler,
        Func<T, ValueTask> onNext,
        Action<Exception> onError)
    {
        ArgumentNullException.ThrowIfNull(scheduler);
        ArgumentNullException.ThrowIfNull(onError);

        return scheduler.Target.SubscribeAsync(
            scheduler,
            source,
            onNext,
            ex =>
            {
                onError(ex);
                return default;
            },
            null);
    }

    /// <summary>
    /// Subscribes three handlers to <paramref name="source"/>, the target's own
    /// <c>Subscribe(onNext, onError, onCompleted)</c>.
    /// </summary>
    /// <param name="source">The sequence to subscribe to.</param>
    /// <param name="scheduler">The test's scheduler, which carries the target.</param>
    /// <param name="onNext">Handler for each element, awaited before the next delivery.</param>
    /// <param name="onError">Handler for an error.</param>
    /// <param name="onCompleted">Handler for completion.</param>
    /// <remarks>
    /// The error and completion handlers are typically a testable observer's
    /// <see cref="TestableObserver{T}.OnErrorAsync"/> and
    /// <see cref="TestableObserver{T}.OnCompletedAsync"/>.
    /// </remarks>
    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(
        this Seq<T> source,
        TestSchedulerRef scheduler,
        Func<T, ValueTask> onNext,
        Func<Exception, ValueTask> onError,
        Func<ValueTask> onCompleted)
    {
        ArgumentNullException.ThrowIfNull(scheduler);

        return scheduler.Target.SubscribeAsync(scheduler, source, onNext, onError, onCompleted);
    }

    /// <summary>
    /// Subscribes an observer written over descriptions to <paramref name="source"/>.
    /// </summary>
    /// <param name="source">The sequence to subscribe to.</param>
    /// <param name="scheduler">The test's scheduler, which carries the target.</param>
    /// <param name="observer">The observer, such as the one a <c>Create</c> body is handed.</param>
    /// <remarks>
    /// Rx.NET's <c>Subscribe(IObserver)</c>: the target unwraps the observer into its own, as
    /// it does for an operator that takes one, so a scenario can forward another sequence into
    /// the observer it was given.
    /// </remarks>
    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(
        this Seq<T> source,
        TestSchedulerRef scheduler,
        ObserverRef<T> observer)
    {
        ArgumentNullException.ThrowIfNull(scheduler);

        return scheduler.Target.SubscribeAsync(scheduler, source, observer);
    }

    /// <summary>Subscribes synchronous element and error handlers.</summary>
    /// <param name="source">The sequence to subscribe to.</param>
    /// <param name="scheduler">The test's scheduler, which carries the target.</param>
    /// <param name="onNext">Handler for each element.</param>
    /// <param name="onError">Handler for an error.</param>
    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(
        this Seq<T> source,
        TestSchedulerRef scheduler,
        Action<T> onNext,
        Action<Exception> onError)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        return source.SubscribeAsync(
            scheduler,
            x =>
            {
                onNext(x);
                return default;
            },
            onError);
    }

    /// <summary>Subscribes three synchronous handlers to <paramref name="source"/>.</summary>
    /// <param name="source">The sequence to subscribe to.</param>
    /// <param name="scheduler">The test's scheduler, which carries the target.</param>
    /// <param name="onNext">Handler for each element.</param>
    /// <param name="onError">Handler for an error.</param>
    /// <param name="onCompleted">Handler for completion.</param>
    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(
        this Seq<T> source,
        TestSchedulerRef scheduler,
        Action<T> onNext,
        Action<Exception> onError,
        Action onCompleted)
    {
        ArgumentNullException.ThrowIfNull(scheduler);
        ArgumentNullException.ThrowIfNull(onNext);
        ArgumentNullException.ThrowIfNull(onError);
        ArgumentNullException.ThrowIfNull(onCompleted);

        return scheduler.Target.SubscribeAsync(
            scheduler,
            source,
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

    /// <summary>
    /// Subscribes an element and a completion handler, the target's own
    /// <c>Subscribe(onNext, onCompleted)</c>.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="source">The sequence to subscribe to.</param>
    /// <param name="scheduler">The test's scheduler, which carries the target.</param>
    /// <param name="onNext">Handler for each element, awaited before the next delivery.</param>
    /// <param name="onCompleted">Handler for completion.</param>
    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(
        this Seq<T> source,
        TestSchedulerRef scheduler,
        Func<T, ValueTask> onNext,
        Func<ValueTask> onCompleted)
    {
        ArgumentNullException.ThrowIfNull(scheduler);

        return scheduler.Target.SubscribeAsync(scheduler, source, onNext, onCompleted);
    }

    /// <summary>
    /// As <see cref="SubscribeAsync{T}(Seq{T}, TestSchedulerRef, Func{T, ValueTask}, Func{ValueTask})"/>,
    /// with synchronous handlers.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="source">The sequence to subscribe to.</param>
    /// <param name="scheduler">The test's scheduler, which carries the target.</param>
    /// <param name="onNext">Handler for each element.</param>
    /// <param name="onCompleted">Handler for completion.</param>
    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(
        this Seq<T> source,
        TestSchedulerRef scheduler,
        Action<T> onNext,
        Action onCompleted)
    {
        ArgumentNullException.ThrowIfNull(onNext);
        ArgumentNullException.ThrowIfNull(onCompleted);

        return source.SubscribeAsync(
            scheduler,
            x =>
            {
                onNext(x);
                return default;
            },
            () =>
            {
                onCompleted();
                return default;
            });
    }

    /// <summary>
    /// Subscribes a testable observer to an enumerable, enumerated on
    /// <paramref name="scheduler"/>: where the Rx.NET test writes
    /// <c>xs.Subscribe(results, scheduler)</c> over an <c>IEnumerable</c>.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="source">The enumerable to subscribe to.</param>
    /// <param name="observer">The testable observer, which carries the target.</param>
    /// <param name="scheduler">The scheduler to enumerate on.</param>
    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(
        this IEnumerable<T> source,
        TestableObserver<T> observer,
        SchedulerRef scheduler)
    {
        ArgumentNullException.ThrowIfNull(observer);

        return observer.Target.SubscribeAsync(source, observer, scheduler);
    }

    /// <summary>
    /// Subscribes an observer to an enumerable, enumerated on <paramref name="enumerateOn"/> or,
    /// when that is null, on the library's default: Rx.NET's
    /// <c>Subscribe(IEnumerable, IObserver[, IScheduler])</c>.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="source">The enumerable to subscribe to.</param>
    /// <param name="scheduler">The test's scheduler, which carries the target.</param>
    /// <param name="observer">The observer, whose handlers may dispose the subscription.</param>
    /// <param name="enumerateOn">The scheduler to enumerate on, or null for the default.</param>
    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(
        this IEnumerable<T> source,
        TestSchedulerRef scheduler,
        ObserverRef<T> observer,
        SchedulerRef? enumerateOn)
    {
        ArgumentNullException.ThrowIfNull(scheduler);

        return scheduler.Target.SubscribeAsync(scheduler, source, observer, enumerateOn);
    }
}
