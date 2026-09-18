// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Microsoft.Reactive.Testing.Async;

public sealed partial class TestAsyncScheduler
{
    /// <summary>
    /// Runs a full virtual-time test with the same shape as the sync
    /// <see cref="TestScheduler"/>: invoke the factory at <paramref name="created"/>,
    /// subscribe the recording observer at <paramref name="subscribed"/>, dispose the
    /// subscription at <paramref name="disposed"/>, and pump everything to completion.
    /// </summary>
    /// <remarks>
    /// Per the agreed fail-informatively rule: if the dispose tick arrives and the
    /// <c>SubscribeAsync</c> call has not completed, the test fails with a diagnosis — never
    /// a hang, never a silent late dispose.
    /// </remarks>
    public ITestableAsyncObserver<T> Start<T>(Func<IAsyncObservable<T>> create, long created, long subscribed, long disposed)
    {
        if (create == null)
        {
            throw new ArgumentNullException(nameof(create));
        }

        return Start(create, CreateObserver<T>(), created, subscribed, disposed);
    }

    /// <summary>
    /// As <see cref="Start{T}(Func{IAsyncObservable{T}}, long, long, long)"/>, subscribing the
    /// supplied observer instead of a fresh recording one — the way to run a full scenario with
    /// a consumer that prolongs completion (see the <c>CreateObserver</c> overload that takes a per-notification callback).
    /// </summary>
    public ITestableAsyncObserver<T> Start<T>(Func<IAsyncObservable<T>> create, ITestableAsyncObserver<T> observer, long created, long subscribed, long disposed)
    {
        if (create == null)
        {
            throw new ArgumentNullException(nameof(create));
        }
        if (observer == null)
        {
            throw new ArgumentNullException(nameof(observer));
        }

        if (subscribed < created)
        {
            throw new ArgumentOutOfRangeException(nameof(subscribed));
        }

        if (disposed < created || disposed < subscribed)
        {
            throw new ArgumentOutOfRangeException(nameof(disposed));
        }

        var source = default(IAsyncObservable<T>);
        var subscription = default(IAsyncDisposable);

        ScheduleAbsolute(
            created,
            _ =>
            {
                source = create();
                return default;
            },
            $"harness: create the observable under test (tick {created})");

        ScheduleAbsolute(
            subscribed,
            async _ =>
            {
                subscription = await source!.SubscribeAsync(observer);
            },
            $"harness: subscribe to the observable under test (tick {subscribed})");

        ScheduleAbsolute(
            disposed,
            async _ =>
            {
                if (subscription is null)
                {
                    throw new TestAsyncSchedulerException(
                        $"Disposal is scheduled at tick {disposed} but the subscription is not complete: " +
                        $"SubscribeAsync was called at tick {subscribed} and its task has still not completed.");
                }

                await subscription.DisposeAsync();
            },
            $"harness: dispose the subscription (tick {disposed})");

        Start();

        return observer;
    }

    /// <summary>
    /// Runs a full virtual-time test using the default <see cref="ReactiveTest.Created"/>
    /// and <see cref="ReactiveTest.Subscribed"/> times and the given disposal time.
    /// </summary>
    public ITestableAsyncObserver<T> Start<T>(Func<IAsyncObservable<T>> create, long disposed)
    {
        if (create == null)
        {
            throw new ArgumentNullException(nameof(create));
        }

        return Start(create, AsyncReactiveTest.Created, AsyncReactiveTest.Subscribed, disposed);
    }

    /// <summary>
    /// Runs a full virtual-time test using the default <see cref="ReactiveTest.Created"/>,
    /// <see cref="ReactiveTest.Subscribed"/>, and <see cref="ReactiveTest.Disposed"/> times.
    /// </summary>
    public ITestableAsyncObserver<T> Start<T>(Func<IAsyncObservable<T>> create)
    {
        if (create == null)
        {
            throw new ArgumentNullException(nameof(create));
        }

        return Start(create, AsyncReactiveTest.Created, AsyncReactiveTest.Subscribed, AsyncReactiveTest.Disposed);
    }

    /// <summary>Runs a full scenario with the supplied observer and the default times.</summary>
    public ITestableAsyncObserver<T> Start<T>(Func<IAsyncObservable<T>> create, ITestableAsyncObserver<T> observer) =>
        Start(create, observer, AsyncReactiveTest.Created, AsyncReactiveTest.Subscribed, AsyncReactiveTest.Disposed);

    /// <summary>Runs a full scenario with the supplied observer, the default creation and subscription times, and the given disposal time.</summary>
    public ITestableAsyncObserver<T> Start<T>(Func<IAsyncObservable<T>> create, ITestableAsyncObserver<T> observer, long disposed) =>
        Start(create, observer, AsyncReactiveTest.Created, AsyncReactiveTest.Subscribed, disposed);
}
