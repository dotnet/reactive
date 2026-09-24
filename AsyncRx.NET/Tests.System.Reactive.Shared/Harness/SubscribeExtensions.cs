// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The raw surface's subscribe, as extension methods so it reads as in the sync suite.</summary>
public static class SubscribeExtensions
{
    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(this Seq<T> source, TestableObserver<T> observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        return observer.Platform.SubscribeAsync(source, observer);
    }

    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(this Seq<T> source, TestScheduler scheduler, Func<T, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(scheduler);

        return scheduler.Platform.SubscribeAsync(scheduler, source, onNext);
    }

    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(this Seq<T> source, TestScheduler scheduler, Action<T> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        return source.SubscribeAsync(scheduler, x =>
        {
            onNext(x);
            return default;
        });
    }

    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(this Nested<T> source, TestScheduler scheduler, Func<Seq<T>, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(scheduler);

        return scheduler.Platform.SubscribeAsync(scheduler, source, onNext);
    }

    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(this Nested<T> source, TestScheduler scheduler, Action<Seq<T>> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        return source.SubscribeAsync(scheduler, w =>
        {
            onNext(w);
            return default;
        });
    }
}
