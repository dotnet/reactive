// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Microsoft.Reactive.Testing.Async;

public sealed partial class TestAsyncScheduler
{
    /// <summary>
    /// Runs a full virtual-time test.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This invokes tests using the same schedule phases as <see cref="TestScheduler"/>. Virtual
    /// time will initially be advanced to the <paramref name="created"/> tick. Then the
    /// <paramref name="create"/> factory will be invoked. Then time will run up to
    /// <paramref name="subscribed"/> ticks, at which point this will subscribe to the observable
    /// returned by <paramref name="create"/>. Then virtual time will run up to the
    /// <paramref name="disposed"/> tick at which point the subscription will be disposed. At this
    /// point, all outstanding scheduled work will be run to completion, and then finally this
    /// method will return.
    /// </para>
    /// If the dispose tick arrives and the <c>SubscribeAsync</c> call has not yet completed, the
    /// test reports this as a failure. (This should not happen in a properly constructed test. But
    /// with subscription being an asynchronous operation, it is technically possible for this to
    /// happen, so we detect it.)
    /// </remarks>
    /// <returns>
    /// An <see cref="ITestableAsyncObserver{T}"/> that can be used to verify that the expected
    /// behaviour occurred.
    /// </returns>
    public ITestableAsyncObserver<T> Start<T>(Func<IAsyncObservable<T>> create, long created, long subscribed, long disposed)
    {
        if (create == null)
        {
            throw new ArgumentNullException(nameof(create));
        }

        return Start(create, CreateObserver<T>(), created, subscribed, disposed);
    }

    /// <summary>
    /// Runs a full virtual-time test, using ths supplied observer instead of creating one..
    /// </summary>
    /// <remarks>
    /// This is essentially the same as <see cref="Start{T}(Func{IAsyncObservable{T}}, long, long, long)"/>,
    /// except callers supply their own observer.
    /// </remarks>
    /// <returns>
    /// An <see cref="ITestableAsyncObserver{T}"/> that can be used to verify that the expected
    /// behaviour occurred.
    /// </returns>
    public ITestableAsyncObserver<T> Start<T>(
        Func<IAsyncObservable<T>> create,
        ITestableAsyncObserver<T> observer,
        long created,
        long subscribed,
        long disposed)
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
    /// Runs a full virtual-time test with the default creation and subscription virtual times.
    /// </summary>
    /// <remarks>
    /// This is equivalent to <see cref="Start{T}(Func{IAsyncObservable{T}}, long, long, long)"/>,
    /// but using the default <see cref="ReactiveTest.Created"/> and <see cref="ReactiveTest.Subscribed"/>
    /// times.
    /// </remarks>
    public ITestableAsyncObserver<T> Start<T>(Func<IAsyncObservable<T>> create, long disposed)
    {
        if (create == null)
        {
            throw new ArgumentNullException(nameof(create));
        }

        return Start(create, AsyncReactiveTest.Created, AsyncReactiveTest.Subscribed, disposed);
    }

    /// <summary>
    /// Runs a full virtual-time test with the default creation, subscription, and disposal virtual
    /// times.
    /// </summary>
    /// <remarks>
    /// This is equivalent to <see cref="Start{T}(Func{IAsyncObservable{T}}, long, long, long)"/>,
    /// but using the default <see cref="ReactiveTest.Created"/>, <see cref="ReactiveTest.Subscribed"/>,
    /// and <see cref="ReactiveTest.Disposed"/> times.
    /// </remarks>
    public ITestableAsyncObserver<T> Start<T>(Func<IAsyncObservable<T>> create)
    {
        if (create == null)
        {
            throw new ArgumentNullException(nameof(create));
        }

        return Start(create, AsyncReactiveTest.Created, AsyncReactiveTest.Subscribed, AsyncReactiveTest.Disposed);
    }

    /// <summary>
    /// Runs a full virtual-time test with the default creation, subscription, and disposal virtual
    /// times, using the supplied observer.
    /// </summary>
    /// <remarks>
    /// This is equivalent to <see cref="Start{T}(Func{IAsyncObservable{T}})"/>,
    /// except callers supply their own observer.
    /// </remarks>
    public ITestableAsyncObserver<T> Start<T>(Func<IAsyncObservable<T>> create, ITestableAsyncObserver<T> observer) =>
        Start(create, observer, AsyncReactiveTest.Created, AsyncReactiveTest.Subscribed, AsyncReactiveTest.Disposed);


    /// <summary>
    /// Runs a full virtual-time test with the default creation, and subscription virtual times,
    /// using the supplied observer.
    /// </summary>
    /// <remarks>
    /// This is equivalent to <see cref="Start{T}(Func{IAsyncObservable{T}}, long)"/>, except
    /// callers supply their own observer.
    /// </remarks>
    public ITestableAsyncObserver<T> Start<T>(Func<IAsyncObservable<T>> create, ITestableAsyncObserver<T> observer, long disposed) =>
        Start(create, observer, AsyncReactiveTest.Created, AsyncReactiveTest.Subscribed, disposed);
}
